////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: Component.cs                                 //
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
using Microsoft.Xna.Framework;
#endregion

namespace TomShane.Neoforce.Controls
{
	/// <summary>
	/// ???
	/// </summary>
	public class Component : Disposable
	{
		#region Fields
		/// <summary>
		/// GUI manager for the component.
		/// </summary>
		private Manager manager = null;
		/// <summary>
		/// Indicates if the component has been initialized or not.
		/// </summary>
		private bool initialized = false;
		#endregion

		#region Properties
		/// <summary>
		/// Gets the GUI manager for the component.
		/// </summary>
		public virtual Manager Manager 
		{ 
			get { return manager; } 
			set { manager = value; } 
		}
		
		/// <summary>
		/// Indicates if the component has been initialized or not.
		/// </summary>
		public virtual bool Initialized 
		{ 
			get { return initialized; } 
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Creates a new Component.
		/// </summary>
		/// <param name="manager">GUI manager for the component.</param>
		public Component(Manager manager)
		{
			if (manager != null)
			{
				this.manager = manager;
			}

			else
			{
				throw new Exception("Component cannot be created. Manager instance is needed.");
			}
		}
		#endregion

		#region Destructor
		/// <summary>
		/// Releases resources used by the component.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
			}
			base.Dispose(disposing);
		}
		#endregion

		#region Init
		/// <summary>
		/// Initializes the component.
		/// </summary>
		public virtual void Init()
		{
			initialized = true;
		}
		#endregion

		#region Update
		/// <summary>
		/// Updates the component.
		/// </summary>
		/// <param name="gameTime">Snapshot of the application's timing values.</param>
		protected internal virtual void Update(GameTime gameTime)
		{
		}
		#endregion
	}
}
