using System;
using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.Document;

namespace WindowsApplication1
{
	public class ActiveReport1 : ActiveReport
	{
		public ActiveReport1()
		{
			InitializeReport();
		}

		#region ActiveReports Designer generated code
		private void InitializeReport()
		{
			this.LoadLayout(this.GetType(),"ActiveReport1.rpx");
		}
		#endregion
	}
}
