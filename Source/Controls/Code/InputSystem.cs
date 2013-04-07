////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: InputSystem.cs                               //
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
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
#endregion

namespace TomShane.Neoforce.Controls
{
	#region Enums
	/// <summary>
	/// Defines the input devices a Neoforce Application supports.
	/// </summary>
	[Flags]
	public enum InputMethods
	{
		None = 0x00,
		Keyboard = 0x01,
		Mouse = 0x02,
		GamePad = 0x04,
		All = Keyboard | Mouse | 0x04
	}
	
	/// <summary>
	/// Identifies a particular button on a mouse.
	/// </summary>
	public enum MouseButton
	{
		None = 0,
		Left,
		Right,
		Middle,
		XButton1,
		XButton2
	}
	
	/// <summary>
	/// Identifies a particular button on an Xbox 360 gamepad.
	/// </summary>
	public enum GamePadButton
	{
		None = 0,
		Start = 6,
		Back,
		Up,
		Down,
		Left,
		Right,
		A,
		B,
		X,
		Y,
		BigButton,
		LeftShoulder,
		RightShoulder,
		LeftTrigger,
		RightTrigger,
		LeftStick,
		RightStick,
		LeftStickLeft,
		LeftStickRight,
		LeftStickUp,
		LeftStickDown,
		RightStickLeft,
		RightStickRight,
		RightStickUp,
		RightStickDown
	}

	/// <summary>
	/// Identifies the index of the player who has input focus.
	/// </summary>
	public enum ActivePlayer
	{
		None = -1,
		One = 0,
		Two = 1,
		Three = 2,
		Four = 3
	}
	#endregion

	#region Structs
	/// <summary>
	/// Stores the thumb stick and trigger values of a gamepad.
	/// </summary>
	public struct GamePadVectors
	{
		#region Fields
		/// <summary>
		/// Left thumb stick axis values.
		/// </summary>
		public Vector2 LeftStick;
		/// <summary>
		/// Right thumb stick axis values.
		/// </summary>
		public Vector2 RightStick;
		/// <summary>
		/// Left trigger value.
		/// </summary>
		public float LeftTrigger;
		/// <summary>
		/// Right trigger value.
		/// </summary>
		public float RightTrigger;
		#endregion
	}
	
	/// <summary>
	/// Defines the input offset and ratio to use when rescaling controls in the render target.
	/// </summary>
	public struct InputOffset
	{
		#region Fields
		/// <summary>
		/// Mouse position X offset.
		/// </summary>
		public int X;
		/// <summary>
		/// Mouse position Y offset.
		/// </summary>
		public int Y;
		/// <summary>
		/// Target Width / Actual Screen Width.
		/// </summary>
		public float RatioX;
		/// <summary>
		/// Target Height / Actual Screen Height.
		/// </summary>
		public float RatioY;
		#endregion

		#region Constructor
		/// <summary>
		/// Creates a new instance of the InputOffset class.
		/// </summary>
		/// <param name="x">Mouse position X offset.</param>
		/// <param name="y">Mouse position Y offset.</param>
		/// <param name="rx">X ratio.</param>
		/// <param name="ry">Y ratio.</param>
		public InputOffset(int x, int y, float rx, float ry)
		{
			X = x;
			Y = y;
			RatioX = rx;
			RatioY = ry;
		}
		#endregion
	}
	#endregion

	#region Classes
	/// <summary>
	/// Defines the input system for a Neoforce application.
	/// </summary>
	public class InputSystem : Disposable
	{
		#region Classes
		/// <summary>
		/// Represents a key, its state, and repeat delay timer.
		/// </summary>
		private class InputKey
		{
			#region Fields
			/// <summary>
			/// Key that this object represents.
			/// </summary>
			public Keys Key = Keys.None;
			/// <summary>
			/// Indicates if the key is pressed or released.
			/// </summary>
			public bool Pressed = false;
			/// <summary>
			/// Timer used to delay firing of repeat key presses.
			/// </summary>
			public double Countdown = RepeatDelay;
			#endregion
		}

