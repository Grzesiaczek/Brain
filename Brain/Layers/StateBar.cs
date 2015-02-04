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
        int state;
        bool stopped;
        bool running;

        public StateBar()
        {
            InitializeComponent();

            BackColor = System.Drawing.SystemColors.InactiveBorder;
            Location = new System.Drawing.Point(20, 240);
            Name = "stateBar";
            Size = new System.Drawing.Size(23, 120);
            TabIndex = 32;
            Visible = false;
        }

        //sterowanie
        #region
        public void run()
        {
            state = Height;
            running = true;
        }

        public void pause()
        {
            running = false;
            stopped = true;
        }

        public void next()
        {
            stopped = false;
        }
        #endregion

        void redraw()
        {
            Graphics g = buffer.Graphics;
            g.Clear(SystemColors.Control);

            Brush brush = Brushes.Green;
            Rectangle rect = new Rectangle(0, 0, Width, Height);

            if(running)
            {
                brush = Brushes.IndianRed;
                rect = new Rectangle(0, Height - state, Width, state);
            }
            else if (stopped)
                brush = Brushes.Orange;

            g.DrawRectangle(Pens.Purple, rect);
            g.FillRectangle(brush, rect);
            buffer.Render(graphics);
        }

        public override void resize()
        {
            initializeGraphics();
        }

        protected override void tick(object sender, EventArgs e)
        {
            if (running)
            {
                state -= 3;

                if (state <= 0)
                {
                    finished(this, null);
                    running = false;
                }
            }

            redraw();
        }

        public event EventHandler finished;
    }
}
