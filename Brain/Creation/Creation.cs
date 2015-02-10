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

        Tuple<int, int> tuple;

        int count;
        int time;
        int length;

        bool built;

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
            frameChanged(count, null);

            int index = 1;

            foreach(CreationSequence cs in sequences)
                foreach (CreationFrame cf in cs.Frames)
                {
                    cf.finish += new EventHandler(finish);
                    CreatedNeuron neuron = mapNeurons[cf.Neuron.Neuron];

                    if (neuron.Frame == 0)
                        neuron.Frame = index;

                    index++;
                }
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
                nextFrame();
        }

        #endregion

        #region sterowanie

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

            nextFrame();
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

        public override void add(char key)
        {
            if (count == 0)
            {
                addSequence();
                frameAdd();
            }

            sequence.add(key);
        }

        //todo
        public override void erase()
        {
            sequence.erase();
        }

        public override void space()
        {
            addFrame();
            frameUp();
        }

        public override void enter()
        {
            if (sequence.Frames.Count == 0)
                return;

            if(built)
            {
                addFrame(true);
                built = false;
                return;
            }

            addSequence();
        }

        //todo
        public override void delete()
        {
            frameDown();
        }

        #endregion

        #region zmiany klatek

        void addFrame(bool enter = false)
        {
            if (sequence == null)
                return;

            Dictionary<object, object> map;
            changeFrame(sequence.add(brain, count));

            if (frame != null)
            {
                map = display.loadFrame(frame, count);
                balanceSize();

                if(enter)
                    frame.Neuron.changeType(SequenceElementType.Active);
            }
            else
                return;

            tuple = new Tuple<int, int>(tuple.Item1, tuple.Item2 + 1);
            tracking.Insert(count, tuple);

            addMap(map);
            frame.change();
        }

        void addMap(Dictionary<object, object> map)
        {
            foreach (object key in map.Keys)
                if (map[key] is bool)
                {
                    CreatedNeuron neuron = mapNeurons[(Neuron)key];
                    neurons.Add(neuron);
                    neuron.create();

                    if(count < neuron.Frame)
                        neuron.Frame = count;
                }
                else if (key is Neuron)
                {
                    CreatedNeuron neuron = (CreatedNeuron)(map[key]);
                    mapNeurons.Add((Neuron)key, neuron);
                    neurons.Add(neuron);

                    neuron.create();
                    neuron.Frame = count;
                }
                else
                {
                    CreatedSynapse synapse = (CreatedSynapse)(map[key]);
                    mapSynapses.Add((Synapse)key, synapse);
                    synapses.Add(synapse);
                }
        }

        void addSequence()
        {
            int index = tuple.Item1 + 1;

            sequence = new CreationSequence();
            sequences.Insert(index, sequence);

            while (++count < length && tracking[count].Item1 != index) ;

            for (int i = count; i < tracking.Count; i++)
            {
                int it1 = tracking[i].Item1 + 1;
                int it2 = tracking[i].Item2;
                tracking[i] = new Tuple<int, int>(it1, it2);
            }

            if (count == length || tuple.Item1 == -1)
                tuple = new Tuple<int, int>(0, -1);

            display.show(sequence);
            built = true;
        }

        void changeFrame(CreationFrame cf, bool change = false)
        {
            if (frame != null)
            {
                frame.create();
                frame.Neuron.changeType(SequenceElementType.Normal);
            }

            frame = cf;
            frame.Neuron.changeType(SequenceElementType.Active);

            if (change)
                frame.change();

            if (animation)
                frame.step();
        }

        void setFrame(int index)
        {
            frameChanged(index, null);

            if (count > index)
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
                display.clear();
                tuple = tracking[0];
                frame.Neuron.changeType(SequenceElementType.Normal);
                return;
            }

            Tuple<int, int> tup = tracking[count];

            if (tup.Item1 != tuple.Item1)
            {
                sequence = sequences[tup.Item1];
                display.show(sequence);
            }

            changeFrame(sequence.get(tup.Item2), true);
        }

        void nextFrame()
        {
            if (count == length)
            {
                creationFinished(true, null);
                return;
            }

            setFrame(count + 1);
            frameChanged(count, null);
        }

        void frameAdd()
        {
            framesChanged(++length, null);
            frameChanged(count, null);
        }

        void frameUp()
        {
            framesChanged(++length, null);
            frameChanged(++count, null);
        }

        void frameDown()
        {
            framesChanged(--length, null);
            frameChanged(--count, null);
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