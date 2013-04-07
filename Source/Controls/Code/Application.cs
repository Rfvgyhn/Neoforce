////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: Application.cs                               //
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
using TomShane.Neoforce.Controls;
using System;

#if (!XBOX && !XBOX_FAKE)
using System.Windows.Forms;
#endif
#endregion

namespace TomShane.Neoforce.Controls
{
	/// <summary>
	/// Base class for your Neoforce application. Use it.
	/// </summary>
	public class Application : Game
	{
		#region Fields
		/// <summary>
		/// Graphics device manager for the application.
		/// </summary>
		private GraphicsDeviceManager graphics;
		/// <summary>
		/// GUI control manager for the application.
		/// </summary>
		private Manager manager;
		/// <summary>
		/// Sprite batch object for the application. 
		/// </summary>
		private SpriteBatch sprite;
		/// <summary>
		/// Indicates if the background should be cleared to the BG color by the application. ???
		/// </summary>
		private bool clearBackground = false;
		/// <summary>
		/// Application background color.
		/// </summary>
		private Color backgroundColor = Color.Black;
		/// <summary>
		/// Image to use as the application background.
		/// </summary>
		private Texture2D backgroundImage;
		/// <summary>
		/// ???
		/// </summary>
		private Window mainWindow;
		/// <summary>
		/// Indicates if the application should create and use the MainWindow. 
		/// </summary>
		private bool appWindow = false;
		/// <summary>
		/// Current position of the mouse cursor.
		/// </summary>
		private Point mousePos = Point.Zero;
		/// <summary>
		/// ???
		/// </summary>
		private bool systemBorder = true;
		/// <summary>
		/// ???
		/// </summary>
		private bool fullScreenBorder = true;
		/// <summary>
		/// Indicates whether the exit confirmation dialog should be shown before
		/// closing the application. 
		/// </summary>
		private bool exitConfirmation = true;
		/// <summary>
		/// Indicates whether a request to terminate the application has been received.
		/// </summary>
		private bool exit = false;
		/// <summary>
		/// Exit confirmation dialog object.
		/// </summary>
		private ExitDialog exitDialog = null;
#if (!XBOX && !XBOX_FAKE)
		/// <summary>
		/// Tracks the mouse button state when running on Windows.
		/// </summary>
		private bool mouseDown = false;
#endif
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the graphics device manager for the application.
		/// </summary>
		public virtual GraphicsDeviceManager Graphics
		{
			get { return graphics; }
			set { graphics = value; }
		}
		
		/// <summary>
		/// Gets or sets the GUI manager for the application. 
		/// </summary>
		public virtual Manager Manager
		{
			get { return manager; }
			set { manager = value; }
		}
		
		/// <summary>
		/// ???
		/// </summary>
		public virtual bool ClearBackground
		{
			get { return clearBackground; }
			set { clearBackground = value; }
		}
		
		/// <summary>
		/// ???
		/// </summary>
		public virtual Color BackgroundColor
		{
			get { return backgroundColor; }
			set { backgroundColor = value; }
		}
		
		/// <summary>
		/// Image to use as the application background.
		/// </summary>
		public virtual Texture2D BackgroundImage
		{
			get { return backgroundImage; }
			set { backgroundImage = value; }
		}
		
		/// <summary>
		/// ???
		/// </summary>
		public virtual Window MainWindow
		{
			get { return mainWindow; }
		}
		
		/// <summary>
		/// Indicates whether the system border should be drawn ???
		/// </summary>
		public virtual bool SystemBorder
		{
			get { return systemBorder; }
			set { systemBorder = value; }
		}
		
		/// <summary>
		/// Indicates whether the window border should be drawn in full screen mode. ???
		/// </summary>
		public virtual bool FullScreenBorder
		{
			get { return fullScreenBorder; }
			set { fullScreenBorder = value; }
		}
		
