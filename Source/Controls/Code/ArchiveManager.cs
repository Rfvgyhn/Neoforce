////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: ArchiveManager.cs                            //
//                                                            //
//      Version: 0.7                                          //
//                                                            //
//         Date: 11/09/2010                                   //
//                                                            //
//       Author: Tom Shane                                    //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//  Copyright (c) by Tom Shane                                //
//                                                            //
////////////////////////////////////////////////////////////////
#region Using
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Content;
using TomShane.Neoforce.External.Zip;
using System.Globalization;
#endregion

namespace TomShane.Neoforce.Controls
{
	/// <summary>
    /// Loads managed objects from the binary files produced by the design time 
    /// content pipeline the same way like <see cref="Microsoft.Xna.Framework.Content.ContentManager"/> does. 
    /// Additionally it is capable of loading assets from a zip file.
    /// </summary>
    /// <remarks>
    /// This class is based on Nick Gravelyn's EasyZip library.
    /// </remarks> 
	public class ArchiveManager : ContentManager
	{
		#region Fields
		/// <summary>
		/// Path to the archive file associated with the archive manager.
		/// </summary>
		private string archivePath = null;
		/// <summary>
		/// Archive file associated with the archive manager.
		/// </summary>
		private ZipFile archive = null;
		/// <summary>
		/// Indicates if an archive file was specified to read from or not. ???
		/// </summary>
		private bool useArchive = false;
		#endregion

		#region Properties
		/// <summary>
		/// Gets the path to the archive file associated with the manager.
		/// </summary>
		public virtual string ArchivePath
		{
			get { return archivePath; }
		}

