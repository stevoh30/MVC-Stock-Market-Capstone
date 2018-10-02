using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using StockMarket.Models.Computation;
using StockMarket.Models.AccessDatabse;
using StockMarket.Models.List.Chart;
using System.IO;

namespace StockMarket.Models.List
{
    public class ListProperties
    {
        public string symbol { get; set; }
        public string companyName { get; set; }
        public string primaryExchange { get; set; }
        public string sector { get; set; }
        public string calculationPrice { get; set; }
        public double? open { get; set; }
        public object openTime { get; set; }
        public double? close { get; set; }
        public object closeTime { get; set; }
        public double? high { get; set; }
        public double? low { get; set; }
        public double? latestPrice { get; set; }
        public string latestSource { get; set; }
        public string latestTime { get; set; }
        public object latestUpdate { get; set; }
        public double? latestVolume { get; set; }
        public double? iexRealtimePrice { get; set; }
        public int? iexRealtimeSize { get; set; }
        public object iexLastUpdated { get; set; }
        public double? delayedPrice { get; set; }
        public object delayedPriceTime { get; set; }
        public double? extendedPrice { get; set; }
        public double? extendedChange { get; set; }
        public double? extendedChangePercent { get; set; }
        public object extendedPriceTime { get; set; }
        public double? previousClose { get; set; }
        public double? change { get; set; }
        public double? changePercent { get; set; }
        public double? iexMarketPercent { get; set; }
        public int? iexVolume { get; set; }
        public int? avgTotalVolume { get; set; }
        public double? iexBidPrice { get; set; }
        public int? iexBidSize { get; set; }
        public double? iexAskPrice { get; set; }
        public int? iexAskSize { get; set; }
        public object marketCap { get; set; }
        public double? peRatio { get; set; }
        public double? week52High { get; set; }
        public double? week52Low { get; set; }
        public double? ytdChange { get; set; }
    }

    public class NewsSection
    {
        public DateTime datetime { get; set; }
        public string headline { get; set; }
        public string source { get; set; }
        public string url { get; set; }
        public string summary { get; set; }
        public string related { get; set; }
        public string image { get; set; }
    }

    public class FinancialInfo
    {
        public string reportDate { get; set; }
        public object grossProfit { get; set; }
        public object costOfRevenue { get; set; }
        public object operatingRevenue { get; set; }
        public object totalRevenue { get; set; }
        public object operatingIncome { get; set; }
        public object netIncome { get; set; }
        public object researchAndDevelopment { get; set; }
        public object operatingExpense { get; set; }
        public object currentAssets { get; set; }
        public object totalAssets { get; set; }
        public object totalLiabilities { get; set; }
        public object currentCash { get; set; }
        public object currentDebt { get; set; }
        public object totalCash { get; set; }
        public object totalDebt { get; set; }
        public object shareholderEquity { get; set; }
        public object cashChange { get; set; }
        public object cashFlow { get; set; }
        public object operatingGainsLosses { get; set; }
    }

    public class CompanyInfo
    {
        public string symbol { get; set; }
        public string companyName { get; set; }
        public string exchange { get; set; }
        public string industry { get; set; }
        public string website { get; set; }
        public string description { get; set; }
        public string CEO { get; set; }
        public string issueType { get; set; }
        public string sector { get; set; }
    }

    public class Tops
    {
        public string symbol { get; set; }
        public string sector { get; set; }
        public string securityType { get; set; }
        public double bidPrice { get; set; }
        public int bidSize { get; set; }
        public double? askPrice { get; set; }
        public int askSize { get; set; }
        public object lastUpdated { get; set; }
        public double? lastSalePrice { get; set; }
        public int lastSaleSize { get; set; }
        public object lastSaleTime { get; set; }
        public int volume { get; set; }
        public double? marketPercent { get; set; }
    }

    public class Last
    {
        public string symbol { get; set; }
        public double price { get; set; }
        public int size { get; set; }
        public object time { get; set; }
    }

