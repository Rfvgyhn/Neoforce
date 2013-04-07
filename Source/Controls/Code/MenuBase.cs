////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: MenuBase.cs                                  //
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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace TomShane.Neoforce.Controls
{
	/// <summary>
	/// Represents a single, selectable, menu entry.
	/// </summary>
	public class MenuItem : Unknown
	{
		#region Fields
		/// <summary>
		/// Text to display for this menu item.
		/// </summary>
		public string Text = "MenuItem";
		/// <summary>
		/// List of child menu items belonging to this menu item.
		/// </summary>
		public List<MenuItem> Items = new List<MenuItem>();
		/// <summary>
		/// Indicates if the menu item appears after a menu separator. ??? 
		/// </summary>
		public bool Separated = false;
		/// <summary>
		/// Image to display to the left of the menu item. 
		/// </summary>
		public Texture2D Image = null;
		/// <summary>
		/// Indicates if the menu item is able to be selected or not.
		/// </summary>
		public bool Enabled = true;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new default menu item.
		/// </summary>
		public MenuItem()
		{
		}

		/// <summary>
		/// Creates a new menu item with the specified text.
		/// </summary>
		/// <param name="text">Menu item's text representation.</param>
		public MenuItem(string text)
			: this()
		{
			Text = text;
		}

		/// <summary>
		/// Creates a new menu item with the specified text.
		/// </summary>
		/// <param name="text">Menu item's text representation.</param>
		/// <param name="separated">Indicates if the menu item appears after a menu separator and needs some extra padding. ???</param>
		public MenuItem(string text, bool separated)
			: this(text)
		{
			Separated = separated;
		}
		#endregion

		#region Events
		/// <summary>
		/// Occurs when the menu item is clicked.
		/// </summary>
		public event EventHandler Click;
		/// <summary>
		/// Occurs when the menu item is selected.
		/// </summary>
		public event EventHandler Selected;
		#endregion

		#region Click Invoke
		/// <summary>
		/// Raises the menu item click event.
		/// </summary>
		/// <param name="e"></param>
		internal void ClickInvoke(EventArgs e)
		{
			if (Click != null)
			{
				Click.Invoke(this, e);
			}
		}
		#endregion

		#region Selected Invoke
		/// <summary>
		/// Raises the menu item selected event.
		/// </summary>
		/// <param name="e"></param>
		internal void SelectedInvoke(EventArgs e)
		{
			if (Selected != null)
			{
				Selected.Invoke(this, e);
			}
		}
		#endregion
	}

	/// <summary>
	/// Base class for a menu control.
	/// </summary>
	public abstract class MenuBase : Control
	{
		#region Fields
		/// <summary>
		/// Selected menu item index.
		/// </summary>
		private int itemIndex = -1;
		/// <summary>
		/// List of menu items composing the menu.
		/// </summary>
		private List<MenuItem> items = new List<MenuItem>();
		/// <summary>
		/// Child menu of this menu.
		/// </summary>
		private MenuBase childMenu = null;
		/// <summary>
		/// Root menu of this menu.
		/// </summary>
		private MenuBase rootMenu = null;
		/// <summary>
		/// Parent menu of this menu.
		/// </summary>
		private MenuBase parentMenu = null;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the menu's selected menu item index.
		/// </summary>
		protected internal int ItemIndex { get { return itemIndex; } set { itemIndex = value; } }
		/// <summary>
		/// Gets or sets the menu's child menu.
		/// </summary>
		protected internal MenuBase ChildMenu { get { return childMenu; } set { childMenu = value; } }
		/// <summary>
		/// Gets or sets the menu's root menu.
		/// </summary>
		protected internal MenuBase RootMenu { get { return rootMenu; } set { rootMenu = value; } }
		/// <summary>
		/// Gets or sets the menu's parent menu.
		/// </summary>
		protected internal MenuBase ParentMenu { get { return parentMenu; } set { parentMenu = value; } }
		/// <summary>
		/// Gets the list of menu items that make up the menu.
		/// </summary>
		public List<MenuItem> Items { get { return items; } }
		#endregion

		#region Constructor
		/// <summary>
		/// Creates a new MenuBase object.
		/// </summary>
		/// <param name="manager">GUI manager for the menu base.</param>
		public MenuBase(Manager manager)
			: base(manager)
		{
			rootMenu = this;
		}
		#endregion
	}
}
