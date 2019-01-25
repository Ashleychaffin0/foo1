/**************************************************************************/
/*                                                                        */
/*    File: listwmf.c                                                     */
/*  Author: lmd                                                           */
/* Created: 8/7/95                                                        */
/*                                                                        */
/* Description: Print out a textual representation of a WMF metafile      */
/*                                                                        */
/**************************************************************************/

// #define	LRS_PRINT_PREFIX

#include <io.h>
#include <stdio.h>
#include <stdlib.h>
#include <stddef.h>
#include <string.h>
#include <windows.h>

// typedefs
typedef void (*PFV)();
typedef char *(*PFCP)();
typedef _int16 (*PFI)();
typedef double (*PFD)();

// defines
#define LIMIT_XMIN  0
#define LIMIT_XMAX  16383
#define LIMIT_YMIN  0
#define LIMIT_YMAX  12660

#define ISBETWEEN( x, min, max ) ( ( ( x ) >= ( min ) ) && ( ( x ) <= ( max ) ) )

#define DIM( x ) ( sizeof(x) / sizeof(x[0]) )

#ifdef _DEBUG

#include "assert.h"
#define ASSERT(f) assert(f)

#else

#define ASSERT(f) ((void)0)

#endif

enum PwError_t
{
	PWERROR_NO_ERROR,
	PWERROR_FILE_NOT_FOUND,
	PWERROR_INVALID_CLIPBOARD_FILE_ID,
	PWERROR_READING_CLIPBOARD_HEADER,
	PWERROR_NO_METAFILE_IN_CLP_FILE,
	PWERROR_NOT_A_WMF_OR_CLP_FILE,
	PWERROR_READING_PLACEABLE_METAFILE_KEY,
	PWERROR_READING_PLACEABLE_METAFILE_HEADER,
	PWERROR_INVALID_METAFILE_HEADER_TYPE,
	PWERROR_READING_METAFILE_HEADER,
	PWERROR_READING_METAFILE_PICT_HEADER,
	PWERROR_READING_METAFILE_RECORD,
	PWERROR_ALLOCATING_MEMORY_FOR_RECORD,
	PWERROR_UNABLE_TO_OPEN_FILE
};

#define PWERROR_FIRST  PWERROR_NO_ERROR
#define PWERROR_LAST   PWERROR_UNABLE_TO_OPEN_FILE

char *apszErrorMessage[] =
{
	"",
	"File not found.",
	"Invalid clipboard file ID.",
	"Error reading clipboard header.",
	"CLP file doesn't contain a valid metafile.",
	"Expected a WMF or CLP file.",
	"Error reading placeable metafile key.",
	"Error reading placeable metafile header." ,
	"Invalid metafile header type.",
	"Error reading metafile header.",
	"Error reading metafile pict header.",
	"Error reading metafile record.",
	"Unable to allocate memory for the following record."
};

/* clipboard data definitions */

#define  CLP_ID          0xC350

/* clipboard file header */
typedef struct {
	WORD FileIdentifier;
	WORD FormatCount;
} CLIPFILEHEADER;

/* clipboard format header */
typedef struct {
	WORD  FormatID;
	DWORD DataLen;
	DWORD DataOffset;
	char  Name[79];
} CLIPFILEFORMAT;

/* placeable metafile data definitions */

#define PLACEABLE_METAFILE_KEY  0x9AC6CDD7

typedef WORD HANDLE16;

typedef struct tagRECT16
{
	_int16 left;
	_int16 top;
	_int16 right;
	_int16 bottom;
} RECT16, *PRECT16;

/* placeable metafile header */
typedef struct {
	DWORD     key;
	HANDLE16  hmf;
	RECT16    bbox;
	WORD      inch;
	DWORD     reserved;
	WORD      checksum;
} PLACEABLE_METAFILE_HEADER, *PPLACEABLE_METAFILE_HEADER;

typedef struct tagMETARECORD_HEADER
{
	DWORD rdSize;
	WORD  rdFunction;
	WORD  rdParam[0];
} METARECORD_HEADER, *PMETARECORD_HEADER;

typedef struct {
	WORD wId;
	char *pszName;
} ID_NAME_PAIR, *PID_NAME_PAIR;

typedef struct tagMETAFILEPICT16 {
	_int16   mm;
	_int16   xExt;
	_int16   yExt;
	HANDLE16 hMF;
} METAFILEPICT16, *PMETAFILEPICT16;

