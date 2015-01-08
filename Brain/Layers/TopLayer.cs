using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brain
{
    class TopLayer : Layer
    {
        public TopLayer(Control parent) : base(parent)
        { 

        }
        
        public override void resize()
        {
            Height = 90;
            Width = Parent.Width - 168;
            initializeGraphics();
        }

        protected override void tick(object sender, EventArgs e)
        {

        }
    }
}
