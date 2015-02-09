using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class AnimatedState : AnimatedElement
    {
        #region deklaracje

        AnimatedVector vector;
        Circle state;
        Circle control;

        List<CreationData> history;
        Synapse synapse;

        bool activated;
        bool duplex;

        float change;
        float weight;

        #endregion

        public AnimatedState(Synapse synapse, AnimatedVector vector, bool duplex = false)
        {
            this.synapse = synapse;
            this.vector = vector;
            this.duplex = duplex;

            radius = Constant.Radius / 2;
            history = new List<CreationData>();
        }

        #region sterowanie

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

        public override void changePosition()
        {
            float length = vector.Length * factor;
            int stateLength = 50 + (int)(length / 12);
            int controlLength = 50 + (int)((9 * radius + length) / 12);

            if (duplex)
            {
                state = new Circle(vector.getLocation(stateLength, true), radius);
                control = new Circle(vector.getLocation(controlLength, true), radius);
            }
            else
            {
                state = new Circle(vector.getLocation(stateLength), radius);
                control = new Circle(vector.getLocation(controlLength), radius);
            }   
        }

        #endregion

        #region rysowanie

        public void draw(int frame)
        {
            draw(graphics, frame);
        }

        public void draw(Graphics g, int frame)
        {
            Brush brush = Brushes.LightYellow;
            Pen pen = new Pen(Brushes.DarkSlateGray, 2);

            if (frame > 0 && synapse.Activity[frame - 1])
                brush = Brushes.Red;

            control.draw(graphics, brush, pen);
            state.draw(g, synapse.Weight, pen);
        }

        public void draw(Graphics g)
        {
            Pen pen = new Pen(Brushes.DarkSlateGray, 2);

            if (change == 0)
            {
                if (activated)
                    control.draw(g, Brushes.LightSkyBlue, pen);
                else
                    control.draw(g, Brushes.LightYellow, pen);

                state.draw(g, Weight, pen);
            }
            else
            {
                if (activated)
                    control.draw(g, Brushes.SkyBlue, pen);
                else if(change > 0)
                    control.draw(g, Brushes.DarkOliveGreen, pen);
                else
                    control.draw(g, Brushes.Violet, pen);

                state.draw(g, weight, change, pen);
            }
        }
        #endregion

        #region właściwości

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

        public float Change
        {
            get
            {
                return change;
            }
            set
            {
                change = value;
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

        public override float Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;
                state.Radius = value;
                control.Radius = value;
            }
        }

        public override PointF Position
        {
            get
            {
                return state.Center;
            }
            set
            {

            }
        }

        public override PointF Location
        {
            get
            {
                return state.Center;
            }
            set
            {

            }
        }

        #endregion
    }
}