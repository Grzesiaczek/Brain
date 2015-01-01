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
        Dictionary<Synapse, CreatedSynapse> mapSynapses;
        Dictionary<SynapseState, CreationHistory> mapHistory;

        List<CreatedNeuron> neurons;
        List<CreatedSynapse> synapses;
        List<CreationData> data;

        List<AnimatedNeuron> animated;
        List<Synapse> duplex;

        List<CreatedNeuron> createdNeurons;
        List<CreatedSynapse> createdSynapses;

        CreatedElement element;
        CreatedSynapse synapse;

        SynapseState active;
        Sentence sentence;

        int time;
        int count;
        int frame;
        int interval;

        bool animation;

        public Creation(GroupBox groupBox, List<CreationData> data) : base(groupBox)
        {
            neurons = new List<CreatedNeuron>();
            synapses = new List<CreatedSynapse>();

            createdNeurons = new List<CreatedNeuron>();
            createdSynapses = new List<CreatedSynapse>();

            mapSynapses = new Dictionary<Synapse, CreatedSynapse>();
            mapHistory = new Dictionary<SynapseState, CreationHistory>();
            duplex = new List<Synapse>();

            this.data = data;

            frame = 0;
            count = 0;
        }

        public void load(List<AnimatedNeuron> neurons, List<AnimatedSynapse> synapses)
        {
            animated = neurons;

            foreach (AnimatedNeuron an in neurons)
            {
                CreatedNeuron neuron = new CreatedNeuron(an, null);
                neuron.finish += new EventHandler(finish);
                this.neurons.Add(neuron);
            }

            foreach (AnimatedSynapse s in synapses)
            {
                if (s.Pre is AnimatedReceptor)
                    continue;

                CreatedSynapse synapse = new CreatedSynapse(s, null);
                synapse.Pre = this.neurons.Find(k => k.getNeuron() == s.Pre);
                synapse.Post = this.neurons.Find(k => k.getNeuron() == s.Post);
                synapse.finish += new EventHandler(finish);

                s.zero();
                mapSynapses.Add(s.getSynapse(), synapse);
                this.synapses.Add(synapse);

                if (s.Duplex)
                {
                    mapSynapses.Add(s.getDuplex(), synapse);
                    duplex.Add(s.getDuplex());
                }

                if (data[0].getSynapse() == s.getSynapse())
                {
                    this.synapse = synapse;
                    element = synapse;
                    next();
                }
            }
        }

        public override void resize()
        {
            base.resize();

            foreach (CreatedNeuron n in neurons)
                n.updateGraphics(buffer.Graphics);

            foreach (CreatedSynapse s in synapses)
                s.updateGraphics(buffer.Graphics);
        }

        protected override void changeSize()
        {
            layer.Height = layer.Parent.Height - 158;
            layer.Width = layer.Parent.Width - 168;
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
                    GroupBox history = new GroupBox();
                    layer.Controls.Add(history);

                    int x = (int)(active.State.X);
                    int y = (int)(active.State.Y);

                    history.Location = new Point(x, y);
                    mapHistory.Add(active, new CreationHistory(history, active));
                }
            }

            if (animation)
                animate();
            else
                redraw();
        }

        protected override void animate()
        {
            Graphics g = buffer.Graphics;
            g.Clear(SystemColors.Control);

            if (element == null)
            {
                foreach (CreatedSynapse synapse in createdSynapses)
                    synapse.draw();

                foreach (CreatedSynapse synapse in createdSynapses)
                    synapse.drawState();

                foreach (CreatedNeuron neuron in createdNeurons)
                    neuron.draw();

                buffer.Render(graphics);
                return;
            }

            element.tick();

            foreach (CreatedSynapse synapse in createdSynapses)
                synapse.draw();

            if (element is CreatedSynapse)
                element.draw();

            foreach (CreatedSynapse synapse in createdSynapses)
                synapse.drawState();

            if (element is CreatedSynapse)
                this.synapse.drawState();

            foreach (CreatedNeuron neuron in createdNeurons)
                neuron.draw();

            if (element is CreatedNeuron)
                element.draw();

            buffer.Render(graphics);
        }

        protected override void redraw()
        {
            Graphics g = buffer.Graphics;
            g.Clear(SystemColors.Control);

            foreach (CreatedSynapse synapse in createdSynapses)
                synapse.draw();

            foreach (CreatedSynapse synapse in createdSynapses)
                synapse.drawState();

            foreach (CreatedNeuron neuron in createdNeurons)
                neuron.draw();

            buffer.Render(graphics);
        }

        public void changePace(int pace)
        {
            interval = pace / 25;

            if (element != null)
                element.setInterval(interval);

            if (synapse != null)
            {
                if (synapse.Duplex)
                    synapse.Step = synapse.DuplexWeight / interval;
                else
                    synapse.Step = synapse.SynapseWeight / interval;
            }
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
            element.create();

            if (count == data.Count)
            {
                element = null;
                return;
            }

            next();
        }

        public bool started()
        {
            return animation;
        }

        void next()
        {
            frameChanged(this, new FrameEventArgs(++frame));

            if (element is CreatedNeuron)
            {
                createdNeurons.Add((CreatedNeuron)element);
            }

                CreationData cd = data[count];
                Synapse s = cd.getSynapse();
                synapse = mapSynapses[s];

                if (!synapse.Pre.Created)
                    element = synapse.Pre;
                else if (!synapse.Post.Created)
                    element = synapse.Post;
                else
                {
                    if (duplex.Contains(s))
                    {
                        synapse.addHistory(cd, true);
                        synapse.Duplex = true;
                    }
                    else
                    {
                        synapse.addHistory(cd, false);
                        synapse.Duplex = false;
                    }

                    if (!createdSynapses.Contains(synapse))
                        createdSynapses.Add(synapse);

                    if (animation)
                        synapse.Step = cd.step(interval);
                    else
                        synapse.Step = cd.step(1);

                    element = synapse;
                    count++;
                }

            if (animation)
                element.setInterval(interval);
        }        

        void finish(object sender, EventArgs e)
        {
            if (count == data.Count)
            {
                animation = false;
                element = null;
                return;
            }

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
                AnimatedSynapse s = synapse.getSynapse();

                if(s.active(e.Location, false))
                {
                    active = s.getState(false);
                    return;
                }

                if(s.Duplex && s.active(e.Location, true))
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
        public event EventHandler<FrameEventArgs> frameChanged;
    }
}