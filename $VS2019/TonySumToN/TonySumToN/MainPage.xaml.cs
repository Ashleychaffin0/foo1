using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TonySumToN {
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	[DesignTimeVisible(true)]
	public partial class MainPage : ContentPage {
		public MainPage() {
			InitializeComponent();
		}

		private void Button_Clicked(object sender, EventArgs e) {
			int n = int.Parse(nValue.Text);
			int sum = 0;
			for (int i = 0; i < n; i++) {
				sum += i;
			}
			Msg.Text = sum.ToString();
		}
	}
}
