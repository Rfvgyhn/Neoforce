////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: Window.cs                                    //
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
using Microsoft.Xna.Framework.Input;
#endregion

namespace TomShane.Neoforce.Controls
{
	/// <summary>
	/// Maps gamepad buttons to window actions.
	/// </summary>
	public class WindowGamePadActions : GamePadActions
	{
		public GamePadButton Accept = GamePadButton.Start;
		public GamePadButton Cancel = GamePadButton.Back;
	}

	/// <summary>
	/// Represents a window with a close button.
	/// </summary>
	public class Window : ModalContainer
	{
		#region Constants
		/// <summary>
		/// String for accessing the skin object of the window.
		/// </summary>
		private const string skWindow = "Window";
		/// <summary>
		/// String for accessing the window layer.
		/// </summary>
		private const string lrWindow = "Control";
		/// <summary>
		/// String for accessing the window's caption area layer.
		/// </summary>
		private const string lrCaption = "Caption";
		/// <summary>
		/// String for accessing the window's top border layer.
		/// </summary>
		private const string lrFrameTop = "FrameTop";
		/// <summary>
		/// String for accessing the window's left border layer.
		/// </summary>
		private const string lrFrameLeft = "FrameLeft";
		/// <summary>
		/// String for accessing the window's right border layer.
		/// </summary>
		private const string lrFrameRight = "FrameRight";
		/// <summary>
		/// String for accessing the window's bottom border layer.
		/// </summary>
		private const string lrFrameBottom = "FrameBottom";
		/// <summary>
		/// String for accessing the window icon layer.
		/// </summary>
		private const string lrIcon = "Icon";
		/// <summary>
		/// String for accessing the skin object for the close button of the window.
		/// </summary>
		private const string skButton = "Window.CloseButton";
		/// <summary>
		/// String for accessing the window button control layer.
		/// </summary>
		private const string lrButton = "Control";
		/// <summary>
		/// String for accessing the skin object for the window shadow.
		/// </summary>
		private const string skShadow = "Window.Shadow";
		/// <summary>
		/// String for accessing the window shadow layer.
		/// </summary>
		private const string lrShadow = "Control";
		#endregion

		#region Fields
		/// <summary>
		/// The button that closes the window.
		/// </summary>
		private Button btnClose;
		/// <summary>
		/// Indicates if the close button is drawn.
		/// </summary>
		private bool closeButtonVisible = true;
		/// <summary>
		/// Indicates if the window icon is drawn.
		/// </summary>
		private bool iconVisible = true;
		/// <summary>
		/// Window icon image.
		/// </summary>
		private Texture2D icon = null;
		/// <summary>
		/// Indicates if the window shadow is drawn.
		/// </summary>
		private bool shadow = true;
		/// <summary>
		/// Indicates if the window caption is drawn.
		/// </summary>
		private bool captionVisible = true;
		/// <summary>
		/// Indicates if the window border is drawn.
		/// </summary>
		private bool borderVisible = true;
		/// <summary>
		/// Alpha value of the window.
		/// </summary>
		private byte oldAlpha = 255;
		/// <summary>
		/// Alpha value used when dragging the window.
		/// </summary>
		private byte dragAlpha = 200;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the window icon image.
		/// </summary>
		public virtual Texture2D Icon
		{
			get { return icon; }
			set { icon = value; }
		}

		/// <summary>
		/// Indicates if the window should draw its shadow.
		/// </summary>
		public virtual bool Shadow
		{
			get { return shadow; }
			set { shadow = value; }
		}

		/// <summary>
		/// Indicates if the window should draw its close button.
		/// </summary>
		public virtual bool CloseButtonVisible
		{
			get
			{
				return closeButtonVisible;
			}
			set
			{
				closeButtonVisible = value;
				if (btnClose != null) btnClose.Visible = value;
			}
		}

		/// <summary>
		/// Indicates if the window should draw its icon.
		/// </summary>
		public virtual bool IconVisible
		{
			get
			{
				return iconVisible;
			}
			set
			{
				iconVisible = value;
			}
		}

		/// <summary>
		/// Indicates if the window should draw its caption.
		/// </summary>
		public virtual bool CaptionVisible
		{
			get { return captionVisible; }
			set
			{
				captionVisible = value;
				AdjustMargins();
			}
		}

		/// <summary>
		/// Indicates if the window should draw its border.
		/// </summary>
		public virtual bool BorderVisible
		{
			get { return borderVisible; }
			set
			{
				borderVisible = value;
				AdjustMargins();
			}
		}
		 
