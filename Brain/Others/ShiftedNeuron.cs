using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class ShiftedNeuron
    {
        List<AnimatedNeuron> neurons;
        AnimatedNeuron neuron;

        PointF click;
        PointF shift;
        PointF original;

        bool moved;

        public ShiftedNeuron(AnimatedNeuron neuron, PointF click, List<AnimatedNeuron> neurons)
        {
            this.click = click;
            this.neuron = neuron;
            this.neurons = neurons;

            shift = new PointF();
            original = new PointF(neuron.Position.X, neuron.Position.Y);

            neuron.activate(true);
            moved = false;
        }

        public void move(float x, float y)
        {
            shift.X = (x - click.X) / AnimatedElement.Factor;
            shift.Y = (y - click.Y) / AnimatedElement.Factor;

            neuron.Position = new PointF(original.X + shift.X, original.Y + shift.Y);
            neuron.checkCollision(neurons);
            neuron.recalculate();
            moved = true;
        }

        public void activate()
        {
            if(!moved)
                neuron.activate(false);
        }

        public void save()
        {
            neuron.save(original);
        }
    }
}
