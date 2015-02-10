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

        List<CreationFrame> frames;

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

            frames = new List<CreationFrame>();
            frames.Add(null);

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

            int index = 0;

            foreach (String word in words)
            {
                CreationFrame frame = create(word, ++sentences);
                Neuron neuron = frame.Neuron.Neuron;

                for (int i = 0; i < index; i++)
                {
                    Synapse synapse = neuron.Input.Find(k => k.Pre.Equals(sequence[i]));

                    if (synapse == null)
                    {
                        synapse = new Synapse(sequence[i], neuron);
                        synapses.Add(synapse);

                        neuron.Input.Add(synapse);
                        sequence[i].Output.Add(synapse);
                        frame.add(synapse);
                    }

                    synapse.Change += 1 / (float)(index - i);
                }

                foreach (Synapse synapse in neuron.Input)
                {
                    if (synapse.Change == 0)
                        continue;

                    float weight = synapse.Weight;
                    synapse.Factor += synapse.Change;
                    synapse.Weight = (2 * synapse.Factor) / (neuron.Count + synapse.Factor);

                    CreationData data = new CreationData(synapse, frame, synapse.Change, weight, synapse.Weight);
                    synapse.Changes.Add(data);
                    frame.add(data);

                    synapse.Change = 0;
                }

                sequence.Add(neuron);
                frames.Add(frame);
                this.frames.Add(frame);

                index++;
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

        public CreationFrame add(CreationSequence sequence, BuiltElement element, int frame)
        {
            CreationFrame result = create(element.Name, frame);
            Neuron neuron = result.Neuron.Neuron;
            int index = sequence.Frames.Count;

            for (int i = 0; i < index; i++)
            {
                Neuron previous = sequence.Frames[i].Neuron.Neuron;
                Synapse synapse = neuron.Input.Find(k => k.Pre.Equals(previous));

                if (synapse == null)
                {
                    synapse = new Synapse(previous, neuron);
                    synapses.Add(synapse);

                    neuron.Input.Add(synapse);
                    previous.Output.Add(synapse);
                    result.add(synapse);
                }

                synapse.Change += 1 / (float)(index - i);
            }

            foreach (Synapse synapse in neuron.Input)
            {
                if (synapse.Change == 0)
                    continue;

                int count = 0;
                float start = 0;
                float weight = 0;
                
                List<CreationData> changes = synapse.Changes;
                synapse.Factor = 0;

                foreach(CreationData cd in changes)
                {
                    if (cd.Frame > frame)
                        break;

                    count++;
                    start = cd.Weight;
                    synapse.Factor += cd.Change;
                }

                synapse.Factor += synapse.Change;
                weight = (2 * synapse.Factor) / (synapse.Factor + count + 1);

                CreationData data = new CreationData(synapse, result, synapse.Change, start, weight);
                
                changes.Insert(count, data);
                result.add(data);
                start = weight;

                for(int i = count; i < changes.Count; i++)
                {
                    synapse.Factor += changes[i].Change;
                    weight = (2 * synapse.Factor) / (synapse.Factor + i + 1);

                    changes[i].Start = start;
                    changes[i].Weight = weight;

                    start = synapse.Weight;
                }

                synapse.Weight = weight;
                synapse.Change = 0;
            }

            for (int i = frame; i < frames.Count; i++)
                frames[i].Frame = i;

            frames.Insert(result.Frame, result);
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