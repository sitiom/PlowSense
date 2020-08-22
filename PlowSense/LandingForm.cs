﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlowSense
{
	public sealed partial class LandingForm : Form
	{
		public LandingForm()
		{
			InitializeComponent();
			//Prepare to paint backgroundImage
			DoubleBuffered = true;
			SetStyle(ControlStyles.ResizeRedraw, true);
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);
			var rc = new Rectangle(ClientSize.Width - Properties.Resources.landscape_png_95_images_in_collection_page_2_landscape_png_1024_232.Width + 100,
				ClientSize.Height - Properties.Resources.landscape_png_95_images_in_collection_page_2_landscape_png_1024_232.Height,
				Properties.Resources.landscape_png_95_images_in_collection_page_2_landscape_png_1024_232.Width, Properties.Resources.landscape_png_95_images_in_collection_page_2_landscape_png_1024_232.Height);
			e.Graphics.DrawImage(Properties.Resources.landscape_png_95_images_in_collection_page_2_landscape_png_1024_232, rc);
		}

		private void passwordTextBox_IconRightClick(object sender, EventArgs e)
		{
			if (passwordTextBox.PasswordChar == '*')
			{
				passwordTextBox.PasswordChar = '\0';
				passwordTextBox.IconRight = Properties.Resources.icons8_eye_512px;

			}
			else
			{
				passwordTextBox.PasswordChar = '*';
				passwordTextBox.IconRight = Properties.Resources.icons8_hide_512px;
			}
		}

		private void signUpLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			SignUpForm signUpForm = new SignUpForm();
			signUpForm.ShowDialog();
		}
	}
}
