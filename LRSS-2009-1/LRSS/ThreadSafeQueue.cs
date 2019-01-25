using System;
using System.Collections.Generic;
using System.Text;

namespace LRSS {
	public class ThreadSafeQueue<T> {
		private Queue<T> queue = new Queue<T>();

//---------------------------------------------------------------------------------------

		public int Count {
			get {
				lock (queue) {
					return queue.Count;
				}
			}
		}

//---------------------------------------------------------------------------------------

		public void Enqueue(T item) {
			lock (queue) {
				queue.Enqueue(item);
			}
		}

//---------------------------------------------------------------------------------------

		public T Dequeue() {
			lock (queue) {
				return queue.Dequeue();
			}
		}

//---------------------------------------------------------------------------------------

		public T Peek() {
			lock (queue) {
				return queue.Peek();
			}
		}
	}
}
