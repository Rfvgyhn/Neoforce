////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: EventArgs.cs                                 //
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
using Microsoft.Xna.Framework.Input;
#endregion

namespace TomShane.Neoforce.Controls
{
	/// <summary>
	/// Base object for Neoforce events.
	/// </summary>
	public class EventArgs : System.EventArgs
	{
		#region Fields
		/// <summary>
		/// Indicates if the event the arguments belong with
		/// has been handled or not.
		/// </summary>
		public bool Handled = false;
		#endregion

		#region Constructors 
		/// <summary>
		/// Creates a new EventArgs instance.
		/// </summary>
		public EventArgs()
		{
		}
		#endregion
	}

	/// <summary>
	/// Event arguments for keyboard related events.
	/// </summary>
	public class KeyEventArgs : EventArgs
	{
		#region Fields
		/// <summary>
		/// Key for the event.
		/// </summary>
		public Keys Key = Keys.None;
		/// <summary>
		/// Indicates if the Control key modifier is pressed.
		/// </summary>
		public bool Control = false;
		/// <summary>
		/// Indicates if the Shift key modifier is pressed.
		/// </summary>
		public bool Shift = false;
		/// <summary>
		/// Indicates if the Alt key modifier is pressed.
		/// </summary>
		public bool Alt = false;
		/// <summary>
		/// Indicates if Caps Lock key is enabled.
		/// </summary>
		public bool Caps = false;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new instance of the KeyEventArgs class.
		/// </summary>
		public KeyEventArgs()
		{
		}
		
		/// <summary>
		/// Creates a new instance of the KeyEventArgs class for the specified key.
		/// </summary>
		/// <param name="key">Key argument for the event.</param>
		public KeyEventArgs(Keys key)
		{
			Key = key;
			Control = false;
			Shift = false;
			Alt = false;
			Caps = false;
		}
		
		/// <summary>
		/// Creates a new instance of the KeyEventArgs class for the specified key and modifiers.
		/// </summary>
		/// <param name="key">Key argument for the event.</param>
		/// <param name="control">Indicates if the Control key modifier is pressed.</param>
		/// <param name="shift">Indicates if the Shift key modifier is pressed.</param>
		/// <param name="alt">Indicates if the Alt key modifier is pressed.</param>
		/// <param name="caps">Indicates if Caps Lock is enabled.</param>
		public KeyEventArgs(Keys key, bool control, bool shift, bool alt, bool caps)
		{
			Key = key;
			Control = control;
			Shift = shift;
			Alt = alt;
			Caps = caps;
		}
		#endregion
	}
	
	/// <summary>
	/// Event arguments for mouse related events.
	/// </summary>
	public class MouseEventArgs : EventArgs
	{
		#region Fields
		/// <summary>
		/// Mouse state at the time of the event.
		/// </summary>
		public MouseState State = new MouseState();
		/// <summary>
		/// Mouse button state at the time of the event.
		/// </summary>
		public MouseButton Button = MouseButton.None;
		/// <summary>
		/// Mouse cursor position.
		/// </summary>
		public Point Position = new Point(0, 0);
		/// <summary>
		/// Mouse cursor position delta.
		/// </summary>
		public Point Difference = new Point(0, 0);
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new instance of the MouseEventArgs class. 
		/// </summary>
		public MouseEventArgs()
		{
		}
		
		/// <summary>
		/// Creates a new initialized instace of the MouseEventArgs class.
		/// </summary>
		/// <param name="state">Mouse state at the time of the event.</param>
		/// <param name="button">Mouse button state at the time of the event.</param>
		/// <param name="position">Mosue cursor position at the time of the event.</param>
		public MouseEventArgs(MouseState state, MouseButton button, Point position)
		{
			State = state;
			Button = button;
			Position = position;
		}
		
		/// <summary>
		/// Creates a clone of an existing MouseEventArgs object.
		/// </summary>
		/// <param name="e"></param>
		public MouseEventArgs(MouseEventArgs e)
		{
			State = e.State;
			Button = e.Button;
			Position = e.Position;
			Difference = e.Difference;
		}
		#endregion
	}

	/// <summary>
	/// Event arguments for gamepad related events.
	/// </summary>
	public class GamePadEventArgs : EventArgs
	{
		#region Fields
		/// <summary>
		/// Index of the player the gamepad is associated with. 
		/// </summary>
		public PlayerIndex PlayerIndex = PlayerIndex.One;
		/// <summary>
		/// State of the gamepad at the time of the event.
		/// </summary>
		public GamePadState State = new GamePadState();
		/// <summary>
		/// Gamepad button pressed for the event arguments.
		/// </summary>
		public GamePadButton Button = GamePadButton.None;
		/// <summary>
		/// Values of the gamepad sticks and trigs.
		/// </summary>
		public GamePadVectors Vectors;
		#endregion

		#region Constructors 
		/*
		 public GamePadEventArgs()
		 {                      
		 }*/

		/// <summary>
		/// Creates a new instance of the GamePadEventArgs class for the specified player.
		/// </summary>
		/// <param name="playerIndex">Player index of the gamepad.</param>
		public GamePadEventArgs(PlayerIndex playerIndex)
		{
			PlayerIndex = playerIndex;
		}
		
		/// <summary>
		/// Creates a new instance of the GamePadEventArgs class for the specified player.
		/// </summary>
		/// <param name="playerIndex">Player index of the gamepad.</param>
		/// <param name="button">Button pressed for the event.</param>
		public GamePadEventArgs(PlayerIndex playerIndex, GamePadButton button)
		{
			PlayerIndex = playerIndex;
			Button = button;
		}
		#endregion
	}
	
