using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Bartizan;

using System.ServiceProcess;

namespace TestCRC64 {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

		private void btnGo_Click(object sender, EventArgs e) {
			BartCRC64	crc = new BartCRC64();
			crc.AddData(txtRaw.Text);
			lblCRC32.Text = string.Format("CRC = {0:X}", crc.GetCRC());

#if false
			ServiceController	sc = new ServiceController("SQL Server (MSSQLSERVER)");
			if (sc.Status == ServiceControllerStatus.Running) {
				sc.Stop();
			}
#endif
		}
	}
}
#if false
  // #define POLY64REV     0x95AC9329AC4BC9B5ULL
  // #define INITIALCRC    0xFFFFFFFFFFFFFFFFULL



void crc64(char *seq, char *res)
{
    int i, j, low, high;
    unsigned long long crc = INITIALCRC, part;
    static int init = 0;
    static unsigned long long CRCTable[256];
    
    if (!init)
    {
	init = 1;
	for (i = 0; i < 256; i++)
	{
	    part = i;
	    for (j = 0; j < 8; j++)
	    {
		if (part & 1)
		    part = (part >> 1) ^ POLY64REV;
		else
		    part >>= 1;
	    }
	    CRCTable[i] = part;
	}
    }
    
    while (*seq)
	crc = CRCTable[(crc ^ *seq++) & 0xff] ^ (crc >> 8);

    /* 
     The output is done in two parts to avoid problems with 
     architecture-dependent word order
     */
    low = crc & 0xffffffff;
    high = (crc >> 32) & 0xffffffff;
    sprintf (res, "%08X%08X", high, low);

    return;
}

