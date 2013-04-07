////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: TabControl.cs                                //
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
#endregion

namespace TomShane.Neoforce.Controls
{
	/// <summary>
	/// Provides tab navigation support for gamepads.
	/// </summary>
	public class TabControlGamePadActions : GamePadActions
	{
		/// <summary>
		/// Button used to switch to the next tab. (RightTrigger)
		/// </summary>
		public GamePadButton NextTab = GamePadButton.RightTrigger;
		/// <summary>
		/// Button used to switch to the previous tab. (LeftTrigger)
		/// </summary>
		public GamePadButton PrevTab = GamePadButton.LeftTrigger;
	}

	/// <summary>
	/// Represents a single page of a tab control.
	/// </summary>
	public class TabPage : Control
	{
		#region Fields
		/// <summary>
		/// Defines the header region of the tab page.
		/// </summary>
		private Rectangle headerRect = Rectangle.Empty;
		#endregion

		#region Properties
		/// <summary>
		/// Gets the header region of the tab page.
		/// </summary>
		protected internal Rectangle HeaderRect
		{
			get { return headerRect; }
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new TabPage control.
		/// </summary>
		/// <param name="manager">GUI manager for the control.</param>
		public TabPage(Manager manager)
			: base(manager)
		{
			Color = Color.Transparent;
			Passive = true;
			CanFocus = false;
		}
		#endregion

		#region Calc Rect
		/// <summary>
		/// Calculates the region where the tab page header will be displayed.
		/// </summary>
		/// <param name="prev">Header region of the previous tab page control.</param>
		/// <param name="font">Font used to draw the header text.</param>
		/// <param name="margins">Tab header content margins.</param>
		/// <param name="offset">Offset to apply from previous tab.</param>
		/// <param name="first">Indicates if this is the first tab page header.</param>
		protected internal void CalcRect(Rectangle prev, SpriteFont font, Margins margins, Point offset, bool first)
		{
			int size = (int)Math.Ceiling(font.MeasureString(Text).X) + margins.Horizontal;

			// First header shouldn't have an offset.
			if (first)
			{
				offset.X = 0;
			}
			
			// Set the header region.
			headerRect = new Rectangle(prev.Right + offset.X, prev.Top, size, prev.Height);
		}
		#endregion
	}

	/// <summary>
	/// Represents a control containing one or more tab pages.
	/// </summary>
	public class TabControl : Container
	{
		#region Fields
		/// <summary>
		/// List of tab pages that make up the tab control.
		/// </summary>
		private List<TabPage> tabPages = new List<TabPage>();
		/// <summary>
		/// Index of the selected tab page.
		/// </summary>
		private int selectedIndex = 0;
		/// <summary>
		/// Index of the tab page header hovered by the mouse, if any.
		/// </summary>
		private int hoveredIndex = -1;
		#endregion

		#region Properties
		/// <summary>
		/// Returns the list of tab pages belonging to the tab control as an array.
		/// </summary>
		public TabPage[] TabPages
		{
			get { return tabPages.ToArray(); }
		}

		/// <summary>
		/// Gets or sets the index of the selected tab page.
		/// </summary>
		public virtual int SelectedIndex
		{
			get { return selectedIndex; }
			set
			{
				if (selectedIndex >= 0 && selectedIndex < tabPages.Count && value >= 0 && value < tabPages.Count)
				{
					TabPages[selectedIndex].Visible = false;
				}
				if (value >= 0 && value < tabPages.Count)
				{
					TabPages[value].Visible = true;
					ControlsList c = TabPages[value].Controls as ControlsList;
					if (c.Count > 0) c[0].Focused = true;
					selectedIndex = value;
					if (!Suspended) OnPageChanged(new EventArgs());
				}
			}
		}

		/// <summary>
		/// Gets or sets the selected tab page.
		/// </summary>
		public virtual TabPage SelectedPage
		{
			get { return tabPages[SelectedIndex]; }
			set
			{
				for (int i = 0; i < tabPages.Count; i++)
				{
					if (tabPages[i] == value)
					{
						SelectedIndex = i;
						break;
					}
				}
			}
		}
		#endregion

