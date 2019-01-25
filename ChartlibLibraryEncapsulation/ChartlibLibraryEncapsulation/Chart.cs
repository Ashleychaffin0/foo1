using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Bartizan.Charting {
	public class BasicChart<T> where T: DundasChart {
		public Size		ChartSize;
		public Point	Location;
		private string	_Caption;

		private T		Chart;

		public string Caption {
			get { return _Caption; }
			set { _Caption = value; 
				// Call routine to set Caption, e.g.
				Chart.SetCaption(_Caption);
			}
		}

		public abstract void Draw();

		// TODO: Add dtor, Dispose, etc
	}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class PieChart<T> : BasicChart<T> {
		public bool			Exploded;
		public float []		Data;
		public string []	Labels;
		public Color []		Colors;

//---------------------------------------------------------------------------------------

		public virtual void Draw() {
			// TODO:
		}
	}


//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	public class BarChart<T> : BasicChart<T> {
		public float[,] Data;
		public string[] Labels;
		public Color[] Colors;

		public bool Horizontal;			// If true, Horizontal, else Vertical

//---------------------------------------------------------------------------------------

		public virtual void Draw() {
			// TODO:
		}
	}

}
