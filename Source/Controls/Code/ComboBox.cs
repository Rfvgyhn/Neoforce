////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: ComboBox.cs                                  //
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
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TomShane.Neoforce.Controls
{
	/// <summary>
	/// Represents a combo box control with a drop-down list of selectable items.
	/// </summary>
	public class ComboBox : TextBox
	{
		#region Fields
		/// <summary>
		/// Drop-down button.
		/// </summary>
		private Button btnDown = null;
		/// <summary>
		/// List of selectable entries that compose the drop-down list.
		/// </summary>
		private List<object> items = new List<object>();
		/// <summary>
		/// List box control of the combo box.
		/// </summary>
		private ListBox lstCombo = null;
		/// <summary>
		/// Maximum number of entries in the combo box's drop-down list.
		/// </summary>
		private int maxItems = 5;
		/// <summary>
		/// Indicates whether the selected index should be drawn. ???
		/// </summary>
		private bool drawSelection = true;
		#endregion

		#region Properties
		/// <summary>
		/// Indicates if the combo box's text box control is read-only or not.
		/// </summary>
		public override bool ReadOnly
		{
			get { return base.ReadOnly; }
			set
			{
				base.ReadOnly = value;
				CaretVisible = !value;
				if (value)
				{
#if (!XBOX && !XBOX_FAKE)
					Cursor = Manager.Skin.Cursors["Default"].Resource;
#endif
				}
				else
				{
#if (!XBOX && !XBOX_FAKE)
					Cursor = Manager.Skin.Cursors["Text"].Resource;
#endif
				}
			}
		}
		
		/// <summary>
		/// Indicates if the selection image should be drawn on the drop-down list control.
		/// </summary>
		public bool DrawSelection
		{
			get { return drawSelection; }
			set { drawSelection = value; }
		}
		
		/// <summary>
		/// Gets or sets the combo box's text box text.
		/// </summary>
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
				if (!items.Contains(value))
				{
					ItemIndex = -1;
				}
			}
		}
		
		/// <summary>
		/// Gets the combo box's drop-down list contents.
		/// </summary>
		public virtual List<object> Items
		{
			get { return items; }
		}
		
		/// <summary>
		/// Gets or sets the number of selections in the drop-down list.
		/// </summary>
		public int MaxItems
		{
			get { return maxItems; }
			set
			{
				if (maxItems != value)
				{
					maxItems = value;
					if (!Suspended) OnMaxItemsChanged(new EventArgs());
				}
			}
		}
		
		/// <summary>
		/// Gets or sets the index of the selected item.
		/// </summary>
		public int ItemIndex
		{
			get { return lstCombo.ItemIndex; }
			set
			{
				if (lstCombo != null)
				{
					if (value >= 0 && value < items.Count)
					{
						lstCombo.ItemIndex = value;
						Text = lstCombo.Items[value].ToString();
					}
					else
					{
						lstCombo.ItemIndex = -1;
					}
				}

				if (!Suspended)
				{
					OnItemIndexChanged(new EventArgs());
				}
			}
		}
		#endregion

		#region Events
		/// <summary>
		/// Occurs when the number of selections in the drop-down list changes.
		/// </summary>
		public event EventHandler MaxItemsChanged;
		/// <summary>
		/// Occurs when the index of the selected item changes.
		/// </summary>
		public event EventHandler ItemIndexChanged;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new combo box control.
		/// </summary>
		/// <param name="manager">GUI manager for the combo box control.</param>
		public ComboBox(Manager manager)
			: base(manager)
		{
			// Set dimensions of the combo box text box control.
			Height = 20;
			Width = 64;
			ReadOnly = true;

			// Configure the combo box drop-down button control.
			btnDown = new Button(Manager);
			btnDown.Init();
			btnDown.Skin = new SkinControl(Manager.Skin.Controls["ComboBox.Button"]);
			btnDown.CanFocus = false;
			btnDown.Click += new EventHandler(btnDown_Click);
			Add(btnDown, false);

			// Configure the combo box drop-down list control.
			lstCombo = new ListBox(Manager);
			lstCombo.Init();
			lstCombo.HotTrack = true;
			lstCombo.Detached = true;
			lstCombo.Visible = false;
			lstCombo.Click += new EventHandler(lstCombo_Click);
			lstCombo.FocusLost += new EventHandler(lstCombo_FocusLost);
			lstCombo.Items = items;
			manager.Input.MouseDown += new MouseEventHandler(Input_MouseDown);
		}
		#endregion

		#region Destructors
		/// <summary>
		/// Releases resources used by the combo box control.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// We added the listbox to another parent than this control, so we dispose it manually
				if (lstCombo != null)
				{
					lstCombo.Dispose();
					lstCombo = null;
				}
				Manager.Input.MouseDown -= Input_MouseDown;
			}
			base.Dispose(disposing);
		}
		#endregion

		#region Init
		/// <summary>
		/// Initializes the combo box control.
		/// </summary>
		public override void Init()
		{
			base.Init();

			lstCombo.Skin = new SkinControl(Manager.Skin.Controls["ComboBox.ListBox"]);

			btnDown.Glyph = new Glyph(Manager.Skin.Images["Shared.ArrowDown"].Resource);
			btnDown.Glyph.Color = Manager.Skin.Controls["ComboBox.Button"].Layers["Control"].Text.Colors.Enabled;
			btnDown.Glyph.SizeMode = SizeMode.Centered;
		}
		#endregion

		#region Init Skin
		/// <summary>
		/// Initializes the skin of the combo box control.
		/// </summary>
		protected internal override void InitSkin()
		{
			base.InitSkin();
			Skin = new SkinControl(Manager.Skin.Controls["ComboBox"]);
			AdjustMargins();
			ReadOnly = ReadOnly; // To init the right cursor
		}
		#endregion

		#region Draw Control
		/// <summary>
		/// Draws the combo box control.
		/// </summary>
		/// <param name="renderer">Render management object.</param>
		/// <param name="rect">Destination rectangle.</param>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		protected override void DrawControl(Renderer renderer, Rectangle rect, GameTime gameTime)
		{
			base.DrawControl(renderer, rect, gameTime);

			if (ReadOnly && (Focused || lstCombo.Focused) && drawSelection)
			{
				SkinLayer lr = Skin.Layers[0];
				Rectangle rc = new Rectangle(rect.Left + lr.ContentMargins.Left,
											 rect.Top + lr.ContentMargins.Top,
											 Width - lr.ContentMargins.Horizontal - btnDown.Width,
											 Height - lr.ContentMargins.Vertical);
				renderer.Draw(Manager.Skin.Images["ListBox.Selection"].Resource, rc, Color.FromNonPremultiplied(255, 255, 255, 128));
			}
		}
		#endregion

		#region On Resize Event Handler
		/// <summary>
		/// Handles the resize event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(ResizeEventArgs e)
		{
			base.OnResize(e);

			if (btnDown != null)
			{
				btnDown.Width = 16;
				btnDown.Height = Height - Skin.Layers[0].ContentMargins.Vertical;
				btnDown.Top = Skin.Layers[0].ContentMargins.Top;
				btnDown.Left = Width - btnDown.Width - 2;
			}
		}
		#endregion

		#region Btn Down Click Event Handler
		/// <summary>
		/// Handles the click event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void btnDown_Click(object sender, EventArgs e)
		{
			// Is there a drop-down list to display?
			if (items != null && items.Count > 0)
			{
				if (this.Root != null && this.Root is Container)
				{
					(this.Root as Container).Add(lstCombo, false);
					lstCombo.Alpha = Root.Alpha;
					lstCombo.Left = AbsoluteLeft - Root.Left;
					lstCombo.Top = AbsoluteTop - Root.Top + Height + 1;
				}

				else
				{
					Manager.Add(lstCombo);
					lstCombo.Alpha = Alpha;
					lstCombo.Left = AbsoluteLeft;
					lstCombo.Top = AbsoluteTop + Height + 1;
				}

				lstCombo.AutoHeight(maxItems);

				if (lstCombo.AbsoluteTop + lstCombo.Height > Manager.TargetHeight)
				{
					lstCombo.Top = lstCombo.Top - Height - lstCombo.Height - 2;
				}

				lstCombo.Visible = !lstCombo.Visible;
				lstCombo.Focused = true;
				lstCombo.Width = Width;
				lstCombo.AutoHeight(maxItems);
			}
		}
		#endregion

		#region Mouse Button Down Event Handler
		/// <summary>
		/// Handles the mouse button down event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Input_MouseDown(object sender, MouseEventArgs e)
		{
			if (ReadOnly &&
				(e.Position.X >= AbsoluteLeft && e.Position.X <= AbsoluteLeft + Width &&
				 e.Position.Y >= AbsoluteTop && e.Position.Y <= AbsoluteTop + Height))
			{
				return;
			}

			if (lstCombo.Visible &&
			   (e.Position.X < lstCombo.AbsoluteLeft ||	e.Position.X > lstCombo.AbsoluteLeft + lstCombo.Width || 
			   e.Position.Y < lstCombo.AbsoluteTop ||	e.Position.Y > lstCombo.AbsoluteTop + lstCombo.Height) &&
			   (e.Position.X < btnDown.AbsoluteLeft ||	e.Position.X > btnDown.AbsoluteLeft + btnDown.Width || 
			   e.Position.Y < btnDown.AbsoluteTop || e.Position.Y > btnDown.AbsoluteTop + btnDown.Height))
			{
				//lstCombo.Visible = false;      
				btnDown_Click(sender, e);
			}
		}
		#endregion

		#region Combo Box List Click Event Handler
		/// <summary>
		/// Handles a click on the drop-down list of the combo box control.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void lstCombo_Click(object sender, EventArgs e)
		{
			MouseEventArgs ex = (e is MouseEventArgs) ? (MouseEventArgs)e : new MouseEventArgs();

			if (ex.Button == MouseButton.Left || ex.Button == MouseButton.None)
			{
				lstCombo.Visible = false;
				
				if (lstCombo.ItemIndex >= 0)
				{
					Text = lstCombo.Items[lstCombo.ItemIndex].ToString();
					Focused = true;
					ItemIndex = lstCombo.ItemIndex;
				}
			}
		}
		#endregion

		#region On Key Down Event Handler
		/// <summary>
		/// Handles a key down event for the combo box control.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.Key == Keys.Down)
			{
				e.Handled = true;
				btnDown_Click(this, new MouseEventArgs());
			}
			base.OnKeyDown(e);
		}
		#endregion

		#region On Game Pad Down Event Handler
		/// <summary>
		/// Handles gamepad button down events for the combo box control.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnGamePadDown(GamePadEventArgs e)
		{
			if (!e.Handled)
			{
				if (e.Button == GamePadActions.Click || e.Button == GamePadActions.Press || e.Button == GamePadActions.Down)
				{
					e.Handled = true;
					btnDown_Click(this, new MouseEventArgs());
				}
			}
			base.OnGamePadDown(e);
		}
		#endregion

		#region On Mouse Down Event Handler
		/// <summary>
		/// Handles mouse down event for the combo box control.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (ReadOnly && e.Button == MouseButton.Left)
			{
				btnDown_Click(this, new MouseEventArgs());
			}
		}
		#endregion

		#region On Max Items Changed Event Handler
		/// <summary>
		/// Handles additions/removals to/from the combo box list control.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnMaxItemsChanged(EventArgs e)
		{
			if (MaxItemsChanged != null)
			{
				MaxItemsChanged.Invoke(this, e);
			}
		}
		#endregion

		#region On Item Index Changed Event Handler
		/// <summary>
		/// Handles list item index changed events for the combo box control.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnItemIndexChanged(EventArgs e)
		{
			if (ItemIndexChanged != null)
			{
				ItemIndexChanged.Invoke(this, e);
			}
		}
		#endregion

		#region Focus Lost Event Handler
		/// <summary>
		/// Handles the focus lost event for a combo box's list control.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void lstCombo_FocusLost(object sender, EventArgs e)
		{
			//lstCombo.Visible = false;
			Invalidate();
		}
		#endregion

		#region Adjust Margins
		/// <summary>
		/// Adjusts the margins of the client area of the combo box control.
		/// </summary>
		protected override void AdjustMargins()
		{
			base.AdjustMargins();
			ClientMargins = new Margins(ClientMargins.Left, ClientMargins.Top, ClientMargins.Right + 16, ClientMargins.Bottom);
		}
		#endregion
	}
}
