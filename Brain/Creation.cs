using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brain
{
    class Creation : Layer
    {
        public Creation(GroupBox groupBox) : base(groupBox)
        {
        }

        protected override void changeSize()
        {
            layer.Height = layer.Parent.Height - 58;
            layer.Width = layer.Parent.Width - 168;

            initializeGraphics();
        }

        protected override void tick(object sender, EventArgs e)
        {

        }
    }
}
