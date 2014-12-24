using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Brain
{
    class Animation : Layer
    {
        GraphBalancing balancing;
        ShiftedNeuron shift;
        Sequence sequence;

        bool animation = false;
        bool loaded = false;
        bool sequenceBar = true;

        int frame = 1;
        int interval = 1;
        int arrival = 0;

        int count = 0;
        int length = 250;

        List<AnimatedNeuron> neurons;
        List<AnimatedSynapse> synapses;
        List<AnimatedReceptor> receptors;

        public event EventHandler animationStop;
        public event EventHandler balanceStarted;
        public event EventHandler balanceFinished;
        public event EventHandler neuronShifted;
        public event EventHandler queryAccepted;
        public event EventHandler<FrameEventArgs> frameChanged;

        public Animation(GroupBox groupBox) : base(groupBox)
        {
            balancing = GraphBalancing.getInstance();
            balancing.balanceFinished += balanceEnded;

            neurons = new List<AnimatedNeuron>();
            synapses = new List<AnimatedSynapse>();
            receptors = new List<AnimatedReceptor>();

            layer.Location = new Point(10, 110);
            layer.Visible = true;

            layer.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseClick);
            layer.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.mouseClick);
            layer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mouseDown);
            layer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseUp);
            layer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mouseMove);
        }

        public void setSequence(Sequence seq)
        {
            sequence = seq;
            seq.setData(neurons, receptors);
            seq.setInterval(interval);
            frameChanged += new EventHandler<FrameEventArgs>(seq.frameChanged);
        }

        PointF randomPoint(Random random)
        {
            float x = 40 + random.Next() % (buffer.Graphics.VisibleClipBounds.Width - 80);
            float y = 40 + random.Next() % (buffer.Graphics.VisibleClipBounds.Height - 80);
            return new PointF(x, y);
        }

        public void loadNeurons(List<Neuron> list)
        {
            StreamReader reader = new StreamReader(File.Open("data.xml", FileMode.Open));
            Random random = new Random(DateTime.Now.Millisecond);

            XmlDocument xml = new XmlDocument();
            xml.Load(reader);
            reader.Close();

            XmlNode node = xml.ChildNodes.Item(1).FirstChild;
            int counter = 0;

            while (buffer == null)
                Thread.Sleep(100);

            Graphics g = buffer.Graphics;

            foreach (XmlNode xn in node.ChildNodes)
            {
                PointF position = randomPoint(random);

                try
                {
                    if (xn.Attributes.Count != 0)
                    {
                        int x = Int32.Parse(xn.Attributes[0].Value);
                        int y = Int32.Parse(xn.Attributes[1].Value);

                        position = new PointF(x, y);
                    }
                }
                catch (Exception) { }

                Neuron n = list.Find(k => k.Word == xn.InnerText);
                AnimatedNeuron an = new AnimatedNeuron(n, g, position);
                neurons.Add(an);

                if (n.Sensin.Count > 0)
                {
                    Receptor r = n.Sensin[0];
                    AnimatedReceptor ar = new AnimatedReceptor(r, an, counter++ % 4);
                    synapses.Add(new AnimatedSynapse(ar, an, r.Output, g));
                    receptors.Add(ar);
                }
            }

            foreach (AnimatedNeuron start in neurons)
                foreach (Synapse s in start.getNeuron().Output)
                    foreach (AnimatedNeuron n in neurons)
                        if (n.getNeuron() == s.Post)
                        {
                            bool single = true;

                            foreach (AnimatedSynapse syn in synapses)
                                if (syn.Pre == n && syn.Post == start)
                                {
                                    syn.setDuplex(s);
                                    single = false;
                                    break;
                                }

                            if (single)
                                synapses.Add(new AnimatedSynapse(start, n, s, g));

                            break;
                        }
        }

        protected override void tick(object sender, EventArgs e)
        {
            if (frame == length && animation)
            {
                animationStop(this, new EventArgs());
                animation = false;
            }   

            if (animation)
                animate();
            else
                redraw();
        }

        public void start()
        {
            animation = true;
            sequence.animate(true);
        }

        public void stop()
        {
            animation = false;
            sequence.animate(false);
            animationStop(this, new EventArgs());
        }

        public bool started()
        {
            return animation;
        }

        public void redraw()
        {
            buffer.Graphics.Clear(SystemColors.Control);

            foreach (AnimatedSynapse s in synapses)
                s.draw(frame);

            foreach (AnimatedSynapse s in synapses)
                s.drawState(frame);

            foreach (AnimatedNeuron an in neurons)
                an.draw(frame);

            foreach (AnimatedReceptor ar in receptors)
                ar.draw(frame);

            buffer.Render(graphics);
        }

        void animate()
        {
            if (!loaded)
                return;

            if (count++ == interval)
            {
                frameChanged(this, new FrameEventArgs(frame));
                frame++;
                count = 1;
            }

            buffer.Graphics.Clear(SystemColors.Control);

            int number = count - interval / 2;

            if (number >= 0 && number <= interval / 4)
            {
                foreach (AnimatedSynapse s in synapses)
                    s.animate(frame, ((float)number * 4) / interval);
            }
            else foreach (AnimatedSynapse s in synapses)
                    s.draw(frame);

            foreach (AnimatedSynapse s in synapses)
                s.drawState(frame);

            foreach (AnimatedNeuron an in neurons)
                an.animate(frame, count, (double)count / interval);

            foreach (AnimatedReceptor ar in receptors)
                ar.draw(frame);

            buffer.Render(graphics);
        }

        public void changePace(int pace)
        {
            count = (int)(count * (float)pace / (interval * 25));
            interval = pace / 25;
            arrival = (interval * 3) / 4;

            if(sequence != null)
                sequence.setInterval(interval);
        }

        public void balance()
        {
            foreach (AnimatedNeuron neuron in neurons)
                neuron.State = false;

            balancing.animate(neurons, synapses, receptors, buffer.Graphics, 80);
            balanceStarted(this, new EventArgs());
        }

        public void stopBalance()
        {
            balancing.stop();
        }

        private void balanceEnded(object sender, EventArgs e)
        {
            foreach (AnimatedNeuron neuron in neurons)
                neuron.State = true;

            redraw();
            balanceFinished(this, new EventArgs());
        }

        public void back()
        {
            if (frame > 1)
                frame--;

            frameChanged(this, new FrameEventArgs(frame));
        }

        public void forth()
        {
            if (frame < length)
                frame++;

            frameChanged(this, new FrameEventArgs(frame));
        }

        public override void resize()
        {
            base.resize();

            foreach (AnimatedNeuron n in neurons)
                n.updateGraphics(buffer.Graphics);

            foreach (AnimatedSynapse s in synapses)
                s.updateGraphics(buffer.Graphics);

            foreach (AnimatedReceptor r in receptors)
                r.updateGraphics(buffer.Graphics);
            
            if (loaded)
                balance();
        }

        protected override void changeSize()
        {
            layer.Height = layer.Parent.Height - 58;
            layer.Width = layer.Parent.Width - 168;

            if (sequenceBar)
                layer.Height -= 100;

            initializeGraphics();
        }

        public void relocate(bool seq)
        {
            sequenceBar = seq;
            resize();

            if (seq)
                layer.Location = new Point(10, 110);
            else
                layer.Location = new Point(10, 10);
        }

        public void labelChanged(bool value)
        {
            foreach (AnimatedNeuron neuron in neurons)
                neuron.Label = value;

            redraw();
        }

        public void stateChanged(bool value)
        {
            foreach (AnimatedNeuron neuron in neurons)
                neuron.State = value;

            redraw();
        }

        public void load(int value)
        {
            loaded = true;
            length = value;
        }

        public void unload()
        {
            loaded = false;
        }

        public Mode newQuery(Mode mode)
        {
            Query query = new Query();
            DialogResult result = query.ShowDialog();

            if (result == DialogResult.Cancel)
                return mode;
            
            foreach (AnimatedReceptor r in receptors)
                r.setInterval(1000);

            AnimatedReceptor ar = receptors[query.Index];
            ar.setInterval(query.Interval);

            queryAccepted(this, new EventArgs());
            loaded = true;

            setMode(Mode.Query);
            return this.mode;
        }

        public List<Neuron> getNeurons()
        {
            List<Neuron> result = new List<Neuron>();

            foreach (AnimatedNeuron n in neurons)
                result.Add(n.getNeuron());

            return result;
        }

        private void mouseClick(object sender, MouseEventArgs e)
        {
            if (shift != null && mode == Mode.Manual)
                shift.activate();
        }

        private void mouseDown(object sender, MouseEventArgs e)
        {
            foreach (AnimatedNeuron neuron in neurons)
            {
                if (neuron.click(e.Location))
                {
                    shift = new ShiftedNeuron(neuron, new PointF(e.X, e.Y), neurons);
                    neurons[neurons.IndexOf(neuron)] = shift;

                    break;
                }
            }

            redraw();
        }

        private void mouseMove(object sender, MouseEventArgs e)
        {
            if (shift == null)
                return;

            shift.move(e.X, e.Y);
            redraw();
        }

        private void mouseUp(object sender, MouseEventArgs e)
        {
            if (shift == null)
                return;

            shift.save();
            shift = null;

            neuronShifted(this, new EventArgs());
            redraw();
        }
    }
}