typedef struct tagLOGFONT16 { // lf  
	_int16 lfHeight; 
	_int16 lfWidth; 
	_int16 lfEscapement; 
	_int16 lfOrientation; 
	_int16 lfWeight; 
	BYTE   lfItalic; 
	BYTE   lfUnderline; 
	BYTE   lfStrikeOut; 
	BYTE   lfCharSet; 
	BYTE   lfOutPrecision; 
	BYTE   lfClipPrecision; 
	BYTE   lfQuality; 
	BYTE   lfPitchAndFamily; 
	char   lfFaceName[LF_FACESIZE]; 
} LOGFONT16; 

// globals

BOOL g_in_polygon_set;
BOOL g_in_polygon;

// command line parameters
char* g_pszFilespec;
BOOL  g_bPrintSize = TRUE;
BOOL  g_bPrintData = TRUE;
BOOL  g_bPrintLargeDataSets = TRUE;
BOOL  g_bPrintHeaders = TRUE;
BOOL  g_bPrintFile = TRUE;
BOOL  g_bPrintRecords = TRUE;

void main(int, char *[]);
BOOL ProcessArguments( int argc, char *argv[] );
static void error( int nError );
static BOOL ProcessFile(char *pszPath, char *pszFilename);
static BOOL IsPlaceableMetafile( FILE* fp );
static BOOL ProcessPlaceableMetafileHeader( FILE* fp );
static BOOL ProcessMetafileHeader( FILE* fp );
static BOOL ProcessMetafilePictHeader( FILE* fp );
static BOOL ProcessMetafileRecords( FILE* fp );

static void PrintPlaceableMetafileHeader( PPLACEABLE_METAFILE_HEADER pPlaceableMetafileHeader );
static void PrintMetafileHeader( PMETAHEADER pMetafileHeader );
static void PrintMetafilePictHeader( PMETAFILEPICT16 pMetafilePict );
static void PrintMetafileRecord( PMETARECORD_HEADER pMetafileRecord );
static void Print_GenericMetafileRecord( PMETARECORD_HEADER pMetafileRecord );
static void Print_CreateFontIndirect( PMETARECORD_HEADER pMetafileRecord );
static void GetFunctionNameFromId( WORD wId, char* pszName, size_t stSizeofName );
static int GetTableIndexFromId( WORD wId, PID_NAME_PAIR pTable, int nCount );
static int GetTableIndexFromName( char* pszName, PID_NAME_PAIR pTable, int nCount );

static void Print_Rectangle(PMETARECORD_HEADER pMetafileRecord);	// LRS
static void Print_SetWindowExt(PMETARECORD_HEADER pMetafileRecord);	// LRS
static void Print_SetWindowOrg(PMETARECORD_HEADER pMetafileRecord);	// LRS
static void Print_CreatePenIndirect(PMETARECORD_HEADER pMetafileRecord);	// LRS
static void Print_SelectObject(PMETARECORD_HEADER pMetafileRecord);	// LRS
static void Print_CreateBrushIndirect(PMETARECORD_HEADER pMetafileRecord);	// LRS
static void Print_SetBkMode(PMETARECORD_HEADER pMetafileRecord);	// LRS
static void Print_SetBkColor(PMETARECORD_HEADER pMetafileRecord);	// LRS

void main(int argc, char *argv[])
{

	// V1.10 - added URL to banner
	printf( "LISTWMF V1.20\n" );
	printf( "Copyright 1996 Companion Software. All rights reserved.\n" );
	printf( "http://www.CompanionSoftware.com\n");
	printf( "\n" );

	if ( !ProcessArguments( argc, argv ) || !g_pszFilespec )
	{
		printf( "Description: Lists records in Windows metafiles (WMF and CLP files).\n" );
		printf( "\n" );
		printf( "     Syntax: listwmf filespec [/NO_FILE] [/NO_HEADERS] [/NO_RECORDS]\n" );
		printf( "                              [/NO_SIZE] [/NO_DATA]\n" );
		printf( "                              [/NO_LARGE_DATA_SETS] [/FUNCTIONS_ONLY]\n" );
		printf( "\n" );
		printf( "   Examples: listwmf myfile.wmf\n" );
		printf( "             listwmf *.wmf /NO_RECORDS\n" );
		printf( "             listwmf *.clp /NO_SIZE /NO_DATA\n" );
		printf( "             listwmf *.wmf /FUNCTIONS_ONLY\n" );
		printf( "\n" );
	}
	else {

		char szPath[_MAX_PATH];
		char szDrive[_MAX_DRIVE];
		char szDir[_MAX_DIR];

		struct _finddata_t info;
		long hFile;

		hFile = _findfirst( g_pszFilespec, &info );

		if (hFile != -1) {

			_splitpath( g_pszFilespec, szDrive, szDir, NULL, NULL );
			_makepath( szPath, szDrive, szDir, NULL, NULL );

			ProcessFile( szPath, info.name );

			while ( _findnext( hFile, &info ) == 0 )
				ProcessFile( szPath, info.name );

			_findclose( hFile );

		}
		else {
			error( PWERROR_FILE_NOT_FOUND );
			exit( 1 );
		}

	}

}

