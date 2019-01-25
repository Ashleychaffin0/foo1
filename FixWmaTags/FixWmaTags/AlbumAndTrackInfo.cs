// TODO: Break this source module up into separate modules

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;	// Needed for DllImport
using System.Text;

namespace LRS.CDInfo {
	/// <summary>
	/// The result of having retrieved information on an album from a CDDB. Note that a
	/// single CDDB request may result in more than one AlbumInfo, if more than one album
	/// matches the CD's fingerprint.
	/// </summary>
	class AlbumInfo {
		Dictionary<string, string>	Tags;
		List<TrackInfo>				Tracks;

		public string				title;
		public string				artist;
		public string				category;
		public string				style;			// Essentially a sub-category
		// For example, Abby Newton's "Crossing to Scotland" has a category of "WORLD",
		// and a style of "Celtic"
		public string				site;
		public string				extra;			// Extra info about disk
		public string				year;			// aka Release Date
		public string				label;			// e.g. Capitol, Rhino, Green Linnet
		public string				smallCoverURL;
		public string				largeCoverURL;
		public string				moreInfoURL;
		public string				XML;

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Default constructor. The user will fill in all the info herself.
		/// </summary>
		public AlbumInfo() {
			SetDefaultValues();
		}

//---------------------------------------------------------------------------------------

