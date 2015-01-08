using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brain
{
    class LayerSequence : TopLayer
    {
        List<List<Neuron>> data;

        public LayerSequence(Control parent): base(parent)
        { 

        }
        /*
        protected override void tick(object sender, EventArgs e)
        {
            buffer.Graphics.Clear(Color.FromArgb(255, 225, 225, 225));

           

            buffer.Render(graphics);
        }*/

        public void next()
        {

        }
    }

    class LearningSequence
    {
        List<Neuron> neurons;

        public LearningSequence(List<Neuron> neurons)
        {
            this.neurons = neurons;
        }

        public List<Neuron> Neurons
        {
            get
            {
                return neurons;
            }
        }
    }
}
