////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: SideBarPanel.cs                              //
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
#region Fields
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TomShane.Neoforce.Controls
{
	/// <summary>
	/// Standard container control. Nothing special about it. See Container class for more information.
	/// </summary>
	public class SideBarPanel : Container
	{
		#region Constructor
		/// <summary>
		/// Creates a new SideBarPanel control.
		/// </summary>
		/// <param name="manager">GUI manager for the control.</param>
		public SideBarPanel(Manager manager)
			: base(manager)
		{
			CanFocus = false;
			Passive = true;
			Width = 64;
			Height = 64;
		}
		#endregion

		#region Init
		/// <summary>
		/// Initializes the side bar panel control.
		/// </summary>
		public override void Init()
		{
			base.Init();
		}
		#endregion
	}
}
