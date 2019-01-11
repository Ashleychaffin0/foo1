// Copyright (c) 2006 by Larry Smith

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace LRSS {

	public delegate void UIIdleRoutine();

	/// <summary>
	/// Monitors the mouse and keyboard so that a program doesn't try to update the
	/// display while the user's trying to actually *do* something.
	/// </summary>
	class UIMonitor {

		public static DateTime	LastUserInteraction;

		private const int		DefaultMinIdleTime = 3000;	// Arbitrary
		private int				_MinIdleTime;				// In milliseconds

		Timer					timer;						// System.Windows.Forms.Timer
		UIIdleRoutine			IdleRoutine;

//---------------------------------------------------------------------------------------

		public int MinIdleTime {
			get { return _MinIdleTime; }
			set { _MinIdleTime = value; }
		}

//---------------------------------------------------------------------------------------

		public UIMonitor() :
			this(DefaultMinIdleTime) {
		}

//---------------------------------------------------------------------------------------

		public UIMonitor(int MinIdleTime) {
			this.MinIdleTime = MinIdleTime;
			LastUserInteraction = DateTime.Now;
			Application.AddMessageFilter(new UIMonitorMessageFilter());
			timer = new Timer();
			timer.Tick += new EventHandler(timer_Tick);
		}

//---------------------------------------------------------------------------------------

		public bool IsUIIdle() {
			// Note: The problem here is that if we return <false>, the user a) doesn't
			//		 know how much longer to wait, b) has to do the timer wait himself,
			//		 and c) doesn't know if, while he's waiting, whether the user has
			//		 been busy (although for this last point, he can call this routine
			//		 again). What we really need is something like "Here's a delegate.
			//		 Call it either immediately (if the user is idle), or when the user
			//		 has indeed been idle long enough (taking into account intermediate
			//		 mouse/keyboard activity)." So that's why we wrote the
			//		 CallWhenUIBecomesIdle() method.

			// Note: In some ways, this routine would be more useful if it returned
			//		 the time remaining before the UI is considered idle. But if would
			//		 be harder to use. So we'll live with the small overhead hit of
			//		 recalculating the time remaining if/when we need it (e.g. in the
			//		 CallWhenUIBecomesIdle routine.

			TimeSpan HowLong = DateTime.Now - LastUserInteraction;
			return HowLong.TotalMilliseconds > MinIdleTime;
		}

//---------------------------------------------------------------------------------------

		public void CallWhenUIBecomesIdle(UIIdleRoutine routine) {
			// Note: This is the Call *When* UI Becomes Idle routine. It fires exactly
			//		 once. If the user wants to poll, he can do it himself, calling this
			//		 routine every <n> seconds or so.
			if (IsUIIdle()) {
				routine();
				return;
			}
			IdleRoutine = routine;

			StartTiming();
		}

//---------------------------------------------------------------------------------------

		void timer_Tick(object sender, EventArgs e) {
			timer.Stop();			// We may restart later, but for now...
#if false
			string msg = string.Format("{0} Timer tick in UIMonitor", DateTime.Now);
			Debug.Print(msg);
#endif
			if (IsUIIdle()) {
				IdleRoutine();
				return;
			}
			StartTiming();
		}

//---------------------------------------------------------------------------------------

		private void StartTiming() {
			TimeSpan HowLong = DateTime.Now - LastUserInteraction;
			int erval = (int)HowLong.TotalMilliseconds;
			if (erval == 0)			// If we're called right away, delay a nominal 1 sec
				erval = 1000;
			timer.Interval = erval;
			timer.Start();
		}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------


		private class UIMonitorMessageFilter : IMessageFilter {
			#region IMessageFilter Members

//---------------------------------------------------------------------------------------

			public bool PreFilterMessage(ref Message m) {
				// Check for either the mouse moving or a key being depressed (poor, sad
				// little key...), or the user scrolling with the mouse wheel. Note that
				// we do *not* check for a button being pressed (or ironed). We've gone
				// from two buttons to (currently) five buttons, and who knows how many
				// more in the future. So rather than checking for a click, we'll just
				// assume that any click is (realistically) probably preceded immediately
				// by a mouse move.
				if (   (m.Msg == (int)WM_Messages.WM_MOUSEMOVE)
					|| (m.Msg == (int)WM_Messages.WM_MOUSEWHEEL)
					|| (m.Msg == (int)WM_Messages.WM_KEYDOWN)) {
					LastUserInteraction = DateTime.Now;
#if false	// We're pretty sure things are working OK, so comment out this code.
					// TODO: The following code is mostly for MOUSEMOVE. I haven't
					//		 checked to see if LParam holds for the others (especially
					//		 KEYDOWN!). But hey, this is only preliminary debug code to
					//		 make sure we're intercepting things properly.
					uint	x, y;
					x = (uint)m.LParam / 0xffffu;
					y = (uint)m.LParam & 0xffffu;
					string s = string.Format("{4}: Msg = {3} -- Mouse at ({0},{1}) - hWnd = {2:X8}", 
						x, y, (int)m.HWnd, (WM_Messages)m.Msg, LastUserInteraction);
					Debug.WriteLine(s);
#endif
				}
				return false;		// Let other users see this message
			}
			#endregion
		}
	}
}
