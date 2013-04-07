////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: ToolBarButton.cs                             //
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
	/// Represents a single button of a ToolBar control.
	/// </summary>
	public class ToolBarButton : Button
	{
		#region Constructors
		/// <summary>
		/// Creates a new ToolBarButton control.
		/// </summary>
		/// <param name="manager">GUI manager for the control.</param>
		public ToolBarButton(Manager manager)
			: base(manager)
		{
			CanFocus = false;
			Text = "";
		}
		#endregion

		#region Init
		/// <summary>
		/// Initializes the tool bar button.
		/// </summary>
		public override void Init()
		{
			base.Init();
		}
		#endregion

		#region Init Skin
		/// <summary>
		/// Initializes the skin of the tool bar button.
		/// </summary>
		protected internal override void InitSkin()
		{
			base.InitSkin();
			Skin = new SkinControl(Manager.Skin.Controls["ToolBarButton"]);
		}
		#endregion

		#region Draw Control
		/// <summary>
		/// Draws the tool bar button control.
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
