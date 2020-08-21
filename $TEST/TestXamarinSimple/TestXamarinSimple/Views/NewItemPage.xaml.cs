using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TestXamarinSimple.Models;
using TestXamarinSimple.ViewModels;

namespace TestXamarinSimple.Views
{
	public partial class NewItemPage : ContentPage
	{
		public Item Item { get; set; }

		public NewItemPage() {
			InitializeComponent();
			BindingContext = new NewItemViewModel();
		}
	}
}