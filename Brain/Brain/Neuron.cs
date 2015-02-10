using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class Neuron : Element
    {
        #region deklaracje

        List<Receptor> sensin;
        List<Synapse> input;
        List<Synapse> output;
        List<NeuronData> activity;

        String word;

        int id;
        int count;

        bool active;

        float alpha;
        float beta;

        double impulse;
        double treshold;
        double original;
        double value;

        #endregion

        #region konstruktory

        public Neuron(String word, float alpha, float beta)
        {
            this.word = word;
            this.alpha = alpha;
            this.beta = beta;

            sensin = new List<Receptor>();
            input = new List<Synapse>();
            output = new List<Synapse>();
            activity = new List<NeuronData>();
            initiate();
        }

        void initiate()
        {
            treshold = 1.0;
            impulse = 0;
        }

        #endregion

        #region sterowanie

        public void tick()
        {
            double initial = value;
            double relaxation = 0;

            if (active)
            {
                impulse -= beta * treshold + value;
                value += impulse;
            }

            relaxation = (alpha - 1) * value;

            if (value < 0)
                relaxation += ((1 - alpha) * beta * (value * value)) / treshold;
            else
                relaxation += ((alpha - 1) * beta * (value * value)) / treshold;

            if (!active)
                value += impulse;
            
            value += relaxation;

            if (value < treshold)
                activity.Add(new NeuronData(false, initial, impulse, relaxation, value));
            else
                activity.Add(new NeuronData(true, initial, impulse, relaxation, value));

            if (value < treshold)
                active = false;
            else
                active = true;

            original = value;
            impulse = 0;
        }

        public void activate()
        {
            if (original >= 1)
                return;

            if(value < 1)
            {
                value = 1;
                activity[activity.Count - 1].Active = true;
                activity[activity.Count - 1].Value = value;
                active = true;

                foreach (Synapse s in output)
                    s.refresh(true);
            }
            else
            {
                value = original;
                activity[activity.Count - 1].Active = false;
                activity[activity.Count - 1].Value = value;
                active = false;

                foreach (Synapse s in output)
                    s.refresh(false);
            }
        }

        public void undo()
        {
            if (activity.Count > 1)
                activity.RemoveAt(activity.Count - 1);

            impulse = 0;
            active = activity[activity.Count - 1].Active;
            original = activity[activity.Count - 1].Original;
            value = activity[activity.Count - 1].Value;
        }

        public void clear(bool init)
        {
            activity.Clear();
            active = false;
            original = 0;
            value = 0;

            if(init)
                activity.Add(new NeuronData());
        }

        public void receiveSignal(double impulse)
        {
            this.impulse += impulse;
        }

        #endregion

        #region właściwości

        public List<Synapse> Input
        {
            get
            {
                return input;
            }
            set
            {
                input = value;
            }
        }

        public List<Synapse> Output
        {
            get
            {
                return output;
            }
            set
            {
                output = value;
            }
        }

        public List<NeuronData> Activity
        {
            get
            {
                return activity;
            }
            set
            {
                activity = value;
            }
        }

        public string Word
        {
            get
            {
                return word;
            }
            set
            {
                word = value;
            }
        }

        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
            }
        }

        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
            }
        }

        public String Name
        {
            get
            {
                return word;
            }
        }

        #endregion
    }
}