﻿using System;
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
            MonitorSizeInch,
            Unit,
            Direction,
            MaxSize
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


        ToolStripMenuItem screensizecheckeditem;
        ToolStripMenuItem unitcheckeditem;
        ToolStripMenuItem directioncheckeditem;
        ToolStripMenuItem maxsizecheckeditem;

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

            rulerCtrl1.sizeChangeReqEvent += RulerCtrl1_sizeChangeReqEvent;
        }

        private void RulerCtrl1_sizeChangeReqEvent(SizeF s)
        {
            if (IsToporBottom(rulerCtrl1.Direction))
            {
                this.Width = (int)s.Width;
            }
            else
            {
                this.Height = (int)s.Height;
            }
        }


        #region Control Event

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.LightGray;
            
            ToolStripMenuItem toolstrip = contextMenuStrip1.Items[(int)MenuItems.MonitorSizeInch] as ToolStripMenuItem;
            foreach (double s in RulerCtrl.MonitorSizeInch)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = s.ToString();
                item.Tag = s;
                item.Click += monitorsizeItem_Click;
                toolstrip.DropDownItems.Add(item);

                if (rulerCtrl1.currentMonitorSizeInch == s)
                {
                    item.Checked = true;
                    screensizecheckeditem = item;
                }
            }


            toolstrip = contextMenuStrip1.Items[(int)MenuItems.Unit] as ToolStripMenuItem;
            unitcheckeditem = (ToolStripMenuItem)toolstrip.DropDownItems[(int)MenuItemsforUnit.mm];
            unitcheckeditem.Checked = true;

            toolstrip = contextMenuStrip1.Items[(int)MenuItems.Direction] as ToolStripMenuItem;
            directioncheckeditem = (ToolStripMenuItem)toolstrip.DropDownItems[0];
            directioncheckeditem.Checked = true;

            toolstrip = contextMenuStrip1.Items[(int)MenuItems.MaxSize] as ToolStripMenuItem;
            foreach (float s in RulerCtrl.SizeUnit)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = s.ToString();
                item.Tag = s;
                item.Click += maxSizeItem_Click;
                toolstrip.DropDownItems.Add(item);

                if (rulerCtrl1.currentMaxSizeforUnit == s)
                {
                    item.Checked = true;
                    maxsizecheckeditem = item;
                }
            }
            maxsizecheckeditem.Checked = true;            
        }

        private void maxSizeItem_Click(object sender, EventArgs e)
        {
            maxsizecheckeditem.Checked = false;
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            rulerCtrl1.currentMaxSizeforUnit = (float)item.Tag;

            maxsizecheckeditem = item;
            maxsizecheckeditem.Checked = true;

            rulerCtrl1.MaxSizeChangeRequest = true;

            rulerCtrl1.Update();
            this.Refresh();
        }

        private void monitorsizeItem_Click(object sender, EventArgs e)
        {
            screensizecheckeditem.Checked = false;
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            rulerCtrl1.currentMonitorSizeInch = (double)item.Tag;

            screensizecheckeditem = item;
            screensizecheckeditem.Checked = true;

            rulerCtrl1.Update();
            this.Refresh();
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
                pictureBox1.Image = null;
                pictureBox1.Visible = false;
            }
            else
            {
                pictureBox1.Image = Properties.Resources.HonasLogo;
                pictureBox1.Visible = true;
            }
            bcollapsed = !bcollapsed;
        }

        private void mmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            unitcheckeditem.Checked = false;
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            unitcheckeditem = item;
            unitcheckeditem.Checked = true;

            rulerCtrl1.UnitType = RulerCtrl.EUnitType.mm;
            this.Refresh();
        }

        private void inchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            unitcheckeditem.Checked = false;
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            unitcheckeditem = item;
            unitcheckeditem.Checked = true;

            rulerCtrl1.UnitType = RulerCtrl.EUnitType.inch;
            this.Refresh();
        }

        private void pixelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            unitcheckeditem.Checked = false;
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            unitcheckeditem = item;
            unitcheckeditem.Checked = true;

            rulerCtrl1.UnitType = RulerCtrl.EUnitType.pixel;
            this.Refresh();
        }

        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            directioncheckeditem.Checked = false;
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            directioncheckeditem = item;
            directioncheckeditem.Checked = true;

            RotateTo(RulerCtrl.DirectionType.Top);
            rulerCtrl1.Direction = RulerCtrl.DirectionType.Top;
            this.Refresh();
        }

        private void bottomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            directioncheckeditem.Checked = false;
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            directioncheckeditem = item;
            directioncheckeditem.Checked = true;

            RotateTo(RulerCtrl.DirectionType.Bottom);
            rulerCtrl1.Direction = RulerCtrl.DirectionType.Bottom;
            this.Refresh();
        }

        private void leftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            directioncheckeditem.Checked = false;
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            directioncheckeditem = item;
            directioncheckeditem.Checked = true;

            RotateTo(RulerCtrl.DirectionType.Left);
            rulerCtrl1.Direction = RulerCtrl.DirectionType.Left;
            this.Refresh();
        }

        private void rightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            directioncheckeditem.Checked = false;
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            directioncheckeditem = item;
            directioncheckeditem.Checked = true;

            RotateTo(RulerCtrl.DirectionType.Right);
            rulerCtrl1.Direction = RulerCtrl.DirectionType.Right;
            this.Refresh();
        }


        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About a = new About();
            a.ShowDialog(this);
        }

        #endregion




        #region Misc Functions

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
                    break;
                case RulerCtrl.DirectionType.Bottom:
                    rulerCtrl1.Dock = DockStyle.Bottom;
                    break;
                case RulerCtrl.DirectionType.Left:
                    rulerCtrl1.Dock = DockStyle.Left;
                    break;
                case RulerCtrl.DirectionType.Right:
                    rulerCtrl1.Dock = DockStyle.Right;
                    break;
            }

            if (pictureBox1.Image != null)
            {
                RotateFlipType rotatefliptype = GetRotateFilpType(type);
                pictureBox1.Image = ImageHelper.RotateImage(pictureBox1.Image, rotatefliptype);
            }
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
                        case RulerCtrl.DirectionType.Left: rotatetype = RotateFlipType.Rotate90FlipNone; break;
                        case RulerCtrl.DirectionType.Right: rotatetype = RotateFlipType.Rotate270FlipNone; break;
                    }
                    break;
                case RulerCtrl.DirectionType.Left:
                    switch (rulerCtrl1.Direction)
                    {
                        case RulerCtrl.DirectionType.Top: rotatetype = RotateFlipType.Rotate270FlipNone; break;
                        case RulerCtrl.DirectionType.Bottom: rotatetype = RotateFlipType.Rotate270FlipNone; break;
                        case RulerCtrl.DirectionType.Left: rotatetype = RotateFlipType.RotateNoneFlipNone; break;
                        case RulerCtrl.DirectionType.Right: rotatetype = RotateFlipType.Rotate180FlipNone; break;
                    }
                    break;
                case RulerCtrl.DirectionType.Right:
                    switch (rulerCtrl1.Direction)
                    {
                        case RulerCtrl.DirectionType.Top: rotatetype = RotateFlipType.Rotate90FlipNone; break;
                        case RulerCtrl.DirectionType.Bottom: rotatetype = RotateFlipType.Rotate90FlipNone; break;
                        case RulerCtrl.DirectionType.Left: rotatetype = RotateFlipType.Rotate180FlipNone; break;
                        case RulerCtrl.DirectionType.Right: rotatetype = RotateFlipType.RotateNoneFlipNone; break;
                    }
                    break;
            }

            return rotatetype;
        }

        #endregion

        private void logoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bcollapsed == false)
            {
                pictureBox1.Image = null;
                pictureBox1.Visible = false;
            }
            else
            {
                pictureBox1.Image = Properties.Resources.HonasLogo;
                pictureBox1.Visible = true;
            }
            bcollapsed = !bcollapsed;

            logoToolStripMenuItem.Checked = !bcollapsed;
        }

        private void Form1_LocationChanged(object sender, EventArgs e)
        {
            ProcessStartInfo pro = new ProcessStartInfo();
            Process[] honasrulers = Process.GetProcessesByName("HonasRuler");

            if (honasrulers.Length <= 1)
                return;

            foreach (Process ruler in honasrulers)
            {
                
            }
        }
    }
}
