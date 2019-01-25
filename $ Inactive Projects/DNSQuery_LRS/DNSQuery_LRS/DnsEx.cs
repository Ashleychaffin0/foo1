using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Text;

namespace LumiSoft.Net.Dns
{
	/// <summary>
	/// Dns reply codes.
	/// </summary>
	public enum DnsReplyCode
	{
		/// <summary>
		/// Requested records retrieved sucessfully.
		/// </summary>
		Ok = 0,

		/// <summary>
		/// No requested records found.
		/// </summary>
		NoEntries = 1,

		/// <summary>
		/// There was error retrieving records.
		/// </summary>
		TempError = 2,
	}

	/// <summary>
	/// Dns resolver.
	/// </summary>
	/// <example>
	/// <code>
	/// // Set dns servers
	/// DnsEx.DnsServers = new string[]{"194.126.115.18"};
	/// 
	/// DnsEx dns = DnsEx();
	/// 
	/// // Get MX records
	/// MX_Record[] mxRecords = dns.GetMXRecords("lumisoft.ee")
	/// 
	/// // Do your stuff
	/// </code>
	/// </example>
	public class DnsEx
	{
		private static string[] m_DnsServers  = null;
		private static bool     m_UseDnsCache = true;
		private static int      m_ID          = 100;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public DnsEx()
		{
		}


		#region method GetARecords

		/// <summary>
		/// Gets A records.
		/// </summary>
		/// <param name="domain">Domain name which A records to get.</param>
		/// <returns></returns>
		public A_Record[] GetARecords(string domain)
		{
			int id = DnsEx.ID;
			ArrayList answers = QueryServer(id,domain,QTYPE.A,1);

			ArrayList aRecords = new ArrayList();
			foreach(object answer in answers){
				if(answer is A_Record){
					aRecords.Add(answer);
				}
			}

			return (A_Record[])aRecords.ToArray(typeof(A_Record));
		}

		#endregion

		#region method GetPTRRecords

		/// <summary>
		/// Gets PTR records.
		/// </summary>
		/// <param name="ip">IP address which domain names to get.</param>
		/// <returns></returns>
		public PTR_Record[] GetPTRRecords(string ip)
		{	
			// See if IP is ok.
			IPAddress.Parse(ip);
		
			string[] ipParts = ip.Split('.');
			ip = "";
			//--- Reverse IP ----------
			for(int i=3;i>-1;i--){
				ip += ipParts[i] + ".";
			}
			ip += "in-addr.arpa";

			int id = DnsEx.ID;
			ArrayList answers = QueryServer(id,ip,QTYPE.PTR,1);

			ArrayList aRecords = new ArrayList();
			foreach(object answer in answers){
				if(answer is PTR_Record){
					aRecords.Add(answer);
				}
			}

			return (PTR_Record[])aRecords.ToArray(typeof(PTR_Record));
		}

		#endregion

		#region method GetMXRecords

		/// <summary>
		/// Gets MX records.(MX records are sorted by preference, lower array element is prefered)
		/// </summary>
		/// <param name="domain"></param>
		/// <returns></returns>
		public MX_Record[] GetMXRecords(string domain)
		{
			MX_Record[] retVal = null;
			GetMXRecords(domain,out retVal);

			return retVal;
		}

		/// <summary>
		/// Gets MX records.(MX records are sorted by preference, lower array element is prefered)
		/// </summary>
		/// <param name="domain"></param>
		/// <param name="mxRecords"></param>
		/// <returns></returns>
		public DnsReplyCode GetMXRecords(string domain,out MX_Record[] mxRecords)
		{
			mxRecords = null;

			int id = DnsEx.ID;
			ArrayList answers = QueryServer(id,domain,QTYPE.MX,1);
			if(answers != null){
				SortedList mx            = new SortedList();
				ArrayList  duplicateList = new ArrayList();
				foreach(object answer in answers){
					if(answer is MX_Record){
						MX_Record mxRec = (MX_Record)answer;

						if(!mx.Contains(mxRec.Preference)){
							mx.Add(mxRec.Preference,mxRec);
						}
						else{
							duplicateList.Add(mxRec);
						}
					}
				}

				mxRecords = new MX_Record[mx.Count + duplicateList.Count];
				mx.Values.CopyTo(mxRecords,0);
				duplicateList.CopyTo(mxRecords,mx.Count);
			}
						
			return DnsReplyCode.TempError;			
		}

