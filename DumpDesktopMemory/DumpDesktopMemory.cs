using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

// For error codes, see https://msdn.microsoft.com/en-us/library/windows/desktop/ms681381(v=vs.85).aspx

// http://www.codeproject.com/Articles/670373/Csharp-Read-Write-another-Process-Memory
// https://vctipsplusplus.wordpress.com/tag/error_not_all_assigned/
// https://bytes.com/topic/c-sharp/answers/441340-gettokeninformation
// https://vctipsplusplus.wordpress.com/tag/error_not_all_assigned/
// https://msdn.microsoft.com/en-us/library/bb762195(v=vs.85).aspx (SHGetPathFromIDListEx )
// See shlobj.h
// Next item may turn out to be especially useful. Maybe.
// http://microsoft.public.platformsdk.shell.narkive.com/biNqrziF/getting-path-from-a-shell-idlist-array-in-c
// http://stackoverflow.com/questions/10617034/converting-a-pidl-to-file-path-with-shgetpathfromidlist

// http://hintdesk.com/c-how-to-enable-sedebugprivilege/
// http://hintdesk.com/Web/Source/Program.cs

namespace DumpDesktopMemory {
	public partial class DumpDesktopMemory : Form {
		[DllImport("advapi32.dll")]
		internal static extern bool LookupPrivilegeValue(string lpsystemname, string lpname, [MarshalAs(UnmanagedType.Struct)] out LUID lpLuid);

		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool OpenProcessToken(IntPtr ProcessHandle,
			uint DesiredAccess, out IntPtr TokenHandle);

		[DllImport("kernel32.dll")]
		static extern IntPtr GetCurrentProcess();

		[DllImport("kernel32.dll", CharSet=CharSet.Auto)]
		public static extern IntPtr GetModuleHandle(string lpModuleName);

		// Use this signature if you want the previous state information returned
		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool AdjustTokenPrivileges(IntPtr TokenHandle,
		  [MarshalAs(UnmanagedType.Bool)]bool DisableAllPrivileges,
		  ref TOKEN_PRIVILEGES NewState,
		  UInt32 BufferLengthInBytes,
		  // ref TOKEN_PRIVILEGES PreviousState,
		  object PreviousState,
		  // out UInt32 ReturnLengthInBytes);
		  object ReturnLengthInBytes);

		[StructLayout(LayoutKind.Sequential)]
		public struct LUID {
			public uint LowPart;
			public int HighPart;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct TOKEN_PRIVILEGES {
			public uint PrivilegeCount;
			// We need only 1 LUID_AND ATTRIBUTES, so just put 1 in
			//public LUID_AND_ATTRIBUTES[] Privileges;
			public LUID Luid;
			public uint Attributes;
		}

		public const UInt32 SE_PRIVILEGE_ENABLED_BY_DEFAULT    = 0x00000001;
		public const UInt32 SE_PRIVILEGE_ENABLED               = 0x00000002;
		public const UInt32 SE_PRIVILEGE_REMOVED               = 0x00000004;
		public const UInt32 SE_PRIVILEGE_USED_FOR_ACCESS       = 0x80000000;

		[StructLayout(LayoutKind.Sequential)]
		public struct PRIVILEGE_SET {
			const int ANYSIZE_ARRAY = 1;
			public const int PRIVILEGE_SET_ALL_NECESSARY = 1;
			public uint PrivilegeCount;
			public uint Control;
			// [MarshalAs(UnmanagedType.ByValArray, SizeConst = ANYSIZE_ARRAY)]
			// public LUID_AND_ATTRIBUTES[] Privilege;
			public LUID Luid;
			public uint Attributes;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct LUID_AND_ATTRIBUTES {
			public LUID Luid;
			public uint Attributes;
		}

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern bool ReadProcessMemory(
			IntPtr hProcess,
			IntPtr lpBaseAddress,
			// long lpBaseAddress,
			byte[] lpBuffer,
			int dwSize,
			out int lpNumberOfBytesRead);

		[DllImport("user32.dll", SetLastError = true)]
		static extern IntPtr GetDesktopWindow();

		[DllImport("user32.dll", SetLastError = true)]
		static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int processId);

