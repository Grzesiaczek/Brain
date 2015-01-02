using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    interface AnimatedElement
    {
        void draw(int frame);

        PointF Position
        {
            get;
            set;
        }

        float Radius
        {
            get;
        }

        String Name
        {
            get;
        }
    }

    interface Element
    {
        bool Active
        {
            get;
            set;
        }

        String Name
        {
            get;
        }
    }

    interface Controller
    {
        void start();
        void stop();
        void back();
        void forth();
        bool started();
        void changePace(int pace);
    }
}
