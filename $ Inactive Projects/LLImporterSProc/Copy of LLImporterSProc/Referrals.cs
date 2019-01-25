// Copyright (c) 2003-2004 by Bartizan Data Systems, LLC

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace Bartizan.Importer {
    public class Referrals {

        public string       DefaultFromPartner = null;
        public string       DefaultToPartner = null;
        public string       DefaultProvider = null;

        public List<string>     FromPartners;
		public List<string>		ToPartners;
		public List<string>		Providers;

//---------------------------------------------------------------------------------------

        public Referrals() {
			FromPartners = new List<string>();
			ToPartners	 = new List<string>();
			Providers	 = new List<string>();
        }

//---------------------------------------------------------------------------------------

        /// <summary>
        /// This enum is used only inside the ParseSetupFile routine, to assist in a
        /// simple FSM. What I'd <i>like</i> to do is to define the enum inside the
        /// function, so no other method in this class can even see it, much less use
        /// it by mistake. But the compiler frowns on my doing that, so here it is. Sigh.
        /// </summary>
        private enum State {    // Used only in ParseSetupFile
            ScanningHeader,     // Scanning DefXX
            ScanningFrom,       // Scanning <@@FROMPARTNER@@>
            ScanningTo,         // Scanning <@@TOPARTNER@@>
            ScananingProviders  // Scanning <@@PROVIDER@@>
        }

//---------------------------------------------------------------------------------------

        public void ParseSetupFile(BartIniFile suf) {

            if (! suf.FindSection("REFERRALS"))
                return;

            State       CurState = State.ScanningHeader;    // Simple FSM
            string      line;
            string []   fields;

            while ((line = suf.ReadLine()) != null) {
                line = line.Trim();
                if (line.Length == 0)
                    continue;
                switch (line.ToUpper()) {
                case "<@@FROMPARTNER@@>":
                    CurState = State.ScanningFrom;
                    continue;
                case "<@@TOPARTNER@@>":
                    CurState = State.ScanningTo;
                    continue;
                case "<@@PROVIDER@@>":
                    CurState = State.ScananingProviders;
                    continue;
                }


                switch (CurState) {
                case State.ScanningHeader:
                    fields = line.Split(new char [] {'='}, 2);
                    // TODO: Make sure we have exactly 2 fields
                    fields[0] = fields[0].Trim();
                    fields[1] = fields[1].Trim();
                    if (string.Compare(fields[0], "DEFFROMPARTNER") == 0) {
                        DefaultFromPartner = fields[1];
                    } else if (string.Compare(fields[0], "DEFTOPARTNER") == 0) {
                        DefaultToPartner = fields[1];
                    } else if (string.Compare(fields[0], "DEFPROVIDER") == 0) {
                        DefaultProvider = fields[1];
                    } else {
                        // TODO: Set an error of some sort
                    }
                    break;
                case State.ScanningFrom:
                    FromPartners.Add(line);
                    break;
                case State.ScanningTo:
                    ToPartners.Add(line);
                    break;
                case State.ScananingProviders:
                    Providers.Add(line);
                    break;
                }
            }
        }
    }
}