BOOL ProcessArguments( int argc, char *argv[] )
{

	enum ArgId_t {
		ARG_NO_SIZE, 
		ARG_NO_DATA,
		ARG_NO_LARGE_DATA_SETS,
		ARG_NO_HEADERS,
		ARG_NO_FILE,
		ARG_NO_RECORDS,
		ARG_FUNCTIONS_ONLY,
	};

	ID_NAME_PAIR ArgNameLookupTable[] = {
		ARG_NO_SIZE,            "NO_SIZE",
		ARG_NO_DATA,            "NO_DATA",               
		ARG_NO_LARGE_DATA_SETS, "NO_LARGE_DATA_SETS",
		ARG_NO_HEADERS,         "NO_HEADERS",               
		ARG_NO_FILE,            "NO_FILE",               
		ARG_NO_RECORDS,         "NO_RECORDS",               
		ARG_FUNCTIONS_ONLY,     "FUNCTIONS_ONLY",               
	};

	BOOL bProcessed = TRUE;
	int i;

	for ( i = 1; (i < argc) && bProcessed; ++i )
	{

		if ( argv[i][0] == '/' )
		{

			switch ( GetTableIndexFromName( &argv[i][1], ArgNameLookupTable, DIM( ArgNameLookupTable ) ) )
			{

			case ARG_NO_SIZE:
				g_bPrintSize = FALSE;
				break;

			case ARG_NO_DATA:
				g_bPrintData = FALSE;
				break;

			case ARG_NO_LARGE_DATA_SETS:
				g_bPrintLargeDataSets = FALSE;
				break;

			case ARG_NO_HEADERS:
				g_bPrintHeaders = FALSE;
				break;

			case ARG_NO_FILE:
				g_bPrintFile = FALSE;
				break;

			case ARG_NO_RECORDS:
				g_bPrintRecords = FALSE;
				break;

			case ARG_FUNCTIONS_ONLY:
				g_bPrintFile    = FALSE;
				g_bPrintHeaders = FALSE;
				g_bPrintSize    = FALSE;
				g_bPrintData    = FALSE;
				g_bPrintRecords = TRUE;
				break;

			default:
				bProcessed = FALSE;
				printf( "\7Invalid command line argument: %s\n", argv[i] );
				break;

			}

		}
		else g_pszFilespec = argv[i];

	}

	return bProcessed;

}

static void error( int nError )
{

	ASSERT( ISBETWEEN( nError, PWERROR_FIRST, PWERROR_LAST ) );

	printf( "\7Error PW%d: %s\n", 
		nError, 
		ISBETWEEN( nError, PWERROR_FIRST, PWERROR_LAST )
		? apszErrorMessage[ nError ] 
	: "Unknown error" );

}

