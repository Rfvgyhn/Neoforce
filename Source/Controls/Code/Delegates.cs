////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: Delegates.cs                                 //
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
namespace TomShane.Neoforce.Controls
{
	/// <summary>
	/// Defines the signature for a device event handler.
	/// </summary>
	/// <param name="e"></param>
	public delegate void DeviceEventHandler(DeviceEventArgs e);
	/// <summary>
	/// Defines the signature for a skin event handler.
	/// </summary>
	/// <param name="e"></param>
	public delegate void SkinEventHandler(EventArgs e);
	/// <summary>
	/// Defines the signature for event handlers.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void EventHandler(object sender, EventArgs e);
	/// <summary>
	/// Defines the signature for mouse event handlers.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void MouseEventHandler(object sender, MouseEventArgs e);
	/// <summary>
	/// Defines the signature for key event handlers.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void KeyEventHandler(object sender, KeyEventArgs e);
	/// <summary>
	/// Defines the signature for gamepad event handlers.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void GamePadEventHandler(object sender, GamePadEventArgs e);
	/// <summary>
	/// Defines the signature for draw event handlers.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void DrawEventHandler(object sender, DrawEventArgs e);
	/// <summary>
	/// Defines the signature for move event handlers.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void MoveEventHandler(object sender, MoveEventArgs e);
	/// <summary>
	/// Defines the signature for resize event handlers.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void ResizeEventHandler(object sender, ResizeEventArgs e);
	/// <summary>
	/// Defines the signature for window closing event handlers.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void WindowClosingEventHandler(object sender, WindowClosingEventArgs e);
	/// <summary>
	/// Defines the signature for window closed event handlers.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void WindowClosedEventHandler(object sender, WindowClosedEventArgs e);
	/// <summary>
	/// Defines the signature for console message event handlers.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void ConsoleMessageEventHandler(object sender, ConsoleMessageEventArgs e);
}
