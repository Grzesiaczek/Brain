using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brain
{
    abstract class SequenceElement
    {
        #region deklaracje

        static Graphics graphics;

        Brush background;
        Brush fontColor;
        Pen border;

        protected Font font;
        protected String name;
        protected StringFormat format;
        protected Rectangle rect;

        protected int width;
        protected int height;
        int code = 0;

        #endregion

        public SequenceElement(String name)
        {
            width = 16 + (int)(8.5 * name.Length);
            height = 32;

            this.name = name;
            rect = new Rectangle(0, 0, width, height);

            fontColor = Brushes.Indigo;
            font = new Font("Calibri", 12, FontStyle.Bold);
            format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            Random random = new Random();
            code = random.Next(1000);
        }

        #region grafika

        public void changeType(SequenceElementType type)
        {
            switch(type)
            {
                case SequenceElementType.Activated:

                    break;

                case SequenceElementType.Active:
                    background = Brushes.LightSkyBlue;
                    border = Pens.IndianRed;
                    break;

                case SequenceElementType.Built:
                    background = Brushes.GreenYellow;
                    border = Pens.Purple;
                    break;

                case SequenceElementType.Normal:
                    background = Brushes.LightYellow;
                    border = Pens.Thistle;
                    break;

                case SequenceElementType.Receptor:
                    background = Brushes.LightCyan;
                    fontColor = Brushes.DarkSlateGray;
                    border = Pens.Purple;
                    break;

                case SequenceElementType.ActiveReceptor:
                    background = Brushes.PaleVioletRed;
                    fontColor = Brushes.DarkSlateBlue;
                    border = Pens.Purple;
                    break;
            }
        }

        public virtual void draw()
        {
            graphics.FillRectangle(background, rect);
            graphics.DrawRectangle(border, rect);
            graphics.DrawString(name, font, fontColor, rect, format);
        }

        #endregion

        #region właściwości

        public static Graphics Graphics
        {
            set
            {
                graphics = value;
            }
        }

        public String Name
        {
            get
            {
                return name;
            }
        }

        public int Left
        {
            get
            {
                return rect.X;
            }
            set
            {
                rect.X = value;
            }
        }

        public int Top
        {
            set
            {
                rect.Y = value;
            }
        }

        public int Right
        {
            get
            {
                return rect.Right;
            }
        }

        #endregion
    }
}
