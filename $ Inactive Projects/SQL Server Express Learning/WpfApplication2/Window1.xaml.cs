using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication2 {
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window {

        public Window1() {
            InitializeComponent();
            statusBar1.Items.Add("");
        }

        private void button1_MouseMove(object sender, MouseEventArgs e) {
            statusBar1.Items[0] = string.Format("{0}", e.GetPosition(sender as IInputElement));
        }

        private void button1_MouseLeave(object sender, MouseEventArgs e) {
            statusBar1.Items[0] = "";
        }

        private void button1_Click(object sender, RoutedEventArgs e) {
            string s = txtURL.Text;
            if (! s.StartsWith("http")) {
                s = "http://" + s;
            }
            webBrowser1.Navigate(new Uri(s));
        }

        private void webBrowser1_LoadCompleted(object sender, NavigationEventArgs e) {
            listBox1.Items.Add(string.Format("LoadCompleted -- {0}", e.Uri.AbsoluteUri));
            listBox1.Items.Add("");
        }

        private void webBrowser1_Navigated(object sender, NavigationEventArgs e) {
            listBox1.Items.Add(string.Format("Navigated to {0}", e.Uri.AbsoluteUri));
        }

        private void webBrowser1_Navigating(object sender, NavigatingCancelEventArgs e) {
            listBox1.Items.Add(string.Format("Navigating to {0}", e.Uri.AbsoluteUri));
       }

        private void webBrowser1_SourceUpdated(object sender, DataTransferEventArgs e) {
            listBox1.Items.Add("SourceUpdated");
       }
    }
}
