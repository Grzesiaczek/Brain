using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brain
{
    partial class StateBar : Layer
    {
        StateBarPhase phase;
        int state;

        public StateBar()
        {
            InitializeComponent();

            BackColor = System.Drawing.SystemColors.InactiveBorder;
            Location = new System.Drawing.Point(20, 30);
            Size = new System.Drawing.Size(20, 120);
            TabIndex = 32;
            Visible = false;

            Name = "stateBar";
            phase = StateBarPhase.Idle;
        }

        public void reset()
        {
            phase = StateBarPhase.Idle;
            state = Height;
        }

        public override void resize()
        {
            initializeGraphics();
        }

        protected override void tick(object sender, EventArgs e)
        {
            Graphics g = buffer.Graphics;
            g.Clear(SystemColors.Control);

            Brush brush = Brushes.Green;
            Rectangle rect = new Rectangle(0, Height - state, Width, state);

            switch (phase)
            {
                case StateBarPhase.Activation:
                    brush = Brushes.IndianRed;
                    break;

                case StateBarPhase.BalanceNormal:
                    brush = Brushes.DarkSlateBlue;
                    break;

                case StateBarPhase.BalanceExtra:
                    brush = Brushes.DarkSlateGray;
                    break;

                case StateBarPhase.Idle:
                    rect = new Rectangle(0, 0, Width, Height);
                    break;
            }

            g.DrawRectangle(Pens.Purple, rect);
            g.FillRectangle(brush, rect);
            buffer.Render(graphics);
        }

        #region właściwości

        public StateBarPhase Phase
        {
            get
            {
                return phase;
            }
            set
            {
                phase = value;
            }
        }

        public int State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }

        #endregion
    }
}