		#endregion

		
		#region method ParseARecord

		private A_Record ParseARecord(byte[] reply,ref int offset,int rdLength)
		{
			// IPv4 = byte byte byte byte

			byte[] ip = new byte[rdLength];
			Array.Copy(reply,offset,ip,0,rdLength);

			return new A_Record(ip[0] + "." + ip[1] + "." + ip[2] + "." + ip[3]);	
		}

		#endregion

		#region method ParsePTRRecord

		private PTR_Record ParsePTRRecord(byte[] reply,ref int offset,int rdLength)
		{
			string name = "";
			GetQName(reply,ref offset,ref name);

			return new PTR_Record(name);	
		}

		#endregion
		
		#region method ParseMxRecord

		/// <summary>
		/// Parses MX record.
		/// </summary>
		/// <param name="reply"></param>
		/// <param name="offset"></param>
		/// <returns>Returns null, if failed.</returns>
		private MX_Record ParseMxRecord(byte[] reply,ref int offset)
		{
			/* RFC 1035	3.3.9. MX RDATA format

			+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
			|                  PREFERENCE                   |
			+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
			/                   EXCHANGE                    /
			/                                               /
			+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+

			where:

			PREFERENCE      
				A 16 bit integer which specifies the preference given to
				this RR among others at the same owner.  Lower values
                are preferred.

			EXCHANGE 
			    A <domain-name> which specifies a host willing to act as
                a mail exchange for the owner name. 
			*/

			try{
				int pref = reply[offset++] << 8 | reply[offset++];
		
				string name = "";			
				if(GetQName(reply,ref offset,ref name)){
					return new MX_Record(pref,name);
				}
			}
			catch{
			}

			return null;
		}

		#endregion


		#region method QueryServer

		/// <summary>
		/// Sends query to server.
		/// </summary>
		/// <param name="queryID">Query ID.</param>
		/// <param name="qname">Query text.</param>
		/// <param name="qtype">Query type.</param>
		/// <param name="qclass">Query class.</param>
		/// <returns></returns>
		private ArrayList QueryServer(int queryID,string qname,QTYPE qtype,int qclass)
		{	
			// See if query is in cache
			if(m_UseDnsCache){
				ArrayList entries = DnsCache.GetFromCache(qname,(int)qtype);
				if(entries != null){
					return entries;
				}
			}

			byte[] query = CreateQuery(queryID,qname,qtype,qclass);

			int helper = 0;
			for(int i=0;i<10;i++){				
				if(helper > m_DnsServers.Length){
					helper = 0;
				}

				try{
					IPEndPoint ipRemoteEndPoint = new IPEndPoint(IPAddress.Parse(m_DnsServers[helper]),53);
					Socket udpClient = new Socket(AddressFamily.InterNetwork,SocketType.Dgram,ProtocolType.Udp);
			
					IPEndPoint ipLocalEndPoint = new IPEndPoint(IPAddress.Any,0);
					EndPoint localEndPoint = (EndPoint)ipLocalEndPoint;
					udpClient.Bind(localEndPoint);	
			
					udpClient.Connect(ipRemoteEndPoint);

					//send query
					udpClient.Send(query);

					// Wait until we have a reply
					byte[] retVal = null;
					if(udpClient.Poll(5*1000000,SelectMode.SelectRead)){
						retVal = new byte[512];
						udpClient.Receive(retVal);
					}

					udpClient.Close();

					// If reply is ok, return it
					ArrayList answers = ParseAnswers(retVal,queryID);
					if(answers != null){
						// Cache query
						if(m_UseDnsCache && answers.Count > 0){
							DnsCache.AddToCache(qname,(int)qtype,answers);
						}

						return answers;
					}
				}
				catch{
				}

				helper++;
			}

			return null;
		}

		#endregion

		#region method CreateQuery

