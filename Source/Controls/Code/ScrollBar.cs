////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: ScrollBar.cs                                 //
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
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TomShane.Neoforce.Controls
{
	/// <summary>
	/// Represents a horizontal or vertical scroll bar control.
	/// </summary>
	public class ScrollBar : Control
	{
		#region Fields
		/// <summary>
		/// Maximum value of the scroll bar control.
		/// </summary>
		private int range = 100;
		/// <summary>
		/// Current value of the scroll bar control.
		/// </summary>
		private int value = 0;
		/// <summary>
		/// Increment used for large changes to the scroll bar value.
		/// </summary>
		private int pageSize = 50;
		/// <summary>
		/// Increment used for small changes to the scroll bar value.
		/// </summary>
		private int stepSize = 1;
		/// <summary>
		/// Indicates if the control is a horizontal or vertical scroll bar.
		/// </summary>
		private Orientation orientation = Orientation.Vertical;
		/// <summary>
		/// Button used to decrease the value of the scroll bar.
		/// </summary>
		private Button btnMinus = null;
		/// <summary>
		/// Button used to increase the value of the scroll bar.
		/// </summary>
		private Button btnPlus = null;
		/// <summary>
		/// Button indicating the current value of the scroll bar that the user 
		/// can drag up/down or left/right to scroll.
		/// </summary>
		private Button btnSlider = null;
		/// <summary>
		/// String for accessing the resource used to draw the scroll bar plus and 
		/// minus buttons.
		/// </summary>
		private string strButton = "ScrollBar.ButtonVert";
		/// <summary>
		/// String for accessing the resource used to draw the rail that the slider 
		/// button moves on.
		/// </summary>
		private string strRail = "ScrollBar.RailVert";
		/// <summary>
		/// String for accessing the resource used to draw the scroll bar slider button.
		/// </summary>
		private string strSlider = "ScrollBar.SliderVert";
		/// <summary>
		/// String for accessing the glyph displayed on the scroll bar slider button.
		/// </summary>
		private string strGlyph = "ScrollBar.GlyphVert";
		/// <summary>
		/// String for accessing the Arrow Up glyph for the scroll bar.
		/// </summary>
		private string strMinus = "ScrollBar.ArrowUp";
		/// <summary>
		/// String for accessing the Arrow Down glyph for the scroll bar.
		/// </summary>
		private string strPlus = "ScrollBar.ArrowDown";
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the current value of the scroll bar.
		/// </summary>
		public virtual int Value
		{
			get { return this.value; }
			set
			{
				if (this.value != value)
				{
					this.value = value;
					if (this.value < 0) this.value = 0;
					if (this.value > range - pageSize) this.value = range - pageSize;
					Invalidate();
					if (!Suspended) OnValueChanged(new EventArgs());
				}
			}
		}

		/// <summary>
		/// Gets or sets the maximum value of the scroll bar.
		/// </summary>
		public virtual int Range
		{
			get { return range; }
			set
			{
				if (range != value)
				{
					range = value;
					if (pageSize > range) pageSize = range;
					RecalcParams();
					if (!Suspended) OnRangeChanged(new EventArgs());
				}
			}
		}

		/// <summary>
		/// Gets or sets the increment the scroll bar value changes for large increments.
		/// </summary>
		public virtual int PageSize
		{
			get { return pageSize; }
			set
			{
				if (pageSize != value)
				{
					pageSize = value;
					if (pageSize > range) pageSize = range;
					RecalcParams();
					if (!Suspended) OnPageSizeChanged(new EventArgs());
				}
			}
		}

		/// <summary>
		/// Gets or sets the increment the scroll bar value changes for small increments.
		/// </summary>
		public virtual int StepSize
		{
			get { return stepSize; }
			set
			{
				if (stepSize != value)
				{
					stepSize = value;
					if (!Suspended) OnStepSizeChanged(new EventArgs());
				}
			}
		}
		#endregion

		#region Events
		/// <summary>
		/// Occurs when the value of the scroll bar changes.
		/// </summary>
		public event EventHandler ValueChanged;
		/// <summary>
		/// Occurs when the range of the scroll bar changes.
		/// </summary>
		public event EventHandler RangeChanged;
		/// <summary>
		/// Occurs when the step size of the scroll bar changes.
		/// </summary>
		public event EventHandler StepSizeChanged;
		/// <summary>
		/// Occurs when the page size of the scroll bar changes.
		/// </summary>
		public event EventHandler PageSizeChanged;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new ScrollBar control.
		/// </summary>
		/// <param name="manager">GUI manager for the scroll bar.</param>
		/// <param name="orientation">Indicates if the scroll bar is horizontal or vertical.</param>
		public ScrollBar(Manager manager, Orientation orientation)
			: base(manager)
		{
			this.orientation = orientation;
			CanFocus = false;

			// Set the skin accessor strings based on orientation.
			if (orientation == Orientation.Horizontal)
			{
				strButton = "ScrollBar.ButtonHorz";
				strRail = "ScrollBar.RailHorz";
				strSlider = "ScrollBar.SliderHorz";
				strGlyph = "ScrollBar.GlyphHorz";
				strMinus = "ScrollBar.ArrowLeft";
				strPlus = "ScrollBar.ArrowRight";

				MinimumHeight = 16;
				MinimumWidth = 46;
				Width = 64;
				Height = 16;
			}
			else
			{
				strButton = "ScrollBar.ButtonVert";
				strRail = "ScrollBar.RailVert";
				strSlider = "ScrollBar.SliderVert";
				strGlyph = "ScrollBar.GlyphVert";
				strMinus = "ScrollBar.ArrowUp";
				strPlus = "ScrollBar.ArrowDown";

				MinimumHeight = 46;
				MinimumWidth = 16;
				Width = 16;
				Height = 64;
			}

			// Create the minus button.
			btnMinus = new Button(Manager);
			btnMinus.Init();
			btnMinus.Text = "";
			btnMinus.MousePress += new MouseEventHandler(ArrowPress);
			btnMinus.CanFocus = false;

			// Create the slider button.
			btnSlider = new Button(Manager);
			btnSlider.Init();
			btnSlider.Text = "";
			btnSlider.CanFocus = false;
			btnSlider.MinimumHeight = 16;
			btnSlider.MinimumWidth = 16;

			// Create the plus button.
			btnPlus = new Button(Manager);
			btnPlus.Init();
			btnPlus.Text = "";
			btnPlus.MousePress += new MouseEventHandler(ArrowPress);
			btnPlus.CanFocus = false;

			btnSlider.Move += new MoveEventHandler(btnSlider_Move);

			Add(btnMinus);
			Add(btnSlider);
			Add(btnPlus);
		}
		////////////////////////////////////////////////////////////////////////////

		#endregion

		#region Init
		/// <summary>
		/// Initializes the scroll bar control.
		/// </summary>
		public override void Init()
		{
			base.Init();

			SkinControl sc = new SkinControl(btnPlus.Skin);
			sc.Layers["Control"] = new SkinLayer(Skin.Layers[strButton]);
			sc.Layers[strButton].Name = "Control";
			btnPlus.Skin = btnMinus.Skin = sc;

			SkinControl ss = new SkinControl(btnSlider.Skin);
			ss.Layers["Control"] = new SkinLayer(Skin.Layers[strSlider]);
			ss.Layers[strSlider].Name = "Control";
			btnSlider.Skin = ss;

			btnMinus.Glyph = new Glyph(Skin.Layers[strMinus].Image.Resource);
			btnMinus.Glyph.SizeMode = SizeMode.Centered;
			btnMinus.Glyph.Color = Manager.Skin.Controls["Button"].Layers["Control"].Text.Colors.Enabled;

			btnPlus.Glyph = new Glyph(Skin.Layers[strPlus].Image.Resource);
			btnPlus.Glyph.SizeMode = SizeMode.Centered;
			btnPlus.Glyph.Color = Manager.Skin.Controls["Button"].Layers["Control"].Text.Colors.Enabled;

			btnSlider.Glyph = new Glyph(Skin.Layers[strGlyph].Image.Resource);
			btnSlider.Glyph.SizeMode = SizeMode.Centered;

		}
		#endregion

		#region Init Skin
		/// <summary>
		/// Initializes the skin for the scroll bar control.
		/// </summary>
		protected internal override void InitSkin()
		{
			base.InitSkin();
			Skin = new SkinControl(Manager.Skin.Controls["ScrollBar"]);
		}
		#endregion

		#region Draw Control
		/// <summary>
		/// Draws the scroll bar control.
		/// </summary>
		/// <param name="renderer">Render management object.</param>
		/// <param name="rect">Destination region where the scroll bar should be drawn.</param>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		protected override void DrawControl(Renderer renderer, Rectangle rect, GameTime gameTime)
		{
			RecalcParams();

			SkinLayer bg = Skin.Layers[strRail];
			renderer.DrawLayer(bg, rect, Color.White, bg.States.Enabled.Index);
		}
		#endregion

		#region Arrow Press
		/// <summary>
		/// Handles click events for plus and minus button controls.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void ArrowPress(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButton.Left)
			{
				// Decrease value?
				if (sender == btnMinus)
				{
					Value -= StepSize;
					if (Value < 0) Value = 0;
				}

				// Increase value?
				else if (sender == btnPlus)
				{
					Value += StepSize;
					if (Value > range - pageSize) Value = range - pageSize - 1;
				}
			}
		}
		#endregion

		#region On Resize Event Handler
		/// <summary>
		/// Handles resizing the scroll bar control.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(ResizeEventArgs e)
		{
			base.OnResize(e);
			RecalcParams();
			if (Value + PageSize > Range) Value = Range - PageSize;
		}
		#endregion

		#region Recalc Params
		/// <summary>
		/// Adjusts the sizes and positions of all buttons based on scroll bar orientation, dimensions, and value.
		/// </summary>
		private void RecalcParams()
		{
			// Buttons exist?
			if (btnMinus != null && btnPlus != null && btnSlider != null)
			{
				// Horizontal scroll bar?
				if (orientation == Orientation.Horizontal)
				{
					// Plus/Minus buttons are square and the same height as the scroll bar control.
					btnMinus.Width = Height;
					btnMinus.Height = Height;

					btnPlus.Width = Height;
					btnPlus.Height = Height;
					btnPlus.Left = Width - Height;
					btnPlus.Top = 0;

					btnSlider.Movable = true;
					int size = btnMinus.Width + Skin.Layers[strSlider].OffsetX;

					// Minimum size of the slider button is a square the same height as the scroll bar.
					btnSlider.MinimumWidth = Height;
					int w = (Width - 2 * size);
					btnSlider.Width = (int)Math.Ceiling((pageSize * w) / (float)range);
					btnSlider.Height = Height;

					// Position the slider button based on its size and the scroll bar value. 
					float px = (float)(Range - PageSize) / (float)(w - btnSlider.Width);
					int pos = (int)(Math.Ceiling(Value / (float)px));
					btnSlider.SetPosition(size + pos, 0);
					if (btnSlider.Left < size) btnSlider.SetPosition(size, 0);
					if (btnSlider.Left + btnSlider.Width + size > Width) btnSlider.SetPosition(Width - size - btnSlider.Width, 0);
				}

				// Vertical scroll bar.
				else
				{
					// Plus/Minus buttons are square and the same width as the scroll bar control.
					btnMinus.Width = Width;
					btnMinus.Height = Width;

					btnPlus.Width = Width;
					btnPlus.Height = Width;
					btnPlus.Top = Height - Width;

					btnSlider.Movable = true;
					int size = btnMinus.Height + Skin.Layers[strSlider].OffsetY;

					// Minimum size of the slider button is a square the same width as the scroll bar.
					btnSlider.MinimumHeight = Width;
					int h = (Height - 2 * size);
					btnSlider.Height = (int)Math.Ceiling((pageSize * h) / (float)range);
					btnSlider.Width = Width;

					// Position the slider button based on its size and the scroll bar value.
					float px = (float)(Range - PageSize) / (float)(h - btnSlider.Height);
					int pos = (int)(Math.Ceiling(Value / (float)px));
					btnSlider.SetPosition(0, size + pos);
					if (btnSlider.Top < size) btnSlider.SetPosition(0, size);
					if (btnSlider.Top + btnSlider.Height + size > Height) btnSlider.SetPosition(0, Height - size - btnSlider.Height);
				}
			}
		}
		#endregion

		#region Slider Button Move Event Handler
		/// <summary>
		/// Handles movement of the slider button.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void btnSlider_Move(object sender, MoveEventArgs e)
		{
			// Update the slider button's position.
			if (orientation == Orientation.Horizontal)
			{
				// Slider moves horizontally. Set new position and clamp it to the rail.
				int size = btnMinus.Width + Skin.Layers[strSlider].OffsetX;
				btnSlider.SetPosition(e.Left, 0);
				if (btnSlider.Left < size) btnSlider.SetPosition(size, 0);
				if (btnSlider.Left + btnSlider.Width + size > Width) btnSlider.SetPosition(Width - size - btnSlider.Width, 0);
			}
			else
			{
				// Slider moves vertically. Set new position and clamp it to the rail.
				int size = btnMinus.Height + Skin.Layers[strSlider].OffsetY;
				btnSlider.SetPosition(0, e.Top);
				if (btnSlider.Top < size) btnSlider.SetPosition(0, size);
				if (btnSlider.Top + btnSlider.Height + size > Height) btnSlider.SetPosition(0, Height - size - btnSlider.Height);
			}

			// Update the scroll bar's value based on slider button's position.
			if (orientation == Orientation.Horizontal)
			{
				int size = btnMinus.Width + Skin.Layers[strSlider].OffsetX;
				int w = (Width - 2 * size) - btnSlider.Width;
				float px = (float)(Range - PageSize) / (float)w;
				Value = (int)(Math.Ceiling((btnSlider.Left - size) * px));
			}
			else
			{
				int size = btnMinus.Height + Skin.Layers[strSlider].OffsetY;
				int h = (Height - 2 * size) - btnSlider.Height;
				float px = (float)(Range - PageSize) / (float)h;
				Value = (int)(Math.Ceiling((btnSlider.Top - size) * px));
			}
		}
		#endregion

		#region On Mouse Up Event Handler
		/// <summary>
		/// Handles mouse button up events for the scroll bar.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			btnSlider.Passive = false;
			base.OnMouseUp(e);
		}
		#endregion

		#region On Mouse Down Event Handler
		/// <summary>
		/// Handles mouse button down events for the scroll bar.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			btnSlider.Passive = true;

			// Check for change in slider position?
			if (e.Button == MouseButton.Left)
			{
				// Change in X position?
				if (orientation == Orientation.Horizontal)
				{
					int pos = e.Position.X;

					// Big change? Which direction do we page?
					if (pos < btnSlider.Left)
					{
						Value -= pageSize;
						if (Value < 0) Value = 0;
					}
					else if (pos >= btnSlider.Left + btnSlider.Width)
					{
						Value += pageSize;
						if (Value > range - pageSize) Value = range - pageSize;
					}
				}

				// Check for change in Y position.
				else
				{
					int pos = e.Position.Y;

					// Big change? Which direction do we page?
					if (pos < btnSlider.Top)
					{
						Value -= pageSize;
						if (Value < 0) Value = 0;
					}
					else if (pos >= btnSlider.Top + btnSlider.Height)
					{
						Value += pageSize;
						if (Value > range - pageSize) Value = range - pageSize;
					}
				}
			}
		}
		#endregion

		#region On Value Changed Event Handler
		/// <summary>
		/// Handles scroll bar value changes.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnValueChanged(EventArgs e)
		{
			if (ValueChanged != null) ValueChanged.Invoke(this, e);
		}
		#endregion

		#region On Range Changed Event Handler
		/// <summary>
		/// Handles changes in the scroll bar's range.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnRangeChanged(EventArgs e)
		{
			if (RangeChanged != null) RangeChanged.Invoke(this, e);
		}
		#endregion

		#region On Page Size Changed Event Handler
		/// <summary>
		/// Handles changes in the scroll bar's page size.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnPageSizeChanged(EventArgs e)
		{
			if (PageSizeChanged != null) PageSizeChanged.Invoke(this, e);
		}
		#endregion

		#region On Step Size Changed Event Handler
		/// <summary>
		/// Handles changes in the scroll bar's step size.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnStepSizeChanged(EventArgs e)
		{
			if (StepSizeChanged != null) StepSizeChanged.Invoke(this, e);
		}
		#endregion
	}
}