		/// <summary>
		/// Gets or sets the alpha value that should be applied to the window during a drag operation.
		/// </summary>
		public virtual byte DragAlpha
		{
			get { return dragAlpha; }
			set { dragAlpha = value; }
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new Window.
		/// </summary>
		/// <param name="manager">GUI manager for the window.</param>
		public Window(Manager manager)
			: base(manager)
		{
			// Make sure all the required layers are defined for the window's skin.
			CheckLayer(Skin, lrWindow);
			CheckLayer(Skin, lrCaption);
			CheckLayer(Skin, lrFrameTop);
			CheckLayer(Skin, lrFrameLeft);
			CheckLayer(Skin, lrFrameRight);
			CheckLayer(Skin, lrFrameBottom);
			CheckLayer(Manager.Skin.Controls[skButton], lrButton);
			CheckLayer(Manager.Skin.Controls[skShadow], lrShadow);

			SetDefaultSize(640, 480);
			SetMinimumSize(100, 75);

			// Set up the window close button.
			btnClose = new Button(manager);
			btnClose.Skin = new SkinControl(Manager.Skin.Controls[skButton]);
			btnClose.Init();
			btnClose.Detached = true;
			btnClose.CanFocus = false;
			btnClose.Text = null;
			btnClose.Click += new EventHandler(btnClose_Click);
			btnClose.SkinChanged += new EventHandler(btnClose_SkinChanged);

			// Set up window margins.
			AdjustMargins();

			AutoScroll = true;
			Movable = true;
			Resizable = true;
			Center();

			Add(btnClose, false);

			oldAlpha = Alpha;
		}
		#endregion

		#region Dispose
		/// <summary>
		/// Cleans up window resources.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
			}
			base.Dispose(disposing);
		}
		#endregion
		
		#region Init
		/// <summary>
		/// Initializes the window.
		/// </summary>
		public override void Init()
		{
			base.Init();

			// Set up the window's close button.
			SkinLayer l = btnClose.Skin.Layers[lrButton];
			btnClose.Width = l.Width - btnClose.Skin.OriginMargins.Horizontal;
			btnClose.Height = l.Height - btnClose.Skin.OriginMargins.Vertical;
			btnClose.Left = OriginWidth - Skin.OriginMargins.Right - btnClose.Width + l.OffsetX;
			btnClose.Top = Skin.OriginMargins.Top + l.OffsetY;
			btnClose.Anchor = Anchors.Top | Anchors.Right;

			//SkinControl sc = new SkinControl(ClientArea.Skin);
			//sc.Layers[0] = Skin.Layers[lrWindow];
			//ClientArea.Color = Color.Transparent;
			//ClientArea.BackColor = Color.Transparent;
			//ClientArea.Skin = sc;                     
		}
		#endregion

		#region Init Skin
		/// <summary>
		/// Initializes the skin of the window.
		/// </summary>
		protected internal override void InitSkin()
		{
			base.InitSkin();
			Skin = new SkinControl(Manager.Skin.Controls[skWindow]);
			AdjustMargins();
		}
		#endregion

		#region Close Button Skin Changed Event Handler
		/// <summary>
		/// Handles reskinning the close button when the skin changes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void btnClose_SkinChanged(object sender, EventArgs e)
		{
			btnClose.Skin = new SkinControl(Manager.Skin.Controls[skButton]);
		}
		#endregion

		#region Render
		/// <summary>
		/// Draws the window.
		/// </summary>
		/// <param name="renderer">Render management object.</param>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		internal override void Render(Renderer renderer, GameTime gameTime)
		{
			// Draw the shadow first if the window is displayed and the shadow is being used.
			if (Visible && Shadow)
			{
				SkinControl c = Manager.Skin.Controls[skShadow];
				SkinLayer l = c.Layers[lrShadow];

				Color cl = Color.FromNonPremultiplied(l.States.Enabled.Color.R, l.States.Enabled.Color.G, l.States.Enabled.Color.B, Alpha);

				renderer.Begin(BlendingMode.Default);
				renderer.DrawLayer(l, new Rectangle(Left - c.OriginMargins.Left, Top - c.OriginMargins.Top, Width + c.OriginMargins.Horizontal, Height + c.OriginMargins.Vertical), cl, 0);
				renderer.End();
			}

			// Draw the window.
			base.Render(renderer, gameTime);
		}
		#endregion

