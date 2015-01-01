using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class CreatedNeuron : CreatedElement
    {
        AnimatedNeuron neuron;

        public override event EventHandler finish;

        public CreatedNeuron(AnimatedNeuron neuron, Graphics g)
        {
            this.neuron = neuron;
            neuron.setRadius(0);
            created = false;
            frame = 0;
        }

        public override void create()
        {
            created = true;
            neuron.setRadius(Config.Radius);
        }

        public override void draw()
        {
            neuron.draw(graphics);
        }

        public override void tick()
        {
            if (++frame == interval)
            {
                created = true;
                neuron.setRadius(Config.Radius);
                finish(this, new EventArgs());
                return;
            }

            neuron.setRadius((float)(frame * Config.Radius) / interval);
        }

        public AnimatedNeuron getNeuron()
        {
            return neuron;
        }
    }
}
