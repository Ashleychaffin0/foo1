using System;
using System.Collections.Generic;
using System.Text;

namespace BalancedLineMerge {
	class BalancedLineMerge {
	}
}

// Code from LRSFC

#if false

bool BLMDirs(Directory &left,     Directory   &right,
			 Win32FileInfo             *pFileLeft, Win32FileInfo *pFileRight,
			 Directory::BLMReason code,       void *pParms) {
	string		subdirnameLeft, subdirnameRight;
	CompParms *pComp = (CompParms *)pParms;
	++pComp->nDirs;

	switch (code) {
	case Directory::BLM_NotInLeft:
		++pComp->nNotInLeft;
		MsgMissingFilename(pFileRight, false);
		break;
	case Directory::BLM_NotInRight:
		++pComp->nNotInRight;
		MsgMissingFilename(pFileLeft, true);
		break;
	case Directory::BLM_Same:
		subdirnameLeft  = left.GetCurDir()  + pFileLeft->sFileName  + "\\";
		subdirnameRight = right.GetCurDir() + pFileRight->sFileName + "\\";
		CompareFiles(subdirnameLeft, subdirnameRight, (CompParms *)pParms);
		break;
	}
	return true;
}

/******************************************************************************/
/* Do a standard balance line algorithm on this Directory and another, both   */
/* sorted by filename.                                                        */
/*                                                                            */
/* In principle, move your left finger down one list of files, and your right */
/* finger down the other. When the filenames match, call a user-supplied      */
/* routine to process the two files. This might be a file copy, or file       */
/* compare, or whatever.                                                      */
/*                                                                            */
/* We pass a void * structure to this routine, and it in turn passes it on to */
/* the user routine that's called. This is essentially a communications area, */
/* allowing the user routine to keep information over multiple calls. This    */
/* might contain, say, the highest return code so far, or statistics (e.g.    */
/* total number of files processed), or all of the above. The format of this  */
/* structure is totally up to the application, and may be NULL.               */
/*                                                                            */
/* When the filenames don't match, call the same routine as above (but with a */
/* different entry code), indicating whether it was missing from the left     */
/* column or the right column. The routine might create a new file, or give a */
/* "file missing" message, or whatever. Now move the appropriate finger down  */
/* to the next entry, and continue.                                           */
/*                                                                            */
/* When you run off the end of either list, exit the main loop, and process   */
/* the rest of the entries, if any.                                           */
/******************************************************************************/
int Directory::BalanceLineMerge(Directory &RightDir,    bool bDoFiles, 
								BLMProcess pProc,       void *pParms) {
	vector<Win32FileInfo *>::iterator	itLeft,  itRight,
									endLeft, endRight;
	string	        psLeftName, psRightName;
    const char      *pLeft, *pRight;
    int             compare_result;

	// Choose to iterate through files or dirs
	if (bDoFiles) {
		itLeft   = iFilesBegin();
		endLeft  = iFilesEnd();
		itRight  = RightDir.iFilesBegin();
		endRight = RightDir.iFilesEnd();
	} else {
		itLeft   = iDirsBegin();
		endLeft  = iDirsEnd();
		itRight  = RightDir.iDirsBegin();
		endRight = RightDir.iDirsEnd();
	}

    // TODO: Add quit immediately support, of some sort
    // Note: We had a tricky bug to track down in here. While the filenames
    // are sorted in a case insensitive fashion, the filename comparisons 
    // below were *not* case sensitive, initially. We ran into synchronization
    // problems when the left name started with "Xc", and the right one
    // started with "xb"! We've left in the original (simple, but wrong)
    // comparisons so you can see a bit better what's happening.
	while (itLeft != endLeft && itRight != endRight) {
		psLeftName  = (*itLeft)->sFileName;
        pLeft       = psLeftName.c_str();
		psRightName = (*itRight)->sFileName;
        pRight      = psRightName.c_str();
//		if (*psLeftName < *psRightName) {
        compare_result = stricmp(pLeft, pRight);    // Case insensitive compare
        if (compare_result < 0) {
			pProc(*this, RightDir, *itLeft, *itRight, BLM_NotInRight, pParms);
			++itLeft;
//		} else if (*psLeftName > *psRightName) {
        } else if (compare_result > 0) {
			pProc(*this, RightDir, *itLeft, *itRight, BLM_NotInLeft, pParms);
			++itRight;
		} else {
			pProc(*this, RightDir, *itLeft, *itRight, BLM_Same, pParms);
			++itLeft;
			++itRight;
		}
	}

	// Bleed the file lists, but hopefully both are now empty.
    // Note: At least one of these two loops will be empty.
	while (itLeft != endLeft) {
		pProc(*this, RightDir, *itLeft, *itRight, BLM_NotInRight, pParms);
		++itLeft;
	}

	while (itRight != endRight) {
		pProc(*this, RightDir, *itLeft, *itRight, BLM_NotInLeft, pParms);
		++itRight;
	}

	return 0;						// TODO: What does this return?
}
#endif
