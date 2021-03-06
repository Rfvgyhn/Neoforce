////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Central                                          //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: Dialog.cs                                    //
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
	/// Represents a dialog window.
	/// </summary>
	public class Dialog : Window
	{
		#region Fields
		/// <summary>
		/// Panel containing the title and message of the dialog.
		/// </summary>
		private Panel pnlTop = null;
		/// <summary>
		/// Dialog window title.
		/// </summary>
		private Label lblCapt = null;
		/// <summary>
		/// Dialog window message.
		/// </summary>
		private Label lblDesc = null;
		/// <summary>
		/// Panel containing the dialog buttons.
		/// </summary>
		private Panel pnlBottom = null;
		#endregion

		#region Properties
		/// <summary>
		/// Gets the top panel of the dialog. (Contains the caption and description.)
		/// </summary>
		public Panel TopPanel { get { return pnlTop; } }
		/// <summary>
		/// Gets the bottom panel of the dialog. (Contains the dialog button controls.)
		/// </summary>
		public Panel BottomPanel { get { return pnlBottom; } }
		/// <summary>
		/// Gets the dialog window title.
		/// </summary>
		public Label Caption { get { return lblCapt; } }
		/// <summary>
		/// Gets the dialog window message.
		/// </summary>
		public Label Description { get { return lblDesc; } }
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new dialog window.
		/// </summary>
		/// <param name="manager">GUI manager for the dialog window.</param>
		public Dialog(Manager manager)
			: base(manager)
		{
			// Create the top panel control.
			pnlTop = new Panel(manager);
			pnlTop.Anchor = Anchors.Left | Anchors.Top | Anchors.Right;
			pnlTop.Init();
			pnlTop.Parent = this;
			pnlTop.Width = ClientWidth;
			pnlTop.Height = 64;
			pnlTop.BevelBorder = BevelBorder.Bottom;

			// Create the caption label and add it to the top panel.
			lblCapt = new Label(manager);
			lblCapt.Init();
			lblCapt.Parent = pnlTop;
			lblCapt.Width = lblCapt.Parent.ClientWidth - 16;
			lblCapt.Text = "Caption";
			lblCapt.Left = 8;
			lblCapt.Top = 8;
			lblCapt.Alignment = Alignment.TopLeft;
			lblCapt.Anchor = Anchors.Left | Anchors.Top | Anchors.Right;

			// Create the description label and add it to the top panel.
			lblDesc = new Label(manager);
			lblDesc.Init();
			lblDesc.Parent = pnlTop;
			lblDesc.Width = lblDesc.Parent.ClientWidth - 16;
			lblDesc.Left = 8;
			lblDesc.Text = "Description text.";
			lblDesc.Alignment = Alignment.TopLeft;
			lblDesc.Anchor = Anchors.Left | Anchors.Top | Anchors.Right;

			// Create the bottom panel control.
			pnlBottom = new Panel(manager);
			pnlBottom.Init();
			pnlBottom.Parent = this;
			pnlBottom.Width = ClientWidth;
			pnlBottom.Height = 24 + 16;
			pnlBottom.Top = ClientHeight - pnlBottom.Height;
			pnlBottom.BevelBorder = BevelBorder.Top;
			pnlBottom.Anchor = Anchors.Left | Anchors.Bottom | Anchors.Right;
		}
		#endregion

		#region Init
		/// <summary>
		/// Initializes the dialog window.
		/// </summary>
		public override void Init()
		{
			base.Init();

			SkinLayer lc = new SkinLayer(lblCapt.Skin.Layers[0]);
			lc.Text.Font.Resource = Manager.Skin.Fonts[Manager.Skin.Controls["Dialog"].Layers["TopPanel"].Attributes["CaptFont"].Value].Resource;
			lc.Text.Colors.Enabled = Utilities.ParseColor(Manager.Skin.Controls["Dialog"].Layers["TopPanel"].Attributes["CaptFontColor"].Value);

			SkinLayer ld = new SkinLayer(lblDesc.Skin.Layers[0]);
			ld.Text.Font.Resource = Manager.Skin.Fonts[Manager.Skin.Controls["Dialog"].Layers["TopPanel"].Attributes["DescFont"].Value].Resource;
			ld.Text.Colors.Enabled = Utilities.ParseColor(Manager.Skin.Controls["Dialog"].Layers["TopPanel"].Attributes["DescFontColor"].Value);

			pnlTop.Color = Utilities.ParseColor(Manager.Skin.Controls["Dialog"].Layers["TopPanel"].Attributes["Color"].Value);
			pnlTop.BevelMargin = int.Parse(Manager.Skin.Controls["Dialog"].Layers["TopPanel"].Attributes["BevelMargin"].Value);
			pnlTop.BevelStyle = Utilities.ParseBevelStyle(Manager.Skin.Controls["Dialog"].Layers["TopPanel"].Attributes["BevelStyle"].Value);

			lblCapt.Skin = new SkinControl(lblCapt.Skin);
			lblCapt.Skin.Layers[0] = lc;
			lblCapt.Height = Manager.Skin.Fonts[Manager.Skin.Controls["Dialog"].Layers["TopPanel"].Attributes["CaptFont"].Value].Height;

			lblDesc.Skin = new SkinControl(lblDesc.Skin);
			lblDesc.Skin.Layers[0] = ld;
			lblDesc.Height = Manager.Skin.Fonts[Manager.Skin.Controls["Dialog"].Layers["TopPanel"].Attributes["DescFont"].Value].Height;
			lblDesc.Top = lblCapt.Top + lblCapt.Height + 4;
			lblDesc.Height = lblDesc.Parent.ClientHeight - lblDesc.Top - 8;

			pnlBottom.Color = Utilities.ParseColor(Manager.Skin.Controls["Dialog"].Layers["BottomPanel"].Attributes["Color"].Value);
			pnlBottom.BevelMargin = int.Parse(Manager.Skin.Controls["Dialog"].Layers["BottomPanel"].Attributes["BevelMargin"].Value);
			pnlBottom.BevelStyle = Utilities.ParseBevelStyle(Manager.Skin.Controls["Dialog"].Layers["BottomPanel"].Attributes["BevelStyle"].Value);
		}
		#endregion
	}
}
