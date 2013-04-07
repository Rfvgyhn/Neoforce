////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: KeyboardLayout.cs                            //
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
using Microsoft.Xna.Framework.Input;
using System.Globalization;
using System.Collections.Generic;
using System;
using System.Text;
#endregion

namespace TomShane.Neoforce.Controls
{
	/// <summary>
	/// Represents the layout of an English keyboard and helps to map
	/// Microsoft.Xna.Framework.Input.Keys values to their proper 
	/// string representations.
	/// </summary>
	public class KeyboardLayout
	{
		#region Fields
		/// <summary>
		/// Defines the type of keyboard layout.
		/// </summary>
		private string name = "English";
		/// <summary>
		/// ???
		/// </summary>
		public List<int> LayoutList = new List<int>();
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the name of the keyboard layout.
		/// </summary>
		public virtual string Name
		{
			get { return name; }
			set { name = value; }
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new English KeyboardLayout object.
		/// </summary>
		public KeyboardLayout()
		{
			LayoutList.Add(1033);
		}
		#endregion

		#region Get Key
		/// <summary>
		/// Gets the key value from a KeyEventArgs object case-adjusted based on modifiers.
		/// </summary>
		/// <param name="args">KeyEventArgs object to retrieve the key value from.</param>
		/// <returns>Returns the pressed key as a string.</returns>
		public virtual string GetKey(KeyEventArgs args)
		{
			string ret = "";

			// Check for Caps Lock and Shift modifiers.
			if (args.Caps && !args.Shift)
			{
				ret = KeyToString(args).ToUpper();
			}

			else if (!args.Caps && args.Shift)
			{
				ret = KeyToString(args).ToUpper();
			}

			else if (args.Caps && args.Shift)
			{
				ret = KeyToString(args).ToLower();
			}

			else if (!args.Caps && !args.Shift)
			{
				ret = KeyToString(args).ToLower();
			}

			return ret;
		}
		#endregion

		#region Key To String
		/// <summary>
		/// Maps Keys objects to their respective keys.
		/// </summary>
		/// <param name="args">KeyEventArgs to get the key value from.</param>
		/// <returns>Returns the mapped Keys value as a string.</returns>
		protected virtual string KeyToString(KeyEventArgs args)
		{
			switch (args.Key)
			{
				case Keys.A:
					return "a";
				case Keys.B:
					return "b";
				case Keys.C:
					return "c";
				case Keys.D:
					return "d";
				case Keys.E:
					return "e";
				case Keys.F:
					return "f";
				case Keys.G:
					return "g";
				case Keys.H:
					return "h";
				case Keys.I:
					return "i";
				case Keys.J:
					return "j";
				case Keys.K:
					return "k";
				case Keys.L:
					return "l";
				case Keys.M:
					return "m";
				case Keys.N:
					return "n";
				case Keys.O:
					return "o";
				case Keys.P:
					return "p";
				case Keys.Q:
					return "q";
				case Keys.R:
					return "r";
				case Keys.S:
					return "s";
				case Keys.T:
					return "t";
				case Keys.U:
					return "u";
				case Keys.V:
					return "v";
				case Keys.W:
					return "w";
				case Keys.X:
					return "x";
				case Keys.Y:
					return "y";
				case Keys.Z:
					return "z";

				case Keys.D0:
					return (args.Shift) ? ")" : "0";
				case Keys.D1:
					return (args.Shift) ? "!" : "1";
				case Keys.D2:
					return (args.Shift) ? "@" : "2";
				case Keys.D3:
					return (args.Shift) ? "#" : "3";
				case Keys.D4:
					return (args.Shift) ? "$" : "4";
				case Keys.D5:
					return (args.Shift) ? "%" : "5";
				case Keys.D6:
					return (args.Shift) ? "^" : "6";
				case Keys.D7:
					return (args.Shift) ? "&" : "7";
				case Keys.D8:
					return (args.Shift) ? "*" : "8";
				case Keys.D9:
					return (args.Shift) ? "(" : "9";

				case Keys.OemPlus:
					return (args.Shift) ? "+" : "=";
				case Keys.OemMinus:
					return (args.Shift) ? "_" : "-";
				case Keys.OemOpenBrackets:
					return (args.Shift) ? "{" : "[";
				case Keys.OemCloseBrackets:
					return (args.Shift) ? "}" : "]";
				case Keys.OemQuestion:
					return (args.Shift) ? "?" : "/";
				case Keys.OemPeriod:
					return (args.Shift) ? ">" : ".";
				case Keys.OemComma:
					return (args.Shift) ? "<" : ",";
				case Keys.OemPipe:
					return (args.Shift) ? "|" : "\\";
				case Keys.Space:
					return " ";
				case Keys.OemSemicolon:
					return (args.Shift) ? ":" : ";";
				case Keys.OemQuotes:
					return (args.Shift) ? "\"" : "'";
				case Keys.OemTilde:
					return (args.Shift) ? "~" : "`";

				case Keys.NumPad0:
					return (args.Shift) ? "" : "0";
				case Keys.NumPad1:
					return (args.Shift) ? "" : "1";
				case Keys.NumPad2:
					return (args.Shift) ? "" : "2";
				case Keys.NumPad3:
					return (args.Shift) ? "" : "3";
				case Keys.NumPad4:
					return (args.Shift) ? "" : "4";
				case Keys.NumPad5:
					return (args.Shift) ? "" : "5";
				case Keys.NumPad6:
					return (args.Shift) ? "" : "6";
				case Keys.NumPad7:
					return (args.Shift) ? "" : "7";
				case Keys.NumPad8:
					return (args.Shift) ? "" : "8";
				case Keys.NumPad9:
					return (args.Shift) ? "" : "9";
				case Keys.Decimal:
					return (args.Shift) ? "" : ".";

				case Keys.Divide:
					return (args.Shift) ? "/" : "/";
				case Keys.Multiply:
					return (args.Shift) ? "*" : "*";
				case Keys.Subtract:
					return (args.Shift) ? "-" : "-";
				case Keys.Add:
					return (args.Shift) ? "+" : "+";

				default:
					return "";
			}
		}
		#endregion

	}

