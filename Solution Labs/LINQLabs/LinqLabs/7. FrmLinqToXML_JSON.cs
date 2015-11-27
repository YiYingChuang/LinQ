using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using MyLINQ;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;

namespace MyLINQ.StartupLabs
{
    public partial class FrmLinqToXML_JSON_EXCEL : Form
    {
        public FrmLinqToXML_JSON_EXCEL()
        {
            InitializeComponent();
        }


        private void button3_Click(object sender, EventArgs e)
        {
         
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser2.ScriptErrorsSuppressed = true;

            XDocument docRss = XDocument.Load("http://tw.news.yahoo.com/rss/weather");
             this.webBrowser1.DocumentText = docRss.ToString();

            var query = from item in docRss.Descendants("item")

                        select new
                        {
                            標題 = item.Element("title").Value,
                            說明 = item.Element("description").Value,
                            發佈日期 = item.Element("pubDate").Value
                        };


            dataGridView1.DataSource = query.ToList();
      
            //for BindingSource====================
            BindingSource bs = new BindingSource();
            dataGridView4.DataSource = bs;
            bs.DataSource = query.ToList();
            this.webBrowser2.DataBindings.Add("DocumentText", bs, "說明");
            //this.dataRepeater2.DataSource = query.ToList();
            
        }

        async private void button6_Click(object sender, EventArgs e)
        {
          
            this.progressBar1.Value = 50;
            this.progressBar1.Style = ProgressBarStyle.Marquee;

            this.Cursor = Cursors.WaitCursor;
            string result = await flickrWebAPI(results: 10);

            await Task.Run(() => this.webBrowser1.DocumentText = result);

            XDocument xDocument = XDocument.Parse(result);

            var photoQuery =
                 from entry in xDocument.Descendants("photo")
                 select new
                 {
                     UniqueId = (String)entry.Attribute("url_m"),
                     Title = (String)entry.Attribute("title"),
                     //OwnerName = (String)entry.Attribute("ownername"),
                     Subtitle = (String)entry.Attribute("ownername"),
                     Image = DownLoadData((String)entry.Attribute("url_m")),
                     Description = (String)entry.Element("description"),
                     DateTaken = (String)entry.Attribute("datetaken")
                 };

            var list = await Task.Run(() => photoQuery.ToList()); //呼叫任意方法 for CPU bound 工作- 用 Task 加 await 會在另外執行緒做 看Timer
             this.dataGridView1.DataSource = list;

            this.Cursor = Cursors.Default;

            //update UI
            this.progressBar1.Value = this.progressBar1.Maximum;
            this.progressBar1.Style = ProgressBarStyle.Blocks;

        }


        public async static Task<string> CallWebAPI(string url)
        {
            //httpClient
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            return result;
        }

     


        public async static Task<string> flickrWebAPI(string SearchTerm = "Rivers", int results = 50)
        {
            Console.WriteLine(" Add Thread ID : " +
              Thread.CurrentThread.ManagedThreadId);

            string url = string.Format(
                "https://api.flickr.com/services/rest/?" +
                "method=flickr.photos.search&" +
                "api_key=ec2ac3f3b314abae67f27cfc85e21998" +
                "&text=" + SearchTerm + "&sort=" + "interestingness-desc" +
                "&safe_search=" + "1" + "&content_type=" + "1" +
                "&license=1,2,3,4,5,6,7&per_page=" + results + "&page=1&" +
                "extras=owner_name,description,date_upload,date_taken," +
                "geo,tags,url_sq,url_t,url_s,url_m,url_o" +
                "&has_geo1&media=photos&content_type=1&safe_search=1");

            //httpClient
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string pageXML = await response.Content.ReadAsStringAsync();

            return pageXML;

        }

        Bitmap DownLoadData(string url)
        {
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
                myRequest.Method = "GET";
                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(myResponse.GetResponseStream());
                myResponse.Close();

                return bmp;
            }
            catch
            {
                return null;// (Bitmap)this.pictureBox1.ErrorImage;
            }

        }
        NorthwindEntityModel.northwindEntities db = new NorthwindEntityModel.northwindEntities();
        private void button8_Click(object sender, EventArgs e)
        {
            var q = from p in db.Products.AsEnumerable()
                        select new XElement("Product", new XElement("ProductName", p.ProductName),
                                                                             new XElement("UnitPrice", p.UnitPrice));


            XElement doc = new XElement("Products", q);
            doc.Save("Products.xml");
            Process.Start("Products.xml");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            XElement doc;
            doc = XElement.Load("Products.xml");


            //xml 文件 轉物件

            var q = from element in doc.Elements("Product")
                        select new
                       {
                            ProductName = element.Element("ProductName").Value,
                           UnitPrice = element.Element("UnitPrice").Value
                       };
            this.dataGridView1.DataSource = q.ToList();

        }

