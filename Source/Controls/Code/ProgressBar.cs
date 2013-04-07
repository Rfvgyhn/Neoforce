////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: ProgressBar.cs                               //
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
using System;
#endregion

namespace TomShane.Neoforce.Controls
{
	/// <summary>
	/// Specifies how the progress bar updates.
	/// </summary>
	public enum ProgressBarMode
	{
		/// <summary>
		/// Standard progress bar behavior.
		/// </summary>
		Default,
		/// <summary>
		/// Progress bar oscillates between 0 and 75 percent complete. ???
		/// </summary>
		Infinite
	}

	/// <summary>
	/// Represents a progress bar control.
	/// </summary>
	public class ProgressBar : Control
	{
		#region Fields
		/// <summary>
		/// Maximum value of the progress bar. (Minumum range value is 0)
		/// </summary>
		private int range = 100;
		/// <summary>
		/// Current value of the progress bar.
		/// </summary>
		private int value = 0;
		/// <summary>
		/// Tracks elapsed time between updates for infinite progress bars.
		/// </summary>
		private double time = 0;
		/// <summary>
		/// Used to control infinite progress bar value updates. 
		/// </summary>
		private int sign = 1;
		/// <summary>
		/// Indicates how the progress bar is updated.
		/// </summary>
		private ProgressBarMode mode = ProgressBarMode.Default;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the current value of the progress bar.
		/// </summary>
		public int Value
		{
			get { return this.value; }
			set
			{
				if (mode == ProgressBarMode.Default)
				{
					// Update value and clamp within range.
					if (this.value != value)
					{
						this.value = value;
						if (this.value > range) this.value = range;
						if (this.value < 0) this.value = 0;
						Invalidate();

						if (!Suspended) OnValueChanged(new EventArgs());
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the mode of the progress bar.
		/// </summary>
		public ProgressBarMode Mode
		{
			get { return mode; }
			set
			{
				if (mode != value)
				{
					mode = value;

					// Reset range, time, and value when the mode changes.
					if (mode == ProgressBarMode.Infinite)
					{
						range = 100;
						this.value = 0;
						time = 0;
						sign = 1;
					}

					else
					{
						this.value = 0;
						range = 100;
					}
					Invalidate();

					if (!Suspended)
					{
						OnModeChanged(new EventArgs());
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the maximum range of the progress bar.
		/// </summary>
		public int Range
		{
			get { return range; }
			set
			{
				if (range != value)
				{
					if (mode == ProgressBarMode.Default)
					{
						range = value;
						if (range < 0) range = 0;
						if (range < this.value) this.value = range;
						Invalidate();

						if (!Suspended) OnRangeChanged(new EventArgs());
					}
				}
			}
		}
		#endregion

		#region Events
		/// <summary>
		/// Occurs when the value of the progress bar is changed.
		/// </summary>
		public event EventHandler ValueChanged;
		/// <summary>
		/// Occurs when the range of the progress bar is changed.
		/// </summary>
		public event EventHandler RangeChanged;
		/// <summary>
		/// Occurs when the mode of the progress bar is changed.
		/// </summary>
		public event EventHandler ModeChanged;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new ProgressBar control.
		/// </summary>
		/// <param name="manager">GUI manager for the control.</param>
		public ProgressBar(Manager manager)
			: base(manager)
		{
			Width = 128;
			Height = 16;
			MinimumHeight = 8;
			MinimumWidth = 32;
			Passive = true;
			CanFocus = false;
		}
		#endregion

		#region Init
		/// <summary>
		/// Initializes the progress bar control.
		/// </summary>
		public override void Init()
		{
			base.Init();
		}
		#endregion

		#region Draw Control
		/// <summary>
		/// Draws the progress bar control.
		/// </summary>
		/// <param name="renderer">Render management object.</param>
		/// <param name="rect">Destination region where the progress bar will be drawn.</param>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		protected override void DrawControl(Renderer renderer, Rectangle rect, GameTime gameTime)
		{
			CheckLayer(Skin, "Control");
			CheckLayer(Skin, "Scale");

			base.DrawControl(renderer, rect, gameTime);

			if (Value > 0 || mode == ProgressBarMode.Infinite)
			{
				SkinLayer p = Skin.Layers["Control"];
				SkinLayer l = Skin.Layers["Scale"];
				Rectangle r = new Rectangle(rect.Left + p.ContentMargins.Left,
											rect.Top + p.ContentMargins.Top,
											rect.Width - p.ContentMargins.Vertical,
											rect.Height - p.ContentMargins.Horizontal);

				// Calculate the current progress of the operations this is tracking.
				float perc = ((float)value / range) * 100;

				// Figure out how much of the progress bar should be filled in.
				int w = (int)((perc / 100) * r.Width);
				Rectangle rx;

				if (mode == ProgressBarMode.Default)
				{
					// NOTE: Shouldn't this be checking against the horizontal margins?
					if (w < l.SizingMargins.Vertical)
					{
						w = l.SizingMargins.Vertical;
					}
					
					rx = new Rectangle(r.Left, r.Top, w, r.Height);
				}

				else
				{
					int s = r.Left + w;

					if (s > r.Left + p.ContentMargins.Left + r.Width - (r.Width / 4))
					{
						s = r.Left + p.ContentMargins.Left + r.Width - (r.Width / 4);
					}
					
					rx = new Rectangle(s, r.Top, (r.Width / 4), r.Height);
				}

				renderer.DrawLayer(this, l, rx);
			}
		}
		#endregion

		#region Update
		/// <summary>
		/// Updates the progress bar control.
		/// </summary>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		protected internal override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (mode == ProgressBarMode.Infinite && Enabled && Visible)
			{
				time += gameTime.ElapsedGameTime.TotalMilliseconds;
				
				// 1/30 of a second has passed?
				if (time >= 33f)
				{
					// Update the progress bar value.
					value += sign * (int)Math.Ceiling(time / 20f);
					
					// Value exceeds 75%?
					if (value >= Range - (Range / 4))
					{
						// Adjust value and start counting down.
						value = Range - (Range / 4);
						sign = -1;
					}

					// Value less than 0%?
					else if (value <= 0)
					{
						// Adjust value and start counting up.
						value = 0;
						sign = 1;
					}

					time = 0;
					Invalidate();
				}
			}
		}
		#endregion

		#region On Value Changed Event Handler
		/// <summary>
		/// Handles progress bar value changes.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnValueChanged(EventArgs e)
		{
			if (ValueChanged != null)
			{
				ValueChanged.Invoke(this, e);
			}
		}
		#endregion

		#region On Range Changed Event Handler
		/// <summary>
		/// Handles progress bar range changes.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnRangeChanged(EventArgs e)
		{
			if (RangeChanged != null)
			{
				RangeChanged.Invoke(this, e);
			}
		}
		#endregion

		#region On Mode Changed Event Handler
		/// <summary>
		/// Handles progress bar mode changes.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnModeChanged(EventArgs e)
		{
			if (ModeChanged != null)
			{
				ModeChanged.Invoke(this, e);
			}
		}
		#endregion
	}
}