static BOOL ProcessFile(char *pszPath, char *pszFilename)
{

	BOOL bProcessed = FALSE;
	char szFullFilename[ _MAX_PATH ];
	FILE *fp;

	// assemble the full filename
	strncpy(szFullFilename, pszPath, sizeof szFullFilename);
	strncat(szFullFilename, pszFilename, sizeof szFullFilename);

	if ( g_bPrintFile )
		printf( "File: %s\n", szFullFilename );

	/* try to open the file. */
	fp = fopen( szFullFilename, "rb" );
	ASSERT( fp );

	/* if opened successfully */
	if ( fp )  
	{

		char ext[ _MAX_EXT ];

		/* get the filename extension */
		_splitpath( szFullFilename, NULL, NULL, NULL, ext );

		/* if the file is a "traditional" or placeable metafile
		as per the normal naming conventions */
		if ( stricmp( ext, ".WMF" ) == 0 )
		{

			bProcessed = TRUE;

			// process optional placeable metafile header
			if ( IsPlaceableMetafile( fp ) )
				bProcessed = ProcessPlaceableMetafileHeader( fp );

			// process metafile header
			if ( bProcessed )
				bProcessed = ProcessMetafileHeader( fp );

			// process metafile records
			if ( bProcessed )
				bProcessed = ProcessMetafileRecords( fp );

		}

		/* file ext is not WMF so check to see if it is a clipboard CLP file */
		else if (stricmp(ext, ".CLP") == 0)  
		{

			CLIPFILEHEADER FileHeader;
			CLIPFILEFORMAT ClipHeader;
			DWORD HeaderPos;
			WORD i;

			/* read the clipboard file header */
			FileHeader.FormatCount = 0;
			fread(&FileHeader, sizeof(CLIPFILEHEADER), 1, fp);

			/* if this is not a valid clipboard file based on the file
			identifier of the file header */
			if (FileHeader.FileIdentifier != CLP_ID)  
				error( PWERROR_INVALID_CLIPBOARD_FILE_ID );

			HeaderPos = sizeof(CLIPFILEHEADER);

			/* search the formats contained within the clipboard file looking
			for a metafile.  Break if and when it is found */

			for ( i = 0; i < FileHeader.FormatCount; ++i )  {

				fseek(fp, HeaderPos, SEEK_SET);

				/* read the clipboard header found at current position */
				if ( fread( &ClipHeader, sizeof(ClipHeader), 1, fp ) != 1 )
					error( PWERROR_READING_CLIPBOARD_HEADER );

				/* increment the file offset */
				HeaderPos += sizeof(ClipHeader);

				/* if a metafile was found break */
				if (ClipHeader.FormatID == CF_METAFILEPICT)
					break;
			}

			/* was it really so? */
			if (ClipHeader.FormatID == CF_METAFILEPICT)  {

				fseek( fp, ClipHeader.DataOffset, SEEK_SET );

				// process metafile pict header
				bProcessed = ProcessMetafilePictHeader( fp );

				// process metafile header
				if ( bProcessed )
					bProcessed = ProcessMetafileHeader( fp );

				// process metafile records
				if ( bProcessed )
					bProcessed = ProcessMetafileRecords( fp );

			}

			/* a metafile was not found within the clipboard file */
			else error( PWERROR_NO_METAFILE_IN_CLP_FILE );

		}
		else error( PWERROR_NOT_A_WMF_OR_CLP_FILE );

	}
	else error( PWERROR_UNABLE_TO_OPEN_FILE );

	/* done with file so close it */
	fclose(fp);

	// separate one file's output from another
	printf( "\n" );

	return bProcessed;

}

static BOOL IsPlaceableMetafile( FILE* fp )
{

	BOOL bIsPlaceableMetafile;
	DWORD dwPlaceableMetafileKey;
	long lPosition;
	size_t stBlockCount;

	// save the original position in the file
	lPosition = ftell( fp );

	// read the first DWORD of the file 
	stBlockCount = fread( &dwPlaceableMetafileKey, sizeof dwPlaceableMetafileKey, 1, fp );
	ASSERT( stBlockCount == 1 );
	if ( stBlockCount != 1 )
		error( PWERROR_READING_PLACEABLE_METAFILE_KEY );

	// see if DWORD is placeable metafile key 
	bIsPlaceableMetafile = ( stBlockCount == 1 ) && ( dwPlaceableMetafileKey == PLACEABLE_METAFILE_KEY );

	// seek back to the original position in the file 
	fseek( fp, lPosition, SEEK_SET );

	return bIsPlaceableMetafile;

}

static BOOL ProcessPlaceableMetafileHeader( FILE* fp )
{

	BOOL bProcessed;
	PLACEABLE_METAFILE_HEADER PlaceableMetafileHeader;

	// read the placeable metafile header
	bProcessed = ( fread( &PlaceableMetafileHeader, sizeof PlaceableMetafileHeader, 1, fp ) == 1 );
	ASSERT( bProcessed );

	// print the placeable metafile header 
	if ( bProcessed ) 
		PrintPlaceableMetafileHeader( &PlaceableMetafileHeader );
	else error( PWERROR_READING_PLACEABLE_METAFILE_HEADER );

	return bProcessed;

}

static void PrintPlaceableMetafileHeader( PPLACEABLE_METAFILE_HEADER pPlaceableMetafileHeader )
{

	if ( g_bPrintHeaders )
	{

		printf( "Placeable Metafile Header\n" );

		printf( "   key  = 0x%08lX\n", pPlaceableMetafileHeader->key );
		printf( "   hmf  = 0x%04hX\n", pPlaceableMetafileHeader->hmf );
		printf( "   bbox = ( %hd, %hd ), ( %hd, %hd )\n", 
			pPlaceableMetafileHeader->bbox.left, pPlaceableMetafileHeader->bbox.top, 
			pPlaceableMetafileHeader->bbox.right, pPlaceableMetafileHeader->bbox.bottom );
		printf( "   inch = %hd\n", pPlaceableMetafileHeader->inch );
		printf( "   reserved = 0x%08lX\n", pPlaceableMetafileHeader->reserved );
		printf( "   checksum = 0x%04hX\n", pPlaceableMetafileHeader->checksum );

	}

}

