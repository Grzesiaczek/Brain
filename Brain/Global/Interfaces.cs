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

    interface Drawable
    {
        void show();
        void hide();
        void save();
    }
}