void main (int argc, char *argv[])
{
    char *testseq1 = "MNIIQGNLVGTGLKIGIVVGRFNDFITSKLLSGAEDALLRHGVDTNDIDVAWVPGAFEIPFAAKKMAETKKYDAIITLGTVIRGATTSYDYVCNEAAKGIAQAANTTGVPVIFGIVTTENIEQAIERAGTKAGNKGVDCAVSAIEMANLNRSFE";
    char *testseq2 = "MNIIQGNLVGTGLKIGIVVGRFNDFITSKLLSGAEDALLRHGVDTNDIDVAWVPGAFEIPFAAKKMAETKKYDAIITLGDVIRGATTHYDYVCNEAAKGIAQAANTTGVPVIFGIVTTENIEQAIERAGTKAGNKGVDCAVSAIEMANLNRSFE"; /* Differs from 1st seq in two places */
    char result[20];


    crc64(testseq1, result);
    printf("The CRC-64 for sequence %s is %s\n", testseq1, result);

    crc64(testseq2, result);
    printf("The CRC-64 for sequence %s is %s\n", testseq2, result);
#endif

#if false
const ulong crc64::cc[] = {
    0x5a0127dd34af1e81ULL,  // [0]
    0x4ef12e145d0e3ccdULL,  // [1]
    0x16503f45acce9345ULL,  // [2]
    0x24e8034491298b3fULL,  // [3]
    0x9e4a8ad2261db8b1ULL,  // [4]
    0xb199aecfbb17a13fULL,  // [5]
    0x3f1fa2cc0dfbbf51ULL,  // [6]
    0xfb6e45b2f694fb1fULL,  // [7]
    0xd4597140a01d32edULL,  // [8]
    0xbd08ba1a2d621bffULL,  // [9]
    0xae2b680542730db1ULL,  // [10]
    0x8ec06ec4a8fe8f6dULL,  // [11]
    0xb89a2ecea2233001ULL,  // [12]
    0x8b996e790b615ad1ULL,  // [13]
    0x7eaef8397265e1f9ULL,  // [14]
    0xf368ae22deecc7c3ULL,  // [15]
};
#endif

#if false		// http://gcc.gnu.org/ml/gcc-bugs/2001-03/msg00506.html
	ulong
crc64(unsigned char const *buf, unsigned len)
{
        unsigned long long crc = 0;
        extern unsigned crctable[256];
        unsigned char c;

        while (len--) {
                c = ((unsigned char)crc ^ *buf++) & 255;
                crc >>= 8;
                crc ^= (unsigned long long)crctable[c] << 32;
        }
        return crc;
}

#endif

#if false		// http://apollo.backplane.com/matt/crc64.html
/*
 * UTIL/CRCTEST.C	- CRC tester, by Matt Dillon
 * 
 * This program is designed to test an N-bit CRC against a word dictionary
 * which you pipe into it.   It is not required for normal diablo operation.
 *
 * Warning: This program will eat a lot of memory with large sets.  If the
 * set is known to be unique, you can use -u to reduce the memory footprint.
 *
 * cat unique-words | CRCTEST [-u] [-v] [-q] [-h#]
 *
 *	-h#	set final hash size, in bits 16-64, Default is 64 bits.
 *	-u	assume unique input, do not store string contents
 *	-v	verbose output, print collisions
 *	-q	quiet output, do not print the count every 100,000 tests
 *
 * The expected number of collisions is (NSAMP * (NSAMP-1) / 2) / 2^CRCBITS.
 *
 * This is calculated through statistics.  If you had 7 samples and an 8 bit
 * CRC (256 slots), the number of collisions is
 *
 *		sample #1	0/256
 *		sample #2	1/256
 *		sample #3	2/256
 *		sample #4	3/256
 *		sample #5	4/256
 *		sample #6	5/256
 *		sample #7	6/256
 *	+	sample #8	7/256
 *	------------------------------
 *		    [ 8*(8-1)/2 ] / 2^CRCBITS
 *
 *      NOTE!! this only works if 2^CRCBITS is substantially larger then NSAMP
 *      because we aren't taking into account the fact that a prior samples
 *      may collide and not increment the chance of collision for later 
 *      samples.
 * 
 * So, for example, a 36 bit CRC with 1M samples should result in around 7
 * collisions.  A 42 bit CRC with 1M samples should result in around 0.1
 * collision.  A 42 bit CRC with 3 million samples should result in around 1 
 * collision.
 */

typedef unsigned int hint_t;	/* we want a 32 bit unsigned integer here */

typedef struct hash_t {
    hint_t	h1;
    hint_t	h2;
} hash_t;

typedef struct Hash {
    struct Hash *ha_Next;
    hash_t	ha_Hv;
} Hash;

// #define TESTHSIZE	(4 * 1024 * 1024)
// #define TESTHMASK	(TESTHSIZE - 1)

void inithash(void);
hash_t testhash(const char *p);

Hash	*HashAry[TESTHSIZE];
int	UniqueOpt;
int	HashLimit = 64;
int	VerboseOpt = 0;
int	QuietOpt = 0;
void	*rmalloc(int bytes);

int
main(int ac, char **av)
{
    char buf[256];
    int count = 0;
    int total = 0;
    int skip = 100000;
    int i;

    for (i = 1; i < ac; ++i) {
	char *p = av[i];

	if (*p == '-') {
	    p += 2;
	    switch(p[-1]) {
	    case 'u':
		UniqueOpt = 1;
		break;
	    case 'h':
		/*
		 * We can't go above 64 for obvious reasons.  We can't go
		 * below 16 due to the way I generate the polynomial.
		 */
		HashLimit = strtol(p, NULL, 0);
		if (HashLimit > 64 || HashLimit < 16) {
		    printf("valid values for -h between 16 & 64 inclusive\n");
		    exit(1);
		}
		break;
	    case 'q':
		QuietOpt = 1;
		break;
	    case 'v':
		VerboseOpt = 1;
		break;
	    default:
		fprintf(stderr, "Unknown option: %s\n", p - 2);
		exit(1);
	    }
	}
    }

    inithash();

    while (fgets(buf, sizeof(buf), stdin) != NULL) {
	int i;
	hash_t hv;
	Hash *h;
	char *s;

	for (s = strtok(buf, " ,\t\r\n"); s; s = strtok(NULL, " ,\t\r\n")) {
	    hv = testhash(s);
	    i = (hv.h1 ^ hv.h2) & TESTHMASK;
	    /* printf("%08x.%08x (%d) %s\n", hv.h1, hv.h2, i, s); */
	    for (h = HashAry[i]; h; h = h->ha_Next) {
		if (h->ha_Hv.h1 == hv.h1 && h->ha_Hv.h2 == hv.h2) {
		    if (UniqueOpt || strcmp(s, (char *)(h + 1)) != 0) {
			if (VerboseOpt) {
			    printf("Collision: %s\t%s\n", 
				s, 
				((UniqueOpt) ? "?" : (char *)(h + 1))
			    );
			}
			++count;
			++total;
		    }
		    break;
		}
	    }
	    if (h == NULL) {
		h = rmalloc(sizeof(Hash) + ((UniqueOpt) ? 0 : strlen(s) + 1));
		h->ha_Next = HashAry[i];
		h->ha_Hv = hv;
		if (UniqueOpt == 0)
		    strcpy((char *)(h + 1), s);
		HashAry[i] = h;
		++total;
	    }
	}
	if (total >= skip) {
	    if (QuietOpt == 0) {
		printf("Count %d/%d\n", count, total);
		fflush(stdout);
	    }
	    skip += 100000;
	}
    }
    printf("Count %d/%d\n", count, total);
    return(0);
}

/*
 * Poly: 0x00600340.00F0D50A
 *
 */

// #define HINIT1	0xFAC432B1UL
// #define HINIT2	0x0CD5E44AUL

// #define POLY1	0x00600340UL
// #define POLY2	0x00F0D50BUL

hash_t CrcXor[256];
hash_t Poly[64+1];

void
inithash(void)
{
    int i;

    /*
     * Polynomials to use for various crc sizes.  Start with the 64 bit
     * polynomial and shift it right to generate the polynomials for fewer
     * bits.  Note that the polynomial for N bits has no bit set above N-8.
     * This allows us to do a simple table-driven CRC.
     */

    Poly[64].h1 = POLY1;
    Poly[64].h2 = POLY2;
    for (i = 63; i >= 16; --i) {
	Poly[i].h1 = Poly[i+1].h1 >> 1;
	Poly[i].h2 = (Poly[i+1].h2 >> 1) | ((Poly[i+1].h1 & 1) << 31) | 1;
    }

    for (i = 0; i < 256; ++i) {
	int j;
	int v = i;
	hash_t hv = { 0, 0 };

	for (j = 0; j < 8; ++j, (v <<= 1)) {
	    hv.h1 <<= 1;
	    if (hv.h2 & 0x80000000UL)
		hv.h1 |= 1;
	    hv.h2 = (hv.h2 << 1);
	    if (v & 0x80) {
		hv.h1 ^= Poly[HashLimit].h1;
		hv.h2 ^= Poly[HashLimit].h2;
	    }
	}
	CrcXor[i] = hv;
    }
}

/*
 * testhash() - do the CRC.  The complexity is simply due to the programmable
 *		nature of the number of bits.   We extract the top 8 bits to
 *		use as a table lookup to obtain the polynomial XOR 8 bits at
 *		a time rather then 1 bit at a time.
 */

hash_t
testhash(const char *p)
{
    hash_t hv = { HINIT1, HINIT2 };

    if (HashLimit <= 32) {
	int s = HashLimit - 8;
	hint_t m = (hint_t)-1 >> (32 - HashLimit);

	hv.h1 = 0;
	hv.h2 &= m;

	while (*p) {
	    int i = (hv.h2 >> s) & 255;
	    /* printf("i = %d %08lx\n", i, CrcXor[i].h2); */
	    hv.h2 = ((hv.h2 << 8) & m) ^ *p ^ CrcXor[i].h2;
	    ++p;
	}
    } else if (HashLimit < 32+8) {
	int s2 = 32 + 8 - HashLimit;	/* bits in byte from h2 */
	hint_t m = (hint_t)-1 >> (64 - HashLimit);

	hv.h1 &= m;
	while (*p) {
	    int i = ((hv.h1 << s2) | (hv.h2 >> (32 - s2))) & 255;
	    hv.h1 = (((hv.h1 << 8) ^ (int)(hv.h2 >> 24)) & m) ^ CrcXor[i].h1;
	    hv.h2 = (hv.h2 << 8) ^ *p ^ CrcXor[i].h2;
	    ++p;
	}
    } else {
	int s = HashLimit - 40;
	hint_t m = (hint_t)-1 >> (64 - HashLimit);

	hv.h1 &= m;
	while (*p) {
	    int i = (hv.h1 >> s) & 255;
	    hv.h1 = ((hv.h1 << 8) & m) ^ (int)(hv.h2 >> 24) ^ CrcXor[i].h1;
	    hv.h2 = (hv.h2 << 8) ^ *p ^ CrcXor[i].h2;
	    ++p;
	}
    }
    /* printf("%08lx.%08lx\n", (long)hv.h1, (long)hv.h2); */
    return(hv);
}

void *
rmalloc(int bytes)
{
    static char *RBuf = NULL;
    static int	  RSize = 0;

    bytes = (bytes + 3) & ~3;

    if (bytes > RSize) {
	RBuf = malloc(65536);
	RSize = 65536;
    }
    RBuf += bytes;
    RSize -= bytes;
    return(RBuf - bytes);
}
	
#endif

namespace Bartizan {
	public class BartCRC64 {
		// const ulong POLY = 0xC96C5795D7870F42UL;
		// const ulong POLY = 0x92d8af2baf0e1e85UL;
		const ulong POLY = 0X42F0E1EBA9EA3693UL;

		// const ulong POLY = 0x000000000000001BUL;
		// const ulong POLY = 0xD800000000000000UL;

		// Note: Apparently, CRC64 of "MLWWEEVEDCYEREDVQKKTFTKWVNAQFSKFGKQHIENLFSDLQDGRRLLDLLEGLTGQ"
		//		 should be "E7236AA93DFAB56F"

		static bool TableIsInitialized = false;
		static ulong[] CRCTable = new ulong[256];

		ulong crc;

//---------------------------------------------------------------------------------------

		public BartCRC64() {
			if (!TableIsInitialized) {
				InitTable();
				// Init_CRC_Table();
				InitTable_Swiss();		}
			Reset();
		}

//---------------------------------------------------------------------------------------

		public ulong GetCRC() {
			return crc ^ ~0UL;
		}

//---------------------------------------------------------------------------------------

		public void Reset() {
			crc = ~0UL;
		}

//---------------------------------------------------------------------------------------

		public void AddData(byte[] buf) {
			foreach (byte b in buf) {
				crc = CRCTable[(crc ^ b) & 0xff] ^ (crc >> 8);
			}
		}

//---------------------------------------------------------------------------------------

		public void AddData(string str) {
			byte[] rawBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(str);
			AddData(rawBytes);
		}

//---------------------------------------------------------------------------------------

		private void InitTable() {
			ulong part;
			for (int i = 0; i < 256; i++) {
				part = (ulong)i;
				for (int j = 0; j < 8; j++) {
					if ((part & 1) != 0)
						part = (part >> 1) ^ POLY;
					else
						part >>= 1;
				}
				CRCTable[i] = part;
			}
			TableIsInitialized = true;
		}

//---------------------------------------------------------------------------------------

		private void InitTable_Swiss() {
			// swissknife.sourceforce.net/lib/SWISS/CRC64.pm
			uint []		CRCTable_l = new uint[256];
			uint []		CRCTable_h = new uint[256];
#if false
			const		uint POLYRev_l = 0x00000000U;
			const		uint POLYRev_h = 0xd8000000U;
#else
#if true
			const		uint POLYRev_l = 0x04C11DB7U;
			const		uint POLYRev_h = 0x00000000U;
#else
			const		uint POLYRev_l = 0xA9EA3693U;
			const		uint POLYRev_h = 0x42F0E1EBU;
#endif
#endif
			uint		part_l, part_h;
			bool		bLowBitOn;

			for (uint i = 0; i < 256; i++) {
				// part = (ulong)i;
				part_l = i;
				part_h = 0;
				for (int j = 0; j < 8; j++) {
					bLowBitOn = (part_l & 1) != 0;
					part_l >>= 1;
					if ((part_h & 1) != 0) {
						part_l |= 1U << 31;
					}

					part_h >>= 1;
					if (bLowBitOn) {
						part_l ^= POLYRev_l;
						part_h ^= POLYRev_h;
					}
				}
				CRCTable_l[i] = part_l;
				CRCTable_h[i] = part_h;
			}
			TableIsInitialized = true;
		}
	}
}
