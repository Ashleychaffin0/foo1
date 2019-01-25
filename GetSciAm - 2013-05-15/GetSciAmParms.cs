using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsGetSciAm {
    public class GetSciAmParms {
        public string SourceDirectory;				// e.g. C:\Downloads
        public string TargetDirectory;				// e.g. C:\$ SciAm
        public string FilenamePrefix;				// e.g. SciAm - %Y - %m - %n - 

        public string UserId;						// e.g. lrs5
        public string Password;						// Ah, but that would be telling

        public string SciAmDigitalUrl;				// e.g. http://...

        // Various strings used in the code that might, some day, change
        public string ArticleUrlPrefix;				// Distinguish articles from other links
        public string SelectYearUrl;				// To go to page for Year <n>

        // Other config parms
        public int MaxFileCopyTries;				// One per second, so, 60 maybe

//---------------------------------------------------------------------------------------

        public GetSciAmParms() {
            SourceDirectory = @"C:\Downloads\SciAmTemp";
            TargetDirectory = @"C:\$$LRS-SciAm";
            FilenamePrefix = "SciAm - %YM-%M - %n - ";

            UserId = "lrs5";
            Password = "lrs5_Sciam";

            SciAmDigitalUrl = "https://www.sciamdigital.com/index.cfm?fa=Account.LoginUser";

            ArticleUrlPrefix = "http://www.sciamdigital.com/index.cfm?fa=Products.ViewIssuePreview&ARTICLEID_CHAR=";
            SelectYearUrl = "http://www.sciamdigital.com/index.cfm?fa=Products.ViewBrowseCategoryList&CATEGORY_CHAR={0}:1";

            MaxFileCopyTries = 60;
        }
    }
}
