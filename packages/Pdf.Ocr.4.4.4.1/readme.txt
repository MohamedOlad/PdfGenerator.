Iron Software PDF & OCR Complete for .Net
PDF Docs - http://ironpdf.com
Ocr Docs http://ironsoftware.com/csharp/ocr/

C# Code Examples - HTML to PDF:
----------------------------------
  
using IronPdf;
IronPdf.HtmlToPdf Renderer = new IronPdf.HtmlToPdf();
Renderer.PrintOptions.DPI = 300;
var Pdf1 = Renderer.RenderHtmlAsPdf("<h1>Hello World</h1>").SaveAs("html-string.pdf");
var Pdf2 = Renderer.RenderHtmlAsPdf("<img src='photo.jpg'", @"C:\path\to\my\assets").SaveAs("with-assets.pdf");
var Pdf3 = Renderer.RenderHTMLFileAsPdf(@"C:\mysite\index.html").SaveAs("html-file.pdf");
var Pdf4 = Renderer.RenderUrlAsPdf("http//ironpdf.com/").SaveAs("url.pdf");
      
 

C# Code Examples - Advanced PDF OCR:
----------------------------------

using IronPdf;
using IronOcr;

//OCR Shorthand
HtmlToPdf Renderer = new HtmlToPdf();
var OcrResults = Renderer.Ocr("path/to.pdf")    



//OCR Advanced Longhand
var Ocr = new AdvancedOcr()
{
    CleanBackgroundNoise = false,
    ColorDepth = 4,
    ColorSpace = AdvancedOcr.OcrColorSpace.Color,
    EnhanceContrast = false,
    DetectWhiteTextOnDarkBackgrounds = false,
    RotateAndStraighten = false,
    Language = IronOcr.Languages.English.OcrLanguagePack,
    EnhanceResolution = false,
    InputImageType = AdvancedOcr.InputTypes.Document,
    ReadBarCodes = true,
    Strategy = AdvancedOcr.OcrStrategy.Fast
};

var Results = Ocr.ReadPdf(@"C:\My\File.pdf");
var Pages = Results.Pages;
var Barcodes = Results.Barcodes;
var FullPdfText = Results.Text;