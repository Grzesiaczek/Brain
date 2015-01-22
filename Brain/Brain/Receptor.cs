using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class Receptor : Element
    {
        Synapse synapse;

        int count = 0;
        int interval = 0;

        List<bool> activity;
        bool active;

        public Receptor()
        {
            activity = new List<bool>();
            interval = 0;
        }

        public void initialize(int interval, int count, float weight)
        {
            activity = new List<bool>();
            this.interval = interval;
            this.count = count;
            synapse.Weight = weight;
        }

        public void tick()
        {
            if(++count == interval)
            {
                count = 0;
                activity.Add(true);
                active = true;
            }
            else
            {
                activity.Add(false);
                active = false;
            }
        }

        public void tick(bool value)
        {
            activity.Add(value);
            active = value;
        }

        public void undo()
        {
            if (activity.Count > 1)
                activity.RemoveAt(activity.Count - 1);
        }

        public void clear()
        {
            activity.Clear();
            activity.Add(false);
            active = false;

            interval = 0;
            count = 0;
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

        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
            }
        }

        public Synapse Output
        {
            get
            {
                return synapse;
            }
            set
            {
                synapse = value;
            }
        }

        public String Name
        {
            get
            {
                return synapse.Post.Name;
            }
        }
    }
}