	/// <summary>
	/// Represents the layout of a Czech keyboard and helps to map
	/// Microsoft.Xna.Framework.Input.Keys values to their proper 
	/// string representations.
	/// </summary>
	public class CzechKeyboardLayout : KeyboardLayout
	{
		#region Constructors
		/// <summary>
		/// Creates a new instance of the CzechKeyboardLayout class.
		/// </summary>
		public CzechKeyboardLayout()
		{
			Name = "Czech";
			LayoutList.Clear();
			LayoutList.Add(1029);
		}
		#endregion

		#region Key To String
		/// <summary>
		/// Maps Keys objects to their respective keys.
		/// </summary>
		/// <param name="args">KeyEventArgs to get the key value from.</param>
		/// <returns>Returns the mapped Keys value as a string.</returns>
		protected override string KeyToString(KeyEventArgs args)
		{
			switch (args.Key)
			{
				case Keys.D0:
					return (args.Shift) ? "0" : "È";
				case Keys.D1:
					return (args.Shift) ? "1" : "+";
				case Keys.D2:
					return (args.Shift) ? "2" : "Ï";
				case Keys.D3:
					return (args.Shift) ? "3" : "ö";
				case Keys.D4:
					return (args.Shift) ? "4" : "Ë";
				case Keys.D5:
					return (args.Shift) ? "5" : "¯";
				case Keys.D6:
					return (args.Shift) ? "6" : "û";
				case Keys.D7:
					return (args.Shift) ? "7" : "˝";
				case Keys.D8:
					return (args.Shift) ? "8" : "·";
				case Keys.D9:
					return (args.Shift) ? "9" : "Ì";

				case Keys.OemPlus:
					return (args.Shift) ? "°" : "¥";
				case Keys.OemMinus:
					return (args.Shift) ? "%" : "=";
				case Keys.OemOpenBrackets:
					return (args.Shift) ? "/" : "˙";
				case Keys.OemCloseBrackets:
					return (args.Shift) ? "(" : ")";
				case Keys.OemQuestion:
					return (args.Shift) ? "_" : "-";
				case Keys.OemPeriod:
					return (args.Shift) ? ":" : ".";
				case Keys.OemComma:
					return (args.Shift) ? "?" : ",";
				case Keys.OemPipe:
					return (args.Shift) ? "'" : "®";
				case Keys.Space:
					return " ";
				case Keys.OemSemicolon:
					return (args.Shift) ? "\"" : "˘";
				case Keys.OemQuotes:
					return (args.Shift) ? "!" : "ß";
				case Keys.OemTilde:
					return (args.Shift) ? "∞" : ";";

				case Keys.Decimal:
					return (args.Shift) ? "" : ",";

				default:
					return base.KeyToString(args);
			}
		}
		#endregion
	}

