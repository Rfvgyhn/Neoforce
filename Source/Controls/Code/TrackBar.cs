////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: TrackBar.cs                                  //
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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
#endregion

namespace TomShane.Neoforce.Controls
{
	/// <summary>
	/// Represents a slider control.
	/// </summary>
	public class TrackBar : Control
	{
		#region Fields
		/// <summary>
		/// Range the track bar control's value can be within. [0, range]
		/// </summary>
		private int range = 100;
		/// <summary>
		/// Current value of the track bar control.
		/// </summary>
		private int value = 0;
		/// <summary>
		/// Small increment at which the track bar value changes.
		/// </summary>
		private int stepSize = 1;
		/// <summary>
		/// Large increment at which the track bar value changes.
		/// </summary>
		private int pageSize = 5;
		/// <summary>
		/// Indicates if the scale for the track bar should be drawn.
		/// </summary>
		private bool scale = true;
		/// <summary>
		/// Slider button that can be dragged to change the value of the track bar control.
		/// </summary>
		private Button btnSlider;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the current value of the track bar control.
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
					if (this.value > range) this.value = range;
					Invalidate();
					if (!Suspended) OnValueChanged(new EventArgs());
				}
			}
		}

		/// <summary>
		/// Gets or sets the value range of the track bar control.
		/// </summary>
		public virtual int Range
		{
			get { return range; }
			set
			{
				if (range != value)
				{
					range = value;
					range = value;
					if (pageSize > range) pageSize = range;
					RecalcParams();
					if (!Suspended) OnRangeChanged(new EventArgs());
				}
			}
		}

		/// <summary>
		/// Gets or sets the amount the track bar's value is altered for large increments.
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
		/// Gets or sets the amount the track bar's value is altered for small increments.
		/// </summary>
		public virtual int StepSize
		{
			get { return stepSize; }
			set
			{
				if (stepSize != value)
				{
					stepSize = value;
					if (stepSize > range) stepSize = range;
					if (!Suspended) OnStepSizeChanged(new EventArgs());
				}
			}
		}

		/// <summary>
		/// Indicates if the scale for the track bar should be drawn.
		/// </summary>
		public virtual bool Scale
		{
			get { return scale; }
			set { scale = value; }
		}
		#endregion

		#region Events
		/// <summary>
		/// Occurs when the value of the track bar changes.
		/// </summary>
		public event EventHandler ValueChanged;
		/// <summary>
		/// Occurs when the range of the track bar changes.
		/// </summary>
		public event EventHandler RangeChanged;
		/// <summary>
		/// Occurs when the step size value of the track bar changes.
		/// </summary>
		public event EventHandler StepSizeChanged;
		/// <summary>
		/// Occurs when the page size of the track bar changes.
		/// </summary>
		public event EventHandler PageSizeChanged;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new TrackBar control.
		/// </summary>
		/// <param name="manager">GUI manager for the control.</param>
		public TrackBar(Manager manager)
			: base(manager)
		{
			Width = 64;
			Height = 20;
			CanFocus = false;

			btnSlider = new Button(Manager);
			btnSlider.Init();
			btnSlider.Text = "";
			btnSlider.CanFocus = true;
			btnSlider.Parent = this;
			btnSlider.Anchor = Anchors.Left | Anchors.Top | Anchors.Bottom;
			btnSlider.Detached = true;
			btnSlider.Movable = true;
			btnSlider.Move += new MoveEventHandler(btnSlider_Move);
			btnSlider.KeyPress += new KeyEventHandler(btnSlider_KeyPress);
			btnSlider.GamePadPress += new GamePadEventHandler(btnSlider_GamePadPress);
		}
		#endregion

		#region Init
		/// <summary>
		/// Initializes the track bar control.
		/// </summary>
		public override void Init()
		{
			base.Init();
			btnSlider.Skin = new SkinControl(Manager.Skin.Controls["TrackBar.Button"]);
		}
		#endregion

		#region Init Skin
		/// <summary>
		/// Initializes the skin of the track bar control.
		/// </summary>
		protected internal override void InitSkin()
		{
			base.InitSkin();
			Skin = new SkinControl(Manager.Skin.Controls["TrackBar"]);
		}
		#endregion

		#region Draw Control
		/// <summary>
		/// Draws the track bar control.
		/// </summary>
		/// <param name="renderer">Render management object.</param>
		/// <param name="rect">Destination region where the control will be drawn.</param>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		protected override void DrawControl(Renderer renderer, Rectangle rect, GameTime gameTime)
		{
			RecalcParams();

			SkinLayer p = Skin.Layers["Control"];
			SkinLayer l = Skin.Layers["Scale"];

			// Scales down the height of the track bar to allow room for the scale to be drawn.
			float ratio = 0.66f;
			int h = (int)(ratio * rect.Height);
			int t = rect.Top + (Height - h) / 2;

			float px = ((float)value / (float)range);
			int w = (int)Math.Ceiling(px * (rect.Width - p.ContentMargins.Horizontal - btnSlider.Width)) + 2;

			// NOTE: Should this be using horizontal sizing margins?
			if (w < l.SizingMargins.Vertical)
			{
				w = l.SizingMargins.Vertical;
			}

			if (w > rect.Width - p.ContentMargins.Horizontal)
			{
				w = rect.Width - p.ContentMargins.Horizontal;
			}

			// Create the region for the track bar scale.
			Rectangle r1 = new Rectangle(rect.Left + p.ContentMargins.Left, t + p.ContentMargins.Top, w, h - p.ContentMargins.Vertical);
			base.DrawControl(renderer, new Rectangle(rect.Left, t, rect.Width, h), gameTime);
			
			if (scale)
			{
				renderer.DrawLayer(this, l, r1);
			}
		}
		#endregion

		#region Slider Button Move Event Handler
		/// <summary>
		/// Handles slider button move events.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void btnSlider_Move(object sender, MoveEventArgs e)
		{
			SkinLayer p = Skin.Layers["Control"];
			int size = btnSlider.Width;
			int w = Width - p.ContentMargins.Horizontal - size;
			int pos = e.Left;

			// Keep button on track by clamping position values..
			if (pos < p.ContentMargins.Left) pos = p.ContentMargins.Left;
			if (pos > w + p.ContentMargins.Left) pos = w + p.ContentMargins.Left;

			// Update button position and value.
			btnSlider.SetPosition(pos, 0);
			float px = (float)range / (float)w;
			Value = (int)(Math.Ceiling((pos - p.ContentMargins.Left) * px));
		}
		#endregion

		#region Recalc Params
		/// <summary>
		/// Updates the position of the slider button.
		/// </summary>
		private void RecalcParams()
		{
			// The slider button is created?
			if (btnSlider != null)
			{
				// Width of the slider is large enough to fit a glyph on?
				if (btnSlider.Width > 12)
				{
					// Reload the glyph and center it on the button.
					btnSlider.Glyph = new Glyph(Manager.Skin.Images["Shared.Glyph"].Resource);
					btnSlider.Glyph.SizeMode = SizeMode.Centered;
				}

				else
				{
					btnSlider.Glyph = null;
				}

				SkinLayer p = Skin.Layers["Control"];
				btnSlider.Width = (int)(Height * 0.8);
				btnSlider.Height = Height;
				int size = btnSlider.Width;
				int w = Width - p.ContentMargins.Horizontal - size;

				// Determine the position of the slider button.
				float px = (float)range / (float)w;
				int pos = p.ContentMargins.Left + (int)(Math.Ceiling(Value / (float)px));

				// Clamp position within the bounds of the track bar control.
				if (pos < p.ContentMargins.Left) pos = p.ContentMargins.Left;
				if (pos > w + p.ContentMargins.Left) pos = w + p.ContentMargins.Left;

				btnSlider.SetPosition(pos, 0);
			}
		}
		#endregion

		#region On Mouse Press Event Handler
		/// <summary>
		/// Handles mouse press events for the track bar control.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMousePress(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (e.Button == MouseButton.Left)
			{
				int pos = e.Position.X;

				// Step down track bar value.
				if (pos < btnSlider.Left)
				{
					Value -= pageSize;
				}

				// Step up track bar value.
				else if (pos >= btnSlider.Left + btnSlider.Width)
				{
					Value += pageSize;
				}
			}
		}
		#endregion

		#region Slider Button Game Pad Press Event Handler
		/// <summary>
		/// Handles gamepad left/right button presses for the track bar control.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void btnSlider_GamePadPress(object sender, GamePadEventArgs e)
		{
			// Step up/down the track bar's value as needed.
			if (e.Button == GamePadActions.Left || e.Button == GamePadActions.Down) Value -= stepSize;
			if (e.Button == GamePadActions.Right || e.Button == GamePadActions.Up) Value += stepSize;
		}
		#endregion

		#region Slider Button Key Press Event Handler
		/// <summary>
		/// Handles key press events for the track bar control.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void btnSlider_KeyPress(object sender, KeyEventArgs e)
		{
			if (e.Key == Microsoft.Xna.Framework.Input.Keys.Left || e.Key == Microsoft.Xna.Framework.Input.Keys.Down) Value -= stepSize;
			else if (e.Key == Microsoft.Xna.Framework.Input.Keys.Right || e.Key == Microsoft.Xna.Framework.Input.Keys.Up) Value += stepSize;
			else if (e.Key == Microsoft.Xna.Framework.Input.Keys.PageDown) Value -= pageSize;
			else if (e.Key == Microsoft.Xna.Framework.Input.Keys.PageUp) Value += pageSize;
			else if (e.Key == Microsoft.Xna.Framework.Input.Keys.Home) Value = 0;
			else if (e.Key == Microsoft.Xna.Framework.Input.Keys.End) Value = Range;
		}
		#endregion

		#region On Resize Event Handler
		/// <summary>
		/// Handles resizing of the track bar control.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(ResizeEventArgs e)
		{
			base.OnResize(e);
			RecalcParams();
		}
		#endregion

		#region On Value Changed
		/// <summary>
		/// Handles changes in the track bar's value.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnValueChanged(EventArgs e)
		{
			if (ValueChanged != null) ValueChanged.Invoke(this, e);
		}
		#endregion

		#region On Range Changed Event Handler
		/// <summary>
		/// Handles changes in the track bar's range.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnRangeChanged(EventArgs e)
		{
			if (RangeChanged != null) RangeChanged.Invoke(this, e);
		}
		#endregion

		#region On Page Size Changed
		/// <summary>
		/// Handles changes in the track bar's page size.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnPageSizeChanged(EventArgs e)
		{
			if (PageSizeChanged != null) PageSizeChanged.Invoke(this, e);
		}
		#endregion

		#region On Step Size Changed Event Handler
		/// <summary>
		/// Handles changes in the track bar's step size.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnStepSizeChanged(EventArgs e)
		{
			if (StepSizeChanged != null) StepSizeChanged.Invoke(this, e);
		}
		#endregion
	}
}
