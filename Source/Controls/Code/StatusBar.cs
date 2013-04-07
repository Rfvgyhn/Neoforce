////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: StatusBar.cs                                 //
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
#endregion

namespace TomShane.Neoforce.Controls
{
	/// <summary>
	/// Represents a status bar control usually placed at the bottom of a window.
	/// </summary>
	public class StatusBar : Control
	{
		#region Constructors
		/// <summary>
		/// Creates a new StatusBar control.
		/// </summary>
		/// <param name="manager">GUI manager for the control.</param>
		public StatusBar(Manager manager)
			: base(manager)
		{
			Left = 0;
			Top = 0;
			Width = 64;
			Height = 24;
			CanFocus = false;
		}
		#endregion

		#region Init
		/// <summary>
		/// Initializes the status bar control.
		/// </summary>
		public override void Init()
		{
			base.Init();
		}
		#endregion

		#region Init Skin
		/// <summary>
		/// Initializes the skin of the status bar control.
		/// </summary>
		protected internal override void InitSkin()
		{
			base.InitSkin();
			Skin = new SkinControl(Manager.Skin.Controls["StatusBar"]);
		}
		#endregion

		#region Draw Control
		/// <summary>
		/// Draws the status bar control.
		/// </summary>
		/// <param name="renderer">Render management object.</param>
		/// <param name="rect">Destination region where the control will be drawn.</param>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		protected override void DrawControl(Renderer renderer, Rectangle rect, GameTime gameTime)
		{
			base.DrawControl(renderer, rect, gameTime);
		}
		#endregion
	}
}
