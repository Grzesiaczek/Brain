using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class CreatedSynapse : CreatedElement
    {
        AnimatedSynapse synapse;
        CreatedNeuron pre;
        CreatedNeuron post;

        public override event EventHandler finish;

        float step;
        bool duplex;

        public CreatedSynapse(AnimatedSynapse synapse, Graphics g)
        {
            this.synapse = synapse;
            graphics = g;
            created = false;
        }

        public override void create()
        {
            created = true;
            synapse.getState(duplex).Weight += step;
        }

        public override void draw()
        {
            if (created)
                synapse.draw(graphics);
            else
                synapse.draw(graphics, (float)frame / interval);
        }

        public void drawState()
        {
            if (created)
                synapse.drawState(graphics);
        }

        public override void tick()
        {
            if (created)
                synapse.getState(duplex).Weight += step;

            if (++frame == interval)
            {
                if (!created)
                {
                    created = true;
                    frame = 0;
                    return;
                }

                frame = 0;
                finish(this, new EventArgs());
                return;
            }
        }

        public void addHistory(CreationData data, bool duplex)
        {
            synapse.addHistory(data, duplex);
        }

        public AnimatedSynapse getSynapse()
        {
            return synapse;
        }

        public CreatedNeuron Pre
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

        public CreatedNeuron Post
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

        public bool Duplex
        {
            get
            {
                return duplex;
            }
            set
            {
                duplex = value;
            }
        }

        public float Step
        {
            set
            {
                step = value;
            }
        }

        public float DuplexWeight
        {
            get
            {
                return synapse.getDuplex().Weight;
            }
        }

        public float SynapseWeight
        {
            get
            {
                return synapse.getSynapse().Weight;
            }
        }
    }
}