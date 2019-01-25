using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace ClassTimer {

	class LRSTimer {

		SessionSlot	CurrentSlot;	// slot = linked list of sessions starting at a given time

		// ctor
		LRSTimer() {
			// Get current TOD
			// Use that to find first session slot >= now
			WaitUntil(CurrentSlot.TOD);
		}

		void OnTimeChange() {
			Reset();
			ClearCurrentSlot();
			DateTime TOD = GetCurrentTOD();
			CurrentSlot = FindPreviousSlot();
			if (CurrentSlot.TOD > TOD)
				WaitUntil(CurrentSlot);
			else
				ProcessSlot(CurrentSlot);
		}

		void OnWakeup() {
			OnTimeChange();
		}

		void Reset() {
			KillTimer();
		}

		void WaitUntil(DateTime dt) {
		}

		void WaitFor(TimeSpan ts) {
			// Not needed
		}

		void Fire() {
			// Code to execute when timer fires
			ProcessSlot(CurrentSlot);
			CurrentSlot = GetNextSlot(CurrentSlot);
			WaitUntil(CurrentSlot.TOD);
		}
	}







	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.Size = new System.Drawing.Size(300,300);
			this.Text = "Form1";
		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}
	}
}