		/// <summary>
		/// Represents a mouse button, its state, and repeat delay timer.
		/// </summary>
		private class InputMouseButton
		{
			#region Fields
			/// <summary>
			/// The mouse button this object represents.
			/// </summary>
			public MouseButton Button = MouseButton.None;
			/// <summary>
			/// Indicates if the button is pressed or released.
			/// </summary>
			public bool Pressed = false;
			/// <summary>
			/// Timer to delay the firing of repeat click events.
			/// </summary>
			public double Countdown = RepeatDelay;
			#endregion

			#region Constructors
			/// <summary>
			/// Creates a default instance of the InputMouseButton class.
			/// </summary>
			public InputMouseButton()
			{
			}

			/// <summary>
			/// Creates an instance of the InputMouseButton class set for the specified mouse button.
			/// </summary>
			/// <param name="button">Mouse button this instance will represent.</param>
			public InputMouseButton(MouseButton button)
			{
				Button = button;
			}
			#endregion
		}

		/// <summary>
		/// Represents the state of the mouse device and the current mouse cursor position.
		/// </summary>
		private class InputMouse
		{
			#region Fields
			/// <summary>
			/// Current mouse state.
			/// </summary>
			public MouseState State = new MouseState();
			/// <summary>
			/// Current mouse cursor position.
			/// </summary>
			public Point Position = new Point(0, 0);
			#endregion
		}

		/// <summary>
		/// Represents a gamepad button, its state, and repeat delay timer.
		/// </summary>
		private class InputGamePadButton
		{
			#region Fields
			/// <summary>
			/// Gamepad button this object represents.
			/// </summary>
			public GamePadButton Button = GamePadButton.None;
			/// <summary>
			/// Indicates if the button is pressed or released.
			/// </summary>
			public bool Pressed = false;
			/// <summary>
			/// Delay timer for firing repeat button presses.
			/// </summary>
			public double Countdown = RepeatDelay;
			#endregion

			#region Constructors
			/// <summary>
			/// Creates a default instance of the InputGamePadButton class.
			/// </summary>
			public InputGamePadButton()
			{
			}

			/// <summary>
			/// Creates an InputGamePadButton instance for the specified button.
			/// </summary>
			/// <param name="button">Button this instance will represent.</param>
			public InputGamePadButton(GamePadButton button)
			{
				Button = button;
			}
			#endregion
		}
		#endregion

		#region Constants
		/// <summary>
		/// Indicates how much delay (in ms) there will be before a key/button
		/// will register a repeated press event when held down.
		/// </summary>
		private const int RepeatDelay = 500;
		/// <summary>
		/// Indicates how much delay (in ms) there will be between repeated key/button
		/// press events after the initial RepeatDelay timer has expired. 
		/// </summary>
		private const int RepeatRate = 50;
		/// <summary>
		/// Indicates how far a thumbstick must be moved from center position to register 
		/// an action. 
		/// </summary>
		private float ClickThreshold = 0.5f;
		#endregion

		#region Fields
#if (!XBOX && !XBOX_FAKE)
		/// <summary>
		/// The focused form of the application when running on Windows.
		/// </summary>
		private System.Windows.Forms.Form window = null;
#endif
		/// <summary>
		/// List to track the state and repeat timers of all keyboard keys.
		/// </summary>
		private List<InputKey> keys = new List<InputKey>();
		/// <summary>
		/// List to track the state and repeat timers of all mouse buttons.
		/// </summary>
		private List<InputMouseButton> mouseButtons = new List<InputMouseButton>();
		/// <summary>
		/// List to track the state and repeat timers of all gamepad buttons.
		/// </summary>
		private List<InputGamePadButton> gamePadButtons = new List<InputGamePadButton>();
		/// <summary>
		/// Current mouse state.
		/// </summary>
		private MouseState mouseState = new MouseState();
		/// <summary>
		/// Current gamepad state.
		/// </summary>
		private GamePadState gamePadState = new GamePadState();
		/// <summary>
		/// Application's GUI manager.
		/// </summary>
		private Manager manager = null;
		/// <summary>
		/// Input offset and ratio to use when rescaling controls in the render target.
		/// </summary>
		private InputOffset inputOffset = new InputOffset(0, 0, 1.0f, 1.0f);
		/// <summary>
		/// Specifies what input devices can be used to navigate the application's controls.
		/// </summary>
		private InputMethods inputMethods = InputMethods.All;
		/// <summary>
		/// Index of the player with input focus over the application.
		/// </summary>
		private ActivePlayer activePlayer = ActivePlayer.None;
		#endregion

