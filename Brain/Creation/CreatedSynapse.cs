using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class CreatedSynapse
    {
        AnimatedSynapse synapse;

        public CreatedSynapse(AnimatedSynapse synapse)
        {
            this.synapse = synapse;
        }

        public void draw()
        {
            synapse.draw();
        }

        public void draw(float factor)
        {
            synapse.draw(factor);
        }

        public void drawState()
        {
            synapse.drawState();
        }

        public void tick(CreationData cd)
        {
            if (synapse.Synapse == cd.Synapse)
                synapse.getState(false).Change += cd.Step;
            else
                synapse.getState(true).Change += cd.Step;
        }

        #region właściwości

        public AnimatedSynapse Synapse
        {
            get
            {
                return synapse;
            }
        }

        #endregion;
    }
}