// PlayWithWinMD_1.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"


int _tmain(int argc, _TCHAR* argv[])
{
	return 0;
}


HRESULT PrintMetaDataFilePathForTypeName(PCWSTR pszTypename);

int ShowUsage()
{
	wprintf(L"Usage: RoGetMetaDataFileSample TypeName\n");
	return -1;
}

int __cdecl wmain(int argc, WCHAR **argv)
{
	if (argc != 2)
	{
		return ShowUsage();
	}

	HRESULT hr = PrintMetaDataFilePathForTypeName(argv[1]);

	if (SUCCEEDED(hr))
	{
		return 0;
	}
	else
	{
		return -1;
	}
}

HRESULT PrintMetaDataFilePathForTypeName(PCWSTR pszTypename)
{
	HRESULT hr;
	HSTRING hstrTypeName = nullptr;
	HSTRING hstrMetaDataFilePath = nullptr;
	CComPtr<IMetaDataImport2> spMetaDataImport;
	mdTypeDef typeDef;

	hr = WindowsCreateString(
		pszTypename,
		static_cast<UINT32>(wcslen(pszTypename)),
		&hstrTypeName);

	if (SUCCEEDED(hr))
	{
		hr = RoGetMetaDataFile(
			hstrTypeName,
			nullptr,
			&hstrMetaDataFilePath,
			&spMetaDataImport,
			&typeDef);
	}

	if (SUCCEEDED(hr))
	{
		wprintf(L"Type %s was found in %s\n", pszTypename, WindowsGetStringRawBuffer(hstrMetaDataFilePath, nullptr));
	}
	else if (hr == RO_E_METADATA_NAME_NOT_FOUND)
	{
		wprintf(L"Type %s was not found!\n", pszTypename);
	}
	else
	{
		wprintf(L"Error %x occured while trying to resolve %s!\n", hr, pszTypename);
	}

	// Clean up resources.
	if (hstrTypeName != nullptr)
	{
		WindowsDeleteString(hstrTypeName);
	}

	if (hstrMetaDataFilePath != nullptr)
	{
		WindowsDeleteString(hstrMetaDataFilePath);
	}

	return hr;
}

