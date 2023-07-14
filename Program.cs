using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace WindowsFormsApp1


/* Main 线程逻辑 Barrier*/
{

    /* 全局类 */
    public class Globle_package
    {
        public static IWebDriver driver;
        public static int IterationNum;
        public static String filmType;
        public static String filmSize;
    };


    class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

        }



        public List<List<IWebElement>> GetTable(ChromeDriver driver)
        {
            //设置隐式等待指定元素，等待时长为20秒
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(7);
            //driver.Url = url;
            //web页面表格的结构：table（表格）、tbody（表格主体）、tr（表格一行）、td（表格一列）
            var table = driver.FindElement(By.ClassName("searchList"));
            //如果没有找到，或者多于一个，则暂时先不处理
            if ((table is IWebElement) == false) throw new Exception("Ex202102121134：不支持");
            var tbody = table.FindElement(By.TagName("tbody"));

            List<List<IWebElement>> res = new List<List<IWebElement>>();
            //处理每一行
            foreach (var tr in tbody.FindElements(By.TagName("tr")))
            {
                List<IWebElement> row = new List<IWebElement>();
                //处理每一列
                foreach (var td in tr.FindElements(By.TagName("td")))
                {
                    row.Add(td);
                }
                res.Add(row);
            }

            return res;
        }

        public DataTable GetTableText(ChromeDriver driver)
        {
            var elments = this.GetTable(driver);
            List<List<string>> res = new List<List<string>>();
            DataTable dt = new DataTable("MyDataTable");
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("leixing",typeof(string));
            dt.Columns.Add("size", typeof(string));

            int W_K = 0;
            foreach (var row in elments)
            {
                List<string> newRow = new List<string>();
                var N_K = 1;
                DataRow dr = dt.NewRow();
                dr["ID"] = W_K;
                Console.WriteLine(dr["ID"]);
                foreach (var item in row)
                {
  
                    if (N_K == 2)
                    {
                        dr["leixing"] = item.Text; 
                    }
                    if (N_K == 4)
                    {
                        dr["size"] = item.Text;
                    }
 
                    N_K++;
                   
                }
                dt.Rows.Add(dr);
                W_K++;


            }
 
            return dt;
        }
        /* 定位元素 */
        public void Phase1Doing()
        {
            /* --------------加载策略----------------- */
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("lang=zh_CN.UTF-8");
            options.PageLoadStrategy = PageLoadStrategy.Normal;

           
            ChromeDriver driver = new OpenQA.Selenium.Chrome.ChromeDriver(options);
            /* ------------ 登录 --------------- */
            Globle_package.driver = driver;
            driver.Navigate().GoToUrl("http://114.116.64.11:8025");
            driver.Manage().Window.Maximize(); // 窗体最大化
            Thread.Sleep(500);
            driver.Navigate().Refresh(); // 刷新页面
            driver.FindElement(By.Id("TextBox1")).Clear();
            driver.FindElement(By.Id("TextBox1")).SendKeys("admin");
            Thread.Sleep(500);
            driver.FindElement(By.Id("TextBox2")).Clear();
            driver.FindElement(By.Id("TextBox2")).SendKeys("123");
            Thread.Sleep(500);
            driver.FindElement(By.Id("Button1")).Click();
            // 定位右侧元素
            var education = driver.FindElement(By.XPath("//*[@id='ContentPlaceHolder1_DropDownList1']"));
            var selectElement_one = new SelectElement(education);
            selectElement_one.SelectByText("识别失败");// 识别失败 人工识别
            var Film_type = driver.FindElement(By.XPath("//*[@id='ContentPlaceHolder1_DropDownList2']"));
            Thread.Sleep(2000);
            var selectElement_two = new SelectElement(Film_type);

            //selectElement_two.SelectByText(Globle_package.filmType);// PR_aw43 Eleva
            driver.FindElement(By.Id("ContentPlaceHolder1_Button1")).Click();
            /*----------------------- 登录结束 ----------------------------*/



            DataTable dt = this.GetTableText(driver);
            foreach (DataRow row in dt.Rows)
            {
                Console.WriteLine("ID:{0},leixing:{1}, size:{2}",row["ID"], row["leixing"], row["size"]);
                int k = dt.Rows.Count - 1;
                Console.WriteLine("行数:{0}",k);
                if ((int)row["ID"] > 0)
                {
                    OCR_Function((int)row["ID"], (string)row["leixing"], (string)row["size"]);
                    // #ContentPlaceHolder1_DataList1_lbtDelete_0   ... 1 - 1
                    // #ContentPlaceHolder1_DataList1_lbtDelete_1
                    // #ContentPlaceHolder1_DataList1_lbtDelete_14
                }

            }

            driver.Quit();
            System.Environment.Exit(0);


        }

        private static void OCR_Function(int sum, string Film_type, string Film_size)
        {
            try
            {

                Console.WriteLine("OCR 代码运行");
                Console.WriteLine(Film_type);
                Console.WriteLine(Film_size);
                IWebDriver driver = Globle_package.driver;

                Globle_package.filmSize = Film_size;
                Globle_package.filmType = Film_type;
                Console.WriteLine("点击函数");
                
                Console.WriteLine("点击函数sum:{0}",sum);
                driver.FindElement(By.Id("ContentPlaceHolder1_DataList1_lbtDelete_" + (sum-1))).Click();
                var all_pages = driver.WindowHandles; // 获取所有句柄
                driver.SwitchTo().Window(all_pages.Last());
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(13);
                Thread.Sleep(3000);
                Screenshot_JudgmentTools SJ = new Screenshot_JudgmentTools();
                Thread.Sleep(5000);
                var ocr_text = SJ.Screen_JudgmentToolsMethods();
                var res = SJ.Screen_JudgmentToolsMetodsTrueFalse(ocr_text, Globle_package.filmType);
                Console.WriteLine("res:{0},fize:{1}",res, Globle_package.filmType);
                if (res == "" && Globle_package.filmType == "PR_aw43")
                {
                    Console.WriteLine("CT 匹配失败");
                    Thread.Sleep(6000);
                    Globle_package.filmType = "PR_aw43_2";
                    var ocr_text_pw43 = SJ.Screen_JudgmentToolsMethods();
                    var res_pw43 = SJ.Screen_JudgmentToolsMetodsTrueFalse(ocr_text_pw43, Globle_package.filmType);
                    if (res_pw43 != "")
                    {
                        Console.WriteLine("CT-识别成功", res_pw43);
                        driver.FindElement(By.Id("TextBox1")).Clear();
                        Thread.Sleep(500);
                        driver.FindElement(By.Id("TextBox1")).SendKeys(res_pw43);
                        Thread.Sleep(500);
                        var btn1 = driver.FindElement(By.Id("Button1"));
                        Thread.Sleep(500);
                        driver.ExecuteJavaScript("arguments[0].click();", btn1);
                        Thread.Sleep(1800);
                    }
                }
                if (res == "" && Globle_package.filmType == "Eleva")
                {
                    Console.WriteLine("体检 匹配失败");
                }
                if (res != "")
                {
                    Console.WriteLine("Progam-识别成功", res);
            
                    driver.FindElement(By.Id("TextBox1")).Clear();
                    Thread.Sleep(500);
                    driver.FindElement(By.Id("TextBox1")).SendKeys(res);
                    Thread.Sleep(500);
                    var btn1 = driver.FindElement(By.Id("Button1"));
                    Thread.Sleep(500);
                    driver.ExecuteJavaScript("arguments[0].click();", btn1);
                    Thread.Sleep(1800);
                }
           
                driver.Close();
                Thread.Sleep(1000);
                driver.SwitchTo().Window(all_pages.First());

            }
            catch (Exception ex)
            {
                Console.WriteLine("异常输出{0}", ex);
                Globle_package.driver.Quit();
                Environment.Exit(0);

           }

        }
    }
}
