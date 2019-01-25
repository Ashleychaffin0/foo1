using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MovieCollection2.Controls {
	[DefaultProperty("CurrentRating"), DefaultEvent("CurrentRatingChanged"), DefaultBindingProperty("CurrentRating")]
	public partial class RatingPickerControl : Panel, ISupportInitialize {
		private const int DEFAULTVALUE = -1;

		private bool m_Initializing = false;
		private int m_CurrentRating = DEFAULTVALUE;

		public event EventHandler CurrentRatingChanged;

		[Bindable(true)]
		public int CurrentRating {
			get {
				return this.m_CurrentRating;
			}
			set {
				if ((value != this.m_CurrentRating) && !(this.m_Initializing)) {
					this.m_CurrentRating = value;
					if (this.CurrentRatingChanged != null) {
						OnCurrentRatingChanged(EventArgs.Empty);
					}
					ShowNewRating(value);
				}
			}
		}

		protected void OnCurrentRatingChanged(EventArgs e) {
			this.CurrentRatingChanged(this, e);
		}

		private bool ShouldSerializeCurrentRating() {
			return (this.m_CurrentRating == DEFAULTVALUE);
		}

		public override string ToString() {
			return this.CurrentRating.ToString();
		}

		public enum RatingPickerValue {
			NotSet = -1,
			Poor = 1,
			BelowAverage = 2,
			Average = 3,
			AboveAverage = 4,
			Excellent = 5
		};

		protected internal virtual void Star_Click(object sender, System.EventArgs e) {
			PictureBox currentStar = (PictureBox)sender;

			if (!this.m_Initializing) {
				switch (currentStar.Name) {
				case "Star1":
					this.CurrentRating = (int)RatingPickerValue.Poor;
					break;
				case "Star2":
					this.CurrentRating = (int)RatingPickerValue.BelowAverage;
					break;
				case "Star3":
					this.CurrentRating = (int)RatingPickerValue.Average;
					break;
				case "Star4":
					this.CurrentRating = (int)RatingPickerValue.AboveAverage;
					break;
				case "Star5":
					this.CurrentRating = (int)RatingPickerValue.Excellent;
					break;
				}
			}

		}


		protected internal virtual void Star_MouseHover(object sender, System.EventArgs e) {
			PictureBox hoverOverStar = (PictureBox)sender;

			if (!this.m_Initializing) {
				switch (hoverOverStar.Name) {
				case "Star1":
					this.PreviewNewRating((int)RatingPickerValue.Poor);
					break;
				case "Star2":
					this.PreviewNewRating((int)RatingPickerValue.BelowAverage);
					break;
				case "Star3":
					this.PreviewNewRating((int)RatingPickerValue.Average);
					break;
				case "Star4":
					this.PreviewNewRating((int)RatingPickerValue.AboveAverage);
					break;
				case "Star5":
					this.PreviewNewRating((int)RatingPickerValue.Excellent);
					break;
				}
			}

		}

		protected internal virtual void Star_MouseLeave(object sender, System.EventArgs e) {
			if (!this.m_Initializing) {
				this.ShowNewRating(this.CurrentRating);
			}

		}

		protected internal virtual void ShowNewRating(int newRating) {
			Bitmap notSetImage = Properties.Resources.star_empty;
			Bitmap setImage = Properties.Resources.star_filled;

			RatingPickerValue newRatingValue = (RatingPickerValue)newRating;

			switch (newRatingValue) {
			case RatingPickerValue.NotSet:
				this.Star1.Image = notSetImage;
				this.Star2.Image = notSetImage;
				this.Star3.Image = notSetImage;
				this.Star4.Image = notSetImage;
				this.Star5.Image = notSetImage;
				break;
			case RatingPickerValue.Poor:
				this.Star1.Image = setImage;
				this.Star2.Image = notSetImage;
				this.Star3.Image = notSetImage;
				this.Star4.Image = notSetImage;
				this.Star5.Image = notSetImage;
				break;
			case RatingPickerValue.BelowAverage:
				this.Star1.Image = setImage;
				this.Star2.Image = setImage;
				this.Star3.Image = notSetImage;
				this.Star4.Image = notSetImage;
				this.Star5.Image = notSetImage;
				break;
			case RatingPickerValue.Average:
				this.Star1.Image = setImage;
				this.Star2.Image = setImage;
				this.Star3.Image = setImage;
				this.Star4.Image = notSetImage;
				this.Star5.Image = notSetImage;
				break;
			case RatingPickerValue.AboveAverage:
				this.Star1.Image = setImage;
				this.Star2.Image = setImage;
				this.Star3.Image = setImage;
				this.Star4.Image = setImage;
				this.Star5.Image = notSetImage;
				break;
			case RatingPickerValue.Excellent:
				this.Star1.Image = setImage;
				this.Star2.Image = setImage;
				this.Star3.Image = setImage;
				this.Star4.Image = setImage;
				this.Star5.Image = setImage;
				break;
			default:
				this.Star1.Image = notSetImage;
				this.Star2.Image = notSetImage;
				this.Star3.Image = notSetImage;
				this.Star4.Image = notSetImage;
				this.Star5.Image = notSetImage;
				break;
			}
		}

		protected internal virtual void PreviewNewRating(int newRating) {
			//currently same as ShowNewRating; in future one might want a 
			//custom image or behavior
			this.ShowNewRating(newRating);
		}

		public void BeginInit() {
			this.m_Initializing = true;

		}

		public void EndInit() {
			this.m_Initializing = false;
			this.CurrentRating = this.m_CurrentRating;

			//Refresh display in case there was no change via the Set
			this.ShowNewRating(this.CurrentRating);
		}
	}
}


