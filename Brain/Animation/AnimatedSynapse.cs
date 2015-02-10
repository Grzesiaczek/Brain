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
        #region deklaracje

        AnimatedElement pre;
        AnimatedElement post;

        AnimatedVector vector;

        AnimatedState synapse;
        AnimatedState duplex;

        #endregion

        #region konstruktory

        public AnimatedSynapse(AnimatedNeuron pre, AnimatedNeuron post, Synapse syn)
        {
            this.pre = pre;
            this.post = post;

            duplex = null;
            vector = new AnimatedVector(pre, post);
            synapse = new AnimatedState(syn, vector);

            pre.Output.Add(this);
            post.Input.Add(this);

            changePosition();
        }

        public AnimatedSynapse(AnimatedReceptor pre, AnimatedNeuron post, Synapse syn)
        {
            this.pre = pre;
            this.post = post;
            
            duplex = null;

            vector = new AnimatedVector(pre, post);
            synapse = new AnimatedState(syn, vector);

            pre.Output = this;
            post.Input.Add(this);

            changePosition();
        }

        #endregion

        #region pozycjonowanie

        public override void changePosition()
        {
            vector.changePosition();
            synapse.changePosition();

            if (duplex != null)
                duplex.changePosition();
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
                vector.drawSignal(factor);

            if (duplex != null && duplex.Activity[frame - 1])
                vector.drawSignal(1 - factor);
        }

        public void draw()
        {
            vector.draw();
        }

        public void draw(float factor)
        {
            vector.draw(factor);
        }

        public void drawState()
        {
            synapse.draw();

            if (duplex != null)
                duplex.draw();
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

        public void undo(CreationData data)
        {
            if (data.Synapse == synapse.Synapse)
                synapse.Weight = data.Start;
            else
                duplex.Weight = data.Weight;
        }

        public void create()
        {
            synapse.create();

            if (duplex != null)
                duplex.create();
        }

        public void setDuplex(Synapse synapse)
        {
            duplex = new AnimatedState(synapse, vector, true);
            duplex.changePosition();
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

        public AnimatedVector Vector
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
