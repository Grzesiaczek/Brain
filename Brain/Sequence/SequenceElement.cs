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
        #region deklaracje

        Rectangle rect;
        Color background;
        Brush fontColor;
        Pen border;

        protected Font font;
        protected String name;
        protected StringFormat format;

        #endregion

        public SequenceElement(String name)
        {
            Width = 16 + 8 * name.Length;
            Height = 32;

            this.name = name;
            rect = new Rectangle(0, 0, Width, Height);

            Visible = true;
            initializeGraphics();

            fontColor = Brushes.Indigo;
            font = new Font("Calibri", 12, FontStyle.Bold);
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
                    background = Color.LightSkyBlue;
                    border = Pens.Thistle;
                    break;

                case SequenceElementType.Built:
                    background = Color.GreenYellow;
                    border = Pens.Purple;
                    break;

                case SequenceElementType.Normal:
                    background = Color.GreenYellow;
                    border = Pens.Thistle;
                    break;

                case SequenceElementType.Receptor:
                    background = Color.LightCyan;
                    fontColor = Brushes.DarkSlateGray;
                    border = Pens.Purple;
                    break;

                case SequenceElementType.ActiveReceptor:
                    background = Color.PaleVioletRed;
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
