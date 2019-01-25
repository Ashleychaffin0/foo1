using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Data;
using System.Xml;

namespace SharpLog {
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class SharpLog : System.Windows.Forms.Form {
		private System.Windows.Forms.TextBox txtSharpDir;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnBrowse;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ListBox lbDebug;
		private System.Windows.Forms.TreeView tvFeeds;
		private System.Windows.Forms.Button btnGo;

		string	SharpDir, CacheDir;
		private System.Windows.Forms.StatusBar StatMsg;

		XmlDocument		xdoc = new XmlDocument();

		public SharpLog() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			SharpDir = @"C:\Documents and Settings\" + Environment.UserName
				+ @"\Application Data\SharpReader\";
			txtSharpDir.Text = SharpDir;
		}

//---------------------------------------------------------------------------------------

		private void btnGo_Click(object sender, System.EventArgs e) {
			SharpDir = txtSharpDir.Text;
			CacheDir = SharpDir + @"Cache\";

			XmlDocument		subs = null;
			StreamReader	sr = null;
			try {
				subs = new XmlDocument();
				sr = new StreamReader(SharpDir + "subscriptions.xml");
				subs.LoadXml(sr.ReadToEnd());
			} catch (Exception ex) {
				MessageBox.Show("Unable to process Subscriptions.xml - " + ex.Message);	// TODO: Better msg
				return;
			} finally {
				if (sr != null)
					sr.Close();
			}

			TreeNode		rootNode = tvFeeds.Nodes.Add("Subscribed Feeds");
			XmlNodeList		xnl;
			xnl = subs.GetElementsByTagName("feeds");
			XmlNode	rootXmlNode = xnl.Item(0);
			int				nNodes = rootXmlNode.ChildNodes.Count;
			int				n = 0;
			foreach (XmlNode node in rootXmlNode.ChildNodes) {
				++n;
				StatMsg.Text = "Processing node " + node.Name
					+ " (" + n.ToString() + " of " + nNodes.ToString() + ")";
				ProcessNode(rootNode, node);
			}
		}

//---------------------------------------------------------------------------------------

		void ProcessNode(TreeNode treeNode, XmlNode node) {
			switch (node.Name) {
			case "RssFeed":
				ProcessFeed(treeNode, node);
				break;
			case "RssFeedsCategory":
				ProcessCategory(treeNode, node);
				break;
			default:
				MessageBox.Show("Unable to recognize node type '" + node.Name + "'");
				break;
			}
		}

//---------------------------------------------------------------------------------------

		void ProcessFeed(TreeNode treeNode, XmlNode node) {
			string	name = node.Attributes["name"].Value;
			TreeNode	tn = treeNode.Nodes.Add(name);
			tn.Tag = node;
			// debug("ProcessFeed - " + name);
			// xdoc.LoadXml(GetFileFromCache(node));
			try {
				xdoc.Load(CacheDir + GetCacheUrl(node));
			} catch (Exception) {
				return;			// Silently ignore any errors, for now. TODO:
			}
			ProcessContents(xdoc);
		}

//---------------------------------------------------------------------------------------

		void ProcessCategory(TreeNode treeNode, XmlNode node) {
			string	name = node.Attributes["name"].Value;
			// debug("ProcessCategory - " + name);
			TreeNode	tn = treeNode.Nodes.Add(name);
			tn.Tag = node;
			foreach (XmlNode child in node.ChildNodes) {
				ProcessNode(tn, child);
			}
		}

//---------------------------------------------------------------------------------------

		private void tvFeeds_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e) {
			XmlNode	node = (XmlNode)e.Node.Tag;
			if (node != null) {
				string s = GetFileFromCache(node);
				if (s == null) {		
					MessageBox.Show("Can't find cached file for this node");
					return;
				}
				string url = GetCacheUrl(node);
				MessageBox.Show("Node name = " + e.Node.Text + ", " + url);
				MessageBox.Show(s);
			}
		}

//---------------------------------------------------------------------------------------

		void ProcessContents(XmlDocument doc) {
			XmlElement	root = doc.DocumentElement;
			LoopThroughElements(root);
		}

//---------------------------------------------------------------------------------------

		void LoopThroughElements(XmlNode root) {
			debug("<node name = " + root.Name + ">");			
			foreach (XmlNode node in root.ChildNodes) {
				if (node.NodeType == XmlNodeType.Element) {
					switch (node.Name) {
					case "Items":
						ProcessItems(node);
						break;
					case "Title":
						ProcessTitle(node);
						break;
					case "Description":
						ProcessDescription(node);
						break;
					default:
						debug("*** Not processed: " + node.Name);
						break;
					}
					LoopThroughElements(node);
				}
			}
			debug("</" + root.Name + ">\n");
		}

