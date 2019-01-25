using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace nsLRSPE {

    class LRSPE {
        _IMAGE_DOS_HEADER DosHeader;

//---------------------------------------------------------------------------------------

        public LRSPE(string FileName) {
            using (var mmf1 = MemoryMappedFile.CreateFromFile(FileName,
                FileMode.Open, null, 0)) {
                    using (var reader = mmf1.CreateViewAccessor(offset,
                    length, MemoryMappedFileAccess.Read)) {
                        // Read from MMF
                        buffer = new byte[length];
                        reader.ReadArray<byte>(0, buffer, 0, length);
                    }
            }
        }
}

//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------

    [StructLayout(LayoutKind.Sequential)]
    class _IMAGE_DOS_HEADER {  // DOS .EXE header
        public ushort e_magic;         // Magic number
        public ushort e_cblp;          // Bytes on last page of file
        public ushort e_cp;            // Pages in file
        public ushort e_crlc;          // Relocations
        public ushort e_cparhdr;       // Size of header in paragraphs
        public ushort e_minalloc;      // Minimum extra paragraphs needed
        public ushort e_maxalloc;      // Maximum extra paragraphs needed
        public ushort e_ss;            // Initial (relative) SS value
        public ushort e_sp;            // Initial SP value
        public ushort e_csum;          // Checksum
        public ushort e_ip;            // Initial IP value
        public ushort e_cs;            // Initial (relative) CS value
        public ushort e_lfarlc;        // File address of relocation table
        public ushort e_ovno;          // Overlay number
        //  public   ushort e_res[4];        // Reserved words
        public ushort e_res_1_0;
        public ushort e_res_1_1;
        public ushort e_res_1_2;
        public ushort e_res_1_3;
        public ushort e_oemid;         // OEM identifier (for e_oeminfo)
        public ushort e_oeminfo;       // OEM information; e_oemid specific
        //  public   ushort e_res2[10];      // Reserved words
        public ushort e_res2_1_0;      // Reserved words
        public ushort e_res2_1_1;      // Reserved words
        public ushort e_res2_1_2;      // Reserved words
        public ushort e_res2_1_3;      // Reserved words
        public ushort e_res2_1_4;      // Reserved words
        public ushort e_res2_1_5;      // Reserved words
        public ushort e_res2_1_6;      // Reserved words
        public ushort e_res2_1_7;      // Reserved words
        public ushort e_res2_1_8;      // Reserved words
        public ushort e_res2_1_9;      // Reserved words
        public long e_lfanew;        // File address of new exe header
    }
}
