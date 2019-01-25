// Copyright (c) 2007 by Larry Smith

// TODO: 
//	*	Big problem. I think what we need to pass in is a KeyValuePair<> to some kind 
//		of Get (or better yet maybe, an indexer) routine.
//	*	No Remove method
//	*	No IEnumerable<T> support
//	*	Test that classes can derive from it as expected

using System;
using System.Collections.Generic;
using System.Text;

namespace LRSClasses {
	class Cache<TKey, TValue> {
		protected List<KeyValuePair<TKey, TValue>>		items;
		uint					MaxSize;
		uint					CurSize;

		// A cache, in general terms, has
		//	a)	Contents
		//	b)	A maximum size (perhaps in terms of number of elements, or in 
		//		total element size)
		//			i)  For this class, for now, we'll just use a counter, rather than 
		//				a size
		//	c)	A replacement algorithm (defaulting to simple LRU)
		//	d)	Ways to add and retrieve data from the cache.
		//	e)	A way to remove elements from the cache (seldom used, but nice to have)
		//	f)	A way to resize the cache
		//	g)	A way to empty the cache
		//	h)	A way to iterate through the items (just on general principles)

//---------------------------------------------------------------------------------------

		public Cache(uint MaxSize) {
			_Empty(); 
			this.MaxSize = MaxSize;
		}

//---------------------------------------------------------------------------------------

		public void Add(KeyValuePair<TKey, TValue> item) {
			if (CurSize == MaxSize) {
				DropItem(0);
			}
			AddItem(item);
		}

//---------------------------------------------------------------------------------------

		protected void AddItem(KeyValuePair<TKey, TValue> item) {
			// We get here only when we know that adding this element won't overflow
			// the cache, so we don't have to do any checking.
			items.Add(item);
			++CurSize;
		}

//---------------------------------------------------------------------------------------

		protected void DropItem(int index) {
			if (index < 0 || index >= items.Count) {
				string	msg = string.Format("Invalid parameter passed - {0}", index);
				throw new ArgumentException(msg, "index");
			}
			items.RemoveAt(index);
		}

//---------------------------------------------------------------------------------------

		public void Empty() {
			// Naively, we just set "items" to null. But arguably, we also might want to
			// Dispose of elements. Except that the cache would normally just contain 
			// references to items, and it isn't really our call whether we should 
			// Dispose of an item just because the user doesn't want it in the cache any
			// more. So some day we might add a bool parameter to this method to indicate
			// that we should call Dispose on the objects. But not today. Also, if the
			// user really needs to do this, he can derive from this class and override
			// this method.
			_Empty();
		}

//---------------------------------------------------------------------------------------

		protected void _Empty() {
			items = new List<KeyValuePair<TKey, TValue>>();
			CurSize = 0;
		}

//---------------------------------------------------------------------------------------

		public void Resize(uint NewSize) {
			if (NewSize < CurSize) {
				Empty();
			}
			MaxSize = NewSize;
		}
	}
}
