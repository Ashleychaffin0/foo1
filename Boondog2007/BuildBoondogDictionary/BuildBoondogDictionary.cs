using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BuildBoondogDictionary {
	public partial class BuildBoondogDictionary : Form {
		public BuildBoondogDictionary() {
			InitializeComponent();
		}
	}
}

// BogDump1.bas
/*
DEFINT A-Z

DIM Hdr(26, 2) AS LONG
DIM SubDict(26, 26) AS INTEGER

DIM Dict AS STRING, Letter AS STRING

OPEN "c:\lrs\bas\words4.ix" FOR BINARY ACCESS READ SHARED AS #1

FOR i = 1 TO 26                     ' Read in Header
    GET #1, , Hdr(i, 1)
    GET #1, , Hdr(i, 2)
NEXT i

FOR i = 1 TO 26
    FOR j = 1 TO 26
        GET #1, , SubDict(i, j)
    NEXT j
NEXT i

Letter = "S"
nLetter = ASC(Letter) - ASC("A") + 1

Dict = STRING$(Hdr(nLetter, 2), " ")
GET #1, Hdr(nLetter, 1), Dict

FOR i = 1 TO 26
    ds = SubDict(nLetter, i) + 1          ' Dict Start
    PRINT Letter; CHR$(i + 64); " ";
    FOR j = 0 TO 1
        PRINT HEX$(ASC(MID$(Dict, ds + j, 1))); " ";
    NEXT j
    FOR j = 1 TO ASC(MID$(Dict, ds + 1, 1))
        PRINT MID$(Dict, ds + 1 + j, 1);
    NEXT j
    PRINT
NEXT i

CLOSE #1

*/


