////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: GroupBox.cs                                  //
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
	/// Defines how the GroupBox looks when rendered.
	/// </summary>
	public enum GroupBoxType
	{
		Normal,
		Flat
	}

	/// <summary>
	/// Represents a container used to group together related controls.
	/// </summary>
	public class GroupBox : Container
	{
		#region Fields
		/// <summary>
		/// Defines the rendered look of the group box.
		/// </summary>
		private GroupBoxType type = GroupBoxType.Normal;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the group box type.
		/// </summary>
		public virtual GroupBoxType Type
		{
			get { return type; }
			set { type = value; Invalidate(); }
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Creates a new GroupBox container control.
		/// </summary>
		/// <param name="manager">GUI manager for the group box.</param>
		public GroupBox(Manager manager)
			: base(manager)
		{
			CheckLayer(Skin, "Control");
			CheckLayer(Skin, "Flat");

			CanFocus = false;
			Passive = true;
			Width = 64;
			Height = 64;
			BackColor = Color.Transparent;
		}
		#endregion

		#region Init
		/// <summary>
		/// Initializes the group box control.
		/// </summary>
		public override void Init()
		{
			base.Init();
		}
		#endregion

		#region Draw Control
		/// <summary>
		/// Draws the group box control.
		/// </summary>
		/// <param name="renderer">Render management object.</param>
		/// <param name="rect">Destination region where the group box should be drawn.</param>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		protected override void DrawControl(Renderer renderer, Rectangle rect, GameTime gameTime)
		{
			SkinLayer layer = type == GroupBoxType.Normal ? Skin.Layers["Control"] : Skin.Layers["Flat"];
			SpriteFont font = (layer.Text != null && layer.Text.Font != null) ? layer.Text.Font.Resource : null;
			Color col = (layer.Text != null) ? layer.Text.Colors.Enabled : Color.White;
			Point offset = new Point(layer.Text.OffsetX, layer.Text.OffsetY);
			Vector2 size = font.MeasureString(Text);
			size.Y = font.LineSpacing;
			Rectangle r = new Rectangle(rect.Left, rect.Top + (int)(size.Y / 2), rect.Width, rect.Height - (int)(size.Y / 2));

			renderer.DrawLayer(this, layer, r);

			// Group box has header text to draw?
			if (font != null && Text != null && Text != "")
			{
				Rectangle bg = new Rectangle(r.Left + offset.X, (r.Top - (int)(size.Y / 2)) + offset.Y, (int)size.X + layer.ContentMargins.Horizontal, (int)size.Y);
				renderer.DrawLayer(Manager.Skin.Controls["Control"].Layers[0], bg, new Color(64, 64, 64), 0);
				renderer.DrawString(this, layer, Text, new Rectangle(r.Left, r.Top - (int)(size.Y / 2), (int)(size.X), (int)size.Y), true, 0, 0, false);
			}
		}
		#endregion
	}
}
