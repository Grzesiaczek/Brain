using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class AnimatedReceptor : AnimatedElement
    {
        #region deklaracje

        Circle circle;
        Receptor receptor;

        AnimatedNeuron neuron;
        AnimatedSynapse synapse;

        int wall;

        #endregion

        public AnimatedReceptor(Receptor r, AnimatedNeuron n, int wall)
        {
            receptor = r;
            neuron = n;

            this.wall = wall;
            radius = Constant.Radius * 2 / 3;

            //0-góra, 1-lewo, 2-prawo, 3-dół
            switch(wall)
            {
                case 0:
                    circle = new Circle(new PointF(n.Position.X, 0), radius);
                    n.Position = new PointF(n.Position.X, 2 * Constant.Radius);
                    break;
                case 1:
                    circle = new Circle(new PointF(0, n.Position.Y), radius);
                    n.Position = new PointF(2 * Constant.Radius, n.Position.Y);
                    break;
                case 2:
                    circle = new Circle(new PointF(size.Width - 1, n.Position.Y), radius);
                    n.Position = new PointF(2 * Constant.Radius, n.Position.Y);
                    break;
                case 3:
                    circle = new Circle(new PointF(n.Position.X, size.Height - 1), radius);
                    n.Position = new PointF(n.Position.X, 2 * Constant.Radius);
                    break;
            }

            position = circle.Center;
        }

        #region osbługa żądań

        public override void changePosition()
        {
            circle.update(calculatePosition());
        }

        public int getWall()
        {
            if (wall == 0 || wall == 3)
                return 0;

            return 1;
        }

        public bool click(PointF pos)
        {
            return circle.click(pos);
        }

        #endregion

        #region rysowanie

        public void draw()
        {
            Pen pen = new Pen(Brushes.BlueViolet, 2);
            circle.draw(graphics, Brushes.LightYellow, pen);
        }

        public void draw(int frame)
        {
            Pen pen = new Pen(Brushes.BlueViolet, 2);

            if(Activity[frame - 1])
                circle.draw(graphics, Brushes.OrangeRed, pen);
            else
                circle.draw(graphics, Brushes.LightYellow, pen);
        }

        #endregion

        #region właściwości

        public List<bool> Activity
        {
            get
            {
                return receptor.Activity;
            }
        }

        public override PointF Location
        {
            get
            {
                return circle.Center;
            }
            set
            {
                circle.update(value);
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
                circle.update(calculatePosition());
            }
        }

        public AnimatedSynapse Output
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

        public override float Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;
                circle.Radius = radius;
            }
        }

        #endregion
    }
}
