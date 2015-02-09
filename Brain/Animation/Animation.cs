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

        Mode mode;
        QuerySequence query;

        GraphBalancing balancing;
        StateBar stateBar;

        List<AnimatedNeuron> neurons;
        List<AnimatedSynapse> synapses;
        List<AnimatedReceptor> receptors;

        bool activation = false;
        bool loaded = false;
        bool pause = false;

        int frame = 0;
        int interval = 1;
        int arrival = 0;

        int count = 0;
        int length = 250;

        public event EventHandler animationStop;
        public event EventHandler balanceFinished;
        public event EventHandler queryAccepted;
        public event EventHandler frameChanged;
        public event EventHandler framesChanged;

        #endregion

        public Animation(Display display) : base(display)
        {
            balancing = GraphBalancing.getInstance();
            balancing.balanceEnded += balanceEnded;
            balancing.balanceState += balanceState;

            size = new Size(Width, Height);
            query = new QuerySequence();

            display.add(this);
            display.show(query);

            MouseClick += new MouseEventHandler(mouseClick);
            MouseDoubleClick += new MouseEventHandler(mouseClick);

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

        public void loadBrain(Brain brain)
        {
            Random random = new Random();
            Presentation.brain = brain;

            balanceSize();

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
            creation.load(neurons, synapses);

            Thread thread = new Thread(fastBalancing);
            thread.Start();
        }

        void fastBalancing()
        {
            stateBar.Phase = StateBarPhase.BalanceNormal;
            balancing.balance(neurons, synapses, receptors);
        }

        public void setBar(StateBar stateBar)
        {
            this.stateBar = stateBar;
        }

        #endregion

        #region obsługa grafiki

        void animate()
        {
            if (!loaded)
                return;

            if (pause)
            {
                stateBar.State -= 3;

                if (stateBar.State <= 0)
                {
                    stateBar.reset();
                    pause = false;
                }
            }
            else if (count++ == interval)
            {
                if (activation)
                {
                    stateBar.Phase = StateBarPhase.Activation;
                    activation = false;
                    pause = true;
                    count--;
                    return;
                }
                else
                {
                    frame++;
                    changeFrame();
                    count = 1;

                    foreach (AnimatedNeuron neuron in neurons)
                    {
                        if (neuron.Neuron.Activity[frame].Active)
                        {
                            activation = true;
                            break;
                        }
                    }
                }
            }

            buffer.Graphics.Clear(SystemColors.Control);

            int number = count - interval / 2;

            if (number >= 0 && number <= interval / 4)
            {
                foreach (AnimatedSynapse synapse in synapses)
                    synapse.animate(frame, ((float)number * 4) / interval);
            }
            else foreach (AnimatedSynapse synapse in synapses)
                    synapse.draw();

            foreach (AnimatedSynapse synapse in synapses)
                synapse.drawState(frame);

            foreach (AnimatedNeuron neuron in neurons)
                neuron.animate(frame, count, (double)count / interval);

            foreach (AnimatedReceptor receptor in receptors)
                receptor.draw(frame);

            buffer.Render(graphics);
        }

        void draw()
        {
            buffer.Graphics.Clear(SystemColors.Control);

            foreach (AnimatedSynapse synapse in synapses)
                synapse.draw();

            foreach (AnimatedSynapse synapse in synapses)
                synapse.drawState();

            foreach (AnimatedNeuron neuron in neurons)
                neuron.draw(0.0);

            foreach (AnimatedReceptor receptor in receptors)
                receptor.draw();

            buffer.Render(graphics);
        }

        void redraw()
        {
            buffer.Graphics.Clear(SystemColors.Control);

            foreach (AnimatedSynapse synapse in synapses)
                synapse.draw();

            foreach (AnimatedSynapse synapse in synapses)
                synapse.drawState(frame);

            foreach (AnimatedNeuron neuron in neurons)
                neuron.draw(frame);

            foreach (AnimatedReceptor receptor in receptors)
                receptor.draw(frame);

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
            stateBar.Phase = StateBarPhase.BalanceNormal;

            if(Visible)
                balancing.animate(neurons, synapses, receptors, 80);
            else
                balancing.animate(neurons, synapses, receptors, 120);
        }

        public void stopBalance()
        {
            balancing.stop();
        }

        private void balanceState(object sender, EventArgs e)
        {
            double result = Math.Log10((float)sender);
            int value = (int)Math.Min(stateBar.Height, result * 40);
            stateBar.State = value;
        }

        private void balanceEnded(object sender, EventArgs e)
        {
            if ((bool)sender)
            {
                stateBar.reset();
                balanceFinished(this, new EventArgs());
            }
            else
            {
                stateBar.reset();
                stateBar.Phase = StateBarPhase.BalanceExtra;
            }
        }

        #endregion

        #region zmiana rozmiaru i skali

        public void scroll(int x, int y)
        {
            area.X = 20 * x;
            area.Y = 20 * y;

            AnimatedElement.Area = area;
            
            foreach (AnimatedNeuron neuron in neurons)
                neuron.changePosition();

            foreach (AnimatedReceptor receptor in receptors)
                receptor.changePosition();

            foreach (AnimatedSynapse synapse in synapses)
                synapse.changePosition();
        }

        public void rescale(float value)
        {
            float radius;
            float rec;
            float syn;

            if(value < 50)
            {
                radius = 12;
                rec = 8;
                syn = 6;
            }
            else
            {
                radius = value * 0.24f;
                rec = value * 0.16f;
                syn = value * 0.12f;
            }

            Constant.Radius = radius;

            foreach (AnimatedNeuron neuron in neurons)
            {
                neuron.changePosition();
                neuron.Radius = radius;
            }

            foreach (AnimatedReceptor receptor in receptors)
            {
                receptor.changePosition();
                receptor.Radius = rec;
            }

            foreach (AnimatedSynapse synapse in synapses)
            {
                synapse.changePosition();
                synapse.Radius = syn;
            }
        }

        public override void resize()
        {
            base.resize();
            query.resize();
            balance();
        }

        public void calculateShift(float factor)
        {
            if (factor > 0.1)
            {
                frames = (int)(factor / 0.01);

                foreach (AnimatedNeuron neuron in neurons)
                    neuron.calculateShift(frames);

                foreach (AnimatedReceptor receptor in receptors)
                    receptor.calculateShift(frames);
            }

            balance();
        }

        public void changeSize(float factor)
        {
            foreach (AnimatedNeuron neuron in neurons)
                neuron.changePosition(factor);

            foreach (AnimatedReceptor receptor in receptors)
                receptor.changePosition(factor);

            foreach (AnimatedSynapse synapse in synapses)
                synapse.changePosition();
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

                Receptor receptor = brain.Receptors.Find(k => k.Name == word);
                Synapse synapse = brain.Synapses.Find(k => k.Pre == receptor);

                receptor.initialize(interval, interval - index - 1, intensivity);
                new SequenceReceptor(query, receptor);

                AnimatedReceptor ar = new AnimatedReceptor(receptor, an, index++ % 4);
                synapses.Add(new AnimatedSynapse(ar, an, synapse));
                receptors.Add(ar);
            }

            query.arrange();
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

            framesChanged(length, null);
            frameChanged(frame, null);

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

        public void executeShift()
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
                executeShift();

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
