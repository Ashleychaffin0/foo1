using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.SpeechSynthesis;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

// Note: From Iris Classon, http://irisclasson.com/2015/02/10/pdf-to-audio-in-windows-runtime-apps-in-c-for-when-i-dont-want-to-read/

#if false
namespace Mail
{
    public sealed partial class MainPage 
    {
        public MainPage()
        {
            InitializeComponent();
        }
 
        private StorageFile _selectedFile;
 
        private async void OnSelect(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".pdf");
 
            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                _selectedFile = file;
                play.IsEnabled = true;
                slider.IsEnabled = true;
 
                var buffer = await FileIO.ReadBufferAsync(_selectedFile);
 
                using (var reader = new PdfReader(buffer.ToArray()))
                    slider.Maximum = reader.NumberOfPages;
            }
            else
            {
                await new MessageDialog("Could not open file").ShowAsync();
            }
        }
 
        private async void OnPlay(object sender, RoutedEventArgs e)
        {
            if (_selectedFile == null) return;
 
            var buffer = await FileIO.ReadBufferAsync(_selectedFile);
 
            using (var reader = new PdfReader(buffer.ToArray()))
            {
                var text = new StringBuilder();
                var selectedPageNr = (int) slider.Value;
 
                for (var i = selectedPageNr; i <= reader.NumberOfPages && i <= selectedPageNr + 3; i++)
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
 
                var cleaned = text.Replace("\n", "");
 
                await SynthesizeTextToSpeachAsync(cleaned.ToString());
            }
        }
 
        public async Task SynthesizeTextToSpeachAsync(string text)
        {
            using (var speechSynthesizer = new SpeechSynthesizer())
            {
                speechSynthesizer.Voice = SpeechSynthesizer.AllVoices.First(x => x.Gender == VoiceGender.Male);
 
                var stream = await speechSynthesizer.SynthesizeTextToStreamAsync(text);
 
                var mediaElement = new MediaElement { DefaultPlaybackRate = 1.5 };
 
                mediaElement.SetSource(stream, stream.ContentType);
            }
        }
    }
}

#endif


namespace Pdf2Audio {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
		}
	}
}
