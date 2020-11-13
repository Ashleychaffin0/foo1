using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PWW {
	// Represents a comment about this User/Url/etc. Can hold things like
	// Security questions and answers, and anything else

	// Note: The methods in this class are almost identical (other than referencing
	//		 different fields) to those in User.cs. Please see comments there.

	[Table("tblComments")]
	public class Comment {
		public int CommentId { get; set; }
		[Required]
		public int UserId { get; set; }
		[Required]
		public int SiteId { get; set; }
		[Required]
		public string Text { get; set; }

//---------------------------------------------------------------------------------------

		public Comment() => Text = "";

//---------------------------------------------------------------------------------------

		public static Comment Add(PWContext db, int userid, int siteid, ImportUserInfo userinfo) {
			var comment = new Comment() {
				UserId = userid,
				SiteId = siteid,
				Text   = userinfo.comment.ToString()
			};
			db.Comments.Add(comment);
			return comment;
		}
	}
}