        private void button1_Click(object sender, EventArgs e)
        {   //LINQ IQueryable Toolkit
            //工具/套件管理主控台
            // Install-Package LinqToExcel -Version 1.7.1 
            //http://files.dotblogs.com.tw/merlin/1207/201271792540765.jpg
            //            類型 'System.BadImageFormatException' 的未處理例外狀況發生於 System.Windows.Forms.dll

            //其他資訊: 無法載入檔案或組件 'LinqToExcel, Version=1.7.1.0, Culture=neutral, PublicKeyToken=9c8ea65a58f03f1f' 或其相依性的其中之一。 試圖載入格式錯誤的程式。
            //            解法一：

            // 使用 64-bit 的 DLL



            //解法二：

            // C#專案 → 滑鼠右鍵 → 屬性 → (標籤)建置 → 平台目標 → Any CPU 改成 x86

            //var ExcelFile = new ExcelQueryFactory("員工基本資料.xlsx");
            //ExcelFile.DatabaseEngine = LinqToExcel.Domain.DatabaseEngine.Ace;
            //ExcelFile.AddMapping<myEmp>(p => p.C_uID, "員工編號");
            //ExcelFile.AddMapping<myEmp>(p => p.C_uName, "員工姓名");
            //ExcelFile.AddMapping<myEmp>(p => p.C_uTel, "連絡電話");
            //ExcelFile.AddMapping<myEmp>(p => p.C_uSalary, "薪資");

            //IQueryable<myEmp> q = ExcelFile.Worksheet<myEmp>("員工基本資料二").Where(p => p.C_uID == "A009");
            ////foreach (var itemRow in sqlExcelData_3)
            ////{
            ////    this.listBox1.Items.Add(string.Format("員工編號：{0}、員工姓名：{1}、連絡電話：{2}、薪資：{3}",
            ////         itemRow.C_uID,
            ////         itemRow.C_uName,
            ////         itemRow.C_uTel,
            ////         itemRow.C_uSalary
            ////         ));

            //this.dataGridView1.DataSource = q.ToList();



        }

        async private void button5_Click(object sender, EventArgs e)
        {

        
            //NuGet  使用   json.NET  套件較好   namespace Newtonsoft.Json (Deserialize 可讀 \n\r....)
            //使用  dynamic 較容易-		動態檢視	展開動態檢視會取得物件的動態成員	
            //json- {} 物件, [] array
    
            //StreamReader sr = new StreamReader("JsonRecipes.json");
            //string jsonText = sr.ReadToEnd();

            //JArray jArray = (JArray)JsonConvert.DeserializeObject(jsonText);

            //var q = from dynamic dic in jArray
            //        group dic by dic.@group.key into g
            //        select new
            //        {
            //            g.Key,
            //            Items = g
            //        };

            //this.dataGridView1.DataSource = q.ToList();
            //=========================================================
            //台北市資料開放平台
            //http://data.taipei/opendata/datalist/listDataset         
            string url = "http://data.taipei/opendata/datalist/apiAccess?scope=resourceAquire&rid=36847f3f-deff-4183-a5bb-800737591de5";
            string result = await CallWebAPI(url);

            dynamic jsonObject = JsonConvert.DeserializeObject(result);
            JArray jArray1 = jsonObject.result.results;

            var q1 = from dynamic item in jArray1
                     select new
                     {
                         item.CAT1,
                         item.CAT2,
                         item.stitle,
                         item.xbody,
                         item.avBegin,
                         item.avEnd,
                         item.address,
                         item.file,
                         item.info,
                         ImageFiles = GetImages(item.file.ToString()),
                         FirstImageURL = GetFirstImageURL(item.file.ToString())
                     };
            this.dataGridView4.DataSource = q1.ToList();


            //做 bindingsource========================================
             bs.PositionChanged += bs_PositionChanged;
            bs.DataSource = q1.ToList();
            this.dataGridView5.DataSource = bs;

             this.webBrowser2.DataBindings.Add("Url", bs, "FirstImageURL");
        }
   BindingSource bs = new BindingSource();
        
        void bs_PositionChanged(object sender, EventArgs e)
        {
            dynamic d = this.bs.Current;

            this.flowLayoutPanel1.Controls.Clear();
            for (int i=0; i<=d.ImageFiles.Length-1;i++)
            {
                PictureBox p = new PictureBox();
                p.Width = 80; p.Height = 50; p.BorderStyle = BorderStyle.Fixed3D;
                p.Load(d.ImageFiles[i]);
                p.SizeMode = PictureBoxSizeMode.StretchImage;
                this.flowLayoutPanel1.Controls.Add(p);
              //  Application.DoEvents();
            }

        }