		#region Properties
		/// <summary>
		/// Sets or gets input offset and ratio when rescaling controls in render target.
		/// </summary>
		public virtual InputOffset InputOffset
		{
			get { return inputOffset; }
			set { inputOffset = value; }
		}

		/// <summary>
		/// Sets or gets input methods allowed for navigation.
		/// </summary>
		public virtual InputMethods InputMethods
		{
			get { return inputMethods; }
			set { inputMethods = value; }
		}

		/// <summary>
		/// Gets or sets the index of the player who has input focus.
		/// </summary>
		public virtual ActivePlayer ActivePlayer
		{
			get { return activePlayer; }
			set { activePlayer = value; }
		}
		#endregion

		#region Events
		/// <summary>
		/// Occurs when a key enters the pressed state.
		/// </summary>
		public event KeyEventHandler KeyDown;
		/// <summary>
		/// Occurs after a key down event and also occurs for repeat key press events.
		/// </summary>
		public event KeyEventHandler KeyPress;
		/// <summary>
		/// Occurs when a key leaves the pressed state.
		/// </summary>
		public event KeyEventHandler KeyUp;
		/// <summary>
		/// Occurs when a mouse button enters the pressed state.
		/// </summary>
		public event MouseEventHandler MouseDown;
		/// <summary>
		/// Occurs after a mouse down event and occurs for repeat mouse press events. 
		/// </summary>
		public event MouseEventHandler MousePress;
		/// <summary>
		/// Occurs when a mouse button leaves the pressed state.
		/// </summary>
		public event MouseEventHandler MouseUp;
		/// <summary>
		/// Occurs when the mouse is moved.
		/// </summary>
		public event MouseEventHandler MouseMove;
		/// <summary>
		/// Occurs when a gamepad button leaves the pressed state.
		/// </summary>
		public event GamePadEventHandler GamePadUp;
		/// <summary>
		/// Occurs when a gamepad button enters the pressed state.
		/// </summary>
		public event GamePadEventHandler GamePadDown;
		/// <summary>
		/// Occurs after a gamepad down event and occurs for repeat button press events.
		/// </summary>
		public event GamePadEventHandler GamePadPress;
		/// <summary>
		/// Occurs when the values of the gamepad thumb sticks change.
		/// </summary>
		public event GamePadEventHandler GamePadMove;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates the Input System.
		/// </summary>
		/// <param name="manager">Application's GUI manager.</param>
		/// <param name="offset">???</param>
		public InputSystem(Manager manager, InputOffset offset)
		{
			this.inputOffset = offset;
			this.manager = manager;
#if (!XBOX && !XBOX_FAKE)
			window = manager.Window;
#endif
		}
		
		/// <summary>
		/// Creates the Input System
		/// </summary>
		/// <param name="manager">Application's GUI manager.</param>
		public InputSystem(Manager manager)
			: this(manager, new InputOffset(0, 0, 1.0f, 1.0f))
		{
		}
		#endregion

