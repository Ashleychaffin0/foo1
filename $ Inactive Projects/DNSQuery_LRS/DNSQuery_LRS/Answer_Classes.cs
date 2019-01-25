using System;

namespace LumiSoft.Net.Dns
{
	#region class A_Record

	/// <summary>
	/// A record class.
	/// </summary>
	public class A_Record
	{
		private string m_IP = "";

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="IP">IP address.</param>
		public A_Record(string IP)
		{
			m_IP = IP;
		}

		#region Properties Implementation

		/// <summary>
		/// Gets mail host dns name.
		/// </summary>
		public string IP
		{
			get{ return m_IP; }
		}

		#endregion

	}

	#endregion

	#region class PTR_Record

	/// <summary>
	/// PTR record class.
	/// </summary>
	public class PTR_Record
	{
		private string m_DomainName = "";

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="domainName">DomainName.</param>
		public PTR_Record(string domainName)
		{
			m_DomainName = domainName;
		}

		#region Properties Implementation

		/// <summary>
		/// Gets domain name.
		/// </summary>
		public string DomainName
		{
			get{ return m_DomainName; }
		}

		#endregion

	}

	#endregion

	#region class MX_Record

	/// <summary>
	/// MX record class.
	/// </summary>
	public class MX_Record
	{
		private int    m_Preference = 0;
		private string m_Host       = "";

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="preference">MX record preference.</param>
		/// <param name="host">Mail host dns name.</param>
		public MX_Record(int preference,string host)
		{
			m_Preference = preference;
			m_Host       = host;
		}

		#region Properties Implementation

		/// <summary>
		/// Gets MX record preference.
		/// </summary>
		public int Preference
		{
			get{ return m_Preference; }
		}

		/// <summary>
		/// Gets mail host dns name.
		/// </summary>
		public string Host
		{
			get{ return m_Host; }
		}

		#endregion

	}

	#endregion
}
