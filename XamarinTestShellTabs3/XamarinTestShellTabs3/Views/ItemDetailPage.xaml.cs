using System;
using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using XamarinTestShellTabs3.Models;
using XamarinTestShellTabs3.ViewModels;

namespace XamarinTestShellTabs3.Views {
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	[DesignTimeVisible(false)]
	public partial class ItemDetailPage : ContentPage {
		readonly ItemDetailViewModel viewModel;

//---------------------------------------------------------------------------------------

		public ItemDetailPage(ItemDetailViewModel viewModel) {
			InitializeComponent();

			BindingContext = this.viewModel = viewModel;
		}

//---------------------------------------------------------------------------------------

		public ItemDetailPage() {
			InitializeComponent();

			var item = new Item {
				Text = "Item 1",
				Description = "This is an item description."
			};

			viewModel = new ItemDetailViewModel(item);
			BindingContext = viewModel;
		}
	}
}