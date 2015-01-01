using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brain
{
    abstract class TopLayer : Layer
    {
        public TopLayer(GroupBox groupBox) : base(groupBox) { }
    }
}