		#region Events
		/// <summary>
		/// Occurs when the selected tab page changes.
		/// </summary>
		public event EventHandler PageChanged;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new TabControl.
		/// </summary>
		/// <param name="manager">GUI manager for the control.</param>
		public TabControl(Manager manager)
			: base(manager)
		{
			GamePadActions = new TabControlGamePadActions();
			Manager.Input.GamePadDown += new GamePadEventHandler(Input_GamePadDown);
			this.CanFocus = false;
		}
		#endregion

		#region Init
		/// <summary>
		/// Initializes the tab control.
		/// </summary>
		public override void Init()
		{
			base.Init();
		}
		#endregion

		#region Init Skin
		/// <summary>
		/// Initializes the skin of the tab control.
		/// </summary>
		protected internal override void InitSkin()
		{
			base.InitSkin();
		}
		#endregion

		#region Draw Control
		/// <summary>
		/// Draws the tab control.
		/// </summary>
		/// <param name="renderer">Render management object.</param>
		/// <param name="rect">Destination region where the tab control will be drawn.</param>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		protected override void DrawControl(Renderer renderer, Rectangle rect, GameTime gameTime)
		{
			SkinLayer l1 = Skin.Layers["Control"];
			SkinLayer l2 = Skin.Layers["Header"];
			Color col = this.Color != UndefinedColor ? this.Color : Color.White;

			Rectangle r1 = new Rectangle(rect.Left, rect.Top + l1.OffsetY, rect.Width, rect.Height - l1.OffsetY);
			
			if (tabPages.Count <= 0)
			{
				r1 = rect;
			}

			base.DrawControl(renderer, r1, gameTime);

			// Has tab pages to draw?
			if (tabPages.Count > 0)
			{
				Rectangle prev = new Rectangle(rect.Left, rect.Top + l2.OffsetY, 0, l2.Height);
				
				for (int i = 0; i < tabPages.Count; i++)
				{
					SpriteFont font = l2.Text.Font.Resource;
					Margins margins = l2.ContentMargins;
					Point offset = new Point(l2.OffsetX, l2.OffsetY);
					if (i > 0) prev = tabPages[i - 1].HeaderRect;

					// Create the header region rectangle for each tab page.
					tabPages[i].CalcRect(prev, font, margins, offset, i == 0);
				}


				for (int i = tabPages.Count - 1; i >= 0; i--)
				{
					// Get the layer color and index for the current tab page.
					int li = tabPages[i].Enabled ? l2.States.Enabled.Index : l2.States.Disabled.Index;
					Color lc = tabPages[i].Enabled ? l2.Text.Colors.Enabled : l2.Text.Colors.Disabled;
					
					// Is the current tab page header hovered?
					if (i == hoveredIndex)
					{
						// Update index and color values.
						li = l2.States.Hovered.Index;
						lc = l2.Text.Colors.Hovered;
					}

					// Calculate the region where text is displayed in the header, respecting content margin values.
					Margins m = l2.ContentMargins;
					Rectangle rx = tabPages[i].HeaderRect;
					Rectangle sx = new Rectangle(rx.Left + m.Left, rx.Top + m.Top, rx.Width - m.Horizontal, rx.Height - m.Vertical);

					// Draw the header for the unselected tab pages.
					if (i != selectedIndex)
					{
						renderer.DrawLayer(l2, rx, col, li);
						renderer.DrawString(l2.Text.Font.Resource, tabPages[i].Text, sx, lc, l2.Text.Alignment);
					}
				}

				// Calculate the region where text is displayed in the header, respecting content margin values.
				Margins mi = l2.ContentMargins;
				Rectangle ri = tabPages[selectedIndex].HeaderRect;
				Rectangle si = new Rectangle(ri.Left + mi.Left, ri.Top + mi.Top, ri.Width - mi.Horizontal, ri.Height - mi.Vertical);

				// Draw the header for the selected tab page.
				renderer.DrawLayer(l2, ri, col, l2.States.Focused.Index);
				renderer.DrawString(l2.Text.Font.Resource, tabPages[selectedIndex].Text, si, l2.Text.Colors.Focused, l2.Text.Alignment, l2.Text.OffsetX, l2.Text.OffsetY, false);
			}
		}
		#endregion

		#region Add Page
		/// <summary>
		/// Creates a tab page with the specified header text and adds it to the tab control.
		/// </summary>
		/// <param name="text">Tab page header text.</param>
		/// <returns>Returns the created tab page.</returns>
		public virtual TabPage AddPage(string text)
		{
			TabPage p = AddPage();
			p.Text = text;

			return p;
		}

