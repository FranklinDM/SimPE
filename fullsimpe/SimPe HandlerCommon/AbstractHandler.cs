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
using SimPe.Interfaces.Plugin.Internal;
using SimPe.Interfaces;
using SimPe.Interfaces.Files;

namespace SimPe.Interfaces.Plugin
{
	/// <summary>
	/// The Abstarct Handler implements some common Methods of the 
	/// SimPe.Interfaces.IPackedFileWrapper and SimPe.Interfaces.IPackedFileSaveExtension 
	/// Interface. This is the easiest Way to Implement a Plugin.
	/// </summary>
	public abstract class AbstractWrapper : IWrapper, IPackedFileWrapper, IPackedFileSaveExtension
	{
		protected bool processed;
		/// <summary>
		/// true if <see cref="ProcessData"/> was called once
		/// </summary>
		public bool Processed 
		{
			get { return processed; }
		}		

		/// <summary>
		/// Stores the FileDescriptor
		/// </summary>
		protected IPackedFileDescriptor pfd;

		/// <summary>
		/// Stores the Package
		/// </summary>
		protected IPackageFile package;

		/// <summary>
		/// Stores the UI Handler
		/// </summary>
		protected IPackedFileUI ui;

		/// <summary>
		/// Stores the Priority of this Wrapper
		/// </summary>
		private int priority;

		/// <summary>
		/// Stors Human readable Informations about the Wrapper
		/// </summary>
		private IWrapperInfo wrapperinfo;

		/// <summary>
		/// Called By ProcessData() when the File nedds to update its Data Storage (Attributes, not the UserData)
		/// </summary>
		/// <param name="data">The Data to process</param>
		protected abstract void Unserialize(System.IO.BinaryReader reader);

		/// <summary>
		/// Creates the default UI Handler Object
		/// </summary>
		/// <returns>the default UI Handler Object is needed when the UIHandler is set to null</returns>
		protected abstract IPackedFileUI CreateDefaultUIHandler();

		/// <summary>
		/// Called when the data Stored in the Wrappers Attributes must be written to a Stream 
		/// </summary>
		/// <param name="writer">The Stream the Data should be written to</param>
		/// <remarks>This implemenation wont save anything, you have to reimplement 
		/// this in your class. </remarks>
		protected virtual void Serialize(System.IO.BinaryWriter writer) 
		{
		}

		/// <summary>
		/// Creates a new Wrapper Infor Object
		/// </summary>
		/// <returns></returns>
		protected virtual IWrapperInfo CreateWrapperInfo() 
		{
			return new AbstractWrapperInfo(
				Localization.Manager.GetString("unnamed"),
				Localization.Manager.GetString("unknown"),
				base.ToString(),
				1
				); 
		}

		/// <summary>
		/// Creates a new Instance assigning this Wrapper to an UI Handler
		/// </summary>
		/// <param name="ui">The UiHandler</param>
		/*public AbstractWrapper (IPackedFileUI ui) 
		{
			this.ui = ui;
		}*/

		
		/// <summary>
		/// Creates a new Instance
		/// </summary>
		public AbstractWrapper () 
		{
			processed = false;
			changed = false;
			this.ui = null;
		}	
	
		private string ExceptionMessage(string msg)
		{
            if (msg == null) msg = "";
			msg += "\n\nPackage: ";
			if (this.Package != null)
				msg += this.Package.FileName;
			else 
				msg += "null";

			msg += "\nFile: ";
			if (this.FileDescriptor!=null) 
			{
				msg += this.FileDescriptor.ExceptionString;
			} 
			else 
				msg += "null";

			return msg;
		}
		
		private void ExceptionMessage(string msg, Exception ex)
		{
			msg = ExceptionMessage(msg);
			Helper.ExceptionMessage(msg, ex);
		}			

		#region IWrapper Member

		public void Register(IWrapperRegistry registry)
		{
			registry.Register((IWrapper)this);
		}

		public virtual bool CheckVersion(uint version) 
		{
			return false;
		}

