using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HonasRuler
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {
            richTextBox1.Rtf = @"{\rtf1\pc \b HonasRuler 1.0 \par \par Copyright 2017. HONAS all rights reserved. \par \par Email : racuca@gmail.com \par \par URL : https://github.com/racuca/honasRuler";
            richTextBox1.SelectAll();
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox1.Select(0, 0);
            richTextBox1.ReadOnly = true;
            richTextBox1.LinkClicked += RichTextBox1_LinkClicked;

            this.Focus();
        }

        private void RichTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("iexplore", e.LinkText);
        }
    }
}
