using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using App2.Models;
using App2.ViewModels;

namespace App2.Views {
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