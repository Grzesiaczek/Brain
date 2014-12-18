﻿using System;
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
    }

    interface Element
    {
        bool Active
        {
            get;
            set;
        }
    }
}