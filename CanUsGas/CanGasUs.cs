using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
// using System.Json;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


// One liter  is equal to 0.2641721 gallons. 
// One gallon is equal to 3.7854111 litres

// http://finance.yahoo.com/d/quotes.csv?e=.csv&f=c4l1&s=CADUSD=X,USDCAD=X

// UI Notes:
//	*	We have to be careful with events such as Click, Changed, etc. Consider the
//		(erroneous) design, such that when we change the # of USD dollars (and fire the
//		Changed event that field), that it immediately changes the # of CAD, which fires
//		the Changed event for that field and modifies the #USD field, leading to an
//		infinite loop.
//	*	So it works like this. 
//			*	We support the Focus/Enter event. This selects the text in that control.
//			*	The Keypress event beeps if a there is an invalid number in the field,
//				and restores the previous value. Note: Have to be careful of a field
//				with just "." in it. Also, blanks are silently ignored. But if we have a
//				valid value in there, we set the .Text property of the other $ field to
//				the properly calculated value.

namespace CanUsGas {
	public partial class CanGasUs : Form {

		const float LitresInAGallon = 0.2641721f;

		float Cad;
		float Usd;

//---------------------------------------------------------------------------------------

		public CanGasUs() {
			InitializeComponent();

			Cad = GetCanInUsd();
			Usd = 1 / Cad;

			txtUSD.Text = "1";

			var zz = GasBuddy.GetBrands();

			var xx = CanPriceInLitresToUsPriceInGallons(1f);

			int yy = 1;
		}

//---------------------------------------------------------------------------------------

		private static float GetCanInUsd() {
			var wc = new WebClient();
			var xx = wc.DownloadData("http://finance.yahoo.com/d/quotes.csv?e=.csv&f=c4l1&s=CADUSD=X");
			string CadToUsd = ASCIIEncoding.ASCII.GetString(xx);	// "CAD","0,9243"\r\n
			var Cad = (CadToUsd.Split(',', '\r'))[1];
			return Convert.ToSingle(Cad);
		}

//---------------------------------------------------------------------------------------

		private void txtUSD_TextChanged(object sender, EventArgs e) {
			var NumUSD = Convert.ToSingle(((TextBox)sender).Text);
			txtCAD.Text = (NumUSD * Usd).ToString();
		}

//---------------------------------------------------------------------------------------

		private void txtUSD_Click(object sender, EventArgs e) {

		}

//---------------------------------------------------------------------------------------

		private void txtUSD_Enter(object sender, EventArgs e) {
			txtUSD.SelectAll();
		}

//---------------------------------------------------------------------------------------

		private void txtUSD_KeyPress(object sender, KeyPressEventArgs e) {

		}

//---------------------------------------------------------------------------------------

		private void txtCAD_Click(object sender, EventArgs e) {

		}

//---------------------------------------------------------------------------------------

		private void txtCAD_Enter(object sender, EventArgs e) {
			txtCAD.SelectAll();
		}

//---------------------------------------------------------------------------------------

		private void txtCAD_KeyPress(object sender, KeyPressEventArgs e) {
			if (e.KeyChar == ' ') {
				return;
			}
			var bOK = float.TryParse(txtCAD.Text, out float Amt);
			
			if (bOK) {
				txtUSD.Text = Amt.ToString();
			}
		}

//---------------------------------------------------------------------------------------

		private float CanPriceInLitresToUsPriceInGallons(float litres) {
			var LitersCorrspondingTo1GalUs = 1 / LitresInAGallon;		// 3.78 (or so)
			var PriceCanadianPerLiter = (float)1.322f;
			var CanPrice = LitersCorrspondingTo1GalUs * PriceCanadianPerLiter * Usd;
			var UsPrice = CanPrice / Usd;
			return UsPrice;
		}

//---------------------------------------------------------------------------------------

		private void txtUsPerGallon_TextChanged(object sender, EventArgs e) {
			// if (! Focused) {
			// 	return;
			// }
			string txt = txtUsPerGallon.Text.Replace("$", "");
			bool bOK = float.TryParse(txt, out float UsCostPerGallon);
			if (!bOK) {
				MessageBox.Show("Invalid number - digits and optional decimal point only", 
					"CanGasUs", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			// MessageBox.Show("Nonce");
		}

//---------------------------------------------------------------------------------------

		private void txtCanPerLitre_TextChanged(object sender, EventArgs e) {
			// if (! Focused) {
			// 	return;
			// }
			string txt = txtCanPerLitre.Text.Replace("$", "");
			bool bOK = float.TryParse(txt, out float CanCostPerLitre);
			if (!bOK) {
				MessageBox.Show("Invalid number - digits and optional decimal point only", 
					"CanGasUs", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			var LitersCorrspondingTo1GalUs = 1 / LitresInAGallon;		// 3.78 (or so)
			// var PriceCanadianPerLiter = (float)1.369f;
			var CanPrice = LitersCorrspondingTo1GalUs * CanCostPerLitre * Usd;
			var UsPrice = CanPrice / Usd;
			txtUsPerGallon.Text = string.Format("{0:C2}", UsPrice);
		}
	}
}
