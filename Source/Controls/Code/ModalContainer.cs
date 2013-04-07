////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: ModalContainer.cs                            //
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

#region //// Using /////////////

////////////////////////////////////////////////////////////////////////////
using Microsoft.Xna.Framework;
////////////////////////////////////////////////////////////////////////////

#endregion

namespace TomShane.Neoforce.Controls
{
	/// <summary>
	/// Represents a control/window that remains focused until dealt with.
	/// </summary>
	public class ModalContainer : Container
	{
		#region Fields
		/// <summary>
		/// Indicates the result of the modal dialog.
		/// </summary>
		private ModalResult modalResult = ModalResult.None;
		/// <summary>
		/// Parent modal control, if there is any.
		/// </summary>
		private ModalContainer lastModal = null;
		#endregion

		#region Properties
		/// <summary>
		/// Indicates if the modal container is visible or not.
		/// </summary>
		public override bool Visible
		{
			get
			{
				return base.Visible;
			}
			set
			{
				if (value) Focused = true;
				base.Visible = value;
			}
		}

		/// <summary>
		/// Indicates if the container is modal or not.
		/// </summary>
		public virtual bool IsModal
		{
			get { return Manager.ModalWindow == this; }
		}

		/// <summary>
		/// Gets or sets the result of the modal dialog. (Usually indicating 
		/// which button of the dialog was pressed.)
		/// </summary>
		public virtual ModalResult ModalResult
		{
			get
			{
				return modalResult;
			}
			set
			{
				modalResult = value;
			}
		}
		#endregion

		#region Events
		/// <summary>
		/// Occurs when the dialog is beginning to close.
		/// </summary>
		public event WindowClosingEventHandler Closing;
		/// <summary>
		/// Occurs when the dialog has finished closing.
		/// </summary>
		public event WindowClosedEventHandler Closed;
		#endregion

		#region Constructor
		/// <summary>
		/// Creates a new modal dialog control.
		/// </summary>
		/// <param name="manager">GUI manager for the modal dialog control.</param>
		public ModalContainer(Manager manager)
			: base(manager)
		{
			Manager.Input.GamePadDown += new GamePadEventHandler(Input_GamePadDown);
			GamePadActions = new WindowGamePadActions();
		}
		#endregion

		#region Show Modal
		/// <summary>
		/// Shows the control as a modal dialog.
		/// </summary>
		public virtual void ShowModal()
		{
			lastModal = Manager.ModalWindow;
			Manager.ModalWindow = this;
			Manager.Input.KeyDown += new KeyEventHandler(Input_KeyDown);
			Manager.Input.GamePadDown += new GamePadEventHandler(Input_GamePadDown);
		}
		#endregion

		#region Close
		/// <summary>
		/// Closes the modal dialog.
		/// </summary>
		public virtual void Close()
		{
			WindowClosingEventArgs ex = new WindowClosingEventArgs();
			OnClosing(ex);
			if (!ex.Cancel)
			{
				Manager.Input.KeyDown -= Input_KeyDown;
				Manager.Input.GamePadDown -= Input_GamePadDown;
				Manager.ModalWindow = lastModal;
				if (lastModal != null) lastModal.Focused = true;
				Hide();
				WindowClosedEventArgs ev = new WindowClosedEventArgs();
				OnClosed(ev);

				if (ev.Dispose)
				{
					this.Dispose();
				}
			}
		}

		/// <summary>
		/// Closes the modal dialog with the specified result.
		/// </summary>
		/// <param name="modalResult">Dialog result to close the window with.</param>
		public virtual void Close(ModalResult modalResult)
		{
			ModalResult = modalResult;
			Close();
		}
		#endregion

		#region On Closing Event Handler
		/// <summary>
		/// Handles the closing of the modal container control.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnClosing(WindowClosingEventArgs e)
		{
			if (Closing != null) Closing.Invoke(this, e);
		}
		#endregion

		#region On Closed Event Handler
		/// <summary>
		/// Handles the closed event of the modal container.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnClosed(WindowClosedEventArgs e)
		{
			if (Closed != null) Closed.Invoke(this, e);
		}
		#endregion

		#region Key Down Event Handler
		/// <summary>
		/// Handles key press events for the modal container.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Input_KeyDown(object sender, KeyEventArgs e)
		{
			if (Visible && (Manager.FocusedControl != null && Manager.FocusedControl.Root == this) &&
				e.Key == Microsoft.Xna.Framework.Input.Keys.Escape)
			{
				//Close(ModalResult.Cancel);
			}
		}
		#endregion

		#region Game Pad Button Down Event Handler
		/// <summary>
		/// Handles gamepad button down events for the modal container.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Input_GamePadDown(object sender, GamePadEventArgs e)
		{
			if (Visible && (Manager.FocusedControl != null && Manager.FocusedControl.Root == this))
			{
				if (e.Button == (GamePadActions as WindowGamePadActions).Accept)
				{
					Close(ModalResult.Ok);
				}
				else if (e.Button == (GamePadActions as WindowGamePadActions).Cancel)
				{
					Close(ModalResult.Cancel);
				}
			}
		}
		#endregion
	}
}
