using	System;
using	System.Collections;
using	System.Collections.Generic;
using   System.Collections.Specialized;
using	System.Data.OleDb;
using	System.IO;
using	System.Net;
using	System.Net.Sockets;
using	System.Text;
using	System.Threading;
// using	System.Windows.Forms;
using	System.Xml;

using	LRS;

namespace LRSCDDB {
	/// <summary>
	/// An abstract base class for CDDB processing. It contains the common
	/// logic for accessing the database. CDDBviaSockets and CDDBviaHTTP 
	/// inherit from this.
	/// </summary>
	/// <remarks>Since most of our logic is in this module, I'm going to put our current 
	/// TODO list (but in no particular order) here:
	/// <ol>
	/// <li>Maybe add checkboxes to UI to let us freeze boxes. The ADD button
	/// could then easily pick-and-choose from information from different CDDB servers.</li>
	/// 
	/// <li>Go through "using's" in all modules and delete unneeded ones.</li>
	/// 
	/// <li>Look at the whole namespace issue. Also public vs protected vs 
	/// internal/assembly, etc</li>
	/// 
	/// <li>Go through the TODO comments in the code.</li>
	/// 
	/// <li>The whole exception thing.</li>
	/// 
	/// <li>The queries are fast enough, but multithread them, just for the
	/// exercise.</li>
	/// 
	/// <li>Look up lyrics on www.lyrics.ch, aka songfile.com/index_2.html</li>
	/// 
	/// <li>Console.WriteLine -- Should this use Diagnostic.Trace or 
	/// Diagnostic.Debug? Or should we add a Listener, and add an option to
	/// dump out our data to a file, even in Release mode?</li>
	/// 
	/// <li>Look at all instances of variables called "msg", and find better names.</li>
	/// 
	/// <li>Get macros working. ON A COPY!!!</li>
	/// </ol>
	/// </remarks>
	abstract public class CDDBBase {
		string			code;				// 1st 3 chars of each msg from server
		public string	cmd;				// The most recent CDDB command sent.
											// This is kept here for debugging and
											// someday perhaps for exception throwing
		protected string site;				// So we can tell whence a given AlbumInfo
											// came from
		CDInfo			inf;
		ArrayList		Albums = new ArrayList();

