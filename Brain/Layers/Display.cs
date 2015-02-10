using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class Display : Layer
    {
        #region deklaracje

        Animation animation;
        Creation creation;

        Sequence sequence;
        Sequence background;

        #endregion

        public Display()
        {
            background = new Sequence();
            clear();
        }

        #region sterowanie

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
            show(background);
        }

        #endregion

        #region dostęp do animation

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

        public Dictionary<object, object> loadFrame(CreationFrame frame, int index)
        {
            return animation.loadFrame(frame, index);
        }

        #endregion

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
