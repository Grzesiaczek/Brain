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
    class Animation : Presentation
    {
        #region deklaracje

        Brain brain;
        Mode mode;

        //wynik nie jest sekwencją, lecz grafem!
        QuerySequence query;

        GraphBalancing balancing;
        StateBar stateBar;

        List<AnimatedNeuron> neurons;
        List<AnimatedSynapse> synapses;
        List<AnimatedReceptor> receptors;

        bool loaded = false;
        bool pause = false;

        int frame = 0;
        int interval = 1;
        int arrival = 0;

        int count = 0;
        int length = 250;

        int frames = 0;

        public event EventHandler animationStop;
        public event EventHandler balanceStarted;
        public event EventHandler balanceFinished;
        public event EventHandler queryAccepted;

        public event EventHandler factorChanged;
        public event EventHandler frameChanged;
        public event EventHandler sizeChanged;

        #endregion

        public Animation()
        {
            balancing = GraphBalancing.getInstance();
            balancing.balanceFinished += balanceEnded;
            size = new Size(Width, Height);

            Padding = new Padding(10, 50, 10, 10);
            query = new QuerySequence();
            area = new Rectangle(10, 50, 0, 0);
            
            Controls.Add(query);
            query.setMargin(new Padding(0));

            MouseClick += new MouseEventHandler(mouseClick);
            MouseDoubleClick += new MouseEventHandler(mouseClick);
            AnimatedNeuron.activation += new EventHandler(neuronActivated);

            neurons = new List<AnimatedNeuron>();
            synapses = new List<AnimatedSynapse>();
            receptors = new List<AnimatedReceptor>();
        }

        #region funkcje inicjujące

        PointF randomPoint(Random random)
        {
            float x = 40 + random.Next() % (size.Width - 80);
            float y = 40 + random.Next() % (size.Height - 80);
            return new PointF(x, y);
        }

        void balanceSize()
        {
            int optimum = (int)Math.Pow(brain.Neurons.Count + 1, 1.5) * 20;
            int nominal = (size.Width + size.Height) / 2;

            if (optimum > nominal * 1.5)
            {
                float min = Height;

                if (Width < min)
                    min = Width;

                size.Width = optimum;
                size.Height = optimum;

                float factor = min / optimum;
                AnimatedElement.Factor = factor;

                Constant.Radius = 24 * factor;
                factorChanged(Constant.Radius, null);

                sizeChanged(this, null);
            }
            else
                AnimatedElement.Factor = 1.0f;

            resize();
        }

        public void loadBrain(Brain brain)
        {
            Random random = new Random();
            this.brain = brain;

            balanceSize();

            while (buffer == null)
                Thread.Sleep(10);

            AnimatedElement.Graphics = buffer.Graphics;
            AnimatedElement.Area = area;
            AnimatedElement.Size = size;
            BalancedElement.Size = size;

            foreach (Neuron n in brain.Neurons)
            {
                PointF position = randomPoint(random);
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

            String[] data = { brain.Neurons[0].Word, brain.Neurons[1].Word };
            addQuery(data, 4, 0.6f);
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
            //balancing.balance(neurons, synapses, receptors);
            creation.load(neurons, synapses);
        }

        #endregion

        #region obsługa paska stanu

        public void setBar(StateBar stateBar)
        {
            this.stateBar = stateBar;
            stateBar.finished += new EventHandler(pauseFinished);
        }

        void neuronActivated(object sender, EventArgs e)
        {
            stateBar.run();
            pause = true;
        }

        void pauseFinished(object sender, EventArgs e)
        {
            stateBar.next();
            pause = false;
        }

        #endregion

        #region obsługa grafiki

        void clear()
        {
            buffer.Graphics.Clip = new Region();
            buffer.Graphics.Clear(SystemColors.Control);
            buffer.Graphics.Clip = new Region(area);
        }

        void animate()
        {
            if (!loaded)
                return;

            if (!pause && count++ == interval)
            {
                frame++;
                changeFrame();
                count = 1;
            }

            clear();

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
            clear();

            foreach (AnimatedSynapse s in synapses)
                s.draw();

            foreach (AnimatedSynapse s in synapses)
                s.drawState();

            foreach (AnimatedNeuron an in neurons)
                an.draw(0.0);

            foreach (AnimatedReceptor ar in receptors)
                ar.draw();

            buffer.Render(graphics);
        }

        void redraw()
        {
            clear();

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

        #endregion

        #region funkcje sterujące

        public override void start()
        {
            if (animation)
                return;

            if (frame == 0)
            {
                frame = 1;
                frameChanged(frame, null);
            }

            animation = true;
            AnimatedNeuron.Animation = true;
        }

        public override void stop()
        {
            if (!animation)
                return;

            animation = false;
            animationStop(this, null);
            AnimatedNeuron.Animation = false;
        }

        public override void back()
        {
            if (frame > 1)
                frame--;

            changeFrame();
        }

        public override void forth()
        {
            if (frame < length)
                frame++;

            changeFrame();
        }

        void changeFrame()
        {
            query.tick(frame);
            frameChanged(frame, null);
        }

        public override void changeFrame(int frame)
        {
            this.frame = frame;
            changeFrame();
        }

        public override void changePace(int pace)
        {
            count = (int)(count * (float)pace / (interval * 25));
            interval = pace / 25;
            arrival = (interval * 3) / 4;
        }

        #endregion

        #region balansowanie grafu

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
        #endregion

        #region zmiana rozmiaru i skali

        public void scroll(int x, int y)
        {
            area.X = 20 * x + padding.Left;
            area.Y = 20 * y + padding.Top;

            AnimatedElement.Area = area;
            
            foreach (AnimatedNeuron neuron in neurons)
                neuron.changePosition();

            foreach (AnimatedReceptor receptor in receptors)
                receptor.changePosition();

            foreach (AnimatedSynapse synapse in synapses)
                synapse.changePosition();
        }

        public void rescale(float radius)
        {
            Constant.Radius = radius;
            float syn = radius / 2;

            for (int i = 0; i < neurons.Count; i++)
                neurons[i].Radius = radius;

            for (int i = 0; i < synapses.Count; i++)
                synapses[i].Radius = syn;
        }

        public override void resize()
        {
            base.resize();

            if(area.Height == 0)
            {
                area.Width = Width - padding.Horizontal;
                area.Height = Height - padding.Vertical;
                return;
            }

            float f = (float)Height / area.Height;
            query.resize();

            if (Height > Width)
                f = (float)Width / area.Width;

            area.Width = Width - padding.Horizontal;
            area.Height = Height - padding.Vertical;

            AnimatedElement.Graphics = buffer.Graphics;
            AnimatedElement.Factor *= f;
            AnimatedElement.Area = area;

            frameChanged(frame, null);
            sizeChanged(this, null);
            
            if (Math.Abs(1 - f) > 0.1)
            {
                frames = (int)(Math.Abs(1 - f) / 0.01);

                foreach (AnimatedNeuron neuron in neurons)
                    neuron.calculateShift(frames);

                foreach (AnimatedReceptor receptor in receptors)
                    receptor.calculateShift(frames);
            }

            balance();
        }

        #endregion

        #region różne

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

        #endregion

        #region obsługa zapytań

        public void addQuery(String[]words, int interval, float intensivity)
        {
            int index = 0;
            frame = 0;

            frameChanged(frame, null);
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
            addQuery(query.Data, query.Interval, query.Intensity);

            balancing.animate(neurons, synapses, receptors, 160);
            queryAccepted(this.query, new EventArgs());
            loaded = true;
        }
        #endregion

        #region interfejs drawable

        public override void show()
        {
            create();
            query.show();
            base.show();

            AnimatedElement.Graphics = buffer.Graphics;
        }

        public override void hide()
        {
            query.hide();
            base.hide();
        }

        public override void save()
        {
            Bitmap bitmap = new Bitmap(Width, Height, graphics);
            bitmap.Save(Constant.Path + "test.png");
        }

        void animatedResize()
        {
            frames--;

            foreach (AnimatedNeuron neuron in neurons)
                neuron.executeShift();

            foreach (AnimatedReceptor receptor in receptors)
                receptor.executeShift();

            foreach (AnimatedSynapse synapse in synapses)
                synapse.executeShift();
        }

        protected override void tick(object sender, EventArgs e)
        {
            if (frames != 0)
                animatedResize();

            if (frame == length && animation)
            {
                animationStop(this, new EventArgs());
                animation = false;
            }

            if (frame == 0)
            {
                draw();
                return;
            }

            if (animation)
                animate();
            else
                redraw();
        }
        #endregion

        #region obsługa zdarzeń myszy

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
        #endregion
    }
}
