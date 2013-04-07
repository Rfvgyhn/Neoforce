////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: ButtonBase.cs                                //
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
namespace TomShane.Neoforce.Controls
{
	/// <summary>
	/// Base class for a button control.
	/// </summary>
	public abstract class ButtonBase : Control
	{
		#region Properties
		/// <summary>
		/// Gets the state of the button control.
		/// </summary>
		public override ControlState ControlState
		{
			get
			{
				if (DesignMode)
				{
					return ControlState.Enabled;
				}

				else if (Suspended)
				{
					return ControlState.Disabled;
				}

				else
				{
					if (!Enabled)
					{
						return ControlState.Disabled;
					}

					if ((Pressed[(int)MouseButton.Left] && Inside) || (Focused && (Pressed[(int)GamePadActions.Press] || Pressed[(int)MouseButton.None])))
					{
						return ControlState.Pressed;
					}

					else if (Hovered && Inside)
					{
						return ControlState.Hovered;
					}

					else if ((Focused && !Inside) || (Hovered && !Inside) || (Focused && !Hovered && Inside))
					{
						return ControlState.Focused;
					}

					else
					{
						return ControlState.Enabled;
					}
				}
			}
		}
		#endregion

		#region Constructor
		/// <summary>
		/// ButtonBase constructor.
		/// </summary>
		/// <param name="manager">GUI manager for this control.</param>
		protected ButtonBase(Manager manager)
			: base(manager)
		{
			SetDefaultSize(72, 24);
			DoubleClicks = false;
		}
		#endregion

		#region Init
		/// <summary>
		/// Initializes the button base.
		/// </summary>
		public override void Init()
		{
			base.Init();
		}
		#endregion

		#region On Click Event Handler
		/// <summary>
		/// Handles button click events.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnClick(EventArgs e)
		{
			MouseEventArgs ex = (e is MouseEventArgs) ? (MouseEventArgs)e : new MouseEventArgs();
			
			if (ex.Button == MouseButton.Left || ex.Button == MouseButton.None)
			{
				base.OnClick(e);
			}
		}
		#endregion
	}
}
