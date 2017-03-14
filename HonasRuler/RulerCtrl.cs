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

        public delegate void SizeChangeReqDelegate(SizeF s);
        public event SizeChangeReqDelegate sizeChangeReqEvent;


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
                                         new UnitInterval(0.5f, 0.5f, 2),
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


        public static double[] MonitorSizeInch = new double[] { 3.5, 4, 5, 7.9, 9.7, 10.1, 11.6, 13.3, 14, 14.4,
                                                                15, 15.6, 17, 18.4, 19, 21, 22, 23 };

        public double currentMonitorSizeInch = 15.6;


        public static float[] SizeUnit = { 5, 10, 15, 20, 25, 30 };
        public static float[] SizeUnitforPixel = { 100, 200, 400, 800, 1000, 1500, 2000, 2500 };

        public float currentMaxSizeforUnit = 15;
        public bool MaxSizeChangeRequest = false;


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

        Screen currentScreen;




        public RulerCtrl()
        {
            InitializeComponent();
            this.ResizeRedraw = true;
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
            Debug.WriteLine("DrawRuler");

            ctrlSize = this.Size;

            startX = 2;
            startY = 0;

            float mm = 0;

            float dpix = g.DpiX;
            float dpiy = g.DpiY;
            Screen[] sc = Screen.AllScreens;
            foreach (Screen s in sc)
            {
                Size size = s.WorkingArea.Size;
                Point screenpoint = this.PointToScreen(this.Location);
                if (s.WorkingArea.Contains(screenpoint))
                    currentScreen = s;
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
                    if (Direction == DirectionType.Left || Direction == DirectionType.Right)
                    {
                        lineHeight = ctrlSize.Width - bottomMargin;
                    }
                    else 
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

                float RelativeStartX = 0;
                switch (unittype)
                {
                    case EUnitType.mm: RelativeStartX = ConvertMMToPixel(mm, g.DpiX); break;
                    case EUnitType.inch: RelativeStartX = ConvertInchToPixel(mm, g.DpiX); break;
                    case EUnitType.pixel: RelativeStartX = mm; break;
                }

                PointF TextPoint = new PointF();
                switch (Direction)
                {
                    case DirectionType.Top:
                        slineX = elineX = RelativeStartX;
                        slineY = 0;
                        elineY = lineHeight;
                        TextPoint.X = slineX + 3;
                        TextPoint.Y = elineY - 12;
                        break;
                    case DirectionType.Bottom:
                        slineX = elineX = RelativeStartX;
                        slineY = this.Height;
                        elineY = this.Height - lineHeight;
                        TextPoint.X = slineX + 3;
                        TextPoint.Y = elineY - 3;
                        break;
                    case DirectionType.Left:
                        slineX = this.Location.X;
                        slineY = elineY = this.Height - RelativeStartX;
                        elineX = lineHeight;
                        TextPoint.X = elineX - 16;
                        TextPoint.Y = elineY - 16;
                        break;
                    case DirectionType.Right:
                        slineX = this.Width;
                        slineY = elineY = RelativeStartX;
                        elineX = this.Width - lineHeight;
                        TextPoint.X = elineX + 3;
                        TextPoint.Y = elineY + 3;
                        break;
                }

                // Don't draw control outside
                if (slineX < 0 || slineY < 0 || elineX < 0 || elineY < 0)
                    break;

                float realunits = mm;
                if (unittype == EUnitType.mm)
                {
                    realunits = mm / 10f;
                }

                if (MaxSizeChangeRequest)
                {
                    if (currentMaxSizeforUnit < realunits)
                    {
                        MaxSizeChangeRequest = false;
                        SizeF maxSize = new SizeF(Math.Abs(RelativeStartX), this.Height);
                        if (Direction == DirectionType.Left || Direction == DirectionType.Right)
                            maxSize = new SizeF(this.Width, Math.Abs(RelativeStartX));
                        if (sizeChangeReqEvent != null)
                            sizeChangeReqEvent(maxSize);
                        break;
                    }
                }
                else if (slineX > this.Width || slineY > this.Height || elineX > this.Width || elineY > this.Height)
                {
                    break;
                }

                // Draw line 
                g.DrawLine(linepen, new PointF(slineX, slineY), new PointF(elineX, elineY));

                // Draw Unit Text
                if (mm % CurrentInterval.FullInterval == 0)
                {
                    string intervalvalue = (mm / CurrentInterval.FullInterval).ToString();
                    if (unittype == EUnitType.pixel)
                        intervalvalue = mm.ToString();
                    g.DrawString(intervalvalue, new Font("Fixedsys", 8), Brushes.Black, TextPoint);
                }
            }
        }

        double ConvertPixelToMM(float pixels, float dpi)
        {
            double mm = (pixels * 25.4) / dpi;
            return mm;
        }

        float ConvertMMToPixel(double mm, float dpi)
        {
            float pixels = 0;

            // DPI method - works fine on only 1920x1080 96 dpi monitor.
            //pixels = (float)(mm * dpi) / 25.4f;

            // Resolution method
            Size s = currentScreen.Bounds.Size;
            float xinches;
            float yinches;
            GetRealInchforWidthHeight(out xinches, out yinches);
            
            if (Direction == DirectionType.Top || Direction == DirectionType.Bottom)
                pixels = (float)mm * s.Width / (xinches * 25.4f);
            else
                pixels = (float)mm * s.Height / (yinches * 25.4f);

            return pixels;
        }

        double ConvertPixelToInch(float pixels, float dpi)
        {
            double mm = (pixels * 72) / dpi;
            return mm;
        }

        float ConvertInchToPixel(double inch, float dpi)
        {
            float pixels;

            Size s = currentScreen.Bounds.Size;
            float xinches;
            float yinches;
            GetRealInchforWidthHeight(out xinches, out yinches);

            if (Direction == DirectionType.Top || Direction == DirectionType.Bottom)
                pixels = (float)inch * s.Width / xinches;
            else
                pixels = (float)inch * s.Height / yinches;

            return pixels;
        }

        void GetRealInchforWidthHeight(out float xinches, out float yinches)
        {
            Size s = currentScreen.Bounds.Size;
            double sqrtdiagonal = Math.Sqrt(s.Width * s.Width + s.Height * s.Height);
            xinches = (float)(currentMonitorSizeInch * s.Width / sqrtdiagonal);
            yinches = (float)(currentMonitorSizeInch * s.Height / sqrtdiagonal);
        }
    }

    public class UnitInterval
    {
        /// <summary>
        /// One Unit for Each UnitType
        /// </summary>
        public float Interval;

        /// <summary>
        /// Half Seperator Line for Each UnitType
        /// </summary>
        public float halfInterval;


        public float Multifier;

        public UnitInterval(float interval, float half, float multi)
        {
            Interval = interval;
            halfInterval = half;
            Multifier = multi;
        }

        public float FullInterval
        {
            get {
                return halfInterval * Multifier;
            }
        }
    }
}
