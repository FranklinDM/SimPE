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

namespace SimPe.Plugin
{
	/// <summary>
	/// Zusammenfassung f�r StandardLightBase.
	/// </summary>
	public class DirectionalLight
		: AbstractRcolBlock
	{
		#region Attributes

		StandardLightBase slb;		
		public StandardLightBase StandardLightBase
		{
			get { return slb; }
			set { slb = value; }
		}

		LightT lt;
		public LightT LightT
		{
			get { return lt; }
			set { lt = value; }
		}

		ReferentNode rn;
		public ReferentNode ReferentNode
		{
			get { return rn; }
			set { rn = value; }
		}

		ObjectGraphNode ogn;
		public ObjectGraphNode ObjectGraphNode 
		{
			get { return ogn; }
			set { ogn = value; }
		}


		string unknown2;
		public string Name 
		{
			get { return unknown2; }
			set { unknown2 = value; }
		}

		float unknown3;
		public float Val1 
		{
			get { return unknown3; }
			set { unknown3 = value; }
		}

		float unknown4;
		public float Val2 
		{
			get { return unknown4; }
			set { unknown4 = value; }
		}

		float red;
		public float Red 
		{
			get { return red; }
			set { red = value; }
		}

		float green;
		public float Green 
		{
			get { return green; }
			set { green = value; }
		}

		float blue;
		public float Blue 
		{
			get { return blue; }
			set { blue = value; }
		}


		#endregion
		

		/// <summary>
		/// Constructor
		/// </summary>
		public DirectionalLight(Rcol parent) : base(parent)
		{
			version = 1;
			BlockID = 0xC9C81BA3;

			slb = new StandardLightBase(null);
			sgres = new SGResource(null);
			lt = new LightT(null);
			rn = new ReferentNode(null);
			ogn = new ObjectGraphNode(null);

			unknown2 = "";
		}
		
		#region IRcolBlock Member

		/// <summary>
		/// Unserializes a BinaryStream into the Attributes of this Instance
		/// </summary>
		/// <param name="reader">The Stream that contains the FileData</param>
		public override void Unserialize(System.IO.BinaryReader reader)
		{
			version = reader.ReadUInt32();

			slb.BlockName = reader.ReadString();
			slb.BlockID = reader.ReadUInt32();			
			slb.Unserialize(reader);

			sgres.BlockName = reader.ReadString();
			sgres.BlockID = reader.ReadUInt32();			
			sgres.Unserialize(reader);

			lt.BlockName = reader.ReadString();
			lt.BlockID = reader.ReadUInt32();			
			lt.Unserialize(reader);

			rn.BlockName = reader.ReadString();
			rn.BlockID = reader.ReadUInt32();			
			rn.Unserialize(reader);

			ogn.BlockName = reader.ReadString();
			ogn.BlockID = reader.ReadUInt32();			
			ogn.Unserialize(reader);

			unknown2 = reader.ReadString();
			unknown3 = reader.ReadSingle();
			unknown4 = reader.ReadSingle();
			red = reader.ReadSingle();
			green = reader.ReadSingle();
			blue = reader.ReadSingle();
		}

		/// <summary>
		/// Serializes a the Attributes stored in this Instance to the BinaryStream
		/// </summary>
		/// <param name="writer">The Stream the Data should be stored to</param>
		/// <remarks>
		/// Be sure that the Position of the stream is Proper on 
		/// return (i.e. must point to the first Byte after your actual File)
		/// </remarks>
		public override void Serialize(System.IO.BinaryWriter writer)
		{
			writer.Write(version);

			writer.Write(slb.BlockName);
			writer.Write(slb.BlockID);
			slb.Serialize(writer);

			writer.Write(sgres.BlockName);
			writer.Write(sgres.BlockID);
			sgres.Serialize(writer);

			writer.Write(lt.BlockName);
			writer.Write(lt.BlockID);
			lt.Serialize(writer);

			writer.Write(rn.BlockName);
			writer.Write(rn.BlockID);
			rn.Serialize(writer);

			writer.Write(ogn.BlockName);
			writer.Write(ogn.BlockID);
			ogn.Serialize(writer);

			writer.Write(unknown2);
			writer.Write(unknown3);
			writer.Write(unknown4);
			writer.Write(red);
			writer.Write(green);
			writer.Write(blue);
		}

		protected TabPage.DirectionalLight tDirectionalLight;
		public override System.Windows.Forms.TabPage TabPage
		{
			get
			{
				if (tDirectionalLight==null) tDirectionalLight = new SimPe.Plugin.TabPage.DirectionalLight();
				return tDirectionalLight;
			}
		}
		#endregion

		/// <summary>
		/// You can use this to setop the Controls on a TabPage befor it is dispplayed
		/// </summary>
		protected override void InitTabPage() 
		{
			if (tDirectionalLight==null) tDirectionalLight = new SimPe.Plugin.TabPage.DirectionalLight();
			tDirectionalLight.tb_l_ver.Text = "0x"+Helper.HexString(this.version);
			tDirectionalLight.tb_l_name.Text = unknown2;

			tDirectionalLight.tb_l_1.Text = unknown3.ToString();
			tDirectionalLight.tb_l_2.Text = unknown4.ToString();
			tDirectionalLight.tb_l_3.Text = red.ToString();
			tDirectionalLight.tb_l_4.Text = green.ToString();
			tDirectionalLight.tb_l_5.Text = blue.ToString();

			tDirectionalLight.label39.Visible = false;
			tDirectionalLight.label44.Visible = false;
			tDirectionalLight.label45.Visible = false;
			tDirectionalLight.label46.Visible = false;

			tDirectionalLight.tb_l_6.Visible = false;
			tDirectionalLight.tb_l_7.Visible = false;
			tDirectionalLight.tb_l_8.Visible = false;
			tDirectionalLight.tb_l_9.Visible = false;
		}

		public override void ExtendTabControl(System.Windows.Forms.TabControl tc)
		{
			base.ExtendTabControl (tc);
			this.slb.AddToTabControl(tc);
			this.lt.AddToTabControl(tc);
			this.rn.AddToTabControl(tc);
			this.ogn.AddToTabControl(tc);
		}

		#region IDisposable Member

		public override void Dispose()
		{
			if (this.tDirectionalLight!=null) this.tDirectionalLight.Dispose();
			tDirectionalLight = null;
		}

		#endregion
	}
}