		/// <summary>
		/// Creates new query.
		/// </summary>
		/// <param name="ID"></param>
		/// <param name="qname"></param>
		/// <param name="qtype"></param>
		/// <param name="qclass"></param>
		/// <returns></returns>
		private byte[] CreateQuery(int ID,string qname,QTYPE qtype,int qclass)
		{
			byte[] query = new byte[521];

			//---- Create header --------------------------------------------//
			// Header is first 12 bytes of query

			/* 4.1.1. Header section format
										  1  1  1  1  1  1
			0  1  2  3  4  5  6  7  8  9  0  1  2  3  4  5
			+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
			|                      ID                       |
			+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
			|QR|   Opcode  |AA|TC|RD|RA|   Z    |   RCODE   |
			+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
			|                    QDCOUNT                    |
			+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
			|                    ANCOUNT                    |
			+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
			|                    NSCOUNT                    |
			+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
			|                    ARCOUNT                    |
			+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
			*/

			//--------- Header part -----------------------------------//
			query[0]  = (byte) (ID >> 8); query[1]  = (byte) ID;
			query[2]  = (byte) 0;         query[3]  = (byte) 0;
			query[4]  = (byte) 0;         query[5]  = (byte) 1;
			query[6]  = (byte) 0;         query[7]  = (byte) 0;
			query[8]  = (byte) 0;         query[9]  = (byte) 0;
			query[10] = (byte) 0;         query[11] = (byte) 0;
			//---------------------------------------------------------//

			//---- End of header --------------------------------------------//


			//----Create query ------------------------------------//

			/* 	Rfc 1035 4.1.2. Question section format
											  1  1  1  1  1  1
			0  1  2  3  4  5  6  7  8  9  0  1  2  3  4  5
			+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
			|                                               |
			/                     QNAME                     /
			/                                               /
			+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
			|                     QTYPE                     |
			+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
			|                     QCLASS                    |
			+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
			
			QNAME
				a domain name represented as a sequence of labels, where
				each label consists of a length octet followed by that
				number of octets.  The domain name terminates with the
				zero length octet for the null label of the root.  Note
				that this field may be an odd number of octets; no
				padding is used.
			*/
			string[] labels = qname.Split(new char[] {'.'});
			int position = 12;
					
			// Copy all domain parts(labels) to query
			// eg. lumisoft.ee = 2 labels, lumisoft and ee.
			// format = label.length + label(bytes)
			foreach(string label in labels){
				// add label lenght to query
				query[position++] = (byte)(label.Length); 

				// convert label string to byte array
				byte[] b = Encoding.ASCII.GetBytes(label.ToCharArray());
				b.CopyTo(query,position);

				// Move position by label length
				position += b.Length;
			}

			// Terminate domain (see note above)
			query[position++] = (byte) 0; 
			
			// Set QTYPE 
			query[position++] = (byte) 0;
			query[position++] = (byte)qtype;
				
			// Set QCLASS
			query[position++] = (byte) 0;
			query[position++] = (byte)qclass;
			//-------------------------------------------------------//
			
			return query;
		}

		#endregion

		#region method GetQName

		private bool GetQName(byte[] reply,ref int offset,ref string name)
		{	
			try
			{
				// Do while not terminator
				while(reply[offset] != 0){
					
					// Check if it's pointer(In pointer first two bits always 1)
					bool isPointer = ((reply[offset] & 0xC0) == 0xC0);
					
					// If pointer
					if(isPointer){
						int pStart = ((reply[offset] & 0x3F) << 8) | (reply[++offset]);
						offset++;						
						return GetQName(reply,ref pStart,ref name);
					}
					else{
						// label lenght (length = 8Bit and first 2 bits always 0)
						int labelLenght = (reply[offset] & 0x3F);
						offset++;
						
						// Copy label into name 
						name += Encoding.ASCII.GetString(reply,offset,labelLenght);
						offset += labelLenght;
					}
									
					// If the next char isn't terminator,
					// label continues - add dot between two labels
					if (reply[offset] != 0){
						name += ".";
					}
				}

				// Move offset by terminator lenght
				offset++;

				return true;
			}
			catch//(Exception x)
			{
		//		System.Windows.Forms.MessageBox.Show(x.Message);
				return false;
			}
		}

		#endregion

		#region method ParseAnswers