		/// <summary>
		/// Indicates if the default exit confirmation dialog will be 
		/// shown when the application is about to close.
		/// </summary>
		public virtual bool ExitConfirmation
		{
			get { return exitConfirmation; }
			set { exitConfirmation = value; }
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Creates an application using the "Default" skin and not using the Main Window.
		/// </summary>
		public Application()
			: this("Default", false)
		{
		}
		
		/// <summary>
		/// Creates an application using the specified skin and not using the Main Window.
		/// </summary>
		/// <param name="skin">Name of the skin to load.</param>
		public Application(string skin)
			: this(skin, false)
		{
		}
		
		/// <summary>
		/// Creates an application using the "Default" skin and using the Main Window.
		/// </summary>
		/// <param name="appWindow">Indicates if the application should create its MainWindow member.</param>
		public Application(bool appWindow)
			: this("Default", appWindow)
		{
		}
		
		/// <summary>
		/// Creates an application using the specified skin and using the Main Window.
		/// </summary>
		/// <param name="skin">Name of the skin to load.</param>
		/// <param name="appWindow">Indicates if the application should create its MainWindow member.</param>
		public Application(string skin, bool appWindow)
		{
			this.appWindow = appWindow;

			// Create the graphics device manager for the application.
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = 1024;
			graphics.PreferredBackBufferHeight = 768;
			graphics.PreferredBackBufferFormat = SurfaceFormat.Color;
			graphics.IsFullScreen = false;
			graphics.PreferMultiSampling = false;
			graphics.SynchronizeWithVerticalRetrace = false;
			graphics.DeviceReset += new EventHandler<System.EventArgs>(Graphics_DeviceReset);

			IsFixedTimeStep = false;
			IsMouseVisible = true;

			// Create the GUI manager for the application.
			manager = new Manager(this, graphics, skin);
			manager.AutoCreateRenderTarget = false;
			manager.TargetFrames = 60;
			manager.WindowClosing += new WindowClosingEventHandler(Manager_WindowClosing);
		}
		#endregion

		#region Dispose
		/// <summary>
		/// Releases resources used by the GUI manager and the SpriteBatch objects.
		/// </summary>
		/// <param name="disposing">Indicates if the resources should be released from memory.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Dispose of the GUI manager and sprite batch object.
				if (manager != null) manager.Dispose();
				if (sprite != null) sprite.Dispose();
			}
		
			base.Dispose(disposing);
		}
		#endregion

		#region Initialize
		/// <summary>
		/// Initializes the application.
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();

			// Initialize the GUI manager and create the application's render target.
			manager.Initialize();
			manager.RenderTarget = CreateRenderTarget();
			manager.Input.InputOffset = new InputOffset(0, 0, manager.ScreenWidth / (float)manager.TargetWidth, manager.ScreenHeight / (float)manager.TargetHeight);

			// Create the sprite batch object.
			sprite = new SpriteBatch(GraphicsDevice);

#if (!XBOX && !XBOX_FAKE)
			manager.Window.BackColor = System.Drawing.Color.Black;
			manager.Window.FormBorderStyle = systemBorder ? System.Windows.Forms.FormBorderStyle.FixedDialog : System.Windows.Forms.FormBorderStyle.None;

			// Wire up the mouse event handlers if running on Windows.
			manager.Input.MouseMove += new MouseEventHandler(Input_MouseMove);
			manager.Input.MouseDown += new MouseEventHandler(Input_MouseDown);
			manager.Input.MouseUp += new MouseEventHandler(Input_MouseUp);
#endif
			// Create the application main window?
			if (appWindow)
			{
				mainWindow = CreateMainWindow();
			}

