////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: Panel.cs                                     //
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
#endregion

namespace TomShane.Neoforce.Controls
{
	/// <summary>
	/// Represents a panel container. (Mainly for grouping controls or splitting up a form.) 
	/// </summary>
	public class Panel : Container
	{
		#region Fields
        /// <summary>
        /// Describes the edges of the panel.
        /// </summary>
		private Bevel bevel = null;
		/// <summary>
		/// Describes the style of beveled edge to apply when drawing.
		/// </summary>
		private BevelStyle bevelStyle = BevelStyle.None;
		/// <summary>
		/// Specifies which side(s) of the panel container will be drawn with a beveled edge.
		/// </summary>
		private BevelBorder bevelBorder = BevelBorder.None;
		/// <summary>
		/// Margin between the beveled edge and the panel's content area. ???
		/// </summary>
		private int bevelMargin = 0;
		/// <summary>
		/// Color of the panel's bevel.
		/// </summary>
		private Color bevelColor = Color.Transparent;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the style of beveled edge to use when drawing the panel border.
		/// </summary>
		public BevelStyle BevelStyle
		{
			get { return bevelStyle; }
			set
			{
				if (bevelStyle != value)
				{
					bevelStyle = bevel.Style = value;
					AdjustMargins();
					if (!Suspended) OnBevelStyleChanged(new EventArgs());
				}
			}
		}

		/// <summary>
		/// Get or set which side(s) of the panel will have a beveled edge.
		/// </summary>
		public BevelBorder BevelBorder
		{
			get { return bevelBorder; }
			set
			{
				if (bevelBorder != value)
				{
					bevelBorder = bevel.Border = value;
					bevel.Visible = bevelBorder != BevelBorder.None;
					AdjustMargins();
					if (!Suspended) OnBevelBorderChanged(new EventArgs());
				}
			}
		}

		/// <summary>
		/// Gets or sets the margin amount between the bevel and the panel's content area.
		/// </summary>
		public int BevelMargin
		{
			get { return bevelMargin; }
			set
			{
				if (bevelMargin != value)
				{
					bevelMargin = value;
					AdjustMargins();
					if (!Suspended) OnBevelMarginChanged(new EventArgs());
				}
			}
		}

		/// <summary>
		/// Gets or sets the color of the panel's beveled edges.
		/// </summary>
		public virtual Color BevelColor
		{
			get { return bevelColor; }
			set
			{
				bevel.Color = bevelColor = value;
			}
		}
		#endregion

		#region Events
		/// <summary>
		/// Occurs when the panel's bevel border style has changed.
		/// </summary>
		public event EventHandler BevelBorderChanged;
		/// <summary>
		/// Occurs when the panel's bevel styl has changed.
		/// </summary>
		public event EventHandler BevelStyleChanged;
		/// <summary>
		/// Occurs when the panel's bevel margin has changed.
		/// </summary>
		public event EventHandler BevelMarginChanged;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new Panel control.
		/// </summary>
		/// <param name="manager">GUI manager for the control.</param>
		public Panel(Manager manager)
			: base(manager)
		{
			Passive = false;
			CanFocus = false;
			Width = 64;
			Height = 64;

			bevel = new Bevel(Manager);
		}
		#endregion

		#region Init
		/// <summary>
		/// Initializes the Panel control.
		/// </summary>
		public override void Init()
		{
			base.Init();

			bevel.Init();
			bevel.Style = bevelStyle;
			bevel.Border = bevelBorder;
			bevel.Left = 0;
			bevel.Top = 0;
			bevel.Width = Width;
			bevel.Height = Height;
			bevel.Color = bevelColor;
			bevel.Visible = (bevelBorder != BevelBorder.None);
			bevel.Anchor = Anchors.Left | Anchors.Top | Anchors.Right | Anchors.Bottom;
			Add(bevel, false);
			AdjustMargins();
		}
		#endregion

