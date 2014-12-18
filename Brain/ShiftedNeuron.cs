using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class ShiftedNeuron : AnimatedNeuron
    {
        List<AnimatedNeuron> neurons;
        AnimatedNeuron shifted;
        AnimatedNeuron original;

        PointF click;
        PointF shift;

        bool collision;
        bool moved;
        int index;

        public ShiftedNeuron(AnimatedNeuron neuron, PointF click, List<AnimatedNeuron> neurons)
        {
            this.original = neuron;
            this.click = click;
            this.neurons = neurons;
            this.neuron = neuron.getNeuron();
            this.Label = neuron.Label;

            shifted = new AnimatedNeuron(neuron.getNeuron(), neuron.getGraphics(), neuron.Position);
            createCircle(neuron.Position);
            graphics = neuron.getGraphics();
            shift = new PointF();
            collision = false;
            moved = false;

            setSynapses(original.Input, original.Output);
            index = neurons.IndexOf(neuron);
        }

        public override void draw(int number)
        {
            if (collision)
                draw(3, shifted.Activity[number - 1].Value);
            else
                draw(2, shifted.Activity[number - 1].Value);
        }

        public void move(float x, float y)
        {
            shift.X = x - click.X;
            shift.Y = y - click.Y;

            setPosition(new PointF(original.Position.X + shift.X, original.Position.Y + shift.Y));
            collision = false;

            foreach (AnimatedSynapse s in Input)
                s.recalculate();

            foreach (AnimatedSynapse s in Output)
                s.recalculate();

            foreach (AnimatedNeuron neuron in neurons)
            {
                if (neuron == this)
                    continue;

                double dx = neuron.Position.X - Position.X;
                double dy = neuron.Position.Y - Position.Y;

                if (Math.Sqrt(dx * dx + dy * dy) < Config.Diameter)
                {
                    collision = true;
                    break;
                }
            }

            if (Position.X < Config.Radius || Position.X > graphics.VisibleClipBounds.Width - Config.Radius)
                collision = true;

            if (Position.Y < Config.Radius || Position.Y > graphics.VisibleClipBounds.Height - Config.Radius)
                collision = true;

            moved = true;
        }

        public void activate()
        {
            if(!moved)
                neuron.activate();
        }

        public void save(bool label)
        {
            if (collision)
            {
                original.setSynapses(Input, Output);
                neurons[index] = original;
                original.Label = label;
            }
            else
            {
                shifted.setPosition(Position);
                shifted.setSynapses(Input, Output);
                neurons[index] = shifted;
                shifted.Label = label;
            }
        }
    }
}