		#region Initialize
		/// <summary>
		/// Initializes the input system.
		/// </summary>
		public virtual void Initialize()
		{
			keys.Clear();
			mouseButtons.Clear();
			gamePadButtons.Clear();

#if (!XBOX && !XBOX_FAKE)
			// Initialize the keys list.
			foreach (string str in Enum.GetNames(typeof(Keys)))
			{
				InputKey key = new InputKey();
				key.Key = (Keys)Enum.Parse(typeof(Keys), str);
				keys.Add(key);
			}

			// Initialize the mouse buttons list.
			foreach (string str in Enum.GetNames(typeof(MouseButton)))
			{
				InputMouseButton btn = new InputMouseButton();
				btn.Button = (MouseButton)Enum.Parse(typeof(MouseButton), str);
				mouseButtons.Add(btn);
			}

			// Initialize the gamepad buttons list.
			foreach (string str in Enum.GetNames(typeof(GamePadButton)))
			{
				InputGamePadButton btn = new InputGamePadButton();
				btn.Button = (GamePadButton)Enum.Parse(typeof(GamePadButton), str);
				gamePadButtons.Add(btn);
			}
#else       
      		// Initialize the gamepad buttons list.
			gamePadButtons.Add(new InputGamePadButton(GamePadButton.None));
			gamePadButtons.Add(new InputGamePadButton(GamePadButton.Start));
			gamePadButtons.Add(new InputGamePadButton(GamePadButton.Back));
			gamePadButtons.Add(new InputGamePadButton(GamePadButton.Up));        
			gamePadButtons.Add(new InputGamePadButton(GamePadButton.Down));
			gamePadButtons.Add(new InputGamePadButton(GamePadButton.Left));
			gamePadButtons.Add(new InputGamePadButton(GamePadButton.Right));       
			gamePadButtons.Add(new InputGamePadButton(GamePadButton.A));
			gamePadButtons.Add(new InputGamePadButton(GamePadButton.B));
			gamePadButtons.Add(new InputGamePadButton(GamePadButton.X));
			gamePadButtons.Add(new InputGamePadButton(GamePadButton.Y));         
			gamePadButtons.Add(new InputGamePadButton(GamePadButton.BigButton)); 
			gamePadButtons.Add(new InputGamePadButton(GamePadButton.LeftShoulder)); 
			gamePadButtons.Add(new InputGamePadButton(GamePadButton.RightShoulder)); 
			gamePadButtons.Add(new InputGamePadButton(GamePadButton.LeftTrigger)); 
			gamePadButtons.Add(new InputGamePadButton(GamePadButton.RightTrigger));         
			gamePadButtons.Add(new InputGamePadButton(GamePadButton.LeftStick)); 
			gamePadButtons.Add(new InputGamePadButton(GamePadButton.RightStick));         
			gamePadButtons.Add(new InputGamePadButton(GamePadButton.LeftStickLeft));         
			gamePadButtons.Add(new InputGamePadButton(GamePadButton.LeftStickRight));                 
			gamePadButtons.Add(new InputGamePadButton(GamePadButton.LeftStickUp));
			gamePadButtons.Add(new InputGamePadButton(GamePadButton.LeftStickDown));                         
			gamePadButtons.Add(new InputGamePadButton(GamePadButton.RightStickLeft));                 
			gamePadButtons.Add(new InputGamePadButton(GamePadButton.RightStickRight));                 
			gamePadButtons.Add(new InputGamePadButton(GamePadButton.RightStickUp));                 
			gamePadButtons.Add(new InputGamePadButton(GamePadButton.RightStickDown));                 
#endif
		}
		#endregion

		#region Send Mouse State
		/// <summary>
		/// Updates the current mouse state using the supplied arguments.
		/// </summary>
		/// <param name="state">Mouse state to use to update the current mouse state.</param>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		public virtual void SendMouseState(MouseState state, GameTime gameTime)
		{
			UpdateMouse(state, gameTime);
		}
		#endregion

		#region Send Keyboard State
		/// <summary>
		/// Updates the current keyboard state with the supplied arguments.
		/// </summary>
		/// <param name="state">Keyboard state used to update the current state of the keyboard.</param>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		public virtual void SendKeyboardState(KeyboardState state, GameTime gameTime)
		{
			UpdateKeys(state, gameTime);
		}
		#endregion

		#region Send Game Pad State
		/// <summary>
		/// Updates the state of the specified gamepad using the supplied arguments.
		/// </summary>
		/// <param name="playerIndex">PlayerIndex of the gamepad to update.</param>
		/// <param name="state">Gamepad state to use to update the current gamepad state values.</param>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		public virtual void SendGamePadState(PlayerIndex playerIndex, GamePadState state, GameTime gameTime)
		{
			UpdateGamePad(playerIndex, state, gameTime);
		}
		#endregion

