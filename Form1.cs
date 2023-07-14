using PaddleOCRSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Threading;
using System.Diagnostics;
using OpenQA.Selenium.Support.Extensions;
using System.Text.RegularExpressions;
/* 页面交互逻辑 */
namespace WindowsFormsApp1
{



    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
           
            var p1 = new Program();
            p1.Phase1Doing();

            // 退出所有
            System.Environment.Exit(0);
        }

       
    }
    public class Screenshot_JudgmentTools
    {
        public string Screen_JudgmentToolsMetodsTrueFalse(string ocrText, string filmType)
        {
            var reg = new Regex("[0-9]+", RegexOptions.IgnoreCase | RegexOptions.Singleline, TimeSpan.FromSeconds(2));
            Console.WriteLine("filmtype:{0}",  filmType);
            if (filmType == "Eleva")
            {
                foreach (Match m in reg.Matches(ocrText))
                {
                    var val = m.Groups[0].Value;
                    Regex rx = new Regex("^[0-9]{10}$");
                    if (rx.IsMatch(val))
                    {
                        Debug.WriteLine("是10位数字", val);
                        return val;
                    }
          
                }
            }
            else
            {
                foreach (Match m in reg.Matches(ocrText))
                {
                    string str = m.Groups[0].Value;
                    int start = 0, length = 16;
                    Regex _regex_ = new Regex("^(202[1-3])(0[1-9]|1[1-2])(0[1-9]|1[0-9]|2[0-9]|30|31)(0000)([0-9]{4})$");
                    Console.WriteLine("初始识别内容:{0}",str);
                    if (str.Length >= 16)
                    {
                        str = str.Substring(start, length);
                        if (_regex_.IsMatch(str))
                        {
                            Debug.WriteLine("识别对象是16位数字:{0}", str);
                            return str;
                        }

                     }
                    if (_regex_.IsMatch(str))
                    {
                        Debug.WriteLine("识别对象是16位数字2:{0}", str);
                        return str;
                    }
                }
            }
            return "";
        }
        public string Screen_JudgmentToolsMethods()
        {
            
            
            if (Globle_package.filmType == "Eleva")
            {
                ScreenshotTools ST = new Eleva();
                OCRResult oCRResult = ST.ScreenshotTools_public();
                //Screen_JudgmentToolsMetodsTrueFalse(oCRResult.Text, Globle_package.filmType);

                return oCRResult.Text;
               
            }
            if (Globle_package.filmType == "SKDICOMINT")
            {
                ScreenshotTools ST = new SKDICOMINT();
                OCRResult oCRResult = ST.ScreenshotTools_public();
                return oCRResult.Text;
              
            }

            if (Globle_package.filmType == "YKJQ010400")
            {
                ScreenshotTools ST = new YKJQ010400();
                OCRResult oCRResult = ST.ScreenshotTools_public();
                return oCRResult.Text;
           

            }
               if (Globle_package.filmType == "PR_aw43")
            {

                ScreenshotTools ST = new PR_aw43();
                OCRResult oCRResult = ST.ScreenshotTools_public();
                return oCRResult.Text;
             
            }

            if (Globle_package.filmType == "PR_aw43_2")
            {

                ScreenshotTools ST = new PR_aw43_2();
                OCRResult oCRResult = ST.ScreenshotTools_public();
                return oCRResult.Text;

            }

            if (Globle_package.filmType == "CANON_CXDI")
            {
                ScreenshotTools ST = new CANON_CXDI();
                OCRResult oCRResult = ST.ScreenshotTools_public();
                return oCRResult.Text;
            }
            return "" ;

        }
        
    }
    public abstract class ScreenshotTools {


        public abstract Rectangle ScreenshotToolsmethod();
        public OCRResult ScreenshotTools_public()
        {
            Screen screen = Screen.AllScreens.FirstOrDefault();//获取当前第一块屏幕(根据需求也可以换其他屏幕)
            Rectangle rc = ScreenshotToolsmethod();
            Bitmap bitmap = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);
            using (Graphics memoryGrahics = Graphics.FromImage(bitmap))
            {
                memoryGrahics.CopyFromScreen(rc.X, rc.Y, 0, 0, rc.Size, CopyPixelOperation.SourceCopy);//对屏幕指定区域进行图像复制
            }
            //bitmap.Save("c://123.png", ImageFormat.Png);
            OCRModelConfig config = null;
            OCRParameter oCRParameter = new OCRParameter();
            OCRResult ocrResult = new OCRResult();
            using (PaddleOCREngine engine = new PaddleOCREngine(config, oCRParameter))
            {
                ocrResult = engine.DetectText(bitmap);
              //  MessageBox.Show(ocrResult.Text);
                return ocrResult;
            }
        }

    }
    public class Eleva : ScreenshotTools
    {
        public override Rectangle ScreenshotToolsmethod()
        {
            Rectangle rc = new Rectangle(43, 194, 92, 340);
            return rc;
        }
    }

    //public class Eleva_2 : ScreenshotTools
    //{
    //    public override Rectangle ScreenshotToolsmethod()
    //    {
    //        Rectangle rc = new Rectangle(43, 194, 92, 340);
    //        return rc;
    //    }
    //}


    public class CANON_CXDI : ScreenshotTools
    {
        public override Rectangle ScreenshotToolsmethod()
        {
            if (Globle_package.filmSize == "11INX14IN")
            {
                Globle_package.driver.ExecuteJavaScript("document.body.style.zoom='0.67'");
                Thread.Sleep(3000);
                Rectangle rc = new Rectangle(1315, 174, 522, 191);
                return rc;
            }
            else
            {
                Globle_package.driver.ExecuteJavaScript("document.body.style.zoom='0.5'");
                Thread.Sleep(3000);
                Rectangle rc = new Rectangle(1403, 181, 313, 170);
                return rc;
            }
        }


    }

    public class SKDICOMINT : ScreenshotTools
    {
        public override Rectangle ScreenshotToolsmethod()
        {
            Rectangle rc = new Rectangle(12, 371, 356, 85);
            return rc;
        }
    }

    public class YKJQ010400 : ScreenshotTools
    {
        public override Rectangle ScreenshotToolsmethod()
        {
            Globle_package.driver.ExecuteJavaScript("document.body.style.zoom='0.25'");
            Thread.Sleep(3000);
            Rectangle rc = new Rectangle(13, 173, 876, 704);
            return rc;
        }
    }


    public class PR_aw43 : ScreenshotTools
    {
        public override Rectangle ScreenshotToolsmethod()
        {
            Globle_package.driver.ExecuteJavaScript("document.body.style.zoom='2'");
            Thread.Sleep(500);
            Globle_package.driver.ExecuteJavaScript("window.scroll(30000,30000)");
            Thread.Sleep(2000);
            //Rectangle rc = new Rectangle(22, 223, 695, 250);
            Rectangle rc = new Rectangle(68, 151, 955, 311);
            return rc;
        }
    }



    public class PR_aw43_2 : ScreenshotTools
    {
        public override Rectangle ScreenshotToolsmethod()
        {
            Globle_package.driver.ExecuteJavaScript("document.body.style.zoom='-2'");
            Thread.Sleep(500);
            Globle_package.driver.ExecuteJavaScript("window.scroll(-30000,-30000)");
            Thread.Sleep(2000);
            Globle_package.driver.ExecuteJavaScript("document.body.style.zoom='3'");
            Thread.Sleep(5000);
            Rectangle rc = new Rectangle(19, 241, 1403, 452);
            return rc;
        }
    }
}