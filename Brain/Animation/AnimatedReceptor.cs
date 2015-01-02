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
        Graphics graphics;

        Circle circle;
        Receptor receptor;

        AnimatedNeuron neuron;
        AnimatedSynapse synapse;

        int wall;

        public AnimatedReceptor(Receptor r, AnimatedNeuron n, int wall)
        {
            receptor = r;
            neuron = n;
            graphics = n.Graphics;
            this.wall = wall;

            //0-góra, 1-lewo, 2-prawo, 3-dół
            switch(wall)
            {
                case 0:
                    circle = new Circle(new PointF(n.Position.X, 0), 16);

                    if (n.Position.Y > graphics.VisibleClipBounds.Height / 2)
                        n.setPosition(new PointF(n.Position.X, graphics.VisibleClipBounds.Height - n.Position.Y));

                    break;
                case 1:
                    circle = new Circle(new PointF(0, n.Position.Y), 16);

                    if (n.Position.X > graphics.VisibleClipBounds.Width / 2)
                        n.setPosition(new PointF(graphics.VisibleClipBounds.Width - n.Position.X, n.Position.Y));

                    break;
                case 2:
                    circle = new Circle(new PointF(graphics.VisibleClipBounds.Width - 1, n.Position.Y), 16);

                    if (n.Position.X < graphics.VisibleClipBounds.Width / 2)
                        n.setPosition(new PointF(graphics.VisibleClipBounds.Width - n.Position.X, n.Position.Y));

                    break;
                case 3:
                    circle = new Circle(new PointF(n.Position.X, graphics.VisibleClipBounds.Height - 1), 16);

                    if (n.Position.Y < graphics.VisibleClipBounds.Height / 2)
                        n.setPosition(new PointF(n.Position.X, graphics.VisibleClipBounds.Height - n.Position.Y));

                    break;
            }
        }

        public void draw(int frame)
        {
            Pen pen = new Pen(Brushes.BlueViolet, 2);

            if(Activity[frame - 1])
                circle.draw(graphics, Brushes.OrangeRed, pen);
            else
                circle.draw(graphics, Brushes.LightYellow, pen);
        }

        public void setPosition(PointF pos)
        {
            circle.update(pos);
        }

        public void updateGraphics(Graphics g)
        {
            try
            {
                float fx = g.VisibleClipBounds.Width / graphics.VisibleClipBounds.Width;
                float fy = g.VisibleClipBounds.Height / graphics.VisibleClipBounds.Height;

                switch (wall)
                {
                    case 0:
                        circle.update(new PointF(fx * circle.Center.X, 0));
                        break;
                    case 1:
                        circle.update(new PointF(0, fx * circle.Center.Y));
                        break;
                    case 2:
                        circle.update(new PointF(graphics.VisibleClipBounds.Width - 1, fx * circle.Center.Y));
                        break;
                    case 3:
                        circle.update(new PointF(fx * circle.Center.X, graphics.VisibleClipBounds.Height - 1));
                        break;
                }
            }
            catch (Exception) { }

            graphics = g;
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

        public PointF Position
        {
            get
            {
                return circle.Center;
            }
            set
            {
                circle.Center = value;
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

        public float Radius
        {
            get
            {
                return 16;
            }
        }
    }
}
