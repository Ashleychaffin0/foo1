using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using nsBoondog2019.Models;
using nsBoondog2019.ViewModels;

namespace nsBoondog2019.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ItemDetailPage : ContentPage {
		ItemDetailViewModel viewModel;

		public ItemDetailPage(ItemDetailViewModel viewModel) {
			InitializeComponent();

			BindingContext = this.viewModel = viewModel;
		}

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