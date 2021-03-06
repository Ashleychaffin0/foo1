namespace LRSAntlr {
	partial class LRSAntlr {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.label1 = new System.Windows.Forms.Label();
			this.TxtFolderName = new System.Windows.Forms.TextBox();
			this.BtnBrowseDir = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.CmbGrammar = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.RadGUI = new System.Windows.Forms.RadioButton();
			this.RadConsole = new System.Windows.Forms.RadioButton();
			this.ChkListener = new System.Windows.Forms.CheckBox();
			this.ChkVisitor = new System.Windows.Forms.CheckBox();
			this.BtnGo = new System.Windows.Forms.Button();
			this.RadDll = new System.Windows.Forms.RadioButton();
			this.LbMsgs = new System.Windows.Forms.ListBox();
			this.ChkCompileAsJava = new System.Windows.Forms.CheckBox();
			this.ChkCompileAsCSharp = new System.Windows.Forms.CheckBox();
			this.ChkCompileAsPython3 = new System.Windows.Forms.CheckBox();
			this.ChkCompileAsCpp = new System.Windows.Forms.CheckBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.ChkCompileAsPython2 = new System.Windows.Forms.CheckBox();
			this.ChkCompileAsSwift = new System.Windows.Forms.CheckBox();
			this.ChkCompileAsGo = new System.Windows.Forms.CheckBox();
			this.ChkCompileAsJavaScript = new System.Windows.Forms.CheckBox();
			this.lblGrunOptions = new System.Windows.Forms.Label();
			this.RadGrunTokens = new System.Windows.Forms.RadioButton();
			this.RadGrunTree = new System.Windows.Forms.RadioButton();
			this.RadGrunGui = new System.Windows.Forms.RadioButton();
			this.RadGrunTrace = new System.Windows.Forms.RadioButton();
			this.RadGrunDiagnostics = new System.Windows.Forms.RadioButton();
			this.TxtGrunStartRuleName = new System.Windows.Forms.TextBox();
			this.LblGrunStartRuleName = new System.Windows.Forms.Label();
			this.BtnEdit = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(29, 30);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(84, 29);
			this.label1.TabIndex = 0;
			this.label1.Text = "Folder";
			// 
			// TxtFolderName
			// 
			this.TxtFolderName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TxtFolderName.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TxtFolderName.Location = new System.Drawing.Point(152, 27);
			this.TxtFolderName.Name = "TxtFolderName";
			this.TxtFolderName.Size = new System.Drawing.Size(1011, 34);
			this.TxtFolderName.TabIndex = 1;
			this.TxtFolderName.TextChanged += new System.EventHandler(this.TxtFolderName_TextChanged);
			// 
			// BtnBrowseDir
			// 
			this.BtnBrowseDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnBrowseDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.BtnBrowseDir.Location = new System.Drawing.Point(1194, 25);
			this.BtnBrowseDir.Name = "BtnBrowseDir";
			this.BtnBrowseDir.Size = new System.Drawing.Size(114, 34);
			this.BtnBrowseDir.TabIndex = 2;
			this.BtnBrowseDir.Text = "Browse";
			this.BtnBrowseDir.UseVisualStyleBackColor = true;
			this.BtnBrowseDir.Click += new System.EventHandler(this.BtnBrowseDir_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(29, 87);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(113, 29);
			this.label2.TabIndex = 3;
			this.label2.Text = "Grammar";
			// 
			// CmbGrammar
			// 
			this.CmbGrammar.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CmbGrammar.FormattingEnabled = true;
			this.CmbGrammar.Location = new System.Drawing.Point(152, 84);
			this.CmbGrammar.Name = "CmbGrammar";
			this.CmbGrammar.Size = new System.Drawing.Size(222, 37);
			this.CmbGrammar.TabIndex = 4;
			this.CmbGrammar.SelectedIndexChanged += new System.EventHandler(this.CmbGrammar_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(29, 133);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(97, 29);
			this.label3.TabIndex = 5;
			this.label3.Text = "Options";
			// 
			// RadGUI
			// 
			this.RadGUI.AutoSize = true;
			this.RadGUI.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.RadGUI.Location = new System.Drawing.Point(152, 131);
			this.RadGUI.Name = "RadGUI";
			this.RadGUI.Size = new System.Drawing.Size(75, 33);
			this.RadGUI.TabIndex = 6;
			this.RadGUI.TabStop = true;
			this.RadGUI.Text = "GUI";
			this.RadGUI.UseVisualStyleBackColor = true;
			// 
			// RadConsole
			// 
			this.RadConsole.AutoSize = true;
			this.RadConsole.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.RadConsole.Location = new System.Drawing.Point(260, 131);
			this.RadConsole.Name = "RadConsole";
			this.RadConsole.Size = new System.Drawing.Size(124, 33);
			this.RadConsole.TabIndex = 7;
			this.RadConsole.TabStop = true;
			this.RadConsole.Text = "Console";
			this.RadConsole.UseVisualStyleBackColor = true;
			// 
			// ChkListener
			// 
			this.ChkListener.AutoSize = true;
			this.ChkListener.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ChkListener.Location = new System.Drawing.Point(152, 175);
			this.ChkListener.Name = "ChkListener";
			this.ChkListener.Size = new System.Drawing.Size(121, 33);
			this.ChkListener.TabIndex = 8;
			this.ChkListener.Text = "Listener";
			this.ChkListener.UseVisualStyleBackColor = true;
			this.ChkListener.CheckedChanged += new System.EventHandler(this.ChkListener_CheckedChanged);
			// 
			// ChkVisitor
			// 
			this.ChkVisitor.AutoSize = true;
			this.ChkVisitor.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ChkVisitor.Location = new System.Drawing.Point(300, 175);
			this.ChkVisitor.Name = "ChkVisitor";
			this.ChkVisitor.Size = new System.Drawing.Size(102, 33);
			this.ChkVisitor.TabIndex = 9;
			this.ChkVisitor.Text = "Visitor";
			this.ChkVisitor.UseVisualStyleBackColor = true;
			this.ChkVisitor.CheckedChanged += new System.EventHandler(this.ChkVisitor_CheckedChanged);
			// 
			// BtnGo
			// 
			this.BtnGo.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.BtnGo.Location = new System.Drawing.Point(29, 399);
			this.BtnGo.Name = "BtnGo";
			this.BtnGo.Size = new System.Drawing.Size(85, 42);
			this.BtnGo.TabIndex = 10;
			this.BtnGo.Text = "Go";
			this.BtnGo.UseVisualStyleBackColor = true;
			this.BtnGo.Click += new System.EventHandler(this.BtnGo_Click);
			// 
			// RadDll
			// 
			this.RadDll.AutoSize = true;
			this.RadDll.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.RadDll.Location = new System.Drawing.Point(417, 131);
			this.RadDll.Name = "RadDll";
			this.RadDll.Size = new System.Drawing.Size(206, 33);
			this.RadDll.TabIndex = 11;
			this.RadDll.TabStop = true;
			this.RadDll.Text = "DLL??? (Nonce)";
			this.RadDll.UseVisualStyleBackColor = true;
			// 
			// LbMsgs
			// 
			this.LbMsgs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LbMsgs.Font = new System.Drawing.Font("Courier New", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.LbMsgs.FormattingEnabled = true;
			this.LbMsgs.HorizontalScrollbar = true;
			this.LbMsgs.ItemHeight = 25;
			this.LbMsgs.Location = new System.Drawing.Point(29, 467);
			this.LbMsgs.Name = "LbMsgs";
			this.LbMsgs.Size = new System.Drawing.Size(1312, 229);
			this.LbMsgs.TabIndex = 12;
			this.LbMsgs.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LbMsgs_MouseMove);
			// 
			// ChkCompileAsJava
			// 
			this.ChkCompileAsJava.AutoSize = true;
			this.ChkCompileAsJava.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ChkCompileAsJava.Location = new System.Drawing.Point(216, 227);
			this.ChkCompileAsJava.Name = "ChkCompileAsJava";
			this.ChkCompileAsJava.Size = new System.Drawing.Size(84, 33);
			this.ChkCompileAsJava.TabIndex = 14;
			this.ChkCompileAsJava.Text = "Java";
			this.ChkCompileAsJava.UseVisualStyleBackColor = true;
			this.ChkCompileAsJava.CheckedChanged += new System.EventHandler(this.ChkCompileAsJava_CheckedChanged);
			// 
			// ChkCompileAsCSharp
			// 
			this.ChkCompileAsCSharp.AutoSize = true;
			this.ChkCompileAsCSharp.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ChkCompileAsCSharp.Location = new System.Drawing.Point(151, 227);
			this.ChkCompileAsCSharp.Name = "ChkCompileAsCSharp";
			this.ChkCompileAsCSharp.Size = new System.Drawing.Size(65, 33);
			this.ChkCompileAsCSharp.TabIndex = 13;
			this.ChkCompileAsCSharp.Text = "C#";
			this.ChkCompileAsCSharp.UseVisualStyleBackColor = true;
			this.ChkCompileAsCSharp.CheckedChanged += new System.EventHandler(this.ChkCompileAsCSharp_CheckedChanged);
			// 
			// ChkCompileAsPython3
			// 
			this.ChkCompileAsPython3.AutoSize = true;
			this.ChkCompileAsPython3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ChkCompileAsPython3.Location = new System.Drawing.Point(300, 227);
			this.ChkCompileAsPython3.Name = "ChkCompileAsPython3";
			this.ChkCompileAsPython3.Size = new System.Drawing.Size(127, 33);
			this.ChkCompileAsPython3.TabIndex = 15;
			this.ChkCompileAsPython3.Text = "Python 3";
			this.ChkCompileAsPython3.UseVisualStyleBackColor = true;
			this.ChkCompileAsPython3.CheckedChanged += new System.EventHandler(this.ChkCompileAsPython3_CheckedChanged);
			// 
			// ChkCompileAsCpp
			// 
			this.ChkCompileAsCpp.AutoSize = true;
			this.ChkCompileAsCpp.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ChkCompileAsCpp.Location = new System.Drawing.Point(554, 227);
			this.ChkCompileAsCpp.Name = "ChkCompileAsCpp";
			this.ChkCompileAsCpp.Size = new System.Drawing.Size(80, 33);
			this.ChkCompileAsCpp.TabIndex = 16;
			this.ChkCompileAsCpp.Text = "C++";
			this.ChkCompileAsCpp.UseVisualStyleBackColor = true;
			this.ChkCompileAsCpp.CheckedChanged += new System.EventHandler(this.ChkCompileAsCpp_CheckedChanged);
			// 
			// ChkCompileAsPython2
			// 
			this.ChkCompileAsPython2.AutoSize = true;
			this.ChkCompileAsPython2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ChkCompileAsPython2.Location = new System.Drawing.Point(427, 227);
			this.ChkCompileAsPython2.Name = "ChkCompileAsPython2";
			this.ChkCompileAsPython2.Size = new System.Drawing.Size(127, 33);
			this.ChkCompileAsPython2.TabIndex = 17;
			this.ChkCompileAsPython2.Text = "Python 2";
			this.ChkCompileAsPython2.UseVisualStyleBackColor = true;
			this.ChkCompileAsPython2.CheckedChanged += new System.EventHandler(this.ChkCompileAsPython2_CheckedChanged);
			// 
			// ChkCompileAsSwift
			// 
			this.ChkCompileAsSwift.AutoSize = true;
			this.ChkCompileAsSwift.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ChkCompileAsSwift.Location = new System.Drawing.Point(847, 227);
			this.ChkCompileAsSwift.Name = "ChkCompileAsSwift";
			this.ChkCompileAsSwift.Size = new System.Drawing.Size(87, 33);
			this.ChkCompileAsSwift.TabIndex = 18;
			this.ChkCompileAsSwift.Text = "Swift";
			this.ChkCompileAsSwift.UseVisualStyleBackColor = true;
			this.ChkCompileAsSwift.CheckedChanged += new System.EventHandler(this.ChkCompileAsSwift_CheckedChanged);
			// 
			// ChkCompileAsGo
			// 
			this.ChkCompileAsGo.AutoSize = true;
			this.ChkCompileAsGo.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ChkCompileAsGo.Location = new System.Drawing.Point(780, 227);
			this.ChkCompileAsGo.Name = "ChkCompileAsGo";
			this.ChkCompileAsGo.Size = new System.Drawing.Size(67, 33);
			this.ChkCompileAsGo.TabIndex = 19;
			this.ChkCompileAsGo.Text = "Go";
			this.ChkCompileAsGo.UseVisualStyleBackColor = true;
			this.ChkCompileAsGo.CheckedChanged += new System.EventHandler(this.ChkCompileAsGo_CheckedChanged);
			// 
			// ChkCompileAsJavaScript
			// 
			this.ChkCompileAsJavaScript.AutoSize = true;
			this.ChkCompileAsJavaScript.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ChkCompileAsJavaScript.Location = new System.Drawing.Point(634, 227);
			this.ChkCompileAsJavaScript.Name = "ChkCompileAsJavaScript";
			this.ChkCompileAsJavaScript.Size = new System.Drawing.Size(146, 33);
			this.ChkCompileAsJavaScript.TabIndex = 20;
			this.ChkCompileAsJavaScript.Text = "JavaScript";
			this.ChkCompileAsJavaScript.UseVisualStyleBackColor = true;
			this.ChkCompileAsJavaScript.CheckedChanged += new System.EventHandler(this.ChkCompileAsJavaScript_CheckedChanged);
			// 
			// lblGrunOptions
			// 
			this.lblGrunOptions.AutoSize = true;
			this.lblGrunOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblGrunOptions.Location = new System.Drawing.Point(29, 351);
			this.lblGrunOptions.Name = "lblGrunOptions";
			this.lblGrunOptions.Size = new System.Drawing.Size(156, 29);
			this.lblGrunOptions.TabIndex = 21;
			this.lblGrunOptions.Text = "Grun options:";
			// 
			// RadGrunTokens
			// 
			this.RadGrunTokens.AutoSize = true;
			this.RadGrunTokens.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.RadGrunTokens.Location = new System.Drawing.Point(203, 349);
			this.RadGrunTokens.Name = "RadGrunTokens";
			this.RadGrunTokens.Size = new System.Drawing.Size(115, 33);
			this.RadGrunTokens.TabIndex = 22;
			this.RadGrunTokens.TabStop = true;
			this.RadGrunTokens.Text = "Tokens";
			this.RadGrunTokens.UseVisualStyleBackColor = true;
			// 
			// RadGrunTree
			// 
			this.RadGrunTree.AutoSize = true;
			this.RadGrunTree.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.RadGrunTree.Location = new System.Drawing.Point(320, 349);
			this.RadGrunTree.Name = "RadGrunTree";
			this.RadGrunTree.Size = new System.Drawing.Size(86, 33);
			this.RadGrunTree.TabIndex = 23;
			this.RadGrunTree.TabStop = true;
			this.RadGrunTree.Text = "Tree";
			this.RadGrunTree.UseVisualStyleBackColor = true;
			// 
			// RadGrunGui
			// 
			this.RadGrunGui.AutoSize = true;
			this.RadGrunGui.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.RadGrunGui.Location = new System.Drawing.Point(408, 349);
			this.RadGrunGui.Name = "RadGrunGui";
			this.RadGrunGui.Size = new System.Drawing.Size(75, 33);
			this.RadGrunGui.TabIndex = 24;
			this.RadGrunGui.TabStop = true;
			this.RadGrunGui.Text = "GUI";
			this.RadGrunGui.UseVisualStyleBackColor = true;
			// 
			// RadGrunTrace
			// 
			this.RadGrunTrace.AutoSize = true;
			this.RadGrunTrace.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.RadGrunTrace.Location = new System.Drawing.Point(485, 349);
			this.RadGrunTrace.Name = "RadGrunTrace";
			this.RadGrunTrace.Size = new System.Drawing.Size(97, 33);
			this.RadGrunTrace.TabIndex = 25;
			this.RadGrunTrace.TabStop = true;
			this.RadGrunTrace.Text = "Trace";
			this.RadGrunTrace.UseVisualStyleBackColor = true;
			// 
			// RadGrunDiagnostics
			// 
			this.RadGrunDiagnostics.AutoSize = true;
			this.RadGrunDiagnostics.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.RadGrunDiagnostics.Location = new System.Drawing.Point(584, 349);
			this.RadGrunDiagnostics.Name = "RadGrunDiagnostics";
			this.RadGrunDiagnostics.Size = new System.Drawing.Size(159, 33);
			this.RadGrunDiagnostics.TabIndex = 26;
			this.RadGrunDiagnostics.TabStop = true;
			this.RadGrunDiagnostics.Text = "Diagnostics";
			this.RadGrunDiagnostics.UseVisualStyleBackColor = true;
			// 
			// TxtGrunStartRuleName
			// 
			this.TxtGrunStartRuleName.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TxtGrunStartRuleName.Location = new System.Drawing.Point(263, 283);
			this.TxtGrunStartRuleName.Name = "TxtGrunStartRuleName";
			this.TxtGrunStartRuleName.Size = new System.Drawing.Size(265, 34);
			this.TxtGrunStartRuleName.TabIndex = 27;
			// 
			// LblGrunStartRuleName
			// 
			this.LblGrunStartRuleName.AutoSize = true;
			this.LblGrunStartRuleName.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.LblGrunStartRuleName.Location = new System.Drawing.Point(24, 283);
			this.LblGrunStartRuleName.Name = "LblGrunStartRuleName";
			this.LblGrunStartRuleName.Size = new System.Drawing.Size(189, 29);
			this.LblGrunStartRuleName.TabIndex = 28;
			this.LblGrunStartRuleName.Text = "Start Rule Name";
			// 
			// BtnEdit
			// 
			this.BtnEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.BtnEdit.Location = new System.Drawing.Point(455, 79);
			this.BtnEdit.Name = "BtnEdit";
			this.BtnEdit.Size = new System.Drawing.Size(85, 42);
			this.BtnEdit.TabIndex = 29;
			this.BtnEdit.Text = "Edit";
			this.BtnEdit.UseVisualStyleBackColor = true;
			this.BtnEdit.Click += new System.EventHandler(this.BtnEdit_Click);
			// 
			// LRSAntlr
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1353, 757);
			this.Controls.Add(this.BtnEdit);
			this.Controls.Add(this.LblGrunStartRuleName);
			this.Controls.Add(this.TxtGrunStartRuleName);
			this.Controls.Add(this.RadGrunDiagnostics);
			this.Controls.Add(this.RadGrunTrace);
			this.Controls.Add(this.RadGrunGui);
			this.Controls.Add(this.RadGrunTree);
			this.Controls.Add(this.RadGrunTokens);
			this.Controls.Add(this.lblGrunOptions);
			this.Controls.Add(this.ChkCompileAsJavaScript);
			this.Controls.Add(this.ChkCompileAsGo);
			this.Controls.Add(this.ChkCompileAsSwift);
			this.Controls.Add(this.ChkCompileAsPython2);
			this.Controls.Add(this.ChkCompileAsCpp);
			this.Controls.Add(this.ChkCompileAsPython3);
			this.Controls.Add(this.ChkCompileAsJava);
			this.Controls.Add(this.ChkCompileAsCSharp);
			this.Controls.Add(this.LbMsgs);
			this.Controls.Add(this.RadDll);
			this.Controls.Add(this.BtnGo);
			this.Controls.Add(this.ChkVisitor);
			this.Controls.Add(this.ChkListener);
			this.Controls.Add(this.RadConsole);
			this.Controls.Add(this.RadGUI);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.CmbGrammar);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.BtnBrowseDir);
			this.Controls.Add(this.TxtFolderName);
			this.Controls.Add(this.label1);
			this.Name = "LRSAntlr";
			this.Text = "LRS Antlr Interface";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox TxtFolderName;
		private System.Windows.Forms.Button BtnBrowseDir;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox CmbGrammar;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.RadioButton RadGUI;
		private System.Windows.Forms.RadioButton RadConsole;
		private System.Windows.Forms.CheckBox ChkListener;
		private System.Windows.Forms.CheckBox ChkVisitor;
		private System.Windows.Forms.Button BtnGo;
		private System.Windows.Forms.RadioButton RadDll;
		private System.Windows.Forms.ListBox LbMsgs;
		private System.Windows.Forms.CheckBox ChkCompileAsJava;
		private System.Windows.Forms.CheckBox ChkCompileAsCSharp;
		private System.Windows.Forms.CheckBox ChkCompileAsPython3;
		private System.Windows.Forms.CheckBox ChkCompileAsCpp;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.CheckBox ChkCompileAsPython2;
		private System.Windows.Forms.CheckBox ChkCompileAsSwift;
		private System.Windows.Forms.CheckBox ChkCompileAsGo;
		private System.Windows.Forms.CheckBox ChkCompileAsJavaScript;
		private System.Windows.Forms.Label lblGrunOptions;
		private System.Windows.Forms.RadioButton RadGrunTokens;
		private System.Windows.Forms.RadioButton RadGrunTree;
		private System.Windows.Forms.RadioButton RadGrunGui;
		private System.Windows.Forms.RadioButton RadGrunTrace;
		private System.Windows.Forms.RadioButton RadGrunDiagnostics;
		private System.Windows.Forms.TextBox TxtGrunStartRuleName;
		private System.Windows.Forms.Label LblGrunStartRuleName;
		private System.Windows.Forms.Button BtnEdit;
	}
}

