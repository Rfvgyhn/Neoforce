////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: Label.cs                                     //
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
	/// Represents a Label control.
	/// </summary>
	public class Label : Control
	{
		#region Fields
		/// <summary>
		/// Indicates how the label's text is aligned.
		/// </summary>
		private Alignment alignment = Alignment.MiddleLeft;
		/// <summary>
		/// Indicates if the text should be truncated with "..."
		/// </summary>
		private bool ellipsis = true;
		#endregion

		#region Properties
		/// <summary>
		/// Indicates how the label's text is aligned.
		/// </summary>
		public virtual Alignment Alignment
		{
			get { return alignment; }
			set { alignment = value; }
		}
		
		/// <summary>
		/// Indicates if the label's text should be truncated with "..." if it's too large.
		/// </summary>
		public virtual bool Ellipsis
		{
			get { return ellipsis; }
			set { ellipsis = value; }
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new label control.
		/// </summary>
		/// <param name="manager">GUI management object for the label control.</param>
		public Label(Manager manager)
			: base(manager)
		{
			CanFocus = false;
			Passive = true;
			Width = 64;
			Height = 16;
		}
		#endregion

		#region Init
		/// <summary>
		/// Initializes the label control.
		/// </summary>
		public override void Init()
		{
			base.Init();
		}
		#endregion

		#region Draw Control
		/// <summary>
		/// Draws the label control in the specified region.
		/// </summary>
		/// <param name="renderer">Render management object.</param>
		/// <param name="rect">Destination rectangle where drawing takes place.</param>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		protected override void DrawControl(Renderer renderer, Rectangle rect, GameTime gameTime)
		{
			//base.DrawControl(renderer, rect, gameTime);

			SkinLayer s = new SkinLayer(Skin.Layers[0]);
			s.Text.Alignment = alignment;
			renderer.DrawString(this, s, Text, rect, true, 0, 0, ellipsis);
		}
		#endregion
	}
}
