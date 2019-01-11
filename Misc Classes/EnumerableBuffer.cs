// Copyright (c) 2007 - Bartizan Connects LLC

using System;
using System.Collections.Generic;
using System.Text;

namespace MiscClasses.com.Bartizan {
	public class EnumerableBuffer<T> : IEnumerable<T> {

		public delegate List<T> FillBuffer();

		List<T>		buffer;
		FillBuffer	BufferFiller;

//---------------------------------------------------------------------------------------

		public EnumerableBuffer(FillBuffer BufferFiller) {
			buffer = null;
			this.BufferFiller = BufferFiller;
		}

//---------------------------------------------------------------------------------------

		#region IEnumerable<T> Members

		public IEnumerator<T> GetEnumerator() {
			while (true) {
				if (buffer == null) {
					buffer = BufferFiller();
					if (buffer == null) {		// Is it still null?
						yield break;
					}
				}
				foreach (T element in buffer) {
					yield return element;
				}
				buffer = null;					// Indicate we need more data
			}
		}

		#endregion


		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			throw new Exception("EnumerableBuffer<T>::GetEnumerator is not implemented.");
		}

		#endregion
	}
}
