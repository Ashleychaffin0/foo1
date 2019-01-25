using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

// TODO: Go over things and do...
//  *   Sort by function names / struct names / enum names / whatever
//  *   Add/cleanup comments

namespace LRS {
    class ExternsViaPInvoke {

        [DllImport("kernel32.dll")]
        static extern Int32 GetLastError();

        [DllImport("advapi32.dll")]
        internal static extern bool LookupPrivilegeValue(string lpsystemname, string lpname, [MarshalAs(UnmanagedType.Struct)] out LUID lpLuid);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool OpenProcessToken(IntPtr ProcessHandle,
            uint DesiredAccess, out IntPtr TokenHandle);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentProcess();

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
            [Out] byte[] lpBuffer,
            int dwSize,
            out int lpNumberOfBytesRead);

        [DllImport("user32.dll", SetLastError = false)]
        static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int processId);

        [DllImport("oleacc.dll")]
        static extern IntPtr GetProcessHandleFromHwnd(IntPtr hWnd);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(
            uint /* ProcessAccessFlags */ processAccess,
            bool bInheritHandle,
            int processId
        );

        [Flags]
        public enum ProcessAccessType {
            PROCESS_TERMINATE = 0x0001,
            PROCESS_CREATE_THREAD = 0x0002,
            PROCESS_SET_SESSIONID = 0x0004,
            PROCESS_VM_OPERATION = 0x0008,
            PROCESS_VM_READ = 0x0010,
            PROCESS_VM_WRITE = 0x0020,
            PROCESS_DUP_HANDLE = 0x0040,
            PROCESS_CREATE_PROCESS = 0x0080,
            PROCESS_SET_QUOTA = 0x0100,
            PROCESS_SET_INFORMATION = 0x0200,
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

        [DllImport("advapi32.dll")]
        public static extern bool PrivilegeCheck(
            IntPtr ClientToken,
            ref PRIVILEGE_SET RequiredPrivileges,
            out bool pfResult);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    }
}
