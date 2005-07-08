/***************************************************************************
 *   Copyright (C) 2005 by Ambertation                                     *
 *   quaxi@ambertation.de                                                  *
 *                                                                         *
 *   This program is free software; you can redistribute it and/or modify  *
 *   it under the terms of the GNU General Public License as published by  *
 *   the Free Software Foundation; either version 2 of the License, or     *
 *   (at your option) any later version.                                   *
 *                                                                         *
 *   This program is distributed in the hope that it will be useful,       *
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of        *
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the         *
 *   GNU General Public License for more details.                          *
 *                                                                         *
 *   You should have received a copy of the GNU General Public License     *
 *   along with this program; if not, write to the                         *
 *   Free Software Foundation, Inc.,                                       *
 *   59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.             *
 ***************************************************************************/
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace SimPe
{
	/// <summary>
	/// Zusammenfassung f�r About.
	/// </summary>
	public class About : System.Windows.Forms.Form
	{
		private System.Windows.Forms.PictureBox pbTop;
		private System.Windows.Forms.PictureBox pbBottom;
		private System.Windows.Forms.RichTextBox rtb;
		private System.Windows.Forms.Button button1;
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public About()
		{
			//
			// Erforderlich f�r die Windows Form-Designerunterst�tzung
			//
			InitializeComponent();

			
		}

		/// <summary>
		/// Die verwendeten Ressourcen bereinigen.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Vom Windows Form-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode f�r die Designerunterst�tzung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor ge�ndert werden.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(About));
			this.pbTop = new System.Windows.Forms.PictureBox();
			this.pbBottom = new System.Windows.Forms.PictureBox();
			this.rtb = new System.Windows.Forms.RichTextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// pbTop
			// 
			this.pbTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pbTop.Image = ((System.Drawing.Image)(resources.GetObject("pbTop.Image")));
			this.pbTop.Location = new System.Drawing.Point(0, 0);
			this.pbTop.Name = "pbTop";
			this.pbTop.Size = new System.Drawing.Size(792, 128);
			this.pbTop.TabIndex = 0;
			this.pbTop.TabStop = false;
			// 
			// pbBottom
			// 
			this.pbBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pbBottom.Image = ((System.Drawing.Image)(resources.GetObject("pbBottom.Image")));
			this.pbBottom.Location = new System.Drawing.Point(0, 414);
			this.pbBottom.Name = "pbBottom";
			this.pbBottom.Size = new System.Drawing.Size(792, 24);
			this.pbBottom.TabIndex = 1;
			this.pbBottom.TabStop = false;
			// 
			// rtb
			// 
			this.rtb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.rtb.BackColor = System.Drawing.Color.White;
			this.rtb.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.rtb.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.rtb.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.rtb.Location = new System.Drawing.Point(32, 128);
			this.rtb.Name = "rtb";
			this.rtb.ReadOnly = true;
			this.rtb.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.rtb.Size = new System.Drawing.Size(736, 288);
			this.rtb.TabIndex = 2;
			this.rtb.Text = "";
			this.rtb.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.rtb_LinkClicked);
			this.rtb.Enter += new System.EventHandler(this.rtb_Enter);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(344, 80);
			this.button1.Name = "button1";
			this.button1.TabIndex = 3;
			this.button1.Text = "button1";
			// 
			// About
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = new System.Drawing.Size(792, 438);
			this.Controls.Add(this.rtb);
			this.Controls.Add(this.pbBottom);
			this.Controls.Add(this.pbTop);
			this.Controls.Add(this.button1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "About";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "About";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Display the About Screen
		/// </summary>
		public static void ShowAbout()
		{
			About f = new About();
			f.Text = SimPe.Localization.GetString("About");

			System.Diagnostics.FileVersionInfo v = Helper.SimPeVersion;
			System.IO.Stream s = f.GetType().Assembly.GetManifestResourceStream("SimPe.about.rtf");
			System.IO.StreamReader sr = new System.IO.StreamReader(s);
			string vtext = v.FileMajorPart +"."+v.FileMinorPart;
			if (Helper.QARelease) vtext = "QA " + vtext;
			if (Helper.DebugMode) vtext += " [debug]";
			f.rtb.Rtf = sr.ReadToEnd().Replace("\\{Version\\}", vtext);

			f.ShowDialog();
		}

		/// <summary>
		/// Display the Update Screen
		/// </summary>
		/// <param name="show">true, if it should be visible even if no updates were found</param>
		public static void ShowUpdate(bool show)
		{
			WaitingScreen.Wait();
			About f = new About();
			f.Text = SimPe.Localization.GetString("Updates");
			long version = 0;
			long qaversion = 0;
			string text = "";
			SimPe.UpdateState res = WebUpdate.CheckUpdate(ref version, ref qaversion);
			
			if (!show && (res != SimPe.UpdateState.Nothing)) 
			{
				DialogResult dr = Message.Show("A new SimPE Version is available. Should SimPE display the new Version Information?", "Updates", MessageBoxButtons.YesNo);
				if (dr==DialogResult.No) res = SimPe.UpdateState.Nothing;
			}

			text = "<h2><span class=\"highlight\">Current Version:</span> "+Helper.SimPeVersionString;
			if (Helper.DebugMode) text += " ("+Helper.SimPeVersionLong.ToString()+")";
			text += "</h2>";
			if (Helper.QARelease) text += "<h2><span class=\"highlight\">Available QA-Version:</span> "+Helper.LongVersionToString(qaversion)+"</h2>";
			text += "<h2><span class=\"highlight\">Available Version:</span> "+Helper.LongVersionToString(version);
			if (res == SimPe.UpdateState.NewRelease) text += " (download: <b>http://sims.ambertation.de/download.shtml</b>)";
			text += "</h2>";
			text += "<br /><br />";

			if (res == SimPe.UpdateState.NewRelease) text += WebUpdate.GetChangeLog();
			else if (res == SimPe.UpdateState.NewQARelease) text += "A new <b>QA Version</b> has been released.<br />You can get it at <b>http://ambertation.de/simpeforum/viewforum.php?f=14</b>.";
			else text += "There is no new SimPE Version available.";

			f.rtb.Rtf = Ambertation.Html2Rtf.Convert(text);
			WaitingScreen.Stop();			
			if (show || (res != SimPe.UpdateState.Nothing)) f.ShowDialog();
		}

		private void rtb_LinkClicked(object sender, System.Windows.Forms.LinkClickedEventArgs e)
		{
			try 
			{
				System.Windows.Forms.Help.ShowHelp(this, e.LinkText);
			} 
			catch (Exception ex) 
			{
				Helper.ExceptionMessage(ex);
			}
		}

		private void rtb_Enter(object sender, System.EventArgs e)
		{
			button1.Focus();
		}
	}
}