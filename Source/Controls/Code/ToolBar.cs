////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: ToolBar.cs                                   //
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
	/// Represents a tool bar that is part of a menu system made of tool bar buttons.
	/// </summary>
	public class ToolBar : Control
	{
		#region Fields
		/// <summary>
		/// Row of the tool bar panel this tool bar defines.
		/// </summary>
		private int row = 0;
		/// <summary>
		/// Indicates if the tool bar should take up the full width of its parent tool bar container.
		/// </summary>
		private bool fullRow = false;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the row index this tool bar occupies in its parent container.
		/// </summary>
		public virtual int Row
		{
			get { return row; }
			set
			{
				row = value;
				if (row < 0) row = 0;
				if (row > 7) row = 7;
			}
		}

		/// <summary>
		/// Indicates if the tool bar should stretch across the entire width of its container.
		/// </summary>
		public virtual bool FullRow
		{
			get { return fullRow; }
			set { fullRow = value; }
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new ToolBar control.
		/// </summary>
		/// <param name="manager">GUI manager for the tool bar.</param>
		public ToolBar(Manager manager)
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
		/// Initializes the tool bar control.
		/// </summary>
		public override void Init()
		{
			base.Init();
		}
		#endregion

		#region Init Skin
		/// <summary>
		/// Initializes the skin of the tool bar control.
		/// </summary>
		protected internal override void InitSkin()
		{
			base.InitSkin();
			Skin = new SkinControl(Manager.Skin.Controls["ToolBar"]);
		}
		#endregion

		#region Draw Control
		/// <summary>
		/// Draws the tool bar control.
		/// </summary>
		/// <param name="renderer">Render management object.</param>
		/// <param name="rect">Destination region where the tool bar will be drawn.</param>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		protected override void DrawControl(Renderer renderer, Rectangle rect, GameTime gameTime)
		{
			base.DrawControl(renderer, rect, gameTime);
		}
		#endregion
	}
}
