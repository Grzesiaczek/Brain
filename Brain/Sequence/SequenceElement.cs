using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brain
{
    abstract class SequenceElement : Layer
    {
        static Rectangle rect = new Rectangle(0, 0, 80, 32);

        Color background;
        Brush fontColor;
        Pen border;

        protected Font font;
        protected String name;
        protected StringFormat format;

        public SequenceElement()
        {
            Width = rect.Width;
            Height = rect.Height;

            Visible = true;
            initializeGraphics();

            fontColor = Brushes.Indigo;
            font = new Font("Arial", 12, FontStyle.Bold);
            format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
        }

        public void changeType(SequenceElementType type)
        {
            switch(type)
            {
                case SequenceElementType.Activated:

                    break;

                case SequenceElementType.Active:
                    background = Pens.LightSkyBlue.Color;
                    border = Pens.Thistle;
                    break;

                case SequenceElementType.Normal:
                    background = Pens.GreenYellow.Color;
                    border = Pens.Thistle;
                    break;

                case SequenceElementType.Receptor:
                    background = Pens.LightCyan.Color;
                    fontColor = Brushes.DarkSlateGray;
                    border = Pens.Purple;
                    break;

                case SequenceElementType.ActiveReceptor:
                    background = Pens.PaleVioletRed.Color;
                    fontColor = Brushes.DarkSlateBlue;
                    border = Pens.Purple;
                    break;
            }
        }

        public virtual void draw()
        {
            Graphics g = buffer.Graphics;
            g.Clear(background);
            g.DrawRectangle(border, rect);
            g.DrawString(name, font, fontColor, rect, format);
            buffer.Render(graphics);
        }

        protected override void tick(object sender, EventArgs e){ }
    }
}
