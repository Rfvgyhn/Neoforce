////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: EventedList.cs                               //
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
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using System.Collections.Generic;
#endregion

namespace TomShane.Neoforce.Controls
{
	/// <summary>
	/// Represents a collection that fires events when items are added/removed.
	/// </summary>
	/// <typeparam name="T">Specifies the type of objects in the collection.</typeparam>
	public class EventedList<T> : List<T>
	{
		#region Events
		/// <summary>
		/// Occurs when an item is added to the list.
		/// </summary>
		public event EventHandler ItemAdded;
		/// <summary>
		/// Occurs when an item is removed from the list.
		/// </summary>
		public event EventHandler ItemRemoved;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new evented list.
		/// </summary>
		public EventedList() : base() { }
		/// <summary>
		/// Creates a new evented list of the specified size.
		/// </summary>
		/// <param name="capacity">Initial capacity of the evented list.</param>
		public EventedList(int capacity) : base(capacity) { }
		/// <summary>
		/// Creates a new evented list from the specified source list.
		/// </summary>
		/// <param name="collection">Source collection used to populate the evented list.</param>
		public EventedList(IEnumerable<T> collection) : base(collection) { }
		#endregion

		#region Add
		/// <summary>
		/// Adds a new item to the collection.
		/// </summary>
		/// <param name="item">Item to add to the collection.</param>
		public new void Add(T item)
		{
			int c = this.Count;
			base.Add(item);
			if (ItemAdded != null && c != this.Count)
			{
				ItemAdded.Invoke(this, new EventArgs());
			}
		}
		#endregion

		#region Remove
		/// <summary>
		/// Removes the specified item from the collection.
		/// </summary>
		/// <param name="obj">Item to remove from the collection.</param>
		public new void Remove(T obj)
		{
			int c = this.Count;
			base.Remove(obj);
			if (ItemRemoved != null && c != this.Count)
			{
				ItemRemoved.Invoke(this, new EventArgs());
			}
		}
		#endregion

		#region Clear
		/// <summary>
		/// Removes all the items from the collection.
		/// </summary>
		public new void Clear()
		{
			int c = this.Count;
			base.Clear();
			if (ItemRemoved != null && c != this.Count)
			{
				ItemRemoved.Invoke(this, new EventArgs());
			}
		}
		#endregion

		#region Add Range
		/// <summary>
		/// Adds a collection of items to the collection.
		/// </summary>
		/// <param name="collection">Collection of items to add to the collection.</param>
		public new void AddRange(IEnumerable<T> collection)
		{
			int c = this.Count;
			base.AddRange(collection);
			if (ItemAdded != null && c != this.Count) ItemAdded.Invoke(this, new EventArgs());
		}
		#endregion

		#region Insert
		/// <summary>
		/// Inserts a new item into the collection at the specified index.
		/// </summary>
		/// <param name="index">Zero-based index defining the insertion position.</param>
		/// <param name="item">Item to be inserted into the collection.</param>
		public new void Insert(int index, T item)
		{
			int c = this.Count;
			base.Insert(index, item);
			if (ItemAdded != null && c != this.Count)
			{
				ItemAdded.Invoke(this, new EventArgs());
			}
		}
		#endregion

		#region Insert Range
		/// <summary>
		/// Inserts a collection of items into the collection at the specified position.
		/// </summary>
		/// <param name="index">Zero-based index where the collection will be inserted.</param>
		/// <param name="collection">Collection of items to add to the collection at the specified index.</param>
		public new void InsertRange(int index, IEnumerable<T> collection)
		{
			int c = this.Count;
			base.InsertRange(index, collection);
			if (ItemAdded != null && c != this.Count)
			{
				ItemAdded.Invoke(this, new EventArgs());
			}
		}
		#endregion

		#region Remove All
		/// <summary>
		/// Removes all items in the collection that match the specified condition.
		/// </summary>
		/// <param name="match">Predicate method used to evaluate collection items.</param>
		/// <returns>Returns the number of items removed from the collection.</returns>
		public new int RemoveAll(Predicate<T> match)
		{
			int c = this.Count;
			int ret = base.RemoveAll(match);
			if (ItemRemoved != null && c != this.Count)
			{
				ItemRemoved.Invoke(this, new EventArgs());
			}
			return ret;
		}
		#endregion

		#region Remove At
		/// <summary>
		/// Removes an item from the collection at the specified index.
		/// </summary>
		/// <param name="index">Zero-based index specifying which item to remove.</param>
		public new void RemoveAt(int index)
		{
			int c = this.Count;
			base.RemoveAt(index);
			if (ItemRemoved != null && c != this.Count)
			{
				ItemRemoved.Invoke(this, new EventArgs());
			}
		}
		#endregion

		#region Remove Range
		/// <summary>
		/// Removes a range of items from the collection beginning at the specified index.
		/// </summary>
		/// <param name="index">Zero-based index to begin removal.</param>
		/// <param name="count">Number of items to remove from the collection.</param>
		public new void RemoveRange(int index, int count)
		{
			int c = this.Count;
			base.RemoveRange(index, count);
			if (ItemRemoved != null && c != this.Count) ItemRemoved.Invoke(this, new EventArgs());
		}
		#endregion
	}
}
