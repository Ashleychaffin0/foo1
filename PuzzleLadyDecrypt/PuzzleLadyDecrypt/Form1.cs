using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PuzzleLadyDecrypt {
	public partial class Form1 : Form {

		char [] map = new char[128];

//---------------------------------------------------------------------------------------

		public Form1() {
			InitializeComponent();
		}

//---------------------------------------------------------------------------------------

		private void Form1_Load(object sender, EventArgs e) {
			Init();

			string clear             = SetPuzzle_1();
			ShowPuzzle(clear);
		}

//---------------------------------------------------------------------------------------

		private void ShowPuzzle(string clear) {
			textBox1.Text = Encode(clear);
			textBox1.SelectionStart = 0;
			textBox1.SelectionLength = 0;
			textBox1.ReadOnly = true;

			string s          = Translate(textBox1.Text);
			web1.DocumentText = s;

			foo(textBox1.Text);
		}

//---------------------------------------------------------------------------------------

		private void foo(string s) {
			dgvCounts.Rows.Add(1);
			for (int i = 0; i < dgv.ColumnCount; i++) {
				dgvCounts.Columns[i].Width = dgv.Columns[i].Width;
				dgvCounts.Columns[i].HeaderText = "";
			}
			var q1 = from c in s
					 where char.IsLetter(c)
					 group c by c into cGrp
					 orderby cGrp.Count() descending
					 select cGrp;
			int col = 0;
			foreach (var x in q1) {
				int ofs = x.Key - 'a';
				int cnt = x.Count();
				dgvCounts.Columns[col].HeaderText = x.Key.ToString();
				dgvCounts.Rows[0].Cells[col].Value = cnt.ToString();
				++col;
			}
		}

//---------------------------------------------------------------------------------------

		private void Remap(string Code, string ClearText) {
			map[Code[0]] = ClearText[0];
		}

//---------------------------------------------------------------------------------------

		private void Init() {
			for (int i = 0; i < map.Length; i++) {
				map[i] = Convert.ToChar(i);
			}

			for (int i = 0; i < dgv.ColumnCount; i++) {
				dgv.Columns[i].Width = 44;
			}
			dgv.Rows.Add(1);
		}

//---------------------------------------------------------------------------------------

		private string SetPuzzle_1() {
			return @"DEAR PUZZLE LADY,
YOU'RE MAKING A BIG MISTAKE. THIS IS A BAD MATCH. IF
YOU'RE SMART, YOU'LL BREAK IT OFF, BEFORE SOMETHING
HAPPENS. BEFORE SOMEONE GETS HURT. LEAVE HIM. WALK
AWAY, AND DON'T LOOK BACK. IT'S NOT TOO LATE TO RECTIFY
THE ERRORS THAT YOU'VE MADE. WALK AWAY WHILE THERE'S
STILL TIME. WALK AWAY.".ToLower();
		}

//---------------------------------------------------------------------------------------

		private string SetPuzzle_2() {
			return @"DEAR BRIDEGROOM,
YOU THINK YOU'RE SO SMART BUT YOU'RE NOT. BUTT OUT OF MY
BUSINESS, OR THIS WEDDING ISN'T GOING TO HAPPEN. THIS IS
A WARNING. I'M GETTING ANGRY.".ToLower();
		}

//---------------------------------------------------------------------------------------

		private string SetPuzzle_3() {
			return @"DEAR PUZZLE LADY,

SHUT HIM UP, IF YOU KNOW WHAT'S GOOD FOR YOU.
I DON'T HAVE TO TAKE THIS FROM ANYONE.".ToLower();
		}

//---------------------------------------------------------------------------------------

		private string SetPuzzle_4() {
			string s = @"All of this amused me greatly, for of course neither the newness nor the orthodoxy of a hypothesis will make it true if it is not true, nor untrue if it is true. Nor could the luck or will-power, with which I had resisted their hypnotists and psychoanalysts, make what might or might not be a universal fact one whit more or less of a fact than it really was. But the prestige I had gained among them, and the novelty of my expressed opinion carried much weight with them.

Yet, did not even brilliant scientists frequently exhibit the same lack of logic back in the Twentieth Century? Did not the historians, the philosophers of ancient Greece and Rome show themselves to be the same shrewd observers as those of succeeding centuries, the same masters of the logical and slaves of the illogical?

After all, I reflected, man makes little progress within himself. Through succeeding generations he piles up those resources which he possesses outside of himself, the tools of his hands, and the warehouses of knowledge for his brain, whether they be parchment manuscripts, printed book, or electronorecordographs. For the rest he is born today, as in ancient Greece, with a blank brain, and struggles through to his grave, with a more or less beclouded understanding, and with distinct limitations to what we used to call his ""think tank.""

This particular reflection of mine proved unpopular with them, for it stabbed their vanity, and neither my prestige nor the novelty of the idea was sufficient salve. These Hans for centuries had believed and taught their children that they were a super-race, a race of destiny. Destined to Whom, for What, was not so clear to them; but nevertheless destined to ""elevate"" humanity to some sort of super-plane. Yet through these same centuries they had been busily engaged in the extermination of ""weaklings,"" whom, by their very persecutions, they had turned into ""super men,"" now rising in mighty wrath to destroy them; and in reducing themselves to the depths of softening vice and flabby moral fiber. Is it strange that they looked at me in amazed wonder when I laughed outright in the midst of some of their most serious speculations?
";

			return s;
		}

//---------------------------------------------------------------------------------------

		string SetPuzzle_5() {
			string s = File.ReadAllText(@"C:\lrs\201-h.txt");
			return s;
		}

//---------------------------------------------------------------------------------------

		string SetPuzzle_6() {
			string filename;
			string dirname = @"C:\downloads\gutenberg\";
			var rand = new Random();
			while (true) {
				var files = Directory.GetFiles(dirname, "*.txt");
				var n = rand.Next(files.Count());
				filename = files[n];
				FileInfo fi = new FileInfo(Path.Combine(dirname, filename));
				if (fi.Length <= 2 * 1024 * 1024) {
					break;
				}
			}
			string s = File.ReadAllText(filename);
			return s;
		}

//---------------------------------------------------------------------------------------

		private string Translate(string txt) {
			StringBuilder sb = new StringBuilder(txt.Length);
			for (int i = 0; i < txt.Length; i++) {
				char c = txt[i];
				if (c > 128) c = '?';
				c = map[c];
				string s;
				if (c == '\n') {
					s = "<br>";
				} else if (char.IsLower(c)) {
					s = string.Format("<font color=\"red\">{0}</font>", c);
				} else {
					s = c.ToString();
				}
				sb.Append(s);
			}
			return sb.ToString();
		}

//---------------------------------------------------------------------------------------

		private string Encode(string plain) {
			// Create random map
			char[] map = new char[26];
			int cnt = 0;			// # of slots that are filled
			Random rand = new Random();
			char[] alpha = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
			int nLoops = 0;
			int i = 0;
			while (cnt < 26) {
				++nLoops;
				int n = rand.Next(26);
				if (map[n] == '\0' && n != i) {
					map[n] = alpha[i++];
					++cnt;
				}
			}
			MessageBox.Show("# of loops = " + nLoops);
			// Now encode it
			var coded = new StringBuilder(plain.Length);
			plain = plain.ToLower();
			foreach (char c in plain) {
				if (char.IsLetter(c)) {
					coded.Append(map[c - 'a']);
				} else {
					coded.Append(c);
				}
			}
			return coded.ToString();
		}

//---------------------------------------------------------------------------------------

		private void dgv_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			if (e.RowIndex != 0) {
				return;
			}
			var Code = dgv.Columns[e.ColumnIndex].HeaderText.ToLower();
			var ClearText = (string)dgv[e.ColumnIndex, e.RowIndex].Value;
			if (ClearText == null) {
				ClearText = Code.ToLower();		// Revert to default
			} else {
				ClearText = ClearText.ToUpper();
			}

			Remap(Code, ClearText);
			string s = Translate(textBox1.Text);
			web1.DocumentText = s;
		}
	}
}