		#region Update
		/// <summary>
		/// Updates the state of supported input devices.
		/// </summary>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		public virtual void Update(GameTime gameTime)
		{
			// Do nothing if the guide can be used and the guide is displayed.
			if (manager.UseGuide && Guide.IsVisible)
			{
				return;
			}

#if (!XBOX && !XBOX_FAKE)
			// Mouse and keyboard support on Windows only.
			MouseState ms = Mouse.GetState();
			KeyboardState ks = Keyboard.GetState(PlayerIndex.One);
#endif


#if (!XBOX && !XBOX_FAKE)
			if (window.Focused)
#endif
			{
#if (!XBOX && !XBOX_FAKE)
				// Update the mouse and keyboard device states on Windows, if the application uses them.
				if ((inputMethods & InputMethods.Mouse) == InputMethods.Mouse) UpdateMouse(ms, gameTime);
				if ((inputMethods & InputMethods.Keyboard) == InputMethods.Keyboard) UpdateKeys(ks, gameTime);
#endif
				// Update the state of the active gamepad, if the application supports it.
				if ((inputMethods & InputMethods.GamePad) == InputMethods.GamePad)
				{
					PlayerIndex index = PlayerIndex.One;

					// If gamers are signed in and an active player is not defined, 
					// use the index of the first player in the SignedInGamers list.
					if (Gamer.SignedInGamers.Count > 0 && activePlayer == ActivePlayer.None)
					{
						int i = 0; // Have to be done this way, else it crashes for player other than one
						index = Gamer.SignedInGamers[i].PlayerIndex;
					}
				
					// If an active player is specified, use that player index to update.
					else if (activePlayer != ActivePlayer.None)
					{
						index = (PlayerIndex)activePlayer;
					}

					GamePadState gs = GamePad.GetState(index);
					UpdateGamePad(index, gs, gameTime);
				}
			}
		}
		#endregion

		#region Get Vector State
		/// <summary>
		/// Checks the specified thumbstick or trigger button and returns its ButtonState.
		/// </summary>
		/// <param name="button">Left/Right thumbstick direction or trigger button to get the state of.</param>
		/// <param name="state">Gamepad state to check the specified button on.</param>
		/// <returns>Returns ButtonState.Pressed if the value of the specified button exceeds the ClickThreshold value;
		/// otherwise it returns ButtonState.Released.</returns>
		private ButtonState GetVectorState(GamePadButton button, GamePadState state)
		{
			ButtonState ret = ButtonState.Released;
			bool down = false;
			float t = ClickThreshold;

			switch (button)
			{
				case GamePadButton.LeftStickLeft: down = state.ThumbSticks.Left.X < -t; break;
				case GamePadButton.LeftStickRight: down = state.ThumbSticks.Left.X > t; break;
				case GamePadButton.LeftStickUp: down = state.ThumbSticks.Left.Y > t; break;
				case GamePadButton.LeftStickDown: down = state.ThumbSticks.Left.Y < -t; break;

				case GamePadButton.RightStickLeft: down = state.ThumbSticks.Right.X < -t; break;
				case GamePadButton.RightStickRight: down = state.ThumbSticks.Right.X > t; break;
				case GamePadButton.RightStickUp: down = state.ThumbSticks.Right.Y > t; break;
				case GamePadButton.RightStickDown: down = state.ThumbSticks.Right.Y < -t; break;

				case GamePadButton.LeftTrigger: down = state.Triggers.Left > t; break;
				case GamePadButton.RightTrigger: down = state.Triggers.Right > t; break;
			}

			ret = down ? ButtonState.Pressed : ButtonState.Released;

			return ret;
		}
		#endregion