		void SetDefaultValues() {
			Tags			= new Dictionary<string, string>();
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
			Tracks			= new List<TrackInfo>();
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	class TrackInfo {
		Dictionary<string, string>	Properties;

		public string title;
		public string extra;			// Extra info about the track. Often the
										// artist, especially on anthologies
		public string composer;

//---------------------------------------------------------------------------------------

		public TrackInfo() {
			SetDefaultValues();
		}

//---------------------------------------------------------------------------------------

		void SetDefaultValues() {
			Properties = new Dictionary<string, string>();
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	/// <summary>
	/// Information on an audio CD, such as the number of tracks, their durations, etc,
	/// retrieved from the disc itself.  Note that we can't retrieve information such 
	/// as Artist, Album Name, etc, since that information simply isn't on the CD. 
	/// Guess the original designers were thinking CD players, not PCs.
	/// </summary>
	public class CDInfo {
		MCI							mci;
		public CDBasicTrackInfo[]	tracks;
		public int					NumberOfTracks;
		public int					TotalLengthInFrames;
		// Sigh. There are two CD "lengths" relevant here. The start of the
		// "lead-out" track (also known as track 0xAA) is the end of the CD,
		// and is thus one definition of the start of the CD. But the first
		// track on the CD may not start at mm:ss:ff of 0:0:0. For example,
		// many start at 0m:2s:0f. So the actual playing time of the CD would,
		// in this case, be two seconds shorter than the start of the lead-out
		// area would imply. We need both these numbers to talk to the CD Database.
		public int					CDTotalLengthInSeconds;		// Start of lead-out area
		public int					CDPlayLengthInSeconds;		// Actual playing time
		public string				Duration;					// mm:ss (mm may be >60)
		public int					DiscID;
		const int					FPS = 75;				// Frames per second

//---------------------------------------------------------------------------------------

		public CDInfo(MCI mci) {
			this.mci = mci;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Gathers all the info on a the current CD in the CD player, such
		/// as number of tracks, where each starts and how long it is.
		/// </summary>
		/// <returns>true/false depending on whether we could get the information.
		/// For example, we might return "false" if there were no CD in the drive.
		/// </returns>
		public bool GetInfo() {
			// TODO: LRS 5-17-05
			// mci.DoMciCmd("Open CDAudio alias CD");

			// The following routines pretty well must be called in
			// exactly this order. For example, GetTrackInfo() needs
			// to know how many tracks there are, so it must follow
			// GetNumberOfTracks(). And so on.
			if (!IsMediaPresent())
				return false;
			GetNumberOfTracks();
			GetDuration();
			GetTrackInfo();
			CalcCDLength();
			CalcDiscID();
			// TODO: LRS 5-17-05
			// mci.DoMciCmd("close CD");
			return true;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Check to see if there's a CD in the drive.
		/// </summary>
		/// <returns>true if there is one, false if not.</returns>
		bool IsMediaPresent() {
			mci.DoMciCmd("Status CD media present");
			return mci.result.ToString() == "true";
		}

//---------------------------------------------------------------------------------------

		void GetDuration() {
			mci.DoMciCmd("status CD length");
			string dur = mci.result.ToString();
			int[] msf = CDBasicTrackInfo.ConvertToMmssff(mci.result);
			// TODO: Probable class design error. The next line is
			// the same as CDBasicTrackInfo::FrameFromMSF. We should
			// either make a separate MSF class, or something else, to 
			// clean things up a bit.
			TotalLengthInFrames = (msf[0] * 60 + msf[1]) * FPS + msf[2];
			// The result comes back as mm:ss:ff. Chop off ff
			Duration = dur.Substring(0, dur.LastIndexOf(":"));
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Sets the "NumberOfTracks" field inside the class to
		/// the number of tracks on the CD.
		/// </summary>
		void GetNumberOfTracks() {
			mci.DoMciCmd("Status CD Number of Tracks");
			NumberOfTracks = Int32.Parse(mci.result.ToString());
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// For each track, get its position, duration, etc.
		/// </summary>
		void GetTrackInfo() {
			tracks = new CDBasicTrackInfo[NumberOfTracks];
			mci.DoMciCmd("set cd time format msf");		// Minutes, Seconds, Frames
			for (int i = 1; i <= NumberOfTracks; ++i) {
				// This one threw me at first. All the <tracks> array
				// elements were coming out as <null>, despite being new'ed
				// above. Only one of my C# books (Eric Gunnerson, first edition)
				// explained what was happening. Believe it or don't, the <new>
				// above sets the <tracks> array to nulls, rather than doing what
				// C++ and Java do, which is to give instances of the object.
				// But C# doesn't do this for performance reasons. If you were
				// new-ing a large number of objects, you might not want the
				// default ctor called for each, especially if your next act
				// were to set them to something else (e.g. from database data).
				// And I know what he means. I remember being aghast the first
				// time I traced through a <new> in C++ for an array of perhaps
				// 1000 elements, and while it was clear what was happening in
				// hindsight, I wasn't expected to see 1000 calls to the ctor,
				// one for each element of the array, just to set a field to
				// zero. So maybe C# got the tradeoff right, making the programmer
				// do a bit more work, so that s/he wouldn't fall into the trap
				// of hidden performance gotchas. Maybe.
				tracks[i - 1] = new CDBasicTrackInfo(mci, i);
			}
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Get the length of the CD in seconds.
		/// </summary>
		/// <remarks>There is somewhat extensive commentary in the code
		/// at this point, mostly to do with a certain amount of jumping
		/// through hoops we have to do due to deficiencies in Microsoft's
		/// MCI interface.</remarks>
		void CalcCDLength() {
			// Get the length of the CD in seconds. 
			// Note: There are two ways to do this. First, by issuing "Status CD Length".
			// The second is to get the position of the last track, and add on its
			// length. We'll use the latter technique. It's slightly more involved but 
			// seems to have the advantage that it gives the right result! But see the
			// "MCI bug" comments below.

			// Oh yeah, another possibility might be to get the Start Position of the
			// disk as a whole, and add on the Length. But I think I'll stick with the
			// algorithm suggested by the CDDB people, as follows:

			// "The Windows MCI interface does not recongize data tracks, as you find on
			// CD Extra CD's. [LRS note - and what about CDs that have both data and
			// audio, such as 11th Hour?] Therefore a wrong disc ID is generated for CD
			// Extra's when using the MCI interface to read the TOC. Because of this,
			// using the MCI interface should only be the last resort - if possible you
			// should use other methods to read the TOC, like ASPI calls." [skip text
			// about sample ASPI code]

			// The Windows MCI interface does not provide the MSF location of
			// the lead-out [LRS - pseudo track 0xAA]. Thus you must compute
			// the lead-out location by taking the starting position of the
			// last track and add the length of the last track to it. However,
			// the MCI intreface returns the length of the last track as ONE
			// FRAME SHORT of the actual length found in the CD's TOC. In most 
			// cases this does not affect the disk ID generated, because we
			// truncate the frame count when computing the disc ID anyway. 
			// However, if the lead-out track has an actual a frame count
			// of zero, the computed quantity (based on the MSF data returned
			// from the MCI interface) would result in the seconds being one
			// short and the frame count be 74. For example, a CD with the last
			// track at an offset of 48m 32s 12f and having a track length of
			// 2m 50s 63f has a lead-out offset of 51m 23s 0f long. Windows
			// MCI incorrectly reports the length as 2m 50s 62f, which would
			// yield a lead-out offset of 51m 22s 74f, which causes the 
			// resulting truncated disc length to be off by one second.
			// This will cuase an incorrect disc ID to be generated. You
			// should thus add one frame to the length of the last track when
			// computing the location of the lead-out."

			// I don't know if the above off-by-one bug has ever been fixed.
			// OTOH, MSDN doesn't seem to mention it, one way or the other.
			// I'm going to assume that it's never been fixed, since it would
			// break working programs.

			int EndOfLastTrack;
			EndOfLastTrack = tracks[NumberOfTracks - 1].GetFrameEnd();
			CDTotalLengthInSeconds = EndOfLastTrack / FPS;		// Ignore remainder
			CDPlayLengthInSeconds = CDTotalLengthInSeconds;
			CDPlayLengthInSeconds -= tracks[0].GetStartTime();
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Calculates the CDDB Disc ID. This algorithm must not be modifed, else
		/// the ID calculated will not match that on the CDDB database.
		/// </summary>
		void CalcDiscID() {
			int n = 0;
			for (int i = 0; i < NumberOfTracks; ++i) {
				n += CddbSum(tracks[i].GetStartTime());
			}
			DiscID = ((n % 0xff) << 24) | (CDPlayLengthInSeconds << 8) | NumberOfTracks;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Auxilliary function used to calculate the CDDB Disc ID. The algorithm 
		/// must not be modified.
		/// </summary>
		/// <param name="n">The starting time of the track.</param>
		/// <returns>A hashing of the start time of a track. It's basically the sum
		/// of each digit in the decimal representation of the start time in seconds.
		/// </returns>
		int CddbSum(int n) {
			int ret = 0;
			while (n > 0) {
				ret += n % 10;
				n /= 10;
			}
			return ret;
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	/// <summary>
	/// Contains a subset of the Media Control Interface routines.
	/// Initially this is to support retrieving info from CDs.
	/// Note: Arguably, the correct way to do this would be via
	/// device driver calls, perhaps using ASPI. Maybe we'll look
	/// into that some day. But for now...
	/// <br/>
	/// There's currently a restriction on what devices this class
	/// can access. I suppose I could test it, but the Microsoft
	/// documentation implies that when you're using the mciSendString()
	/// function, you can reference a CD drive only as "CDAudio". But
	/// if you have more than one drive, you can't reference "CDAudio1",
	/// "CDAudio2", etc. This restriction is removed if you use the
	/// mciSendCommand() API, but I got lazy. But that doesn't seem
	/// to be a big deal in practice. I suppose someone's second CD
	/// drive might spin up a second or two faster than the default one,
	/// but for the time being I'll live with things like this.
	/// <br/>
	/// Another Note: Actually, a better design would be to have
	/// a class (perhaps part of CDInfo) that has functions such
	/// as GetNumberOfTracks(), and it's up to it whether it uses
	/// mciSendString or perhaps something else (such as direct
	/// ASPI calls). But again, later...
	/// </summary>
	public class MCI {

		public StringBuilder result;

//---------------------------------------------------------------------------------------
		
		[DllImport("winmm.dll")]
		static extern int mciSendString(string cmd, StringBuilder result,
			uint resultLen, int hWndCallback);

//---------------------------------------------------------------------------------------

		[DllImport("winmm.dll")]
		static extern Int32 mciGetErrorString(Int32 errorCode,
			StringBuilder errorText, Int32 errorTextSize);

		/// <summary>
		/// Send a command string to the Win32 MCI routine "mciSendString"
		/// </summary>
		/// <param name="cmd">The MCI command string</param>
		/// <returns>The return code from mciSendString</returns>
		public int DoMciCmd(string cmd) {
			int rc;					// Return code
			const int reslen = 256;	// Largest result seems to be 128, so double it

			result = new StringBuilder(reslen);
			rc = mciSendString(cmd, result, reslen, 0);
			// TODO: We should really check for a non-zero return code
			// here and throw an exception based on mciGetErrorString.
			// If we ever do this, change the <returns> comment above. Hmmm.
			// Actually, we might not even return a return code at all.
			// Either it worked, or we throw an exception. Check to see what
			// return codes we get. Maybe Console.WriteLine().
			if (rc != 0) {
				// TODO: We're still not doing anything with this, but at
				//		 least the message can show up in the debugger.
				const int MaxErrorSize = 1000;
				StringBuilder errorText = new StringBuilder(MaxErrorSize);
				mciGetErrorString(rc, errorText, MaxErrorSize);
				throw new Exception("MCI command: \"" + cmd + "\", error text=" + errorText.ToString());
			}
			return rc;
		}
	}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

	/// <summary>
	/// Information on one track of an audio CD.
	/// </summary>
	public class CDBasicTrackInfo {
		public int		StartPositionInFrames;
		public int		DurationInFrames;
		public int[]	StartPosition;			// mm:ss:ff
		public int[]	Duration;				// mm:ss:ff
		const int		FPS = 75;				// Frames per second

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Get information on the specified track
		/// </summary>
		/// <param name="mci">An instance of class MCI, open on the CD</param>
		/// <param name="TrackNo">The track number for which we need information</param>
		public CDBasicTrackInfo(MCI mci, int TrackNo) {
			mci.DoMciCmd("Status CD position track " + TrackNo);
			StartPosition = ConvertToMmssff(mci.result);
			StartPositionInFrames = FrameFromMSF(StartPosition);

			mci.DoMciCmd("Status CD length track " + TrackNo);
			Duration = ConvertToMmssff(mci.result);
			DurationInFrames = FrameFromMSF(Duration);
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Internal routine to take an MCI-returned string in the 
		/// format "minutes:seconds:frames" and convert it to a
		/// numeric vector. This can be either a Duration, or a
		/// StartingPosition.
		/// </summary>
		/// <returns>A three-element vector with minutes, seconds
		/// and frames.</returns>
		public static int[] ConvertToMmssff(StringBuilder s) {
			// Dumb. The String class won't accept a string as a
			// list of separators. It's got to be a char[].
			string[] parts = (s.ToString()).Split(":".ToCharArray());
			int[] vals = new int[3];
			for (int i = 0; i < 3; ++i) {
				vals[i] = Int32.Parse(parts[i]);
			}
			return vals;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Calculates the end of a track, from its starting position
		/// and duration.
		/// </summary>
		/// <returns>An integer of the last frame used by this track.</returns>
		public int GetFrameEnd() {
			int Start = FrameFromMSF(StartPosition);
			int Dur = FrameFromMSF(Duration);
			// Note: MCI duration is off by 1. See note in CDInfo::GetCDLength.
			// Add it in here.
			++Dur;
			return Start + Dur;
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Gets the starting time for this track.
		/// </summary>
		/// <returns>The time in seconds. The frame count is ignored
		/// and does not produce rounding.</returns>
		public int GetStartTime() {
			return StartPosition[0] * 60 + StartPosition[1];
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Converts a numeric vector representing minutes,
		/// seconds and frames into a frame value.
		/// </summary>
		/// <param name="msf">The vector of to be converted.</param>
		/// <returns>The converted value, in frames.</returns>
		public int FrameFromMSF(int[] msf) {
			return (msf[0] * 60 + msf[1]) * FPS + msf[2];
		}

//---------------------------------------------------------------------------------------

		/// <summary>
		/// Converts the duration of this track to m:ss format. We ignore the
		/// frame count field. It does not introduce rounding.
		/// </summary>
		/// <returns>The readable duration, suitable for display.</returns>
		public string DurationToMmSs() {
			string s = Duration[0] + ":";
			s += Duration[1].ToString("d2");
			return s;
		}
	}
}