		/// <summary>
		/// Creates a tab page with the default header text and adds it to the tab control.
		/// </summary>
		/// <returns>Returns the created tab page.</returns>
		public virtual TabPage AddPage()
		{
			TabPage page = new TabPage(Manager);
			page.Init();
			page.Left = 0;
			page.Top = 0;
			page.Width = ClientWidth;
			page.Height = ClientHeight;
			page.Anchor = Anchors.All;
			page.Text = "Tab " + (tabPages.Count + 1).ToString();
			page.Visible = false;
			Add(page, true);
			tabPages.Add(page);
			tabPages[0].Visible = true;

			return page;
		}
		#endregion

		#region Remove Page
		/// <summary>
		/// Removes the specified tab page from the tab control and disposes it (if specified.)
		/// </summary>
		/// <param name="page">Tab page to remove from the tab control.</param>
		/// <param name="dispose">Indicates if the tab page should be disposed after removal.</param>
		public virtual void RemovePage(TabPage page, bool dispose)
		{
			tabPages.Remove(page);
			if (dispose)
			{
				page.Dispose();
				page = null;
			}
			SelectedIndex = 0;
		}
		#endregion

		#region Remove Page
		/// <summary>
		/// Removes the specified tab page from the control.
		/// </summary>
		/// <param name="page">Tab page to remove from the tab control.</param>
		public virtual void RemovePage(TabPage page)
		{
			RemovePage(page, true);
		}
		#endregion

		#region On Mouse Down Event Handler
		/// <summary>
		/// Handles mouse down events for the tab control.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			// More than one tab page?
			if (tabPages.Count > 1)
			{
				// Convert mouse position to relative offset.
				Point p = new Point(e.State.X - Root.AbsoluteLeft, e.State.Y - Root.AbsoluteTop);
				
				// See if any of the tab page headers were clicked.
				for (int i = 0; i < tabPages.Count; i++)
				{
					Rectangle r = tabPages[i].HeaderRect;

					// Select page if mouse position is within header.
					if (p.X >= r.Left && p.X <= r.Right && p.Y >= r.Top && p.Y <= r.Bottom)
					{
						SelectedIndex = i;
						break;
					}
				}
			}
		}
		#endregion

		#region On Mouse Move Event Handler
		/// <summary>
		/// Handles mouse move events for the tab control.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			// More than one tab page control created?
			if (tabPages.Count > 1)
			{
				int index = hoveredIndex;

				// Get mouse position as a relative offset from the tab control.
				Point p = new Point(e.State.X - Root.AbsoluteLeft, e.State.Y - Root.AbsoluteTop); 
				
				// Determine if the mouse is hovering any of the tab page headers.
				for (int i = 0; i < tabPages.Count; i++)
				{
					Rectangle r = tabPages[i].HeaderRect;

					// Mouse is within the current header region?
					if (p.X >= r.Left && p.X <= r.Right && p.Y >= r.Top && p.Y <= r.Bottom && tabPages[i].Enabled)
					{
						index = i;
						break;
					}

					else
					{
						index = -1;
					}
				}

				// Update the hovered tab page header index?
				if (index != hoveredIndex)
				{
					hoveredIndex = index;
					Invalidate();
				}
			}
		}
		#endregion

		#region Game Pad Down Event Handler
		/// <summary>
		/// Handles gamepad input for the tab control.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Input_GamePadDown(object sender, GamePadEventArgs e)
		{
			// Tab control has focus?
			if (this.Contains(Manager.FocusedControl, true))
			{
				// Switch to the next tab page on RightTrigger presses.
				if (e.Button == (GamePadActions as TabControlGamePadActions).NextTab)
				{
					e.Handled = true;
					SelectedIndex += 1;
				}

				// Switch to the previous tab page on LeftTrigger presses.
				else if (e.Button == (GamePadActions as TabControlGamePadActions).PrevTab)
				{
					e.Handled = true;
					SelectedIndex -= 1;
				}
			}
		}
		#endregion

		#region On Page Changed Event Handler
		/// <summary>
		/// Handler for when a new tab page is selected.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnPageChanged(EventArgs e)
		{
			if (PageChanged != null) PageChanged.Invoke(this, e);
		}
		#endregion
	}
}
