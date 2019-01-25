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

namespace Bing4FreeLRS {
    public partial class BingWallpapers : Form {

        string ImgDir = @"D:\LRS\BingImages";
		ImageList ImgList = new ImageList();

        public BingWallpapers() {
            InitializeComponent();

            SetupImages();
        }

        private void SetupImages() {
            lvImages.Items.Clear();
            ImgList.Images.Clear();

            foreach (var di in Directory.EnumerateFiles(ImgDir, "*.jpg")) {
                SetImage(di);    
            }
        }

        private void SetImage(string di) {
            var img = Image.FromFile(di);
            ImgList.Images.Add(img);
            lvImages.Items.Add(di);
        }

        private void btnGo_Click(object sender, EventArgs e) {

            var img = Image.FromFile(@"D:\LRS\BingImages\image.jpg");
            ImgList.Images.Add(img);
            lvImages.Items.Add("Image");

            var lvItem = new ListViewItem("image.jpg");
            lvImages.LargeImageList = ImgList;

        }
    }
}
