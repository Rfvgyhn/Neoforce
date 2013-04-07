////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: Disposable.cs                                //
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
#endregion

namespace TomShane.Neoforce.Controls
{
	/// <summary>
	/// Represents a reference counted resource.
	/// </summary>
	public abstract class Disposable : Unknown, IDisposable
	{
		#region Fields
		/// <summary>
		/// Number of references to this object.
		/// </summary>
		private static int count = 0;
		#endregion

		#region Properties
		/// <summary>
		/// Gets the number of active references this object has in memory.
		/// </summary>
		public static int Count 
		{ 
			get { return count; } 
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a reference counted object.
		/// </summary>
		protected Disposable()
		{
			count += 1;
		}
		#endregion

		#region Destructors
		/// <summary>
		/// Releases resources used by the object.
		/// </summary>
		~Disposable()
		{
			Dispose(false);
		}
		
		/// <summary>
		/// Cleans up after the disposable object.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		
		/// <summary>
		/// Decreases the object's reference count.
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				count -= 1;
			}
		}
		#endregion
	}
}
