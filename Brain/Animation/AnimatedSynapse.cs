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

        AnimatedState synapse;
        AnimatedState duplex;
        
        public AnimatedSynapse(AnimatedNeuron pre, AnimatedNeuron post, Synapse synapse)
        {
            this.pre = pre;
            this.post = post;
            this.synapse = new AnimatedState(synapse);

            duplex = null;
            vector = new Vector();

            pre.Output.Add(this);
            post.Input.Add(this);

            changePosition();
        }

        public AnimatedSynapse(AnimatedReceptor pre, AnimatedNeuron post, Synapse synapse)
        {
            this.pre = pre;
            this.post = post;
            this.synapse = new AnimatedState(synapse);

            duplex = null;
            vector = new Vector();

            pre.Output = this;
            post.Input.Add(this);

            changePosition();
        }

        #region pozycjonowanie

        public override void changePosition()
        {
            vector.update(pre.Location, post.Location);
            synapse.load(vector);

            if (duplex != null)
                duplex.load(vector);
        }

        public override void executeShift()
        {
            changePosition();
        }

        #endregion

        #region rysowanie

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

        public void draw(float factor)
        {
            vector.draw(graphics, factor);
        }

        public void drawState()
        {
            drawState(0);
        }

        public void drawState(int frame)
        {
            synapse.draw(frame);

            if (duplex != null)
                duplex.draw(frame);
        }
        #endregion

        #region sterowanie

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
            duplex = new AnimatedState(synapse, true);
            duplex.load(vector);
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

        public AnimatedState getState(bool duplex)
        {
            if (duplex)
                return this.duplex;

            return synapse;
        }
        #endregion

        #region właściwości

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

        public override PointF Location
        {
            get
            {
                return position;
            }
            set
            {

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

        public override float Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;
                synapse.Radius = radius;

                if (duplex != null)
                    duplex.Radius = radius;
            }
        }

        #endregion
    }
}