static BOOL ProcessMetafileHeader( FILE* fp )
{

	BOOL bProcessed;
	METAHEADER MetafileHeader;

	// read the metafile header
	bProcessed = fread( &MetafileHeader, sizeof MetafileHeader, 1, fp ) == 1;
	ASSERT( bProcessed );

	// print the metafile header 
	if ( bProcessed )
	{

		// simple check for valid header type
		bProcessed = ( MetafileHeader.mtType == 1 ) || ( MetafileHeader.mtType == 2 );

		if ( bProcessed )
			PrintMetafileHeader( &MetafileHeader );
		else error( PWERROR_INVALID_METAFILE_HEADER_TYPE );

	}
	else error( PWERROR_READING_METAFILE_HEADER );

	return bProcessed;

}

static void PrintMetafileHeader( PMETAHEADER pMetafileHeader )
{

	if ( g_bPrintHeaders )
	{

		printf( "Metafile Header\n" );

		printf( "   mtType         = 0x%04hX\n", pMetafileHeader->mtType );
		printf( "   mtHeaderSize   = 0x%04hX\n", pMetafileHeader->mtHeaderSize );
		printf( "   mtVersion      = 0x%04hX\n", pMetafileHeader->mtVersion );
		printf( "   mtSize         = 0x%08lX\n", pMetafileHeader->mtSize );
		printf( "   mtNoObjects    = 0x%04hX\n", pMetafileHeader->mtNoObjects );
		printf( "   mtMaxRecord    = 0x%08lX\n", pMetafileHeader->mtMaxRecord );
		printf( "   mtNoParameters = 0x%04hX\n", pMetafileHeader->mtNoParameters );
		printf("\n");
	}

}

static BOOL ProcessMetafilePictHeader( FILE* fp )
{

	BOOL bProcessed;
	METAFILEPICT16 MetafilePict;

	// read the metafile pict header
	bProcessed = ( fread( &MetafilePict, sizeof MetafilePict, 1, fp ) == 1 );
	ASSERT( bProcessed );

	// print the metafile pict header 
	if ( bProcessed ) 
		PrintMetafilePictHeader( &MetafilePict );
	else error( PWERROR_READING_METAFILE_PICT_HEADER );

	return bProcessed;

}

static void PrintMetafilePictHeader( PMETAFILEPICT16 pMetafilePict )
{

	if ( g_bPrintHeaders )
	{

		printf( "Metafile Pict Header\n" );

		printf( "   mm   = %hd\n", pMetafilePict->mm );
		printf( "   xExt = %hd\n", pMetafilePict->xExt );
		printf( "   yExt = %hd\n", pMetafilePict->yExt );
		printf( "   hMF  = 0x%04hX\n", pMetafilePict->hMF );

	}

}

static BOOL ProcessMetafileRecords( FILE* fp )
{

	BOOL bDone;
	BOOL bProcessed;

	bDone = FALSE;
	bProcessed = TRUE;

	while ( bProcessed && !bDone && !feof(fp) ) 
	{

		METARECORD_HEADER MetaRecordHeader;

		int		size = sizeof MetaRecordHeader;
		// long	seek = ftell(fp);
		int		n = fread( &MetaRecordHeader, 1, size, fp );
		size_t	ofs = offsetof(METARECORD_HEADER, rdParam);
		if ((n >= ofs) && (MetaRecordHeader.rdFunction == 0)) {
			return true;
		}
		bProcessed = n == size;
		ASSERT( bProcessed );

		bDone = ( MetaRecordHeader.rdFunction == 0 );

		if ( bProcessed && !bDone )
		{

			PMETARECORD_HEADER pMetaRecord;

			pMetaRecord = (PMETARECORD_HEADER) malloc( MetaRecordHeader.rdSize * 2 );

			if ( pMetaRecord )
			{

				fseek(fp, -(long)(sizeof MetaRecordHeader), SEEK_CUR);
#if defined(LRS_PRINT_PREFIX)
				printf("%08X-", ftell(fp));		// LRS
#endif

				bProcessed = fread( pMetaRecord, MetaRecordHeader.rdSize * 2, 1, fp ) == 1;
				ASSERT( bProcessed );

				if ( bProcessed ) {
					PrintMetafileRecord( pMetaRecord );
				}
				else error( PWERROR_READING_METAFILE_RECORD );

				free( pMetaRecord );

			}
			else 
			{

				error( PWERROR_ALLOCATING_MEMORY_FOR_RECORD );
				printf("Function 0x%04hX, Size 0x%08lX\n", MetaRecordHeader.rdFunction, MetaRecordHeader.rdSize); 

				fseek(fp, (MetaRecordHeader.rdSize * 2) - sizeof MetaRecordHeader, SEEK_CUR);

			}

		}

	}

	return bProcessed;

}