		#region Init Skin
		/// <summary>
		/// Initializes the skin of the panel control.
		/// </summary>
		protected internal override void InitSkin()
		{
			base.InitSkin();
			Skin = new SkinControl(Manager.Skin.Controls["Panel"]);
		}
		#endregion

		#region Adjust Margins
		/// <summary>
		/// Updates the client area margins of the panel based on style, borders, and skin settings.
		/// </summary>
		protected override void AdjustMargins()
		{
			int l = 0;
			int t = 0;
			int r = 0;
			int b = 0;
			int s = bevelMargin;

			// Has a border?
			if (bevelBorder != BevelBorder.None)
			{
				if (bevelStyle != BevelStyle.Flat)
				{
					s += 2;
				}
				else
				{
					s += 1;
				}

				// Adjust each side of the client margins if the border is set.
				if (bevelBorder == BevelBorder.Left || bevelBorder == BevelBorder.All)
				{
					l = s;
				}

				if (bevelBorder == BevelBorder.Top || bevelBorder == BevelBorder.All)
				{
					t = s;
				}
				
				if (bevelBorder == BevelBorder.Right || bevelBorder == BevelBorder.All)
				{
					r = s;
				}
				
				if (bevelBorder == BevelBorder.Bottom || bevelBorder == BevelBorder.All)
				{
					b = s;
				}
			}

			// Update client margins accounting for skin settings and bevel margins.
			ClientMargins = new Margins(Skin.ClientMargins.Left + l, Skin.ClientMargins.Top + t, Skin.ClientMargins.Right + r, Skin.ClientMargins.Bottom + b);

			base.AdjustMargins();
		}
		#endregion
				
		#region Draw Control
		/// <summary>
		/// Draws the panel control.
		/// </summary>
		/// <param name="renderer">Render management object.</param>
		/// <param name="rect">Destination region where the panel will be drawn.</param>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		protected override void DrawControl(Renderer renderer, Rectangle rect, GameTime gameTime)
		{
			// Grab location, dimensions, and margins.
			int x = rect.Left;
			int y = rect.Top;
			int w = rect.Width;
			int h = rect.Height;
			int s = bevelMargin;

			if (bevelBorder != BevelBorder.None)
			{
				if (bevelStyle != BevelStyle.Flat)
				{
					s += 2;
				}
				else
				{
					s += 1;
				}

				// Adjust position and dimensions so everything is drawn inside the borders of the panel.
				if (bevelBorder == BevelBorder.Left || bevelBorder == BevelBorder.All)
				{
					x += s;
					w -= s;
				}
				if (bevelBorder == BevelBorder.Top || bevelBorder == BevelBorder.All)
				{
					y += s;
					h -= s;
				}
				if (bevelBorder == BevelBorder.Right || bevelBorder == BevelBorder.All)
				{
					w -= s;
				}
				if (bevelBorder == BevelBorder.Bottom || bevelBorder == BevelBorder.All)
				{
					h -= s;
				}
			}

			base.DrawControl(renderer, new Rectangle(x, y, w, h), gameTime);
		}
		#endregion

		#region On Bevel Border Changed
		/// <summary>
		/// Handles bevel border changes for the panel.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnBevelBorderChanged(EventArgs e)
		{
			if (BevelBorderChanged != null) BevelBorderChanged.Invoke(this, e);
		}
		#endregion

		#region On Bevel Style Changed Event Handler
		/// <summary>
		/// Handles bevel style changes for the panel.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnBevelStyleChanged(EventArgs e)
		{
			if (BevelStyleChanged != null) BevelStyleChanged.Invoke(this, e);
		}
		#endregion

		#region On Bevel Margin Changed Event Handler
		/// <summary>
		/// Handles bevel margin changes for the panel.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnBevelMarginChanged(EventArgs e)
		{
			if (BevelMarginChanged != null) BevelMarginChanged.Invoke(this, e);
		}
		#endregion
	}
}
