using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brain
{
    class CreationHistory
    {
        GroupBox layer;
        Graphics graphics;

        BufferedGraphics buffer;
        BufferedGraphicsContext context;

        SynapseState synapse;
        System.Windows.Forms.Timer timer;

        public CreationHistory(GroupBox groupBox, SynapseState active)
        {
            layer = groupBox;
            synapse = active;

            layer.Visible = true;
            initializeGraphics();

            timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(tick);
            timer.Interval = 25;
            timer.Start();
        }

        void initializeGraphics()
        {
            layer.Height = synapse.History.Count * 36 + 40;
            layer.Width = 160;

            int x = layer.Width;
            int y = layer.Height;

            if (layer.Parent.Width - layer.Location.X < layer.Width)
                x = layer.Location.X - layer.Width;

            if (layer.Parent.Height - layer.Location.Y < layer.Height)
                y = layer.Location.Y - layer.Height;

            if (x != layer.Width || y != layer.Height)
                layer.Location = new Point(x, y);

            graphics = layer.CreateGraphics();
            graphics.FillRectangle(SystemBrushes.Control, graphics.VisibleClipBounds);

            context = BufferedGraphicsManager.Current;
            context.MaximumBuffer = new Size(layer.Width + 1, layer.Height + 1);

            buffer = context.Allocate(graphics, new Rectangle(0, 0, layer.Width, layer.Height));
            buffer.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        }

        public void show()
        {
            initializeGraphics();
            layer.Visible = true;
            timer.Start();
        }

        public void draw()
        {
            Graphics g = buffer.Graphics;
            g.Clear(SystemColors.Control);
            g.DrawRectangle(Pens.CadetBlue, 0, 0, layer.Width - 1, layer.Height - 1);

            for (int i = Math.Max(0, synapse.History.Count - 4); i < synapse.History.Count; i++)
                synapse.History[i].draw(g, i);

            buffer.Render(graphics);
        }

        void tick(object sender, EventArgs e)
        {
            draw();
        }

        public void hide()
        {
            layer.Visible = false;
            timer.Stop();
        }
    }
}
