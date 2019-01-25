using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BingLRS {
    public partial class BingImages {
        private const string TARGET_DIR = @"D:\LRS\BingImages_2";
        private const string BING = "http://www.bing.com";
        private const string IMG_URL = "http://www.bing.com/HPImageArchive" +
                                       ".aspx?format=xml&idx=0&n={0}&mkt={1}";
        // private static string[] Markets = new string[] { "en-US", "zh-CN", 
       //                                "ja-JP", "en-AU", "en-UK", 
        //                               "de-DE", "en-NZ", "en-CA" };
        private static string[] Markets = new string[] { "en-US" };
        private const int NUMBER_OF_IMAGES = 100;

        /// <summary>
        /// Download images from Bing
        /// </summary>
        public void DownLoadImages() {
            // Make sure destination folder exists
            ValidateDownloadPath();
            XDocument doc = null;
            WebRequest request = null;

            var sw = new Stopwatch();
            sw.Start();

            // Because each market can have different images
            // cycle through each of them
            foreach (string market in Markets) {
                // Form the URL based on market
                // Since this will be run once per day only 1 image needs to
                // be downloaded
                string url = string.Format(IMG_URL, NUMBER_OF_IMAGES, market);
                request = WebRequest.Create(url);
                using (Stream stream = request.GetResponse().GetResponseStream()) {
                    // Load the stream into and XDocument for processing
                    doc = XDocument.Load(stream);
                }

                // Iterate through the image elements
                foreach (XElement image in doc.Descendants("image")) {
                    // SaveImage(image.Element("url").Value);
                    SaveImage(image);
                }
            }

            sw.Stop();
            string msg = string.Format("Done in {0}", sw.Elapsed);
            MessageBox.Show(msg);
        }

        /// <summary>
        /// Save image from the give URL to disk
        /// </summary>
        /// <param name="url">URL of image to save</param>
        private async void SaveImage(XElement elem) {
            string url = elem.Element("url").Value;
            string suffix = Path.GetExtension(url);
            string copyright = elem.Element("copyright").Value.Replace('/', '-').Replace('"', '\'').Replace(':', ' ').Replace('\t', ' ');
            // Images can be duplicated between markets
            // so to avoid duplicates from being downloaded
            // get the unique name based on the image number in the URL
            // string filename = GetImageName(url);
            string filename = Path.Combine(TARGET_DIR, copyright) + suffix;
            if (!File.Exists(filename)) {
                Msg("About to download and save " + filename);
                // URL is relative so form the absolute URL
                WebRequest request = WebRequest.Create(BING + url);
                var response = await request.GetResponseAsync();
                Stream stream = response.GetResponseStream();
                Image img = Image.FromStream(stream);
                img.Save(filename);
                stream.Close();
            }
        }

        /// <summary>
        /// Create filename for image based on URL
        /// </summary>
        /// <param name="url">Image URL</param>
        /// <returns>FQN for saving image to</returns>
        private string GetImageName(string url) {
            // URL is in this format /fd/hpk2/DiskoBay_EN-US1415620951.jpg
            // Extract the image number
            Regex reg = new Regex(@"[0-9][0-9][0-9]+\w");
            Match m = reg.Match(url);
            // Should now have 1415620951 from above example
            // Create path to save image to
            return string.Format(@"{0}\{1}.jpg", TARGET_DIR, m.Value);
        }

//---------------------------------------------------------------------------------------

        /// <summary>
        /// Check if download path exist and create if necessary
        /// </summary>
        private void ValidateDownloadPath() {
            if (!Directory.Exists(TARGET_DIR)) {
                Directory.CreateDirectory(TARGET_DIR);
            }
        }

//---------------------------------------------------------------------------------------

        void Msg(string msg) {
            // lbMsgs.Items.Insert(0, msg);
            Application.DoEvents();
        }
    }
}