		/// <summary>
		/// Indicates if an archive file was specified to read from or not. ???
		/// </summary>
		public bool UseArchive
		{
			get { return useArchive; }
			set { useArchive = value; }
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new instance of the archive manager class using the specified service provider.
		/// </summary>
		/// <param name="serviceProvider">Service provider used to locate services</param>
		public ArchiveManager(IServiceProvider serviceProvider) 
			: this(serviceProvider, null) 
		{
		}

		/// <summary>
		/// Creates a new instance of the archive manager class using the specified service provider.
		/// </summary>
		/// <param name="serviceProvider">Service provider used to locate serives.</param>
		/// <param name="archive">Path to the zipped skin file which will be used for reading 
		/// assets from. If this value is null, the archive manager works just like a ContentManager.</param>
		public ArchiveManager(IServiceProvider serviceProvider, string archive)
			: base(serviceProvider)
		{
			if (archive != null)
			{
				this.archive = ZipFile.Read(archive);
				archivePath = archive;
				useArchive = true;
			}
		}
		#endregion

		#region Open Stream
		/// <summary>
		/// Opens a stream for reading the specified asset contained inside the archive.
		/// </summary>
		/// <param name="assetName">Name of the asset to read.</param>
		/// <returns>Returns the opened stream.</returns>
		protected override Stream OpenStream(string assetName)
		{
			if (useArchive && archive != null)
			{
				assetName = assetName.Replace("\\", "/");
				if (assetName.StartsWith("/")) assetName = assetName.Remove(0, 1);

				string fullAssetName = (assetName + ".xnb").ToLower();

				foreach (ZipEntry entry in archive)
				{
					ZipDirEntry ze = new ZipDirEntry(entry);

					string entryName = entry.FileName.ToLower();

					if (entryName == fullAssetName)
					{
						return entry.GetStream();
					}
				}
				throw new Exception("Cannot find asset \"" + assetName + "\" in the archive.");
			}
			else
			{
				return base.OpenStream(assetName);
			}
		}
		#endregion

		#region Get Asset Names
		/// <summary>
		/// Gets the list of all assets contained inside of the archive.
		/// </summary>
		/// <returns>Returns an array of asset names contained in the archive. 
		/// (Full Path + Asset Name)</returns>
		public string[] GetAssetNames()
		{
			if (useArchive && archive != null)
			{
				List<string> filenames = new List<string>();

				foreach (ZipEntry entry in archive)
				{
					string name = entry.FileName;
					if (name.EndsWith(".xnb"))
					{
						name = name.Remove(name.Length - 4, 4);
						filenames.Add(name);
					}
				}
				return filenames.ToArray();
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Gets the list of all assets contained inside the archive in the specified directory.
		/// </summary>
		/// <param name="path">Directory in the archive to retrieve asset names from.</param>
		/// <returns>Returns an array of asset names contained in the specified directory.
		/// (Full Path + Asset Name)</returns>
		public string[] GetAssetNames(string path)
		{
			if (useArchive && archive != null)
			{
				if (path != null && path != "" && path != "\\" && path != "/")
				{
					List<string> filenames = new List<string>();

					foreach (ZipEntry entry in archive)
					{
						string name = entry.FileName;
						if (name.EndsWith(".xnb"))
						{
							name = name.Remove(name.Length - 4, 4);
						}

						string[] parts = name.Split('/');
						string dir = "";
						for (int i = 0; i < parts.Length - 1; i++)
						{
							dir += parts[i] + '/';
						}

						path = path.Replace("\\", "/");
						if (path.StartsWith("/")) path = path.Remove(0, 1);
						if (!path.EndsWith("/")) path += '/';

						if (dir.ToLower() == path.ToLower() && !name.EndsWith("/"))
						{
							filenames.Add(name);
						}
					}
					return filenames.ToArray();
				}
				else
				{
					return GetAssetNames();
				}
			}
			else
			{
				return null;
			}
		}
		#endregion

		#region Get File Stream
		/// <summary>
		/// Opens a stream for reading the specified file from the archive.
		/// </summary>
		/// <param name="filename">Name of the file to read.</param>
		/// <returns>Returns the opened stream.</returns>
		public Stream GetFileStream(string filename)
		{
			if (useArchive && archive != null)
			{
				filename = filename.Replace("\\", "/").ToLower();
				if (filename.StartsWith("/")) filename = filename.Remove(0, 1);

				foreach (ZipEntry entry in archive)
				{
					string entryName = entry.FileName.ToLower();

					if (entryName.Equals(filename))
						return entry.GetStream();
				}

				throw new Exception("Cannot find file \"" + filename + "\" in the archive.");
			}
			else
			{
				return null;
			}
		}
		#endregion

		#region Get Directories
		/// <summary>
		/// Gets the list of all directories contained in the archive.
		/// </summary>
		/// <param name="path">Directory to start searching from in the archive.</param>
		/// <returns>Returns an array of all directories under the specified path.</returns>
		public string[] GetDirectories(string path)
		{
			if (useArchive && archive != null)
			{
				if (path != null && path != "" && path != "\\" && path != "/")
				{
					List<string> dirs = new List<string>();

					path = path.Replace("\\", "/");
					if (path.StartsWith("/")) path = path.Remove(0, 1);
					if (!path.EndsWith("/")) path += '/';

					foreach (ZipEntry entry in archive)
					{
						string name = entry.FileName;
						if (name.ToLower().StartsWith(path.ToLower()))
						{
							int i = name.IndexOf("/", path.Length);
							string item = name.Substring(path.Length, i - path.Length) + "\\";
							if (!dirs.Contains(item))
							{
								dirs.Add(item);
							}
						}
					}
					return dirs.ToArray();
				}
				else
				{
					return GetAssetNames();
				}
			}
			else if (Directory.Exists(path))
			{
				string[] dirs = Directory.GetDirectories(path);

				for (int i = 0; i < dirs.Length; i++)
				{
					string[] parts = dirs[i].Split('\\');
					dirs[i] = parts[parts.Length - 1] + '\\';
				}

				return dirs;
			}
			else return null;
		}
		#endregion
	}
}
