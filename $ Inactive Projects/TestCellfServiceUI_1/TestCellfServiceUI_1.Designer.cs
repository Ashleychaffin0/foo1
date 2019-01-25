namespace TestCellfServiceUI_1 {
	partial class TestCellfServiceUI_1 {
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
			this.label1 = new System.Windows.Forms.Label();
			this.lbQuestions = new System.Windows.Forms.ListBox();
			this.lbreplies = new System.Windows.Forms.ListBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.btnQuestionAdd = new System.Windows.Forms.Button();
			this.btnRepliesAdd = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.txtQuestionName = new System.Windows.Forms.TextBox();
			this.txtQuestionText = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.btnQuestionEdit = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.button5 = new System.Windows.Forms.Button();
			this.button6 = new System.Windows.Forms.Button();
			this.button7 = new System.Windows.Forms.Button();
			this.lvQuestions = new System.Windows.Forms.ListView();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 17);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(54, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Questions";
			// 
			// lbQuestions
			// 
			this.lbQuestions.FormattingEnabled = true;
			this.lbQuestions.Location = new System.Drawing.Point(12, 43);
			this.lbQuestions.Name = "lbQuestions";
			this.lbQuestions.Size = new System.Drawing.Size(204, 173);
			this.lbQuestions.TabIndex = 1;
			// 
			// lbreplies
			// 
			this.lbreplies.FormattingEnabled = true;
			this.lbreplies.Location = new System.Drawing.Point(310, 43);
			this.lbreplies.Name = "lbreplies";
			this.lbreplies.Size = new System.Drawing.Size(204, 173);
			this.lbreplies.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(307, 17);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(42, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Replies";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 251);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(99, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Question Properties";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(307, 251);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(84, 13);
			this.label4.TabIndex = 5;
			this.label4.Text = "Reply Properties";
			// 
			// btnQuestionAdd
			// 
			this.btnQuestionAdd.Location = new System.Drawing.Point(222, 53);
			this.btnQuestionAdd.Name = "btnQuestionAdd";
			this.btnQuestionAdd.Size = new System.Drawing.Size(82, 22);
			this.btnQuestionAdd.TabIndex = 6;
			this.btnQuestionAdd.Text = "Add";
			this.btnQuestionAdd.UseVisualStyleBackColor = true;
			// 
			// btnRepliesAdd
			// 
			this.btnRepliesAdd.Location = new System.Drawing.Point(531, 53);
			this.btnRepliesAdd.Name = "btnRepliesAdd";
			this.btnRepliesAdd.Size = new System.Drawing.Size(82, 22);
			this.btnRepliesAdd.TabIndex = 7;
			this.btnRepliesAdd.Text = "Add";
			this.btnRepliesAdd.UseVisualStyleBackColor = true;
			this.btnRepliesAdd.Click += new System.EventHandler(this.btnRepliesAdd_Click);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(12, 282);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(35, 13);
			this.label5.TabIndex = 8;
			this.label5.Text = "Name";
			// 
			// txtQuestionName
			// 
			this.txtQuestionName.Location = new System.Drawing.Point(67, 282);
			this.txtQuestionName.Name = "txtQuestionName";
			this.txtQuestionName.Size = new System.Drawing.Size(148, 20);
			this.txtQuestionName.TabIndex = 9;
			// 
			// txtQuestionText
			// 
			this.txtQuestionText.Location = new System.Drawing.Point(67, 323);
			this.txtQuestionText.Multiline = true;
			this.txtQuestionText.Name = "txtQuestionText";
			this.txtQuestionText.Size = new System.Drawing.Size(148, 72);
			this.txtQuestionText.TabIndex = 11;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(12, 323);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(28, 13);
			this.label6.TabIndex = 10;
			this.label6.Text = "Text";
			// 
			// btnQuestionEdit
			// 
			this.btnQuestionEdit.Location = new System.Drawing.Point(222, 81);
			this.btnQuestionEdit.Name = "btnQuestionEdit";
			this.btnQuestionEdit.Size = new System.Drawing.Size(82, 22);
			this.btnQuestionEdit.TabIndex = 12;
			this.btnQuestionEdit.Text = "Edit";
			this.btnQuestionEdit.UseVisualStyleBackColor = true;
			this.btnQuestionEdit.Click += new System.EventHandler(this.btnQuestionEdit_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(222, 109);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(82, 22);
			this.button1.TabIndex = 13;
			this.button1.Text = "Remove";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(222, 165);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(82, 22);
			this.button2.TabIndex = 15;
			this.button2.Text = "Move Down";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(222, 137);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(82, 22);
			this.button3.TabIndex = 14;
			this.button3.Text = "Move Up";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(531, 165);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(82, 22);
			this.button4.TabIndex = 19;
			this.button4.Text = "Move Down";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(531, 137);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(82, 22);
			this.button5.TabIndex = 18;
			this.button5.Text = "Move Up";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new System.EventHandler(this.button5_Click);
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(531, 109);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(82, 22);
			this.button6.TabIndex = 17;
			this.button6.Text = "Remove";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new System.EventHandler(this.button6_Click);
			// 
			// button7
			// 
			this.button7.Location = new System.Drawing.Point(531, 81);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(82, 22);
			this.button7.TabIndex = 16;
			this.button7.Text = "Edit";
			this.button7.UseVisualStyleBackColor = true;
			this.button7.Click += new System.EventHandler(this.button7_Click);
			// 
			// lvQuestions
			// 
			this.lvQuestions.Location = new System.Drawing.Point(301, 301);
			this.lvQuestions.Name = "lvQuestions";
			this.lvQuestions.Size = new System.Drawing.Size(253, 173);
			this.lvQuestions.TabIndex = 20;
			this.lvQuestions.UseCompatibleStateImageBehavior = false;
			// 
			// TestCellfServiceUI_1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(747, 548);
			this.Controls.Add(this.lvQuestions);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.button6);
			this.Controls.Add(this.button7);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.btnQuestionEdit);
			this.Controls.Add(this.txtQuestionText);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.txtQuestionName);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.btnRepliesAdd);
			this.Controls.Add(this.btnQuestionAdd);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lbreplies);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lbQuestions);
			this.Controls.Add(this.label1);
			this.Name = "TestCellfServiceUI_1";
			this.Text = "Cellf Service";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox lbQuestions;
		private System.Windows.Forms.ListBox lbreplies;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnQuestionAdd;
		private System.Windows.Forms.Button btnRepliesAdd;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtQuestionName;
		private System.Windows.Forms.TextBox txtQuestionText;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button btnQuestionEdit;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Button button7;
		private System.Windows.Forms.ListView lvQuestions;
	}
}

