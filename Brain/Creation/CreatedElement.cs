using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    abstract class CreatedElement
    {
        protected int interval;
        protected int frame;

        protected Graphics graphics;
        protected bool created;

        public void setInterval(int interval)
        {
            if (this.interval != 0)
            {
                float ratio = (float)interval / this.interval;
                frame = (int)(ratio * frame);
            }

            this.interval = interval;
        }

        public virtual void updateGraphics(Graphics g)
        {
            graphics = g;
        }

        public virtual bool Created
        {
            get
            {
                return created;
            }
        }

        public abstract void create();
        public abstract void draw();
        public abstract void tick();

        public abstract event EventHandler finish;
    }
}