		public IWrapperInfo WrapperDescription 
		{
			get 
			{ 
				if (wrapperinfo==null) wrapperinfo = this.CreateWrapperInfo();
				return wrapperinfo; 
			}
		}

		public override string ToString()
		{
			return WrapperDescription.Name + " (Author="
				+WrapperDescription.Author +", Version="
				+WrapperDescription.Version.ToString()+", GUID="
				+Helper.HexString(WrapperDescription.UID)+", FileName="
				+WrapperFileName+", Type="
				+this.GetType()+")";
		}

		public string WrapperFileName 
		{
			get 
			{
				string i = this.GetType().Assembly.FullName;
				string[] p = i.Split(",".ToCharArray(), 2);
				
				if (p.Length>0) return p[0].Trim();
				else return SimPe.Localization.GetString("unknown");
			}
		}


		#endregion

		#region IPackedFileSaveExtension Member

		public System.IO.MemoryStream CurrentStateData
		{
			get 
			{
				System.IO.MemoryStream ms = new System.IO.MemoryStream();
				Serialize(new System.IO.BinaryWriter(ms));
				return ms;
			}
		}

		public void SynchronizeUserData()
		{
			SynchronizeUserData(true);
		}

		public void SynchronizeUserData(bool catchex)
		{
			SynchronizeUserData(catchex, false);
		}
		
		public void SynchronizeUserData(bool catchex, bool fire)
		{
			if (pfd==null) 
			{
				changed = false;
				return;
			}

			if (catchex) 
			{
				try 
				{				
					//set UserData, but do not fire a change Event!
					pfd.SetUserData(CurrentStateData.ToArray(), fire);
					changed = false;
				}
				catch (Exception ex) 
				{
					ExceptionMessage(Localization.Manager.GetString("errwritingfile"), ex);				
				}
			} 
			else 
			{
				//set UserData, but do not fire a change Event!
				pfd.SetUserData(CurrentStateData.ToArray(), false);
				changed = false;
			}
		}

		public void Save(IPackedFileDescriptor pfd)
		{
			try 
			{
				System.IO.BinaryWriter bw = new System.IO.BinaryWriter(new System.IO.MemoryStream());
				int size = Save(bw);
				System.IO.BinaryReader br = new System.IO.BinaryReader(bw.BaseStream);
				br.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
				pfd.UserData = br.ReadBytes(size);
			}
			catch (Exception ex) 
			{
				ExceptionMessage(Localization.Manager.GetString("errwritingfile"), ex);
			}
		}

		public int Save(System.IO.BinaryWriter writer)
		{
			long pos = writer.BaseStream.Position;
			try 
			{
				Serialize(writer);
			} 
			catch (Exception ex) 
			{
				ExceptionMessage(Localization.Manager.GetString("errwritingfile"), ex);
			}
			return (int)(writer.BaseStream.Position - pos);
		}

		bool changed;
		public virtual bool Changed 
		{
			get { return changed; }
			set { changed = value; }
		}
		#endregion

		#region IPackedFileLoadExtension Member
		public virtual void Fix(IWrapperRegistry registry) 
		{
		}

		public IPackedFileDescriptor FileDescriptor 
		{
			get { return pfd; }
			set { 
				processed = false;
				pfd = value; 
			}
		}

		public IPackedFileUI UIHandler
		{
			get
			{ 
				if (ui==null) ui = CreateDefaultUIHandler();
				return ui; 
			}
			set { ui = value; }
		}

		public IPackageFile Package
		{
			get { return package; }
			set { 
				processed = false;
				package = value; 
			}
		}

		public void ProcessData(IPackedFileDescriptor pfd, IPackageFile package, IPackedFile file)
		{			
			ProcessData(pfd, package, file, true);	
		}

		public void ProcessData(Interfaces.Scenegraph.IScenegraphFileIndexItem item) 
		{
			ProcessData(item, true);
		}

		public void ProcessData(IPackedFileDescriptor pfd, IPackageFile package)
		{
			ProcessData(pfd, package, true);
		}

