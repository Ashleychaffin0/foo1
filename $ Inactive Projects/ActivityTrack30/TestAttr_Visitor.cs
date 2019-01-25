using	System;
using	System.Collections;
using	System.Drawing;
using	System.Reflection;
using	System.Windows.Forms;

namespace Bartizan.ActivityTrack30 {
	
	public class BartLayoutAttribute : System.Attribute {
		public enum			FieldTypes {Class, Field};
		// Type == Class
		public string		Title;
		// Type == Field
		public FieldTypes	FieldType;
		public string		Label;				// AKA caption
		public int			Length;

		public BartLayoutAttribute() {
			FieldType	= FieldTypes.Field;
			Title		= "*Missing*";
			Label		= null;
			Length		= -1;
		}

//---------------------------------------------------------------------------------------

		public void dbgShow() {
			Console.WriteLine("FieldType={0}, Title='{1}', Label='{2}', Length={3}",
				FieldType, Title, Label, Length);
		}

		// Additional possible attributes
		//	For Classes:
		//		* Form width and height
		//	For Fields:
		//		* Row and column
		//		* Whether Labels are above or to the left of the data field
		//	Others:
		//		* Links between tables???
	}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------


	public class BartLayoutManager {
		Form		win;
		int			CurX, CurY;				// Current X, Y locations

		public BartLayoutManager() {
			win = new Form();
			win.Width = 600;
			win.Height = 400;
			win.Text = "LRS generated Form - " + DateTime.Now.ToString();
			win.BackColor = Color.AliceBlue;
			win.SuspendLayout();

			// Arbitrarily start things at (10, 10)
			CurX = 10;
			CurY = 10;

			BuildScreen();
		}

//---------------------------------------------------------------------------------------

		public void BuildScreen() {
			Type	t = typeof(TestAttr_Visitor);
			foreach (Attribute a in t.GetCustomAttributes(false)) {
				if (a.GetType() != typeof(BartLayoutAttribute))
					break;
				// Console.WriteLine("Attr ToString={0}", a.ToString());
				BartLayoutAttribute	aa = (BartLayoutAttribute)a;
				// aa.dbgShow();
				Label	titlebox = new Label();
				titlebox.Text = aa.Title;
				titlebox.Font = new Font("Helvetica", 12, FontStyle.Bold);
				titlebox.Top = CurY;
				titlebox.Left = CurX;
				titlebox.Width = win.ClientRectangle.Width - 20;	// 20 is arbitrary
				titlebox.Height = 20;
				titlebox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
				titlebox.TextAlign = ContentAlignment.MiddleCenter;
				titlebox.BorderStyle = BorderStyle.FixedSingle;
				win.Controls.Add(titlebox);

				CurY = titlebox.Bottom + 20;

				Label		lbl;
				TextBox		txt;

				foreach (MemberInfo mi in t.GetMembers()) {
					if (mi.MemberType == MemberTypes.Field) {
						// Console.WriteLine("\nField - {0}, Type={1}", mi.Name, mi.MemberType);
						foreach (Attribute ma in mi.GetCustomAttributes(false)) {
							aa = (BartLayoutAttribute)ma;
							// aa.dbgShow();
							lbl = new Label();
							lbl.Size = new Size(100, 32);
							lbl.Location = new Point(CurX, CurY);
							if (aa.Label == null)
								lbl.Text = mi.Name;
							else
								lbl.Text = aa.Label;
							win.Controls.Add(lbl);

							txt = new TextBox();
							txt.Size = new Size(100, 32);
							txt.Location = new Point(lbl.Right + 10, CurY);
							txt.Text = "";
							win.Controls.Add(txt);

							CurY += 32;
						}
					}
				}
			}
			win.ResumeLayout(true);
			win.Show();
		}
	}

//---------------------------------------------------------------------------------------

	[BartLayout(FieldType=BartLayoutAttribute.FieldTypes.Class, Title="Test Attribute - Visitor")]
	public class TestAttr_Visitor {
		[BartLayout(Length=12)]
		public string		SSN;
		[BartLayout(Label="First Name", Length=50)]
		public string		FName;
		[BartLayout(Label="Initial", Length=50)]
		public string		MidName;
		[BartLayout(Label="Last Name", Length=50)]
		public string		LastName;
		[BartLayout()]
		public DateTime		DOB;
	
		[BartLayout(Label="Dami")]
		public string		Ahmed;

#if true
		[BartLayout(Length=50)]
		public string		City;
		[BartLayout(Length=50)]
		public string		State;
		[BartLayout(Length=50)]
		public string		Zip;
#endif

		public TestAttr_Visitor() {
			SSN			= "";
			FName		= "";
			MidName		= "";
			LastName	= "";
			DOB			= DateTime.Parse("1900/1/1");
		}

	}

//---------------------------------------------------------------------------------------


}