			// Initialize the main window of the application.
			InitMainWindow();
		}
		#endregion

		#region Update
		/// <summary>
		/// Updates the application.
		/// </summary>
		/// <param name="gameTime">Snapshot of the game's timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			manager.Update(gameTime);
		}
		#endregion

		#region Draw
		/// <summary>
		/// Draws the application.
		/// </summary>
		/// <param name="gameTime">Snapshot of the game's timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			// Start drawing the application.
			manager.BeginDraw(gameTime);

			// Should the application clear the backbuffer?
			if (clearBackground)
			{
				GraphicsDevice.Clear(backgroundColor);
			}
			
			// BG image and no main window.
			if (backgroundImage != null && mainWindow == null)
			{
				// Draw the BG image at the requested size. 
				int sx = manager.TargetWidth;
				int sy = manager.TargetHeight;
				sprite.Begin();
				sprite.Draw(backgroundImage, new Rectangle(0, 0, sx, sy), Color.White);
				sprite.End();
			}

			// Let the game do its draw thing.
			base.Draw(gameTime);

			// Additional drawing logic for your application.
			DrawScene(gameTime);

			manager.EndDraw(new Rectangle(0, 0, Manager.ScreenWidth, Manager.ScreenHeight));
		}
		#endregion

		#region Create Main Window
		/// <summary>
		/// Creates the application's Main Window.
		/// </summary>
		/// <returns>A new Window instance.</returns>
		protected virtual Window CreateMainWindow()
		{
			return new Window(manager);
		}
		#endregion

		#region Init Main Window
		/// <summary>
		/// Initializes the application's Main Window and passes it off the the GUI Manager.
		/// </summary>
		protected virtual void InitMainWindow()
		{
			if (mainWindow != null)
			{
				if (!mainWindow.Initialized) 
				{
					mainWindow.Init();
				}

				mainWindow.Alpha = 255;
				mainWindow.Width = manager.TargetWidth;
				mainWindow.Height = manager.TargetHeight;
				mainWindow.Shadow = false;
				mainWindow.Left = 0;
				mainWindow.Top = 0;
				mainWindow.CloseButtonVisible = true;
				mainWindow.Resizable = false;
				mainWindow.Movable = false;
				mainWindow.Text = this.Window.Title;
				mainWindow.Closing += new WindowClosingEventHandler(MainWindow_Closing);
				mainWindow.ClientArea.Draw += new DrawEventHandler(MainWindow_Draw);
				mainWindow.BorderVisible = mainWindow.CaptionVisible = (!systemBorder && !Graphics.IsFullScreen) || (Graphics.IsFullScreen && fullScreenBorder);
				mainWindow.StayOnBack = true;

				manager.Add(mainWindow);

				mainWindow.SendToBack();
			}
		}
		#endregion

		#region Main Window Draw Event Handler
		/// <summary>
		/// Raises the application's draw event.
		/// </summary>
		private void MainWindow_Draw(object sender, DrawEventArgs e)
		{
			if (backgroundImage != null && mainWindow != null)
			{
				e.Renderer.Draw(backgroundImage, e.Rectangle, Color.White);
			}
		}
		#endregion

		#region Exit
		/// <summary>
		/// Sets the exit flag and begins shutting down the application.
		/// </summary>
		public new virtual void Exit()
		{
			exit = true;
			base.Exit();
		}
		#endregion

		#region Manager Window Closing Event Handler
		/// <summary>
		/// Handles the Window Closing event. 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Manager_WindowClosing(object sender, WindowClosingEventArgs e)
		{
			e.Cancel = !exit && exitConfirmation;

			// Need to create the exit confirmation dialog?
			if (!exit && exitConfirmation && exitDialog == null)
			{
				exitDialog = new ExitDialog(Manager);
				exitDialog.Init();
				exitDialog.Closed += new WindowClosedEventHandler(closeDialog_Closed);
				exitDialog.ShowModal();
				Manager.Add(exitDialog);
			}

			// Nope, just exit. 
			else if (!exitConfirmation)
			{
				Exit();
			}
		}
		#endregion

		#region Close Dialog Closed Event Handler
		/// <summary>
		/// Handles the Dialog Closed event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void closeDialog_Closed(object sender, WindowClosedEventArgs e)
		{
			// Check dialog resule and see if we need to shut down.
			if ((sender as Dialog).ModalResult == ModalResult.Yes)
			{
				Exit();
			}

			// Unhook event handlers and dispose of the dialog.
			else
			{
				exit = false;
				exitDialog.Closed -= closeDialog_Closed;
				exitDialog.Dispose();
				exitDialog = null;
				if (mainWindow != null) mainWindow.Focused = true;
			}
		}
		#endregion

		#region Main Window Closing Event Handler
		/// <summary>
		/// Handles the main window's closing event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MainWindow_Closing(object sender, WindowClosingEventArgs e)
		{
			// Let the GUI manager handle it.
			e.Cancel = true;
			Manager_WindowClosing(sender, e);
		}
		#endregion

		#region Graphics Device Reset Event Handler
		/// <summary>
		/// Handles the graphics device reset event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Graphics_DeviceReset(object sender, System.EventArgs e)
		{
			// Recreate the render target if needed.
			if (Manager.RenderTarget != null)
			{
				Manager.RenderTarget.Dispose();
				Manager.RenderTarget = CreateRenderTarget();
				Manager.Input.InputOffset = new InputOffset(0, 0, Manager.ScreenWidth / (float)Manager.TargetWidth, Manager.ScreenHeight / (float)Manager.TargetHeight);
			}

			// Reset the main window dimensions if needed.
			if (mainWindow != null)
			{
				mainWindow.Height = Manager.TargetHeight;
				mainWindow.Width = Manager.TargetWidth;
				mainWindow.BorderVisible = mainWindow.CaptionVisible = (!systemBorder && !Graphics.IsFullScreen) || (Graphics.IsFullScreen && fullScreenBorder);
			}
		}
		#endregion

		#region Create Render Target
		/// <summary>
		/// Creates a 2D texture that can be used as a render target.
		/// </summary>
		/// <returns></returns>
		protected virtual RenderTarget2D CreateRenderTarget()
		{
			return manager.CreateRenderTarget();
		}
		#endregion

		#region Draw Scene
		/// <summary>
		/// Additional drawing logic for your application can be placed here.
		/// </summary>
		/// <param name="gameTime">Snapshot of the game's timing values.</param>
		protected virtual void DrawScene(GameTime gameTime)
		{
		
		}
		#endregion

		#region Run
		/// <summary>
		/// Runs the application.
		/// </summary>
		public new void Run()
		{
			// try
			{
				base.Run();
			}
			/* catch (Exception x)
			 {
			  #if (!XBOX && !XBOX_FAKE)         
				MessageBox.Show("An unhandled exception has occurred.\n" + x.Message, Window.Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
				Manager.LogException(x);
			  #else
				throw x;
			  #endif 
			 }*/
		}
		#endregion

		#region Input Mouse Move Event Handler
