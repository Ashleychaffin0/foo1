using System.ComponentModel;
using Xamarin.Forms;
using TestXamarinSimple.ViewModels;

namespace TestXamarinSimple.Views
{
	public partial class ItemDetailPage : ContentPage
	{
		public ItemDetailPage() {
			InitializeComponent();
			BindingContext = new ItemDetailViewModel();
		}
	}
}