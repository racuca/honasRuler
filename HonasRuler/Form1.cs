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
                int width = this.Width;
                int height = this.Height;
                this.Height = width;
                this.Width = height;
            }

            switch (type)
            {
                case RulerCtrl.DirectionType.Top: rulerCtrl1.Dock = DockStyle.Top; break;
                case RulerCtrl.DirectionType.Bottom: rulerCtrl1.Dock = DockStyle.Bottom; break;
                case RulerCtrl.DirectionType.Left: rulerCtrl1.Dock = DockStyle.Left; break;
                case RulerCtrl.DirectionType.Right: rulerCtrl1.Dock = DockStyle.Right; break;
            }
        }

        bool IsToporBottom(RulerCtrl.DirectionType type)
        {
            if (type == RulerCtrl.DirectionType.Top || type == RulerCtrl.DirectionType.Bottom)
                return true;

            return false;
        }
    }
}