		public void ProcessData(IPackedFileDescriptor pfd, IPackageFile package, IPackedFile file, bool catchex)
		{			
			changed = false;
			if (pfd==null) return;
			if (package==null) return;
			if (catchex) 
			{
				try 
				{
					if (file!=null) 
					{
						this.pfd = pfd;
						this.package = package;
						file = package.Read(pfd);
						System.IO.MemoryStream ms = new System.IO.MemoryStream(file.UncompressedData);
						if (ms.Length>0) 
						{
							Unserialize(new System.IO.BinaryReader(ms));
							processed = true;
						}
					} 
					else 
					{
						ProcessData(pfd, package);
					}
				} 
				catch (Exception ex) 
				{

					ExceptionMessage(Localization.Manager.GetString("erropenfile"), ex);
				}
			} 
			else 
			{
				if (file!=null) 
				{
					this.pfd = pfd;
					this.package = package;
					file = package.Read(pfd);
					System.IO.MemoryStream ms = new System.IO.MemoryStream(file.UncompressedData);
					if (ms.Length>0) 
					{
						Unserialize(new System.IO.BinaryReader(ms));
						processed = true;
					}
				} 
				else 
				{
					ProcessData(pfd, package);
				}
			}
			
		}

		public void ProcessData(Interfaces.Scenegraph.IScenegraphFileIndexItem item, bool catchex) 
		{
			ProcessData(item.FileDescriptor, item.Package, catchex);
		}

		public void ProcessData(IPackedFileDescriptor pfd, IPackageFile package, bool catchex)
		{
			if (pfd==null) return;
			if (package==null) return;
#if !DEBUG
			if (catchex) 
			{
				try 
				{
					this.pfd = pfd;
					this.package = package;	
					if (StoredData.BaseStream.Length>0) 
					{
						Unserialize(StoredData);
						processed = true;
					}
				}
				catch (Exception ex) 
				{

					ExceptionMessage(Localization.Manager.GetString("erropenfile"), ex);
					
				}
			} 
			else
#endif
            {
				this.pfd = pfd;
				this.package = package;	
				if (StoredData.BaseStream.Length>0) 
				{
					Unserialize(StoredData);
					processed = true;
				}
			}
		}

		public System.IO.BinaryReader StoredData 
		{
			get
			{
				if ((pfd!=null) && (package!=null))
				{
					IPackedFile file = package.Read(pfd);
					return new System.IO.BinaryReader(new System.IO.MemoryStream(file.UncompressedData));
                    
                    /*IPackedFile file = package.GetStream(pfd);
                    return new System.IO.BinaryReader(file.UncompressedStream);*/
				} 
				else 
				{
					return new System.IO.BinaryReader(new System.IO.MemoryStream());
				}
			}
		}

		public virtual bool AllowMultipleInstances
		{
			get 
			{
				return (GetType().GetInterface("SimPe.Interfaces.Plugin.IMultiplePackedFileWrapper", false)==typeof(SimPe.Interfaces.Plugin.IMultiplePackedFileWrapper));
			}
		}

        public virtual bool ReferencesResources
        {
            get
            {
                return (GetType().GetInterface("SimPe.Interfaces.Plugin.IWrapperReferencedResources", false) == typeof(SimPe.Interfaces.Plugin.IWrapperReferencedResources));
            }
        }
		

		public void RefreshUI() 
		{
			if (UIHandler!=null) 
			{
				UIHandler.UpdateGUI((IFileWrapper)this);
			}
		}
		public void Refresh() 
		{
			this.ProcessData(this.FileDescriptor, this.Package);
			RefreshUI();
		}

		public void LoadUI()
		{			
			 RefreshUI();
		}		

		/// <summary>
		/// The Priority of a Wrapper in the Registry
		/// </summary>
		public int Priority 
		{
			get { return priority; }
			set { priority = value;}
		}

		/// <summary>
		/// Get a Description for this Resource
		/// </summary>
		public virtual string Description
		{
			get
			{
				return "";
			}
		}

		/// <summary>
		/// Get the Header for this Description(i.e. Fieldnames)
		/// </summary>
		public virtual string DescriptionHeader
		{
			get
			{
				return "";
			}
		}

