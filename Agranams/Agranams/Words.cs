using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agranams {
    class Words {

        List<string> SixLetterWords;

        public Words(string Filename) {
            SixLetterWords = new List<string>();
            using (var sr = new StreamReader(Filename)) {
                string line = sr.ReadLine();
                if (line != "$LINUX") {
                    System.Windows.Forms.MessageBox.Show("WORDS.TXT must be in LINUX format", "Agranams",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    throw new Exception();
                }
                while ((line = sr.ReadLine()) != null) {
                    if ((line.Length == 6) && (char.IsLower(line[0]))) {
                        SixLetterWords.Add(line);
                    }
                }
            }
        }
    }
}
