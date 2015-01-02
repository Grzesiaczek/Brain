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
    class Creation : MainLayer, Controller
    {
        Dictionary<Neuron, CreatedNeuron> mapNeurons;
        Dictionary<Synapse, CreatedSynapse> mapSynapses;
        Dictionary<SynapseState, CreationHistory> mapHistory;

        List<CreationFrame> frames;
        List<AnimatedNeuron> animated;
        List<Synapse> duplex;

        List<CreatedNeuron> neurons;
        List<CreatedSynapse> synapses;

        SynapseState active;
        Sentence sentence;
        CreationFrame frame;

        int time;
        int count;

        bool animation;

        public Creation(List<CreationFrame> data)
        {
            mapNeurons = new Dictionary<Neuron, CreatedNeuron>();
            mapSynapses = new Dictionary<Synapse, CreatedSynapse>();
            mapHistory = new Dictionary<SynapseState, CreationHistory>();

            neurons = new List<CreatedNeuron>();
            synapses = new List<CreatedSynapse>();

            CreationFrame.setDictionary(mapNeurons, mapSynapses);

            duplex = new List<Synapse>();
            frames = data;
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

            frame = frames[count];
            frame.step();

            foreach (CreationFrame cf in frames)
                cf.finish += new EventHandler(finish);
        }

        public override void resize()
        {
            base.resize();

            CreatedNeuron.Graphics = buffer.Graphics;
            CreatedSynapse.Graphics = buffer.Graphics;
        }

        protected override void changeSize()
        {
            Height = Parent.Height - 158;
            Width = Parent.Width - 168;
            initializeGraphics();
        }

        protected override void tick(object sender, EventArgs e)
        {
            if (active != null && ++time == 12)
            {
                if (mapHistory.ContainsKey(active))
                    mapHistory[active].show();
                else
                {
                    CreationHistory history = new CreationHistory(this, active);

                    int x = (int)(active.State.X);
                    int y = (int)(active.State.Y);

                    history.Location = new Point(x, y);
                    mapHistory.Add(active, history);
                }
            }

            if (animation)
                animate();
            else
                redraw();
        }

        void animate()
        {
            Graphics g = buffer.Graphics;
            g.Clear(SystemColors.Control);

            foreach (CreatedSynapse synapse in synapses)
                synapse.draw();

            foreach (CreatedSynapse synapse in synapses)
                synapse.drawState();

            foreach (CreatedNeuron neuron in neurons)
                neuron.draw();

            if (frame != null)
                frame.tick();

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

        public void changePace(int pace)
        {
            CreationFrame.setInterval(pace / 25);
        }

        public void start()
        {
            animation = true;
        }

        public void stop()
        {
            animation = false;
            animationStop(this, new EventArgs());
        }

        public void back()
        {

        }

        public void forth()
        {
            frame.create(neurons, synapses);
            next();
        }

        public bool started()
        {
            return animation;
        }

        void next()
        {
            if (++count == frames.Count)
            {
                creationFinished(this, null);
                return;
            }

            frameChanged(this, new FrameEventArgs(count));
            frame = frames[count];
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

        public event EventHandler animationStop;
        public event EventHandler creationFinished;
        public event EventHandler<FrameEventArgs> frameChanged;
    }
}