////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: ClipBox.cs                                   //
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
	/// ???
	/// </summary>
	public class ClipBox : Control
	{
		#region Constructor
		/// <summary>
		/// Creates a new clip box control.
		/// </summary>
		/// <param name="manager">GUI manager for the clip box control.</param>
		public ClipBox(Manager manager)
			: base(manager)
		{
			Color = Color.Transparent;
			BackColor = Color.Transparent;
			CanFocus = false;
			Passive = true;
		}
		#endregion

		#region Draw Control
		/// <summary>
		/// Draws the clip box control.
		/// </summary>
		/// <param name="renderer">Render management object.</param>
		/// <param name="rect">Destination rectangle.</param>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		protected override void DrawControl(Renderer renderer, Rectangle rect, GameTime gameTime)
		{
			base.DrawControl(renderer, rect, gameTime);
		}
		#endregion
	}
}