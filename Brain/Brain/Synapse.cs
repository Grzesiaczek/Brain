using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Brain
{
    class Synapse
    {
        #region deklaracje

        Element pre;
        Element post;

        float factor;
        float weight;

        List<CreationFrame> changes;
        List<bool> activity;

        #endregion

        #region konstruktory

        public Synapse(Neuron pre, Neuron post)
        {
            this.pre = pre;
            this.post = post;
            initialize();
        }

        public Synapse(Receptor pre, Neuron post)
        {
            this.pre = pre;
            this.post = post;
            initialize();
        }

        void initialize()
        {
            activity = new List<bool>();
            changes = new List<CreationFrame>();
        }

        #endregion

        #region logika

        public void tick()
        {
            if (Pre.Active)
            {
                activity.Add(true);
                ((Neuron)post).receiveSignal(weight);
            }
            else
                activity.Add(false);
        }

        public void refresh(bool value)
        {
            if (value)
            {
                activity[activity.Count - 1] = true;
                ((Neuron)post).receiveSignal(weight);
            }
            else
                activity[activity.Count - 1] = false;
        }

        public void undo()
        {
            if (activity.Count > 1)
                activity.RemoveAt(activity.Count - 1);

            if(activity[activity.Count - 1])
                ((Neuron)post).receiveSignal(weight);
        }

        public void clear(bool init)
        {
            activity.Clear();

            if (init)
                activity.Add(false);
        }

        #endregion

        #region właściwości

        public Element Pre
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

        public Element Post
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
        
        public List<bool> Activity
        {
            get
            {
                return activity;
            }
            set
            {
                activity = value;
            }
        }

        public List<CreationFrame> t
        {
            get
            {
                return changes;
            }
        }

        public float Factor
        {
            get
            {
                return factor;
            }
            set
            {
                factor = value;
            }
        }

        public float Weight
        {
            get
            {
                return weight;
            }
            set
            {
                weight = value;
            }
        }

        #endregion
    }
}
