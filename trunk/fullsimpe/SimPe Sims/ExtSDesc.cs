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
using SimPe.Interfaces;
using SimPe.PackedFiles.Wrapper.Supporting;
using SimPe.Data;

namespace SimPe.PackedFiles.Wrapper
{
	/// <summary>
	/// Zusammenfassung f�r ExtSDesc.
	/// </summary>
	public class ExtSDesc : SDesc//, SimPe.Interfaces.Plugin.IMultiplePackedFileWrapper
	{
		public ExtSDesc() : base()
		{
			
		}

		
		protected override IWrapperInfo CreateWrapperInfo()
		{
			return new AbstractWrapperInfo(
				"Extended Sim Description Wrapper",
				"Quaxi",
				"This File contains Settings (like interests, friendships, money, age, gender...) for one Sim.",
				1,
				System.Drawing.Image.FromStream(this.GetType().Assembly.GetManifestResourceStream("SimPe.PackedFiles.Wrapper.sdsc.png"))				
				); 
		}

		protected override IPackedFileUI CreateDefaultUIHandler()
		{
			return new SimPe.PackedFiles.UserInterface.ExtSDesc();
		}

		bool chgname;
		string sname, sfname;
		public override string SimFamilyName
		{
			get
			{
				return base.SimFamilyName;
			}
			set
			{
				chgname = true;
				sname = value;
			}
		}

		public override string SimName
		{
			get
			{
				return base.SimName;
			}
			set 
			{
				chgname = true;
				sfname = value;
			}
		}

		protected override void Unserialize(System.IO.BinaryReader reader)
		{
			base.Unserialize(reader);
			chgname = false;
		}


		protected override void Serialize(System.IO.BinaryWriter writer)
		{
			base.Serialize (writer);
			if (chgname) ChangeName();
		}

		protected virtual void ChangeName()
		{
			chgname = false;			
		}

		public bool HasRelationWith(ExtSDesc sdsc)
		{
			
			foreach (uint inst in this.Relations.SimInstances) 
				if (sdsc.FileDescriptor.Instance==inst) 
					return true;
			return false;
		}
	}
}
