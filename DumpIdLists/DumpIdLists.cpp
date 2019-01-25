// DumpIdLists.cpp : Defines the entry point for the console application.

// See https://msdn.microsoft.com/en-us/library/windows/desktop/ms680553(v=vs.85).aspx
// See https://msdn.microsoft.com/en-us/library/ms686701(v=vs.85).aspx
// http://www.cplusplus.com/forum/windows/30570/

#include "stdafx.h"

typedef unsigned char  uchar;
typedef unsigned short ushort;
typedef unsigned int   uint;
typedef unsigned long  ulong;

void Dump(HANDLE hProcExp, long long Address, int Length);
bool PrivilegeCheck(HANDLE PID);
BOOL SetPrivilege(
	HANDLE hToken,          // access token handle
	LPCTSTR lpszPrivilege,  // name of privilege to enable/disable
	BOOL bEnablePrivilege   // to enable or disable privilege
);
HANDLE FindWindowsExplorer();
long long PromptForHexAddress(char *Prompt);
long PromptForDecimalValue(char *Prompt);

//---------------------------------------------------------------------------------------

int main() {
	HANDLE hProcExp;
	// hProcExp = 54040;          // TODO:

	long long IdListAddress;
	char textbuf[100];

	// HWND hWndDesktop = GetDesktopWindow();
	DWORD PID = 78144;
	// GetWindowThreadProcessId(hWndDesktop, &PID);
	// auto xxx = GetProcessHandleFromHwnd(hWndDesktop);
	const int xPROCESS_VM_READ = 0x0010;
	hProcExp = OpenProcess(xPROCESS_VM_READ, false, PID);
	cout << "hProcExp = " << hex << hProcExp << endl;

	PrivilegeCheck(hProcExp);

	// hProcExp = (int)FindWindowsExplorer();
	// GetWindowThreadProcessId(hWndDesktop, (LPDWORD)&hProcExp);


	while (true) {
		cout << "Enter command (eXit, Hprocexp, IdList, Dump): ";
		cin >> textbuf;
		switch (toupper(textbuf[0])) {
		case 'X':
			cout << "Bye...";
			return 0;
		case 'H':
			hProcExp = (HANDLE)PromptForDecimalValue("Enter hProc for explorer.exe: ");
			// hProcExp = atoi(textbuf);
			break;
		case 'I':
			cout << "Nonce" << endl;
			break;
		case 'D':
			IdListAddress = PromptForHexAddress("Enter address: ");
			IdListAddress = 0x7ffb030ccdd2LL;
			Dump(hProcExp, IdListAddress, 64);
			break;
		default:
			cout << "Try again (X, H, I. D)" << endl;
			break;
		}
	}
	return 0;

#if 0
	cout << "Enter Explorer.exe hProcess: ";
	cin >> textbuf;
	// hProcExp = strtol(textbuf, 0, 16);

	while (IdListAddress != 0) {
		cout << "Enter ID List address in hex (no leading 0x) - 0 to quit: ";
		cin >> textbuf;
		IdListAddress = strtol(textbuf, 0, 16);
		if (IdListAddress == 0) {
			break;
		}
		IdListAddress = (long)(char *)main;   // TODO:
	}
	return 0;
#endif
}

//---------------------------------------------------------------------------------------

long long PromptForHexAddress(char *Prompt) {
	char buf[100];                // Plenty of room
	cout << Prompt;
	cin >> buf;
	long long l = std::strtoull(buf, 0, 16);
	return l;
}

//---------------------------------------------------------------------------------------

long PromptForDecimalValue(char *Prompt) {
	char buf[100];                // Plenty of room
	cout << Prompt;
	cin >> buf;
	return atol(buf);
}

//---------------------------------------------------------------------------------------

void Dump(HANDLE hProcExp, long long Address, int Length) {
	uchar buf[4096];
	SIZE_T nBytesRead;
	HANDLE hh  = hProcExp;
	auto h = (void *)hProcExp;
	auto a = (LPCVOID)(void *)Address;
	// auto a = (void far *)Address;
	auto b  = (LPVOID)buf;

#if 1
	Address = 0x7ffB030CCDD2LL;
	Address = 0x100000000LL;
#endif

	cout << "hProcExp = " << hh << ", Address = " << Address << ", buf = " << b << endl;
	auto res = ReadProcessMemory(hProcExp, Address, b, Length, &nBytesRead);
	if (res == 0) {
		auto err = GetLastError();
		cout << "ReadProcessMemory error " << err << endl;
		return;
	}
	// int len2 = *(ushort *)buf;
	cout << "nBytesRead = " << hex << nBytesRead << endl;
	
	// uchar *ptr = (uchar *)Address;
	// ushort len = *(ushort *)ptr;

	for (int i = 0; i < Length; i++) {
		printf("%02X ", buf[i]);
		if ((i % 8) == 7) {
			cout << " - ";
		}
	}
	cout << endl;

	for (int i = 0; i < Length; i++) {
		uchar c = buf[i];
		if (isgraph(c)) {
			cout <<c;
		}
		else {
			cout << '.';
		}
	}
	cout << endl;
}

