using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brain
{
    class CreationHistory : Layer
    {
        AnimatedState synapse;

        public CreationHistory(AnimatedState active)
        {
            synapse = active;

            int x = (int)(active.State.X);
            int y = (int)(active.State.Y);
            Location = new Point(x, y);
        }

        protected override void initializeGraphics()
        {
            Height = Math.Min(synapse.History.Count, 4) * 36 + 40;
            Width = 164;

            int x = Location.X;
            int y = Location.Y;

            if (Parent.Width - Location.X < Width)
                x = Location.X - Width;

            if (Parent.Height - Location.Y < Height)
                y = Location.Y - Height;

            if (x != Location.X || y != Location.Y)
                Location = new Point(x, y);

            base.initializeGraphics();
        }

        public override void show()
        {
            base.show();
            Controls.Clear();

            for (int i = Math.Max(0, synapse.History.Count - 4), j = 0; i < synapse.History.Count; i++, j++)
            {
                CreationData cd = synapse.History[i];
                Controls.Add(cd);
                cd.Location = new Point(2, j * 36 + 40);
                cd.show();
            }
        }

        public void redraw()
        {
            Graphics g = buffer.Graphics;
            g.Clear(SystemColors.Control);
            g.DrawRectangle(Pens.CadetBlue, 0, 0, Width - 1, Height - 1);

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            String text = synapse.Synapse.Pre.Name.ToString() + " -> " + synapse.Synapse.Post.Name.ToString();
            g.DrawString(text, new Font("Verdana", 10, FontStyle.Bold), Brushes.Black, 82, 20, format);

            buffer.Render(graphics);
        }

        protected override void tick(object sender, EventArgs e)
        {
            redraw();
        }

        public override void hide()
        {
            base.hide();

            foreach (Control control in Controls)
                ((CreationData)control).hide();
        }
    }
}
