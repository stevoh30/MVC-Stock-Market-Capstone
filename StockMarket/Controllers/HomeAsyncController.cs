using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Web.Mvc;
using StockMarket.Models.List;
using System.Data.SqlClient;
using StockMarket.Models.AccessDatabse;
using System.Configuration;

namespace StockMarket.Controllers
{
    public class HomeAsyncController : Controller
    {
        DisplayList list = new DisplayList();
       

        ViewPoint view = new ViewPoint();

        string conStr = ConfigurationManager.ConnectionStrings["ConStrs"].ConnectionString;// Connection String 
        // GET: HomeAsync
        public async Task<ActionResult> Index()
        {
            // Get the Gainers
            var gainers = list.gainers();
            // Get the Losers
            var losers = list.losers();
            // Get the Active
            var active = list.active();
            // Get the MarketNews
            var marketnews = list.MarketNews();
            // Get the Top
            var top = list.Top_Company();
            // Get the Last
            var last = list.LastCompany();
            // Await for All Data to be Collected.
            await Task.WhenAll(gainers, losers, active, marketnews, top);

            //What is Returned to the View
            view._gainers = gainers.Result;
           
            view._losers = losers.Result;
            
            view._marketNews = marketnews.Result;

            view._active = active.Result;

            view._topComp = top.Result;

            view._Lastcompany = last.Result;

            //view.

            return View(view);
        }

        //GET: CompanyOverView
        public async Task<ActionResult> CompanyOverView()
        {
            //Get the DatabaseCompanies
            var databaseCompanies = list.DatabaseCompanies();

            await Task.WhenAll(databaseCompanies); // Await for the Data to be Collected
            //What is Returned
            view._databseCompanies = databaseCompanies.Result;

            return View(view);
        }

        //Get: OverViewAsync
        public async Task<ActionResult> OverViewAsync()
        {
            var gainers = list.gainers();

            var losers = list.losers();

            var active = list.active();

            var marketnews = list.MarketNews();

            var top = list.Top_Company();

            var last = list.LastCompany();

            await Task.WhenAll(gainers, losers, active, marketnews, top);// Await for All Data to be Collected.

            view._gainers = gainers.Result;

            view._losers = losers.Result;

            view._marketNews = marketnews.Result;

            view._active = active.Result;

            view._topComp = top.Result;

            view._Lastcompany = last.Result;

            return View(view);
        }

        //GET: Company That was Clicked On : Get the Information relating to the Company including the Query string which contains the symbol

        public async Task<ActionResult> CompanySite() 
        {
            string comp = Request.Params["com"]; // Get the Company Symbol

            var gainers = list.gainers();// Get the Gainers

            var oneday = list.OneDay(comp);// Get Day One

            var oneMonth = list.oneMonth(comp);// Get Month on 

            var threeMonth = list.ThreeMonth(comp);// Get Three Months

            var sixMonth = list.SixMonth(comp);// Get Six Months

            var oneYear = list.OneYear(comp);// Get One Year

            //Get the Company News Associated With It
            var news = list.CompanyNews(comp);

            //Get the CompanySection 
            var companySection = list.CompanySection(comp);//Get the Company Section, This is Information about the Company

            var fincialReport = list.FinancialSection(comp);// Get the Finicial Report of the Company 


            await Task.WhenAll(gainers,oneday, oneMonth, threeMonth, sixMonth, oneYear, news, companySection, fincialReport); // Await for All Data to be Collected.

            //What is Returned to the View

            //Get The CHart Information
            view._gainers = gainers.Result; 

            view._oneDay = oneday.Result;

            view._oneMonth = oneMonth.Result;

            view._threeMonth = threeMonth.Result;

            view._sixMonth = sixMonth.Result;

            view._oneYear = oneYear.Result;

            //Get the News and Company

            view._CompanyNews = news.Result;

            view._CompanySection = companySection.Result;

            //Get the Financial

            view._financial = fincialReport.Result; 

            return View(view); // Return the ViewPoint

        }
        
    }
}