using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace T4Test1 {
	class Program {
		static void Main(string[] args) {
			MyT4Class.HelloT4();
		}
	}

	public class foo : INotifyPropertyChanged {
		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
}
