using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
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

    interface Drawable
    {
        void show();
        void hide();
        void save();
    }
}
