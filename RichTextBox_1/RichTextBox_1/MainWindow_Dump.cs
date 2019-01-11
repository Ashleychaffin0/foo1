using System;
using System.Windows;
using System.Windows.Documents;


namespace RichTextBox_1 {
	public partial class MainWindow {

//---------------------------------------------------------------------------------------

		void msg(string fmt, params object[] o) {
			listBox1.Items.Insert(0, string.Format(fmt, o));
		}

//---------------------------------------------------------------------------------------

		void DumpFd() {
			Console.WriteLine("\n********************\n");
			var fd = rt1.Document;
			var blks = fd.Blocks;
			DumpBlocks(blks);
		}

//---------------------------------------------------------------------------------------

		private void DumpBlocks(BlockCollection blks) {
			Console.WriteLine("{0} Block(s)", blks.Count);
			int ix = 0;
			foreach (var blk in blks) {
				string typename = blk.GetType().Name;
				Console.WriteLine("\tBlock[{0}] is-a {1}", ix++, typename);
				DumpBlock(blk, typename);
			}
		}

//---------------------------------------------------------------------------------------

		private void DumpBlock(Block blk, string typename) {
			switch (typename) {
			case "Paragraph":
				DumpParagraph(blk);
				break;
			case "Section":
			case "List":
			case "Table":
			case "BlockUIContainer":		// e.g. Image etc
			default:
				Console.WriteLine("Unable to process block type {0}", typename);
				break;
			}
		}

//---------------------------------------------------------------------------------------

		private static void DumpParagraph(Block blk) {
			var para = (Paragraph)blk;
			var inl = para.Inlines;
			Console.WriteLine("\t\t{0} Inlines", inl.Count);
			int ix = 0;
			foreach (var item in inl) {
				string typename = item.GetType().Name;
				Console.WriteLine("\t\t\tInline[{0}] is-a {1}", ix++, typename);
				DumpInline(item, typename);
			}
		}

//---------------------------------------------------------------------------------------

		private static void DumpInline(Inline item, string typename) {
			switch (typename) {
			case "RunLRS":
				DumpRunLRS(item as RunLRS);
				break;
			default:
				Console.WriteLine("Unable to process Inline type {0}", typename);
				break;
			}
		}

//---------------------------------------------------------------------------------------

		private static void DumpRunLRS(RunLRS RunLRS) {
			Console.WriteLine("\t\t\t\tRunLRS = {0}", RunLRS.Text);
		}

//---------------------------------------------------------------------------------------

		private void Dump_Click(object sender, RoutedEventArgs e) {
			DumpHashSet();
			DumpFd();
		}

//---------------------------------------------------------------------------------------

		private void DumpHashSet() {
			foreach (var item in hs) {
				Console.WriteLine("{0} -> {1}", item.Key, item.Value);
			}
		}
	}
}
