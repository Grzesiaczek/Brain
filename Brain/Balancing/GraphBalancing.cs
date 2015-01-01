using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class GraphBalancing
    {
        List<BalancedNeuron> neurons;
        List<BalancedReceptor> receptors;
        List<AnimatedSynapse> synapses;

        System.Windows.Forms.Timer timer;
        Graphics graphics;

        float alpha;
        float beta;
        float delta;
        float step;

        int steps;
        bool action;

        static GraphBalancing instance = new GraphBalancing();

        private GraphBalancing()
        {
            alpha = -0.2f;
            beta = 2.0f;
            step = 0.25f;

            timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(tick);
            timer.Interval = 25;
        }

        public static GraphBalancing getInstance()
        {
            return instance;
        }

        public void animate(List<AnimatedNeuron> neurons, List<AnimatedSynapse> synapses, List<AnimatedReceptor> receptors, Graphics g, int steps)
        {
            if (action)
                timer.Stop();

            this.neurons = new List<BalancedNeuron>();
            this.receptors = new List<BalancedReceptor>();

            foreach (AnimatedNeuron neuron in neurons)
                this.neurons.Add(new BalancedNeuron(neuron));

            foreach (AnimatedReceptor receptor in receptors)
                this.receptors.Add(new BalancedReceptor(receptor));

            this.synapses = synapses;
            this.steps = steps;
            graphics = g;

            timer.Start();
        }

        public void balance(List<AnimatedNeuron> neurons, List<AnimatedSynapse> synapses, List<AnimatedReceptor> receptors, Graphics g)
        {
            this.neurons = new List<BalancedNeuron>();
            this.receptors = new List<BalancedReceptor>();

            foreach (AnimatedNeuron neuron in neurons)
                this.neurons.Add(new BalancedNeuron(neuron));

            foreach (AnimatedReceptor receptor in receptors)
                this.receptors.Add(new BalancedReceptor(receptor));

            this.synapses = synapses;
            graphics = g;

            while(true)
            {
                delta = 0;
                calculate();

                if (delta < 0.1)
                    break;
            }

            foreach (AnimatedSynapse s in synapses)
                s.recalculate();
        }

        void tick(object sender, EventArgs e)
        {
            for (int i = 0; i < steps; i++)
                calculate();

            if (Math.Abs(delta) < 0.05)
            {
                timer.Stop();
                action = false;
                balanceFinished(this, new EventArgs());
            }

            foreach(BalancedNeuron n in neurons)
                n.draw(1);

            foreach (AnimatedSynapse s in synapses)
                s.recalculate();
        }

        void calculate()
        {
            delta = 0;

            foreach (BalancedNeuron n1 in neurons)
            {
                n1.zero();
                n1.repulse(graphics.VisibleClipBounds.Size, beta / (2 * neurons.Count));

                foreach (BalancedNeuron n2 in neurons)
                {
                    if (n1 == n2)
                        continue;

                    n1.repulse(n2.getPosition(), beta);
                }
                
                foreach(BalancedReceptor r in receptors)
                    n1.repulse(r.getPosition(), beta / 2);

                foreach (AnimatedSynapse s in n1.getNeuron().Output)
                    n1.attract(s.Post, alpha + alpha * s.getWeight());

                foreach (AnimatedSynapse s in n1.getNeuron().Input)
                    n1.attract(s.Pre, alpha + alpha * s.getWeight());
            }

            foreach (BalancedReceptor r1 in receptors)
            {
                r1.zero();
                r1.attract(alpha);

                foreach (BalancedReceptor r2 in receptors)
                {
                    if (r1 == r2)
                        continue;

                    r1.repulse(r2, beta);
                }
            }

            foreach (BalancedNeuron neuron in neurons)
                delta += neuron.update(step);

            foreach (BalancedReceptor r in receptors)
                delta += r.update(step);
        }

        public void stop()
        {
            timer.Stop();
            action = false;
        }

        public event EventHandler balanceFinished;
    }
}