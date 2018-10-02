using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockMarket.Models.List;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using StockMarket.Models.AccessDatabse;

namespace StockMarket.Controllers
{
    public class ProfileAsyncController : Controller
    {
        DisplayList list = new DisplayList();

        ViewPoint view = new ViewPoint();

        // This method recieves the posted information from the registration form and creates a
        // person object from the values. It then passes the object to the InsertPerson method
        // to insert the new user into the database.
        [HttpPost]
        public ActionResult Register()
        {
            Session["error"] = null;
            string conStr = ConfigurationManager.ConnectionStrings["ConStrs"].ConnectionString;
            AccessDB accessDB = new AccessDB();
            int id = 0;
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                Person person = new Person
                {
                    FName = Request["fname"],
                    LName = Request["lname"],
                    username = Request["username"],
                    password = Request["password"],
                    email = Request["email"]
                };
                id = new AccessDB().InsertPerson(person, con);
                Session["username"] = person.username;
                Session["password"] = person.password;
            }
            return RedirectToAction("Index", "HomeAsync", null);
            //accessDB.InsertPerson
        }

        // This method requests the username and password values posted from the login form. It
        // then passes them to the Login_Check method to validate if the user exists in the 
        // database. If the user is verified, the controller redirects the user to the home-view.
        [HttpPost]
        public ActionResult Login()
        {
            Session["error"] = null;
            string username = Request["username"];
            string password = Request["password"];

            List<Person> people = null;
            string conStr = ConfigurationManager.ConnectionStrings["ConStrs"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                people = new AccessDB().Login_Check(username, password, con); ;

                if (people.Any())
                {
                    Session["username"] = username;
                    Session["password"] = password;
                    return RedirectToAction("Index", "HomeAsync", null);
                }
                else
                {
                    Session["error"] = "Invalid login";
                    return RedirectToAction("RegisterForm", "ProfileAsync", null);
                }
            }
        }
        public ActionResult RegisterForm()
        {
            Session["username"] = null;
            return View();
        }

        // This method requests posted input from the search form and passes the value 
        // to the CompanyCheck method to verify that the company exists in the database. If so,
        // the symbol is passed to the CompanySite view to be populated.
        [HttpPost]
        public ActionResult Search()
        {
            string symbol = Request["input"];
            string confirm_symbol = "";
            string conStr = ConfigurationManager.ConnectionStrings["ConStrs"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                confirm_symbol = new AccessDB().CompanyCheck(symbol, con);
                if (confirm_symbol != null)
                {
                    return RedirectToAction("CompanySite", "HomeAsync", new { com = symbol });
                }
                con.Close();
            }
            return RedirectToAction("Index", "HomeAsync", null);
        }
        public async Task<ActionResult> SearchForm()
        {

            var randomCom = list.RandomCompany();

            await Task.WhenAll(randomCom);

            view._randomCompany = randomCom.Result;

            return View(view);
            
        }
        [HttpGet]
        public async Task<ActionResult> PortFolioForm()
        {
            string username = (string)Session["username"];
            string password = (string)Session["password"];

            var portFolio = list.PortFolioList(username,password);

            await Task.WhenAll(portFolio);

            view._portfolioList = portFolio.Result;

            return View(view);
        }
    }
}