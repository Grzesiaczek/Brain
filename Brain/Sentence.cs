using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brain
{
    class Sentence : TopLayer
    {
        List<String> tak;

        public Sentence()
        { 

        }
        /*
        protected override void changeSize()
        {
            Height = 90;
            Width = Parent.Width - 168;
            initializeGraphics();
        }

        protected override void tick(object sender, EventArgs e)
        {
            buffer.Graphics.Clear(Color.FromArgb(255, 225, 225, 225));

           

            buffer.Render(graphics);
        }*/
    }

    class LearnedNeuron
    {
        Neuron neuron;

        public LearnedNeuron(Neuron neuron)
        {
            this.neuron = neuron;
        }
    }
}
