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
using SimPe.Interfaces.Plugin;
using SimPe.PackedFiles.Wrapper;

namespace SimPe.PackedFiles.UserInterface
{
	/// <summary>
	/// UI Handler for a Str Wrapper
	/// </summary>
	public class CpfUI : IPackedFileUI	
	{
		public delegate void ExecutePreview(Cpf mmat, SimPe.Interfaces.Files.IPackageFile package);

		#region Code to Startup the UI

		/// <summary>
		/// Holds a reference to the Form containing the UI Panel
		/// </summary>
		private Elements2 form;
		ExecutePreview fkt;

		/// <summary>
		/// Constructor for the Class
		/// </summary>
		public CpfUI(ExecutePreview fkt)
		{
			form = new Elements2();
			form.cbtype.Items.Add(Data.MetaData.DataTypes.dtString);
			form.cbtype.Items.Add(Data.MetaData.DataTypes.dtUInteger);
			form.cbtype.Items.Add(Data.MetaData.DataTypes.dtInteger);
			form.cbtype.Items.Add(Data.MetaData.DataTypes.dtSingle);
			form.cbtype.Items.Add(Data.MetaData.DataTypes.dtBoolean);
			

			form.cbtype.SelectedIndex = 0;

			this.fkt = fkt;
		}
		#endregion
		
		#region IPackedFileUI Member

		/// <summary>
		/// Returns the Panel that will be displayed within SimPe
		/// </summary>
		public System.Windows.Forms.Control GUIHandle
		{
			get
			{
				return form.CpfPanel;
			}
		}

		/// <summary>
		/// Is called by SimPe (through the Wrapper) when the Panel is going to be displayed, so
		/// you should updatet the Data displayed by the Panel with the Attributes stored in the
		/// passed Wrapper.
		/// </summary>
		/// <param name="wrapper">The Attributes of this Wrapper have to be displayed</param>
		public void UpdateGUI(IFileWrapper wrapper)
		{
			form.wrapper = (IFileWrapperSaveExtension)wrapper;
			Cpf wrp = (Cpf) wrapper;

			form.lbcpf.Items.Clear();
			foreach (CpfItem item in wrp.Items) 
			{
				form.lbcpf.Items.Add(item);
			}

			form.llcpfchange.Enabled = false;
			form.btprev.Visible = (fkt!=null);

			form.fkt = this.fkt;
		}		

		#endregion

		#region IDisposable Member
		public virtual void Dispose()
		{
			this.form.Dispose();
			this.fkt = null;
		}
		#endregion
	}
}
