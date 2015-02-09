using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class Display : Layer
    {
        Animation animation;
        Creation creation;

        Sequence sequence;
        Sequence background;

        public Display()
        {
            background = new Sequence();
            sequence = background;
        }

        public void add(Animation animation)
        {
            this.animation = animation;
            Controls.Add(animation);
        }

        public void add(Creation creation)
        {
            this.creation = creation;
            Controls.Add(creation);
        }

        public void show(Sequence seq)
        {
            if (sequence != null)
                Controls.Remove(sequence);

            sequence = seq;
            Controls.Add(seq);
            seq.show();
        }

        public void clear()
        {
            Controls.Remove(sequence);
            Controls.Add(background);

            sequence = background;
            background.show();
        }

        public void balance()
        {
            animation.balance();
        }

        public void calculateShift(float factor)
        {
            animation.calculateShift(factor);
        }

        public void executeShift()
        {
            animation.executeShift();
        }

        public void changeSize(float factor)
        {
            animation.changeSize(factor);
        }

        public override void resize()
        {
            base.resize();
            sequence.resize();

            if (animation.Visible)
                animation.resize();
            
            if(creation.Visible)
                creation.resize();
        }
    }
}
