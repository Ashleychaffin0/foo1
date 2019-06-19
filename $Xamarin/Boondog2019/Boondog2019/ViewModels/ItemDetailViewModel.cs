using System;

using nsBoondog2019.Models;

namespace nsBoondog2019.ViewModels {
	public class ItemDetailViewModel : BaseViewModel {
		public Item Item { get; set; }
		public ItemDetailViewModel(Item item = null) {
			Title = item?.Text;
			Item = item;
		}
	}
}
