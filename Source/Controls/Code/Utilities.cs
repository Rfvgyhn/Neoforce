////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: Utilities.cs                                 //
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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
#endregion

namespace TomShane.Neoforce.Controls
{
	/// <summary>
	/// Defines a few additional helper methods.
	/// </summary>
	static class Utilities
	{
		#region Derive Control Name
		/// <summary>
		/// Attempts to return the name of a control.
		/// </summary>
		/// <param name="control">Control to get the name of.</param>
		/// <returns>Returns the unqualified name of the specified control.</returns>
		public static string DeriveControlName(Control control)
		{
			if (control != null)
			{
				try
				{
					string str = control.ToString();
					int i = str.LastIndexOf(".");
					return str.Remove(0, i + 1);
				}
				catch
				{
					return control.ToString();
				}
			}
			return control.ToString();
		}
		#endregion

		#region Parse Color
		/// <summary>
		/// Parses a color value defined in XML. 
		/// </summary>
		/// <param name="str">";" delimited string of RGBA values.</param>
		/// <returns>Returns the string converted to a Color object.</returns>
		public static Color ParseColor(string str)
		{

			string[] val = str.Split(';');
			byte r = 255, g = 255, b = 255, a = 255;

			if (val.Length >= 1) r = byte.Parse(val[0]);
			if (val.Length >= 2) g = byte.Parse(val[1]);
			if (val.Length >= 3) b = byte.Parse(val[2]);
			if (val.Length >= 4) a = byte.Parse(val[3]);

			return Color.FromNonPremultiplied(r, g, b, a);
		}
		#endregion

		#region Parse Bevel Style
		/// <summary>
		/// Parses a BevelStyle enumeration defined in XML.
		/// </summary>
		/// <param name="str">Name of the bevel style value.</param>
		/// <returns>Returns the string converted to a BevelStyle object.</returns>
		public static BevelStyle ParseBevelStyle(string str)
		{
			return (BevelStyle)Enum.Parse(typeof(BevelStyle), str, true);
		}
		#endregion
	}
}