		/// <summary>
		/// Returns the 64 Character Long embedded Filename
		/// </summary>
		/// <param name="ta">The Current Type</param>
		/// <returns></returns>
		string GetEmbeddedFileName(Data.TypeAlias ta)
		{
			if (Package==null) return null;
			if (FileDescriptor==null) return null;

            if (ta.containsfilename)
            {
                SimPe.Interfaces.Files.IPackedFile pf = Package.Read(FileDescriptor);
                return Helper.ToString(pf.GetUncompressedData(0x40));                
            }
            else return null;
		}

		/// <summary>
		/// Override this to add your own Implementation for <see cref="ResourceName"/>
		/// </summary>
		/// <param name="ta">The Current Type</param>
		/// <returns>null, if the Default Name should be generated</returns>
		protected virtual string GetResourceName(Data.TypeAlias ta)
		{
			return null;
		}

		/// <summary>
		/// Get this Resource
		/// </summary>
		public string ResourceName
		{
			get
			{		
				string res = null;
				if (FileDescriptor!=null) 
				{                    
                        Data.TypeAlias ta = Helper.TGILoader.GetByType(FileDescriptor.Type);
                                  
                        if (Helper.WindowsRegistry.ResourceListFormat == Registry.ResourceListFormats.JustLongType) {res  = ta.Name; if (res=="") res = null; }
                        else res = GetResourceName(ta);
                        if (res == null) res = GetEmbeddedFileName(ta);
                        if (res == null) res = FileDescriptor.ToResListString();
                        else if (ta.Name == null) res = SimPe.Localization.GetString("Unknown") + ": " + res;
                        else
                        {
                            
                            if (Helper.WindowsRegistry.ResourceListFormat == Registry.ResourceListFormats.LongTypeNames)
                                res = ta.Name + ": " + res;
                            else if (Helper.WindowsRegistry.ResourceListFormat == Registry.ResourceListFormats.ShortTypeNames)
                                res = ta.shortname + ": " + res;
                            else if (res.Trim() == "")
                                res = "[" + ta.Name + "]";
                        }
				} 
				else 
				{
					res = SimPe.Localization.GetString("Unknown");
				}
				 
				return res;
			}
		}

		/// <summary>
		/// Return the content of the Files
		/// </summary>
		public virtual System.IO.MemoryStream Content
		{
			get
			{
				return null;
			}
		}

		/// <summary>
		/// Returns the default Extension for this File
		/// </summary>
		public virtual string FileExtension
		{
			get
			{
				return Data.MetaData.FindTypeAlias(this.FileDescriptor.Type).Extension;
			}
		}
		#endregion

		IFileWrapper guiwrapper;
		/// <summary>
		/// This is used to initialize single Gui Wrappers, in 
		/// order to not corrupted when ResourceName is called
		/// </summary>
		public IFileWrapper SingleGuiWrapper
		{
			set { guiwrapper = value; }
		}

		#region IMultiplePackedFileWrapper Member
		

		/// <summary>
		/// Create a new Instance of the Wrapper
		/// </summary>
		/// <returns>the new Instance</returns>
		public virtual IFileWrapper Activate()
		{
			if (!this.AllowMultipleInstances) 
			{
				if (guiwrapper==null) guiwrapper = (IFileWrapper)Activator.CreateInstance(this.GetType());
				return guiwrapper;
			} 
			else 
			{
				SimPe.Interfaces.Plugin.IMultiplePackedFileWrapper me = (SimPe.Interfaces.Plugin.IMultiplePackedFileWrapper)this;
				return (IFileWrapper)Activator.CreateInstance(this.GetType(), me.GetConstructorArguments());
			}
		}

		/// <summary>
		/// Returns a list of Arguments that should be passed to the Constructor during <see cref="Activate"/>.
		/// </summary>
		/// <returns></returns>
		public virtual object[] GetConstructorArguments()
		{
			return new object[0];
		}
		#endregion		

		#region IDisposable Member
		public virtual void Dispose()
		{
			if (wrapperinfo!=null) wrapperinfo.Dispose();
			if (ui!=null) ui.Dispose();
			ui = null;
			package = null;
			pfd = null;
		}
		#endregion
	}
}