		[DllImport("oleacc.dll", SetLastError = true)]
		static extern IntPtr GetProcessHandleFromHwnd(IntPtr hWnd);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr OpenProcess(
			uint /* ProcessAccessFlags */ processAccess,
			bool bInheritHandle,
			int processId
		);

		[Flags]
		public enum ProcessAccessType {
			PROCESS_TERMINATE         = 0x0001,
			PROCESS_CREATE_THREAD     = 0x0002,
			PROCESS_SET_SESSIONID     = 0x0004,
			PROCESS_VM_OPERATION      = 0x0008,
			PROCESS_VM_READ           = 0x0010,
			PROCESS_VM_WRITE          = 0x0020,
			PROCESS_DUP_HANDLE        = 0x0040,
			PROCESS_CREATE_PROCESS    = 0x0080,
			PROCESS_SET_QUOTA         = 0x0100,
			PROCESS_SET_INFORMATION   = 0x0200,
			PROCESS_QUERY_INFORMATION = 0x0400
		}

		public const UInt32 STANDARD_RIGHTS_REQUIRED = 0x000F0000;
		public const UInt32 STANDARD_RIGHTS_READ     = 0x00020000;
		public const UInt32 TOKEN_ASSIGN_PRIMARY     = 0x0001;
		public const UInt32 TOKEN_DUPLICATE          = 0x0002;
		public const UInt32 TOKEN_IMPERSONATE        = 0x0004;
		public const UInt32 TOKEN_QUERY              = 0x0008;
		public const UInt32 TOKEN_QUERY_SOURCE       = 0x0010;
		public const UInt32 TOKEN_ADJUST_PRIVILEGES  = 0x0020;
		public const UInt32 TOKEN_ADJUST_GROUPS      = 0x0040;
		public const UInt32 TOKEN_ADJUST_DEFAULT     = 0x0080;
		public const UInt32 TOKEN_ADJUST_SESSIONID   = 0x0100;
		public const UInt32 TOKEN_READ               = (STANDARD_RIGHTS_READ | TOKEN_QUERY);
		public const UInt32 TOKEN_ALL_ACCESS         = (STANDARD_RIGHTS_REQUIRED | TOKEN_ASSIGN_PRIMARY |
				TOKEN_DUPLICATE | TOKEN_IMPERSONATE | TOKEN_QUERY | TOKEN_QUERY_SOURCE |
				TOKEN_ADJUST_PRIVILEGES | TOKEN_ADJUST_GROUPS | TOKEN_ADJUST_DEFAULT |
				TOKEN_ADJUST_SESSIONID);

		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern bool PrivilegeCheck(
			IntPtr ClientToken,
			ref PRIVILEGE_SET RequiredPrivileges,
			out bool pfResult);

		[DllImport("user32.dll", SetLastError = true)]
		static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[StructLayout(LayoutKind.Sequential)]
		public struct TOKEN_PRIVILEGES_ARRAY {
			public uint PrivilegeCount;
			// In case we need more than one, we'll leave space for a dozen
			// I suppose we could try
			// const int ANYSIZE_ARRAY = 12;
			// [MarshalAs(UnmanagedType.ByValArray, SizeConst = ANYSIZE_ARRAY)]
			// But we'll try that later, once we get it to work this way
			public LUID_AND_ATTRIBUTES  LAA_0;
			public LUID_AND_ATTRIBUTES  LAA_1;
			public LUID_AND_ATTRIBUTES  LAA_2;
			public LUID_AND_ATTRIBUTES  LAA_3;
			public LUID_AND_ATTRIBUTES  LAA_4;
			public LUID_AND_ATTRIBUTES  LAA_5;
			public LUID_AND_ATTRIBUTES  LAA_6;
			public LUID_AND_ATTRIBUTES  LAA_7;
			public LUID_AND_ATTRIBUTES  LAA_8;
			public LUID_AND_ATTRIBUTES  LAA_9;
			public LUID_AND_ATTRIBUTES  LAA_10;
			public LUID_AND_ATTRIBUTES  LAA_11;
		}

		[DllImport("advapi32.dll", SetLastError = true)]
		static extern bool GetTokenInformation(
			IntPtr	TokenHandle,
			_TOKEN_INFORMATION_CLASS TokenInformationClass,
			out TOKEN_PRIVILEGES_ARRAY TokenInformation,			// Pointer to output buffer
			uint	TokenInformationLength,
			out int ReturnLength
			);

