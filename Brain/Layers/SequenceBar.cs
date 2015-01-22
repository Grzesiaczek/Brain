using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brain
{
    class SequenceBar : Layer
    {
        bool active;

        public SequenceBar()
        {
            Active = true;
            Location = new Point(10, 10);
            BackColor = Color.FromArgb(255, 225, 225, 225);
        }
        
        public override void resize()
        {
            Height = 90;
            Width = Parent.Width - 158;
            initializeGraphics();
        }

        protected override void tick(object sender, EventArgs e)
        {
        }

        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
                Visible = value;
            }
        }
    }
}