    public class ChartProperties_1D
    {
        public string date { get; set; }
        public string minute { get; set; }
        public string label { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double average { get; set; }
        public int volume { get; set; }
        public double notional { get; set; }
        public int numberOfTrades { get; set; }
        public double marketHigh { get; set; }
        public double marketLow { get; set; }
        public double marketAverage { get; set; }
        public int marketVolume { get; set; }
        public double marketNotional { get; set; }
        public int marketNumberOfTrades { get; set; }
        public double open { get; set; }
        public double close { get; set; }
        public double marketOpen { get; set; }
        public double marketClose { get; set; }
        public double? changeOverTime { get; set; }
        public double? marketChangeOverTime { get; set; }
    }
    public class ChartProperties_MY
    {
        public string date { get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double close { get; set; }
        public int volume { get; set; }
        public int unadjustedVolume { get; set; }
        public double change { get; set; }
        public double changePercent { get; set; }
        public double vwap { get; set; }
        public string label { get; set; }
        public double changeOverTime { get; set; }
    }

    // This method returns a list of random companies gathered from the API.
    public class DisplayList
    {
        Compute sp = new Compute();

        Random rd = new Random();

        public async Task<List<ListProperties>> DatabaseCompanies()
        {
            string conStr = ConfigurationManager.ConnectionStrings["ConStrs"].ConnectionString;

            string Json = String.Empty;

            Task<List<ListProperties>> t2 = Task.Factory.StartNew<List<ListProperties>>(() =>
            {
                List<ListProperties> dbCompanies = new List<ListProperties>();

                List<string> JsonList = new List<string>();

                Task<List<CompanySymbol>> compa = RandomCompany();

                foreach (CompanySymbol csp in compa.Result)
                {
                    try
                    {
                        string url = string.Format("https://api.iextrading.com/1.0/stock/{0}/quote", csp.symbol);

                        using (WebClient Rand_companies = new WebClient())
                        {
                            JsonList.Add(Rand_companies.DownloadString(url));
                            Json = sp.JsonPortFolioCreator(JsonList);
                            dbCompanies = JsonConvert.DeserializeObject<List<ListProperties>>(Json);
                        }
                    }
                    catch (WebException ex) //Log Any Errors
                    {
                        using (StreamWriter writetext = new StreamWriter("ErrorLog.txt"))
                        {
                            writetext.WriteLine("Database Companies_Method\t" + ex.ToString() + "\n----------------------------------------------------------------------");
                        }
                    }
                }
                return dbCompanies;
            });

            await t2;

            return t2.Result;
        }
        public async Task<List<CompanySymbol>> RandomCompany() //Goes to Search Form
        {
            AccessDB accessDB = new AccessDB();
            string conStr = ConfigurationManager.ConnectionStrings["ConStrs"].ConnectionString;


            Task<List<CompanySymbol>> t2 = Task.Factory.StartNew<List<CompanySymbol>>(() =>
            {
                List<CompanySymbol> randomCompany = new List<CompanySymbol>();

                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    randomCompany = accessDB.CompaniesList(con);
                }
                return randomCompany;
            });
            await t2;

            return t2.Result;

        }

        // This method accepts username and password parameters and returns a list of all the user's 
        // portfolio/ company information converted from the API.

        public async Task<List<ListProperties>> PortFolioList(string username, string password)
        {
            AccessDB accessDB = new AccessDB();

            string conStr = ConfigurationManager.ConnectionStrings["ConStrs"].ConnectionString;

            Task<List<ListProperties>> t2 = Task.Factory.StartNew<List<ListProperties>>(() =>
            {

                List<ListProperties> portFolio = new List<ListProperties>();

                List<string> Jsons = new List<string>();

                List<Person> people = new List<Person>();

                string Json = String.Empty;

                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();

                    string userID = string.Empty;
                    people = accessDB.Login_Check(username, password, con);

                    foreach (Person p in people)
                    {
                        userID = p.UserID;
                    }

                    foreach (PortFolio cs in accessDB.ListPortFolio(userID, con))
                    {
                        string url = string.Format("https://api.iextrading.com/1.0/stock/{0}/quote", cs.Name);

                        using (WebClient port_folio = new WebClient())
                        {
                            Jsons.Add(port_folio.DownloadString(url));

                            Json = sp.JsonPortFolioCreator(Jsons);

                            portFolio = JsonConvert.DeserializeObject<List<ListProperties>>(Json);
                        }

                    }
                }
                return portFolio;
            });

            await t2;

            return t2.Result;
        }

        // These method accepts a company_session value and returns a list of Chartproperties from
        // the API within the last day associated with the session value.