		enum _TOKEN_INFORMATION_CLASS {
			TokenUser = 1,
			TokenGroups,
			TokenPrivileges,
			TokenOwner,
			TokenPrimaryGroup,
			TokenDefaultDacl,
			TokenSource,
			TokenType,
			TokenImpersonationLevel,
			TokenStatistics,
			TokenRestrictedSids,
			TokenSessionId,
			TokenGroupsAndPrivileges,
			TokenSessionReference,
			TokenSandBoxInert,
			TokenAuditPolicy,
			TokenOrigin,
			TokenElevationType,
			TokenLinkedToken,
			TokenElevation,
			TokenHasRestrictions,
			TokenAccessInformation,
			TokenVirtualizationAllowed,
			TokenVirtualizationEnabled,
			TokenIntegrityLevel,
			TokenUIAccess,
			TokenMandatoryPolicy,
			TokenLogonSid,
			TokenIsAppContainer,
			TokenCapabilities,
			TokenAppContainerSid,
			TokenAppContainerNumber,
			TokenUserClaimAttributes,
			TokenDeviceClaimAttributes,
			TokenRestrictedUserClaimAttributes,
			TokenRestrictedDeviceClaimAttributes,
			TokenDeviceGroups,
			TokenRestrictedDeviceGroups,
			TokenSecurityAttributes,
			TokenIsRestricted,
			MaxTokenInfoClass
		};

		[DllImport("kernel32.dll", EntryPoint = "FormatMessageW", SetLastError = true)]
		private static extern int FormatMessage(FormatMessageFlags dwFlags, IntPtr lpSource,
		   int dwMessageId, int dwLanguageId, System.Text.StringBuilder lpBuffer, int nSize, IntPtr[] Arguments);

		[Flags]
		public enum FormatMessageFlags {
			FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100,
			FORMAT_MESSAGE_IGNORE_INSERTS  = 0x00000200,
			FORMAT_MESSAGE_FROM_STRING     = 0x00000400,
			FORMAT_MESSAGE_FROM_HMODULE    = 0x00000800,
			FORMAT_MESSAGE_FROM_SYSTEM     = 0x00001000,
			FORMAT_MESSAGE_ARGUMENT_ARRAY  = 0x00002000,
			FORMAT_MESSAGE_MAX_WIDTH_MASK  = 0x000000ff
		}

		[DllImport("kernel32", CharSet=CharSet.Ansi, ExactSpelling=true, SetLastError=true)]
		static extern IntPtr GetProcAddress(IntPtr hModule, string procName);


		IntPtr hProcExp;

//--------------------------------------------------------------------------------------

		public DumpDesktopMemory() {
			InitializeComponent();

			DoTest();

			// TestHexDump();

			// if (this != null) return;			// TODO: Debug

			if (!Environment.Is64BitProcess) {
				msg("************* Sigh. Running only as a 32 bit process");
			}

			if (IsAdministrator()) {
				this.Text = "Dump Desktop Memory -- Running as Administrator";
				Application.DoEvents();
				// MessageBox.Show("Yes, I'm an administrator");
			} else {
				msg("Not running as Administrator");
#if false
				// Restart program and run as admin
				var exeName = Process.GetCurrentProcess().MainModule.FileName;
				ProcessStartInfo startInfo = new ProcessStartInfo(exeName);
				startInfo.Verb = "runas";
				startInfo.UseShellExecute = true;
				Process.Start(startInfo);
				// Application.Current.Shutdown();
				Application.Exit();
				// int Limit = 1000000000;
				// while (Limit-- > 0) {
				Application.DoEvents();
				// }
				return;
#endif
			}

#if false
			long Address = 0x7ffd6b980000;

			// IntPtr hWndDesktop = GetDesktopWindow();
			var hWndProcExp = FindWindow(null, "Program Manager");
			int PID;
			GetWindowThreadProcessId(hWndProcExp, out PID);
			hProcExp = GetProcessHandleFromHwnd(hWndProcExp);

			const uint PROCESS_VM_READ = 0x0010;
			IntPtr h = OpenProcess(PROCESS_VM_READ, false, PID);

			// MessageBox.Show($"h = {h:8X}");
			if (h == IntPtr.Zero) {
				var err = GetLastError();
				// MessageBox.Show($"err = {err}");
			}

			try {
				PrivilegeCheck();
			} catch (Exception ex) {
				MessageBox.Show("Call to Privilege Check failed - ex = " + ex.Message);
				return;
			}
			// MessageBox.Show(sb.ToString());

			Dump(hProcExp, new IntPtr(Address), 20);
#endif
			DoProcessRefresh();
		}

//---------------------------------------------------------------------------------------

