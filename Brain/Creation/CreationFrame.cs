using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class CreationFrame
    {
        Neuron neuron;
        List<Synapse> created;
        List<CreationData> data;

        static Dictionary<Neuron, CreatedNeuron> neurons;
        static Dictionary<Synapse, CreatedSynapse> synapses;

        int count;
        int phase;
        static int interval;

        public CreationFrame(Neuron neuron)
        {
            this.neuron = neuron;
            created = new List<Synapse>();
            data = new List<CreationData>();

            count = 0;
            phase = 1;
        }

        public static void setDictionary(Dictionary<Neuron, CreatedNeuron> mapNeurons, Dictionary<Synapse, CreatedSynapse> mapSynapses)
        {
            neurons = mapNeurons;
            synapses = mapSynapses;
        }

        public void create(List<CreatedNeuron> lcn, List<CreatedSynapse> lcs)
        {
            if (!neurons[neuron].Created)
            {
                CreatedNeuron cn = neurons[neuron];
                cn.create();
                lcn.Add(cn);
            }

            foreach (Synapse s in created)
            {
                CreatedSynapse cs = synapses[s];
                lcs.Add(cs);
            }
                
            foreach (CreationData cd in data)
                synapses[cd.Synapse].Synapse.create(cd);
        }

        public void tick()
        {
            float factor = (float)count / interval;

            switch (phase)
            {
                case 1:
                    if (neurons[neuron].Created)
                        phase = 2;
                    else
                        neurons[neuron].draw(factor);

                    break;
                case 2:
                    if (created.Count == 0)
                        phase = 3;

                    foreach (Synapse s in created)
                        synapses[s].draw(factor);

                    break;
                case 3:
                    foreach (CreationData cd in data)
                    {
                        CreatedSynapse cs = synapses[cd.Synapse];
                        cs.tick(cd);
                    }
                    break;
            }

            if(++count == interval)
            {
                switch(phase++)
                {
                    case 1:
                        if (synapses.Count == 0)
                            phase++;

                        neurons[neuron].Created = true;
                        finish(neurons[neuron], null);
                        break;
                    case 2:
                        finish(created, null);
                        break;
                    case 3:
                        foreach (CreationData cd in data)
                            synapses[cd.Synapse].Synapse.create(cd);

                        finish(this, null);
                        break;
                }

                count = 0;
            }
        }

        public static void setInterval(int value)
        {
            interval = value;
        }

        public void step()
        {
            foreach (CreationData cd in data)
                cd.Step = (cd.Weight - cd.Start) / interval;
        }

        public void add(CreationData data)
        {
            this.data.Add(data);
        }

        public void add(Synapse synapse)
        {
            created.Add(synapse);
        }

        public event EventHandler finish;
    }
}
