﻿using System;
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
        static Graphics graphics;

        bool created;

        public CreatedNeuron(AnimatedNeuron neuron)
        {
            this.neuron = neuron;
            neuron.Radius = 0;
            created = false;
        }

        public void create()
        {
            created = true;
            neuron.Radius = Constant.Radius;
        }

        public void draw()
        {
            neuron.draw(graphics);
        }

        public void draw(float factor)
        {
            neuron.Radius = factor * Constant.Radius;
            neuron.draw(graphics);
        }

        public static Graphics Graphics
        {
            set
            {
                graphics = value;
            }
        }

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
    }
}
