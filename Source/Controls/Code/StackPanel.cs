////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: StackPanel.cs                                //
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
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TomShane.Neoforce.Controls
{
	/// <summary>
	/// Container control that "stacks" child controls horizontally or vertically depending on orientation..
	/// </summary>
	public class StackPanel : Container
	{
		#region Fields
		/// <summary>
		/// Specifies how child controls are positioned. 
		/// </summary>
		private Orientation orientation;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new StackPanel control.
		/// </summary>
		/// <param name="manager">GUI manager for the control.</param>
		/// <param name="orientation">Orientation of the stack panel.</param>
		public StackPanel(Manager manager, Orientation orientation)
			: base(manager)
		{
			this.orientation = orientation;
			this.Color = Color.Transparent;
		}
		#endregion

		#region Calc Layout
		/// <summary>
		/// Sets the positions of the stack panel child controls based on orientation. 
		/// </summary>
		private void CalcLayout()
		{
			// Grab the stack panel's origin.
			int top = Top;
			int left = Left;

			// Set the position of each child control based on dimensions and layout direction.
			foreach (Control c in ClientArea.Controls)
			{
				// Respect the child control margin values.
				Margins m = c.Margins;

				// Controls stack vertically?
				if (orientation == Orientation.Vertical)
				{
					// Position control under previous control.
					top += m.Top;
					c.Top = top;
					top += c.Height;
					top += m.Bottom;
					c.Left = left;
				}

				// Controls stack horizontally?
				if (orientation == Orientation.Horizontal)
				{
					// Position control to the right of the previous control.
					left += m.Left;
					c.Left = left;
					left += c.Width;
					left += m.Right;
					c.Top = top;
				}
			}
		}
		#endregion

		#region Draw Control
		/// <summary>
		/// Draws the stack panel controls.
		/// </summary>
		/// <param name="renderer">Render management object.</param>
		/// <param name="rect">Destination region where the control will be drawn.</param>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		protected override void DrawControl(Renderer renderer, Rectangle rect, GameTime gameTime)
		{
			base.DrawControl(renderer, rect, gameTime);
		}
		#endregion

		#region On Resize Event Handler
		/// <summary>
		/// Handles resizing of the stack panel container.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(ResizeEventArgs e)
		{
			CalcLayout();
			base.OnResize(e);
		}
		#endregion
	}
}