		#region Update Game Pad
		/// <summary>
		/// Updates the state of the gamepad with the specified player index and raises 
		/// applicable gamepad events as needed.
		/// </summary>
		/// <param name="playerIndex">PlayerIndex of the gamepad to update the state of.</param>
		/// <param name="state">The new state to assign to the gamepad.</param>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		private void UpdateGamePad(PlayerIndex playerIndex, GamePadState state, GameTime gameTime)
		{
			GamePadEventArgs e = new GamePadEventArgs(playerIndex);

			// Check for changes in stick and trigger values.
			if (state.ThumbSticks.Left != gamePadState.ThumbSticks.Left ||
				state.ThumbSticks.Right != gamePadState.ThumbSticks.Right ||
				state.Triggers.Left != gamePadState.Triggers.Left ||
				state.Triggers.Right != gamePadState.Triggers.Right)
			{
				BuildGamePadEvent(state, GamePadButton.None, ref e);
				if (GamePadMove != null) GamePadMove.Invoke(this, e);
			}

			// Update the states of the gamepad buttons in the list.
			foreach (InputGamePadButton btn in gamePadButtons)
			{
				// Current button state of the current gamepad button.
				ButtonState bs = ButtonState.Released;

				if (btn.Button == GamePadButton.None) continue;
				else if (btn.Button == GamePadButton.A) bs = state.Buttons.A;
				else if (btn.Button == GamePadButton.B) bs = state.Buttons.B;
				else if (btn.Button == GamePadButton.Back) bs = state.Buttons.Back;
				else if (btn.Button == GamePadButton.Down) bs = state.DPad.Down;
				else if (btn.Button == GamePadButton.Left) bs = state.DPad.Left;
				else if (btn.Button == GamePadButton.Right) bs = state.DPad.Right;
				else if (btn.Button == GamePadButton.Start) bs = state.Buttons.Start;
				else if (btn.Button == GamePadButton.Up) bs = state.DPad.Up;
				else if (btn.Button == GamePadButton.X) bs = state.Buttons.X;
				else if (btn.Button == GamePadButton.Y) bs = state.Buttons.Y;
				else if (btn.Button == GamePadButton.BigButton) bs = state.Buttons.BigButton;
				else if (btn.Button == GamePadButton.LeftShoulder) bs = state.Buttons.LeftShoulder;
				else if (btn.Button == GamePadButton.RightShoulder) bs = state.Buttons.RightShoulder;
				else if (btn.Button == GamePadButton.LeftStick) bs = state.Buttons.LeftStick;
				else if (btn.Button == GamePadButton.RightStick) bs = state.Buttons.RightStick;
				else bs = GetVectorState(btn.Button, state);

				// Current state button pressed?
				bool pressed = (bs == ButtonState.Pressed);

				if (pressed)
				{
					// Update the repeat delay timer for the associated button.
					double ms = gameTime.ElapsedGameTime.TotalMilliseconds;
					if (pressed) btn.Countdown -= ms;
				}

				// Button was just pressed?
				if ((pressed) && (!btn.Pressed))
				{
					// "Press" the associated button.
					btn.Pressed = true;
					BuildGamePadEvent(state, btn.Button, ref e);

					// Fire the GamePadDown event and the initial GamePadPress event.
					if (GamePadDown != null) GamePadDown.Invoke(this, e);
					if (GamePadPress != null) GamePadPress.Invoke(this, e);
				}

				// Button was just released?
				else if ((!pressed) && (btn.Pressed))
				{
					// "Release" the associated button and reset the repeat delay timer.
					btn.Pressed = false;
					btn.Countdown = RepeatDelay;
					BuildGamePadEvent(state, btn.Button, ref e);

					// Fire the GamePadUp event.
					if (GamePadUp != null) GamePadUp.Invoke(this, e);
				}

				// Button is held down and it's time to fire a repeat press event?
				else if (btn.Pressed && btn.Countdown < 0)
				{
					// Update event args and reset timer.
					e.Button = btn.Button;
					btn.Countdown = RepeatRate;
					BuildGamePadEvent(state, btn.Button, ref e);

					// Fire the repeated GamePadPress event.
					if (GamePadPress != null) GamePadPress.Invoke(this, e);
				}
			}
			gamePadState = state;
		}
		#endregion

		#region Build Game Pad Event
		/// <summary>
		/// Updates the GamePadEventArgs with the specified button, stick and trigger values.
		/// </summary>
		/// <param name="state">Current gamepad state to grab stick and trigger values from.</param>
		/// <param name="button">Gamepad button to assign to the event args.</param>
		/// <param name="e">GamePadEventArgs to update with the supplied values.</param>
		private void BuildGamePadEvent(GamePadState state, GamePadButton button, ref GamePadEventArgs e)
		{
			e.State = state;
			e.Button = button;
			e.Vectors.LeftStick = new Vector2(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y);
			e.Vectors.RightStick = new Vector2(state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y);
			e.Vectors.LeftTrigger = state.Triggers.Left;
			e.Vectors.RightTrigger = state.Triggers.Right;
		}
		#endregion

