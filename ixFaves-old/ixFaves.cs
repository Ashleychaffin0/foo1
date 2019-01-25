// Copyright (c) 2013 by Larry Smith

// BUGS:
//  *   Search for "Windows 8" (with quotes) doesn't seem to work. Try other type
//      of query. Also check the F# article to see what was being indexed.

// TODO:
//  *   Change event on search box to react to the Enter key
//  *   Get Back button working. Presumably Forward will come along for the ride
//  *   async / await
//  *   Encapsulate to remove platform dependencies
//  *   Figure out to do with RSS
//  *   Identify other file formats than RSS
//  *   Delete obsolete code
//  *   Store file timestamp as well and re-index if changed

// TODO: Later
//  *   Make web app
//  *   Make Windows 8 app

// TODO: Lucene in general
//  *   Get PDF analyzed
//  *   Get Word (.doc and .docx) analyzed


using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using HtmlAgilityPack;
using LRS;
using Lucene.Net.Documents;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;

namespace ixFaves {
    public partial class ixFaves : Form {

        string FavesDir;

        // Lucene stuff
        MMapDirectory LDir;
        Lucene.Net.Util.Version version;
        Lucene.Net.Analysis.Standard.StandardAnalyzer analyzer;
        Lucene.Net.Index.IndexWriter idxWriter;

//---------------------------------------------------------------------------------------

        public ixFaves() {
            InitializeComponent();

            InitLucene();
        }

//---------------------------------------------------------------------------------------

        private void InitLucene() {
            LDir = new MMapDirectory(new DirectoryInfo(@"D:\LRS\LuceneData\LuceneMM"));
            // LDir = new RAMDirectory();
            version = Lucene.Net.Util.Version.LUCENE_30;
            analyzer = new Lucene.Net.Analysis.Standard.StandardAnalyzer(version);

            // Create indexes
            idxWriter = new Lucene.Net.Index.IndexWriter(LDir, analyzer, Lucene.Net.Index.IndexWriter.MaxFieldLength.UNLIMITED);

            /*
            Lucene.Net.Documents.Document doc2 = new Lucene.Net.Documents.Document();

            fldTitle = new Field("Title", "Adding a Document/object to Index with title2", Field.Store.YES, Lucene.Net.Documents.Field.Index.ANALYZED);
            // fldTitle.SetBoost(1.5f);
            doc2.Add(fldTitle);
            doc2.Add(new Field("Url", "http://antlr4.org/", Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));
            doc2.Add(new Field("Body", "Now you need to index your documents or business objects. To index an object, you use the Lucene Document class", Field.Store.NO, Lucene.Net.Documents.Field.Index.ANALYZED));

            idxWriter.AddDocument(doc2);
            idxWriter.Commit();
             */
        }

//---------------------------------------------------------------------------------------

        private void AddToLucene(string Title, string url, string body) {
            Lucene.Net.Documents.Document doc1 = new Lucene.Net.Documents.Document();

            var fldTitle = new Field("Title", Title, Field.Store.YES, Lucene.Net.Documents.Field.Index.ANALYZED);  // store index and original value
            fldTitle.Boost = 1.5f;  // This will set the boost to this field
            doc1.Add(fldTitle);
            // .Add(new Field("Url", url, Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));  // this field is not searched, and will be used as metadata
            doc1.Add(new Field("Url", url, Field.Store.YES, Lucene.Net.Documents.Field.Index.NOT_ANALYZED));  // this field is not searched, and will be used as metadata
            // doc1.Add(new Field("Body", body, Field.Store.NO, Lucene.Net.Documents.Field.Index.ANALYZED));  // Don't need to store the original text, this is a search-only field
            doc1.Add(new Field("Body", body, Field.Store.NO, Lucene.Net.Documents.Field.Index.ANALYZED));  // Don't need to store the original text, this is a search-only field

            idxWriter.AddDocument(doc1);
        }

//---------------------------------------------------------------------------------------