#if (!XBOX && !XBOX_FAKE)
		/// <summary>
		/// Handles mouse move events on Windows.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Input_MouseMove(object sender, MouseEventArgs e)
		{
			// Support dragging the application window.
			if (mouseDown)
			{
				Manager.Window.Left = e.Position.X + Manager.Window.Left - mousePos.X;
				Manager.Window.Top = e.Position.Y + Manager.Window.Top - mousePos.Y;
			}
		}
#endif
		#endregion

		#region Input Mouse Down Event Handler
#if (!XBOX && !XBOX_FAKE)
		/// <summary>
		/// Handles mouse button down events on Windows.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Input_MouseDown(object sender, MouseEventArgs e)
		{
			// Check mouse position first if there is a chance the click can land outside the application window.
			if (e.Button == MouseButton.Left && !Graphics.IsFullScreen && !systemBorder)
			{
				// Is the mouse cursor hitting the main window but none of its controls?
				if (CheckPos(e.Position))
				{
					mouseDown = true;
					mousePos = e.Position;
				}
			}
		}
#endif
		#endregion

		#region Input Mouse Up Event Handler
#if (!XBOX && !XBOX_FAKE)
		/// <summary>
		/// Handles the mouse button up events on Windows.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Input_MouseUp(object sender, MouseEventArgs e)
		{
			mouseDown = false;
		}
#endif		
		#endregion

		#region Check Pos
#if (!XBOX && !XBOX_FAKE)
		/// <summary>
		/// Determines whether the mouse cursor is over the application Main Window (true) or
		/// if the mouse cursor is outside of the window or hovering a window control. (false)
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		private bool CheckPos(Point pos)
		{
			// Is the mouse cursor within the application window?
			if (pos.X >= 24 && pos.X <= Manager.TargetWidth - 48 &&
				pos.Y >= 0 && pos.Y <= Manager.Skin.Controls["Window"].Layers["Caption"].Height)
			{
				foreach (Control c in Manager.Controls)
				{
					// Is this a visible control other than the MainWindow?
					// Is the mouse cursor within this control's boundaries?
					if (c.Visible && c != MainWindow &&
						pos.X >= c.AbsoluteRect.Left && pos.X <= c.AbsoluteRect.Right &&
						pos.Y >= c.AbsoluteRect.Top && pos.Y <= c.AbsoluteRect.Bottom)
					{
						// Yes, mouse cursor is over this control.
						return false;
					}
				}

				// Mouse is not over any controls, but is within the application window.
				return true;
			}

			else
			{
				// Mouse is outside of the application window.
				return false;
			}
		}
#endif
		#endregion
	}
}