       string[] GetImages(string file)
        {
             string[] files= file.Split(new string[]{"http://"},  StringSplitOptions.RemoveEmptyEntries);
             for (int i = 0; i <= files.Length - 1; i++ )
             {
                 files[i] = "http://" + files[i];
             }
             return files;
        }


       Uri GetFirstImageURL(string file)
       {
          string[] files = file.Split(new string[] { "http://" }, StringSplitOptions.RemoveEmptyEntries);
           return new Uri("http://"+ files[0],  UriKind.Absolute);
       }

    async   private void button2_Click(object sender, EventArgs e)
       {
           //=========================================================
           //台北市資料開放平台
           //http://data.taipei/opendata/datalist/listDataset         
           string url = "http://data.taipei/opendata/datalist/apiAccess?scope=resourceAquire&rid=36847f3f-deff-4183-a5bb-800737591de5";
           string result = await CallWebAPI(url);

           var  jsonObject = JsonConvert.DeserializeObject<TravelInfoResult>(result);
      
          var  jArray1 = jsonObject.result.results;

           var q1 = from item in jArray1
                    select new
                    {
                        item.CAT1,
                        item.CAT2,
                        item.stitle,
                        //item.xbody,
                        //item.avBegin,
                        //item.avEnd,
                        //item.address,
                        //item.file,
                        //item.info,
                        ImageFiles = GetImages(item.file.ToString()),
                        FirstImageURL = GetFirstImageURL(item.file.ToString())
                    };
           this.dataGridView4.DataSource = q1.ToList();


           //做 bindingsource========================================
           bs.PositionChanged += bs_PositionChanged;
           bs.DataSource = q1.ToList();
           this.dataGridView5.DataSource = bs;

          // this.webBrowser2.DataBindings.Add("Url", bs, "FirstImageURL");
       }

        class TravelInfoResult
        {
         
         public  TravelInfo  result {get;set;}
     
        }

        class TravelInfo
        {

            public string count { get; set; }

            public TravelSite[] results { get; set; }
        }
     class TravelSite
     {
        public string  CAT1 {get;set;}
             public string  CAT2 {get;set;}
                         public string  stitle {get;set;}
           public string item  {get;set;}
                     
                        //item.xbody,
                        //item.avBegin,
                        //item.avEnd,
                        //item.address,
           public string file { get; set; }
                        //item.info,
           public string[] ImageFiles { get; set; }
           public Uri FirstImageURL { get; set; }
     }

    
         //{
         //       "_id": "1",
         //       "RowNumber": "1",
         //       "REF_WP": "10",
         //       "CAT1": "景點",
         //       "CAT2": "養生溫泉",
         //       "SERIAL_NO": "2011051800000061",
         //       "MEMO_TIME": "各業者不同，依據現場公告",
         //       "stitle": "新北投溫泉區",
         //       "xbody": "北投溫泉從日據時代便有盛名，深受喜愛泡湯的日人自然不會錯過，瀧乃湯、星乃湯、鐵乃湯就是日本人依照溫泉的特性與療效給予的名稱，據說對皮膚病、神經過敏、氣喘、風濕等具有很好的療效，也因此成為了北部最著名的泡湯景點之一。新北投溫泉的泉源為大磺嘴溫泉，泉質屬硫酸鹽泉，PH值約為3~4之間，水質呈黃白色半透明，泉水溫度約為50-90℃，帶有些許的硫磺味 。目前北投的溫泉旅館、飯店、會館大部分集中於中山路、光明路沿線以及北投公園地熱谷附近，總計約有44家，每一家都各有其特色，多樣的溫泉水療以及遊憩設施，提供遊客泡湯養生，而鄰近的景點也是非常值得造訪，例如被列為三級古蹟的三寶吟松閣、星乃湯、瀧乃湯以及北投第一家溫泉旅館「天狗庵」，都有著深遠的歷史背景，而北投公園、北投溫泉博物館、北投文物館、地熱谷等，更是遊客必遊的景點，來到北投除了可以讓溫泉洗滌身心疲憊，也可以順便了解到北投溫泉豐富的人文歷史。",
         //       "avBegin": "2010/02/14",
         //       "avEnd": "2013/09/30",
         //       "idpt": "臺北旅遊網",
         //       "address": "臺北市  北投區中山路、光明路沿線",
         //       "xpostDate": "2013/09/30",
         //       "file": "http://www.travel.taipei/d_upload_ttn/sceneadmin/pic/11000848.jpghttp://www.travel.taipei/d_upload_ttn/sceneadmin/pic/11002891.jpg",
         //       "langinfo": "10",
         //       "POI": "Y",
         //       "info": "新北投站下車，沿中山路直走即可到達公車：216、218、223、230、266、602、小6、小7、小9、、小22、小25、小26至新北投站下車",
         //       "longitude": "121.503",
         //       "latitude": "25.1364",
         //       "MRT": "新北投"
         //   },


      

    }
}
