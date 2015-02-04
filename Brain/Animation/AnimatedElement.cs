using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    abstract class AnimatedElement
    {
        #region deklaracje

        protected PointF position;
        protected PointF shift;

        protected float radius;
        protected bool drawable;

        protected static Graphics graphics;
        protected static Rectangle area;
        protected static PointF padding;
        protected static Size size;

        protected static bool animation;
        protected static float factor;

        #endregion

        #region logika

        public abstract void changePosition();

        public virtual void calculateShift(int frames)
        {
            PointF position = calculatePosition();

            float dx = position.X - Location.X;
            float dy = position.Y - Location.Y;

            shift = new PointF(dx / frames, dy / frames);
        }

        public virtual void executeShift()
        {
            float x = Location.X + shift.X;
            float y = Location.Y + shift.Y;
            checkDrawable();

            Location = new PointF(x, y);
        }

        protected PointF calculatePosition()
        {
            float x = factor * position.X + padding.X;
            float y = factor * position.Y + padding.Y;
            return new PointF(x, y);
        }

        protected void checkDrawable()
        {
            drawable = true;

            if (Location.X < 0 || Location.X > area.Width)
                drawable = false;

            if (Location.Y < 0 || Location.Y > area.Height)
                drawable = false;
        }

        #endregion

        #region właściwości

        public static Graphics Graphics
        {
            set
            {
                graphics = value;
            }
        }

        public static Rectangle Area
        {
            set
            {
                area = value;
            }
        }

        public static PointF Padding
        {
            set
            {
                padding = value;
            }
        }

        public static Size Size
        {
            set
            {
                size = value;
            }
        }

        public static bool Animation
        {
            set
            {
                animation = value;
            }
        }

        public static float Factor
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

        public virtual float Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;
            }
        }

        public abstract PointF Location
        {
            get;
            set;
        }

        public abstract PointF Position
        {
            get;
            set;
        }

        #endregion
    }
}