        public async Task<List<ChartProperties_1D>> OneDay(string company_session)
        {
            Task<List<ChartProperties_1D>> t2 = Task.Factory.StartNew<List<ChartProperties_1D>>(() =>
            {
                List<ChartProperties_1D> oneday = new List<ChartProperties_1D>();
                string Json = String.Empty;
                string url = string.Format("https://api.iextrading.com/1.0/stock/{0}/chart/1d", company_session);
                using (WebClient one_day = new WebClient())
                {
                    Json = one_day.DownloadString(url);
                    //Json = one_day.DownloadStringAsync(url);
                    oneday = JsonConvert.DeserializeObject<List<ChartProperties_1D>>(Json);
                }
                return oneday;
            });

            await t2;

            return t2.Result;

        }

        // Returns a list of Chartproperties from the API within the last month associated
        // with the session value.
        public async Task<List<ChartProperties_MY>> oneMonth(string company_session)
        {
            Task<List<ChartProperties_MY>> t2 = Task.Factory.StartNew<List<ChartProperties_MY>>(() =>
            {
                List<ChartProperties_MY> oneMonth = new List<ChartProperties_MY>();
                string Json = String.Empty;
                string url = string.Format("https://api.iextrading.com/1.0/stock/{0}/chart/1m", company_session);
                using (WebClient one_Month = new WebClient())
                {
                    Json = one_Month.DownloadString(url);
                    oneMonth = JsonConvert.DeserializeObject<List<ChartProperties_MY>>(Json);
                }
                return oneMonth;
            });

            await t2;

            return t2.Result;

        }

        // Returns a list of Chartproperties from the API within the last 3 months associated
        // with the session value.
        public async Task<List<ChartProperties_MY>> ThreeMonth(string company_session)
        {
            Task<List<ChartProperties_MY>> t2 = Task.Factory.StartNew<List<ChartProperties_MY>>(() =>
            {
                List<ChartProperties_MY> oneMonth = new List<ChartProperties_MY>();
                string Json = String.Empty;
                string url = string.Format("https://api.iextrading.com/1.0/stock/{0}/chart/3m", company_session);
                using (WebClient one_Month = new WebClient())
                {
                    Json = one_Month.DownloadString(url);
                    oneMonth = JsonConvert.DeserializeObject<List<ChartProperties_MY>>(Json);
                }
                return oneMonth;
            });

            await t2;
            return t2.Result;

        }

        // Returns a list of Chartproperties from the API within the last 6 months associated 
        // with the session value.
        public async Task<List<ChartProperties_MY>> SixMonth(string company_session)
        {
            Task<List<ChartProperties_MY>> t2 = Task.Factory.StartNew<List<ChartProperties_MY>>(() =>
            {
                List<ChartProperties_MY> oneMonth = new List<ChartProperties_MY>();
                string Json = String.Empty;
                string url = string.Format("https://api.iextrading.com/1.0/stock/{0}/chart/6m", company_session);
                using (WebClient six_Month = new WebClient())
                {
                    Json = six_Month.DownloadString(url);
                    oneMonth = JsonConvert.DeserializeObject<List<ChartProperties_MY>>(Json);
                }
                return oneMonth;
            });

            await t2;
            return t2.Result;

        }

        // Returns a list of Chartproperties from the API within the last year associated 
        // with the session value.
        public async Task<List<ChartProperties_MY>> OneYear(string company_session)
        {
            Task<List<ChartProperties_MY>> t2 = Task.Factory.StartNew<List<ChartProperties_MY>>(() =>
            {
                List<ChartProperties_MY> oneMonth = new List<ChartProperties_MY>();
                string Json = String.Empty;
                string url = string.Format("https://api.iextrading.com/1.0/stock/{0}/chart/1y", company_session);
                using (WebClient one_Month = new WebClient())
                {
                    Json = one_Month.DownloadString(url);
                    oneMonth = JsonConvert.DeserializeObject<List<ChartProperties_MY>>(Json);
                }
                return oneMonth;
            });

            await t2;

            return t2.Result;

        }

        // 
        //public List<ListProperties> companies
        //{
        //    get
        //    {
        //        List<CompanySymbol> companySymbols = new List<CompanySymbol>();

        //        string Constr = ConfigurationManager.ConnectionStrings["ConStrs"].ConnectionString;
        //        using (SqlConnection con = new SqlConnection(Constr))
        //        {
        //            con.Open();
        //            companySymbols = new AccessDB().CompaniesList(con);
        //        }

        //        List<ListProperties> mp = new List<ListProperties>();

        //        foreach (CompanySymbol css in companySymbols)
        //        {
        //            ListProperties individual_com = new ListProperties();

