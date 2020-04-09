using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsGetSciAm {
    class IssueDocInfo {
        public int SeqNo;
        public string Description;
        public string href;

//---------------------------------------------------------------------------------------

        public IssueDocInfo(int SeqNo, string Description, string href) {
            this.SeqNo = SeqNo;
            this.Description = Cleanup(Description);
            this.href = href;
        }

//---------------------------------------------------------------------------------------

        // Sanitize the Description to remove any system-reserved characters (e.g 
        // <, >, |, :, etc)
        private string Cleanup(string Description) {
            var BadChars = Path.GetInvalidFileNameChars();
            foreach (char c in BadChars) {
                Description = Description.Replace(new string(c, 1), "..");
            }
            return Description.Trim();
        }

//---------------------------------------------------------------------------------------

        public override string ToString() {
            return SeqNo.ToString("0# ") + Description;
        }
    }
}
