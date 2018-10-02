using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Linq;
using System.Web;

namespace StockMarket.Models.AccessDatabse
{
    // Class objects
    public class Person
    {
        public string UserID { get; set; }
        public string FName { get; set; } // Column in Table
        public string LName { get; set; }// Column in Table
        public string username { get; set; }// Column in Table
        public string email { get; set; }// Column in Table
        public string password { get; set; }// Column in Table
    }
    public class CompanySymbol
    {
        public string Id { get; set; }// Column in Table
        public string symbol { get; set; }// Column in Table
        public string name { get; set; }// Column in Table
        public string date { get; set; }// Column in Table
        public string isEnabled { get; set; }// Column in Table
        public string type { get; set; }// Column in Table
        public float iexId { get; set; }// Column in Table
    }
    public class PortFolio
    {
        public string ID { get; set; }// Column in Table
        public string UserID { get; set; }// Column in Table
        public string CompanyID { get; set; }// Column in Table
        public string Name { get; set; }// Column in Table
    }
    // AccessDB class that stores all database-access functions
    public class AccessDB
    {
        // This method accepts a username and password, then verifies that the user
        // exists within the database and returns the values.

        public List<Person> Login_Check(string username, string password, SqlConnection connection)//Validate LogIN
        {
            List<Person> person1 = new List<Person>();

            string query = String.Format("SELECT UserID, username, passwords FROM dbo.tblUserInfo WHERE dbo.tblUserInfo.username='{0}' AND dbo.tblUserInfo.passwords='{1}'", username, password);
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader read = command.ExecuteReader())
                {
                    if (read.HasRows)
                    {
                        while (read.Read())
                        {
                            person1.Add(new Person
                            {
                               UserID = read["UserID"].ToString(),
                               username = read["username"].ToString(),
                               password = read["passwords"].ToString()
                            });
                        }
                    }
                }
            }

            return person1;
        }

        // This method accepts a user ID value and fetches all the user's saved portfolio items from the database.
        // The populated list is then returned. 

        public List<PortFolio> ListPortFolio(string id, SqlConnection connection)
        {
            Task<List<PortFolio>> t2 = Task.Factory.StartNew<List<PortFolio>>(() =>
            {
                List<PortFolio> cys = new List<PortFolio>();

                string query = string.Format("SELECT UserID,CompanyId,Name FROM dbo.PortFolio WHERE UserId = '{0}'", id);
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                cys.Add(new PortFolio
                                {
                                    UserID = reader["UserID"].ToString(),
                                    CompanyID = reader["CompanyId"].ToString(),
                                    Name = reader["Name"].ToString()
                                });
                            }
                        }
                    }
                }
                return cys;
            });
            return t2.Result;
        }

        // This method accepts userID and companyID parameters and the plugs them into a query that 
        // inserts this information into the Portfolio table.
        // This in-turn allows the user to add company pages to his/her portfolio.

        public void InsertPortfolio(string userID, string companyID,string name, SqlConnection connection)
        {
            string query = string.Format("Insert into dbo.PortFolio (UserID,CompanyId,Name) Values('{0}', '{1}', '{2}')" + "Select @@Identity",userID,companyID,name);
            using (SqlCommand com = new SqlCommand(query, connection))
            {
                com.ExecuteScalar();
            }
        }

        // This method accepts a person object and inserts the values into the database tblUserInfo table.
        // After the user is created in the database, the user ID is returned.

        public int InsertPerson(Person person, SqlConnection connection)//Insert New Person
        {
            int userid = 0;
            string query_id = String.Format("Insert into dbo.tblUserInfo(Fname,Lname,username,passwords,email) Values('{0}', '{1}', '{2}', '{3}','{4}')" + "Select @@Identity", person.FName, person.LName, person.username, person.password, person.email);
            using (SqlCommand com = new SqlCommand(query_id, connection))
            {
                userid = Convert.ToInt32(com.ExecuteScalar());
            }
            return userid;
        }

        // CompanyCheck method accepts a companySymbol parameter and verifies that the symbol exists in the database.
        // It returns the symbol if true and a null value otherwise. 

        public string CompanyCheck(string companySymbol, SqlConnection connection)
        {
            string confirm = "";
            string query = String.Format("SELECT Companyid FROM CompanySymbol_s WHERE Symbol LIKE '{0}'", companySymbol);
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        confirm = rdr["Companyid"].ToString();
                        if (confirm!=null)
                        {
                            return companySymbol;
                        }
                    }
                }
            }
            return null;
        }

        // This method accepts a symbol and selects all company information from table CompanySymbol_s associated with the value.
        // It saves all the values from the database to a list and returns it.

        public List<CompanySymbol> CompaniesPortFolio(string symbol, SqlConnection connection)// GET User Company PortFolio
        {
            Task<List<CompanySymbol>> t2 = Task.Factory.StartNew<List<CompanySymbol>>(() =>
            {
                List<CompanySymbol> cys = new List<CompanySymbol>();

                string query = string.Format("SELECT CompanyId,Symbol,Name,Date,Enabled,Type FROM CompanySymbol_s WHERE Symbol LIKE '{0}'", symbol);
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                cys.Add(new CompanySymbol
                                {
                                    Id = reader["CompanyId"].ToString(),
                                    symbol = reader["Symbol"].ToString(),
                                    name = reader["Name"].ToString(),
                                    date = reader["Date"].ToString(),
                                    isEnabled = reader["Enabled"].ToString(),
                                    type = reader["Type"].ToString()
                                });
                            }
                        }
                    }
                }
                return cys;
            });
            return t2.Result;
        }

        // This method is used to return the top 200 companies from CompanySymbol_s table as a list object.

        public List<CompanySymbol> CompaniesList(SqlConnection connection)
        {
            Task<List<CompanySymbol>> t2 = Task.Factory.StartNew<List<CompanySymbol>>(() =>
            {
                List<CompanySymbol> cys = new List<CompanySymbol>();

                string query = string.Format("SELECT TOP 200 * FROM dbo.CompanySymbol_s ORDER BY NEWID()");
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                cys.Add(new CompanySymbol
                                {
                                    Id = reader["CompanyId"].ToString(),
                                    symbol = reader["Symbol"].ToString(),
                                    name = reader["Name"].ToString(),
                                    date = reader["Date"].ToString(),
                                    isEnabled = reader["Enabled"].ToString(),
                                    type = reader["Type"].ToString()
                                });
                            }
                        }
                    }
                }
                return cys;
            });
            return t2.Result;
        }
        
    }
}