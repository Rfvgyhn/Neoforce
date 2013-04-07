////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: ToolBarPanel.cs                              //
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
	/// Represents a ToolBar container.
	/// </summary>
	public class ToolBarPanel : Control
	{
		#region Constructor
		/// <summary>
		/// Creates a new tool bar panel control.
		/// </summary>
		/// <param name="manager">GUI manager for the tool bar panel.</param>
		public ToolBarPanel(Manager manager)
			: base(manager)
		{
			Width = 64;
			Height = 25;
		}
		#endregion

		#region Init
		/// <summary>
		/// Initializes the tool bar panel control.
		/// </summary>
		public override void Init()
		{
			base.Init();
		}
		#endregion

		#region Init Skin
		/// <summary>
		/// Initializes the skin for the tool bar panel control.
		/// </summary>
		protected internal override void InitSkin()
		{
			base.InitSkin();
			Skin = new SkinControl(Manager.Skin.Controls["ToolBarPanel"]);
		}
		#endregion

		#region Draw Control
		/// <summary>
		/// Draws the tool bar panel and all child controls.
		/// </summary>
		/// <param name="renderer">Render management object.</param>
		/// <param name="rect">Destination region where the tool bar panel will be drawn.</param>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		protected override void DrawControl(Renderer renderer, Rectangle rect, GameTime gameTime)
		{
			base.DrawControl(renderer, rect, gameTime);
		}
		#endregion

		#region On Resize Event Handler
		/// <summary>
		/// Handles resize events for the tool bar panel control.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(ResizeEventArgs e)
		{
			base.OnResize(e);
		}
		#endregion

		#region Update
		/// <summary>
		/// Updates the tool bar panel control.
		/// </summary>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		protected internal override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			AlignBars();
		}
		#endregion

		#region Align Bars
		/// <summary>
		/// Positions and sizes the tool bar panel's child tool bar controls.
		/// </summary>
		private void AlignBars()
		{
			int[] rx = new int[8];
			int h = 0;
			int rm = -1;

			foreach (Control c in Controls)
			{
				// This child control is a tool bar?
				if (c is ToolBar)
				{
					ToolBar t = c as ToolBar;
					
					// Tool bar should consume all available width regardless of items?
					if (t.FullRow)
					{
						t.Width = Width;
					}
					
					// Position the tool bar.
					t.Left = rx[t.Row];
					t.Top = (t.Row * t.Height) + (t.Row > 0 ? 1 : 0);
					rx[t.Row] += t.Width + 1;

					if (t.Row > rm)
					{
						rm = t.Row;
						h = t.Top + t.Height + 1;
					}
				}
			}

			Height = h;
		}
		#endregion
	}
}