		// Each derived class knows how to initialize itself, clean up
		// when it's done, and how to send commands in its own special way.
		abstract public void Init();
		abstract public void Term();
		abstract public StringCollection SendCDDBCmd(string command);

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Query the CDDB to get a list (hopefully) of matching discs.
		/// For each disc, retrieve the track name info as well. The
		/// result is a vector of AlbumInfo's.
		/// </summary>
		/// <param name="info">Raw CD information, such as number of tracks, etc</param>
		/// <returns>An ArrayList of zero or more AlbumInfo's.</returns>
		public ArrayList GetAlbumInfo(CDInfo info) {
			StringCollection	TextFromCDDB;
			
			inf = info;

			Init();						// Do any derived class initialization

			// A couple of commands that we could issue, but don't:
			// cddb lscat, stat, whom, sites

			// Query the database - see how many albums we find
			TextFromCDDB = SendCDDBCmd(BuildCDDBQueryString(inf));

			switch (code) {
				case "200":				// Exact match found
					GetTrackNameInfo(inf.NumberOfTracks, TextFromCDDB[0].Substring(4));
					break;
				case "202":				// Disc not found
					// Do nothing. The <Albums> ArrayList will be empty,
					// which is fine.
					break;
				case "210":				// Multiple matches (proto 4 and above)
				case "211":				// Multiple inexact matches
					// I found multiple inexact matches on cddb.com for John
					// O'Conor's Beethoven Piano Sonatas, Volume 1. It looked
					// good, so I'm going to treat 211 the same as 210
					for (int i=1; i< TextFromCDDB.Count; ++i) {
						GetTrackNameInfo(inf.NumberOfTracks, TextFromCDDB[i]);
					}
					break;
				default:
					// Silently ignore any other code
					break;
			}

			Term();						// Do any derived class cleanup

			return Albums;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Constructs the semi-complex CDDB Query command from the CD track info.
		/// </summary>
		/// <param name="inf">Raw CD info, such as number of tracks and durations.</param>
		/// <returns>A query string suitable for sending to a CDDB server.</returns>
		string BuildCDDBQueryString(CDInfo inf) {
			StringBuilder sb = new StringBuilder();
			// Sigh: I'd expect we could do sb+="foo", rather than s.Append("foo")
			sb.Append(string.Format("cddb query {0} {1} ", inf.DiscID.ToString("x8"), inf.NumberOfTracks));
			for (int i=0; i<inf.NumberOfTracks; ++i) {
				sb.Append(string.Format("{0} ", inf.tracks[i].StartPositionInFrames));
			}
			sb.Append(inf.CDTotalLengthInSeconds);
			return sb.ToString();
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Reads one or more text lines from the server. If the Code on the 
		/// first line has a "1" in the second position (e.g. "210"), then we 
		/// continue reading until we see a line with only ".". 
		/// </summary>
		/// <returns>
		/// The lines read from the server (perhaps only one).
		/// </returns>
		public StringCollection ReadFromServer(StreamReader sr) {
			StringCollection	s = new StringCollection();
			string				msg;
			msg = ReadLine(sr);
			// TODO: If message length < 3, or first char isn't "2",
			// then throw an error.
			code = msg.Substring(0, 3);
			if (code.Substring(1, 1) == "1") {		// More info?
				do {
					s.Add(msg);
					msg = ReadLine(sr);
				} while (msg != ".");	
			} else {
				s.Add(msg);					// Only one line, so return it
			}
			return s;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Read a single line from the CDDB server. All input from
		/// it goes through this routine, so we can do things like display 
		/// the messages received on the Debug output stream.
		/// </summary>
		/// <returns></returns>
		string ReadLine(StreamReader sr) {
			string	msg;
			msg = sr.ReadLine();
			// Console.WriteLine(msg);
			return msg;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Reads track info (i.e. the name of the cut) from the CDDB.
		/// </summary>
		/// <param name="nTrks">The number of tracks on the disc</param>
		/// <param name="CDDBQueryResult">The result of a "cddb query" command</param>
		void GetTrackNameInfo(int nTrks, string CDDBQueryResult) {
			StringCollection	msg;
			string []	s = CDDBQueryResult.Split(" ".ToCharArray(), 3);
			// s[0] = category (e.g. "rock"). s[1] is discID.
			// s[2] is "Artist / Album Title".
			cmd = string.Format("cddb read {0} {1}", s[0], s[1]);
			msg = SendCDDBCmd(cmd);
			Albums.Add(new AlbumInfo(inf.NumberOfTracks, s, msg, site));
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	/// <summary>
	/// Derived from CDDBBase, it provides the routines to communicate with a
	/// CDDB server via the raw sockets interface.
	/// </summary>
	public class CDDBviaSockets : CDDBBase {
		StreamReader	netin;
		StreamWriter	netout;
		TcpClient		client;
		NetworkStream	netStream;
		int				proto;

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Pass in any information we'll need later.
		/// </summary>
		/// <param name="site">The URL of the CDDB server (e.g. freedb.freedb.org)</param>
		public CDDBviaSockets(string site, int proto) {
			this.site  = site;
			this.proto = proto;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Virtual routine to send the specified CDDB command (e.g. query, read,
		/// etc) to the server.
		/// </summary>
		/// <param name="command">e.g. cddb read rock "discid"</param>
		/// <returns>A StringCollection of one or more lines resulting 
		/// from the command.</returns>
		override public StringCollection SendCDDBCmd(string command) {
			// Note: Some parts of this routine overlap code in CDDBviaHTTP, such
			// as tracing the commands entered, and reading the results back. But
			// it's such a small amount of code, that I'm not going to go to the
			// effort (at least at this point) of hoisting some of the code up
			// into the base class.
			StringCollection	sc;
			Console.WriteLine("\n> {0}", command);
			cmd = command;					// Copy current cmd for exception msg
			netout.WriteLine(cmd);
			sc = ReadFromServer(netin);			
			return sc;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Virtual function to do initialization required by the CDDBviaSockets 
		/// protocol. 
		/// </summary>
		override public void Init() {
			StringCollection	msg;
			client = new TcpClient(site, 888);
			client.ReceiveTimeout = 10000;			// 10 seconds. Pretty well arbitrary
			netStream = client.GetStream();
			netin  = new StreamReader(netStream, Encoding.ASCII);
			netout = new StreamWriter(netStream, Encoding.ASCII);
			netout.AutoFlush = true;

			msg = ReadFromServer(netin);			// Read server welcome string
	
			// Get "Hello and welcome"
			msg = SendCDDBCmd("cddb hello LRSXP 192.168.0.5 LRSCDDB v0.1");

			msg = SendCDDBCmd("proto " + proto);
		}

		/// <summary>
		/// Virtual function to do any cleanup required by the CDDBviaHTTP 
		/// protocol. 
		/// </summary>
		override public void Term() {
			SendCDDBCmd("quit");
			netStream.Close();
			client.Close();
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------


	/// <summary>
	/// Derived from CDDBBase, it provides the routines to communicate with a
	/// CDDB server via the HTTP interface.
	/// </summary>
	public class CDDBviaHTTP : CDDBBase {
//		string	hello = "&hello=LRSXP+foo.net+CDmax+1.7.6&proto=4\r\n";
		string	hello;
		string	CGIURI;
		string	FullSite;

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Pass in any information we'll need later.
		/// </summary>
		/// <param name="site">The URL of the CDDB CGI server (e.g. 
		/// cddb.cddb.com/~cddb/cddb.cgi). "http://" will be prepended to 
		/// the given name.</param>
		public CDDBviaHTTP(string site, string CGIURI, string hi, int proto) {
			this.site = site;
			this.CGIURI = CGIURI;
			this.FullSite = "http://" + site + CGIURI;
			hello = "&hello=" + hi + "&proto=" + proto;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Virtual routine to send the specified CDDB command (e.g. query, read,
		/// etc) to the server.
		/// </summary>
		/// <param name="command">e.g. cddb read rock "discid"</param>
		/// <returns>A StringCollection of one or more lines resulting 
		/// from the command.</returns>
		override public StringCollection SendCDDBCmd(string command) {
			HttpWebRequest		wReq;
			Stream				outStream;
			StreamWriter		sw;

			HttpWebResponse		wResp;
			Stream				inStream;
			StreamReader		sr;

			StringCollection	sc;

			wReq = (HttpWebRequest)WebRequest.Create(FullSite);
			wReq.Method = "POST";
			wReq.Timeout = 10000;
			// wReq.Headers.Add("User-Agent: Mozilla/2.0 (compatible)");
			// I used to get timeouts on the next line. See the comments below.
			outStream = wReq.GetRequestStream();
			cmd = command.Replace(" ", "+");
			sw = new StreamWriter(outStream);
			Console.WriteLine("> " + cmd);
			sw.WriteLine("\ncmd=" + cmd + hello);
			sw.Close();

			wResp = (HttpWebResponse)wReq.GetResponse();
			inStream = wResp.GetResponseStream();
			sr = new StreamReader(inStream);
			sc = ReadFromServer(sr);
			// Phew, this one took a couple of hours to track down!
			// I'd be able to do a lookup on a single CD, and maybe
			// even a second. Maybe. Sometimes even a third. Maybe.
			// But more often I'd hang above, while trying to get the Request Stream.
			// The system complained that the operation timed out. Which was
			// strange, since a Netmon trace showed that nothing was even being
			// sent upstream (no HTTP POST message). So what was timing out?
			// Well, I finally found a hint. In MSDN, under .NET Documentation |
			// Building .NET Applications | Accessing the Internet | Working with
			// Enhanced Features [still with me?] | Best Practices for Net Classes
			// (whew!), I found the statement that "The number of connections opened
			// to an Internet resource can have a significant impact on network
			// performance and throughput. System.Net uses two connections per
			// host by default. Setting the ConnectionLimit property in the
			// ServicePoint instance of your application can increase this number."
			// Once I found that, it was clear I'd neglected to close something.
			// A bit more digging around and I came up with the .Close() I'd left
			// out at first. But I have two final comments. First of all, I was
			// annoyed (in that oh-so-familiar way) when the error message showed
			// up above, but the fix was down here. Oh well. But I also wish that
			// Microsoft's (and Wrox's) samples had included calls to .Close()!!!
			wResp.Close();
			return sc;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Virtual function to do initialization required by the CDDBviaHTTP 
		/// protocol. Currently there is no processing needed here.
		/// </summary>
		override public void Init() {
			// Nothing
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Virtual function to do any cleanup required by the CDDBviaHTTP 
		/// protocol. Currently there is no processing needed here.
		/// </summary>
		override public void Term() {
			// Nothing
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	/// <summary>
	/// The Windows Media Player (WMP) doesn't use the traditional CDDB protocol,
	/// but it does access a CD DataBase, so we're going to call it CDDBWMP.
	/// </summary>
	class CDDBWMP {
		string		site;

//---------------------------------------------------------------------------------------

		public CDDBWMP(string site) {
			this.site = site;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cdi"></param>
		/// <param name="version">1=WMP version 8 and below; 2=WMP 9 and above</param>
		/// <returns></returns>
		public ArrayList GetAlbumInfo(CDInfo cdi, int version) {
			// TODO: Make an enum for <version>
			ArrayList		albums = new ArrayList();
			AlbumInfo		CurAlbum;
			TrackInfo		CurTrack;
			WebRequest		wrq;
			WebResponse		wrs;
			Stream			inStream;
			StreamReader	sr;

			string			FullSite;
		
			FullSite = "http" + "://" + "windowsmedia.com";
			if (version == 1) {
				FullSite += "/redir/QueryTOC.asp";
				FullSite += "?WMPFriendly=true&locale=409&version=8.0.0.4440";
				// TODO: Can we get away without WMPFriendly, locale, and/or version?
				FullSite += "&cd=" + BuildWMPQueryString(cdi, version);
			} else {
				FullSite += "/redir/servedocument.asp";
				FullSite += "?locale=en-us&version=9.0.0.3705&doc=info_albuminfo_bcd";
				FullSite += "&toc=" + BuildWMPQueryString(cdi, version);
			}
			// For reference, here's what the query string should look like for
			// Rhino's British Invasion, Volume 1
			// site += "&cd=14+96+285A+4A3D+7686+9C81+C0CB+EB69+11CE2+13DA3";
			// site += "+163F0+19203+1B13B+1DED1";
			// site += "+208A2+22E66+25B07+2875A+2BC0F+2E36C+312EA+340AA";

			wrq = WebRequest.Create(FullSite);
			wrq.Timeout = 10000;

			wrs = wrq.GetResponse();
			inStream = wrs.GetResponseStream();
			sr = new StreamReader(inStream);

			string		XmlText;
			XmlText = sr.ReadToEnd();
			wrs.Close();

#if false
			StreamWriter	sw = new StreamWriter("WMP response.xml");
			sw.WriteLine(XmlText);
			sw.Close();
#endif
			Console.WriteLine(XmlText);		// TODO:

			// We now have a single string with the XML in it (we hope!).
			// Load it into an XML parser and traverse its structure.
			XmlDocument	xdoc = new XmlDocument();
			xdoc.LoadXml(XmlText);

			int nAlbum, nTrk;
			// I'm going to assume (until I see two albums with the same ID)
			// that the <OMI> tag represents a single album.
			// OMI == Online Music Index???
			XmlNodeList		xnl;
			if (version == 1)
				 xnl = xdoc.GetElementsByTagName("OMI");
			else
				 xnl = xdoc.GetElementsByTagName("BasicCDMetadata");
#if false
	<AlbumInfo>
		<fullName>Various Artists</fullName>
		<msid_person>CB5EAD96-8413-42D8-9D64-563385A76479</msid_person>
		<fulltitle>The Corner of Bleecker and the Blues</fulltitle>
		<msid_album>2F7ED0D7-61E9-4A44-B56B-DE114F40269D</msid_album>
		<buyURL>providerName=User Feedback&amp;albumID=2F7ED0D7-61E9-4A44-B56B-DE114F40269D&amp;a_id=%20&amp;album=The%20Corner%20of%20Bleecker%20and%20the%20Blues&amp;artistID=CB5EAD96-8413-42D8-9D64-563385A76479&amp;p_id=%20&amp;artist=Various%20Artists</buyURL>
		<Track>
			<trackTitle>The Gallis Pole</trackTitle>
			<trackNumber>1</trackNumber>
			<volume>1</volume>
		</Track>
		<Track>
			<trackTitle>Pretty Boy Floyd</trackTitle>
			<trackNumber>2</trackNumber>
			<volume>1</volume>
		</Track>
		<Track>
			<trackTitle>Midnight Special</trackTitle>
			<trackNumber>3</trackNumber>
			<volume>1</volume>
		</Track>
		<Track>
			<trackTitle>See See Rider</trackTitle>
			<trackNumber>4</trackNumber>
			<volume>1</volume>
		</Track>
		<Track>
			<trackTitle>Ain&apos;t Gonna Study War No More</trackTitle>
			<trackNumber>5</trackNumber>
			<volume>1</volume>
		</Track>
		<Track>
			<trackTitle>Chain Gang Special</trackTitle>
			<trackNumber>6</trackNumber>
			<volume>1</volume>
		</Track>
		<Track>
			<trackTitle>Baby Please Don&apos;t Go</trackTitle>
			<trackNumber>7</trackNumber>
			<volume>1</volume>
		</Track>
		<Track>
			<trackTitle>Girl of Constant Sorrow</trackTitle>
			<trackNumber>8</trackNumber>
			<volume>1</volume>
		</Track>
		<Track>
			<trackTitle>I&apos;ll Fly Away</trackTitle>
			<trackNumber>9</trackNumber>
			<volume>1</volume>
		</Track>
		<Track>
			<trackTitle>More Pretty Girls Than One</trackTitle>
			<trackNumber>10</trackNumber>
			<volume>1</volume>
		</Track>
		<Track>
			<trackTitle>Going Down The Road (I Ain&apos;t Gonna Be Treated This Way)</trackTitle>
			<trackNumber>11</trackNumber>
			<volume>1</volume>
		</Track>
		<Track>
			<trackTitle>Give My Regards To Mayor Laguardia</trackTitle>
			<trackNumber>12</trackNumber>
			<volume>1</volume>
		</Track>
		<Track>
			<trackTitle>Lonesome Road</trackTitle>
			<trackNumber>13</trackNumber>
			<volume>1</volume>
		</Track>
		<Track>
			<trackTitle>Blues For Gamblers</trackTitle>
			<trackNumber>14</trackNumber>
			<volume>1</volume>
		</Track>
		<Track>
			<trackTitle>Where Did You Sleep Last Night</trackTitle>
			<trackNumber>15</trackNumber>
			<volume>1</volume>
		</Track>
		<Track>
			<trackTitle>Great Gospel Blues (Right On That Shore)</trackTitle>
			<trackNumber>16</trackNumber>
			<volume>1</volume>
		</Track>
	</AlbumInfo>
	<Attribution>
		<ProviderName>User Feedback</ProviderName>
	</Attribution>
#endif

#if false
<?xml version='1.0' encoding="UTF-8" ?>
<BasicCDMetadata xmlns:sql="urn:schemas-microsoft-com:xml-sql">
	<AlbumInfo>
		<fullName>Mason Williams</fullName>
		<msid_person>3FB3A113-940A-428D-9812-FE9EC4AB010F</msid_person>
		<fulltitle>The Mason Williams Phonograph Record</fulltitle>
		<msid_album>9C69751B-52FC-4C5F-BA5F-E586E8C1ECEC</msid_album>
		<label>Warner Brothers</label>
		<releaseDate>1968-02-01</releaseDate>
		<coverURL>200/drc400/c477/c4770673qka.jpg</coverURL>
		<coverURLSmall>075/drc400/c477/c4770673qka.jpg</coverURLSmall>
		<buyURL>providerName=AMG&amp;albumID=9C69751B-52FC-4C5F-BA5F-E586E8C1ECEC&amp;a_id=R%20%20%20%2053361&amp;album=The%20Mason%20Williams%20Phonograph%20Record&amp;artistID=3FB3A113-940A-428D-9812-FE9EC4AB010F&amp;p_id=P%20%20%20%20%201911&amp;artist=Mason%20Williams</buyURL>
		<Track>
			<trackTitle>Overture</trackTitle>
			<trackNumber>1</trackNumber>
			<volume>1</volume>
		</Track>
		<Track>
			<trackTitle>All of the Time</trackTitle>
			<trackNumber>2</trackNumber>
			<volume>1</volume>
		</Track>
		<Track>
			<trackTitle>Dylan Thomas</trackTitle>
			<trackNumber>3</trackNumber>
			<volume>1</volume>
		</Track>
		<Track>
			<trackTitle>Wanderlove</trackTitle>
			<trackNumber>4</trackNumber>
			<volume>1</volume>
		</Track>
		<Track>
			<trackTitle>She&apos;s Gone Away</trackTitle>
			<trackNumber>5</trackNumber>
			<volume>1</volume>
		</Track>
		<Track>
			<trackTitle>Here I Am</trackTitle>
			<trackNumber>6</trackNumber>
			<volume>1</volume>
		</Track>
		<Track>
			<trackTitle>Classical Gas</trackTitle>
			<trackNumber>7</trackNumber>
			<volume>1</volume>
		</Track>
		<Track>
			<trackTitle>Long Time Blues</trackTitle>
			<trackNumber>8</trackNumber>
			<volume>1</volume>
		</Track>
		<Track>
			<trackTitle>Baroque-A-Nova</trackTitle>
			<trackNumber>9</trackNumber>
			<volume>1</volume>
		</Track>
		<Track>
			<trackTitle>Prince&apos;s Panties</trackTitle>
			<trackNumber>10</trackNumber>
			<volume>1</volume>
		</Track>
		<Track>
			<trackTitle>Life Song</trackTitle>
			<trackNumber>11</trackNumber>
			<volume>1</volume>
		</Track>
		<Track>
			<trackTitle>Sunflower</trackTitle>
			<trackNumber>12</trackNumber>
			<volume>1</volume>
		</Track>
	</AlbumInfo>
	<Attribution>
		<ProviderName>AMG</ProviderName>
	</Attribution>
</BasicCDMetadata>
#endif

			nAlbum = 0;
			nTrk = 0;
			// TODO: Rename xNode as xAlbum
			foreach (XmlNode xNode in xnl) {
				Console.Write("{0} ", ++nAlbum);
				if (nAlbum > 1) {
					// For now, let us know if we find more than one match, to confirm
					// my guess that each OMI represents an album, but that we could
					// have more than one.
#if false
					System.Windows.Forms.MessageBox.Show("Found " + nAlbum + " albums on " +
						site, "LRS CDDB - Info");
#endif
				}
				CurAlbum = new AlbumInfo();
				CurAlbum.site = site;
				CurAlbum.XML  = XmlText;
				CurAlbum.TrackNames = new TrackInfo[cdi.NumberOfTracks];
				// If the album isn't found, there will be essentially no XML detail
				// records. We're cool on "new AlbumInfo()" above, since that's a scalar
				// and the ctor will fill in default fields. But TrackNames is a vector
				// and as commented elsewhere in this module, C# doesn't call ctor's
				// when allocating arrays. So run through TrackNames, and allocate
				// explicit entries for them, so we have something to write to the
				// database (even if it's null/empty);
				for (int i=0; i<CurAlbum.TrackNames.Length; ++i) {
					CurAlbum.TrackNames[i] = new TrackInfo();
				}
				// TODO: Rename xAlbum as xAlbumInfo
				foreach (XmlNode xAlbum in xNode.ChildNodes) {
					switch (xAlbum.Name) {
						case "name":
							CurAlbum.title = xAlbum.InnerText.Trim();
							break;
						case "author":
							CurAlbum.artist = xAlbum.InnerText.Trim();
							break;
						case "label":
							CurAlbum.label = xAlbum.InnerText.Trim();
							break;
						case "releasedate":
							CurAlbum.year = xAlbum.InnerText.Trim();
							break;
						case "genre":
							CurAlbum.category = xAlbum.InnerText.Trim();
							break;
						case "style":
							CurAlbum.style = xAlbum.InnerText.Trim();
							break;
						case "smallCoverURL":
							CurAlbum.smallCoverURL = xAlbum.InnerText.Trim();
							break;
						case "largeCoverURL":
							CurAlbum.largeCoverURL = xAlbum.InnerText.Trim();
							break;
						case "moreInfoURL":
							CurAlbum.moreInfoURL = xAlbum.InnerText.Trim();
							break;
						case "track":
							++nTrk;
							CurTrack = new TrackInfo();
							foreach (XmlNode xTrack in xAlbum) {
								switch (xTrack.Name) {
									case "name":
										CurTrack.title = xTrack.InnerText.Trim();
										break;
									case "author":
										CurTrack.extra = xTrack.InnerText.Trim();
										break;
									case "composer":
										CurTrack.composer = xTrack.InnerText.Trim();
										break;
									default:
										// Ignore. Maybe add in a tracing statement here
										break;
								}
							}
							// Silently ignore any tracks that we don't expect
							if (nTrk <= cdi.NumberOfTracks)
								CurAlbum.TrackNames[nTrk - 1] = CurTrack;
							break;
						default:
							Console.WriteLine("Found other name '{0}' - {1}", xAlbum.Name, xAlbum.InnerText);
							break;
					}
				}
				// TODO: The following is essentially the same code as StripExtraIfMatchesArtist().
				// Clearly I need to rethink how CDDB and WMP fit together.
				foreach (TrackInfo trk in CurAlbum.TrackNames) {
					// We might not have found any tracks (i.e. the CD isn't on the database).
					// Check to make sure we have a track entry before we try to use it!
					if (trk != null) {
						if (trk.extra == CurAlbum.artist)
							trk.extra = "";
					}
				}
				albums.Add(CurAlbum);
			}
			return albums;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Constructs the semi-complex encoding of track information
		/// used to identify discs. It's basically the number of tracks,
		/// the starting frame number of each track, and the frame number
		/// of the end of the disc, all in hex, and separated by +'s. It's
		/// very similar to the CDDB query format.
		/// </summary>
		/// <param name="cdi">Raw CD information, such as number of tracks
		/// and their durations.</param>
		/// <returns>A string with the number of tracks, the starting frame of
		/// each track, and the end of the disc, all in frames, and all
		/// in hex.</returns>
		string BuildWMPQueryString(CDInfo cdi, int version) {
			StringBuilder sb = new StringBuilder();

			if (version == 1) {
				sb.Append(cdi.NumberOfTracks.ToString("X"));
				sb.Append("+");
				foreach (CDBasicTrackInfo track in cdi.tracks) {
					sb.Append(track.StartPositionInFrames.ToString("X"));
					sb.Append("+");
				}
				// Now we need to tack on the end of the disc
				CDBasicTrackInfo	LastTrack = cdi.tracks[cdi.NumberOfTracks-1];
				int		EndOfDisc;
				EndOfDisc = LastTrack.GetFrameEnd();
				sb.Append(EndOfDisc.ToString("X"));
				return sb.ToString();
			}
			// Must be version 2
			foreach (CDBasicTrackInfo track in cdi.tracks) {
				sb.Append(track.StartPositionInFrames.ToString("X6"));
			}
			// string s = sb.ToString();		// TODO: Test code
			return sb.ToString();
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	/// <summary>
	/// Contains the information about each CDDB site. In particular, it contains
	/// the data and methods required to run each CDDB request as a separate thread.
	/// </summary>
	public class CDDBThreadInfo {
		string		Protocol;
		string		ServerURL;
		string		CGIURI;
		string		hello;
		int			proto;
		CDInfo		cdi;
		ArrayList	Albums;

		// TODO: Yeah, this shouldn't be public
		public Thread	TheThread;

//---------------------------------------------------------------------------------------

		public CDDBThreadInfo(string Protocol, string ServerURL, string CGIURL, 
								   string hello, int proto) {
			this.Protocol  = Protocol;
			this.ServerURL = ServerURL;
			this.CGIURI    = CGIURL;
			this.hello	   = hello;
			this.proto	   = proto;
		}

//---------------------------------------------------------------------------------------

		public void SetAdditionalInfo(CDInfo cdi, ArrayList Albums) {
			this.cdi    = cdi;
			this.Albums = Albums;
		}

//---------------------------------------------------------------------------------------

		public void Run() {
			CDDBBase	cddb;
			CDDBWMP		wmp;			// wmp = Windows Media Player
			CDDBWMP		wmp2;			// For WMP version 9
			ArrayList	newAlbums = new ArrayList();

			try {
				switch (Protocol) {
				case "CDDB":
					cddb	  = new CDDBviaSockets(ServerURL, proto);
					newAlbums = cddb.GetAlbumInfo(cdi);
					break;
				case "HTTP":
					cddb	  = new CDDBviaHTTP(ServerURL, CGIURI, hello, proto);
					newAlbums = cddb.GetAlbumInfo(cdi);
					break;
				case "WMP":
					wmp		  = new CDDBWMP(ServerURL);
					newAlbums = wmp.GetAlbumInfo(cdi, 1);
					break;
				case "WMP2":
					wmp2	  = new CDDBWMP(ServerURL);
					newAlbums = wmp2.GetAlbumInfo(cdi, 2);
					break;
				default:
					// Arguably, any invalid protocols should never had got past
					// creating this instance. TODO:
#if false
					MessageBox.Show("Unrecognized protocol (" + Protocol + ") found in " +
						"Servers table in Access database", "LRS CDDB Warning", 
						MessageBoxButtons.OK, MessageBoxIcon.Warning);
#endif
					break;
				}
				if (newAlbums.Count > 0) {
					lock(Albums) {
						Albums.AddRange(newAlbums);
					}
				}
			} catch (Exception ex) {
#if false
				MessageBox.Show("Exception \"" + ex.Message + "\" occured when processing " +
					ServerURL + "\r\n\r\nStack Trace = " + ex.StackTrace,
					"LRS CDDB Exception - " + ex.Source, 
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
#endif
			}
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	/// <summary>
	/// A simple class to hold information about tracks from a CD Database. This
	/// will include (hopefully) the title, and perhaps other information, such
	/// as artist (if it's a compilation), and perhaps composer, etc.
	/// </summary>
	public class TrackInfo {
		public string			title;
		public string			extra;			// Extra info about the track. Often the
												// artist, especially on anthologies
		public string			composer;

//---------------------------------------------------------------------------------------

		public TrackInfo() {
			title	 = "";
			extra	 = "";
			composer = "";
		}

//---------------------------------------------------------------------------------------

		public TrackInfo(string title, string extra) {
			this.title  = title;
			this.extra  = extra;
			composer	= "";
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	/// <summary>
	/// Information about a single album. Usually includes album title and artist,
	/// and a vector of CDTrackInfo data for each track.
	/// </summary>
	public class AlbumInfo {
		public string			title;
		public string			artist;
		public string			category;
		public string			style;			// Essentially a sub-category
												// For example, Abby Newton's "Crossing
												// to Scotland" has a category of "WORLD",
												// and a style of "Celtic"
		public string			site;
		public string			extra;			// Extra info about disk
		public string			year;			// aka Release Date
		public string			label;			// e.g. Capitol, Rhino, Green Linnet
		public string			smallCoverURL;
		public string			largeCoverURL;
		public string			moreInfoURL;
		public string			XML;
		public TrackInfo []		TrackNames;

		Dictionary<string, string>	AlbumDict;

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Default constructor. The user will fill in all the info herself.
		/// </summary>
		public AlbumInfo() {
			SetDefaultValues();
		}

//---------------------------------------------------------------------------------------

		void SetDefaultValues() {
			site			= "";
			category		= "";
			artist			= "";
			title			= "";
			extra			= "";
			year			= "";
			label			= "";
			smallCoverURL	= "";
			largeCoverURL	= "";
			moreInfoURL		= "";
			style			= "";
			XML				= "";
			TrackNames		= null;
			AlbumDict		= new Dictionary<string,string>();
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Processes the result of a "cddb read discid" command.
		/// </summary>
		/// <param name="nTrks">The number of tracks on the CD.</param>
		/// <param name="CDDBQueryInfo">The result of a "cddb query" command, parsed
		/// into a string array. This might be {"rock", "discid", "Artist/Title".</param>
		/// <param name="msg">The result of a "cddb read" command, with multiple lines
		/// describing each track, its name, etc.</param>
		/// <param name="site">The URL of where we got this info from.</param>
		public AlbumInfo(int nTrks, string [] CDDBQueryInfo, StringCollection msg,
			string site) {
			SetDefaultValues();
			// First fill in the stuff we can without looking at the track data
			string []		Artist_Title;
			category = CDDBQueryInfo[0];
			// CDDBQueryInfo[1] is DiscID. But we already know that
			Artist_Title = CDDBQueryInfo[2].Split("/".ToCharArray());
			artist   = Artist_Title[0].Trim();
			// Yeah, the CDDB spec says there's supposed to be an artist,
			// then a slash, then a title. But sometimes there isn't, and
			// we wind up with Artist_Title being only length==1, not 2.
			if (Artist_Title.Length > 1)		// Do we have a title?
				title   = Artist_Title[1].Trim();
			else
				title	= "";
			year	 = "";
			this.site = site;				// Where we found this entry

			// TODO: Sigh. The design of this is all wrong. An <AlbumInfo>
			// shouldn't need to know anything about the output from a CDDB
			// server. The hint was that the code below doesn't support WMP
			// servers. Sigh again. Gotta go back and redo this.

			int		i;
			int		TrackNo;

			// There's a part of me that wanted to simply allocate the number of
			// tracks on the CD, and be done with it. But I haven't yet seen any
			// CDDB entries that "almost match". So then I worried that some of
			// these might have more tracks than are on the CD currently being
			// queried for. Sounds pretty silly, that they might consider a disk
			// with a different number of tracks as a close match. But just in
			// case, I considered making TrackNames an ArrayList. But that just
			// complicated the logic in the TTITLE<n> and EXTT<n> routines below.
			// So I decided to ignore any extra tracks. Maybe I'll pop up a 
			// MessageBox if we get something unexpected.
			TrackNames = new TrackInfo[nTrks];
			for (i=0; i<nTrks; ++i)
				TrackNames[i] = new TrackInfo();

			// Now process each "Keyword=Value" line
			string []	KeywordVals;			// Pairs of keywords and values
			string		Keyword, Val;
			string		s;
			// Skip over first line; it's 210 <category> <discid> etc
			for (i=1; i<msg.Count; ++i) {
				s = msg[i];
				if (s.Substring(0, 1) == "#")
					continue;					// Ignore comments
				KeywordVals = s.Split("=".ToCharArray());
				// Robustness time. Contrary to the CDDB spec, I've seen a case
				// where the TITLE4 line has an embedded crlf (rather than the
				// two-character string "\n"). But our ReadLine routine takes
				// this as a keyword without an "=" (or following value). So
				// check to see if we got only a keyword, and ignore the line.
				if (KeywordVals.Length <= 1)
					continue;				// Silently ignore line
				Keyword = KeywordVals[0];
				Val     = KeywordVals[1].Trim();
				// I'd like to be able to use a switch, but the fields are
				// named things like "TITLE0=", "TITLE1=", etc.

				if (Keyword == "DISCID")
					continue;
				if (Keyword == "DTITLE")
					continue;					// We already have this from CDDQueryInfo
				if (Keyword == "PLAYORDER")
					continue;
				if (Keyword == "DYEAR") {
					year = Val.Trim();
					continue;
				}
				if (Keyword == "DGENRE") {
					style = Val.Trim();
					continue;
				}
				if (Keyword == "EXTD") {
					extra += Val.Trim();
					continue;
				}
				// 7, not 6. Make sure we have a track number, too
				if (Keyword.Length >= 7	&& Keyword.Substring(0, 6) == "TTITLE") {
					TrackNo = Int32.Parse(Keyword.Substring(6));
					// There may be more than one entry for a given track number. 
					// Just concatenate them
					if (TrackNo < nTrks)		// Silently ignore track numbers too big
						TrackNames[TrackNo].title += Val.Trim();
					continue;
				}
				// 5, not 4, as in TITLEn above
				if (Keyword.Length >= 5	&& Keyword.Substring(0, 4) == "EXTT") {
					TrackNo = Int32.Parse(Keyword.Substring(4));
					// There may be more than one entry for a given track number. 
					// Just concatenate them
					if (TrackNo < nTrks)		// Silently ignore track numbers too big
						TrackNames[TrackNo].extra += Val.Trim();
					continue;
				}
				Console.WriteLine("Unrecognized CDDB Keyword/Value pair: {0}/{1}",
					Keyword, Val);
			}

			CleanupNewlines();					// Map pseudo newlines to real crlf's
			StripExtraIfMatchesArtist(artist);
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// The CDDB protocol allows newlines in some of the fields (e.g. a multiline
		/// "extra" field. However, it represents the newlines as two characters, a backslash
		/// followed by "n". So now we must go through all relevant fields and clean them up.
		/// </summary>
		void CleanupNewlines() {
			StringBuilder	sb = new StringBuilder();
			sb.Append(year); sb.Replace("\\n", "\r\n"); 
			year = sb.ToString(); sb.Remove(0, sb.Length);

			sb.Append(style); sb.Replace("\\n", "\r\n"); 
			style = sb.ToString(); sb.Remove(0, sb.Length);

			sb.Append(extra); sb.Replace("\\n", "\r\n"); 
			extra = sb.ToString(); sb.Remove(0, sb.Length);

			foreach (TrackInfo trk in TrackNames) {
				sb.Append(trk.title); sb.Replace("\\n", "\r\n"); 
				trk.title = sb.ToString(); sb.Remove(0, sb.Length);

				sb.Append(trk.extra); sb.Replace("\\n", "\r\n"); 
				trk.extra = sb.ToString(); sb.Remove(0, sb.Length);
			}
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// In many cases, the "extra" field for a track gives repeats the
		/// artist name for the album as a whole. Go through each track and
		/// replace the "extra" field by "" where the match occurs.
		/// </summary>
		/// <param name="artist"></param>
		void StripExtraIfMatchesArtist(string artist) {
			foreach (TrackInfo trk in TrackNames) {
				if (trk.extra == artist)
					trk.extra = "";
			}
		}
	}
}
