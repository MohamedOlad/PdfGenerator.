using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Drawing;

using IronOcr;
using IronPdf;
using Path = System.IO.Path;
using Com.CloudRail;
using Com.CloudRail.SI.Services;
using Com.CloudRail.SI;
using Com.CloudRail.SI.Interfaces;
using Com.CloudRail.SI.ServiceCode.Commands.CodeRedirect;

namespace PdfGen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string HtmlFile { get; set;}
        private string GetFileUpload { get; set; }

        private HtmlToPdf Renderer { get; set; }
        private PdfDocument PdfDocument { get; set; }
        private string Output;
        
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            bool Find = (bool)openFileDialog.ShowDialog();
           
            if(Find is true) { HtmlFile = openFileDialog.FileName; Convert(null); }
        }

        private void Convert(string file)
        {

            file = HtmlFile;
            Renderer = new HtmlToPdf();
            PdfDocument = Renderer.RenderHTMLFileAsPdf(HtmlFile);
            
            Output = "C:\\Desktop\\Newfile.pdf" + DateTime.Now.Ticks + ".pdf";
            PdfDocument.SaveAs(Output);
            MessageBox.Show("Conversion done saved at:" + Output);
            
        }
        private GoogleDrive Auth_Setup()
        {
            string Clientid = "**********************";
            string Clientsecret = "******************";
            CloudRail.AppKey = "*********************";

            GoogleDrive googleDrive = new GoogleDrive(new LocalReceiver(8082),Clientid,Clientsecret, "http://localhost:8082/auth","Loading");
            string Result = googleDrive.GetUserLogin();

            return googleDrive;
        }

        private string UploadFiles()
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            bool Find = (bool)openFileDialog.ShowDialog();
            

            if (Find is true) { GetFileUpload = openFileDialog.FileName;}
            FileStream fileStream = new FileStream(GetFileUpload, FileMode.Create);

            Auth_Setup().Upload(GetFileUpload, fileStream, 1024, false);
            string UserLogin = Auth_Setup().GetUserLogin();

            return UserLogin;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UploadFiles();

        }
        
    }
}
