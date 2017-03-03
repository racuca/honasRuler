using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace HonasRuler
{
    public partial class RulerCtrl : UserControl
    {

        float startX;
        float startY;

        Pen linepen = Pens.Black;

        Size ctrlSize;

        const float CommonMargin = 2;

        float leftMargin = 0 + CommonMargin;
        float topMargin = 0 + CommonMargin;
        float rightMargin = 0 + CommonMargin;
        float bottomMargin = 0 + CommonMargin;

        float halfLineHeight = 20;
        float normalLineHeight = 10;

        public RulerCtrl()
        {
            InitializeComponent();
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Color.Gray, ButtonBorderStyle.Solid);

            DrawRuler(e.Graphics);

        }


        void DrawRuler(Graphics g)
        {
            ctrlSize = this.Size;

            startX = 2;
            startY = 0;

            int mm = 0;

            float lineX = startX;
            float lineY = startY;
            float lineHeight = normalLineHeight;
            for (mm = 0; mm >= 0; mm++)
            {
                if (mm % 10 == 0)
                {
                    lineHeight = ctrlSize.Height - bottomMargin;
                }
                else if (mm % 5 == 0)
                {
                    lineHeight = halfLineHeight;
                }
                else
                {
                    lineHeight = normalLineHeight;
                }
                lineX = ConvertMMToPixel(mm, g.DpiX);

                // Draw vertical line 
                g.DrawLine(linepen, new PointF(lineX, lineY), new PointF(lineX, lineHeight));

                // Draw Unit Text
                if (mm % 10 == 0)
                {
                    g.DrawString((mm / 10).ToString(), new Font("Fixedsys", 8), Brushes.Black, new PointF(lineX + 3, lineY + lineHeight - 12));
                }

                if (ctrlSize.Width < lineX + rightMargin)
                    break;
            }

        }

        double ConvertPixelToMM(float pixels, float dpi)
        {
            double mm = (pixels * 25.4) / dpi;
            return mm;
        }

        float ConvertMMToPixel(double mm, float dpi)
        {
            float pixels = (float)(mm * dpi) / 25.4f;
            return pixels;
        }
    }
}
