////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: ImageBox.cs                                  //
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
	/// Represents a control that displays an image inside it.
	/// </summary>
	public class ImageBox : Control
	{
		#region Fields
		/// <summary>
		/// Image the control will display.
		/// </summary>
		private Texture2D image = null;
		/// <summary>
		/// Indicates how the image will be positioned/scaled when image 
		/// and control dimensions are not the same.
		/// </summary>
		private SizeMode sizeMode = SizeMode.Normal;
		/// <summary>
		/// Defines the region of the texture that is displayed in the control.
		/// </summary>
		private Rectangle sourceRect = Rectangle.Empty;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the image the control will display.
		/// </summary>
		public Texture2D Image
		{
			get { return image; }
			set
			{
				image = value;
				sourceRect = new Rectangle(0, 0, image.Width, image.Height);
				Invalidate();
				if (!Suspended) OnImageChanged(new EventArgs());
			}
		}

		/// <summary>
		/// Defines the region of the texture that is displayed in the image box control.
		/// </summary>
		public Rectangle SourceRect
		{
			get { return sourceRect; }
			set
			{
				if (value != null && image != null)
				{
					int l = value.Left;
					int t = value.Top;
					int w = value.Width;
					int h = value.Height;

					if (l < 0) l = 0;
					if (t < 0) t = 0;
					if (w > image.Width) w = image.Width;
					if (h > image.Height) h = image.Height;
					if (l + w > image.Width) w = (image.Width - l);
					if (t + h > image.Height) h = (image.Height - t);

					sourceRect = new Rectangle(l, t, w, h);
				}
				else if (image != null)
				{
					sourceRect = new Rectangle(0, 0, image.Width, image.Height);
				}
				else
				{
					sourceRect = Rectangle.Empty;
				}
				Invalidate();
			}
		}

		/// <summary>
		/// Indicates how the image will be positioned and scaled when the
		/// image and control dimensions are different sizes.
		/// </summary>
		public SizeMode SizeMode
		{
			get { return sizeMode; }
			set
			{
				if (value == SizeMode.Auto && image != null)
				{
					Width = image.Width;
					Height = image.Height;
				}
				sizeMode = value;
				Invalidate();
				if (!Suspended) OnSizeModeChanged(new EventArgs());
			}
		}
		#endregion

		#region Events 
		/// <summary>
		/// Occurs when the control's image is changed.
		/// </summary>
		public event EventHandler ImageChanged;
		/// <summary>
		/// Occurs when the control's size mode is changed.
		/// </summary>
		public event EventHandler SizeModeChanged;
		#endregion

		#region Constructor
		/// <summary>
		/// Creates a new image box control.
		/// </summary>
		/// <param name="manager">GUI manager for the image box control.</param>
		public ImageBox(Manager manager)
			: base(manager)
		{
		}
		#endregion

		#region Init
		/// <summary>
		/// Initializes the image box control.
		/// </summary>
		public override void Init()
		{
			base.Init();
			CanFocus = false;
			Color = Color.White;
		}
		#endregion

		#region Draw Control
		/// <summary>
		/// Draws the image box control.
		/// </summary>
		/// <param name="renderer">Render management object.</param>
		/// <param name="rect">Destination rectangle where the image box will be drawn.</param>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		protected override void DrawControl(Renderer renderer, Rectangle rect, GameTime gameTime)
		{
			if (image != null)
			{
				if (sizeMode == SizeMode.Normal)
				{
					renderer.Draw(image, rect.X, rect.Y, sourceRect, Color);
				}
				else if (sizeMode == SizeMode.Auto)
				{
					renderer.Draw(image, rect.X, rect.Y, sourceRect, Color);
				}
				else if (sizeMode == SizeMode.Stretched)
				{
					renderer.Draw(image, rect, sourceRect, Color);
				}
				else if (sizeMode == SizeMode.Centered)
				{
					int x = (rect.Width / 2) - (image.Width / 2);
					int y = (rect.Height / 2) - (image.Height / 2);

					renderer.Draw(image, x, y, sourceRect, Color);
				}
			}
		}
		#endregion

		#region On Image Changed Event Handler
		/// <summary>
		/// Handles when the control's image is changed.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnImageChanged(EventArgs e)
		{
			if (ImageChanged != null) ImageChanged.Invoke(this, e);
		}
		#endregion

		#region On Size Mode Changed Event Handler
		/// <summary>
		/// Handles when the control's size mode value is changed.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnSizeModeChanged(EventArgs e)
		{
			if (SizeModeChanged != null) SizeModeChanged.Invoke(this, e);
		}
		#endregion
	}
}