		#region Update Keys
		/// <summary>
		/// Updates the state of the keys in the list.
		/// </summary>
		/// <param name="state">Current keyboard state.</param>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		private void UpdateKeys(KeyboardState state, GameTime gameTime)
		{
#if (!XBOX && !XBOX_FAKE)
			KeyEventArgs e = new KeyEventArgs();

			// Is CapsLock on?
			e.Caps = (((ushort)NativeMethods.GetKeyState(0x14)) & 0xffff) != 0;

			// Check for modifier key presses.
			foreach (Keys key in state.GetPressedKeys())
			{
				if (key == Keys.LeftAlt || key == Keys.RightAlt) e.Alt = true;
				else if (key == Keys.LeftShift || key == Keys.RightShift) e.Shift = true;
				else if (key == Keys.LeftControl || key == Keys.RightControl) e.Control = true;
			}

			// Update the rest of the key states.
			foreach (InputKey key in keys)
			{
				// Ignore modifier keys, they're already handled.
				if (key.Key == Keys.LeftAlt || key.Key == Keys.RightAlt ||
					key.Key == Keys.LeftShift || key.Key == Keys.RightShift ||
					key.Key == Keys.LeftControl || key.Key == Keys.RightControl)
				{
					continue;
				}

				// Is the current key state key pressed?
				bool pressed = state.IsKeyDown(key.Key);

				// Update the repeat delay timer for this key.
				double ms = gameTime.ElapsedGameTime.TotalMilliseconds;
				if (pressed) key.Countdown -= ms;
				
				// Key was just pressed?
				if ((pressed) && (!key.Pressed))
				{
					// Update the state of the associated key.
					key.Pressed = true;
					e.Key = key.Key;

					// Fire the KeyDown and initial KeyPress events.
					if (KeyDown != null) KeyDown.Invoke(this, e);
					if (KeyPress != null) KeyPress.Invoke(this, e);
				}

				// Key was just released?
				else if ((!pressed) && (key.Pressed))
				{
					// Update the state of the associated key and reset the repeat delay timer.
					key.Pressed = false;
					key.Countdown = RepeatDelay;
					e.Key = key.Key;

					// Fire the KeyUp event.
					if (KeyUp != null) KeyUp.Invoke(this, e);
				}

				// Key is held down and it's time to fire an additional KeyPress event?
				else if (key.Pressed && key.Countdown < 0)
				{
					// Reset the repeat delay timer.
					key.Countdown = RepeatRate;
					e.Key = key.Key;

					// Fire the KeyPress event again.
					if (KeyPress != null) KeyPress.Invoke(this, e);
				}
			}
#endif
		}
		#endregion

		#region Recalc Position
		/// <summary>
		/// Adjusts the mouse position to account for rescaling on the render target.
		/// </summary>
		/// <param name="pos">Original mouse position.</param>
		/// <returns>Returns the adjusted mouse position.</returns>
		private Point RecalcPosition(Point pos)
		{
			return new Point((int)((pos.X - InputOffset.X) / InputOffset.RatioX), (int)((pos.Y - InputOffset.Y) / InputOffset.RatioY));
		}
		#endregion

		#region Adjust Position
		/// <summary>
		/// Adjusts the position of the mouse cursor to keep it within the client region of the window.
		/// </summary>
		/// <param name="e">Mouse event arguments.</param>
		private void AdjustPosition(ref MouseEventArgs e)
		{
			Rectangle screen = manager.Game.Window.ClientBounds;

			if (e.Position.X < 0) e.Position.X = 0;
			if (e.Position.Y < 0) e.Position.Y = 0;
			if (e.Position.X >= screen.Width) e.Position.X = screen.Width - 1;
			if (e.Position.Y >= screen.Height) e.Position.Y = screen.Height - 1;
		}
		#endregion

		#region Build Mouse Event
		/// <summary>
		/// Updates the MouseEventArgs with the specified button and mouse state information.
		/// </summary>
		/// <param name="state">Current mouse state to grab values from.</param>
		/// <param name="button">Mouse button to assign to the event args.</param>
		/// <param name="e">MouseEventArgs to update with the supplied values.</param>
		private void BuildMouseEvent(MouseState state, MouseButton button, ref MouseEventArgs e)
		{
			e.State = state;
			e.Button = button;

			e.Position = new Point(state.X, state.Y);
			AdjustPosition(ref e);

			e.Position = RecalcPosition(e.Position);
			e.State = new MouseState(e.Position.X, e.Position.Y, e.State.ScrollWheelValue, e.State.LeftButton, e.State.MiddleButton, e.State.RightButton, e.State.XButton1, e.State.XButton2);

			Point pos = RecalcPosition(new Point(mouseState.X, mouseState.Y));
			e.Difference = new Point(e.Position.X - pos.X, e.Position.Y - pos.Y);
		}
		#endregion