		#region Get Icon Rect
		/// <summary>
		/// Creates the rectangle where the window icon should be displayed.
		/// </summary>
		/// <returns>Returns the window icon's destination region where it will be drawn. </returns>
		private Rectangle GetIconRect()
		{
			SkinLayer l1 = Skin.Layers[lrCaption];
			SkinLayer l5 = Skin.Layers[lrIcon];

			// Icon will be scaled to fit in the space alloted by the caption bar.
			int s = l1.Height - l1.ContentMargins.Vertical;

			// Return the destination rectangle for the window icon. Left side of the window caption.
			return new Rectangle(DrawingRect.Left + l1.ContentMargins.Left + l5.OffsetX,
								 DrawingRect.Top + l1.ContentMargins.Top + l5.OffsetY,
								 s, s);
		}
		#endregion

		#region Draw Control
		/// <summary>
		/// Draws the window and all child controls.
		/// </summary>
		/// <param name="renderer">Render management object.</param>
		/// <param name="rect">Destination region where the window will be drawn.</param>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		protected override void DrawControl(Renderer renderer, Rectangle rect, GameTime gameTime)
		{
			SkinLayer l1 = captionVisible ? Skin.Layers[lrCaption] : Skin.Layers[lrFrameTop];
			SkinLayer l2 = Skin.Layers[lrFrameLeft];
			SkinLayer l3 = Skin.Layers[lrFrameRight];
			SkinLayer l4 = Skin.Layers[lrFrameBottom];
			SkinLayer l5 = Skin.Layers[lrIcon];
			LayerStates s1, s2, s3, s4;
			SpriteFont f1 = l1.Text.Font.Resource;
			Color c1 = l1.Text.Colors.Enabled;

			// Window has focus?
			if ((Focused || (Manager.FocusedControl != null && Manager.FocusedControl.Root == this.Root)) && ControlState != ControlState.Disabled)
			{
				s1 = l1.States.Focused;
				s2 = l2.States.Focused;
				s3 = l3.States.Focused;
				s4 = l4.States.Focused;
				c1 = l1.Text.Colors.Focused;
			}

			// Window is disabled?
			else if (ControlState == ControlState.Disabled)
			{
				s1 = l1.States.Disabled;
				s2 = l2.States.Disabled;
				s3 = l3.States.Disabled;
				s4 = l4.States.Disabled;
				c1 = l1.Text.Colors.Disabled;
			}

			// Window not active or child control has focus?
			else
			{
				s1 = l1.States.Enabled;
				s2 = l2.States.Enabled;
				s3 = l3.States.Enabled;
				s4 = l4.States.Enabled;
				c1 = l1.Text.Colors.Enabled;
			}

			// Draw the window layer.
			renderer.DrawLayer(Skin.Layers[lrWindow], rect, Skin.Layers[lrWindow].States.Enabled.Color, Skin.Layers[lrWindow].States.Enabled.Index);

			// Need to draw the window border?
			if (borderVisible)
			{
				// Draw caption layer or top frame layer, then draw the left, right, and bottom frame layers.
				renderer.DrawLayer(l1, new Rectangle(rect.Left, rect.Top, rect.Width, l1.Height), s1.Color, s1.Index);
				renderer.DrawLayer(l2, new Rectangle(rect.Left, rect.Top + l1.Height, l2.Width, rect.Height - l1.Height - l4.Height), s2.Color, s2.Index);
				renderer.DrawLayer(l3, new Rectangle(rect.Right - l3.Width, rect.Top + l1.Height, l3.Width, rect.Height - l1.Height - l4.Height), s3.Color, s3.Index);
				renderer.DrawLayer(l4, new Rectangle(rect.Left, rect.Bottom - l4.Height, rect.Width, l4.Height), s4.Color, s4.Index);

				// Draw the window icon if there is one and the window caption is displayed.
				if (iconVisible && (icon != null || l5 != null) && captionVisible)
				{
					Texture2D i = (icon != null) ? icon : l5.Image.Resource;
					renderer.Draw(i, GetIconRect(), Color.White);
				}

				int icosize = 0;
				if (l5 != null && iconVisible && captionVisible)
				{
					icosize = l1.Height - l1.ContentMargins.Vertical + 4 + l5.OffsetX;
				}

				// Draw the close button if visible.
				int closesize = 0;
				if (btnClose.Visible)
				{
					closesize = btnClose.Width - (btnClose.Skin.Layers[lrButton].OffsetX);
				}

				// Create the rectangle defining the remaining caption area to draw text in.
				Rectangle r = new Rectangle(rect.Left + l1.ContentMargins.Left + icosize,
											rect.Top + l1.ContentMargins.Top,
											rect.Width - l1.ContentMargins.Horizontal - closesize - icosize,
											l1.Height - l1.ContentMargins.Top - l1.ContentMargins.Bottom);
				int ox = l1.Text.OffsetX;
				int oy = l1.Text.OffsetY;

				// Draw the window title in the caption area remaining.
				renderer.DrawString(f1, Text, r, c1, l1.Text.Alignment, ox, oy, true);
			}
		}
		#endregion