        private void SearchLucene() {
            // Just a sample of searching. Not used in the actual code for anything other than testing.
            // Lucene.Net.Search.Query query = new MultiFieldQueryParser(version, new string[] { "Title", "Body" }, analyzer).Parse("data title2");  // looking for words "data", "title2" in all fields
            Lucene.Net.Search.Query query = new MultiFieldQueryParser(version, new string[] { "Url" }, analyzer).Parse("antlr4.org/");  // looking for words "data", "title2" in all fields

            using (Lucene.Net.Search.IndexSearcher idxSearcher = new Lucene.Net.Search.IndexSearcher(LDir, true)) {
                TopScoreDocCollector collector = TopScoreDocCollector.Create(100, true);
                idxSearcher.Search(query, collector);
                var sdocs = collector.TopDocs().ScoreDocs;

                for (int i = 0; i < sdocs.Length; i++) {
                    var docId = sdocs[i].Doc;
                    Document doc = idxSearcher.Doc(docId);
                    // string url = doc.GetField("Url").StringValue();
                    string url = doc.GetField("Url").ToString();
                    string title = doc.GetField("Title").ToString();
                    string body = doc.GetField("Body").ToString();
                }
            }
        }

//---------------------------------------------------------------------------------------

        private bool HasUrlBeenIndexed(string Url) {
            // Lucene.Net.Search.Query query = new MultiFieldQueryParser(version, new string[] { "Title", "Body" }, analyzer).Parse("data title2");  // looking for words "data", "title2" in all fields
            try {
#if true
                using (Lucene.Net.Search.IndexSearcher idxSearcher = new Lucene.Net.Search.IndexSearcher(LDir, true)) {
                    Query qry = new TermQuery(new Lucene.Net.Index.Term("Url", Url));
                    TopDocs tds = idxSearcher.Search(qry, null, 10);
                    return tds.ScoreDocs.Length > 0;
                }
#else
                Lucene.Net.Search.Query query = new MultiFieldQueryParser(version, new string[] { "Url" }, analyzer).Parse(Url);

                using (Lucene.Net.Search.IndexSearcher idxSearcher = new Lucene.Net.Search.IndexSearcher(LDir, true)) {
                    TopScoreDocCollector collector = TopScoreDocCollector.Create(100, true);
                    idxSearcher.Search(query, collector);
                    var sdocs = collector.TopDocs().ScoreDocs;
                    return sdocs.Length > 0;
                }
#endif
            } catch (Exception ex) {
                string msg = string.Format("Exception on Url={0}, ex={1}", Url, ex.Message);
                lbMsgs.Items.Insert(0, msg);
                return false;
            }
        }

//---------------------------------------------------------------------------------------

        private void btnIndex_Click(object sender, EventArgs e) {
            FavesDir = Environment.GetFolderPath(Environment.SpecialFolder.Favorites);

            // Note: I don't use the Directory.EnumerateDirectories. At least 
            //       conceptually (and possibly (probably?)) it will seek to the
            //       directory (MFT), let us do our processing (which will probably
            //       involve disk (Database / Lucene) I/O, then seek back to the
            //       MFT for the next directory entry.
            //
            //       So let it get all the (sub-)directory entries at once (hopefully)
            //       with minimal head movement in the MFT). We'll process them as a
            //       group, then go back for the next set of (sub-)directories.
            string[] DirNames = System.IO.Directory.GetDirectories(FavesDir, "*", SearchOption.AllDirectories);
            ProcessDirs(DirNames);
            
            idxWriter.Commit();
            // SearchLucene();

            MessageBox.Show("Done");
        }

//---------------------------------------------------------------------------------------

        private void ProcessDirs(string[] DirNames) {
            foreach (string DirName in DirNames) {
                ProcessSingleDir(DirName);
            }
        }

//---------------------------------------------------------------------------------------

        private void ProcessSingleDir(string DirName) {
            string[] FileNames = System.IO.Directory.GetFiles(DirName);
            foreach (string FileName in FileNames) {
                var bif = new BartIniFile(FileName);
                if (bif.FindSection("InternetShortcut")) {
                    // If not found, ignore
                    string line;
                    while ((line = bif.ReadLine()) != null) {
                        if (line.StartsWith("URL=")) {
                            string url = line.Substring(4);
                            url = CleanUrl(url);

                            string JustDir = DirName.Substring(FavesDir.Length + 1);
                            string Title = Path.GetFileNameWithoutExtension(FileName);
                            string msg = string.Format("Processing {0} - Title:{1}, URL:{2}",
                                JustDir, Title, url);
                            lbMsgs.Items.Insert(0, msg);
                            Application.DoEvents();

                            if (!HasUrlBeenIndexed(url)) {
                                msg = string.Format(">>>>>>>> Not found - {0}", url);
                                lbMsgs.Items.Insert(0, msg);
                                var web = new WebClient();
                                string WebPageContents = web.DownloadString(url);
                                var doc = new HtmlAgilityPack.HtmlDocument();
                                doc.LoadHtml(WebPageContents);
                                HtmlAgilityPack.HtmlNode bodyNode = doc.DocumentNode.SelectSingleNode("//body");
                                if (bodyNode == null) {
                                    // May be, say, an RSS feed, which is just XML)
                                    // TODO: Add Msg(...) here
                                    continue;
                                }
                                var sb = new StringBuilder();
                                string body = GetPureText(bodyNode, sb, 0);

                                AddToLucene(Title, url, body);
                            }
                        }
                        // If we don't find it, ignore it
                    }
                }
                bif.Close();
            }
        }

//---------------------------------------------------------------------------------------

