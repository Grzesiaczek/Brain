using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brain
{
    class GraphBalancing
    {
        #region deklaracje

        List<BalancedNeuron> neurons;
        List<BalancedReceptor> receptors;
        List<BalancedSynapse> synapses;

        System.Windows.Forms.Timer timer;
        Dictionary<AnimatedElement, BalancedElement> map;

        float alpha;
        float beta;
        float delta;
        float step;

        int interval;
        int steps;

        bool extra;
        bool action;

        static GraphBalancing instance = new GraphBalancing();

        public event EventHandler balanceFinished;

        #endregion

        private GraphBalancing()
        {
            alpha = -0.2f;
            beta = 2.0f;
            step = 0.2f;

            timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(tick);
            timer.Interval = 25;
        }

        public static GraphBalancing getInstance()
        {
            return instance;
        }

        public void animate(List<AnimatedNeuron> neurons, List<AnimatedSynapse> synapses, List<AnimatedReceptor> receptors, int steps)
        {
            if (action)
                timer.Stop();

            initialize(neurons, synapses, receptors);

            this.steps = steps;
            interval = 0;
            timer.Start();
        }

        public void balance(List<AnimatedNeuron> neurons, List<AnimatedSynapse> synapses, List<AnimatedReceptor> receptors)
        {
            initialize(neurons, synapses, receptors);

            while(true)
            {
                calculate();
                update();

                if (Math.Abs(delta) < 0.1)
                    break;
            }

            extra = true;

            while (true)
            {
                calculate();
                update();

                foreach (AnimatedSynapse s in synapses)
                    s.changePosition();

                if (Math.Abs(delta) < 0.1)
                    break;
            }
        }

        void initialize(List<AnimatedNeuron> neurons, List<AnimatedSynapse> synapses, List<AnimatedReceptor> receptors)
        {
            this.neurons = new List<BalancedNeuron>();
            this.receptors = new List<BalancedReceptor>();
            this.synapses = new List<BalancedSynapse>();
            map = new Dictionary<AnimatedElement, BalancedElement>();

            foreach (AnimatedNeuron an in neurons)
            {
                BalancedNeuron neuron = new BalancedNeuron(an);
                this.neurons.Add(neuron);
                map.Add(an, neuron);
            }

            foreach (AnimatedReceptor ar in receptors)
            {
                BalancedReceptor receptor = new BalancedReceptor(ar);
                this.receptors.Add(receptor);
                map.Add(ar, receptor);
            }

            foreach (AnimatedSynapse synapse in synapses)
                this.synapses.Add(new BalancedSynapse(synapse, map));

            extra = false;
        }

        void tick(object sender, EventArgs e)
        {
            if (interval < steps)
                interval += 1;

            for (int i = 0; i < interval; i++)
            {
                calculate();
                update();
            }

            if (Math.Abs(delta) < 0.1)
            {
                if (extra)
                {
                    timer.Stop();
                    action = false;
                    balanceFinished(this, new EventArgs());
                }
                else
                {
                    interval = 0;
                    steps /= 2;
                    extra = true;
                }
            }

            foreach (BalancedSynapse bs in synapses)
                bs.Synapse.changePosition();
        }

        void calculate()
        {
            foreach (BalancedNeuron n1 in neurons)
            {
                n1.repulse(beta / (2 * neurons.Count));
                n1.rotate();

                foreach (BalancedNeuron n2 in neurons)
                {
                    if (n1 == n2)
                        continue;

                    n1.repulse(n2.Position, beta);
                }
                
                foreach(BalancedReceptor r in receptors)
                    n1.repulse(r.Position, beta / 2);

                foreach (AnimatedSynapse s in n1.Neuron.Output)
                    n1.attract(s.Post, 2 * alpha);

                foreach (AnimatedSynapse s in n1.Neuron.Input)
                    n1.attract(s.Pre, 3 * alpha);
            }

            foreach (BalancedReceptor r1 in receptors)
            {
                r1.attract(5 * alpha);

                foreach (BalancedReceptor r2 in receptors)
                {
                    if (r1 == r2)
                        continue;

                    r1.repulse(r2, beta);
                }
            }

            foreach (BalancedSynapse bs in synapses)
                bs.rotate();

            if(extra)
                foreach (BalancedSynapse bs in synapses)
                    foreach (BalancedNeuron bn in neurons) bs.repulse(bn, beta);
        }

        void update()
        {
            delta = 0;

            foreach (BalancedNeuron bn in neurons)
                delta += bn.update(step);

            foreach (BalancedReceptor br in receptors)
                delta += br.update(step);
        }

        public void stop()
        {
            timer.Stop();
            action = false;
        }
    }
}