static void PrintMetafileRecord( PMETARECORD_HEADER pMetafileRecord )
{

	if ( g_bPrintRecords )
	{

		// V1.20 - added code to enable record-specific printing if desired

		typedef void ( *PF_PRINT )( PMETARECORD_HEADER pMetafileRecord );

		typedef struct 
		{
			WORD    iType;
			PF_PRINT pfPrint;
		}
		PRINT_TABLE_ENTRY;

		static PRINT_TABLE_ENTRY PrintTable[]
		= {

			// V1.20 - handle META_CREATEFONTINDIRECT specially to get rid of garbage bytes in lfFaceName
			META_CREATEFONTINDIRECT,	Print_CreateFontIndirect,
			// LRS Added
			META_RECTANGLE,			Print_Rectangle,
			META_SETWINDOWEXT,		Print_SetWindowExt,
			META_SETWINDOWORG,		Print_SetWindowOrg,
			META_CREATEPENINDIRECT,	Print_CreatePenIndirect,
			META_SELECTOBJECT,		Print_SelectObject,
			META_CREATEBRUSHINDIRECT,	Print_CreateBrushIndirect,
			META_SETBKMODE,			Print_SetBkMode,
			META_SETBKCOLOR,			Print_SetBkColor,
		};

		int i;

#if defined(LRS_PRINT_PREFIX)
		printf("%04X: ", pMetafileRecord->rdFunction);
#endif

		// look for record in table
		for ( i = 0; i < DIM( PrintTable ); ++i )
			if ( PrintTable[ i ].iType == pMetafileRecord->rdFunction )
				break;

		// if we found it, print the specific the record
		if ( i < DIM( PrintTable ) )
			( *PrintTable[ i ].pfPrint )( pMetafileRecord );

		// else it's unknown, print a generic record
		else Print_GenericMetafileRecord( pMetafileRecord );

	}

}

static void Print_GenericMetafileRecord( PMETARECORD_HEADER pMetafileRecord )
{

#define NUM_INTS_PER_ROW 8
#define LARGE_DATA_SET_COUNT ( NUM_INTS_PER_ROW + 1 )

	char szName[ 80 ];

	GetFunctionNameFromId( pMetafileRecord->rdFunction, szName, sizeof szName );
	printf( "%s", szName );

	if ( g_bPrintSize )
		printf( " (%ld bytes)", pMetafileRecord->rdSize * 2 );

	if ( g_bPrintData && ( g_bPrintLargeDataSets || ( pMetafileRecord->rdSize < LARGE_DATA_SET_COUNT ) ) )
	{

		DWORD dw;
		DWORD dwSize;

		printf( "\n" );
		dwSize = pMetafileRecord->rdSize - 3; 
		for ( dw = 0; dw < dwSize; ++dw )
		{
			if ( ( dw % NUM_INTS_PER_ROW ) == 0 )
				printf( "   " );

			printf( "%6hd", pMetafileRecord->rdParam[ dw ] );

			if ( ( ( ( dw + 1 ) % NUM_INTS_PER_ROW ) == 0 ) || ( dw == ( dwSize - 1 ) ) )
				printf( "\n" );
			else printf( "  " );
		}

	}
	else printf( "\n" );

}

static void Print_CreateFontIndirect( PMETARECORD_HEADER pMetafileRecord )
{

	LOGFONT16* plogfont = ( LOGFONT16* )(&pMetafileRecord->rdParam[0]);

	printf( "   lfHeight         = %d\n", plogfont->lfHeight );
	printf( "   lfWidth          = %d\n", plogfont->lfWidth );
	printf( "   lfEscapement     = %d\n", plogfont->lfEscapement );
	printf( "   lfOrientation    = %d\n", plogfont->lfOrientation );
	printf( "   lfWeight         = %d\n", plogfont->lfWeight );
	printf( "   lfItalic         = %d\n", plogfont->lfItalic );
	printf( "   lfUnderline      = %d\n", plogfont->lfUnderline );
	printf( "   lfStrikeOut      = %d\n", plogfont->lfStrikeOut );
	printf( "   lfCharSet        = %d\n", plogfont->lfCharSet );
	printf( "   lfOutPrecision   = %d\n", plogfont->lfOutPrecision );
	printf( "   lfClipPrecision  = %d\n", plogfont->lfClipPrecision );
	printf( "   lfQuality        = %d\n", plogfont->lfQuality );
	printf( "   lfPitchAndFamily = %d\n", plogfont->lfPitchAndFamily );
	printf( "   lfFaceName       = %s\n", plogfont->lfFaceName );

}

#define LRS_SWAP(x) ((((DWORD)x) << 16) | ((DWORD)x) >> 16)