		private void TestHexDump() {
			// var buf = new byte[] { 1, 2, 65, 0x61, 3, 88, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12 };
			var buf = new byte[128];
			for (byte i = (byte)'A'; i <= 'z'; i++) {
				buf[i - 'A'] = i;
			}
			// var hd = new LRS.HexDump().ShowHeader().ShowAddress().ShowHex().ShowText();
			var hd = new LRS.HexDump().ShowHex().ShowHeader().ShowAddress().ShowText();
			lbOutput.Items.Clear();
			// foreach (var item in hd.Dump(buf, (int)('z' - 'A' + 2), 32, 16, 0)) {
			foreach (var item in hd.Dump(buf, 32, 32, 16, 0)) {
				lbOutput.Items.Add(item);
			}
		}

//---------------------------------------------------------------------------------------

		private void DoTest() {
			GetDebugPrivilege.GetIt();
			// long Address = 0x7ffb030ccdd2L;
			long Address = 0x7ffd6b980000;

			var hWndProcExp = FindWindow(null, "Program Manager");
			int PID;
			GetWindowThreadProcessId(hWndProcExp, out PID);
			hProcExp = GetProcessHandleFromHwnd(hWndProcExp);

			const uint PROCESS_VM_READ = 0x0010;
			IntPtr h = OpenProcess(PROCESS_VM_READ, false, PID);

			Dump(hProcExp, new IntPtr(Address), 40960);
			// Dump(h, new IntPtr(Address), 20);
		}

//---------------------------------------------------------------------------------------

		void Dump(IntPtr hProcExp, IntPtr Address, int Length) {
			// MessageBox.Show($"Enter Dump({hProcExp}, {Address:X16}, {Length}");
			byte[] buf = new byte[Length];
			int nBytesRead;
			// msg(sb, "hProcExp = " << h << ", Address = " << a << ", buf = " << b);
			// Debugger.Break();
			try {
				var res = ReadProcessMemory(hProcExp, Address, buf, Length, out nBytesRead);
				if (!res) {
					var err = Marshal.GetLastWin32Error();
					msg($"ReadProcessMemory error {err}");
					return;
				}
			} catch (Exception ex) {
				MessageBox.Show($"ReadProcessMemory failed with err {Marshal.GetLastWin32Error()}, msg={ex.Message}");
				return;
			}

			var hd = new LRS.HexDump().ShowHex().ShowHeader().ShowAddress().ShowText();
			foreach (var line in hd.Dump(buf, (uint)Length, (ulong)Address.ToInt64())) {
				lbOutput.Items.Add(line);
			}

		}


//---------------------------------------------------------------------------------------

#if false
		bool PrivilegeCheck() {
			string lpszPrivilege = "SeDebugPrivilege";
			bool bEnablePrivilege = true;
			IntPtr token;
			PRIVILEGE_SET privset = new PRIVILEGE_SET();
			bool bResult;

			msg(sb, "Setting SeDebugPrivilege");
			// MessageBox.Show("PrivilegeCheck - About to setup <privset>");

			privset.PrivilegeCount = 1;
			// privset.Control = PRIVILEGE_SET.PRIVILEGE_SET_ALL_NECESSARY;
			privset.Control = 0;
			// privset.Privilege = new LUID_AND_ATTRIBUTES[1];
			privset.Attributes = 0;

			// // MessageBox.Show("PrivilegeCheck - About to call LookupPrivilegeValue");
			if (!LookupPrivilegeValue(null, lpszPrivilege, out privset.Luid)) {
				msg(sb, "Error: LookupPrivilegeValue");
				return false;
			}

			// // MessageBox.Show("PrivilegeCheck - About to call OpenProcessToken");
			if (!OpenProcessToken(GetCurrentProcess(), TOKEN_ALL_ACCESS, out token)) {
				msg(sb, "Error: OpenProcessToken");
				return false;
			} else {
				msg(sb, $"PrivilegeCheck.OpenProcessToken went OK");
			}

			msg(sb, "PrivilegeCheck - About to call PrivilegeCheck API");
			// Debugger.Break();
			/*
			Call to Privilege Check failed -ex = Cannot marshal field 'Luid' of type 'PRIVILEGE_SET': Invalid managed/ unmanaged type combination (this value type must be paired with Struct).
			*/
			if (!PrivilegeCheck(token, ref privset, out bResult)) {
				msg(sb, "Error: PrivilegeCheck");
				return false;
			} else {
				msg(sb, $"PrivilegeCheck.PrivilegeCheck went OK. bResult = {bResult}");
			}

			if (bResult) {
				// MessageBox.Show("PrivilegeCheck - We have debug privileges for the system");
				msg(sb, "We have debug privileges for the system");
			} else {
				// MessageBox.Show("PrivilegeCheck -Nope, Try again. Attempting to get it...");
				msg(sb, "Nope, Try again. Attempting to get it...");
			}

			// // MessageBox.Show("PrivilegeCheck - About to call OpenProcessToken again");
			if (!OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES, out token)) {
				msg(sb, $"OpenProcessToken() error {GetLastError()}");
				return false;
			} else {
				msg(sb, "OpenProcessToken went OK");
			}

