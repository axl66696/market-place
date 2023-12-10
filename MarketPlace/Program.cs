using Dapper;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace MarketPlace
{
    internal class Program
    {
        
        
        static void Main(string[] args)
        {
            InitSQLiteDb();

            //------------------Input--------------------
            while (true)
            {
                //Console.Write("TEST");
                Console.Write("# ");
                string input = Console.ReadLine();
                List<string> command = ParseQuotedStrings(input);

                switch (command[0])
                {
                    case "REGISTER":
                        //REGISTER_USER(command[1]);
                        UserBase.REGISTER_USER(command[1]);
                        break;
                    case "CREATE_LISTING":
                        ListingBase.CREATE_LISTING(command[1], command[2], command[3], int.Parse(command[4]), command[5]);
                        break;
                    case "DELETE_LISTING":
                        ListingBase.DELETE_LISTING(command[1], int.Parse(command[2]));
                        break;
                    case "GET_LISTING":
                        ListingBase.GET_LISTING(command[1], int.Parse(command[2]));
                        break;
                    case "GET_CATEGORY":
                        ListingBase.GET_CATEGORY(command[1], command[2], command[3], command[4]);
                        break;
                    case "GET_TOP_CATEGORY":
                        ListingBase.GET_TOP_CATEGORY(command[1]);
                        break;
                }

                if (command[0].ToLower() == "exit") break;
            }
        }

        static List<string> ParseQuotedStrings(string input)
        {
            List<string> values = new List<string>();
            bool insideQuotes = false;
            string currentString = "";

            foreach (char c in input)
            {
                if (c == '\'' && !insideQuotes)
                {
                    insideQuotes = !insideQuotes;
                }
        
                else if (c == '\'' && insideQuotes)
                {
                    if (!string.IsNullOrEmpty(currentString))
                    {
                        values.Add(currentString);
                        currentString = "";
                    }
                    insideQuotes = !insideQuotes;
                }
                else if (c == ' ' && !insideQuotes)
                {
                    if (!string.IsNullOrEmpty(currentString))
                    {
                        values.Add(currentString);
                        currentString = "";
                    }
                }
                else
                {
                    currentString += c;
                }
            }

            if (!string.IsNullOrEmpty(currentString))
            {
                values.Add(currentString);
            }

            return values;
        }


        public class CategoryBase
        {
            public string Name { get; set; }

            public CategoryBase(string name)
            {
                Name = name;
            }
        }

        static UserBase[] DefaultData = new UserBase[]
            {
                new UserBase("Alpha")
            };

        static ListingBase[] DefaultListing = new ListingBase[]
        {
            new ListingBase(100000, "Tommy", "iPhone XR", "Black color, brand new", 1100, new DateTime(2023, 11, 15, 12, 30, 0), "Default"),
        };

        static CategoryBase[] DefaultCategory = new CategoryBase[]
        {
            new CategoryBase("Electronics")
        };

        

        static string dbPath = @".\Test.sqlite";
        static string cnStr = "data source=" + dbPath;

        static void InitSQLiteDb()
        {
            if (File.Exists(dbPath)) return;
            using (var cn = new SQLiteConnection(cnStr))
            {
                cn.Open();
                cn.Execute(@"
                    CREATE TABLE User (
                    Name VARCHAR(32),
                    CONSTRAINT User_PK PRIMARY KEY (Name)
                )");
                cn.Execute(@"
                    CREATE TABLE Listing (
                    Id INTEGER,
                    UserName VARCHAR(32),
                    Title VARCHAR(100),
                    Description TEXT,
                    Price INTEGER,
                    CreationTime DATE,
                    Category VARCHAR(32),
                    CONSTRAINT Listing_PK PRIMARY KEY (id)
                 )");
                cn.Execute(@"
                    CREATE TABLE Category (
                    Name VARCHAR(32),
                    CONSTRAINT Category_PK PRIMARY KEY (Name)
                )");

                InsertDefaultData();
            }
        }


        static void InsertDefaultData()
        {
            using (var cn = new SQLiteConnection(cnStr))
            {
                cn.Execute("DELETE FROM User");
                var insertScript = " INSERT INTO User VALUES (@Name)";
                var insertListing = " INSERT INTO Listing VALUES (@Id, @UserName, @Title, @Description, @Price, @CreationTime, @Category)";
                var insertCategory = " INSERT INTO Category VALUES (@Name)";

                cn.Execute(insertScript, DefaultData);
                cn.Execute(insertListing, DefaultListing);
                cn.Execute(insertCategory, DefaultCategory);
            }
        }

        //public static UserBase GetUser(string name)
        //{
        //    var sql =
        //        @"
        //            SELECT *
        //            FROM User
        //            Where Name = @Name
        //        ";

        //    var parameters = new DynamicParameters();
        //    parameters.Add("Name", name);
            
        //    using (var cn = new SQLiteConnection(cnStr)) {

        //        var rawUser = cn.QueryFirstOrDefault<UserBase>(sql, parameters);

        //        if (rawUser != null) {
        //            return rawUser;
        //        }
        //        else
        //        {
        //            return null;
        //        }

        //    }
        //}


        //public static bool InsertUser(UserBase user)
        //{
        //    var sql =
        //        @"
        //            INSERT INTO User
        //            (
        //                [Name]
        //            )
        //            VALUES
        //            (
        //                @Name
        //            );

        //        ";

        //    using (var cn = new SQLiteConnection(cnStr)) {

        //        var result = cn.Execute(sql, user);
        //        return result > 0;
        //    }
        //}

        

        //---------------------------Listing--------------------------

        //public static List<ListingBase> GetAllListings()
        //{
        //    var sql =
        //        @"
        //            SELECT *
        //            FROM Listing
        //        ";

        //    using (var cn = new SQLiteConnection(cnStr)) {

        //        var rawListing = cn.Query<ListingBase>(sql).ToList();

        //        if (rawListing != null){
        //            return rawListing;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //}

        //public static ListingBase GetListing(int id)
        //{
        //    var sql =
        //        @"
        //            SELECT *
        //            FROM Listing
        //            WHERE Id = @Id
        //        ";

        //    var parameters = new DynamicParameters();
        //    parameters.Add("Id", id);

        //    using (var cn = new SQLiteConnection(cnStr)) {

        //        ListingBase rawListing = cn.QueryFirstOrDefault<ListingBase>(sql, parameters);
        //        return rawListing;
        //    }
        //}

        //public static ListingBase GetLatestListing()
        //{
        //    var sql =
        //        @"
        //            SELECT *
        //            FROM Listing
        //            ORDER BY CreationTime DESC
        //            LIMIT 1
        //        ";

        //    using (var cn = new SQLiteConnection(cnStr)) {

        //        ListingBase rawListing = cn.QueryFirstOrDefault<ListingBase>(sql);
        //        return rawListing;  
        //    }

        //}
        
        //public static bool InsertListing(ListingBase listing)
        //{
        //    var sql =
        //        @"
        //            INSERT INTO Listing
        //            (
                        
        //                [Id],
        //                [UserName],
        //                [Title],
        //                [Description],
        //                [Price],
        //                [CreationTime],
        //                [Category]
        //            )
        //            VALUES
        //            (
        //                @Id,
        //                @UserName,
        //                @Title,
        //                @Description,
        //                @Price,
        //                @CreationTime,
        //                @Category

        //            );

        //        ";
        //    using (var cn = new SQLiteConnection(cnStr)) {

        //        var result = cn.Execute(sql, listing);
        //        Console.WriteLine(listing.Id);
        //        return result > 0;
        //    }
        //}

        //public static void DeleteListing(int id)
        //{
        //    var sql =
        //        @"
        //            DELETE FROM Listing
        //            WHERE Id = @Id

        //        ";
        //    var parameters = new DynamicParameters();
        //    parameters.Add("Id", id, System.Data.DbType.Int32);
            
        //    using (var cn = new SQLiteConnection(cnStr)) {

        //        var result = cn.Execute(sql, parameters);
        //    }
        //}


        //----------------------------category-------------------

        //public static List<ListingBase> GetCategory(string userName, string category)
        //{
        //    var sql =
        //        @"
        //            SELECT *
        //            FROM Listing
        //            WHERE Category = @Category
        //        ";
        //    var parameters = new DynamicParameters();
        //    parameters.Add("Category", category);

        //    using (var cn = new SQLiteConnection(cnStr)) {

        //        var rawListing = cn.Query<ListingBase>(sql, parameters).ToList();
        //        return rawListing;
        //    }
        //}

        //public static void GetTopCategory()
        //{
        //    var sql =
        //        @"
        //            SELECT Category, COUNT(*) AS ListingCount
        //            FROM Listing
        //            GROUP BY Category
        //            ORDER BY ListingCount DESC
        //            LIMIT 1
        //        ";

        //    using (var cn = new SQLiteConnection(cnStr)) {

        //        var rawCategory = cn.QueryFirstOrDefault<ListingBase>(sql);
        //        if (rawCategory != null) {
        //            Console.WriteLine(rawCategory.Category);
        //        }
        //        else
        //        {
        //            Console.WriteLine("No Top Category");
        //        }
                
        //    }
        //}


        //static void REGISTER_USER(string name)
        //{
        //    if (GetUser(name) == null) {

        //        UserBase newUser = new UserBase(name);
        //        if (InsertUser(newUser)) {
        //            Console.WriteLine("Success");
        //        }
        //    } 
        //    else
        //    {
        //        Console.WriteLine("Error - user already existing");
        //    }
        //}

        //static void CREATE_LISTING(string userName, string title, string description, int price, string category)
        //{
        //    if (GetUser(userName) == null) {
        //        Console.WriteLine("Error - unknown user");
        //        return;
        //    }

        //    ListingBase latestListing = GetLatestListing();
        //    var newId = latestListing.Id + 1;
        //    var currentTime = DateTime.Now;
        //    var newListing = new ListingBase(newId, userName, title, description, price, currentTime, category);
        //    InsertListing(newListing);

        //}

        //static void DELETE_LISTING(string userName, int id)
        //{
        //    ListingBase listing = GetListing(id);
        //    if (listing == null) {
        //        Console.WriteLine("Error - listing does not exist");
        //        return;
        //    }
        //    else if (userName != listing.UserName) {
        //        Console.WriteLine("Error - listing owner mismatch");
        //        return;
        //    }
        //    else
        //    {
        //        DeleteListing(id);
        //        Console.WriteLine("Success");
        //    }
        //}

        //static void GET_LISTING(string userName, int id)
        //{
        //    UserBase user = GetUser(userName);
        //    if (user == null)
        //    {
        //        Console.WriteLine("Error - unknown user");
        //        return;
        //    }

        //    ListingBase listing = GetListing(id);
        //    if (listing == null)
        //    {
        //        Console.WriteLine("Error - not found");
        //        return;
        //    }
        //    else
        //    {
        //        Console.WriteLine(listing.Title + '|' + listing.Description + '|'
        //            + listing.Price + '|' + listing.CreationTime + '|'
        //            + listing.Category + '|' + listing.UserName);
        //        return;
        //    }
            
        //}

        //static void GET_CATEGORY(string userName, string category, string sortedBy, string order)
        //{
        //    UserBase user = GetUser(userName);
        //    if (user == null)
        //    {
        //        Console.WriteLine("Error - unknown user");
        //        return;
        //    }
        //    List<ListingBase> rawListings = GetCategory(userName, category);

        //    if (!rawListings.Any())
        //    {
        //        Console.WriteLine("Error - category not found");
        //    }
        //    else
        //    {
        //        string sortedParam = (sortedBy == "sort_time") ? "CreationTime" : "Price";
        //        var tmpResult = order == "ASC"
        //            ? rawListings.OrderBy(x => x.GetType().GetProperty(sortedParam).GetValue(x, null))
        //            : rawListings.OrderByDescending(x => x.GetType().GetProperty(sortedParam).GetValue(x, null));
                
        //        List<ListingBase> listings = tmpResult.ToList();

        //        foreach (ListingBase listing in listings) 
        //        {
        //            Console.WriteLine(listing.Title + '|' + listing.Description + '|' + listing.Price + '|' + listing.CreationTime + '|' + listing.Category + '|' + listing.UserName);
        //        }
        //    }
        //}

        //static void GET_TOP_CATEGORY(string userName)
        //{
        //    UserBase user = GetUser(userName);
        //    if (user == null)
        //    {
        //        Console.WriteLine("Error - unknown user");
        //        return;
        //    }

        //    GetTopCategory();

        //}
    }
}
