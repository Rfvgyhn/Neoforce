////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: SpinBox.cs                                   //
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
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TomShane.Neoforce.Controls
{
	/// <summary>
	/// Specifies the data that the spin box works with.
	/// </summary>
	public enum SpinBoxMode
	{
		/// <summary>
		/// Adjusts a single numeric value by the defined step amount. 
		/// </summary>
		Range,
		/// <summary>
		/// Adjusts the index referencing the selected item in a collection.
		/// </summary>
		List
	}

	/// <summary>
	/// Represents a text box with two buttons used to increase/decrease its value. 
	/// </summary>
	public class SpinBox : TextBox
	{
		#region Fields
		/// <summary>
		/// Button used to increase the spin box value.
		/// </summary>
		private Button btnUp = null;
		/// <summary>
		/// Button used to decrease the spin box value.
		/// </summary>
		private Button btnDown = null;
		/// <summary>
		/// Defines whether the spin box works with numeric values or if 
		/// it iterates through items defined in its item list.
		/// </summary>
		private SpinBoxMode mode = SpinBoxMode.List;
		/// <summary>
		/// Collection of items the spin box will display 
		/// </summary>
		private List<object> items = new List<object>();
		/// <summary>
		/// Current value of the spin box.
		/// </summary>
		private float value = 0;
		/// <summary>
		/// Minimum value of the spin box.
		/// </summary>
		private float minimum = 0;
		/// <summary>
		/// Maximum value of the spin box.
		/// </summary>
		private float maximum = 100;
		/// <summary>
		/// Amount the value increases/decreases when the up/down buttons are clicked.
		/// </summary>
		private float step = 0.25f;
		/// <summary>
		/// Decimal places to round values to.
		/// </summary>
		private int rounding = 2;
		/// <summary>
		/// Selected item index if the items list is used.
		/// </summary>
		private int itemIndex = -1;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the data that the spin box works with. (Collection or numeric value.)
		/// </summary>
		public new virtual SpinBoxMode Mode
		{
			get { return mode; }
			set { mode = value; }
		}

		/// <summary>
		/// Indicates if the control's text box is read-only or not.
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
		/// Gets the collection of objects the spin box iterates through in List mode.
		/// </summary>
		public virtual List<object> Items
		{
			get { return items; }
		}

		/// <summary>
		/// Gets or sets the spin box value in Range mode.
		/// </summary>
		public float Value
		{
			get { return this.value; }
			set
			{
				if (this.value != value)
				{
					this.value = value;
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Gets or sets the minimum value of the spin box in Range mode.
		/// </summary>
		public float Minimum
		{
			get { return minimum; }
			set
			{
				if (minimum != value)
				{
					minimum = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the maximum value of the spin box in Range mode.
		/// </summary>
		public float Maximum
		{
			get { return maximum; }
			set
			{
				if (maximum != value)
				{
					maximum = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the amount the value of the spin box changes when 
		/// the up or down button is pressed in Range mode.
		/// </summary>
		public float Step
		{
			get { return step; }
			set
			{
				if (step != value)
				{
					step = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the index of the selected item in the collection in List mode.
		/// </summary>
		public int ItemIndex
		{
			get { return itemIndex; }
			set
			{
				if (mode == SpinBoxMode.List)
				{
					itemIndex = value;
					Text = items[itemIndex].ToString();
				}
			}
		}

		/// <summary>
		/// Gets or sets the number of decimal places the spin box allows.
		/// </summary>
		public int Rounding
		{
			get { return rounding; }
			set
			{
				if (rounding != value)
				{
					rounding = value;
					Invalidate();
				}
			}
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Creates a new spin box control.
		/// </summary>
		/// <param name="manager">GUI manager for the control.</param>
		/// <param name="mode">Indicates if the spin box uses numeric values or a collection of items.</param>
		public SpinBox(Manager manager, SpinBoxMode mode)
			: base(manager)
		{
			this.mode = mode;
			ReadOnly = true;

			Height = 20;
			Width = 64;

			btnUp = new Button(Manager);
			btnUp.Init();
			btnUp.CanFocus = false;
			btnUp.MousePress += new MouseEventHandler(btn_MousePress);
			Add(btnUp, false);

			btnDown = new Button(Manager);
			btnDown.Init();
			btnDown.CanFocus = false;
			btnDown.MousePress += new MouseEventHandler(btn_MousePress);
			Add(btnDown, false);
		}
		#endregion

		#region Init
		/// <summary>
		/// Initializes the spin box control.
		/// </summary>
		public override void Init()
		{
			base.Init();

			SkinControl sc = new SkinControl(btnUp.Skin);
			sc.Layers["Control"] = new SkinLayer(Skin.Layers["Button"]);
			sc.Layers["Button"].Name = "Control";
			btnUp.Skin = btnDown.Skin = sc;

			btnUp.Glyph = new Glyph(Manager.Skin.Images["Shared.ArrowUp"].Resource);
			btnUp.Glyph.SizeMode = SizeMode.Centered;
			btnUp.Glyph.Color = Manager.Skin.Controls["Button"].Layers["Control"].Text.Colors.Enabled;

			btnDown.Glyph = new Glyph(Manager.Skin.Images["Shared.ArrowDown"].Resource);
			btnDown.Glyph.SizeMode = SizeMode.Centered;
			btnDown.Glyph.Color = Manager.Skin.Controls["Button"].Layers["Control"].Text.Colors.Enabled;
		}
		#endregion

		#region Init Skin
		/// <summary>
		/// Initializes the skin of the spin box control.
		/// </summary>
		protected internal override void InitSkin()
		{
			base.InitSkin();
			Skin = new SkinControl(Manager.Skin.Controls["SpinBox"]);
		}
		#endregion

		#region Draw Control
		/// <summary>
		/// Draws the spin box control.
		/// </summary>
		/// <param name="renderer">Render management object.</param>
		/// <param name="rect">Destination region where the control will be drawn.</param>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		protected override void DrawControl(Renderer renderer, Rectangle rect, GameTime gameTime)
		{
			base.DrawControl(renderer, rect, gameTime);

			if (ReadOnly && Focused)
			{
				SkinLayer lr = Skin.Layers[0];
				Rectangle rc = new Rectangle(rect.Left + lr.ContentMargins.Left,
											 rect.Top + lr.ContentMargins.Top,
											 Width - lr.ContentMargins.Horizontal - btnDown.Width - btnUp.Width,
											 Height - lr.ContentMargins.Vertical);
				renderer.Draw(Manager.Skin.Images["ListBox.Selection"].Resource, rc, Color.FromNonPremultiplied(255, 255, 255, 128));
			}
		}
		#endregion

		#region On Resize Event Handler
		/// <summary>
		/// Handles resizing the spin box control.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(ResizeEventArgs e)
		{
			base.OnResize(e);

			// Reposition the spin buttons. 
			if (btnUp != null)
			{
				btnUp.Width = 16;
				btnUp.Height = Height - Skin.Layers["Control"].ContentMargins.Vertical;
				btnUp.Top = Skin.Layers["Control"].ContentMargins.Top;
				btnUp.Left = Width - 16 - 2 - 16 - 1;
			}

			if (btnDown != null)
			{
				btnDown.Width = 16;
				btnDown.Height = Height - Skin.Layers["Control"].ContentMargins.Vertical;
				btnDown.Top = Skin.Layers["Control"].ContentMargins.Top; ;
				btnDown.Left = Width - 16 - 2;
			}
		}
		#endregion

		#region Shift Index
		/// <summary>
		/// Adjusts the spin box's value up or down. 
		/// </summary>
		/// <param name="direction">Indicates which way to shift index/values. (True to increase; false to decrease.)</param>
		private void ShiftIndex(bool direction)
		{
			if (mode == SpinBoxMode.List)
			{
				// Collection not empty?
				if (items.Count > 0)
				{
					// Adjust index.
					if (direction)
					{
						itemIndex += 1;
					}
					else
					{
						itemIndex -= 1;
					}

					// Clamp index.
					if (itemIndex < 0) itemIndex = 0;
					if (itemIndex > items.Count - 1) itemIndex = itemIndex = items.Count - 1;

					// Update the text with the current item's string representation.
					Text = items[itemIndex].ToString();
				}
			}
			else
			{
				// Adjust the value.
				if (direction)
				{
					value += step;
				}
				else
				{
					value -= step;
				}

				// Clamp within specified range.
				if (value < minimum) value = minimum;
				if (value > maximum) value = maximum;

				// Display the value with the specified number of digits.
				Text = value.ToString("n" + rounding.ToString());
			}
		}
		#endregion

		#region Button Mouse Press Event Handler
		/// <summary>
		/// Handles button up/down clicks.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btn_MousePress(object sender, MouseEventArgs e)
		{
			Focused = true;
			if (sender == btnUp) ShiftIndex(true);
			else if (sender == btnDown) ShiftIndex(false);
		}
		#endregion

		#region On Key Press Event Handler
		/// <summary>
		/// Handles key press events.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyPress(KeyEventArgs e)
		{
			if (e.Key == Keys.Up)
			{
				e.Handled = true;
				ShiftIndex(true);
			}
			else if (e.Key == Keys.Down)
			{
				e.Handled = true;
				ShiftIndex(false);
			}

			base.OnKeyPress(e);
		}
		#endregion

		#region On Game Pad Press Event Handler
		/// <summary>
		/// Handles up/down inputs from a gamepad.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnGamePadPress(GamePadEventArgs e)
		{
			if (e.Button == GamePadActions.Up)
			{
				e.Handled = true;
				ShiftIndex(true);
			}

			else if (e.Button == GamePadActions.Down)
			{
				e.Handled = true;
				ShiftIndex(false);
			}

			base.OnGamePadPress(e);
		}
		#endregion

		#region On Game Pad Down Event Handler
		/// <summary>
		/// Handles gamepad button press events.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnGamePadDown(GamePadEventArgs e)
		{
			base.OnGamePadDown(e);
		}
		#endregion
	}
}