	/// <summary>
	/// Represents the layout of a German keyboard and helps to map
	/// Microsoft.Xna.Framework.Input.Keys values to their proper 
	/// string representations.
	/// </summary>
	public class GermanKeyboardLayout : KeyboardLayout
	{
		#region Constructors 
		/// <summary>
		/// Creates a new instance of the GermanKeyboardLayout class.
		/// </summary>
		public GermanKeyboardLayout()
		{
			Name = "German";
			LayoutList.Clear();
			LayoutList.Add(1031);
		}
		#endregion

		#region Key To String
		/// <summary>
		/// Maps Keys objects to their respective keys.
		/// </summary>
		/// <param name="args">KeyEventArgs to get the key value from.</param>
		/// <returns>Returns the mapped Keys value as a string.</returns>
		protected override string KeyToString(KeyEventArgs args)
		{
			switch (args.Key)
			{
				case Keys.D0:
					return (args.Shift) ? "=" : "0";
				case Keys.D1:
					return (args.Shift) ? "!" : "1";
				case Keys.D2:
					return (args.Shift) ? "\"" : "2";
				case Keys.D3:
					return (args.Shift) ? "ß" : "3";
				case Keys.D4:
					return (args.Shift) ? "$" : "4";
				case Keys.D5:
					return (args.Shift) ? "%" : "5";
				case Keys.D6:
					return (args.Shift) ? "&" : "6";
				case Keys.D7:
					return (args.Shift) ? "/" : "7";
				case Keys.D8:
					return (args.Shift) ? "*" : "8";
				case Keys.D9:
					return (args.Shift) ? "(" : "9";

				case Keys.OemPlus:
					return (args.Shift) ? "*" : "+";
				case Keys.OemMinus:
					return (args.Shift) ? "_" : "-";
				case Keys.OemOpenBrackets:
					return (args.Shift) ? "?" : "ﬂ";
				case Keys.OemCloseBrackets:
					return (args.Shift) ? "`" : "¥";
				case Keys.OemQuestion:
					return (args.Shift) ? "'" : "#";
				case Keys.OemPeriod:
					return (args.Shift) ? ":" : ".";
				case Keys.OemComma:
					return (args.Shift) ? ";" : ",";
				case Keys.OemPipe:
					return (args.Shift) ? "∞" : "^";
				case Keys.Space:
					return " ";
				case Keys.OemSemicolon:
					return (args.Shift) ? "‹" : "¸";
				case Keys.OemQuotes:
					return (args.Shift) ? "ƒ" : "‰";
				case Keys.OemTilde:
					return (args.Shift) ? "÷" : "ˆ";

				case Keys.Decimal:
					return (args.Shift) ? "" : ".";

				default:
					return base.KeyToString(args);
			}
		}
		#endregion
	}

	/// <summary>
	/// Represents the layout of a Polish keyboard and helps to map
	/// Microsoft.Xna.Framework.Input.Keys values to their proper 
	/// string representations.
	/// </summary>
	public class PolishKeyboardLayout : KeyboardLayout
	{
		#region Constructors
		/// <summary>
		/// Creates a new instance of the PolishKeyboardLayout class.
		/// </summary>
		public PolishKeyboardLayout()
		{
			Name = "Polish";
			LayoutList.Clear();
			LayoutList.Add(1045);
		}
		#endregion

		#region Key To String
		/// <summary>
		/// Maps Keys objects to their respective keys.
		/// </summary>
		/// <param name="args">KeyEventArgs to get the key value from.</param>
		/// <returns>Returns the mapped Keys value as a string.</returns>
		protected override string KeyToString(KeyEventArgs args)
		{
			if (args.Alt)
			{
				switch (args.Key)
				{
					case Keys.A:
						return (args.Shift) ? "•" : "π";
					case Keys.C:
						return (args.Shift) ? "∆" : "Ê";
					case Keys.E:
						return (args.Shift) ? " " : "Í";
					case Keys.L:
						return (args.Shift) ? "£" : "≥";
					case Keys.N:
						return (args.Shift) ? "—" : "Ò";
					case Keys.O:
						return (args.Shift) ? "”" : "Û";
					case Keys.S:
						return (args.Shift) ? "å" : "ú";
					case Keys.X:
						return (args.Shift) ? "è" : "ü";
					case Keys.Z:
						return (args.Shift) ? "Ø" : "ø";
				}
			}
			return base.KeyToString(args);
		}
		#endregion
	}
}

