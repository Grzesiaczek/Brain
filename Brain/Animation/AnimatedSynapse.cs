using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class AnimatedSynapse : AnimatedElement
    {
        AnimatedElement pre;
        AnimatedElement post;

        Vector vector;

        SynapseState synapse;
        SynapseState duplex;
        
        public AnimatedSynapse(AnimatedNeuron pre, AnimatedNeuron post, Synapse synapse)
        {
            this.pre = pre;
            this.post = post;
            this.synapse = new SynapseState(synapse);

            duplex = null;
            vector = new Vector();

            pre.Output.Add(this);
            post.Input.Add(this);

            calculate();
        }

        public AnimatedSynapse(AnimatedReceptor pre, AnimatedNeuron post, Synapse synapse)
        {
            this.pre = pre;
            this.post = post;
            this.synapse = new SynapseState(synapse);

            duplex = null;
            vector = new Vector();

            pre.Output = this;
            post.Input.Add(this);

            calculate();
        }

        public AnimatedSynapse(BinaryReader reader, List<AnimatedNeuron> neurons)
        {/*
            int id = reader.ReadInt32();
            pre = neurons.Find(k => k.ID == id);

            id = reader.ReadInt32();
            post = neurons.Find(k => k.ID == id);

            graphics = g;
            duplex = reader.ReadBoolean();

            pre.Output.Add(this);
            post.Input.Add(this);
            */

            calculate();
        }

        public void calculate()
        {
            vector.update(pre.Position, post.Position);
            synapse.load(vector);

            if (duplex != null)
                duplex.load(vector);
        }

        public void animate(int frame, float factor)
        {
            draw();

            if (synapse.Activity[frame - 1])
                vector.drawSignal(graphics, factor);

            if (duplex != null && duplex.Activity[frame - 1])
                vector.drawSignal(graphics, 1 - factor);
        }

        public void draw()
        {
            vector.draw(graphics);
        }

        public void draw(Graphics g)
        {
            vector.draw(g);
        }

        public void draw(Graphics g, float factor)
        {
            vector.draw(g, factor);
        }

        public void drawState()
        {
            drawState(0);
        }

        public void drawState(Graphics g)
        {
            synapse.draw(g);

            if (duplex != null && duplex.Weight > 0)
                duplex.draw(g);
        }

        public void drawState(int frame)
        {
            synapse.draw(graphics, frame);

            if (duplex != null)
                duplex.draw(graphics, frame);
        }

        public void change(CreationData data)
        {
            if (data.Synapse == synapse.Synapse)
                synapse.Change = data.Weight - synapse.Weight;
            else
                duplex.Change = data.Weight - duplex.Weight;
        }

        public void create(CreationData data)
        {
            if (data.Synapse == synapse.Synapse)
            {
                synapse.History.Add(data);
                synapse.Weight = data.Weight;
                synapse.Change = 0;
            }
            else
            {
                duplex.History.Add(data);
                duplex.Weight = data.Weight;
                duplex.Change = 0;
            }
        }

        public void create()
        {
            synapse.create();

            if (duplex != null)
                duplex.create();
        }

        public void setDuplex(Synapse synapse)
        {
            duplex = new SynapseState(synapse, true);
            duplex.load(vector);
        }

        public void save(BinaryWriter writer)
        {
            //writer.Write(pre.ID);
            //writer.Write(post.ID);
            //writer.Write(duplex);
        }

        public bool active(Point location, bool duplex)
        {
            if (duplex)
                return this.duplex.active(location);

            return synapse.active(location);
        }

        public bool isDuplex()
        {
            if (duplex == null)
                return false;

            return true;
        }

        public SynapseState getState(bool duplex)
        {
            if (duplex)
                return this.duplex;

            return synapse;
        }

        public AnimatedElement Pre
        {
            get
            {
                return pre;
            }
            set
            {
                pre = value;
            }
        }

        public AnimatedElement Post
        {
            get
            {
                return post;
            }
            set
            {
                post = value;
            }
        }

        public Vector Vector
        {
            get
            {
                return vector;
            }
            set
            {
                vector = value;
            }
        } 

        public Synapse Synapse
        {
            get
            {
                return synapse.Synapse;
            }
        }

        public Synapse Duplex
        {
            get
            {
                return duplex.Synapse;
            }
        }

        public override PointF Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        public float getWeight()
        {
            float result = synapse.Weight;

            if (duplex != null)
                result += duplex.Weight;

            return result;
        }
    }
}
