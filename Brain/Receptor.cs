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
        ReceptorData data;
        Synapse synapse;
        Random random;

        int count = 0;
        int interval = 0;

        List<bool> activity;
        bool active;
        bool draw;

        public Receptor(ReceptorData data)
        {
            this.data = data;

            random = new Random();
            activity = new List<bool>();
            draw = true;
            newInterval();
        }

        public void tick()
        {
            if(++count == interval)
            {
                count = 0;
                activity.Add(true);
                active = true;

                if(draw)
                    newInterval();
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
        }

        public void setInterval(int interval)
        {
            this.interval = interval;
            draw = false;
            count = 0;
        }

        public void newInterval()
        {
            interval = (int)(data.Alpha + random.Next() % data.Beta);
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
    }
}
