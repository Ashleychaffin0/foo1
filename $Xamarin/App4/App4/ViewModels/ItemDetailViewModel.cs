using System;

using App4.Models;

namespace App4.ViewModels {
	public class ItemDetailViewModel : BaseViewModel {
		public Item Item { get; set; }
		public ItemDetailViewModel(Item item = null) {
			Title = item?.Text;
			Item = item;
		}
	}
}