        //            string Json = String.Empty;

        //            string url = string.Format("https://api.iextrading.com/1.0/stock/{0}/quote", css.symbol);

        //            using (WebClient watchlist = new WebClient())
        //            {
        //                Json = watchlist.DownloadString(url);

        //                //individual_com =;

        //                mp.Add(JsonConvert.DeserializeObject<ListProperties>(Json));
        //            }

        //        }
        //        return mp;
        //    }
        //}

        // This method returns a filtered list from the API of companies that are
        // are dropping stock prices. 
        public async Task<List<Last>> LastCompany()
        {
            Task<List<Last>> t2 = Task.Factory.StartNew<List<Last>>(() =>
            {
                string Json = String.Empty;

                string url = string.Format("https://api.iextrading.com/1.0/tops/last");

                List<Last> lasts = new List<Last>();

                using (WebClient topss = new WebClient())
                {
                    Json = topss.DownloadString(url);
                    lasts = JsonConvert.DeserializeObject<List<Last>>(Json);
                }
                foreach (Last companies in lasts)
                {
                    if (companies.price < 0)
                    {
                        lasts.Remove(companies);
                    }
                }
                return lasts;
            });

            await t2;

            return t2.Result;
        }

        // This method returns a filtered list from the API of the top 10 companies with
        // rising stocks prices.
        public async Task<List<ListProperties>> Top_Company()
        {
            Task<List<ListProperties>> t2 = Task.Factory.StartNew<List<ListProperties>>(() =>
            {

                string Json = String.Empty;

                string url = string.Format("https://api.iextrading.com/1.0/tops");

                List<Tops> tops = new List<Tops>();

                List<ListProperties> com = new List<ListProperties>();

                using (WebClient topss = new WebClient())
                {
                    Json = topss.DownloadString(url);
                    tops = JsonConvert.DeserializeObject<List<Tops>>(Json);

                    if (tops.Any())
                    {
                        for (int ad = 0; ad < 10; ad++)
                        {
                            int count_To = rd.Next(tops.Count);

                            string url2 = string.Format("https://api.iextrading.com/1.0/stock/{0}/quote", tops[count_To].symbol);
                            Json = topss.DownloadString(url2);
                            com.Add(JsonConvert.DeserializeObject<ListProperties>(Json));
                        }
                    }

                }
                return com;
            });

            await t2;

            return t2.Result;
        }