static void Print_Rectangle( PMETARECORD_HEADER pMetafileRecord ) {
	WORD	*pParms = &pMetafileRecord->rdParam[0];
	int		W = pParms[1] - pParms[3];
	int		H = pParms[0] - pParms[2];
	printf("Rectangle(L=%hd, T=%hd, R=%hd, B=%hd) [W=%hd, H=%hd]\n",
		pParms[3], pParms[2], pParms[1], pParms[0], W, H);
}

static void Print_SetWindowExt( PMETARECORD_HEADER pMetafileRecord ) {
	WORD	*pParms = &pMetafileRecord->rdParam[0];
	printf("SetWindowExt(x=%hd, y=%hd)\n",
		pParms[1], pParms[0]);
}

static void Print_SetWindowOrg( PMETARECORD_HEADER pMetafileRecord ) {
	WORD	*pParms = &pMetafileRecord->rdParam[0];
	printf("SetWindowOrg(x=%hd, y=%hd)\n",
		pParms[1], pParms[0]);
}

static void Print_CreatePenIndirect( PMETARECORD_HEADER pMetafileRecord ) {
#if 0	// We're looking at, essentially, 16 bit values, so we can't use the struct
	LOGPEN	*lp = &pMetafileRecord->rdParam[0];
	printf("CreatePenIndirect(Style=%d, Width=(%d, %d), Color=%08X)\n",
		lp->lopnStyle, lp->lopnWidth.x, lp->lopnWidth.y, LRS_SWAP(lp->lopnColor));
#endif
	WORD	*pParms = &pMetafileRecord->rdParam[0];
	printf("CreatePenIndirect(Style=%hd, Width=(%hd, %hd), Color=%08X)\n",
		pParms[0], pParms[1], pParms[2], *(LPDWORD)&pParms[3]);
}

#define	OBJ(name, value) {##name, value}
static void Print_SelectObject( PMETARECORD_HEADER pMetafileRecord ) {
	static struct {
		char	*name;
		WORD	value;
	} StockObjects [] = {
		{"WHITE_BRUSH",         0},
		{"LTGRAY_BRUSH",        1},
		{"GRAY_BRUSH",          2},
		{"DKGRAY_BRUSH",        3},
		{"BLACK_BRUSH",         4},
		{"NULL_BRUSH",          5},
//#define HOLLOW_BRUSH        NULL_BRUSH
		{"WHITE_PEN",           6},
		{"BLACK_PEN",           7},
		{"NULL_PEN",            8},
		// T'will serve for now
#define OEM_FIXED_FONT      10
#define ANSI_FIXED_FONT     11
#define ANSI_VAR_FONT       12
#define SYSTEM_FONT         13
#define DEVICE_DEFAULT_FONT 14
#define DEFAULT_PALETTE     15
#define SYSTEM_FIXED_FONT   16

#define DEFAULT_GUI_FONT    17

#define DC_BRUSH            18
#define DC_PEN              19
	};
	WORD	*pParms = &pMetafileRecord->rdParam[0];
	WORD	objID = pParms[0];
	char	*name = NULL;
	for (int i=0; i<DIM(StockObjects); ++i) {
		if (StockObjects[i].value == objID) {
			name = StockObjects[i].name;
			break;
		}
	}
	if (name != NULL) {
		printf("SelectObject(hGDIObj=%s (%hd))\n", name, objID);
	} else {
		char buf[10];
		itoa(objID, buf, 16);
		printf("SelectObject(hGDIObj=%s)\n", buf);
	}
}

static void Print_CreateBrushIndirect( PMETARECORD_HEADER pMetafileRecord ) {
#if 0	// We're looking at, essentially, 16 bit values, so we can't use the struct
	LOGBRUSH	*plb = &pMetafileRecord->rdParam[0];
	printf("CreateBrushIndirect(LogBrush=(Style=%04X, Color=%08X, Hatch=%08X)\n",
		LRS_SWAP(plb->lbStyle), plb->lbColor, LRS_SWAP(plb->lbHatch));
#endif
	WORD	*pParms = &pMetafileRecord->rdParam[0];
	printf("CreateBrushIndirect(LogBrush=(Style=%hd, Color=%08X, Hatch=%hd))\n",
		pParms[3], *(LPDWORD)&pParms[1], pParms[0]);
}

static void Print_SetBkMode( PMETARECORD_HEADER pMetafileRecord ) {
	WORD	*pParms = &pMetafileRecord->rdParam[0];
	printf("SetBkMode(Mode=%hd)\n",
		pParms[0]);
}

static void Print_SetBkColor( PMETARECORD_HEADER pMetafileRecord ) {
	WORD	*pParms = &pMetafileRecord->rdParam[0];
	printf("SetBkColor(Mode=%08X)\n",
		*((DWORD *)&pParms[0]));
}