/*
DECLARE SUB NewWord (PrefLen AS INTEGER, Prefix AS STRING, Suffix AS STRING)
DECLARE SUB Initialize ()
DECLARE SUB FinishLetter (c AS STRING)
DECLARE SUB StartNewLetter (c AS STRING)

DEFINT A-Z

DIM SHARED MinLetters AS INTEGER    ' Ignore words shorter than this
DIM SHARED MaxLetters AS INTEGER    ' Ignore words longer than this
DIM SHARED LetterStart AS LONG      ' Where in file current letter begins
DIM SHARED SubDict(27)              ' SubDirectory for given letter
DIM SHARED Hdr2Start AS LONG        ' Where Subdirectory starts

DIM CurWord AS STRING, PrevWord AS STRING, WordLen AS INTEGER
DIM WordLengths(30)                 ' Assume no word longer than 30 chars
DIM NumLetter AS INTEGER            ' "A" <-> 1, "B" <-> 2, etc
DIM PrevLen AS INTEGER              ' Length of previous word

DIM HDrOfs AS LONG                  ' Offset into file for start of letter
DIM HdrLen AS INTEGER               ' Length of entry for a letter
DIM Level2Ofs(26) AS INTEGER        ' Offsets from start of entry for each
                                    ' two letter combo. e.g. if the current
                                    ' letter is "W", then these are the
                                    ' offsets for "WA", "WB",...,"WZ"

Initialize                          ' Scan cmdline, open files, setup gbls

PrevWord = " "                      ' " " (rather than "") to sort before "A"
LenPrev = 0                         ' Dummy previous word is empty

DO                                  ' Main loop to read new words
    LINE INPUT #1, CurWord          ' Get next word

    WordLen = LEN(CurWord)
    WordLengths(WordLen) = WordLengths(WordLen) + 1 ' Accumulate stats

    IF WordLen < MinLetters OR WordLen > MaxLetters THEN   ' Throw back small fish
        GOTO ugh1                   ' Where's C when we need it?
    END IF

    CurWord = UCASE$(CurWord)       ' Make sure words are consistent

    IF LEFT$(CurWord, 1) <> LEFT$(PrevWord, 1) THEN ' First letter change
        FinishLetter LEFT$(PrevWord, 1)  ' Pump out previous letter
        StartNewLetter LEFT$(CurWord, 1)  ' And start new one
    END IF

    IF LEFT$(PrevWord, 2) <> LEFT$(CurWord, 2) THEN ' Second letter change
        NumLetter = ASC(MID$(CurWord, 2, 1)) - ASC("A") + 1 ' "C" -> 3, etc
        SubDict(NumLetter) = SEEK(2) - LetterStart + 1  ' Where next entry
                                                        ' would start
        PRINT "2nd letter change from "; PrevWord; " to "; CurWord; " at ("; NumLetter; ")="; SubDict(NumLetter)
    END IF

    IF WordLen > PrevLen AND LEFT$(CurWord, PrevLen) = PrevWord THEN ' Check
                                    ' for "DOGS" after "DOG"
        NewWord PrevLen, PrevWord, MID$(CurWord, PrevLen + 1)
    ELSE
        FOR i = 1 TO PrevLen            ' Find common prefix
            IF LEFT$(PrevWord, i) <> LEFT$(CurWord, i) THEN ' Check each prefix
                NewWord i - 1, LEFT$(PrevWord, i - 1), MID$(CurWord, i)
                PrevWord = CurWord      ' Get new prdecessor for next time
                EXIT FOR                ' Assume two words in a row not identical
            END IF
        NEXT i
    END IF

ugh1:                               ' Fake label to simulate a Continue stmt
    PrevLen = WordLen               ' Remember for next time
LOOP UNTIL EOF(1)                   ' Until we run out of words

FinishLetter "Z"                    ' Don't forget to do this one!

CLOSE #1, #2

PRINT "Word Lengths"
FOR i = 1 TO 30
    IF WordLengths(i) <> 0 THEN
        PRINT i, WordLengths(i)
    END IF
NEXT i

PRINT
BEEP

SUB FinishLetter (c AS STRING)

' Update the master header with length info for this letter

    DIM CurFileEnd AS LONG
    DIM LetterLen AS LONG
    DIM byte AS STRING * 1
    DIM i AS INTEGER
    DIM NumLetter AS INTEGER

    IF c = " " THEN                     ' Ignore first time through
        EXIT SUB
    END IF

' Add a dummy entry to the end of the letter. This costs a bit of disk
' space but simplifies our logic for creating the sub-directory. The entry
' has a prefix length of 1 (meaning that this fake word starts with the
' current letter), and has a 1 byte suffix of &hFF.

' This simplifies our sub- directory logic as follows:
''''''''''''' Add commentary here...

    SubDict(27) = SEEK(2) - LetterStart + 1 ' Make sure we always have
                                            ' this entry

    byte = CHR$(1)                      ' Prefix length = 1
    PUT #2, , byte                      ' Out it goes
    PUT #2, , byte                      ' Length of suffix also = 1
    byte = CHR$(&HFF)                   ' Highest possible "letter"
    PUT #2, , byte                      ' Out it goes too

    CurFileEnd = SEEK(2)                ' Remember where we were

    SEEK #2, (ASC(c) - ASC("A")) * 2 * 4 + 1   ' Each of 2 entries are 4 bytes

    PUT #2, , LetterStart
    LetterLen = CurFileEnd - LetterStart
    PUT #2, , LetterLen
    PRINT "Letter "; c; " starts at"; STR$(LetterStart); " for"; STR$(LetterLen)

' Now fill in the SubDict array. Scan from right to left.

    PrevDict = SubDict(27)
    FOR i = 26 TO 1 STEP -1
        IF SubDict(i) = -1 THEN
            SubDict(i) = PrevDict
        ELSE
            PrevDict = SubDict(i)
        END IF
    NEXT i

' Now seek to the correct spot for the appropriate subdirectory and
' write it out

    NumLetter = ASC(c) - ASC("A")       ' "A" -> 0, "B" -> 1, etc

    SEEK #2, Hdr2Start + NumLetter * 26 * 2 ' 26 entries of 2 bytes each

    FOR i = 1 TO 26
        PUT #2, , SubDict(i)
    NEXT i

    SEEK #2, CurFileEnd                 ' Restore for subsequent dict writes

END SUB

SUB Initialize

    DIM cmd AS STRING                   ' Copy of COMMAND$
    DIM argv(4) AS STRING               ' Copy of parms found
    DIM word AS STRING                  ' Current ARGV[i]
    DIM i AS INTEGER, j AS INTEGER      ' Misc indices
    DIM WordStart AS INTEGER            ' Where current word starts
    DIM FileName AS STRING              ' For open error reporting
    DIM HdrStart AS LONG, HdrLen AS LONG' To initialize main header
    DIM Hdr2 AS INTEGER                 ' Ditto for sub header

' COMMAND$ isn't available in QBASIC. Sigh...
    IF LEN(COMMAND$) = 0 THEN
        cmd = "3  99 C:\LRS\BAS\BOGMAST.WDS  C:\LRS\BAS\BOGGLE.DCT " ' Note ending " "
    ELSE
        cmd = LTRIM$(COMMAND$)          ' Ensure no leading blanks
    END IF

    i = 0                               ' Index into <cmd>
    j = 0                               ' Index into our ARGV
    DO
        i = i + 1                       ' Look at each char in turn
        IF i > LEN(cmd) THEN EXIT DO    ' Don't scan past end of string
        IF MID$(cmd, i, 1) = " " THEN
            j = j + 1
            argv(j) = LEFT$(cmd, i - 1)
            IF j = 4 THEN EXIT DO       ' Don't care about other parms
            cmd = LTRIM$(MID$(cmd, i))  ' Start at next word
            i = 0                       ' Start scanning at beginning again
        END IF
    LOOP

    IF j <> 4 THEN
        PRINT "Error - Found only"; j; " parm";
        IF j = 1 THEN PRINT  ELSE PRINT "s"
        END
    END IF

    FOR i = 1 TO LEN(argv(1))               ' Check for valid numerics
        IF MID$(argv(1), i, 1) < "0" OR MID$(argv(1), i, 1) > "9" THEN
            PRINT "Error - First parm must be numeric"
            END
        END IF
    NEXT i

    FOR i = 1 TO LEN(argv(2))               ' Check for valid numerics
        IF MID$(argv(2), i, 1) < "0" OR MID$(argv(2), i, 1) > "9" THEN
            PRINT "Error - Second parm must be numeric"
            END
        END IF
    NEXT i

    MinLetters = VAL(argv(1))
    IF MinLetters < 3 OR MinLetters > 9 THEN    ' Reasonableness check
        PRINT "Error - First parm must be in the range 3..9"
        END
    END IF

    MaxLetters = VAL(argv(2))
    IF MaxLetters < MinLetters THEN     ' Reasonableness check
        PRINT "Error - Second parm must be >= First Parm"
        END
    END IF

' It would be nice to give reasonable error msgs if either of the following
' OPEN statements didn't work.
' I tried using ON ERROR RESUME NEXT and got a syntax error at compile time.
' I tried ON ERROR GOTO label and got an error at bind time.
' I then removed the ON ERRORs completely. That worked!

' N.B. The README file for DOS 5 says that contrary to the online
' QBASIC documentation, ON ERROR RESUME NEXT isn't supported. Sigh.

    FileName = argv(3)
    OPEN FileName FOR INPUT ACCESS READ SHARED AS #1

    FileName = argv(4)
    OPEN FileName FOR OUTPUT AS #2          ' Truncate file
    CLOSE #2                                ' So we can reopen
    OPEN FileName FOR BINARY AS #2          ' as binary

' The format of the output dictionary file is as follows:

' A header section preceeding the dictionary proper, followed by the
' dictionary itself.

' The header is divided into two parts. The first is a 26x2 matrix of LONGs.
' Each row represents a letter of the alphabet. The first entry is a seek
' address in the file pointing to where the dictionary entries for that
' letter start. The second is the length of the entries for that letter.
' This is sometimes referred to the sub-dictionary for this letter.

' The second part of the header is a 26x26 matrix of INTEGERs. Each row
' represents the corresponding letter of the alphabet. For each letter,
' there are 26 entries representing the offset into the letter's
' sub-dictionary. If the main header info identifies the start of each
' letter, then these are the offsets of the dictionary entries for each
' two-letter prefix. For example, if the main letter was "S", then there
' would be 26 entries identifying the start of words starting with "SA",
' "SB", "SC", etc.

''''''''''''' What if (as above) "BJ" is empty. Presumably we give the
''''''''''''' start of the one after it ("SK" or even "BL"). What about
''''''''''''' "ZX", "ZY" and "ZZ"?

' Finally we have the dictionary itself.

''''''''''''' Document the sucker.

    FOR i = 1 TO 26                     ' Main Header
        PUT #2, , HdrStart
        PUT #2, , HdrLen
    NEXT i

    Hdr2Start = SEEK(2)                 ' Remember where 2ndary header begins

    FOR i = 1 TO 26                     ' Secondary header
        FOR j = 1 TO 26
            PUT #2, , Hdr2
        NEXT j
    NEXT i

END SUB

SUB NewWord (PrefLen AS INTEGER, Prefix AS STRING, Suffix AS STRING)

    DIM byte AS STRING * 1

    byte = CHR$(PrefLen)                ' Length of prev word to start with
    PUT #2, , byte

    byte = CHR$(LEN(Suffix))            ' Length of suffix to follow
    PUT #2, , byte

    PUT #2, , Suffix                    ' The suffix itself

END SUB

SUB StartNewLetter (c AS STRING)

    DIM i AS INTEGER

    PRINT "Starting Letter "; c
    LetterStart = SEEK(2)                   ' Remember where we started

    FOR i = 1 TO 26                         ' Zero out sub dictionary
        SubDict(i) = -1                     ' for new letter
    NEXT i

END SUB

*/
