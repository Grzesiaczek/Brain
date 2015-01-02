using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brain
{
    class CreationHistory : Control
    {
        Graphics graphics;

        BufferedGraphics buffer;
        BufferedGraphicsContext context;

        SynapseState synapse;
        System.Windows.Forms.Timer timer;

        public CreationHistory(Control parent, SynapseState active)
        {
            parent.Controls.Add(this);
            synapse = active;

            Visible = true;
            initializeGraphics();

            timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(tick);
            timer.Interval = 25;
            timer.Start();
        }

        void initializeGraphics()
        {
            Height = Math.Max(synapse.History.Count, 4) * 36 + 40;
            Width = 160;

            int x = Width;
            int y = Height;

            if (Parent.Width - Location.X < Width)
                x = Location.X - Width;

            if (Parent.Height - Location.Y < Height)
                y = Location.Y - Height;

            if (x != Width || y != Height)
                Location = new Point(x, y);

            graphics = CreateGraphics();
            graphics.FillRectangle(SystemBrushes.Control, graphics.VisibleClipBounds);

            context = BufferedGraphicsManager.Current;
            context.MaximumBuffer = new Size(Width + 1, Height + 1);

            buffer = context.Allocate(graphics, new Rectangle(0, 0, Width, Height));
            buffer.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        }

        public void show()
        {
            initializeGraphics();
            Visible = true;
            timer.Start();
        }

        public void draw()
        {
            Graphics g = buffer.Graphics;
            g.Clear(SystemColors.Control);
            g.DrawRectangle(Pens.CadetBlue, 0, 0, Width - 1, Height - 1);

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            String text = synapse.Synapse.Pre.Name.ToString() + " -> " + synapse.Synapse.Post.Name.ToString();
            g.DrawString(text, new Font("Verdana", 12, FontStyle.Bold), Brushes.Black, 80, 20, format);

            for (int i = Math.Max(0, synapse.History.Count - 4), j = 0; i < synapse.History.Count; i++, j++)
                synapse.History[i].draw(g, j);

            buffer.Render(graphics);
        }

        void tick(object sender, EventArgs e)
        {
            draw();
        }

        public void hide()
        {
            Visible = false;
            timer.Stop();
        }
    }
}
