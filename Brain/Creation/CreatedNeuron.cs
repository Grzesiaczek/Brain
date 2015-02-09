using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class CreatedNeuron
    {
        AnimatedNeuron neuron;

        bool created;
        int frame;

        public CreatedNeuron(AnimatedNeuron neuron)
        {
            this.neuron = neuron;
            created = false;
        }

        public void create()
        {
            created = true;
            neuron.Radius = Constant.Radius;
        }

        public void delete()
        {
            created = false;
            neuron.Radius = 0;
        }

        public void load()
        {
            neuron.Radius = 0;
        }

        public void draw()
        {
            neuron.draw();
        }

        public void draw(float factor)
        {
            neuron.Radius = factor * Constant.Radius;
            neuron.draw();
        }

        #region właściwości

        public bool Created
        {
            get
            {
                return created;
            }
            set
            {
                created = value;
            }
        }

        public int Frame
        {
            get
            {
                return frame;
            }
            set
            {
                frame = value;
            }
        }

        #endregion
    }
}
