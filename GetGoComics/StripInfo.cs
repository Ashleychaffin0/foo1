using System;
using System.ComponentModel.DataAnnotations;

// TODO: Missing Sunday strips
// TODO: Some kind of config file to tell which strips to 
// TODO: Add async support

namespace GetGoComics {
	public class StripInfo {
		public int		 StripInfoId		{ get; set; }
		[Required]
		public string	 Title				{ get; set; }
		[Required]
		public DateTime  BeginningDate		{ get; set; }
		[Required]
		public DateTime  LastDateCopied { get; set; }

//---------------------------------------------------------------------------------------

		public StripInfo(string title, DateTime beginningDate) {
			Title              = title;
			BeginningDate      = beginningDate;
			LastDateCopied = beginningDate.AddDays(-1);
		}

//---------------------------------------------------------------------------------------

		public StripInfo () {
			Title              = "";
			BeginningDate      = default;
			LastDateCopied = default;
		}
	}
}
