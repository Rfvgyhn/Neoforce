////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: Layout.cs                                    //
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
using Microsoft.Xna.Framework;
using System.Xml;
using System.Reflection;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TomShane.Neoforce.Controls
{
	/// <summary>
	/// Provides methods for loading Neoforce controls from XML files.
	/// </summary>
	public static class Layout
	{
		#region Load
		/// <summary>
		/// Reads the specified layout XML file asset.
		/// </summary>
		/// <param name="manager">GUI manager responsible for the controls contained in the layout XML file.</param>
		/// <param name="asset">Name of the layout XML asset. (Default asset names are file names without extensions.)</param>
		/// <returns>Returns the root control of the layout file with all child controls initialized.</returns>
		public static Container Load(Manager manager, string asset)
		{
			Container win = null;
			LayoutXmlDocument doc = new LayoutXmlDocument();
			ArchiveManager content = new ArchiveManager(manager.Game.Services);

			try
			{
				content.RootDirectory = manager.LayoutDirectory;

#if (!XBOX && !XBOX_FAKE)

				string file = content.RootDirectory + asset;

				if (File.Exists(file))
				{
					doc.Load(file);
				}
				else

#endif
				{
					doc = content.Load<LayoutXmlDocument>(asset);
				}


				if (doc != null && doc["Layout"]["Controls"] != null && doc["Layout"]["Controls"].HasChildNodes)
				{
					XmlNode node = doc["Layout"]["Controls"].GetElementsByTagName("Control").Item(0);
					string cls = node.Attributes["Class"].Value;
					Type type = Type.GetType(cls);

					if (type == null)
					{
						cls = "TomShane.Neoforce.Controls." + cls;
						type = Type.GetType(cls);
					}

					win = (Container)LoadControl(manager, node, type, null);
				}

			}
			finally
			{
				content.Dispose();
			}

			return win;
		}
		#endregion

		#region Load Control
		/// <summary>
		/// Loads a control from a layout XML file.
		/// </summary>
		/// <param name="manager">GUI manager for the control to load.</param>
		/// <param name="node">Control XML node.</param>
		/// <param name="type">Type of control specified in XML.</param>
		/// <param name="parent">Parent control of the control about to be loaded.</param>
		/// <returns>Returns the created Control.</returns>
		private static Control LoadControl(Manager manager, XmlNode node, Type type, Control parent)
		{
			Control c = null;

			Object[] args = new Object[] { manager };

			c = (Control)type.InvokeMember(null, BindingFlags.CreateInstance, null, null, args);
			if (parent != null) c.Parent = parent;
			c.Name = node.Attributes["Name"].Value;

			if (node != null && node["Properties"] != null && node["Properties"].HasChildNodes)
			{
				LoadProperties(node["Properties"].GetElementsByTagName("Property"), c);
			}

			if (node != null && node["Controls"] != null && node["Controls"].HasChildNodes)
			{
				foreach (XmlElement e in node["Controls"].GetElementsByTagName("Control"))
				{
					string cls = e.Attributes["Class"].Value;
					Type t = Type.GetType(cls);

					if (t == null)
					{
						cls = "TomShane.Neoforce.Controls." + cls;
						t = Type.GetType(cls);
					}
					LoadControl(manager, e, t, c);
				}
			}

			return c;
		}
		#endregion

		#region Load Properties
		/// <summary>
		/// Loads all properties defined in a layout XML file.
		/// </summary>
		/// <param name="node">Current XML node list.</param>
		/// <param name="c">Control to apply the property values to.</param>
		private static void LoadProperties(XmlNodeList node, Control c)
		{
			foreach (XmlElement e in node)
			{
				string name = e.Attributes["Name"].Value;
				string val = e.Attributes["Value"].Value;

				PropertyInfo i = c.GetType().GetProperty(name);

				if (i != null)
				{
					{
						try
						{
							i.SetValue(c, Convert.ChangeType(val, i.PropertyType, null), null);
						}
						catch
						{
						}
					}
				}
			}
		}
		#endregion
	}
}
