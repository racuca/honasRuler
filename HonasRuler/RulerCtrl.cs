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
        public enum EUnitType
        {
            mm, 
            inch,
            pixel,
            Max
        }
        EUnitType unittype = EUnitType.mm;

        public EUnitType UnitType
        {
            get { return unittype; }
            set {
                CurrentInterval = UnitIntervals[(int)value];
                unittype = value;
            }
        }
        UnitInterval[] UnitIntervals = { new UnitInterval(1, 5, 2),
                                         new UnitInterval(1, 1, 2),
                                         new UnitInterval(50, 50, 2)
        };
        public UnitInterval CurrentInterval;

        public enum DirectionType
        {
            Top,
            Bottom,
            Left,
            Right
        }
        public DirectionType Direction = DirectionType.Top;



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
            CurrentInterval = UnitIntervals[(int)unittype];
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

            float dpix = g.DpiX;
            float dpiy = g.DpiY;
            Screen[] sc = Screen.AllScreens;
            foreach (Screen s in sc)
            {
                int bits = s.BitsPerPixel;
                bool primary = s.Primary;
                Size size = s.WorkingArea.Size;
            }

            float slineX = startX;  // Start X point
            float slineY = startY;  // Start Y point
            float elineX = startX;  // End X point
            float elineY = startY;  // End Y point
            float lineHeight = normalLineHeight;
            for (mm = 0; mm >= 0; mm += CurrentInterval.Interval)
            {
                // Calculate line Height for each unit value
                if (mm % CurrentInterval.FullInterval == 0)
                {
                    lineHeight = ctrlSize.Height - bottomMargin;
                }
                else if (mm % CurrentInterval.halfInterval == 0)
                {
                    lineHeight = halfLineHeight;
                }
                else
                {
                    lineHeight = normalLineHeight;
                }

                switch (unittype)
                {
                    case EUnitType.mm: slineX = ConvertMMToPixel(mm, g.DpiX); break;
                    case EUnitType.inch: slineX = ConvertInchToPixel(mm, g.DpiX); break;
                    case EUnitType.pixel: slineX = ConvertInchToPixel(mm, g.DpiX); break;
                }

                switch (Direction)
                {
                    case DirectionType.Top:
                        slineY = 0;
                        elineX = slineX;
                        elineY = lineHeight;
                        break;
                    case DirectionType.Bottom:
                        slineY = this.Height;
                        elineY = -lineHeight;
                        break;
                    case DirectionType.Left:
                        slineX = this.Location.X;
                        slineY = this.Location.Y - slineX;
                        break;
                    case DirectionType.Right:
                        slineX = this.Location.X;
                        slineY = this.Location.Y + slineX;
                        break;
                }

                // Draw line 
                g.DrawLine(linepen, new PointF(slineX, slineY), new PointF(elineX, elineY));

                // Draw Unit Text
                if (mm % 10 == 0)
                {
                    g.DrawString((mm / 10).ToString(), new Font("Fixedsys", 8), Brushes.Black, new PointF(slineX + 3, slineY + elineY - 12));
                }

                if (ctrlSize.Width < slineX + rightMargin)
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

        double ConvertPixelToInch(float pixels, float dpi)
        {
            double mm = (pixels * 72) / dpi;
            return mm;
        }

        float ConvertInchToPixel(double mm, float dpi)
        {
            float pixels = (float)(mm * dpi) / 72;
            return pixels;
        }
    }

    public class UnitInterval
    {
        /// <summary>
        /// One Unit for Each UnitType
        /// </summary>
        public int Interval;

        /// <summary>
        /// Half Seperator Line for Each UnitType
        /// </summary>
        public int halfInterval;


        public int Multifier;

        public UnitInterval(int i, int half, int multi)
        {
            Interval = i;
            halfInterval = half;
            Multifier = multi;
        }

        public int FullInterval
        {
            get {
                return halfInterval * Multifier;
            }
        }
    }
}
