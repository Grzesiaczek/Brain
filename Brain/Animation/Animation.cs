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
    class Animation : MainLayer, Controller
    {
        Brain brain;
        Mode mode;

        ResultSequence result;
        QuerySequence query;

        GraphBalancing balancing;
        Size size;

        List<AnimatedNeuron> neurons;
        List<AnimatedSynapse> synapses;
        List<AnimatedReceptor> receptors;

        bool animation = false;
        bool loaded = false;

        int frame = 0;
        int interval = 1;
        int arrival = 0;

        int count = 0;
        int length = 250;

        public event EventHandler animationStop;
        public event EventHandler balanceStarted;
        public event EventHandler balanceFinished;
        public event EventHandler queryAccepted;
        public event EventHandler<FrameEventArgs> frameChanged;

        public Animation()
        {
            balancing = GraphBalancing.getInstance();
            balancing.balanceFinished += balanceEnded;
            size = new Size(Width, Height);

            result = new ResultSequence();
            query = new QuerySequence();

            MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseClick);
            MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.mouseClick);

            neurons = new List<AnimatedNeuron>();
            synapses = new List<AnimatedSynapse>();
            receptors = new List<AnimatedReceptor>();
        }

        PointF randomPoint(Random random)
        {
            float x = 40 + random.Next() % (Width - 80);
            float y = 40 + random.Next() % (Height - 80);
            return new PointF(x, y);
        }

        public void loadBrain(Brain brain)
        {
            StreamReader reader = new StreamReader(File.Open("data.xml", FileMode.Open));
            Random random = new Random(DateTime.Now.Millisecond);
            resize();

            XmlDocument xml = new XmlDocument();
            xml.Load(reader);
            reader.Close();

            XmlNode node = xml.ChildNodes.Item(1).FirstChild;
            this.brain = brain;

            if (buffer == null)
                initializeGraphics();

            AnimatedElement.Graphics = buffer.Graphics;
            AnimatedElement.Size = Size;
            BalancedElement.Size = Size;

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

                Neuron n = brain.Neurons.Find(k => k.Word == xn.InnerText);
                AnimatedNeuron an = new AnimatedNeuron(n, position);
                neurons.Add(an);
            }

            foreach (AnimatedNeuron start in neurons)
                foreach (Synapse s in start.Neuron.Output)
                    foreach (AnimatedNeuron n in neurons)
                        if (n.Neuron == s.Post)
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
                                synapses.Add(new AnimatedSynapse(start, n, s));

                            break;
                        }

            addQuery("jupiter is".Split(' '), 4, 0.6f);
            queryAccepted(query, null);
        }

        public void create()
        {
            foreach (AnimatedNeuron an in neurons)
                an.create();

            foreach (AnimatedSynapse s in synapses)
                s.create();
        }

        public void create(Creation creation)
        {
            resize();
            balancing.balance(neurons, synapses, receptors);
            creation.load(neurons, synapses);
        }

        protected override void tick(object sender, EventArgs e)
        {
            if (frame == length && animation)
            {
                animationStop(this, new EventArgs());
                animation = false;
            }

            if(frame == 0)
            {
                draw();
                return;
            }

            if (animation)
                animate();
            else
                redraw();
        }

        public void start()
        {
            if (animation)
                return;

            if (frame == 0)
            {
                frame = 1;
                frameChanged(this, new FrameEventArgs(frame));
            }

            animation = true;
                
            //sequence.animate(true);
        }

        public void stop()
        {
            if (!animation)
                return;

            animation = false;
            //sequence.animate(false);
            animationStop(this, new EventArgs());
        }

        public bool started()
        {
            return animation;
        }

        void animate()
        {
            if (!loaded)
                return;

            if (count++ == interval)
            {
                frame++;
                changeFrame();
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
                    s.draw();

            foreach (AnimatedSynapse s in synapses)
                s.drawState(frame);

            foreach (AnimatedNeuron an in neurons)
                an.animate(frame, count, (double)count / interval);

            foreach (AnimatedReceptor ar in receptors)
                ar.draw(frame);

            buffer.Render(graphics);
        }

        void draw()
        {
            buffer.Graphics.Clear(SystemColors.Control);

            foreach (AnimatedSynapse s in synapses)
                s.draw();

            foreach (AnimatedSynapse s in synapses)
                s.drawState();

            foreach (AnimatedNeuron an in neurons)
                an.draw();

            foreach (AnimatedReceptor ar in receptors)
                ar.draw();

            buffer.Render(graphics);
        }

        void redraw()
        {
            buffer.Graphics.Clear(SystemColors.Control);

            foreach (AnimatedSynapse s in synapses)
                s.draw();

            foreach (AnimatedSynapse s in synapses)
                s.drawState(frame);

            foreach (AnimatedNeuron an in neurons)
                an.draw(frame);

            foreach (AnimatedReceptor ar in receptors)
                ar.draw(frame);

            buffer.Render(graphics);
        }

        public void changePace(int pace)
        {
            count = (int)(count * (float)pace / (interval * 25));
            interval = pace / 25;
            arrival = (interval * 3) / 4;
            /*
            if(sequence != null)
                sequence.setInterval(interval);*/
        }

        public void balance()
        {
            foreach (AnimatedNeuron neuron in neurons)
                neuron.State = false;
            
            if(Visible)
                balancing.animate(neurons, synapses, receptors, 80);
            else
                balancing.animate(neurons, synapses, receptors, 120);

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

            balanceFinished(this, new EventArgs());
        }

        public void back()
        {
            if (frame > 1)
                frame--;

            changeFrame();
        }

        public void forth()
        {
            if (frame < length)
                frame++;

            changeFrame();
        }

        void changeFrame()
        {
            result.tick(frame);
            query.tick(frame);
            frameChanged(this, new FrameEventArgs(frame));
        }

        public override void resize()
        {
            base.resize();

            frameChanged(this, new FrameEventArgs(frame));
            AnimatedElement.Graphics = buffer.Graphics;

            if (size.Width != 0)
            {
                float fx = (float)Width / size.Width;
                float fy = (float)Height / size.Height;

                SizeF factor = new SizeF(fx, fy);

                foreach (AnimatedNeuron an in neurons)
                    an.update(factor);

                foreach (AnimatedReceptor ar in receptors)
                    ar.update(factor);

                foreach (AnimatedSynapse s in synapses)
                    s.update(factor);
            }

            size = new Size(Width, Height);
            AnimatedElement.Size = size;
            BalancedElement.Size = size;
            
            balance();
        }

        public void labelChanged(bool value)
        {
            foreach (AnimatedNeuron neuron in neurons)
                neuron.Label = value;
        }

        public void stateChanged(bool value)
        {
            foreach (AnimatedNeuron neuron in neurons)
                neuron.State = value;
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

        public void setMode(Mode mode)
        {
            this.mode = mode;

            if(mode == Mode.Manual && frame == 0)
            {
                frame = 1;
                changeFrame();
            }
        }

        public void addQuery(String[]words, int interval, float intensivity)
        {
            int index = 0;
            frame = 0;

            frameChanged(this, new FrameEventArgs(0));
            query.clear();

            foreach (String word in words)
            {
                AnimatedNeuron an = neurons.Find(k => k.Name == word);

                if (an == null)
                    continue;

                Receptor r = brain.Receptors.Find(k => k.Name == word);
                Synapse s = brain.Synapses.Find(k => k.Pre == r);

                r.initialize(interval, interval - index - 1, intensivity);
                new SequenceReceptor(query, r);

                AnimatedReceptor ar = new AnimatedReceptor(r, an, index++ % 4);
                synapses.Add(new AnimatedSynapse(ar, an, s));
                receptors.Add(ar);
            }
        }

        public void newQuery()
        {
            Query query = new Query();
            DialogResult result = query.ShowDialog();

            if (result == DialogResult.Cancel)
                return;

            foreach (AnimatedReceptor ar in receptors)
                synapses.Remove(ar.Output);

            receptors.Clear();
            brain.clear(false);
            addQuery(query.Data, query.Interval, query.Intensivity);

            balancing.animate(neurons, synapses, receptors, 160);
            queryAccepted(this.query, new EventArgs());
            loaded = true;
        }

        public override void show()
        {
            create();
            result.show(SequenceBar, SequenceBarType.Upper);
            query.show(SequenceBar, SequenceBarType.Lower);
            base.show();
        }

        public override void hide()
        {
            query.hide();
            result.hide();
            base.hide();
        }

        public override void save()
        {
            Bitmap bitmap = new Bitmap(Width, Height, graphics);
            bitmap.Save(Constant.Path + "test.png");
        }

        private void mouseClick(object sender, MouseEventArgs e)
        {
            if (shift != null && mode == Mode.Manual)
                shift.activate();
        }

        protected override void mouseDown(object sender, MouseEventArgs e)
        {
            foreach (AnimatedNeuron neuron in neurons)
            {
                if (neuron.click(e.Location))
                {
                    shift = new ShiftedNeuron(neuron, new PointF(e.X, e.Y), neurons);
                    break;
                }
            }
        }
    }
}
