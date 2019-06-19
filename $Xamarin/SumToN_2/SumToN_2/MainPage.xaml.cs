using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SumToN_2 {
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	[DesignTimeVisible(true)]
	public partial class MainPage : ContentPage {
		public MainPage() {
			InitializeComponent();
		}

		private void Button_Clicked(object sender, EventArgs e) {
			int n = int.Parse(N.Text);
			Desc.Text = $"Sum from 1 to {n} is {(n * (n + 1)) / 2}";
		}
	}
}
