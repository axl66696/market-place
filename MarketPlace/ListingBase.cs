using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public class ListingBase
    {
        private static IDataBase _db = new SQLliteService(@".\Test.sqlite");
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public DateTime CreationTime { get; set; }
        public string Category { get; set; }

        //public ListingBase()
        //{

        //}


        public ListingBase(int id, string name, string title, string description, int price, DateTime creationTime, string category)
        {
            Id = id;
            UserName = name;
            Title = title;
            Description = description;
            Price = price;
            CreationTime = creationTime;
            Category = category;
        }


        public static ListingBase GetLatestListing()
        {
            var sql =
                @"
                    SELECT *
                    FROM Listing
                    ORDER BY CreationTime DESC
                    LIMIT 1
                ";

            var rawListing = _db.QueryFirstOrDefault(sql);
            return (ListingBase)rawListing;
        //using (var cn = new SQLiteConnection(cnStr))
        //{

        //    ListingBase rawListing = cn.QueryFirstOrDefault<ListingBase>(sql);
        //    return rawListing;
        //}

    }

        public static bool InsertListing(ListingBase listing)
        {
            var sql =
                @"
                    INSERT INTO Listing
                    (
                        
                        [Id],
                        [UserName],
                        [Title],
                        [Description],
                        [Price],
                        [CreationTime],
                        [Category]
                    )
                    VALUES
                    (
                        @Id,
                        @UserName,
                        @Title,
                        @Description,
                        @Price,
                        @CreationTime,
                        @Category

                    );

                ";
                var result = _db.Execute(sql, listing);
                Console.WriteLine(listing.Id);
                return true;

        //using (var cn = new SQLiteConnection(cnStr))
        //{

        //    var result = cn.Execute(sql, listing);
        //    Console.WriteLine(listing.Id);
        //    return result > 0;
        //}
    }

        public static ListingBase GetListing(int id)
        {
            var sql =
                @"
                    SELECT *
                    FROM Listing
                    WHERE Id = @Id
                ";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id);

            var rawListing = _db.QueryFirstOrDefault(sql, parameters);
            return (ListingBase)rawListing;

        //using (var cn = new SQLiteConnection(cnStr))
        //{

        //    ListingBase rawListing = cn.QueryFirstOrDefault<ListingBase>(sql, parameters);
        //    return rawListing;
        //}
    }

        public static List<ListingBase> GetAllListings()
        {
            var sql =
                @"
                    SELECT *
                    FROM Listing
                ";

            List<ListingBase> rawListing = _db.Query(sql).Cast<ListingBase>().ToList();

            return rawListing;

            //using (var cn = new SQLiteConnection(cnStr))
            //{

            //    var rawListing = cn.Query<ListingBase>(sql).ToList();

            //    if (rawListing != null)
            //    {
            //        return rawListing;
            //    }
            //    else
            //    {
            //        return null;
            //    }
            //}
        }

        public static void DeleteListing(int id)
        {
            var sql =
                @"
                    DELETE FROM Listing
                    WHERE Id = @Id

                ";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, System.Data.DbType.Int32);

            _db.Execute(sql, parameters);
        
            //using (var cn = new SQLiteConnection(cnStr))
            //{

            //    var result = cn.Execute(sql, parameters);
            //}
        }

        public static List<ListingBase> GetCategory(string userName, string category)
        {
            var sql =
                @"
                    SELECT *
                    FROM Listing
                    WHERE Category = @Category
                ";
            var parameters = new DynamicParameters();
            parameters.Add("Category", category);

            List<ListingBase> rawListing = _db.Query(sql, parameters).Cast<ListingBase>().ToList();
            return rawListing;

        //using (var cn = new SQLiteConnection(cnStr))
        //{

        //    var rawListing = cn.Query<ListingBase>(sql, parameters).ToList();
        //    return rawListing;
        //}
    }

        public static void GetTopCategory()
        {
            var sql =
                @"
                    SELECT Category, COUNT(*) AS ListingCount
                    FROM Listing
                    GROUP BY Category
                    ORDER BY ListingCount DESC
                    LIMIT 1
                ";
            ListingBase rawCategory = (ListingBase) _db.QueryFirstOrDefault(sql);
            if (rawCategory != null)
            {
                Console.WriteLine(rawCategory.Category);
            }
            else
            {
                Console.WriteLine("No Top Category");
            }

            //using (var cn = new SQLiteConnection(cnStr))
            //{

            //    var rawCategory = cn.QueryFirstOrDefault<ListingBase>(sql);
            //    if (rawCategory != null)
            //    {
            //        Console.WriteLine(rawCategory.Category);
            //    }
            //    else
            //    {
            //        Console.WriteLine("No Top Category");
            //    }

            //}
    }

        public static void CREATE_LISTING(string userName, string title, string description, int price, string category)
        {
            if (UserBase.GetUser(userName) == null)
            {
                Console.WriteLine("Error - unknown user");
                return;
            }

            ListingBase latestListing = GetLatestListing();
            var newId = latestListing.Id + 1;
            var currentTime = DateTime.Now;
            var newListing = new ListingBase(newId, userName, title, description, price, currentTime, category);
            InsertListing(newListing);

        }

        public static void GET_LISTING(string userName, int id)
        {
            UserBase user = UserBase.GetUser(userName);
            if (user == null)
            {
                Console.WriteLine("Error - unknown user");
                return;
            }

            ListingBase listing = GetListing(id);
            if (listing == null)
            {
                Console.WriteLine("Error - not found");
                return;
            }
            else
            {
                Console.WriteLine(listing.Title + '|' + listing.Description + '|'
                    + listing.Price + '|' + listing.CreationTime + '|'
                    + listing.Category + '|' + listing.UserName);
                return;
            }

        }

        public static void DELETE_LISTING(string userName, int id)
        {
            ListingBase listing = GetListing(id);
            if (listing == null)
            {
                Console.WriteLine("Error - listing does not exist");
                return;
            }
            else if (userName != listing.UserName)
            {
                Console.WriteLine("Error - listing owner mismatch");
                return;
            }
            else
            {
                DeleteListing(id);
                Console.WriteLine("Success");
            }
        }

        public static void GET_CATEGORY(string userName, string category, string sortedBy, string order)
        {
            UserBase user = UserBase.GetUser(userName);
            if (user == null)
            {
                Console.WriteLine("Error - unknown user");
                return;
            }
            List<ListingBase> rawListings = GetCategory(userName, category);

            if (!rawListings.Any())
            {
                Console.WriteLine("Error - category not found");
            }
            else
            {
                string sortedParam = (sortedBy == "sort_time") ? "CreationTime" : "Price";
                var tmpResult = order == "ASC"
                    ? rawListings.OrderBy(x => x.GetType().GetProperty(sortedParam).GetValue(x, null))
                    : rawListings.OrderByDescending(x => x.GetType().GetProperty(sortedParam).GetValue(x, null));

                List<ListingBase> listings = tmpResult.ToList();

                foreach (ListingBase listing in listings)
                {
                    Console.WriteLine(listing.Title + '|' + listing.Description + '|' + listing.Price + '|' + listing.CreationTime + '|' + listing.Category + '|' + listing.UserName);
                }
            }
        }

        public static void GET_TOP_CATEGORY(string userName)
        {
            UserBase user = UserBase.GetUser(userName);
            if (user == null)
            {
                Console.WriteLine("Error - unknown user");
                return;
            }

            GetTopCategory();

        }

    }

