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

using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;


namespace PdfGen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string HtmlFile { get; set;}
        private string GetFileUpload { get; set; }

        private HtmlToPdf Renderer { get; set;} 
        private PdfDocument PdfDocument { get; set;} 
        
        private string Output { get; set;} 
        private string Username { get; set; }

        IFirebaseClient firebaseClient { get; set; }
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "**********",
            BasePath = "**************"
        };
        
        
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

        private void UploadFiles(string FileName_)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            bool Find = (bool)openFileDialog.ShowDialog();
            FileName_ = GetFileUpload;

            if (Find is true) { GetFileUpload = openFileDialog.FileName;}
            FileStream fileStream = new FileStream(GetFileUpload, FileMode.Open);
            
            GoogleDrive googleDrive = Auth_Setup();

             if (googleDrive.Exists("/NewFile.pdf") is true) { MessageBox.Show("This file exisit and is going to be replaced!");
                googleDrive.Upload("/NewFile.pdf", fileStream, 1024, true);
                Username = googleDrive.GetUserName();
            }
            else if (googleDrive.Exists("/NewFile.pdf") is false) { MessageBox.Show("The new file has been created!");
                googleDrive.Upload("/NewFile.pdf", fileStream, 1024, true);
                Username = googleDrive.GetUserName();
            }

            SpaceAllocation spaceAllocation = googleDrive.GetAllocation();

            string[] Sizes = { "B", "KB", "MB", "GB", "TB" };
            long UserAllocation = spaceAllocation.GetTotal();
            int Order = 0;

            while (UserAllocation >= 1024 && Order < Sizes.Length - 1)
            {
                Order++;
                UserAllocation = UserAllocation / 1024;
            }

            string Result = string.Format("{0:0.##} {1}", UserAllocation, Sizes[Order]);
            MessageBox.Show("Size available on this account is " + Result);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UploadFiles(null);
            StoreUsername();
        }
        
        private async void StoreUsername()
        {
            firebaseClient = new FireSharp.FirebaseClient(config);
           
                var Users = new Set_Username
                {
                    setUsername = Username,

                };
            
           SetResponse setResponse = await firebaseClient.SetTaskAsync("UserNames/", Users);
           Set_Username set_Username = setResponse.ResultAs<Set_Username>();

           FirebaseResponse firebaseResponse = new FirebaseResponse();
           firebaseResponse = firebaseClient.Get("UserNames/");
            
        }
        
    }
}
