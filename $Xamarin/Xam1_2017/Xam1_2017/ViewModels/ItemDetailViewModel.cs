using System;

using Xam1_2017.Models;

namespace Xam1_2017.ViewModels {
	public class ItemDetailViewModel : BaseViewModel {
		public Item Item { get; set; }
		public ItemDetailViewModel(Item item = null) {
			Title = item?.Text;
			Item = item;
		}
	}
}
