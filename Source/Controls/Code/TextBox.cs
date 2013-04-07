////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: TextBox.cs                                   //
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
using Microsoft.Xna.Framework.Input;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace TomShane.Neoforce.Controls
{
	/// <summary>
	/// Specifies the type of text box.
	/// </summary>
	public enum TextBoxMode
	{
		/// <summary>
		/// Standard text box control. Single line. 
		/// </summary>
		Normal,
		/// <summary>
		/// Masked text box control. Input is replaced by the control's password character.
		/// </summary>
		Password,
		/// <summary>
		/// Multi-line text box control.
		/// </summary>
		Multiline
	}

	/// <summary>
	/// Represents a text box control. 
	/// </summary>
	public class TextBox : ClipControl
	{
		#region Structs
		/// <summary>
		/// 
		/// </summary>
		private struct Selection
		{
			private int start;
			private int end;

			public int Start
			{
				get
				{
					if (start > end && start != -1 && end != -1) return end;
					else return start;
				}
				set
				{
					start = value;
				}
			}

			public int End
			{
				get
				{
					if (end < start && start != -1 && end != -1) return start;
					else return end;
				}
				set
				{
					end = value;
				}
			}

			public bool IsEmpty
			{
				get { return Start == -1 && End == -1; }
			}

			public int Length
			{
				get { return IsEmpty ? 0 : (End - Start); }
			}

			public Selection(int start, int end)
			{
				this.start = start;
				this.end = end;
			}

			public void Clear()
			{
				Start = -1;
				End = -1;
			}
		}


		#endregion

		#region Consts
		/// <summary>
		/// String for accessing the text box control skin.
		/// </summary>
		private const string skTextBox = "TextBox";
		/// <summary>
		/// String for accessing the text box control layer.
		/// </summary>
		private const string lrTextBox = "Control";
		/// <summary>
		/// String for accessing the text box cursor layer.
		/// </summary>
		private const string lrCursor = "Cursor";
		/// <summary>
		/// Not used?
		/// </summary>
		private const string crDefault = "Default";
		/// <summary>
		/// String indicating which cursor resource should be used for this control.
		/// </summary>
		private const string crText = "Text";
		#endregion

		#region Fields
		/// <summary>
		/// Indicates if the cursor should be displayed when hovered. ???
		/// </summary>
		private bool showCursor = false;
		/// <summary>
		/// Tracks elapsed time values used to control the visible state of the text insertion cursor.
		/// </summary>
		private double flashTime = 0;
		/// <summary>
		/// X position of the text caret.
		/// </summary>
		private int posx = 0;
		/// <summary>
		/// Y position of the text caret.
		/// </summary>
		private int posy = 0;
		/// <summary>
		/// Specifies which character will be used to mask input when the text box is in Password mode.
		/// </summary>
		private char passwordChar = '•';
		/// <summary>
		/// Indicates if the text box is a single-line, multi-line, or password text box.
		/// </summary>
		private TextBoxMode mode = TextBoxMode.Normal;
		/// <summary>
		/// Text that is currently visible in the client area. 
		/// </summary>
		private string shownText = "";
		/// <summary>
		/// Indicates if the text box can accept user input or if it is read-only.
		/// </summary>
		private bool readOnly = false;
		/// <summary>
		/// Indicates if the borders of the text box should be drawn.
		/// </summary>
		private bool drawBorders = true;
		/// <summary>
		/// Currently selected text of the control, specified by starting and ending indexes.
		/// </summary>
		private Selection selection = new Selection(-1, -1);
		/// <summary>
		/// Indicates if the caret is displayed or not.
		/// </summary>
		private bool caretVisible = true;
		/// <summary>
		/// Horizontal scroll bar of the text box.
		/// </summary>
		private ScrollBar horz = null;
		/// <summary>
		/// Vertical scroll bar of the text box.
		/// </summary>
		private ScrollBar vert = null;
		/// <summary>
		/// Text content broken into individual lines.
		/// </summary>
		private List<string> lines = new List<string>();
		/// <summary>
		/// Number of lines of text that can fit vertically in the client area.
		/// </summary>
		private int linesDrawn = 0;
		/// <summary>
		/// Number of characters that can fit horizontally in the client area.
		/// </summary>
		private int charsDrawn = 0;
		/// <summary>
		/// Font used to draw the control's text.
		/// </summary>
		private SpriteFont font = null;
		/// <summary>
		/// Indicates if word wrap is enabled on multi-line text boxes.
		/// </summary>
		private bool wordWrap = false;
		/// <summary>
		/// Specifies which, if any, scroll bars should be displayed in the text box.
		/// </summary>
		private ScrollBars scrollBars = ScrollBars.Both;
		/// <summary>
		/// Characted used as the line separator character.
		/// </summary>
		private string Separator = "\n";
		/// <summary>
		/// Current text content of the control.
		/// </summary>
		private string text = "";
		/// <summary>
		/// Internal use during text splitting operations.
		/// </summary>
		private string buffer = "";
		/// <summary>
		/// Indicates if all text should be selected automatically when the control gains focus.
		/// </summary>
		private bool autoSelection = true;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the X position of the caret on the current line.
		/// </summary>
		private int PosX
		{
			get
			{
				return posx;
			}
			set
			{
				posx = value;

				if (posx < 0) posx = 0;
				if (posx > Lines[PosY].Length) posx = Lines[PosY].Length;
			}
		}

		/// <summary>
		/// Gets or sets the Y position of the caret in the text box.
		/// </summary>
		private int PosY
		{
			get
			{
				return posy;
			}
			set
			{
				posy = value;

				if (posy < 0) posy = 0;
				if (posy > Lines.Count - 1) posy = Lines.Count - 1;

				PosX = PosX;
			}
		}

		/// <summary>
		/// Gets or sets the position of the caret in the text box.
		/// </summary>
		private int Pos
		{
			get
			{
				return GetPos(PosX, PosY);
			}
			set
			{
				PosY = GetPosY(value);
				PosX = GetPosX(value);
			}
		}
		
		//>>>>
		/*
		/// <summary>
		/// Indicates if word wrap is enabled in multi-line text box controls.
		/// </summary>
		public virtual bool WordWrap
		{
		  get { return wordWrap; }
		  set 
		  { 
			wordWrap = value;        
			if (ClientArea != null) ClientArea.Invalidate(); 
			SetupBars();
		  }
		}*/
		
		/// <summary>
		/// Gets or sets the scroll bars the text box should display.
		/// </summary>
		public virtual ScrollBars ScrollBars
		{
			get { return scrollBars; }
			set
			{
				scrollBars = value;
				SetupBars();
			}
		}

		/// <summary>
		/// Gets or sets the character used to mask input when the text box is in password mode.
		/// </summary>
		public virtual char PasswordChar
		{
			get { return passwordChar; }
			set { passwordChar = value; if (ClientArea != null) ClientArea.Invalidate(); }
		}

		/// <summary>
		/// Indicates if the text insertion position is visible or not.
		/// </summary>
		public virtual bool CaretVisible
		{
			get { return caretVisible; }
			set { caretVisible = value; }
		}

		/// <summary>
		/// Gets or sets the current mode of the text box control.
		/// </summary>
		public virtual TextBoxMode Mode
		{
			get { return mode; }
			set
			{
				if (value != TextBoxMode.Multiline)
				{
					Text = Text.Replace(Separator, "");
				}
				mode = value;
				selection.Clear();

				if (ClientArea != null) ClientArea.Invalidate();
				SetupBars();
			}
		}

		/// <summary>
		/// Indicates if the text box allows user input or not.
		/// </summary>
		public virtual bool ReadOnly
		{
			get { return readOnly; }
			set { readOnly = value; }
		}

		/// <summary>
		/// Indicates if the borders of the text box control should be drawn or not.
		/// </summary>
		public virtual bool DrawBorders
		{
			get { return drawBorders; }
			set { drawBorders = value; if (ClientArea != null) ClientArea.Invalidate(); }
		}

		/// <summary>
		/// Gets or sets the current position of the caret in the text box.
		/// </summary>
		public virtual int CursorPosition
		{
			get { return Pos; }
			set
			{
				Pos = value;
			}
		}

		/// <summary>
		/// Gets all text within the current selection.
		/// </summary>
		public virtual string SelectedText
		{
			get
			{
				if (selection.IsEmpty)
				{
					return "";
				}
				else
				{
					return Text.Substring(selection.Start, selection.Length);
				}
			}
		}

		/// <summary>
		/// Gets or sets the start position of the selection.
		/// </summary>
		public virtual int SelectionStart
		{
			get
			{
				if (selection.IsEmpty)
				{
					return Pos;
				}
				else
				{
					return selection.Start;
				}
			}
			set
			{
				Pos = value;
				if (Pos < 0) Pos = 0;
				if (Pos > Text.Length) Pos = Text.Length;
				selection.Start = Pos;
				if (selection.End == -1) selection.End = Pos;
				ClientArea.Invalidate();
			}
		}

		/// <summary>
		/// Indicates if all text should be selected automatically when the text box receives focus.
		/// </summary>
		public virtual bool AutoSelection
		{
			get { return autoSelection; }
			set { autoSelection = value; }
		}

		/// <summary>
		/// Gets or sets (from the current value of SelectionStart) the length of the selection. 
		/// </summary>
		public virtual int SelectionLength
		{
			get
			{
				return selection.Length;
			}
			set
			{
				if (value == 0)
				{
					selection.End = selection.Start;
				}
				else if (selection.IsEmpty)
				{
					selection.Start = 0;
					selection.End = value;
				}
				else if (!selection.IsEmpty)
				{
					selection.End = selection.Start + value;
				}

				if (!selection.IsEmpty)
				{
					if (selection.Start < 0) selection.Start = 0;
					if (selection.Start > Text.Length) selection.Start = Text.Length;
					if (selection.End < 0) selection.End = 0;
					if (selection.End > Text.Length) selection.End = Text.Length;
				}
				ClientArea.Invalidate();
			}
		}

		/// <summary>
		/// Returns the text content as a Separator delimited list of strings.
		/// </summary>
		private List<string> Lines
		{
			get
			{
				return lines;
			}
			set
			{
				lines = value;
			}
		}

		/// <summary>
		/// Gets or sets the contents of the text box control.
		/// </summary>
		public override string Text
		{
			get
			{
				return text;
			}
			set
			{
				if (mode != TextBoxMode.Multiline && value != null)
				{
					value = value.Replace(Separator, "");
				}

				text = value;

				if (!Suspended) OnTextChanged(new EventArgs());

				lines = SplitLines(text);
				if (ClientArea != null) ClientArea.Invalidate();

				SetupBars();
				ProcessScrolling();
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new TextBox control.
		/// </summary>
		/// <param name="manager">GUI manager for the control.</param>
		public TextBox(Manager manager)
			: base(manager)
		{
			// Cursor layer defined?
			CheckLayer(Skin, lrCursor);

			SetDefaultSize(128, 20);
			Lines.Add("");

			ClientArea.Draw += new DrawEventHandler(ClientArea_Draw);

			// Create the scroll bars for the text box.
			vert = new ScrollBar(manager, Orientation.Vertical);
			horz = new ScrollBar(manager, Orientation.Horizontal);
		}
		#endregion

		#region Init
		/// <summary>
		/// Initializes the text box control.
		/// </summary>
		public override void Init()
		{
			base.Init();

			// Set up the vertical scroll bar.
			vert.Init();
			vert.Range = 1;
			vert.PageSize = 1;
			vert.Value = 0;
			vert.Anchor = Anchors.Top | Anchors.Right | Anchors.Bottom;
			vert.ValueChanged += new EventHandler(sb_ValueChanged);

			// Set up the horizontal scroll bar.
			horz.Init();
			horz.Range = ClientArea.Width;
			horz.PageSize = ClientArea.Width;
			horz.Value = 0;
			horz.Anchor = Anchors.Right | Anchors.Left | Anchors.Bottom;
			horz.ValueChanged += new EventHandler(sb_ValueChanged);

			horz.Visible = false;
			vert.Visible = false;

			Add(vert, false);
			Add(horz, false);
		}
		#endregion

		#region Init Skin
		/// <summary>
		/// Initializes the skin of the text box control.
		/// </summary>
		protected internal override void InitSkin()
		{
			base.InitSkin();
			Skin = new SkinControl(Manager.Skin.Controls[skTextBox]);

#if (!XBOX && !XBOX_FAKE)
			Cursor = Manager.Skin.Cursors[crText].Resource;
#endif
			font = (Skin.Layers[lrTextBox].Text != null) ? Skin.Layers[lrTextBox].Text.Font.Resource : null;
		}
		#endregion

		#region Draw Control
		/// <summary>
		/// Draws the text box control.
		/// </summary>
		/// <param name="renderer">Render management object.</param>
		/// <param name="rect">Destination region where the text box should be drawn.</param>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		protected override void DrawControl(Renderer renderer, Rectangle rect, GameTime gameTime)
		{
			// Need to draw borders? 
			if (drawBorders)
			{
				base.DrawControl(renderer, rect, gameTime);
			}
		}
		#endregion

		#region Get Fit Chars
		/// <summary>
		/// Returns the number of characters of the specified text that will fit within the specified width.
		/// </summary>
		/// <param name="text">Text string to fit.</param>
		/// <param name="width">Width available for text placement.</param>
		/// <returns>Returns the number of characters, from the start of the string, that will fit in the width specified.</returns>
		private int GetFitChars(string text, int width)
		{
			// All characters will fit unless proven otherwise.
			int ret = text.Length;
			int size = 0;

			for (int i = 0; i < text.Length; i++)
			{
				// Get the width of the current substring.
				size = (int)font.MeasureString(text.Substring(0, i)).X;
				
				// Too large? Update character count and exit.
				if (size > width)
				{
					ret = i;
					break;
				}
			}

			return ret;
		}
		#endregion

		#region Determine Pages
		/// <summary>
		/// Updates the number of lines and characters drawn based on the current dimensions of the
		/// client area of the text box.
		/// </summary>
		private void DeterminePages()
		{
			if (ClientArea != null)
			{
				// Get height of a single line of text.
				int sizey = (int)font.LineSpacing;

				// Update the number of lines that can fit within the current height of the text area.
				linesDrawn = (int)(ClientArea.Height / sizey);

				// Can't draw more lines than there actually is.
				if (linesDrawn > Lines.Count) linesDrawn = Lines.Count;

				// Update the number of characters drawn. 
				// NOTE: How exactly does this work out?
				charsDrawn = ClientArea.Width - 1;
			}
		}
		#endregion

		#region Get Max Line
		/// <summary>
		/// Gets the line of the text box with the greatest length.
		/// </summary>
		/// <returns>The longest line in the text box.</returns>
		private string GetMaxLine()
		{
			int max = 0;
			int x = 0;

			for (int i = 0; i < Lines.Count; i++)
			{
				if (Lines[i].Length > max)
				{
					max = Lines[i].Length;
					x = i;
				}
			}
			return Lines.Count > 0 ? Lines[x] : "";
		}
		#endregion

		#region Client Area Draw Event Handler
		/// <summary>
		/// Handles drawing the client area of the text box control.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void ClientArea_Draw(object sender, DrawEventArgs e)
		{
			// Grab the text box control's skin information.
			SkinLayer layer = Skin.Layers[lrTextBox];
			Color col = Skin.Layers[lrTextBox].Text.Colors.Enabled;
			SkinLayer cursor = Skin.Layers[lrCursor];

			// Multi-line text boxes are aligned top-left; other types have their line centered vertically.
			Alignment al = mode == TextBoxMode.Multiline ? Alignment.TopLeft : Alignment.MiddleLeft;
			Renderer renderer = e.Renderer;
			Rectangle r = e.Rectangle;

			// Text box has a selected text to consider?
			bool drawsel = !selection.IsEmpty;
			string tmpText = "";

			// Get the font used for drawing the text box contents.
			font = (Skin.Layers[lrTextBox].Text != null) ? Skin.Layers[lrTextBox].Text.Font.Resource : null;

			// Control has text to draw and we have a font to draw it with?
			if (Text != null && font != null)
			{
				DeterminePages();

				if (mode == TextBoxMode.Multiline)
				{
					shownText = Text;
					tmpText = Lines[PosY];
				}

				else if (mode == TextBoxMode.Password)
				{
					// Mask the text using the password character.
					shownText = "";
					for (int i = 0; i < Text.Length; i++)
					{
						shownText = shownText + passwordChar;
					}
					tmpText = shownText;
				}
				
				else
				{
					shownText = Text;
					tmpText = Lines[PosY];
				}

				// Text color defined and control not disabled.
				if (TextColor != UndefinedColor && ControlState != ControlState.Disabled)
				{
					// Use the control's text color value.
					col = TextColor;
				}

				if (mode != TextBoxMode.Multiline)
				{
					linesDrawn = 0;
					vert.Value = 0;
				}

				// Is there a selection to draw?
				if (drawsel)
				{
					DrawSelection(e.Renderer, r);
					/*
							  renderer.End();          
							  renderer.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
							  renderer.SpriteBatch.GraphicsDevice.RenderState.SeparateAlphaBlendEnabled = true;
							  renderer.SpriteBatch.GraphicsDevice.RenderState.SourceBlend = Blend.DestinationColor;
							  renderer.SpriteBatch.GraphicsDevice.RenderState.DestinationBlend = Blend.SourceColor;
							  renderer.SpriteBatch.GraphicsDevice.RenderState.BlendFunction = BlendFunction.Subtract;          
							  //renderer.SpriteBatch.GraphicsDevice.RenderState.AlphaFunction = CompareFunction.Equal;
							  //renderer.SpriteBatch.GraphicsDevice.RenderState.AlphaSourceBlend = Blend.One;
							  //renderer.SpriteBatch.GraphicsDevice.RenderState.AlphaDestinationBlend = Blend.DestinationAlpha;
					 */
				}

				int sizey = (int)font.LineSpacing;

				// Need to draw the caret?
				if (showCursor && caretVisible)
				{
					Vector2 size = Vector2.Zero;

					if (PosX > 0 && PosX <= tmpText.Length)
					{
						size = font.MeasureString(tmpText.Substring(0, PosX));
					}

					if (size.Y == 0)
					{
						size = font.MeasureString(" ");
						size.X = 0;
					}

					int m = r.Height - font.LineSpacing;

					// Create the rectangle where the cursor should be drawn. 
					Rectangle rc = new Rectangle(r.Left - horz.Value + (int)size.X, r.Top + m / 2, cursor.Width, font.LineSpacing);

					// Adjust rectangle to account for current vertical scroll bar value?
					if (mode == TextBoxMode.Multiline)
					{
						rc = new Rectangle(r.Left + (int)size.X - horz.Value, r.Top + (int)((PosY - vert.Value) * font.LineSpacing), cursor.Width, font.LineSpacing);
					}

					// Draw the cursor in the text box.
					cursor.Alignment = al;
					renderer.DrawLayer(cursor, rc, col, 0);
				}

				// Draw all visible text.
				for (int i = 0; i < linesDrawn + 1; i++)
				{
					int ii = i + vert.Value;
					if (ii >= Lines.Count || ii < 0) break;

					if (Lines[ii] != "")
					{
						if (mode == TextBoxMode.Multiline)
						{
							renderer.DrawString(font, Lines[ii], r.Left - horz.Value, r.Top + (i * sizey), col);
						}
						else
						{
							Rectangle rx = new Rectangle(r.Left - horz.Value, r.Top, r.Width, r.Height);
							renderer.DrawString(font, shownText, rx, col, al, false);
						}
					}
				}

				/*  if (drawsel)
				  {
					renderer.End();
					renderer.Begin(BlendingMode.Premultiplied);
				  }*/
			}
		}
		#endregion

		#region Get String Width
		/// <summary>
		/// Measures the width of the specified text or a sub-string of the text. 
		/// </summary>
		/// <param name="text">String to measure the width of.</param>
		/// <param name="count">Number of characters from the start of the string to measure.</param>
		/// <returns>Returns the width of the specified number of characters in the supplied text.</returns>
		private int GetStringWidth(string text, int count)
		{
			if (count > text.Length) count = text.Length;
			return (int)font.MeasureString(text.Substring(0, count)).X;
		}
		#endregion

		#region Process Scrolling
		/// <summary>
		/// Updates scroll bar values and page sizes based on the dimensions of the client area
		/// of the text box and the current cursor position within the text box.
		/// </summary>
		private void ProcessScrolling()
		{
			if (vert != null && horz != null)
			{
				// Update page size values based on dimensions of client area. 
				vert.PageSize = linesDrawn;
				horz.PageSize = charsDrawn;

				// Clamp horizontal page value in range.
				if (horz.PageSize > horz.Range) horz.PageSize = horz.Range;

				// Update vertical scroll bar value so the current insertion position is visible.
				if (PosY >= vert.Value + vert.PageSize)
				{
					vert.Value = (PosY + 1) - vert.PageSize;
				}

				else if (PosY < vert.Value)
				{
					vert.Value = PosY;
				}

				// Update horizontal scroll bar value so the current insertion position is visible.
				if (GetStringWidth(Lines[PosY], PosX) >= horz.Value + horz.PageSize)
				{
					horz.Value = (GetStringWidth(Lines[PosY], PosX) + 1) - horz.PageSize;
				}

				else if (GetStringWidth(Lines[PosY], PosX) < horz.Value)
				{
					horz.Value = GetStringWidth(Lines[PosY], PosX) - horz.PageSize;
				}
			}
		}
		#endregion

		#region Draw Selection
		/// <summary>
		/// Draws the text box's selection overlay to highlight selected text.
		/// </summary>
		/// <param name="renderer">Render management object.</param>
		/// <param name="rect">Region where the selection overlay should be drawn.</param>
		private void DrawSelection(Renderer renderer, Rectangle rect)
		{
			if (!selection.IsEmpty)
			{
				int s = selection.Start;
				int e = selection.End;

				// Get selection's starting line index, ending line index, starting column index, and ending column index.
				int sl = GetPosY(s);
				int el = GetPosY(e);
				int sc = GetPosX(s);
				int ec = GetPosX(e);

				// Selection height is the height of a single line of text.
				int hgt = font.LineSpacing;

				int start = sl;
				int end = el;

				// Adjust start and end positions to account for vertical scroll values.
				if (start < vert.Value) start = vert.Value;
				if (end > vert.Value + linesDrawn) end = vert.Value + linesDrawn;

				// Draw each line of the selection. 
				for (int i = start; i <= end; i++)
				{
					Rectangle r = Rectangle.Empty;

					if (mode == TextBoxMode.Normal)
					{
						int m = ClientArea.Height - font.LineSpacing;
						r = new Rectangle(rect.Left - horz.Value + (int)font.MeasureString(Lines[i].Substring(0, sc)).X, rect.Top + m / 2,
										 (int)font.MeasureString(Lines[i].Substring(0, ec + 0)).X - (int)font.MeasureString(Lines[i].Substring(0, sc)).X, hgt);
					}
					else if (sl == el)
					{
						r = new Rectangle(rect.Left - horz.Value + (int)font.MeasureString(Lines[i].Substring(0, sc)).X, rect.Top + (i - vert.Value) * hgt,
										  (int)font.MeasureString(Lines[i].Substring(0, ec + 0)).X - (int)font.MeasureString(Lines[i].Substring(0, sc)).X, hgt);
					}
					else
					{
						if (i == sl) r = new Rectangle(rect.Left - horz.Value + (int)font.MeasureString(Lines[i].Substring(0, sc)).X, rect.Top + (i - vert.Value) * hgt, (int)font.MeasureString(Lines[i]).X - (int)font.MeasureString(Lines[i].Substring(0, sc)).X, hgt);
						else if (i == el) r = new Rectangle(rect.Left - horz.Value, rect.Top + (i - vert.Value) * hgt, (int)font.MeasureString(Lines[i].Substring(0, ec + 0)).X, hgt);
						else r = new Rectangle(rect.Left - horz.Value, rect.Top + (i - vert.Value) * hgt, (int)font.MeasureString(Lines[i]).X, hgt);
					}

					renderer.Draw(Manager.Skin.Images["Control"].Resource, r, Color.FromNonPremultiplied(160, 160, 160, 128));
				}
			}
		}
		#endregion

		#region Update
		/// <summary>
		/// Updates the text box cursor state.
		/// </summary>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		protected internal override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			bool sc = showCursor;

			// Only show the cursor when the text box has focus.
			showCursor = Focused;

			if (Focused)
			{
				// Update the cursor flash timer and display/hide the cursor every 0.5 seconds.
				flashTime += gameTime.ElapsedGameTime.TotalSeconds;
				showCursor = flashTime < 0.5;
				if (flashTime > 1) flashTime = 0;
			}

			// Visibility of the cursor has changed? Redraw.
			if (sc != showCursor) ClientArea.Invalidate();
		}
		#endregion

		#region Find Prev Word
		/// <summary>
		/// From the current cursor position, this finds the index of the start of the word behind it. 
		/// This will be the start of the current word, if the cursor is positioned in the middle of a
		/// word, or the start of the previous word if the cursor is at the start of a word..
		/// </summary>
		/// <param name="text">Text content to search.</param>
		/// <returns>Returns the index of the start of the previous word or zero if the cursor has reached the starting point.</returns>
		private int FindPrevWord(string text)
		{
			bool letter = false;

			// Get current position of the cursor.
			int p = Pos - 1;
			if (p < 0) p = 0;
			if (p >= text.Length) p = text.Length - 1;

			// Search backwards from the current position
			for (int i = p; i >= 0; i--)
			{
				// First non whitespace character from current position indicates start 
				// of the word we want to find the start of.
				if (char.IsLetterOrDigit(text[i]))
				{
					letter = true;
					continue;
				}

				// First white space character indicates that we are at the beginning
				// of the word behind the cursor's current position.
				if (letter && !char.IsLetterOrDigit(text[i]))
				{
					return i + 1;
				}
			}

			// Reached the beginning of the text string.
			return 0;
		}
		#endregion

		#region Find Next Word
		/// <summary>
		/// From the current cursor position, this finds the index of the start of the word ahead of it. 
		/// </summary>
		/// <param name="text">Text content to search.</param>
		/// <returns>Returns the index of the start of the next word or the last valid index if the cursor has reached the end point.</returns>
		private int FindNextWord(string text)
		{
			bool space = false;

			for (int i = Pos; i < text.Length - 1; i++)
			{
				if (!char.IsLetterOrDigit(text[i]))
				{
					space = true;
					continue;
				}

				// First non-whitespace character after the first encountered space is the start of the next word.
				if (space && char.IsLetterOrDigit(text[i]))
				{
					return i;
				}
			}

			// Reached the end of the text.
			return text.Length;
		}
		#endregion

		#region Get Pos Y
		/// <summary>
		/// Gets the line index where the cursor is currently positioned.
		/// </summary>
		/// <param name="pos">Cursor position in text.</param>
		/// <returns>Returns the index of the line where the cursor is positioned.</returns>
		private int GetPosY(int pos)
		{
			// Cursor is past the last line of text?
			if (pos >= Text.Length) return Lines.Count - 1;

			int p = pos;

			// Determine which line the cursor is on.
			for (int i = 0; i < Lines.Count; i++)
			{
				// If p - line length is less than zero, the cursor is located somewhere
				// in the current line. Return the current line index.
				p -= Lines[i].Length + Separator.Length;
				if (p < 0)
				{
					p = p + Lines[i].Length + Separator.Length;
					return i;
				}
			}
			return 0;
		}
		#endregion

		#region Get Pos X
		/// <summary>
		/// Gets the column index of the specified position.
		/// </summary>
		/// <param name="pos">Position of the cursor in the text.</param>
		/// <returns>Returns the index of the column at the specified cursor position.</returns>
		private int GetPosX(int pos)
		{
			// Cursor is at the end of the text content?
			if (pos >= Text.Length) return Lines[Lines.Count - 1].Length;

			int p = pos;

			// Figure out what column the cursor is positioned at.
			for (int i = 0; i < Lines.Count; i++)
			{
				p -= Lines[i].Length + Separator.Length;
				if (p < 0)
				{
					p = p + Lines[i].Length + Separator.Length;
					return p;
				}
			}
			return 0;
		}
		#endregion

		#region Get Pos
		/// <summary>
		/// Given the column (x) and line (y) indexes, this returns the cursor position 
		/// that matches the specified location.
		/// </summary>
		/// <param name="x">Column index.</param>
		/// <param name="y">Line index.</param>
		/// <returns>Returns the cursor position for the specified location.</returns>
		private int GetPos(int x, int y)
		{
			int p = 0;

			for (int i = 0; i < y; i++)
			{
				p += Lines[i].Length + Separator.Length;
			}
			p += x;

			return p;
		}
		#endregion

		#region Char At Pos
		/// <summary>
		/// Given a point (such as mouse position), this determines the position in the text
		/// box that is closest to the specified position.
		/// </summary>
		/// <param name="pos">Point to find the text position of.</param>
		/// <returns>Returns the cursor position that corresponds to the point received.</returns>
		private int CharAtPos(Point pos)
		{
			int x = pos.X;
			int y = pos.Y;
			int px = 0;
			int py = 0;

			// Is there more than one line of text to consider?
			if (mode == TextBoxMode.Multiline)
			{
				// Get the line index under the specified point.
				py = vert.Value + (int)((y - ClientTop) / font.LineSpacing);
				if (py < 0) py = 0;
				if (py >= Lines.Count) py = Lines.Count - 1;
			}
			
			else
			{
				// Otherwise, line index is zero.
				py = 0;
			}

			string str = mode == TextBoxMode.Multiline ? Lines[py] : shownText;

			if (str != null && str != "")
			{
				// Determine X position within the current line.
				for (int i = 1; i <= Lines[py].Length; i++)
				{
					Vector2 v = font.MeasureString(str.Substring(0, i)) - (font.MeasureString(str[i - 1].ToString()) / 3);
					if (x <= (ClientLeft + (int)v.X) - horz.Value)
					{
						px = i - 1;
						break;
					}
				}
				if (x > ClientLeft + ((int)font.MeasureString(str).X) - horz.Value - (font.MeasureString(str[str.Length - 1].ToString()).X / 3)) px = str.Length;
			}

			return GetPos(px, py);
		}
		#endregion

		#region On Mouse Down Event Handler
		/// <summary>
		/// Handles mouse button down events for the text box.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			flashTime = 0;

			// Reposition caret.
			Pos = CharAtPos(e.Position);
			selection.Clear();

			// Update selection?
			if (e.Button == MouseButton.Left && caretVisible && mode != TextBoxMode.Password)
			{
				selection.Start = Pos;
				selection.End = Pos;
			}
			ClientArea.Invalidate();
		}
		#endregion

		#region On Mouse Mose Event Handler
		/// <summary>
		/// Handles mouse move events for the text box.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			// Mouse move + Left button down = Update selection.
			if (e.Button == MouseButton.Left && !selection.IsEmpty && mode != TextBoxMode.Password && selection.Length < Text.Length)
			{
				int pos = CharAtPos(e.Position);
				selection.End = CharAtPos(e.Position);
				Pos = pos;

				ClientArea.Invalidate();

				ProcessScrolling();
			}
		}
		#endregion

		#region On Mouse Up Event Handler
		/// <summary>
		/// Handles mouse up events for the text box.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			// Clear selection if the text box receives a left button click.
			if (e.Button == MouseButton.Left && !selection.IsEmpty && mode != TextBoxMode.Password)
			{
				if (selection.Length == 0) selection.Clear();
			}
		}
		#endregion

        protected override void OnMouseScroll(MouseEventArgs e)
        {
            if (Mode != TextBoxMode.Multiline)
            {
                base.OnMouseScroll(e);
                return;
            }

            if (e.ScrollDirection == MouseScrollDirection.Down)
                vert.ScrollDown();
            else
                vert.ScrollUp();

            base.OnMouseScroll(e);
        }

		#region On Key Press Event Handler
		/// <summary>
		/// Handles key press events for the text box.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyPress(KeyEventArgs e)
		{
			// Reset the timer used to flash the caret.
			flashTime = 0;

			if (Manager.UseGuide && Guide.IsVisible) return;

			// Key event handled already?
			if (!e.Handled)
			{
				// Control + A = Select All Text.
				if (e.Key == Keys.A && e.Control && mode != TextBoxMode.Password)
				{
					SelectAll();
				}

				// Up arrow key press?
				if (e.Key == Keys.Up)
				{
					e.Handled = true;

					// Begin selection on Shift + Up if a selection isn't already set.
					if (e.Shift && selection.IsEmpty && mode != TextBoxMode.Password)
					{
						selection.Start = Pos;
					}

					// Move the caret up a line.
					if (!e.Control)
					{
						PosY -= 1;
					}
				}

				// Down arrow key press?
				else if (e.Key == Keys.Down)
				{
					e.Handled = true;

					// Begin selection on Shift + Down if a selection isn't already set.
					if (e.Shift && selection.IsEmpty && mode != TextBoxMode.Password)
					{
						selection.Start = Pos;
					}

					// Move the caret down a line.
					if (!e.Control)
					{
						PosY += 1;
					}
				}

				// Delete text if Backspace pressed?
				else if (e.Key == Keys.Back && !readOnly)
				{
					e.Handled = true;

					// Delete all selected text?
					if (!selection.IsEmpty)
					{
						Text = Text.Remove(selection.Start, selection.Length);
						Pos = selection.Start;
					}

					// Remove a single character?
					else if (Text.Length > 0 && Pos > 0)
					{
						Pos -= 1;
						Text = Text.Remove(Pos, 1);
					}

					// Clear selection.
					selection.Clear();
				}

				// Delete text if Delete is pressed?
				else if (e.Key == Keys.Delete && !readOnly)
				{
					e.Handled = true;

					// Delete the selected text?
					if (!selection.IsEmpty)
					{
						Text = Text.Remove(selection.Start, selection.Length);
						Pos = selection.Start;
					}

					// Remove the character after the caret?
					else if (Pos < Text.Length)
					{
						Text = Text.Remove(Pos, 1);
					}

					// Clear the selection.
					selection.Clear();
				}

				// Left arrow key pressed?
				else if (e.Key == Keys.Left)
				{
					e.Handled = true;

					// Begin selection?
					if (e.Shift && selection.IsEmpty && mode != TextBoxMode.Password)
					{
						selection.Start = Pos;
					}

					// Move the caret back one position.
					if (!e.Control)
					{
						Pos -= 1;
					}

					// Move the caret to the start of the previous word on Control + Left.
					if (e.Control)
					{
						Pos = FindPrevWord(shownText);
					}
				}

				// Right arrow key pressed?
				else if (e.Key == Keys.Right)
				{
					e.Handled = true;

					// Begin selection?
					if (e.Shift && selection.IsEmpty && mode != TextBoxMode.Password)
					{
						selection.Start = Pos;
					}

					// Move the caret forward one position?
					if (!e.Control)
					{
						Pos += 1;
					}

					// Move the caret to the start of the next word on Control + Right?
					if (e.Control)
					{
						Pos = FindNextWord(shownText);
					}
				}

				// Home key pressed?
				else if (e.Key == Keys.Home)
				{
					e.Handled = true;

					// Begin selection?
					if (e.Shift && selection.IsEmpty && mode != TextBoxMode.Password)
					{
						selection.Start = Pos;
					}

					// Move caret to the start of the current line?
					if (!e.Control)
					{
						PosX = 0;
					}

					// Move caret to the first position on Control + Home?
					if (e.Control)
					{
						Pos = 0;
					}
				}

				// End key pressed?
				else if (e.Key == Keys.End)
				{
					e.Handled = true;

					// Begin selection?
					if (e.Shift && selection.IsEmpty && mode != TextBoxMode.Password)
					{
						selection.Start = Pos;
					}

					// Move the caret to the end of the current line?
					if (!e.Control)
					{
						PosX = Lines[PosY].Length;
					}

					// Move the caret to the end of the text box.
					if (e.Control)
					{
						Pos = Text.Length;
					}
				}

				// Page Up key pressed? 
				else if (e.Key == Keys.PageUp)
				{
					e.Handled = true;

					// Begin selection?
					if (e.Shift && selection.IsEmpty && mode != TextBoxMode.Password)
					{
						selection.Start = Pos;
					}
					
					// Move the caret up a full page of text.
					if (!e.Control)
					{
						PosY -= linesDrawn;
					}
				}

				// Page Down key pressed?
				else if (e.Key == Keys.PageDown)
				{
					e.Handled = true;

					// Begin selection?
					if (e.Shift && selection.IsEmpty && mode != TextBoxMode.Password)
					{
						selection.Start = Pos;
					}

					// Move the caret down a full page of text.
					if (!e.Control)
					{
						PosY += linesDrawn;
					}
				}

				// Insert new line on Enter key press?
				else if (e.Key == Keys.Enter && mode == TextBoxMode.Multiline && !readOnly)
				{
					e.Handled = true;
					Text = Text.Insert(Pos, Separator);
					PosX = 0;
					PosY += 1;
				}

				// Tab key pressed?
				else if (e.Key == Keys.Tab)
				{
				}

				// Handle all other key press events.
				else if (!readOnly && !e.Control)
				{
					string c = Manager.KeyboardLayout.GetKey(e);

					// Insert text?
					if (selection.IsEmpty)
					{
						Text = Text.Insert(Pos, c);
						if (c != "") PosX += 1;
					}

					// Replace selection?
					else
					{
						if (Text.Length > 0)
						{
							Text = Text.Remove(selection.Start, selection.Length);
							Text = Text.Insert(selection.Start, c);
							Pos = selection.Start + 1;
						}
						selection.Clear();
					}
				}

				// Update the end of selection? 
				if (e.Shift && !selection.IsEmpty)
				{
					selection.End = Pos;
				}

				// Copy selected text to clipboard on Control + C pressed and running on Windows.
				if (e.Control && e.Key == Keys.C && mode != TextBoxMode.Password)
				{
#if (!XBOX && !XBOX_FAKE)
					System.Windows.Forms.Clipboard.Clear();
					if (mode != TextBoxMode.Password && !selection.IsEmpty)
					{
						System.Windows.Forms.Clipboard.SetText((Text.Substring(selection.Start, selection.Length)).Replace("\n", Environment.NewLine));
					}
#endif
				}
				
				// Paste from clipboard on Control + V if running on Windows.
				else if (e.Control && e.Key == Keys.V && !readOnly && mode != TextBoxMode.Password)
				{
#if (!XBOX && !XBOX_FAKE)
					string t = System.Windows.Forms.Clipboard.GetText().Replace(Environment.NewLine, "\n");
					
					// Insert text at current position?
					if (selection.IsEmpty)
					{
						Text = Text.Insert(Pos, t);
						Pos = Pos + t.Length;
					}
					
					// Replace selection with pasted text?
					else
					{
						Text = Text.Remove(selection.Start, selection.Length);
						Text = Text.Insert(selection.Start, t);
						PosX = selection.Start + t.Length;
						selection.Clear();
					}
#endif
				}

				// Clear selection?
				if ((!e.Shift && !e.Control) || Text.Length <= 0)
				{
					selection.Clear();
				}

				// Show guide on Control + Down.
				if (e.Control && e.Key == Keys.Down)
				{
					e.Handled = true;
					HandleGuide(PlayerIndex.One);
				}
				
				flashTime = 0;
				
				if (ClientArea != null) ClientArea.Invalidate();

				DeterminePages();
				ProcessScrolling();
			}
			base.OnKeyPress(e);
		}
		#endregion

		#region On Game Pad Down Event Handler
		/// <summary>
		/// Handles gamepad button down events for the text box.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnGamePadDown(GamePadEventArgs e)
		{
			// Guide visible?
			if (Manager.UseGuide && Guide.IsVisible) return;

			if (!e.Handled)
			{
				// Clicking a text box with a gamepad?
				if (e.Button == GamePadActions.Click)
				{
					// Display the on-screen keyboard.
					e.Handled = true;
					HandleGuide(e.PlayerIndex);
				}
			}
			base.OnGamePadDown(e);
		}
		#endregion

		#region Handle Guide
		/// <summary>
		/// Begins showing the on-screen keyboard for text input via gamepad.
		/// </summary>
		/// <param name="pi"></param>
		private void HandleGuide(PlayerIndex pi)
		{
			if (Manager.UseGuide && !Guide.IsVisible)
			{
				Guide.BeginShowKeyboardInput(pi, "Enter Text", "", Text, GetText, pi.ToString());
			}
		}
		#endregion

		#region Get Text
		/// <summary>
		/// Gets the text result from gamepad input via on-screen keyboard.
		/// </summary>
		/// <param name="result"></param>
		private void GetText(IAsyncResult result)
		{
			string res = Guide.EndShowKeyboardInput(result);
			Text = res != null ? res : "";
			Pos = text.Length;
		}
		#endregion

		#region Setup Bars
		/// <summary>
		/// Updates scroll bar settings based on dimensions of the client area and text content.
		/// </summary>
		private void SetupBars()
		{
			DeterminePages();

			if (vert != null) vert.Range = Lines.Count;
			if (horz != null)
			{
				horz.Range = (int)font.MeasureString(GetMaxLine()).X;
				if (horz.Range == 0) horz.Range = ClientArea.Width;
			}

			if (vert != null)
			{
				vert.Left = Width - 16 - 2;
				vert.Top = 2;
				vert.Height = Height - 4 - 16;

				if (Height < 50 || (scrollBars != ScrollBars.Both && scrollBars != ScrollBars.Vertical)) vert.Visible = false;
				else if ((scrollBars == ScrollBars.Vertical || scrollBars == ScrollBars.Both) && mode == TextBoxMode.Multiline) vert.Visible = true;
			}

			if (horz != null)
			{
				horz.Left = 2;
				horz.Top = Height - 16 - 2;
				horz.Width = Width - 4 - 16;

				if (Width < 50 || wordWrap || (scrollBars != ScrollBars.Both && scrollBars != ScrollBars.Horizontal)) horz.Visible = false;
				else if ((scrollBars == ScrollBars.Horizontal || scrollBars == ScrollBars.Both) && mode == TextBoxMode.Multiline && !wordWrap) horz.Visible = true;
			}

			AdjustMargins();

			if (vert != null) vert.PageSize = linesDrawn;
			if (horz != null) horz.PageSize = charsDrawn;
		}
		#endregion

		#region Adjust Margins
		/// <summary>
		/// Update the text box margins based on the visibility of the scroll bars.
		/// </summary>
		protected override void AdjustMargins()
		{
			// Horizontal scroll bar hidden?
			if (horz != null && !horz.Visible)
			{
				vert.Height = Height - 4;
				ClientMargins = new Margins(ClientMargins.Left, ClientMargins.Top, ClientMargins.Right, Skin.ClientMargins.Bottom);
			}

			// Scroll bar is visible.
			else
			{
				ClientMargins = new Margins(ClientMargins.Left, ClientMargins.Top, ClientMargins.Right, 18 + Skin.ClientMargins.Bottom);
			}

			// Vertical scroll bar hidden?
			if (vert != null && !vert.Visible)
			{
				horz.Width = Width - 4;
				ClientMargins = new Margins(ClientMargins.Left, ClientMargins.Top, Skin.ClientMargins.Right, ClientMargins.Bottom);
			}

			// Scroll bar is visible.
			else
			{
				ClientMargins = new Margins(ClientMargins.Left, ClientMargins.Top, 18 + Skin.ClientMargins.Right, ClientMargins.Bottom);
			}
			base.AdjustMargins();
		}
		#endregion

		#region On Resize Event Handler
		/// <summary>
		/// Handles resize events for the text box.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(ResizeEventArgs e)
		{
			// Clear text selection and update scroll bars.
			base.OnResize(e);
			selection.Clear();
			SetupBars();
		}
		#endregion

		#region Wrap Words
		/// <summary>
		/// Breaks up text content so that all lines fit within the width of the client area of the text box.
		/// </summary>
		/// <param name="text">Text content to word wrap.</param>
		/// <param name="size">Width of the text box the text will be wrapped in.</param>
		/// <returns>Returns the word wrapped string.</returns>
		private string WrapWords(string text, int size)
		{
			string ret = "";
			string line = "";

			// Split text at each space and break into a word array.
			string[] words = text.Replace("\v", "").Split(" ".ToCharArray());

			// Concatenate words until it has been reformed into lines that fit 
			// the width of the text box client area.
			for (int i = 0; i < words.Length; i++)
			{
				if (font.MeasureString(line + words[i]).X > size)
				{
					ret += line + "\v";
					line = words[i] + " ";
				}
				else
				{
					line += words[i] + " ";
				}
			}

			// Append last line.
			ret += line;

			// Remove last space and return the new formatted string.
			return ret.Remove(ret.Length - 1, 1);
		}
		#endregion

		#region Select All
		/// <summary>
		/// Selects all text in the text box.
		/// </summary>
		public virtual void SelectAll()
		{
			selection.Start = 0;
			selection.End = Text.Length;
		}
		#endregion

		#region Split Lines
		/// <summary>
		/// Splits the specified text into a list of strings based on the text box separator character.
		/// </summary>
		/// <param name="text">Text to split.</param>
		/// <returns>List of strings delimited by the text box separator character.</returns>
		private List<string> SplitLines(string text)
		{
			if (buffer != text)
			{
				buffer = text;
				List<string> list = new List<string>();
				string[] s = text.Split(new char[] { Separator[0] });
				list.Clear();

				list.AddRange(s);

				if (posy < 0) posy = 0;
				if (posy > list.Count - 1) posy = list.Count - 1;

				if (posx < 0) posx = 0;
				if (posx > list[PosY].Length) posx = list[PosY].Length;

				return list;
			}
			else return lines;
		}
		#endregion

		#region Scroll Bar Value Changed Event Handler
		/// <summary>
		/// Handles scroll events for the text box.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void sb_ValueChanged(object sender, EventArgs e)
		{
			ClientArea.Invalidate();
		}
		#endregion

		#region On Focus Lost Event Handler
		/// <summary>
		/// Handler for when the text box loses focus.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnFocusLost(EventArgs e)
		{
			// Clear the selection. 
			selection.Clear();
			ClientArea.Invalidate();
			base.OnFocusLost(e);
		}
		#endregion

		#region On Focus Gained Event Handler
		/// <summary>
		/// Handler for when the text box gains focus.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnFocusGained(EventArgs e)
		{
			// Auto-select all text?
			if (!readOnly && autoSelection)
			{
				SelectAll();
				ClientArea.Invalidate();
			}

			base.OnFocusGained(e);
		}
		#endregion
	}
}