////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: Container.cs                                 //
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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace TomShane.Neoforce.Controls
{
	/// <summary>
	/// Stores the values of the container's horizontal and vertical scroll bars.
	/// </summary>
	public struct ScrollBarValue
	{
		public int Vertical;
		public int Horizontal;
	}

	/// <summary>
	/// Represents a root-level control designed to hold one or more child controls.
	/// </summary>
	public class Container : ClipControl
	{
		#region Fields
		/// <summary>
		/// Container control's vertical scroll bar.
		/// </summary>
		private ScrollBar sbVert;
		/// <summary>
		/// Container control's horizontal scroll bar.
		/// </summary>
		private ScrollBar sbHorz;
		/// <summary>
		/// Container control's main menu.
		/// </summary>
		private MainMenu mainMenu;
		/// <summary>
		/// Tool bar panel of the container control.
		/// </summary>
		private ToolBarPanel toolBarPanel;
		/// <summary>
		/// Status bar control of the container.
		/// </summary>
		private StatusBar statusBar;
		/// <summary>
		/// Indicates if the container will automatically show/hide the client area scroll bars as needed.
		/// </summary>
		private bool autoScroll = false;
		/// <summary>
		/// Container's default control.
		/// </summary>
		private Control defaultControl = null;
        /// <summary>
        /// Scroll by PageSize (true) or StepSize (false)
        /// </summary>
        private bool scrollAlot = true;
		#endregion

		#region Properties
		/// <summary>
		/// Gets the values of the container's horizontal and vertical scroll bars.
		/// </summary>
		public virtual ScrollBarValue ScrollBarValue
		{
			get
			{
				ScrollBarValue scb = new ScrollBarValue();
				scb.Vertical = (sbVert != null ? sbVert.Value : 0);
				scb.Horizontal = (sbHorz != null ? sbHorz.Value : 0);
				return scb;
			}
		}
		
		/// <summary>
		/// Indicates if the container is visible. 
		/// </summary>
		public override bool Visible
		{
			get
			{
				return base.Visible;
			}
			set
			{
				if (value)
				{
					if (DefaultControl != null)
					{
						DefaultControl.Focused = true;
					}
				}
				base.Visible = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the default control of the container. 
		/// </summary>
		public virtual Control DefaultControl
		{
			get { return defaultControl; }
			set { defaultControl = value; }
		}
		
		/// <summary>
		/// Indicates if scroll bars will be displayed/hidden automatically.
		/// </summary>
		public virtual bool AutoScroll
		{
			get { return autoScroll; }
			set { autoScroll = value; }
		}
		
		/// <summary>
		/// Gets or sets the main menu of the container control.
		/// </summary>
		public virtual MainMenu MainMenu
		{
			get { return mainMenu; }
			set
			{
				if (mainMenu != null)
				{
					mainMenu.Resize -= Bars_Resize;
					Remove(mainMenu);
				}
				mainMenu = value;

				if (mainMenu != null)
				{
					Add(mainMenu, false);
					mainMenu.Resize += Bars_Resize;
				}
				AdjustMargins();
			}
		}
		
		/// <summary>
		/// Gets or sets the tool bar panel of the container control.
		/// </summary>
		public virtual ToolBarPanel ToolBarPanel
		{
			get
			{
				return toolBarPanel;
			}
			set
			{
				if (toolBarPanel != null)
				{
					toolBarPanel.Resize -= Bars_Resize;
					Remove(toolBarPanel);
				}
				toolBarPanel = value;

				if (toolBarPanel != null)
				{
					Add(toolBarPanel, false);
					toolBarPanel.Resize += Bars_Resize;
				}
				AdjustMargins();
			}
		}
		
		/// <summary>
		/// Gets or sets the container's status bar control.
		/// </summary>
		public virtual StatusBar StatusBar
		{
			get
			{
				return statusBar;
			}
			set
			{
				if (statusBar != null)
				{
					statusBar.Resize -= Bars_Resize;
					Remove(statusBar);
				}
				statusBar = value;

				if (statusBar != null)
				{
					Add(statusBar, false);
					statusBar.Resize += Bars_Resize;
				}
				AdjustMargins();
			}
		}

        /// <summary>
        /// Scroll by PageSize (true) or StepSize (false)
        /// </summary>
        public virtual bool ScrollAlot
        {
            get { return this.scrollAlot; }
            set { this.scrollAlot = value; }
        }

        /// <summary>
        /// Gets the container's vertical scroll bar.
        /// </summary>
        protected virtual ScrollBar VerticalScrollBar
        {
            get { return this.sbVert; }
        }

        /// <summary>
        /// Gets the container's horizontal scroll bar.
        /// </summary>
        protected virtual ScrollBar HorizontalScrollBar
        {
            get { return this.sbHorz; }
        }
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new container control.
		/// </summary>
		/// <param name="manager">GUI manager for the container control.</param>
		public Container(Manager manager)
			: base(manager)
		{
			sbVert = new ScrollBar(manager, Orientation.Vertical);
			sbVert.Init();
			sbVert.Detached = false;
			sbVert.Anchor = Anchors.Top | Anchors.Right | Anchors.Bottom;
			sbVert.ValueChanged += new EventHandler(ScrollBarValueChanged);
			sbVert.Range = 0;
			sbVert.PageSize = 0;
			sbVert.Value = 0;
			sbVert.Visible = false;

			sbHorz = new ScrollBar(manager, Orientation.Horizontal);
			sbHorz.Init();
			sbHorz.Detached = false;
			sbHorz.Anchor = Anchors.Right | Anchors.Left | Anchors.Bottom;
			sbHorz.ValueChanged += new EventHandler(ScrollBarValueChanged);
			sbHorz.Range = 0;
			sbHorz.PageSize = 0;
			sbHorz.Value = 0;
			sbHorz.Visible = false;
            
			Add(sbVert, false);
			Add(sbHorz, false);
		}
		#endregion

		#region Init
		/// <summary>
		/// Initializes the container control.
		/// </summary>
		public override void Init()
		{
			base.Init();
		}
		#endregion

		#region Init Skin
		/// <summary>
		/// Initializes the skin of the container control.
		/// </summary>
		protected internal override void InitSkin()
		{
			base.InitSkin();
		}
		#endregion

		#region Bars Resize
		/// <summary>
		/// Handlers for when the main menu, tool bar panel, or status bar are resized. 
		/// (Updates the container margins.)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Bars_Resize(object sender, ResizeEventArgs e)
		{
			AdjustMargins();
		}
		#endregion

		#region Adjust Margins
		/// <summary>
		/// Adjusts the container's margins to account for the visibility of the main menu, 
		/// the tool bar panel, the status bar, and the scroll bars.
		/// </summary>
		protected override void AdjustMargins()
		{
			// Get the skin margin values.
			Margins m = Skin.ClientMargins;
			
			if (this.GetType() != typeof(Container))
			{
				m = ClientMargins;
			}

			// Main menu displaying?
			if (mainMenu != null && mainMenu.Visible)
			{
				// Initialize the main menu if needed.
				if (!mainMenu.Initialized)
				{
					mainMenu.Init();
				}
				
				// Position and size the main menu.
				mainMenu.Left = m.Left;
				mainMenu.Top = m.Top;
				mainMenu.Width = Width - m.Horizontal;
				mainMenu.Anchor = Anchors.Left | Anchors.Top | Anchors.Right;

				// Update the container's top margin to account for the space the main menu is occupying.
				m.Top += mainMenu.Height;
			}

			// Tool bar panel displaying?
			if (toolBarPanel != null && toolBarPanel.Visible)
			{
				// Initialize the tool bar panel if needed.
				if (!toolBarPanel.Initialized)
				{
					toolBarPanel.Init();
				}
				
				// Position and size the tool bar panel.
				toolBarPanel.Left = m.Left;
				toolBarPanel.Top = m.Top;
				toolBarPanel.Width = Width - m.Horizontal;
				toolBarPanel.Anchor = Anchors.Left | Anchors.Top | Anchors.Right;

				// Update the container's top margin to account for the space the tool bar panel is occupying.
				m.Top += toolBarPanel.Height;
			}

			// Status bar displaying?
			if (statusBar != null && statusBar.Visible)
			{
				// Initialize the status bar if needed.
				if (!statusBar.Initialized)
				{
					statusBar.Init();
				}
				
				// Position and size the status bar.
				statusBar.Left = m.Left;
				statusBar.Top = Height - m.Bottom - statusBar.Height;
				statusBar.Width = Width - m.Horizontal;
				statusBar.Anchor = Anchors.Left | Anchors.Bottom | Anchors.Right;

				// Update the container's bottom margin to account for the space the status bar is occupying.
				m.Bottom += statusBar.Height;
			}

			// Vertical scroll bar displayed?
			if (sbVert != null && sbVert.Visible)
			{
				// Update the container's right margin to account for the space the scroll bar is occupying.
				m.Right += (sbVert.Width + 2);
			}

			// Horizontal scroll bar displayed?
			if (sbHorz != null && sbHorz.Visible)
			{
				// Update the container's right margin to account for the space the scroll bar is occupying.
				m.Bottom += (sbHorz.Height + 2);
			}

			// Update the container's margins.
			ClientMargins = m;

			// Adjust the position of the scroll bars.
			PositionScrollBars();

			base.AdjustMargins();
		}
		#endregion

		#region Add
		/// <summary>
		/// Adds a child control to the container.
		/// </summary>
		/// <param name="control">The child control to add to the container.</param>
		/// <param name="client">Indicates if the control will be a child of the client area 
		/// (true) or a direct descendant of the container. (false)</param>
		public override void Add(Control control, bool client)
		{
			base.Add(control, client);
			CalcScrolling();
		}
		#endregion

		#region Update
		/// <summary>
		/// Updates the container control and all child controls.
		/// </summary>
		/// <param name="gameTime"></param>
		protected internal override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}
		#endregion

		#region Draw Control
		/// <summary>
		/// Draws the container control and all child controls.
		/// </summary>
		/// <param name="renderer">Render management object.</param>
		/// <param name="rect">Destination region where the container control will be drawn.</param>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		protected override void DrawControl(Renderer renderer, Rectangle rect, GameTime gameTime)
		{
			base.DrawControl(renderer, rect, gameTime);
		}
		#endregion

		#region On Skin Changed Event Handler
		/// <summary>
		/// Handler for when the container's skin is changed.
		/// </summary>
		/// <param name="e"></param>
		protected internal override void OnSkinChanged(EventArgs e)
		{
			base.OnSkinChanged(e);
			if (sbVert != null && sbHorz != null)
			{
				sbVert.Visible = false;
				sbHorz.Visible = false;
				CalcScrolling();
			}
		}
		#endregion

		#region Position Scroll Bars
		/// <summary>
		/// Adjusts the position of the container's horizontal and vertical scroll bars.
		/// </summary>
		private void PositionScrollBars()
		{
			if (sbVert != null)
			{
				// Vertical scroll bar is positioned just to the right of the 
				// container's client area.
				sbVert.Left = ClientLeft + ClientWidth + 1;
				sbVert.Top = ClientTop + 1;

				// Shorten the scroll bar slightly if the horizontal scroll bar is also shown.
				int m = (sbHorz != null && sbHorz.Visible) ? 0 : 2;
				sbVert.Height = ClientArea.Height - m;
				sbVert.Range = ClientArea.VirtualHeight;
				sbVert.PageSize = ClientArea.ClientHeight;
			}

			if (sbHorz != null)
			{
				// Horizontal scroll bar is positioned just underneath the 
				// container's client area.
				sbHorz.Left = ClientLeft + 1;
				sbHorz.Top = ClientTop + ClientHeight + 1;

				// Shorten the scroll bar slightly if the vertical scroll bar is also shown.
				int m = (sbVert != null && sbVert.Visible) ? 0 : 2;
				sbHorz.Width = ClientArea.Width - m;
				sbHorz.Range = ClientArea.VirtualWidth;
				sbHorz.PageSize = ClientArea.ClientWidth;
			}
		}
		#endregion

		#region Calc Scrolling
		/// <summary>
		/// Updates the visibility of scroll bars based on client area actual and vitual dimensions.
		/// </summary>
		private void CalcScrolling()
		{
			if (sbVert != null && autoScroll)
			{
				bool vis = sbVert.Visible;

				// Should the vertical scroll bar be visible?
				sbVert.Visible = ClientArea.VirtualHeight > ClientArea.ClientHeight;

				if (ClientArea.VirtualHeight <= ClientArea.ClientHeight)
				{
					// No scroll bar needed, no scroll value needed.
					sbVert.Value = 0;
				}

				// Visibility of the scroll bar has changed?
				if (vis != sbVert.Visible)
				{
					// Hiding the scroll bar now?
					if (!sbVert.Visible)
					{
						// Clear all the client area child controls' top modifier values. No scroll bar = No offset.
						foreach (Control c in ClientArea.Controls)
						{
							c.TopModifier = 0;
							c.Invalidate();
						}
					}

					// Update margins to account for the removal of the scroll bar.
					AdjustMargins();
				}

				//Adjust the position of the scroll bars.
				PositionScrollBars();
				
				// Adjust the client area child controls' top modifier to offset 
				// their positions based on the value of the vertical scroll bar.
				foreach (Control c in ClientArea.Controls)
				{
					c.TopModifier = -sbVert.Value;
					c.Invalidate();
				}
			}

			if (sbHorz != null && autoScroll)
			{
				bool vis = sbHorz.Visible;

				// Should the horizontal scroll bar be visible?
				sbHorz.Visible = ClientArea.VirtualWidth > ClientArea.ClientWidth;

				if (ClientArea.VirtualWidth <= ClientArea.ClientWidth)
				{
					// No scroll bar needed, no scroll value needed.
					sbHorz.Value = 0;
				}

				// Visibility of the scroll bar has changed?
				if (vis != sbHorz.Visible)
				{
					// Hiding the scroll bar now?
					if (!sbHorz.Visible)
					{
						// Clear the top modifier value of all client area child controls.
						foreach (Control c in ClientArea.Controls)
						{
							c.LeftModifier = 0;
							sbVert.Refresh();
							c.Invalidate();
						}
					}

					// Update the container margins to account for removal of the scroll bar.
					AdjustMargins();
				}

				// Adjust the position of the scroll bars.
				PositionScrollBars();

				// Adjust the client area child controls' left modifier to offset 
				// their positions based on the value of the horizontal scroll bar.
				foreach (Control c in ClientArea.Controls)
				{
					c.LeftModifier = -sbHorz.Value;
					sbHorz.Refresh();
					c.Invalidate();
				}
			}
		}
		#endregion

		#region Scroll To
		/// <summary>
		/// Scrolls to the specified scroll bar positions.
		/// </summary>
		/// <param name="x">New horizontal scroll bar value.</param>
		/// <param name="y">New vertical scroll bar value.</param>
		public virtual void ScrollTo(int x, int y)
		{
			sbVert.Value = y;
			sbHorz.Value = x;
		}
		
		/// <summary>
		/// Adjusts the scroll bar values so the specified control is displayed in the client region.
		/// </summary>
		/// <param name="control"></param>
		public virtual void ScrollTo(Control control)
		{
			// Make sure the control exists inside the client area somewhere.
			if (control != null && ClientArea != null && ClientArea.Contains(control, true))
			{
				// Scroll down?
				if (control.AbsoluteTop + control.Height > ClientArea.AbsoluteTop + ClientArea.Height)
				{
					sbVert.Value = sbVert.Value + control.AbsoluteTop - ClientArea.AbsoluteTop - sbVert.PageSize + control.Height;
				}
				
				// Scroll up?
				else if (control.AbsoluteTop < ClientArea.AbsoluteTop)
				{
					sbVert.Value = sbVert.Value + control.AbsoluteTop - ClientArea.AbsoluteTop;
				}
				
				// Scroll left?
				if (control.AbsoluteLeft + control.Width > ClientArea.AbsoluteLeft + ClientArea.Width)
				{
					sbHorz.Value = sbHorz.Value + control.AbsoluteLeft - ClientArea.AbsoluteLeft - sbHorz.PageSize + control.Width;
				}
				
				// Scroll right?
				else if (control.AbsoluteLeft < ClientArea.AbsoluteLeft)
				{
					sbHorz.Value = sbHorz.Value + control.AbsoluteLeft - ClientArea.AbsoluteLeft;
				}
			}
		}
		#endregion

		#region Scroll Bar Value Changed
		/// <summary>
		/// Handles changes in scroll bar values.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void ScrollBarValueChanged(object sender, EventArgs e)
		{
			CalcScrolling();
		}
		#endregion

		#region On Resize Event Handler
		/// <summary>
		/// Handles resizing of the container control.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(ResizeEventArgs e)
		{
			base.OnResize(e);
			CalcScrolling();

			// Crappy fix to certain scrolling issue
			//if (sbVert != null) sbVert.Value -= 1; 
			//if (sbHorz != null) sbHorz.Value -= 1;      
		}
		#endregion

		#region Invalidate
		/// <summary>
		/// Invalidates the control and forces redrawing of it. 
		/// </summary>
		public override void Invalidate()
		{
			base.Invalidate();
		}
		#endregion

		#region On Click Event Handler
		/// <summary>
		/// Handles click events for the container control.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnClick(EventArgs e)
		{
			MouseEventArgs ex = e as MouseEventArgs;

			// Adjust mouse position based on scroll bar values.
			ex.Position = new Point(ex.Position.X + sbHorz.Value, ex.Position.Y + sbVert.Value);

			base.OnClick(e);
		}
		#endregion

        protected override void OnMouseScroll(MouseEventArgs e)
        {
            if (!ClientArea.Enabled)
                return;

            // If current control doesn't scroll, scroll the parent control
            if (sbVert.Range - sbVert.PageSize < 1)
            {
                Control c = this;

                while (c != null)
                {
                    var p = c.Parent as Container;

                    if (p != null && p.Enabled)
                    {
                        p.OnMouseScroll(e);

                        break;
                    }

                    c = c.Parent;
                }

                return;
            }

            if (e.ScrollDirection == MouseScrollDirection.Down)
                sbVert.ScrollDown(ScrollAlot);
            else
                sbVert.ScrollUp(ScrollAlot);

            base.OnMouseScroll(e);
        }
	}
}
