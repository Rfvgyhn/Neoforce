////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Central                                          //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: TaskEvents.cs                                //
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
using System.Collections.Generic;
using System.Text;
using TomShane.Neoforce.Controls;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Threading;
#endregion

namespace TomShane.Neoforce.Central
{
	/// <summary>
	/// Window designed to test mouse events.
	/// </summary>
	public class TaskEvents : Window
	{
		#region Fields
		/// <summary>
		/// 
		/// </summary>
		private Button btn;
		/// <summary>
		/// 
		/// </summary>
		private ListBox lst;
		/// <summary>
		/// 
		/// </summary>
		private ListBox txt;
		#endregion

		#region Constructor
		/// <summary>
		/// Creates a new TaskEvents window.
		/// </summary>
		/// <param name="manager">GUI manager for the window.</param>
		public TaskEvents(Manager manager)
			: base(manager)
		{
			Height = 360;
			MinimumHeight = 99;
			MinimumWidth = 78;
			Text = "Events Test";
			Center();

			btn = new Button(manager);
			btn.Init();
			btn.Parent = this;
			btn.Left = 20;
			btn.Top = 20;
			btn.MouseMove += new MouseEventHandler(btn_MouseMove);
			btn.MouseDown += new MouseEventHandler(btn_MouseDown);
			btn.MouseUp += new MouseEventHandler(btn_MouseUp);
			btn.MouseOver += new MouseEventHandler(btn_MouseOver);
			btn.MouseOut += new MouseEventHandler(btn_MouseOut);
			btn.MousePress += new MouseEventHandler(btn_MousePress);
			btn.Click += new EventHandler(btn_Click);

			lst = new ListBox(manager);
			lst.Init();
			lst.Parent = this;
			lst.Left = 20;
			lst.Top = 60;
			lst.Width = 128;
			lst.Height = 128;
			lst.MouseMove += new MouseEventHandler(btn_MouseMove);
			lst.MouseDown += new MouseEventHandler(btn_MouseDown);
			lst.MouseUp += new MouseEventHandler(btn_MouseUp);
			lst.MouseOver += new MouseEventHandler(btn_MouseOver);
			lst.MouseOut += new MouseEventHandler(btn_MouseOut);
			lst.MousePress += new MouseEventHandler(btn_MousePress);
			lst.Click += new EventHandler(btn_Click);

			txt = new ListBox(manager);
			txt.Init();
			txt.Parent = this;
			txt.Left = 200;
			txt.Top = 8;
			txt.Width = 160;
			txt.Height = 300;
		}
		#endregion

		#region Button Click Event Handler
		/// <summary>
		/// Handles button click events.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void btn_Click(object sender, EventArgs e)
		{
			MouseEventArgs ex = (e is MouseEventArgs) ? (MouseEventArgs)e : new MouseEventArgs();
			txt.Items.Add((sender == btn ? "Button" : "List") + ": Click " + ex.Button.ToString());
			txt.ItemIndex = txt.Items.Count - 1;
		}
		#endregion

		#region Mouse Click Event Handler
		/// <summary>
		/// Handles mouse button press events.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void btn_MousePress(object sender, MouseEventArgs e)
		{
			//  txt.Items.Add((sender == btn ? "Button" : "List") + ": Press");
			//  txt.ItemIndex = txt.Items.Count - 1;
		}
		#endregion

		#region Mouse Out Event Handler
		/// <summary>
		/// Handles mouse out events.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void btn_MouseOut(object sender, MouseEventArgs e)
		{
			txt.Items.Add((sender == btn ? "Button" : "List") + ": Out");
			txt.ItemIndex = txt.Items.Count - 1;
		}
		#endregion

		#region Mouse Over Event Handler
		/// <summary>
		/// Handles mouse over/hover events.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void btn_MouseOver(object sender, MouseEventArgs e)
		{
			txt.Items.Add((sender == btn ? "Button" : "List") + ": Over");
			txt.ItemIndex = txt.Items.Count - 1;
		}
		#endregion

		#region Mouse Up Event Handler
		/// <summary>
		/// Handles mouse button up events.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void btn_MouseUp(object sender, MouseEventArgs e)
		{
			txt.Items.Add((sender == btn ? "Button" : "List") + ": Up " + e.Button.ToString());
			txt.ItemIndex = txt.Items.Count - 1;
		}
		#endregion

		#region Mouse Down Event Handler
		/// <summary>
		/// Handles mouse button down events.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void btn_MouseDown(object sender, MouseEventArgs e)
		{
			txt.Items.Add((sender == btn ? "Button" : "List") + ": Down " + e.Button.ToString());
			txt.ItemIndex = txt.Items.Count - 1;
		}
		#endregion

		#region Mouse Move Event Handler
		/// <summary>
		/// Handles mouse move events.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void btn_MouseMove(object sender, MouseEventArgs e)
		{
			txt.Items.Add((sender == btn ? "Button" : "List") + ": Move");
			txt.ItemIndex = txt.Items.Count - 1;
		}
		#endregion

		#region Init
		/// <summary>
		/// Initializes the task events window.
		/// </summary>
		public override void Init()
		{
			base.Init();
		}
		#endregion
	}
}
