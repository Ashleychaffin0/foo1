// TODO:
//  *   D:\LRS-9450\C#\$ Inactive Projects\SnarfG\SnarfG.cs

//  *   See http://www.codeproject.com/Articles/151937/Bing-Image-Download

/*
 * Through some investigation, I found the image information is available from Bing using 
 * this URL: http://www.bing.com/HPImageArchive.aspx?format=xml&idx=0&n=1&mkt=en-US.

As the format parameter indicates, this will return an XML formatted stream that contains
 * information such as the date the images is for, the relative URL, description, and copyright information.

The idx parameter tells where you want to start from. 0 would start at the current day,
 * 1 the previous day, 2 the day before that, etc. For instance, if the date were 1/30/2011, 
 * using idx = 0, the file would start with 20110130; using idx = 1, it would start with 20110129; and so forth.

The n parameter tells how many images to return. n = 1 would return only one, n = 2 would return two, and so on.

The mkt parameter tells which of the eight markets Bing is available for you would like images from.
 * The valid values are: en-US, zh-CN, ja-JP, en-AU, en-UK, de-DE, en-NZ, en-CA.
*/


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BingLRS {
    public partial class BingLRS : Form {

        string ImgDir = @"D:\LRS\BingImages_2";
        ImageList ImgList = new ImageList();

//---------------------------------------------------------------------------------------

        public BingLRS() {
            InitializeComponent();

            var bi = new BingImages();
            bi.DownLoadImages();

            this.Text = ImgDir;
            SetupImages();
        }

//---------------------------------------------------------------------------------------

        private void SetupImages() {
            lvImages.Items.Clear();
            ImgList.Images.Clear();

            ImgList.ImageSize = new Size(4 * 64, 3 * 64); // 4::3
            var PathNames = from di in Directory.EnumerateFiles(ImgDir, "*.jpg")
                            orderby di
                            select di;
            int i = 0;
            foreach (string PathName in PathNames) {
                SetImage(PathName, i++);
            }

            // lvImages.SmallImageList = ImgList;
            lvImages.LargeImageList = ImgList;

            // lvImages.View = View.Details;
            lvImages.View = View.LargeIcon;
            // lvImages.View = View.List;
            // lvImages.View = View.SmallIcon;
            // lvImages.View = View.Tile;
      }

//---------------------------------------------------------------------------------------

        private void SetImage(string di, int n) {
            try {
                var img = Image.FromFile(di);
                ImgList.Images.Add(img);
                lvImages.Items.Add(Path.GetFileNameWithoutExtension(di), n);
            } catch (Exception ex) {
                int i = 1;
            }
        }

//---------------------------------------------------------------------------------------

        private void btnGo_Click(object sender, EventArgs e) {

        }
    }

    /*
     * public class BingImages
    {
        private const string DOWNLOAD_PATH = @"D:\BingImages";
        private const string BING = "http://www.bing.com";
        private const string IMG_URL = "http://www.bing.com/HPImageArchive" + 
                                       ".aspx?format=xml&idx=0&n={0}&mkt={1}";
        private static string[] Markets = new string[] { "en-US", "zh-CN", 
                                          "ja-JP", "en-AU", "en-UK", 
                                          "de-DE", "en-NZ", "en-CA" };
        private const int NUMBER_OF_IMAGES = 1;

        /// <summary>
        /// Download images from Bing
        /// </summary>
        public static void DownLoadImages()
        {
            // Make sure destination folder exists
            ValidateDownloadPath();
            XDocument doc = null;
            WebRequest request = null;
            // Because each market can have different images
            // cycle through each of them
            foreach (string market in Markets)
            {
                // Form the URL based on market
                // Since this will be run once per day only 1 image needs to
                // be downloaded
                string url = string.Format(IMG_URL, NUMBER_OF_IMAGES, market);
                request = WebRequest.Create(url);
                using (Stream stream = request.GetResponse().GetResponseStream())
                {
                    // Load the stream into and XDocument for processing
                    doc = XDocument.Load(stream);
                }

                // Iterate through the image elements
                foreach (XElement image in doc.Descendants("image"))
                {
                    SaveImage(image.Element("url").Value);
                }
            }
        }

        /// <summary>
        /// Save image from the give URL to disk
        /// </summary>
        /// <param name="url">URL of image to save</param>
        private static void SaveImage(string url)
        {
            // Images can be duplicated between markets
            // so to avoid duplicates from being downloaded
            // get the unique name based on the image number in the URL
            string filename = GetImageName(url);
            if (!File.Exists(filename))
            {
                // URL is relative so form the absolute URL
                WebRequest request = WebRequest.Create(BING + url);
                using (Stream stream = request.GetResponse().GetResponseStream())
                {
                    Image img = Image.FromStream(stream);
                    img.Save(filename);
                }
            }
        }

        /// <summary>
        /// Create filename for image based on URL
        /// </summary>
        /// <param name="url">Image URL</param>
        /// <returns>FQN for saving image to</returns>
        private static string GetImageName(string url)
        {
            // URL is in this format /fd/hpk2/DiskoBay_EN-US1415620951.jpg
            // Extract the image number
            Regex reg = new Regex(@"[0-9]+\w");
            Match m = reg.Match(url);
            // Should now have 1415620951 from above example
            // Create path to save image to
            return string.Format(@"{0}\{1}.jpg", DOWNLOAD_PATH, m.Value);
        }

        /// <summary>
        /// Check if download path exist and create if necessary
        /// </summary>
        private static void ValidateDownloadPath()
        {
            if (!Directory.Exists(DOWNLOAD_PATH))
            {
                Directory.CreateDirectory(DOWNLOAD_PATH);
            }
        }
    } 
     */
}
