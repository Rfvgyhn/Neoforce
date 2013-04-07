////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: ToolTip.cs                                   //
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
	/// Represents a tool tip for a control.
	/// </summary>
	public class ToolTip : Control
	{
		#region Properties
		/// <summary>
		/// Indicates whether the tool tip is visible or not.
		/// </summary>
		public override bool Visible
		{
			set
			{
				if (value && Text != null && Text != "" && Skin != null && Skin.Layers[0] != null)
				{
					Vector2 size = Skin.Layers[0].Text.Font.Resource.MeasureString(Text);
					Width = (int)size.X + Skin.Layers[0].ContentMargins.Horizontal;
					Height = (int)size.Y + Skin.Layers[0].ContentMargins.Vertical;
					Left = Mouse.GetState().X;
					Top = Mouse.GetState().Y + 24;
					base.Visible = value;
				}
				else
				{
					base.Visible = false;
				}
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new tool tip control.
		/// </summary>
		/// <param name="manager">GUI manager for the tool tip control.</param>
		public ToolTip(Manager manager)
			: base(manager)
		{
			Text = "";
		}
		#endregion

		#region Init
		/// <summary>
		/// Initializes the tool tip control.
		/// </summary>
		public override void Init()
		{
			base.Init();
			CanFocus = false;
			Passive = true;
		}
		#endregion

		#region Init Skin
		/// <summary>
		/// Initializes the skin of the tool tip control.
		/// </summary>
		protected internal override void InitSkin()
		{
			base.InitSkin();
			Skin = Manager.Skin.Controls["ToolTip"];
		}
		#endregion

		#region Draw Control
		/// <summary>
		/// Draws the tool tip control.
		/// </summary>
		/// <param name="renderer">Render management object.</param>
		/// <param name="rect">Destination rectangle where the tool tip should be drawn.</param>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		protected override void DrawControl(Renderer renderer, Rectangle rect, GameTime gameTime)
		{
			renderer.DrawLayer(this, Skin.Layers[0], rect);
			renderer.DrawString(this, Skin.Layers[0], Text, rect, true);
		}
		#endregion
	}
}
