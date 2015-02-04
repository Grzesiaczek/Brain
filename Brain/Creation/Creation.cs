using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Brain
{
    class Creation : Presentation
    {
        #region deklaracje

        Dictionary<Neuron, CreatedNeuron> mapNeurons;
        Dictionary<Synapse, CreatedSynapse> mapSynapses;
        Dictionary<AnimatedState, CreationHistory> mapHistory;

        List<CreationSequence> sequences;
        List<AnimatedNeuron> animated;
        List<Synapse> duplex;

        List<CreatedNeuron> neurons;
        List<CreatedSynapse> synapses;

        AnimatedState active;
        CreationFrame frame;
        CreationSequence sequence;

        int count;
        int index;
        int time;

        public event EventHandler animationStop;
        public event EventHandler creationFinished;
        public event EventHandler frameChanged;
        #endregion

        public Creation(List<CreationSequence> data)
        {
            mapNeurons = new Dictionary<Neuron, CreatedNeuron>();
            mapSynapses = new Dictionary<Synapse, CreatedSynapse>();
            mapHistory = new Dictionary<AnimatedState, CreationHistory>();

            neurons = new List<CreatedNeuron>();
            synapses = new List<CreatedSynapse>();

            CreationFrame.setDictionary(mapNeurons, mapSynapses);
            duplex = new List<Synapse>();

            sequences = data;
            count = 0;
        }

        public void load(List<AnimatedNeuron> neurons, List<AnimatedSynapse> synapses)
        {
            animated = neurons;

            foreach (AnimatedNeuron an in neurons)
                mapNeurons.Add(an.Neuron, new CreatedNeuron(an));

            foreach (AnimatedSynapse s in synapses)
            {
                if (s.Pre is AnimatedReceptor)
                    continue;

                CreatedSynapse cs = new CreatedSynapse(s);
                mapSynapses.Add(s.Synapse, cs);

                if (s.isDuplex())
                    mapSynapses.Add(s.Duplex, cs);
            }

            sequence = sequences[0];
            sequence.show(Parent);

            foreach(CreationSequence cs in sequences)
                foreach (CreationFrame cf in cs.Frames)
                    cf.finish += new EventHandler(finish);
        }

        protected override void tick(object sender, EventArgs e)
        {
            if (active != null && ++time == 12)
            {
                if (mapHistory.ContainsKey(active))
                    mapHistory[active].show();
                else
                {
                    CreationHistory history = new CreationHistory(active);
                    mapHistory.Add(active, history);
                    Controls.Add(history);
                    history.show();
                }
            }

            if (animation)
                animate();
            else
                redraw();
        }

        void next()
        {
            if ((frame = sequence.next()) == null)
            {
                sequence.hide();

                if (++index == sequences.Count)
                {
                    creationFinished(this, null);
                    return;
                }

                sequence = sequences[index];
                sequence.show(Parent);
                frame = sequence.next();
            }

            frameChanged(++count, null);
            frame.step();
        }

        void finish(object sender, EventArgs e)
        {
            if (sender is CreatedNeuron)
                neurons.Add((CreatedNeuron)sender);
            else if (sender is List<Synapse>)
                foreach (Synapse s in (List<Synapse>)sender)
                    synapses.Add(mapSynapses[s]);
            else
                next();
        }

        #region rysowanie

        void animate()
        {
            Graphics g = buffer.Graphics;
            g.Clear(SystemColors.Control);

            foreach (CreatedSynapse synapse in synapses)
                synapse.draw();

            if (frame != null)
                frame.tick();

            foreach (CreatedSynapse synapse in synapses)
                synapse.drawState();

            foreach (CreatedNeuron neuron in neurons)
                neuron.draw();

            buffer.Render(graphics);
        }

        void redraw()
        {
            Graphics g = buffer.Graphics;
            g.Clear(SystemColors.Control);

            foreach (CreatedSynapse synapse in synapses)
                synapse.draw();

            foreach (CreatedSynapse synapse in synapses)
                synapse.drawState();

            foreach (CreatedNeuron neuron in neurons)
                neuron.draw();

            buffer.Render(graphics);
        }
        #endregion

        #region funkcje sterujące

        public override void start()
        {
            if (animation)
                return;

            animation = true;
            frame = sequence.next();
        }

        public override void stop()
        {
            if (!animation)
                return;

            sequence.pause();
            animation = false;
            animationStop(this, new EventArgs());
        }

        public override void back()
        {

        }

        public override void forth()
        {
            if(frame != null)
                frame.create();

            next();

            if(frame != null)
            {
                frame.create(neurons, synapses);
                frame.change();
            }
            
        }

        public override void changeFrame(int frame)
        {

        }

        public override void changePace(int pace)
        {
            if (frame != null)
                frame.setInterval(pace / 25);
            else
                CreationFrame.Interval = pace / 25;
        }

        #endregion

        #region interfejs drawable
        public override void show()
        {
            sequence.show();
            frameChanged(count, null);
            base.show();

            AnimatedElement.Graphics = buffer.Graphics;
        }

        public override void hide()
        {
            sequence.hide();
            base.hide();
        }

        public override void save()
        {
            Bitmap bitmap = new Bitmap(Width, Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            buffer.Render(graphics);
            bitmap.Save(Path.Combine(Constant.Path, "test.png"));
        }
        #endregion

        #region obsługa zdarzeń myszy
        protected override void mouseMove(object sender, MouseEventArgs e)
        {
            if(shift != null)
            {
                base.mouseMove(sender, e);
                return;
            }

            if (active != null)
            {
                if (active.active(e.Location))
                    return;

                if(mapHistory.ContainsKey(active))
                    mapHistory[active].hide();

                active = null;
                time = 0;
                return;
            }

            foreach(CreatedSynapse synapse in synapses)
            {
                AnimatedSynapse s = synapse.Synapse;

                if(s.active(e.Location, false))
                {
                    active = s.getState(false);
                    return;
                }

                if(s.isDuplex() && s.active(e.Location, true))
                {
                    active = s.getState(true);
                    return;
                }
            }
        }

        protected override void mouseDown(object sender, MouseEventArgs e)
        {
            foreach (AnimatedNeuron neuron in animated)
            {
                if (neuron.click(e.Location))
                {
                    shift = new ShiftedNeuron(neuron, new PointF(e.X, e.Y), animated);
                    break;
                }
            }

            redraw();
        }
        #endregion
    }
}