////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: CheckBox.cs                                  //
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
	/// 
	/// </summary>
	public class CheckBox : ButtonBase
	{
		#region Constants
		/// <summary>
		/// String to reference the check box element in the skin file.
		/// </summary>
		private const string skCheckBox = "CheckBox";
		/// <summary>
		/// String to reference the control layer the checkbox is a part of.
		/// </summary>
		private const string lrCheckBox = "Control";
		/// <summary>
		/// ???
		/// </summary>
		private const string lrChecked = "Checked";
		#endregion

		#region Fields
		/// <summary>
		/// Indicates if the control is checked (true) or unchecked (false.)
		/// </summary>
		private bool state = false;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the checked state of the control.
		/// </summary>
		public virtual bool Checked
		{
			get
			{
				return state;
			}

			set
			{
				state = value;
				Invalidate();
				
				if (!Suspended)
				{
					OnCheckedChanged(new EventArgs());
				}
			}
		}
		#endregion

		#region Events
		/// <summary>
		/// Occurs when the control's check state value changes.
		/// </summary>
		public event EventHandler CheckedChanged;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new check box control.
		/// </summary>
		/// <param name="manager">GUI manager for the check box control.</param>
		public CheckBox(Manager manager)
			: base(manager)
		{
			CheckLayer(Skin, lrChecked);

			Width = 64;
			Height = 16;
		}
		#endregion

		#region Init
		/// <summary>
		/// Initializes the check box control.
		/// </summary>
		public override void Init()
		{
			base.Init();
		}
		#endregion

		#region Init Skin
		/// <summary>
		/// Initializes the check box control's skin.
		/// </summary>
		protected internal override void InitSkin()
		{
			base.InitSkin();
			Skin = new SkinControl(Manager.Skin.Controls[skCheckBox]);
		}
		#endregion

		#region Draw Control
		/// <summary>
		/// Draws the checkbox control.
		/// </summary>
		/// <param name="renderer">Render management object.</param>
		/// <param name="rect">Destination rectangle.</param>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		protected override void DrawControl(Renderer renderer, Rectangle rect, GameTime gameTime)
		{
			// Grab the checked skin layer and skin font.
			SkinLayer layer = Skin.Layers[lrChecked];
			SkinText font = Skin.Layers[lrChecked].Text;

			// Umm. See if we actually need the unchecked layer and font...
			if (!state)
			{
				layer = Skin.Layers[lrCheckBox];
				font = Skin.Layers[lrCheckBox].Text;
			}

			rect.Width = layer.Width;
			rect.Height = layer.Height;
			Rectangle rc = new Rectangle(rect.Left + rect.Width + 4, rect.Y, Width - (layer.Width + 4), rect.Height);

			renderer.DrawLayer(this, layer, rect);
			renderer.DrawString(this, layer, Text, rc, false, 0, 0);
		}
		#endregion

		#region On Click Event Handler
		/// <summary>
		/// Handles checkbox click events.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnClick(EventArgs e)
		{
			MouseEventArgs ex = (e is MouseEventArgs) ? (MouseEventArgs)e : new MouseEventArgs();

			// Change the checked state of the control?
			if (ex.Button == MouseButton.Left || ex.Button == MouseButton.None)
			{
				Checked = !Checked;
			}

			base.OnClick(e);
		}
		#endregion

		#region On Checked Changed Event Handler
		/// <summary>
		/// Handles the checked changed event when the value of the check state changes.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnCheckedChanged(EventArgs e)
		{
			if (CheckedChanged != null)
			{
				CheckedChanged.Invoke(this, e);
			}
		}
		#endregion
	}
}