//---------------------------------------------------------------------------------------

HANDLE FindWindowsExplorer() {
	  PROCESSENTRY32 pe32;
	  auto hProcessSnap = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);

	  // Set the size of the structure before using it.
	  pe32.dwSize = sizeof(PROCESSENTRY32);
	  Process32First(hProcessSnap, &pe32);
	  do {
		  cout << (char *)pe32.szExeFile << endl;
	  } while (Process32Next(hProcessSnap, &pe32));

	  return nullptr;
}

//---------------------------------------------------------------------------------------

bool PrivilegeCheck(HANDLE PID) {
	LPCTSTR lpszPrivilege = _T("SeDebugPrivilege");
	bool bEnablePrivilege = true;
	HANDLE token;
	PRIVILEGE_SET privset;
	BOOL bResult;

	printf("Setting SeDebugPrivilege\r\n");

	privset.PrivilegeCount = 1;
	privset.Control = PRIVILEGE_SET_ALL_NECESSARY;
	privset.Privilege[0].Attributes = 0;

	if (!LookupPrivilegeValue(NULL, lpszPrivilege, &privset.Privilege[0].Luid)) {
		cout << "Error: LookupPrivilegeValue" << endl;
		return false;
	}

	if (!OpenProcessToken(GetCurrentProcess(), TOKEN_ALL_ACCESS, &token)) {
		cout << "Error: OpenProcessToken" << endl;
		return false;
	}

	if (!PrivilegeCheck(token, &privset, &bResult)) {
		cout << "Error: PrivilegeCheck" << endl;
		return false;
	}

	if (bResult) {
		printf(" We have debug privileges for the system\r\n");
	} else {
		printf(" Nope, Try again. Attempting to get it...\r\n");
	}

	if(!OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES, &token))	{
	// if(!OpenProcessToken((HANDLE)PID, TOKEN_ADJUST_PRIVILEGES, &token))	{
		printf("OpenProcessToken() error %u\n", GetLastError());
		return false;
	}	

	printf("SetPrivilege() return value: %d\n\n", SetPrivilege(token, lpszPrivilege, bEnablePrivilege));
	return true;
}

//---------------------------------------------------------------------------------------

BOOL SetPrivilege(
	HANDLE hToken,          // access token handle
	LPCTSTR lpszPrivilege,  // name of privilege to enable/disable
	BOOL bEnablePrivilege   // to enable or disable privilege
	) 
{
	TOKEN_PRIVILEGES tp;
	LUID luid;

	if ( !LookupPrivilegeValue( 
			NULL,            // lookup privilege on local system
			lpszPrivilege,   // privilege to lookup 
			&luid ) )        // receives LUID of privilege
	{
		printf("SetPrivilege: LookupPrivilegeValue error: %u\n", GetLastError() ); 
		return FALSE; 
	}

	tp.PrivilegeCount = 1;
	tp.Privileges[0].Luid = luid;
	if (bEnablePrivilege) {
		tp.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;
	} else {
		tp.Privileges[0].Attributes = 0;
	}

	// Enable the privilege or disable all privileges.

	if ( !AdjustTokenPrivileges(
		   hToken, 
		   FALSE, 
		   &tp, 
		   sizeof(TOKEN_PRIVILEGES), 
		   (PTOKEN_PRIVILEGES) NULL, 
		   (PDWORD) NULL)) { 
		  printf("SetPrivilege: AdjustTokenPrivileges error: %u\n", GetLastError() ); 
		  return FALSE; 
	} 

	auto LastError = GetLastError();
	if (LastError == ERROR_NOT_ALL_ASSIGNED) {
		  printf("SetPrivilege: The token does not have the specified privilege. \n");
		  return FALSE;
	} 

	return TRUE;
}


