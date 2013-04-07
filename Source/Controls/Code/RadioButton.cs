////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: RadioButton.cs                               //
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
#endregion

namespace TomShane.Neoforce.Controls
{
	/// <summary>
	/// Indicates how other radio buttons are updated when a radio button is clicked.
	/// </summary>
	public enum RadioButtonMode
	{
		/// <summary>
		/// Clicked radio button will update the checked state of other radio buttons.
		/// </summary>
		Auto,
		/// <summary>
		/// Updating the check state of other radio buttons is a task left to the user.
		/// </summary>
		Manual
	}
	
	/// <summary>
	/// Similar to a check box control but allows the user to only select a single option of a radio button group.
	/// </summary>
	public class RadioButton : CheckBox
	{
		#region Constants
		/// <summary>
		/// String used to access the RadioButton's skin control.
		/// </summary>
		private const string skRadioButton = "RadioButton";
		#endregion

		#region Fields
		/// <summary>
		/// Indicates if the control will update the check state of other radio button clicks when it's clicked.
		/// </summary>
		private RadioButtonMode mode = RadioButtonMode.Auto;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the way the radio button handles updating other radio button control check states when it is clicked.
		/// </summary>
		public RadioButtonMode Mode
		{
			get { return mode; }
			set { mode = value; }
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new RadioButton control.
		/// </summary>
		/// <param name="manager">GUI manager for the control.</param>
		public RadioButton(Manager manager)
			: base(manager)
		{
		}
		#endregion

		#region Init
		/// <summary>
		/// Initializes the radio button control.
		/// </summary>
		public override void Init()
		{
			base.Init();
		}
		#endregion

		#region Init Skin
		/// <summary>
		/// Initializes the skin of the radio button control.
		/// </summary>
		protected internal override void InitSkin()
		{
			base.InitSkin();
			Skin = new SkinControl(Manager.Skin.Controls[skRadioButton]);
		}
		#endregion

		#region Draw Control
		/// <summary>
		/// Draws the radio button control.
		/// </summary>
		/// <param name="renderer">Render management object.</param>
		/// <param name="rect">Destination region where the control will be drawn.</param>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		protected override void DrawControl(Renderer renderer, Rectangle rect, GameTime gameTime)
		{
			base.DrawControl(renderer, rect, gameTime);
		}
		#endregion

		#region On Click Event Handler
		/// <summary>
		/// Handles radio button mouse click events.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnClick(EventArgs e)
		{
			MouseEventArgs ex = (e is MouseEventArgs) ? (MouseEventArgs)e : new MouseEventArgs();

			if (ex.Button == MouseButton.Left || ex.Button == MouseButton.None)
			{
				// Should we handled updating other radio button siblings?
				if (mode == RadioButtonMode.Auto)
				{
					// Radio button has parent?
					if (Parent != null)
					{
						ControlsList lst = Parent.Controls as ControlsList;
					
						// Radio button has siblings?
						for (int i = 0; i < lst.Count; i++)
						{
							if (lst[i] is RadioButton)
							{
								// Uncheck RB siblings.
								(lst[i] as RadioButton).Checked = false;
							}
						}
					}

					else if (Parent == null && Manager != null)
					{
						ControlsList lst = Manager.Controls as ControlsList;

						// Other radio buttons being managed?
						for (int i = 0; i < lst.Count; i++)
						{
							// Assume all radio buttons are part of a single global 
							// grouping and uncheck the other radio buttons.
							if (lst[i] is RadioButton)
							{
								(lst[i] as RadioButton).Checked = false;
							}
						}
					}
				}
			}
			base.OnClick(e);
		}
		#endregion
	}
}
