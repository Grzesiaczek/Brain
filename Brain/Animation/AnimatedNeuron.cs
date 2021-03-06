﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class AnimatedNeuron : AnimatedElement
    {
        #region deklaracje

        Neuron neuron;
        Circle circle;

        List<AnimatedSynapse> input;
        List<AnimatedSynapse> output;

        bool collision = false;
        bool shifted = false;

        bool label = true;
        bool state = true;

        int frame;

        #endregion

        #region konstruktory

        public AnimatedNeuron() { }

        public AnimatedNeuron(Neuron n, PointF pos)
        {
            neuron = n;
            radius = Constant.Radius;

            position = pos;
            circle = new Circle(calculatePosition(), Constant.Radius);
            checkDrawable();

            input = new List<AnimatedSynapse>();
            output = new List<AnimatedSynapse>();
        }

        #endregion

        #region logika

        public void checkCollision(List<AnimatedNeuron> neurons)
        {
            collision = false;

            foreach (AnimatedNeuron an in neurons)
            {
                if (an == this)
                    continue;

                double dx = Location.X - an.Location.X;
                double dy = Location.Y - an.Location.Y;

                if (Math.Sqrt(dx * dx + dy * dy) < 2 * radius)
                {
                    collision = true;
                    break;
                }
            }

            if (Position.X < radius || Position.X > size.Width - radius)
                collision = true;

            if (Position.Y < radius || Position.Y > size.Height - radius)
                collision = true;
        }

        public void activate(bool shifted)
        {
            if (shifted)
                this.shifted = true;
            else
                neuron.activate();
        }

        public void save(PointF original)
        {
            if(collision)
            {
                Position = new PointF(original.X, original.Y);
                recalculate();
            }

            shifted = false;
        }

        public void recalculate()
        {
            foreach (AnimatedSynapse s in input)
                s.changePosition();

            foreach (AnimatedSynapse s in output)
                s.changePosition();
        }

        public override void changePosition()
        {
            circle.update(calculatePosition());
        }

        public bool click(PointF pos)
        {
            return circle.click(pos);
        }

        public void create()
        {
            Radius = Constant.Radius;
        }

        #endregion

        #region rysowanie

        public void animate(int number, int frame, double factor)
        {
            if (!drawable)
                return;

            NeuronData data = neuron.Activity[number];
            double delta = factor * data.Relaxation;

            if (data.Impulse < 0)
                delta += data.Impulse;
            else if (factor > 0.75)
            {
                factor = 4 * (factor - 0.75);
                delta += factor * data.Impulse;
            }

            draw(data.Initial + delta);
        }

        public void draw()
        {
            if (!drawable)
                return;

            circle.draw(graphics);

            if (radius == Constant.Radius && label)
                drawLabel();
        }

        public void draw(int number)
        {
            if (!drawable)
                return;

            draw(neuron.Activity[number - 1].Value);
        }

        public void draw(double value)
        {
            if (!drawable)
                return;

            Pen pen = new Pen(Brushes.Purple, 3);

            if (shifted)
            {
                if(collision)
                    pen = new Pen(Brushes.IndianRed, 3);
                else
                    pen = new Pen(Brushes.Green, 3);
            }

            if (value >= 1)
            {
                Brush brush;

                if (animation)
                {
                    if (frame++ % 8 < 4)
                        brush = Brushes.Red;
                    else
                        brush = Brushes.Orange;
                }
                else
                    brush = Brushes.OrangeRed;

                if (state)
                    circle.draw(graphics, brush, pen, ((int)(value * 100)).ToString());
                else
                    circle.draw(graphics, brush, pen);
            }
            else
            {
                if (state)
                    circle.draw(graphics, value, pen, ((int)(value * 100)).ToString());
                else
                    circle.draw(graphics, value, pen);
            }

            if (label)
                drawLabel();
        }

        void drawLabel()
        {
            if (!drawable)
                return;

            float size = radius / 4 + 3;
            float width = neuron.Word.Length * size + 6;
            float x = Location.X;
            float y = Location.Y + radius + 12;

            RectangleF rect = new RectangleF(x - width / 2, y - size - 1, width, size + 6);
            graphics.FillRectangle(new SolidBrush(Color.FromArgb(160, Color.AliceBlue)), rect);
            graphics.DrawString(neuron.Word, new Font("Verdana", size, FontStyle.Bold), Brushes.Purple, x, y, Constant.Format);
        }

        #endregion

        #region właściwości

        public Neuron Neuron
        {
            get
            {
                return neuron;
            }
        }

        public String Name
        {
            get
            {
                return neuron.Word;
            }
        }

        public List<AnimatedSynapse> Input
        {
            get
            {
                return input;
            }
            set
            {
                input = value;
            }
        }

        public List<AnimatedSynapse> Output
        {
            get
            {
                return output;
            }
            set
            {
                output = value;
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
                checkDrawable();
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
                checkDrawable();
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

        public bool Label
        {
            get
            {
                return label;
            }
            set
            {
                label = value;
            }
        }

        public bool State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }
        #endregion
    }
}