	/// <summary>
	/// Event arguments for draw related events.
	/// </summary>
	public class DrawEventArgs : EventArgs
	{
		#region Fields
		/// <summary>
		/// Rendering object for the draw event.
		/// </summary>
		public Renderer Renderer = null;
		/// <summary>
		/// Destination region where drawing will occur.
		/// </summary>
		public Rectangle Rectangle = Rectangle.Empty;
		/// <summary>
		/// Snapshot of the application's timing values.
		/// </summary>
		public GameTime GameTime = null;
		#endregion

		#region Constructors 
		/// <summary>
		/// Creates a new default instance of the DrawEventArgs class.
		/// </summary>
		public DrawEventArgs()
		{
		}
		
		/// <summary>
		/// Creates an initialized instance of the DrawEventArgs class.
		/// </summary>
		/// <param name="renderer">Render management object for the event.</param>
		/// <param name="rectangle">Destination region where drawing will occur.</param>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		public DrawEventArgs(Renderer renderer, Rectangle rectangle, GameTime gameTime)
		{
			Renderer = renderer;
			Rectangle = rectangle;
			GameTime = gameTime;
		}
		#endregion
	}
	
	/// <summary>
	/// Event arguments for resize related events.
	/// </summary>
	public class ResizeEventArgs : EventArgs
	{
		#region Fields 
		/// <summary>
		/// New width of the object being resized.
		/// </summary>
		public int Width = 0;
		/// <summary>
		/// New height of the object being resized.
		/// </summary>
		public int Height = 0;
		/// <summary>
		/// Previous width of the object being resized.
		/// </summary>
		public int OldWidth = 0;
		/// <summary>
		/// Previous height of the object being resized.
		/// </summary>
		public int OldHeight = 0;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new default instance of the ResizeEventArgs class.
		/// </summary>
		public ResizeEventArgs()
		{
		}

		/// <summary>
		/// Creates an initialized instance of the ResizeEventArgs class.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="oldWidth"></param>
		/// <param name="oldHeight"></param>
		public ResizeEventArgs(int width, int height, int oldWidth, int oldHeight)
		{
			Width = width;
			Height = height;
			OldWidth = oldWidth;
			OldHeight = oldHeight;
		}
		#endregion
	}
	
	/// <summary>
	/// Event arguments for move/drag releated events.
	/// </summary>
	public class MoveEventArgs : EventArgs
	{
		#region Fields
		/// <summary>
		/// Current X position of the object being moved.
		/// </summary>
		public int Left = 0;
		/// <summary>
		/// Current Y position of the object being moved.
		/// </summary>
		public int Top = 0;
		/// <summary>
		/// Previous X position of the object being moved.
		/// </summary>
		public int OldLeft = 0;
		/// <summary>
		/// Previous Y position of the object being moved.
		/// </summary>
		public int OldTop = 0;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a default instance of the MoveEventArgs class.
		/// </summary>
		public MoveEventArgs()
		{
		}

		/// <summary>
		/// Creates an initialized instance of the MoveEventArgs class.
		/// </summary>
		/// <param name="left">Current X position of the object being moved.</param>
		/// <param name="top">Current Y position of the object being moved.</param>
		/// <param name="oldLeft">Previous X position of the object being moved.</param>
		/// <param name="oldTop">Previous Y position of the object being moved.</param>
		public MoveEventArgs(int left, int top, int oldLeft, int oldTop)
		{
			Left = left;
			Top = top;
			OldLeft = oldLeft;
			OldTop = oldTop;
		}
		#endregion
	}
	
	/// <summary>
	/// Event arguments for creating a new graphics device
	/// </summary>
	public class DeviceEventArgs : EventArgs
	{
		#region Fields
		/// <summary>
		/// Arguments for the graphics manager PreparingDeviceSettings event.
		/// </summary>
		public PreparingDeviceSettingsEventArgs DeviceSettings = null;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a default instance of the DeviceEventArgs class.
		/// </summary>
		public DeviceEventArgs()
		{
		}

		/// <summary>
		/// Creates an initialized instance of the DeviceEventArgs class.
		/// </summary>
		/// <param name="deviceSettings">Arguments for the graphics manager PreparingDeviceSettings event.</param>
		public DeviceEventArgs(PreparingDeviceSettingsEventArgs deviceSettings)
		{
			DeviceSettings = deviceSettings;
		}
		#endregion
	}

	/// <summary>
	/// Event arguments for window closing event.
	/// </summary>
	public class WindowClosingEventArgs : EventArgs
	{
		#region Fields
		/// <summary>
		/// Indicates if the window closing operation should be canceled. 
		/// </summary>
		public bool Cancel = false;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new instance of the WindowClosingEventArgs class.
		/// </summary>
		public WindowClosingEventArgs()
		{
		}
		#endregion
	}

	/// <summary>
	/// Event arguments for window closed events.
	/// </summary>
	public class WindowClosedEventArgs : EventArgs
	{
		#region Fields
		/// <summary>
		/// Indicates if the unmanaged window resources should be released.
		/// </summary>
		public bool Dispose = false;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new instance of the WindowClosedEventArgs class.
		/// </summary>
		public WindowClosedEventArgs()
		{
		}
		#endregion
	}

	/// <summary>
	/// Event arguments for console message events.
	/// </summary>
	public class ConsoleMessageEventArgs : EventArgs
	{
		#region Fields
		/// <summary>
		/// Console message for this event.
		/// </summary>
		public ConsoleMessage Message;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a default instance of the ConsoleMessageEventArgs class.
		/// </summary>
		public ConsoleMessageEventArgs()
		{
		}

		/// <summary>
		/// Creates an initialized instance of the ConsoleMessageEventArgs class.
		/// </summary>
		/// <param name="message">Console message for the event.</param>
		public ConsoleMessageEventArgs(ConsoleMessage message)
		{
			Message = message;
		}
		#endregion
	}
}
