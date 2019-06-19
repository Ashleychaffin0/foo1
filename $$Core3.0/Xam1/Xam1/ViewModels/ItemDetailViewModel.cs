using System;

using Xam1.Models;

namespace Xam1.ViewModels {
	public class ItemDetailViewModel : BaseViewModel {
		public Item Item { get; set; }
		public ItemDetailViewModel(Item item = null) {
			Title = item?.Text;
			Item = item;
		}
	}
}