		#region Update Mouse
		/// <summary>
		/// Updates the mouse device.
		/// </summary>
		/// <param name="state">Current mouse state.</param>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		private void UpdateMouse(MouseState state, GameTime gameTime)
		{
#if (!XBOX && !XBOX_FAKE)
			// Mouse position changed?
			if ((state.X != mouseState.X) || (state.Y != mouseState.Y))
			{
				MouseEventArgs e = new MouseEventArgs();

				// Figure out which button to set in the mouse event args object.
				MouseButton btn = MouseButton.None;
				if (state.LeftButton == ButtonState.Pressed) btn = MouseButton.Left;
				else if (state.RightButton == ButtonState.Pressed) btn = MouseButton.Right;
				else if (state.MiddleButton == ButtonState.Pressed) btn = MouseButton.Middle;
				else if (state.XButton1 == ButtonState.Pressed) btn = MouseButton.XButton1;
				else if (state.XButton2 == ButtonState.Pressed) btn = MouseButton.XButton2;

				BuildMouseEvent(state, btn, ref e);
				
				// Fire the MouseMove event.
				if (MouseMove != null)
				{
					MouseMove.Invoke(this, e);
				}
			}

			// Update the mouse button states.
			UpdateButtons(state, gameTime);
			mouseState = state;
#endif
		}
		#endregion

		#region Update Buttons (Mouse)
		/// <summary>
		/// Updates the state of the Mouse buttons.
		/// </summary>
		/// <param name="state">Current state of the mouse device.</param>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		private void UpdateButtons(MouseState state, GameTime gameTime)
		{
#if (!XBOX && !XBOX_FAKE)
			MouseEventArgs e = new MouseEventArgs();

			// Update the state of the buttons in the mouse button list.
			foreach (InputMouseButton btn in mouseButtons)
			{
				ButtonState bs = ButtonState.Released;
				
				if (btn.Button == MouseButton.Left) bs = state.LeftButton;
				else if (btn.Button == MouseButton.Right) bs = state.RightButton;
				else if (btn.Button == MouseButton.Middle) bs = state.MiddleButton;
				else if (btn.Button == MouseButton.XButton1) bs = state.XButton1;
				else if (btn.Button == MouseButton.XButton2) bs = state.XButton2;
				else continue;

				// Update the repeat delay timer if the button is pressed.
				bool pressed = (bs == ButtonState.Pressed);
				if (pressed)
				{
					double ms = gameTime.ElapsedGameTime.TotalMilliseconds;
					if (pressed) btn.Countdown -= ms;
				}

				// Mouse button just pressed down?
				if ((pressed) && (!btn.Pressed))
				{
					// Update the state of the associated button.
					btn.Pressed = true;
					BuildMouseEvent(state, btn.Button, ref e);

					// Fire the MouseDown and MousePress events.
					if (MouseDown != null) MouseDown.Invoke(this, e);
					if (MousePress != null) MousePress.Invoke(this, e);
				}

				// Mouse button just released?
				else if ((!pressed) && (btn.Pressed))
				{
					// Update the state of the associated button and reset its repeat delay timer
					btn.Pressed = false;
					btn.Countdown = RepeatDelay;
					BuildMouseEvent(state, btn.Button, ref e);

					// Fire the MouseUp event.
					if (MouseUp != null) MouseUp.Invoke(this, e);
				}

				// Time to fire a repeat press event?
				else if (btn.Pressed && btn.Countdown < 0)
				{
					// Update event args and reset the repeat delay timer.
					e.Button = btn.Button;
					btn.Countdown = RepeatRate;
					BuildMouseEvent(state, btn.Button, ref e);

					// Fire the repeat MousePress event.
					if (MousePress != null) MousePress.Invoke(this, e);
				}
			}

#endif
		}
		#endregion
	}
	#endregion
}