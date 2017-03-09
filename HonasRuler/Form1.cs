using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HonasRuler
{
    public partial class Form1 : Form
    {
        public enum MenuItems
        {
            Unit

        }

        public enum MenuItemsforUnit
        {
            mm,
            Inch,
            Pixel                
        }

        string extendstr = "▽";
        string collapsestr = "△";
        bool bcollapsed = false;


        public Form1()
        {
            InitializeComponent();

            this.BackColor = Color.LightSlateGray;
            this.TransparencyKey = Color.LightSlateGray;

            this.TopMost = true;

            this.ResizeRedraw = true;

            rulerCtrl1.UnitType = RulerCtrl.EUnitType.mm;
            ToolStripMenuItem toolstrip = contextMenuStrip1.Items[(int)MenuItems.Unit] as ToolStripMenuItem;
            ToolStripMenuItem unititem = toolstrip.DropDownItems[(int)MenuItemsforUnit.mm] as ToolStripMenuItem;
            unititem.CheckState = CheckState.Checked;
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            advCollpseBtn.Width = 50;
            advCollpseBtn.Location = new Point(rulerCtrl1.Location.X, pictureBox1.Location.Y);
            advCollpseBtn.Text = extendstr;

            pictureBox1.BackColor = Color.LightGray;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            e.Graphics.FillRectangle(Brushes.LightSlateGray, e.ClipRectangle);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }
        
        private Point lastLocation;
        private void rulerCtrl1_MouseDown(object sender, MouseEventArgs e)
        {
            lastLocation = e.Location;
        }

        private void rulerCtrl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Location = new Point((this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void rulerCtrl1_MouseUp(object sender, MouseEventArgs e)
        {
            this.Refresh();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            lastLocation = e.Location;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Location = new Point((this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            this.Refresh();
        }

        private void advCollpseBtn_Click(object sender, EventArgs e)
        {
            if (bcollapsed == false)
            {
                advCollpseBtn.Text = extendstr;
                pictureBox1.Image = null;
                pictureBox1.Visible = false;
            }
            else
            {
                advCollpseBtn.Text = collapsestr;
                pictureBox1.Image = Properties.Resources.HonasLogo;
                pictureBox1.Visible = true;
            }
            bcollapsed = !bcollapsed;
        }

        private void mmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rulerCtrl1.UnitType = RulerCtrl.EUnitType.mm;
            this.Update();
        }

        private void inchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rulerCtrl1.UnitType = RulerCtrl.EUnitType.inch;
            this.Update();
        }

        private void pixelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rulerCtrl1.UnitType = RulerCtrl.EUnitType.pixel;
            this.Update();
        }

        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RotateTo(RulerCtrl.DirectionType.Top);
            rulerCtrl1.Direction = RulerCtrl.DirectionType.Top;
            this.Update();
        }

        private void bottomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RotateTo(RulerCtrl.DirectionType.Bottom);
            rulerCtrl1.Direction = RulerCtrl.DirectionType.Bottom;
            this.Update();
        }

        private void leftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RotateTo(RulerCtrl.DirectionType.Left);
            rulerCtrl1.Direction = RulerCtrl.DirectionType.Left;
            this.Update();
        }

        private void rightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RotateTo(RulerCtrl.DirectionType.Right);
            rulerCtrl1.Direction = RulerCtrl.DirectionType.Right;
            this.Update();
        }
        
        private void RotateTo(RulerCtrl.DirectionType type)
        {
            bool rotate90 = false;

            if (IsToporBottom(rulerCtrl1.Direction) != IsToporBottom(type))
            {
                rotate90 = true;
            }

            if (rotate90)
            {
                int rulerWidth = rulerCtrl1.Width;
                int rulerHeight = rulerCtrl1.Height;

                int pictureWidth = rulerCtrl1.Width;
                int pictureHeight = rulerCtrl1.Height;

                int width = this.Width;
                int height = this.Height;
                this.Width = height;
                this.Height = width;

                rulerCtrl1.Width = rulerHeight;
                rulerCtrl1.Height = rulerWidth;

                pictureBox1.Width = pictureHeight;
                pictureBox1.Height = pictureWidth;

            }

            switch (type)
            {
                case RulerCtrl.DirectionType.Top:
                    rulerCtrl1.Dock = DockStyle.Top; 
                    advCollpseBtn.Location = new Point(rulerCtrl1.Location.X, pictureBox1.Location.Y);
                    break;
                case RulerCtrl.DirectionType.Bottom:
                    rulerCtrl1.Dock = DockStyle.Bottom;
                    advCollpseBtn.Location = new Point(rulerCtrl1.Location.X, rulerCtrl1.Location.Y - advCollpseBtn.Height);
                    break;
                case RulerCtrl.DirectionType.Left:
                    rulerCtrl1.Dock = DockStyle.Left;
                    advCollpseBtn.Location = new Point(rulerCtrl1.Location.X, rulerCtrl1.Location.Y - advCollpseBtn.Height);
                    break;
                case RulerCtrl.DirectionType.Right:
                    rulerCtrl1.Dock = DockStyle.Right;
                    advCollpseBtn.Location = new Point(rulerCtrl1.Location.X, rulerCtrl1.Location.Y - advCollpseBtn.Height);
                    break;
            }

            RotateFlipType rotatefliptype = GetRotateFilpType(type);
            pictureBox1.Image = ImageHelper.RotateImage(pictureBox1.Image, rotatefliptype);
        }

        bool IsToporBottom(RulerCtrl.DirectionType type)
        {
            if (type == RulerCtrl.DirectionType.Top || type == RulerCtrl.DirectionType.Bottom)
                return true;

            return false;
        }

        RotateFlipType GetRotateFilpType(RulerCtrl.DirectionType type)
        {
            RotateFlipType rotatetype = RotateFlipType.RotateNoneFlipNone;
            switch (type)
            {
                case RulerCtrl.DirectionType.Top:
                    switch (rulerCtrl1.Direction)
                    {
                        case RulerCtrl.DirectionType.Top: rotatetype = RotateFlipType.RotateNoneFlipNone; break;
                        case RulerCtrl.DirectionType.Bottom: rotatetype = RotateFlipType.RotateNoneFlipNone; break;
                        case RulerCtrl.DirectionType.Left: rotatetype = RotateFlipType.Rotate90FlipNone; break;
                        case RulerCtrl.DirectionType.Right: rotatetype = RotateFlipType.Rotate270FlipNone; break;
                    }
                    break;
                case RulerCtrl.DirectionType.Bottom:
                    switch (rulerCtrl1.Direction)
                    {
                        case RulerCtrl.DirectionType.Top: rotatetype = RotateFlipType.RotateNoneFlipNone; break;
                        case RulerCtrl.DirectionType.Bottom: rotatetype = RotateFlipType.RotateNoneFlipNone; break;
                        case RulerCtrl.DirectionType.Left: rotatetype = RotateFlipType.Rotate270FlipNone; break;
                        case RulerCtrl.DirectionType.Right: rotatetype = RotateFlipType.Rotate90FlipNone; break;
                    }
                    break;
                case RulerCtrl.DirectionType.Left:
                    switch (rulerCtrl1.Direction)
                    {
                        case RulerCtrl.DirectionType.Top: rotatetype = RotateFlipType.Rotate90FlipNone; break;
                        case RulerCtrl.DirectionType.Bottom: rotatetype = RotateFlipType.Rotate90FlipNone; break;
                        case RulerCtrl.DirectionType.Left: rotatetype = RotateFlipType.RotateNoneFlipNone; break;
                        case RulerCtrl.DirectionType.Right: rotatetype = RotateFlipType.Rotate180FlipNone; break;
                    }
                    break;
                case RulerCtrl.DirectionType.Right:
                    switch (rulerCtrl1.Direction)
                    {
                        case RulerCtrl.DirectionType.Top: rotatetype = RotateFlipType.Rotate270FlipNone; break;
                        case RulerCtrl.DirectionType.Bottom: rotatetype = RotateFlipType.Rotate270FlipNone; break;
                        case RulerCtrl.DirectionType.Left: rotatetype = RotateFlipType.Rotate180FlipNone; break;
                        case RulerCtrl.DirectionType.Right: rotatetype = RotateFlipType.RotateNoneFlipNone; break;
                    }
                    break;
            }

            return rotatetype;
        }
    }
}