			var sp = SetPrivilege(token, lpszPrivilege, bEnablePrivilege);
			msg(sb, $"SetPrivilege() return value: {sp}");
			Console.WriteLine(sb.ToString());
			return true;
		}

//---------------------------------------------------------------------------------------

		bool SetPrivilege(
			IntPtr hToken,              // access token handle
			string lpszPrivilege,       // name of privilege to enable/disable
			bool bEnablePrivilege       // to enable or disable privilege
			) {
			TOKEN_PRIVILEGES tp;
			LUID luid;

			// MessageBox.Show("SetPrivilege - About to call LookupPrivilegeValue");
			if (!LookupPrivilegeValue(
					null,               // lookup privilege on local system
					lpszPrivilege,      // privilege to lookup 
					out luid))          // receives LUID of privilege
			{
				// MessageBox.Show($"SetPrivilege: LookupPrivilegeValue error: {GetLastError()}");
				return false;
			} else {
				msg(sb, "LookupPrivilegeValue went OK");
			}

			tp.PrivilegeCount = 1;
			// tp.Privileges = new LUID_AND_ATTRIBUTES[1];
			// tp.Privileges[0].Luid = luid;
			tp.Luid = luid;
			if (bEnablePrivilege) {
				// tp.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;
				tp.Attributes = SE_PRIVILEGE_ENABLED;
			} else {
				// tp.Privileges[0].Attributes = 0;
				tp.Attributes = 0;
			}

			// var TokenInformationBuf = new byte[4096];
			int ReturnLength;
			TOKEN_PRIVILEGES_ARRAY tpa = new TOKEN_PRIVILEGES_ARRAY();
			bool bGTI = GetTokenInformation(hToken,
				_TOKEN_INFORMATION_CLASS.TokenPrivileges,
				out tpa,
				4096,
				out ReturnLength);
			if (bGTI) {
				msg(sb, "GetTokenInformation went OK. Dump of data to follow later.");
			} else {
				int LastErr = GetLastError();
				msg(sb, "GetTokenInformation failed");
				var gtisb = new StringBuilder(1024);
				int res = FormatMessage(FormatMessageFlags.FORMAT_MESSAGE_FROM_SYSTEM,
					IntPtr.Zero, LastErr, 0, gtisb, 1024, null);
				msg(sb, $"GTI FormatMessage Info: res={res}, msg={gtisb.ToString()}");
			}

			// Enable the privilege or disable all privileges.

			// TOKEN_PRIVILEGES PreviousState = new TOKEN_PRIVILEGES();
			// PreviousState.Privileges = new LUID_AND_ATTRIBUTES[1];
			// uint ReturnLengthInBytes;
			msg(sb, "SetPrivilege - About to call AdjustTokenPrivileges");
			string s = $"hToken={hToken}, tp.PrivilegeCount={tp.PrivilegeCount}, tp.luid={tp.Luid.HighPart:X}+{tp.Luid.LowPart:X8}, Attrs = {tp.Attributes:X}";
			msg(sb, s);
			s = $"Sizeof = {Marshal.SizeOf(tp)}";
			msg(sb, s);
			if (!AdjustTokenPrivileges(
				   hToken,
				   false,
				   ref tp,
				   (uint)Marshal.SizeOf(tp),
				   IntPtr.Zero,  /* ref PreviousState */
				   IntPtr.Zero  /* out ReturnLengthInBytes */)) {
				// If it returns error 5, then this is Access is Denied
				// MessageBox.Show($"SetPrivilege: AdjustTokenPrivileges error: {GetLastError()}");
				var LastErrorATP = GetLastError();
				// TODO: See FormatMessage 
				msg(sb, $"***** AdjustTokenPrivileges failed - {LastErrorATP} *****");
				return false;
			} else {
				msg(sb, "AdjustTokenPrivileges wen OK");
			}
			// MessageBox.Show("SetPrivilege - return from AdjustTokenPrivileges");

			var LastError = GetLastError();
			const int ERROR_NOT_ALL_ASSIGNED = 1300;
			if (LastError == ERROR_NOT_ALL_ASSIGNED) {
				msg(sb, "SetPrivilege/AdjustTokenPrivileges: ERROR_NOT_ALL_ASSIGNED");
				return false;
			} else {
				msg(sb, $"LastError (on AdjustTokenPrivileges) was {LastError}");
			}

			return true;
		}
#endif

//---------------------------------------------------------------------------------------

