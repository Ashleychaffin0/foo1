using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using LRS.Utils;

namespace CoreDownloadMsConferences {
	public class FlowManager {
		FlowLayoutPanel Flower;
		int				SeqNo;

//---------------------------------------------------------------------------------------

		public FlowManager(FlowLayoutPanel panel) {
			Flower = panel;
			SeqNo  = 0;
		}

//---------------------------------------------------------------------------------------

		public void Clear() {
			Flower.Controls.Clear();
		}

//---------------------------------------------------------------------------------------

		public void Add(DownloadFileProgress ctl) {
			// twc.Progress = new DownloadFileProgress(twc);	// pass in twc.Progress
			// Add a new custom progress control to our Flow Layout Panel
			lock (Flower) {
				Flower.SuspendLayout();
				int n = Flower.Controls.Count;   // Note: next control will be [n]
				Flower.Controls.Add(ctl);
				// Flower.Controls.SetChildIndex(ctl, 0);
				var OurControl = Flower.Controls[n];
				OurControl.Name = $"X{SeqNo++:D6}";
				Flower.ResumeLayout();
			}
		}

//---------------------------------------------------------------------------------------

		public void SortFlowPanel() {
			try {
				var ctls = Flower.Controls;
				var dict = new Dictionary<string, DownloadFileProgress>();
				for (int i = 0; i < ctls.Count; i++) {
					var ctl = ctls[i] as DownloadFileProgress;
					dict[ctl.twc.Title] = ctl;
				}

				Flower.SuspendLayout();
				Flower.Controls.Clear();
				var keys = dict.Keys.OrderBy(key => key);
				foreach (var item in keys) {
					Flower.Controls.Add(dict[item]);
				}
				Flower.ResumeLayout();
			} catch (Exception ex) {
				Debugger.Break();       // TODO:
			}
		}

//---------------------------------------------------------------------------------------

		public void SetToolTip(ToolTip tip, string text) {
			tip.SetToolTip(Flower, text);
		}

#if true

		// TODO: Integrate MoveCompletedDownloadsToEndOfList.
		// TODO: See also DLPILects::Lecture.cs

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Go through all items in the FlowLayoutPanel. Move all that are flagged as
		/// done to the end of the panel, thus keeping the active downloads visible
		/// at the front of the list.
		/// </summary>
		public void MoveCompletedDownloadsToEndOfList() {
			lock (Flower) {
				int n = Flower.Controls.Count;
				Flower.SuspendLayout();
				foreach (DownloadFileProgress ctl in Flower.Controls) {
					var prog = ctl.twc.Progress;
					if (prog.IsDownloadDone) {
						// Move to end
						// Flower.Controls.SetChildIndex(lect.OurControl, n - 1);
						Flower.Controls.SetChildIndex(ctl, n - 1);
					}
				}
				Flower.ResumeLayout();
			}
			Application.DoEvents();
		}
#endif

	}
}
