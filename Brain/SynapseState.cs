using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class SynapseState
    {
        Circle state;
        Circle control;

        List<CreationData> history;
        Synapse synapse;

        bool activated;
        float weight;

        public SynapseState(Synapse synapse)
        {
            this.synapse = synapse;
            history = new List<CreationData>();
        }

        public void load(Circle state, Circle control)
        {
            this.state = state;
            this.control = control;
        }

        public void draw(Graphics g, int frame)
        {
            Brush brush = Brushes.LightYellow;
            Pen pen = new Pen(Brushes.DarkSlateGray, 2);

            if (synapse.Activity[frame - 1])
                brush = Brushes.Red;

            control.draw(g, brush, pen);
            state.draw(g, synapse.Weight, pen);
        }

        public void draw(Graphics g)
        {
            Pen pen = new Pen(Brushes.DarkSlateGray, 2);

            if(activated)
                control.draw(g, Brushes.LightSkyBlue, pen);
            else
                control.draw(g, Brushes.LightYellow, pen);

            state.draw(g, Weight, pen);
        }

        public bool active(Point location)
        {
            if (synapse.Weight == 0)
                return false;

            float x = state.Center.X - location.X;
            float y = state.Center.Y - location.Y;

            if (x * x + y * y < 144)
            {
                activated = true;
                return true;
            }

            activated = false;
            return false;
        }

        public void create()
        {
            weight = synapse.Weight;
            activated = false;
        }

        public Synapse Synapse
        {
            get
            {
                return synapse;
            }
        }

        public List<bool> Activity
        {
            get
            {
                return synapse.Activity;
            }
            set
            {
                synapse.Activity = value;
            }
        }

        public List<CreationData> History
        {
            get
            {
                return history;
            }
            set
            {
                history = value;
            }
        }

        public PointF State
        {
            get
            {
                return state.Center;
            }
            set
            {
                state.Center = value;
            }
        }

        public PointF Control
        {
            get
            {
                return control.Center;
            }
            set
            {
                control.Center = value;
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
    }
}