        // This method returns a list of NewsSection class objects deserialized from the API.
        // This method in particular returns TOP-GAINER stock values.
        public async Task<List<NewsSection>> MarketNews()
        {
            Task<List<NewsSection>> t2 = Task.Factory.StartNew<List<NewsSection>>(() => {

                Compute compute = new Compute();
                List<NewsSection> news = new List<NewsSection>();
                List<string> json_save = new List<string>();

                Task<List<ListProperties>> lists_gainers = gainers();
                if (lists_gainers.Result.Any())
                {
                    foreach (ListProperties gainers in lists_gainers.Result)
                    {
                        String Json = String.Empty;
                        string gainer_root = string.Format("https://api.iextrading.com/1.0/stock/{0}/news", gainers.symbol);
                        using (WebClient gainer_news = new WebClient())
                        {
                            Json = gainer_news.DownloadString(gainer_root);
                            json_save.Add(Json);
                        }
                    }
                }
                using (WebClient client = new WebClient())
                {

                    string newJson = compute.JsonNewsCreator(json_save);       //Method  in Compute.cs in the Computation Folder
                    news = JsonConvert.DeserializeObject<List<NewsSection>>(newJson);
                }
                return news;
            });

            await t2;

            return t2.Result;
        }
        // This method accepts a symbol parameter and returns a list of objects associated
        // with the symbol value from the API.
        public async Task<List<NewsSection>> CompanyNews(string symbol)
        {
            Task<List<NewsSection>> t2 = Task.Factory.StartNew<List<NewsSection>>(() => {

                Compute compute = new Compute();

                List<NewsSection> news = new List<NewsSection>();

                List<string> json_save = new List<string>();

                String Json = String.Empty;
                string gainer_root = string.Format("https://api.iextrading.com/1.0/stock/{0}/news", symbol);

                using (WebClient company_news = new WebClient())
                {
                    Json = company_news.DownloadString(gainer_root);
                    json_save.Add(Json);
                }

                using (WebClient client = new WebClient())
                {
                    string newJson = compute.JsonNewsCreator(json_save);       //Method  in Compute.cs in the Computation Folder
                    news = JsonConvert.DeserializeObject<List<NewsSection>>(newJson);
                }
                return news;
            });
            await t2;

            return t2.Result;
        }
        // This method returns a list of all gainer company objects from the API. 
        public async Task<List<ListProperties>> gainers()
        {
            Task<List<ListProperties>> t2 = Task.Factory.StartNew<List<ListProperties>>(() =>
            {
                string Json = String.Empty;
                string url = string.Format("https://api.iextrading.com/1.0/stock/market/list/gainers");

                List<ListProperties> gainers = new List<ListProperties>();
                using (WebClient gainer_Client = new WebClient())
                {
                    Json = gainer_Client.DownloadString(url);
                    gainers = JsonConvert.DeserializeObject<List<ListProperties>>(Json);
                }
                return gainers;
            });

            await t2;

            return t2.Result;
        }
        // This method returns a list of the most-active company objects. 
        public async Task<List<ListProperties>> active()
        {
            Task<List<ListProperties>> t2 = Task.Factory.StartNew<List<ListProperties>>(() => {

                string Json1 = String.Empty;
                string url1 = string.Format("https://api.iextrading.com/1.0/stock/market/list/mostactive");
                List<ListProperties> actives = new List<ListProperties>();
                using (WebClient active_client = new WebClient())
                {
                    Json1 = active_client.DownloadString(url1);
                    actives = JsonConvert.DeserializeObject<List<ListProperties>>(Json1);
                }
                return actives;
            });

            await t2;

            return t2.Result;
        }
        // This method returns a list of company objects that are dropping in price.
        public async Task<List<ListProperties>> losers()
        {
            Task<List<ListProperties>> t2 = Task.Factory.StartNew<List<ListProperties>>(() => {

                string Json2 = String.Empty;
                string url2 = string.Format("https://api.iextrading.com/1.0/stock/market/list/losers");
                List<ListProperties> losers = new List<ListProperties>();
                using (WebClient Loser_client = new WebClient())
                {
                    Json2 = Loser_client.DownloadString(url2);
                    losers = JsonConvert.DeserializeObject<List<ListProperties>>(Json2);
                }
                return losers;
            });

            await t2;

            return t2.Result;
        }
        // This method accepts a symbol parameter and returns a list of financials 
        // associated with the symbol value from the API.
        public async Task<List<FinancialInfo>> FinancialSection(string symbol)
        {
            Task<List<FinancialInfo>> t2 = Task.Factory.StartNew<List<FinancialInfo>>(() =>
            {

                List<FinancialInfo> foList = new List<FinancialInfo>();
                string Json1, split = String.Empty;
                string url1 = string.Format("https://api.iextrading.com/1.0/stock/{0}/financials", symbol);

                //connect to api to collect and deserialize object data
                using (WebClient client = new WebClient())
                {
                    Json1 = client.DownloadString(url1);
                    split = Json1.Split(new string[] { "[" }, StringSplitOptions.None)[1].Split(']')[0].Trim();

                    foList = JsonConvert.DeserializeObject<List<FinancialInfo>>("[" + split + "]");
                    //foList = JsonConvert.DeserializeObject<List<Financial>>(Json1);
                }
                return foList;

            });

            await t2;

            return t2.Result;

        }
        // This method accepts a symbol parameter and returns a list of company information 
        // values associated with the symbol value from the API.
        public async Task<CompanyInfo> CompanySection(string symbol)
        {
            Task<CompanyInfo> t2 = Task.Factory.StartNew<CompanyInfo>(() =>
            {
                //create company variables
                CompanyInfo co = new CompanyInfo();
                string Json1 = String.Empty;
                string url1 = string.Format("https://api.iextrading.com/1.0/stock/{0}/company", symbol);

                //connect to api to collect and deserialize object data
                using (WebClient client = new WebClient())
                {
                    Json1 = client.DownloadString(url1);
                    co = JsonConvert.DeserializeObject<CompanyInfo>(Json1);
                }
                return co;
            });

            await t2;

            return t2.Result;
        }

    }
    public class MultipleReturn
    {
        public DisplayList display { get; set; }
        public ChartProperties_1D oneday { get; set; }
    }
    public class ViewPoint
    {
        public LineGraph_AGL CreateCompaniesApi(Dictionary<string, double> companies, string label)
        {
            List<Row> rowlist = new List<Row>();//List of Company Data/Rows
            List<Col> colum1 = new List<Col>();

            // Column labels
            colum1.Add(new Col
            {
                id = "date",
                label = "number",
                pattern = "string",
                type = "string"
            });
            colum1.Add(new Col
            {
                id = "Date",
                label = label,
                pattern = "title1",
                type = "number"
            });
            //Contains Data for each companies
            foreach (KeyValuePair<string, double> items in companies)
            {
                Row rows = new Row();// Represent One Company
                rows.c = new List<C>();
                rows.c.Add(new C
                {
                    v = items.Key
                });
                rows.c.Add(new C
                {
                    v = items.Value
                });
                rowlist.Add(rows);// Add company to RowList
            }

            LineGraph_AGL sp = new LineGraph_AGL();//Combining the Columns And Row Together
            sp.rows = rowlist;
            sp.cols = colum1;

            return sp;
        }