		/// <summary>
		/// Parses answer.
		/// </summary>
		/// <param name="reply"></param>
		/// <param name="queryID"></param>
		/// <returns>Returns Dns_Answer[] collection if answer parsed successfully or null if failed.</returns>
		private ArrayList ParseAnswers(byte[] reply,int queryID)
		{			
			if (reply == null)
				return null;
			try{
				//--- Parse headers ------------------------------------//

				/*
												1  1  1  1  1  1
				  0  1  2  3  4  5  6  7  8  9  0  1  2  3  4  5
				 +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
				 |                      ID                       |
				 +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
				 |QR|   Opcode  |AA|TC|RD|RA|   Z    |   RCODE   |
				 +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
				 |                    QDCOUNT                    |
				 +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
				 |                    ANCOUNT                    |
				 +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
				 |                    NSCOUNT                    |
				 +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
				 |                    ARCOUNT                    |
				 +--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
			 
				QDCOUNT
					an unsigned 16 bit integer specifying the number of
					entries in the question section.

				ANCOUNT
					an unsigned 16 bit integer specifying the number of
					resource records in the answer section.
				*/
		
				// Get reply code
				int    id          = (reply[0]  << 8 | reply[1]);
				OPCODE opcode      = (OPCODE)((reply[2] >> 3) & 15);
				RCODE  replyCode   = (RCODE)(reply[3]  & 15);	
				int    queryCount  = (reply[4]  << 8 | reply[5]);
				int    answerCount = (reply[6]  << 8 | reply[7]);
				int    nsCount     = (reply[8]  << 8 | reply[9]);
				int    arCount     = (reply[10] << 8 | reply[11]);
				//---- End of headers ---------------------------------//

				// Check that it's query what we want
				if(queryID != id){
					return null;
				}
		
				int pos = 12;

				//----- Parse question part ------------//
				for(int q=0;q<queryCount;q++){
					string dummy = "";
					GetQName(reply,ref pos,ref dummy);
					//qtype + qclass
					pos += 4;
				}
				//--------------------------------------//
			

				/*
											   1  1  1  1  1  1
				 0  1  2  3  4  5  6  7  8  9  0  1  2  3  4  5
				+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
				|                                               |
				/                                               /
				/                      NAME                     /
				|                                               |
				+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
				|                      TYPE                     |
				+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
				|                     CLASS                     |
				+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
				|                      TTL                      |
				|                                               |
				+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
				|                   RDLENGTH                    |
				+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--|
				/                     RDATA                     /
				/                                               /
				+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+--+
				*/

				ArrayList answers = new ArrayList();
				//---- Start parsing aswers ------------------------------------------------------------------//
				for(int i=0;i<answerCount;i++){
					string name = "";
					if(!GetQName(reply,ref pos,ref name)){
						return null;
					}

					int type     = reply[pos++] << 8  | reply[pos++];
					int rdClass  = reply[pos++] << 8  | reply[pos++];
					int ttl      = reply[pos++] << 24 | reply[pos++] << 16 | reply[pos++] << 8  | reply[pos++];
					int rdLength = reply[pos++] << 8  | reply[pos++];

					object answerObj = null;
					switch((QTYPE)type)
					{
						case QTYPE.A:
							answerObj = ParseARecord(reply,ref pos,rdLength);
							pos += rdLength;		
							break;

						case QTYPE.PTR:
							answerObj = ParsePTRRecord(reply,ref pos,rdLength);							
						//	pos += rdLength;		
							break;

						case QTYPE.MX:
							answerObj = ParseMxRecord(reply,ref pos);
							break;

						default:
							answerObj = "dummy"; // Dummy place holder for now
							pos += rdLength;
							break;
					}
			
					// Add answer to answer list
					if(answerObj != null){
						answers.Add(answerObj);
					}
				}
				//-------------------------------------------------------------------------------------------//

				return answers;
			}
			catch{
				return null;
			}
		}

		#endregion


		#region Properties Implementation

		/// <summary>
		/// Gets or sets dns servers.
		/// </summary>
		public static string[] DnsServers
		{
			get{ return m_DnsServers; }

			set{ m_DnsServers = value; }
		}

		/// <summary>
		/// Gets or sets if to use dns caching.
		/// </summary>
		public static bool UseDnsCache
		{
			get{ return m_UseDnsCache; }

			set{ m_UseDnsCache = value; }
		}

		internal static int ID
		{
			get{
				if(m_ID >= 65535){
					m_ID = 100;
				}
				return m_ID++; 
			}
		}

		#endregion

	}
}
