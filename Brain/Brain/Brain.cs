using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Office.Interop.Excel;

namespace Brain
{
    class Brain
    {
        #region deklaracje

        List<Neuron> neurons;  
        List<Synapse> synapses;
        List<Receptor> receptors;

        float alpha;
        float beta;

        int sentences;
        int length;

        #endregion

        public Brain()
        {
            neurons = new List<Neuron>();
            synapses = new List<Synapse>();
            receptors = new List<Receptor>();

            alpha = Constant.Alpha;
            beta = Constant.Beta;

            sentences = 0;
            length = 0;
        }

        #region sterowanie

        public void simulate(int length)
        {
            for (int i = 0; i < length; i++)
            {
                foreach (Receptor r in receptors)
                    r.tick();

                foreach (Neuron n in neurons)
                    n.tick();

                foreach (Synapse s in synapses)
                    s.tick();
            }

            foreach (Neuron n in neurons)
                n.tick();

            this.length = length;
        }

        public void tick()
        {
            foreach (Receptor r in receptors)
                r.tick(false);

            foreach (Neuron n in neurons)
                n.tick();

            foreach (Synapse s in synapses)
                s.tick();

            length++;
        }

        public void undo()
        {
            foreach (Receptor r in receptors)
                r.undo();

            foreach (Neuron n in neurons)
                n.undo();

            foreach (Synapse s in synapses)
                s.undo();

            length--;
        }

        public void clear(bool manual)
        {
            foreach (Neuron n in neurons)
                n.clear(manual);

            foreach (Synapse s in synapses)
                s.clear(manual);

            foreach (Receptor r in receptors)
                r.clear();

            length = 0;
        }

        #endregion

        #region uczenie

        public CreationSequence addSentence(String sentence)
        {
            List<CreationFrame> frames = new List<CreationFrame>();
            List<Neuron> sequence = new List<Neuron>();
            String[] words = sentence.Split(' ');

            foreach (String word in words)
            {
                CreationFrame frame = create(word, ++sentences);
                sequence.Add(frame.Neuron.Neuron);
                frames.Add(frame);
            }

            for (int i = 0; i < sequence.Count; i++)
            {
                for (int j = i + 1; j < sequence.Count; j++)
                {
                    Synapse synapse = sequence[i].Output.Find(k => k.Post.Equals(sequence[j]));

                    if (synapse == null)
                    {
                        synapse = new Synapse(sequence[i], sequence[j]);
                        synapses.Add(synapse);
                        sequence[i].Output.Add(synapse);
                        sequence[j].Input.Add(synapse);
                        frames[j].add(synapse);
                    }

                    synapse.Factor += 1 / (float)(j - i);
                }

                foreach (Synapse synapse in sequence[i].Input)
                {
                    float weight = synapse.Weight;
                    synapse.Weight = (2 * synapse.Factor) / (sequence[i].Count + synapse.Factor);

                    if (weight != synapse.Weight)
                        frames[i].add(new CreationData(synapse, frames[i], weight, synapse.Weight));
                }
            }

            return new CreationSequence(frames);
        }

        CreationFrame create(String word, int frame)
        {
            Neuron neuron = neurons.Find(i => i.Word == word);

            if (neuron == null)
            {
                neuron = new Neuron(word, alpha, beta);
                neurons.Add(neuron);

                Receptor receptor = new Receptor();
                Synapse synapse = new Synapse(receptor, neuron);

                synapses.Add(synapse);
                receptors.Add(receptor);
                receptor.Output = synapse;
            }

            neuron.Count++;
            return new CreationFrame(neuron, frame);
        }

        public CreationFrame add(BuiltSequence sequence, BuiltElement element, int frame)
        {
            CreationFrame result = create(element.Name, frame);

            return result;
        }

        public void remove(CreationFrame frame)
        {

        }

        #endregion

        #region właściwości

        public List<Neuron> Neurons
        {
            get
            {
                return neurons;
            }
        }

        public List<Synapse> Synapses
        {
            get
            {
                return synapses;
            }
        }

        public List<Receptor> Receptors
        {
            get
            {
                return receptors;
            }
        }

        public int Length
        {
            get
            {
                return length;
            }
        }

        #endregion
    }
}