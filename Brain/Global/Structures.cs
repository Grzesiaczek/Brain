using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Brain
{
    #region enumeratory

    enum Mode { Chart, Creation, Manual, Query }
    enum SequenceElementType { Normal, Built, Active, Activated, Receptor, ActiveReceptor}
    enum StateBarPhase { Idle, Activation, BalanceNormal, BalanceExtra}

    #endregion

    class NeuronData
    {
        #region deklaracje

        bool active;
        double initial;
        double impulse;
        double relaxation;
        double original;
        double value;

        #endregion

        #region konstruktory

        public NeuronData(bool active, double initial, double impulse, double relaxation, double value)
        {
            this.active = active;
            this.initial = initial;
            this.impulse = impulse;
            this.relaxation = relaxation;
            this.value = value;
            original = value;
        }

        public NeuronData()
        {
            active = false;
            initial = 0;
            impulse = 0;
            relaxation = 0;
            value = 0;
        }

        #endregion

        #region właściwości

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

        public double Initial
        {
            get
            {
                return initial;
            }
            set
            {
                initial = value;
            }
        }

        public double Impulse
        {
            get
            {
                return impulse;
            }
            set
            {
                impulse = value;
            }
        }

        public double Relaxation
        {
            get
            {
                return relaxation;
            }
            set
            {
                relaxation = value;
            }
        }

        public double Original
        {
            get
            {
                return original;
            }
            set
            {
                original = value;
            }
        }

        public double Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        #endregion
    }
}