        private string CleanUrl(string url) {
            int ix = url.IndexOf("?utm_source=");
            if (ix >= 0) {
                return url.Substring(0, ix);
            }
            return url;
        }

//---------------------------------------------------------------------------------------

        /// <summary>
        /// Gets the pure inner text associated with a node
        /// </summary>
        /// <param name="bodyNode"></param>
        /// <param name="sb"></param>
        /// <param name="Level"></param>
        /// <returns></returns>
        private string OldVersion_GetPureText(HtmlNode bodyNode, StringBuilder sb, int Level) {
            foreach (var node in bodyNode.ChildNodes) {
                switch (node.Name) {
                    case "#comment":
                    case "script":
                        // Ignore these
                        break;
                    default:
                        // Append InnerText iff there are no child nodes
                        var kids = node.ChildNodes;
                        if (kids.Count == 0) {
                            sb.Append(" ");
                            // Console.WriteLine(">>>>>>>> Adding: {0}", node.InnerText);
                            // Console.WriteLine("<<<<<<<<");
                            sb.Append(node.InnerText);
                        } else {
                            foreach (var ChildNode in kids) {
                                OldVersion_GetPureText(ChildNode, sb, Level + 1);
                            }
                        }
                        break;
                }
            }
            return sb.ToString();
        }

//---------------------------------------------------------------------------------------

        /// <summary>
        /// Gets the pure inner text associated with a node
        /// </summary>
        /// <param name="bodyNode"></param>
        /// <param name="sb"></param>
        /// <param name="Level"></param>
        /// <returns></returns>
        private string GetPureText(HtmlNode node, StringBuilder sb, int Level) {
            // foreach (var node in bodyNode.ChildNodes) {}
                switch (node.Name) {
                    case "#comment":
                    case "script":
                        // Ignore these
                        break;
                    default:
                        // Append InnerText iff there are no child nodes
                        var kids = node.ChildNodes;
                        if (kids.Count == 0) {
                            sb.Append(" ");
                            sb.Append(node.InnerText);
                        } else {
                            foreach (var ChildNode in kids) {
                                GetPureText(ChildNode, sb, Level + 1);
                            }
                        }
                        break;
                }
            // }
            return sb.ToString();
        }

//---------------------------------------------------------------------------------------

        private void btnSearch_Click(object sender, EventArgs e) {
            lbMsgs.Items.Clear();
            Lucene.Net.Search.Query query = new MultiFieldQueryParser(version, new string[] { "Title", "Body" }, analyzer).Parse(txtSearch.Text);
            using (Lucene.Net.Search.IndexSearcher idxSearcher = new Lucene.Net.Search.IndexSearcher(LDir, true)) {
                TopScoreDocCollector collector = TopScoreDocCollector.Create(100, true);
                idxSearcher.Search(query, collector);
                var sdocs = collector.TopDocs().ScoreDocs;

                if (sdocs.Length == 0) {
                    MessageBox.Show("No hits found");
                    return;
                }

                var sb = new StringBuilder();
                sb.Append("<html>\n<head>\n\t<title>ixFaves Hits</title>\n</head>\n");
                sb.Append("<body>\n");

                for (int i = 0; i < sdocs.Length; i++) {
                    var docId = sdocs[i].Doc;
                    Document doc = idxSearcher.Doc(docId);
                    string url = doc.GetField("Url").StringValue;
                    string title = doc.GetField("Title").StringValue;
                    sb.AppendFormat("<p><a href=\"{0}\">{1}</a></p>\n", url, title);
                    string msg = string.Format("Title: {0}, Url={1}", title, url);
                    lbMsgs.Items.Insert(0, msg);
                }

                sb.Append("</body>\n</html>\n");

                web.DocumentText = sb.ToString();
            }
        }

//---------------------------------------------------------------------------------------

        private void btnBack_Click(object sender, EventArgs e) {
            web.GoBack();
        }

//---------------------------------------------------------------------------------------

        private void btnForward_Click(object sender, EventArgs e) {
            web.GoForward();
        }
    }
}