        public List<ListProperties> _gainers { get; set; }
        public List<ListProperties> _losers { get; set; }
        public List<ListProperties> _active { get; set; }
        public List<ListProperties> _topComp { get; set; }
        public List<NewsSection> _CompanyNews { get; set; }
        public List<FinancialInfo> _financial { get; set; }
        public CompanyInfo _CompanySection { get; set; }
        public List<NewsSection> _marketNews { get; set; }
        public List<ListProperties> _databseCompanies { get; set; }
        public List<CompanySymbol> _randomCompany { get; set; }
        public List<ListProperties> _portfolioList { get; set; }
        public List<ChartProperties_1D> _oneDay { get; set; }
        public List<ChartProperties_MY> _oneMonth { get; set; }
        public List<ChartProperties_MY> _threeMonth { get; set; }
        public List<ChartProperties_MY> _sixMonth { get; set; }
        public List<ChartProperties_MY> _oneYear { get; set; }
        public List<Last> _Lastcompany { get; set; }

        //Charts Section
        public string ActiveChartVolume
        {
            get
            {
                Dictionary<string, double> activeChartVolume = new Dictionary<string, double>();
                foreach (ListProperties rst in _active)
                {
                    activeChartVolume.Add(rst.symbol, Convert.ToDouble(rst.latestVolume));
                }
                return JsonConvert.SerializeObject(CreateCompaniesApi(activeChartVolume, " Active Volume"));//Convert Object to Json
            }
        }
        public string LosersChartVolume
        {
            get
            {
                Dictionary<string, double> loserChartVolume = new Dictionary<string, double>();
                foreach (ListProperties rst in _losers)
                {
                    loserChartVolume.Add(rst.symbol, Convert.ToDouble(rst.latestVolume));
                }
                return JsonConvert.SerializeObject(CreateCompaniesApi(loserChartVolume, "Losers Volume"));//Convert Object to Json
            }
        }
        public string GainersChartVolume
        {
            get
            {
                Dictionary<string, double> gainerscompanies = new Dictionary<string, double>();
                foreach (ListProperties rst in _gainers)
                {
                    gainerscompanies.Add(rst.symbol, Convert.ToDouble(rst.latestVolume));
                }//Convert Object to Json
                return JsonConvert.SerializeObject(CreateCompaniesApi(gainerscompanies, "Gainers Volume"));
            }
        }

        public string _losersChart
        {
            get
            {
                Dictionary<string, double> losescompanies = new Dictionary<string, double>();
                foreach (ListProperties rst in _losers)
                {
                    losescompanies.Add(rst.symbol, Convert.ToDouble(rst.latestPrice));
                }
                return JsonConvert.SerializeObject(CreateCompaniesApi(losescompanies, "losers"));
            }
        }
        public string _gainersChart
        {
            get
            {
                Dictionary<string, double> gainerscompanies = new Dictionary<string, double>();
                foreach (ListProperties rst in _gainers)
                {
                    gainerscompanies.Add(rst.symbol, Convert.ToDouble(rst.latestPrice));
                }
                return JsonConvert.SerializeObject(CreateCompaniesApi(gainerscompanies, "gainers"));
            }
        }
        public string _activeChart
        {
            get
            {
                Dictionary<string, double> activecompany = new Dictionary<string, double>();
                foreach (ListProperties rst in _active)
                {
                    activecompany.Add(rst.symbol, Convert.ToDouble(rst.latestPrice));
                }

                return JsonConvert.SerializeObject(CreateCompaniesApi(activecompany, "Active"));
            }
        }

    }
}