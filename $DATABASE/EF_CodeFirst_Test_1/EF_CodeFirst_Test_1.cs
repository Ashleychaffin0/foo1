using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// http://www.entityframeworktutorial.net/

// http://stackoverflow.com/questions/21563940/how-to-connect-to-localdb-in-visual-studio-server-explorer

namespace EF_CodeFirst_Test_1 {
	public partial class EF_CodeFirst_Test_1 : Form {
		public EF_CodeFirst_Test_1() {
			InitializeComponent();

			var cook = Environment.SpecialFolder.Cookies;
			var dir = Environment.GetFolderPath(cook);
			MessageBox.Show("Hello world 2");


			Database.SetInitializer<Context>(new DropCreateDatabaseIfModelChanges<Context>());

			using (var ctx = new Context()) {
				Issue ish = new Issue() { Year = 2002, Month = 1, Title = "The First Human Clone" };
				ish.Articles.Add(new Article() {
					Name = "The Gas Between the Stars",
					Description = "Filled with colossal fountains of hot gas and vast bubbles from exploding stars, the interstellar medium is far from dull",
					Genre = new Genre("Astronomy")
				});
				ish.Articles.Add(new Article() {
					Name = "The First Human Cloned Embryo",
					Description = "For the first time, human embryos have been created by two extrordinary means: cloning and parhenogenesis. A firsthand report by the research team",
					Genre = new Genre("Biology")
				});
				ctx.Issues.Add(ish);
				var n = ctx.SaveChanges();

				var Issues = from ish2 in ctx.Issues
							 select ish2;

				foreach (var item in Issues) {

				}

			}
		}
	}
}
