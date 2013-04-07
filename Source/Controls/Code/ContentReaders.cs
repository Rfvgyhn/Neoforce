////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: ContentReaders.cs                            //
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
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework.Content;
#if (!XBOX && !XBOX_FAKE)
using System.Windows.Forms;
#endif
#endregion

namespace TomShane.Neoforce.Controls
{
	/// <summary>
	/// Represents a Neoforce Layout file.
	/// </summary>
	public class LayoutXmlDocument : XmlDocument { }
	/// <summary>
	/// Represents a Neoforce Skin file.
	/// </summary>
	public class SkinXmlDocument : XmlDocument { }

	/// <summary>
	/// Reads a Skin document from binary format. (.xml | .skin) ???
	/// </summary>
	public class SkinReader : ContentTypeReader<SkinXmlDocument>
	{
		#region Read
		/// <summary>
		/// Reads a Skin file from binary format.
		/// </summary>
		/// <param name="input">Content reader used to read the skin file.</param>
		/// <param name="existingInstance">Existing instance to read stream data into.</param>
		/// <returns>Returns the loaded skin file.</returns>
		protected override SkinXmlDocument Read(ContentReader input, SkinXmlDocument existingInstance)
		{
			if (existingInstance == null)
			{
				SkinXmlDocument doc = new SkinXmlDocument();
				doc.LoadXml(input.ReadString());
				return doc;
			}
			else
			{
				existingInstance.LoadXml(input.ReadString());
			}

			return existingInstance;
		}
		#endregion
	}

	/// <summary>
	/// Reads a Layout document from binary format.
	/// </summary>
	public class LayoutReader : ContentTypeReader<LayoutXmlDocument>
	{
		#region Read
		/// <summary>
		/// Reads a Layout document from the current stream.
		/// </summary>
		/// <param name="input">Content reader used to read the Layout document.</param>
		/// <param name="existingInstance">Existing instance to read into.</param>
		/// <returns>Returns the Layout document from the stream.</returns>
		protected override LayoutXmlDocument Read(ContentReader input, LayoutXmlDocument existingInstance)
		{
			if (existingInstance == null)
			{
				LayoutXmlDocument doc = new LayoutXmlDocument();
				doc.LoadXml(input.ReadString());
				return doc;
			}
			else
			{
				existingInstance.LoadXml(input.ReadString());
			}

			return existingInstance;
		}
		#endregion
	}

#if (!XBOX && !XBOX_FAKE)
	/// <summary>
	/// Reads a cursor file from binary format.
	/// </summary>
	public class CursorReader : ContentTypeReader<Cursor>
	{
		#region Read
		/// <summary>
		/// Reads a cursor type from the current stream.
		/// </summary>
		/// <param name="input">Content reader used to read the cursor.</param>
		/// <param name="existingInstance">Existing cursor object to read into.</param>
		/// <returns>Returns the cursor object from the stream.</returns>
		protected override Cursor Read(ContentReader input, Cursor existingInstance)
		{
			if (existingInstance == null)
			{
				int count = input.ReadInt32();
				byte[] data = input.ReadBytes(count);

				string path = Path.GetTempFileName();
				File.WriteAllBytes(path, data);

				IntPtr handle = NativeMethods.LoadCursor(path);
				Cursor cur = new Cursor(handle);
				File.Delete(path);

				return cur;
			}
			else
			{
			}

			return existingInstance;
		}
		#endregion
	}
#endif

}

