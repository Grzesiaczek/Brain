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
        List<Tuple<int, int>> tracking;

        AnimatedState active;
        CreationFrame frame;

        CreationSequence sequence;
        BuiltSequence built;

        Tuple<int, int> tuple;

        int count;
        int time;
        int length;

        public event EventHandler animationStop;
        public event EventHandler creationFinished;
        public event EventHandler frameChanged;
        public event EventHandler framesChanged;

        #endregion

        public Creation(Display display) : base(display)
        {
            mapNeurons = new Dictionary<Neuron, CreatedNeuron>();
            mapSynapses = new Dictionary<Synapse, CreatedSynapse>();
            mapHistory = new Dictionary<AnimatedState, CreationHistory>();

            neurons = new List<CreatedNeuron>();
            synapses = new List<CreatedSynapse>();

            CreationFrame.setDictionary(mapNeurons, mapSynapses);
            duplex = new List<Synapse>();

            tracking = new List<Tuple<int, int>>();
            tuple = new Tuple<int, int>(-1, -1);
            tracking.Add(tuple);

            display.add(this);
            count = 0;
        }

        #region logika

        public void load(List<CreationSequence> data)
        {
            sequences = data;

            for (int i = 0; i < sequences.Count; i++)
            {
                int size = sequences[i].Frames.Count;
                length += size;

                for (int j = 0; j < size; j++)
                    tracking.Add(new Tuple<int, int>(i, j));
            }
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

            framesChanged(length, null);

            foreach(CreationSequence cs in sequences)
                foreach (CreationFrame cf in cs.Frames)
                    cf.finish += new EventHandler(finish);
        }

        protected override void tick(object sender, EventArgs e)
        {
            if (frames != 0)
                display.executeShift();

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
            if (count == length)
            {
                creationFinished(true, null);
                return;
            }

            setFrame(count + 1);
            frameChanged(count, null);
        }

        void setFrame(int index)
        {
            frameChanged(index, null);

            if(count > index)
            {
                for (int i = count; i > index; i--)
                    sequences[tracking[i].Item1].Frames[tracking[i].Item2].undo(neurons, synapses);
            }
            else
            {
                for (int i = count + 1; i <= index; i++)
                    sequences[tracking[i].Item1].Frames[tracking[i].Item2].create(neurons, synapses);
            }

            count = index;

            if (index == 0)
            {
                sequence.hide();
                display.clear();

                tuple = tracking[0];
                frame.Neuron.changeType(SequenceElementType.Normal);
                return;
            }

            Tuple<int, int> tup = tracking[count];

            if(tup.Item1 != tuple.Item1)
            {
                if(tuple.Item1 != -1)
                    sequence.hide();

                sequence = sequences[tup.Item1];
                display.show(sequence);
            }

            if (frame != null)
                frame.Neuron.changeType(SequenceElementType.Normal);

            frame = sequence.get(tup.Item2);
            frame.Neuron.changeType(SequenceElementType.Active);
            frame.step();
            tuple = tup;
        }

        public override void resize()
        {
            base.resize();

            if (tuple.Item1 != -1)
                display.balance();
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

        #endregion

        #region funkcje sterujące

        public override void start()
        {
            if (animation)
                return;

            animation = true;
            setFrame(count);
        }

        public override void stop()
        {
            if (!animation)
                return;

            animation = false;
            animationStop(this, new EventArgs());
        }

        public override void back()
        {
            if (count == 0)
            {
                creationFinished(false, null);
                return;
            }

            setFrame(count - 1);
            frameChanged(count, null);
        }

        public override void forth()
        {
            if(frame != null)
                frame.create();

            next();
        }

        public override void changeFrame(int frame)
        {
            if (count >= tracking.Count)
                return;

            setFrame(frame);
        }

        public override void changePace(int pace)
        {
            if (frame != null)
                frame.setInterval(pace / 25);
            else
                CreationFrame.Interval = pace / 25;
        }

        public override void add(int key)
        {
            if (built == null)
                built = new BuiltSequence();
        }

        public override void erase()
        {
            built.erase();
        }

        public override void space()
        {
            base.space();
        }

        public override void confirm()
        {
            base.confirm();
        }

        #endregion

        #region rysowanie

        void animate()
        {
            buffer.Graphics.Clear(SystemColors.Control);

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
            buffer.Graphics.Clear(SystemColors.Control);

            foreach (CreatedSynapse synapse in synapses)
                synapse.draw();

            foreach (CreatedSynapse synapse in synapses)
                synapse.drawState();

            foreach (CreatedNeuron neuron in neurons)
                neuron.draw();

            buffer.Render(graphics);
        }

        #endregion

        #region interfejs drawable

        public override void show()
        {
            if (sequence != null)
                sequence.show();
            else
                display.clear();

            framesChanged(length, null);
            frameChanged(count, null);

            base.show();

            foreach (CreatedNeuron neuron in neurons)
                neuron.load();
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