static void GetFunctionNameFromId( WORD wId, char* pszName, size_t stSizeofName )
{

	/* lookup table for metafile functions */

	ID_NAME_PAIR FunctionNameLookupTable[] = {
		0x001E, "SAVEDC",               
		0x0035, "REALIZEPALETTE",       
		0x0037, "SETPALENTRIES",        
		0x00f7, "CREATEPALETTE",        
		0x00F8, "CREATEBRUSH",          
		0x0102, "SETBKMODE",            
		0x0103, "SETMAPMODE",           
		0x0104, "SETROP2",              
		0x0105, "SETRELABS",            
		0x0106, "SETPOLYFILLMODE",      
		0x0107, "SETSTRETCHBLTMODE",    
		0x0108, "SETTEXTCHAREXTRA",     
		0x0127, "RESTOREDC",            
		0x012A, "INVERTREGION",         
		0x012B, "PAINTREGION",          
		0x012C, "SELECTCLIPREGION",     
		0x012D, "SELECTOBJECT",         
		0x012E, "SETTEXTALIGN",         
		0x0139, "RESIZEPALETTE",        
		0x0142, "DIBCREATEPATTERNBRUSH",
		0x01f0, "DELETEOBJECT",         
		0x01F9, "CREATEPATTERNBRUSH",   
		0x0201, "SETBKCOLOR",           
		0x0209, "SETTEXTCOLOR",         
		0x020A, "SETTEXTJUSTIFICATION", 
		0x020B, "SETWINDOWORG",         
		0x020C, "SETWINDOWEXT",         
		0x020D, "SETVIEWPORTORG",       
		0x020E, "SETVIEWPORTEXT",       
		0x020F, "OFFSETWINDOWORG",      
		0x0211, "OFFSETVIEWPORTORG",    
		0x0213, "LINETO",               
		0x0214, "MOVETO",               
		0x0220, "OFFSETCLIPRGN",        
		0x0228, "FILLREGION",           
		0x0231, "SETMAPPERFLAGS",       
		0x0234, "SELECTPALETTE",        
		0x02FA, "CREATEPENINDIRECT",    
		0x02FB, "CREATEFONTINDIRECT",   
		0x02FC, "CREATEBRUSHINDIRECT",  
		0x02FD, "CREATEBITMAPINDIRECT", 
		0x0324, "POLYGON",              
		0x0325, "POLYLINE",             
		0x0400, "SCALEWINDOWEXT",       
		0x0412, "SCALEVIEWPORTEXT",     
		0x0415, "EXCLUDECLIPRECT",      
		0x0416, "INTERSECTCLIPRECT",    
		0x0418, "ELLIPSE",              
		0x0419, "FLOODFILL",            
		0x041B, "RECTANGLE",            
		0x041F, "SETPIXEL",             
		0x0429, "FRAMEREGION",          
		0x0436, "ANIMATEPALETTE",       
		0x0521, "TEXTOUT",              
		0x0538, "POLYPOLYGON",          
		0x061C, "ROUNDRECT",            
		0x061D, "PATBLT",               
		0x0626, "ESCAPE",               
		0x062F, "DRAWTEXT",             
		0x06FE, "CREATEBITMAP",         
		0x06FF, "CREATEREGION",
		0x0817, "ARC",                  
		0x081A, "PIE",                  
		0x0830, "CHORD",                
		0x0922, "BITBLT",               
		0x0940, "DIBBITBLT",            
		0x0a32, "EXTTEXTOUT",           
		0x0B23, "STRETCHBLT",           
		0x0b41, "DIBSTRETCHBLT",        
		0x0d33, "SETDIBTODEV",          
		0x0f43, "STRETCHDIB",           
	};

	int i;

	i = GetTableIndexFromId( wId, FunctionNameLookupTable, DIM( FunctionNameLookupTable ) );

	// if we found the entry, get its name - else "make up a name"
	if ( i != -1 )
		strncpy( pszName, FunctionNameLookupTable[i].pszName, stSizeofName );
	else sprintf( pszName, "Unknown function: %hX", wId );

}

static int GetTableIndexFromId( WORD wId, PID_NAME_PAIR pTable, int nCount )
{

	int i;
	int nIndex = -1;

	for ( i = 0; i < nCount; ++i )
		if ( wId == pTable[i].wId )
		{
			nIndex = i;
			break;
		}

		return nIndex;

}

static int GetTableIndexFromName( char* pszName, PID_NAME_PAIR pTable, int nCount )
{

	int i;
	int nIndex = -1;

	for ( i = 0; i < nCount; ++i )
		if ( stricmp( pszName, pTable[i].pszName ) == 0 )
		{
			nIndex = i;
			break;
		}

		return nIndex;

}