//---------------------------------------------------------------------------------------

		void ProcessItems(XmlNode node) {
			debug("Processing Item " + node.InnerXml);
		}

//---------------------------------------------------------------------------------------

		void ProcessTitle(XmlNode node) {
			debug("Processing Title " + node.InnerXml);
		}

//---------------------------------------------------------------------------------------

		void ProcessDescription(XmlNode node) {
			debug("Processing Description " + node.InnerXml);
		}

//---------------------------------------------------------------------------------------

		private string GetCacheUrl(XmlNode node) {
			XmlAttribute attr = node.Attributes["url"];
			string	s = attr.Value;
			int		n = s.IndexOf("//");
			if (n > 0)
				s = s.Substring(n + 2);
			s = s.Replace("/", "-");
			if (! s.EndsWith(".xml"))
				s += ".xml";
			return s;
		}

//---------------------------------------------------------------------------------------

		string GetFileFromCache(XmlNode	node) {
			StreamReader	sr = null;
			try {
				string url = GetCacheUrl(node);
				sr = new StreamReader(CacheDir + url);
				return sr.ReadToEnd();
			} catch (Exception) {
				return null;
			} finally {
				if (sr != null)
					sr.Close();
			}
		}

//---------------------------------------------------------------------------------------

		void debug(string txt) {
			lbDebug.Items.Add(txt);
		}

//---------------------------------------------------------------------------------------

		#region Windows Form Designer generated code
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)	{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.txtSharpDir = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.btnGo = new System.Windows.Forms.Button();
			this.lbDebug = new System.Windows.Forms.ListBox();
			this.tvFeeds = new System.Windows.Forms.TreeView();
			this.StatMsg = new System.Windows.Forms.StatusBar();
			this.SuspendLayout();
			// 
			// txtSharpDir
			// 
			this.txtSharpDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtSharpDir.Location = new System.Drawing.Point(136, 24);
			this.txtSharpDir.Name = "txtSharpDir";
			this.txtSharpDir.Size = new System.Drawing.Size(320, 22);
			this.txtSharpDir.TabIndex = 0;
			this.txtSharpDir.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 40);
			this.label1.TabIndex = 1;
			this.label1.Text = "SharpReader Directory";
			// 
			// btnBrowse
			// 
			this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowse.Location = new System.Drawing.Point(464, 24);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.TabIndex = 2;
			this.btnBrowse.Text = "Browse";
			// 
			// btnGo
			// 
			this.btnGo.Location = new System.Drawing.Point(16, 72);
			this.btnGo.Name = "btnGo";
			this.btnGo.TabIndex = 3;
			this.btnGo.Text = "Go";
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// lbDebug
			// 
			this.lbDebug.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lbDebug.ItemHeight = 16;
			this.lbDebug.Location = new System.Drawing.Point(304, 104);
			this.lbDebug.Name = "lbDebug";
			this.lbDebug.Size = new System.Drawing.Size(232, 164);
			this.lbDebug.TabIndex = 4;
			// 
			// tvFeeds
			// 
			this.tvFeeds.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tvFeeds.ImageIndex = -1;
			this.tvFeeds.Location = new System.Drawing.Point(16, 104);
			this.tvFeeds.Name = "tvFeeds";
			this.tvFeeds.SelectedImageIndex = -1;
			this.tvFeeds.Size = new System.Drawing.Size(280, 160);
			this.tvFeeds.TabIndex = 5;
			this.tvFeeds.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvFeeds_AfterSelect);
			// 
			// StatMsg
			// 
			this.StatMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.StatMsg.Dock = System.Windows.Forms.DockStyle.None;
			this.StatMsg.Location = new System.Drawing.Point(0, 280);
			this.StatMsg.Name = "StatMsg";
			this.StatMsg.Size = new System.Drawing.Size(544, 24);
			this.StatMsg.TabIndex = 6;
			// 
			// SharpLog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(544, 304);
			this.Controls.Add(this.StatMsg);
			this.Controls.Add(this.tvFeeds);
			this.Controls.Add(this.lbDebug);
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtSharpDir);
			this.Name = "SharpLog";
			this.Text = "SharpLog";
			this.ResumeLayout(false);

		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new SharpLog());
		}
		#endregion
	}
}
