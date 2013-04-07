////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: Control.cs                                   //
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
	/// Represents a control with a scrollable client area that controls can be added to.
	/// </summary>
	public class ClipControl : Control
	{
		#region Fields
		/// <summary>
		/// Client area of the clip control.
		/// </summary>
		private ClipBox clientArea;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the client area of the clip control.
		/// </summary>
		public virtual ClipBox ClientArea
		{
			get { return clientArea; }
			set { clientArea = value; }
		}
		
		/// <summary>
		/// Gets or sets the client area margins of the clip control.
		/// </summary>
		public override Margins ClientMargins
		{
			get
			{
				return base.ClientMargins;
			}

			set
			{
				base.ClientMargins = value;
				if (clientArea != null)
				{
					clientArea.Left = ClientLeft;
					clientArea.Top = ClientTop;
					clientArea.Width = ClientWidth;
					clientArea.Height = ClientHeight;
				}
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new clip control.
		/// </summary>
		/// <param name="manager">GUI manager for the clip control.</param>
		public ClipControl(Manager manager)
			: base(manager)
		{
			clientArea = new ClipBox(manager);

			clientArea.Init();
			clientArea.MinimumWidth = 0;
			clientArea.MinimumHeight = 0;
			clientArea.Left = ClientLeft;
			clientArea.Top = ClientTop;
			clientArea.Width = ClientWidth;
			clientArea.Height = ClientHeight;

			base.Add(clientArea);
		}
		#endregion

		#region Init
		/// <summary>
		/// Initializes the clip control.
		/// </summary>
		public override void Init()
		{
			base.Init();
		}
		#endregion

		#region Init Skin
		/// <summary>
		/// Initializes the skin for the clip control.
		/// </summary>
		protected internal override void InitSkin()
		{
			base.InitSkin();
		}
		#endregion

		#region Add
		/// <summary>
		/// Adds a child control to the clip control.
		/// </summary>
		/// <param name="control">Child control to add to the clip control.</param>
		/// <param name="client">Indicates if the control to add will be a child of the client area 
		/// clip box (true) or the a direct child of the clip control itself (false)</param>
		public virtual void Add(Control control, bool client)
		{
			if (client)
			{
				clientArea.Add(control);
			}

			else
			{
				base.Add(control);
			}
		}
		#endregion

		#region Add
		/// <summary>
		/// Adds a child control to the clip box.
		/// </summary>
		/// <param name="control">Child control to add to the clip control.</param>
		public override void Add(Control control)
		{
			Add(control, true);
		}
		#endregion

		#region Remove
		/// <summary>
		/// Removes the specified child control from the parent list.
		/// </summary>
		/// <param name="control">Child control to remove from the clip control's list.</param>
		public override void Remove(Control control)
		{
			base.Remove(control);
			clientArea.Remove(control);
		}
		#endregion

		#region Update
		/// <summary>
		/// Updates the clip control and its child controls.
		/// </summary>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		protected internal override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}
		#endregion

		#region Draw Control
		/// <summary>
		/// Draws the clip control and its child controls.
		/// </summary>
		/// <param name="renderer">Render management object.</param>
		/// <param name="rect">Destination rectangle.</param>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		protected override void DrawControl(Renderer renderer, Rectangle rect, GameTime gameTime)
		{
			base.DrawControl(renderer, rect, gameTime);
		}
		#endregion

		#region On Resize Event Handler
		/// <summary>
		/// Handles resize events for the clip control.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(ResizeEventArgs e)
		{
			base.OnResize(e);

			// Update client area dimensions.
			if (clientArea != null)
			{
				clientArea.Left = ClientLeft;
				clientArea.Top = ClientTop;
				clientArea.Width = ClientWidth;
				clientArea.Height = ClientHeight;
			}
		}
		#endregion

		#region Adjust Margins
		/// <summary>
		/// Adjusts the margins of the clip control.
		/// </summary>
		protected virtual void AdjustMargins()
		{

		}
		#endregion
	}
}