		private static bool IsAdministrator() {
			WindowsIdentity identity = WindowsIdentity.GetCurrent();
			WindowsPrincipal principal = new WindowsPrincipal(identity);
			return principal.IsInRole(WindowsBuiltInRole.Administrator);
		}

//---------------------------------------------------------------------------------------

		void msg(string s) {
			lbOutput.Items.Add(s);
		}

//---------------------------------------------------------------------------------------

		private void btnDump_Click(object sender, EventArgs e) {
			var addr = 0x7ffb030ccdd2L;
			Dump(hProcExp, new IntPtr(addr), 20);
		}

//---------------------------------------------------------------------------------------

		private void btnRefreshProcesses_Click(object sender, EventArgs e) {
			DoProcessRefresh();
		}

//---------------------------------------------------------------------------------------

		private void DoProcessRefresh() {
			cmbProcesses.Items.Clear();
			var qry = from p in Process.GetProcesses()
					  orderby p.ProcessName
					  select new ProcWithName(p);
			foreach (var proc in qry) {
				cmbProcesses.Items.Add(proc);
			}
		}

		private void cmbProcesses_SelectedIndexChanged(object sender, EventArgs e) {
			cmbModules.Items.Clear();
			var proc = (ProcWithName)cmbProcesses.SelectedItem;
			var qry = from m in proc.Proc.Modules.Cast<ProcessModule>()
					  orderby m.ModuleName
					  select m;
			try {
				foreach (ProcessModule mod in qry) {
					cmbModules.Items.Add(mod.ModuleName);
				}
			} catch {
					// Ignore errors, such as not authorized
			}
		}

		private void cmbModules_MouseClick(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Right) {
				// TODO: Doesn't work
				// TODO: Pop up menu with options. For now, hard code it
				var mod = (ProcessModule)cmbModules.SelectedItem;
				var hModule = GetModuleHandle(mod.FileName);
				var xx = GetProcAddress(hModule, mod.ModuleName);
			}
		}
	}

	//---------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------
	//-------------------------------------------------------------------------------------
	//-----------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------

	public class ProcWithName {
		public Process Proc;

//---------------------------------------------------------------------------------------

		public ProcWithName(Process Proc) {
			this.Proc = Proc;
		}
 
//---------------------------------------------------------------------------------------

	   public override string ToString() {
			string name = Proc.ProcessName;
			if (Proc.MainWindowTitle.Length > 0) {
				name += " - " + Proc.MainWindowTitle;
			}
			return name;
		}
	}
}