		#region Close Button Click Event Handler
		/// <summary>
		/// Handles closing the window when the close button is clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void btnClose_Click(object sender, EventArgs e)
		{
			Close(ModalResult = ModalResult.Cancel);
		}
		#endregion

		#region Center
		/// <summary>
		/// Centers the window on screen.
		/// </summary>
		public virtual void Center()
		{
			Left = (Manager.ScreenWidth / 2) - (Width / 2);
			Top = (Manager.ScreenHeight - Height) / 2;
		}
		#endregion

		#region On Resize Event Handler
		/// <summary>
		/// Handles resizing of the window.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(ResizeEventArgs e)
		{
			SetMovableArea();
			base.OnResize(e);
		}
		#endregion

		#region On Move Begin Event Handler
		/// <summary>
		/// Handler for when the window starts a move event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMoveBegin(EventArgs e)
		{
			base.OnMoveBegin(e);

			// Swap the current alpha values.
			try
			{
				oldAlpha = Alpha;
				Alpha = dragAlpha;
			}
			catch
			{
			}
		}
		#endregion

		#region On Move End Event Handler
		/// <summary>
		/// Handler for when the window completes a move event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMoveEnd(EventArgs e)
		{
			base.OnMoveEnd(e);

			// Reset to original alpha value.
			try
			{
				Alpha = oldAlpha;
			}
			catch
			{
			}
		}
		#endregion

		#region On Double Click Event Handler
		/// <summary>
		/// Handles double click events for the window. 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnDoubleClick(EventArgs e)
		{
			base.OnDoubleClick(e);

			MouseEventArgs ex = (e is MouseEventArgs) ? (MouseEventArgs)e : new MouseEventArgs();

			// Double clicking the Icon closes the window.
			if (IconVisible && ex.Button == MouseButton.Left)
			{
				Rectangle r = GetIconRect();
				r.Offset(AbsoluteLeft, AbsoluteTop);
				if (r.Contains(ex.Position))
				{
					Close();
				}
			}
		}
		#endregion

		#region Adjust Margins
		/// <summary>
		/// Adjusts the client area margins based on the visibility of the caption area and window border.
		/// </summary>
		protected override void AdjustMargins()
		{
			if (captionVisible && borderVisible)
			{
				// Adjust margins to account for the window border and caption area.
				ClientMargins = new Margins(Skin.ClientMargins.Left, Skin.Layers[lrCaption].Height, Skin.ClientMargins.Right, Skin.ClientMargins.Bottom);
			}

			else if (!captionVisible && borderVisible)
			{
				// Adjust margins to account for the window border.
				ClientMargins = new Margins(Skin.ClientMargins.Left, Skin.ClientMargins.Top, Skin.ClientMargins.Right, Skin.ClientMargins.Bottom);
			}

			else if (!borderVisible)
			{
				// Nothing to account for.
				ClientMargins = new Margins(0, 0, 0, 0);
			}

			// Need to display the close button?
			if (btnClose != null)
			{
				btnClose.Visible = closeButtonVisible && captionVisible && borderVisible;
			}

			SetMovableArea();
			base.AdjustMargins();
		}
		#endregion

		#region Set Movable Area
		/// <summary>
		/// Sets the region where the window can be moved to.
		/// </summary>
		private void SetMovableArea()
		{
			if (captionVisible && borderVisible)
			{
				MovableArea = new Rectangle(Skin.OriginMargins.Left, Skin.OriginMargins.Top, Width, Skin.Layers[lrCaption].Height - Skin.OriginMargins.Top);
			}
			else if (!captionVisible)
			{
				MovableArea = new Rectangle(0, 0, Width, Height);
			}
		}
		#endregion
	}
}
