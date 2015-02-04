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

        Circle state;
        Circle control;

        List<CreationData> history;
        Synapse synapse;

        bool activated;
        bool duplex;

        float change;
        float weight;

        #endregion

        public AnimatedState(Synapse synapse, bool duplex = false)
        {
            this.duplex = duplex;
            this.synapse = synapse;

            radius = Constant.Radius / 2;
            history = new List<CreationData>();
        }

        #region sterowanie

        public void load(Vector vector)
        {
            if(duplex)
            {
                state = new Circle(vector.getPoint(vector.Start, 50 + (int)(vector.Length / 12)), radius);
                control = new Circle(vector.getPoint(state.Center, (int)(3 * radius / 4)), radius);
            }
            else
            {
                state = new Circle(vector.getPoint(vector.End, -50 - (int)(vector.Length / 12)), radius);
                control = new Circle(vector.getPoint(state.Center, (int)(-3 * radius / 4)), radius);
            }                
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

        public override void changePosition()
        {
            
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