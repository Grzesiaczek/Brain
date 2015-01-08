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
        Circle circle;
        Receptor receptor;

        AnimatedNeuron neuron;
        AnimatedSynapse synapse;

        int wall;

        public AnimatedReceptor(Receptor r, AnimatedNeuron n, int wall)
        {
            receptor = r;
            neuron = n;

            this.wall = wall;
            radius = 16;

            //0-góra, 1-lewo, 2-prawo, 3-dół
            switch(wall)
            {
                case 0:
                    circle = new Circle(new PointF(n.Position.X, 0), radius);
                    n.Position = new PointF(n.Position.X, 2 * Config.Radius);
                    break;
                case 1:
                    circle = new Circle(new PointF(0, n.Position.Y), radius);
                    n.Position = new PointF(2 * Config.Radius, n.Position.Y);
                    break;
                case 2:
                    circle = new Circle(new PointF(size.Width - 1, n.Position.Y), radius);
                    n.Position = new PointF(2 * Config.Radius, n.Position.Y);
                    break;
                case 3:
                    circle = new Circle(new PointF(n.Position.X, size.Height - 1), radius);
                    n.Position = new PointF(n.Position.X, 2 * Config.Radius);
                    break;
            }

            position = circle.Center;
        }

        public void draw(int frame)
        {
            Pen pen = new Pen(Brushes.BlueViolet, 2);

            if(Activity[frame - 1])
                circle.draw(graphics, Brushes.OrangeRed, pen);
            else
                circle.draw(graphics, Brushes.LightYellow, pen);
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

        public void setInterval(int interval)
        {
            receptor.setInterval(interval);
        }

        public List<bool> Activity
        {
            get
            {
                return receptor.Activity;
            }
        }

        public String Name
        {
            get
            {
                return neuron.Neuron.Word;
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
                circle.update(value);
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
    }
}
