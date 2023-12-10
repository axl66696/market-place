using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public class UserBase
    {
        private static IDataBase _db = new SQLliteService(@".\Test.sqlite");
        public string Name { get; set; }

    //public UserBase()
    //{

    //}

       
        public UserBase(string name)
        {
            Name = name;
        }

        

        public static UserBase GetUser(string name)
        {
            var sql =
                @"
                    SELECT *
                    FROM User
                    Where Name = @Name
                ";

            var parameters = new DynamicParameters();
            parameters.Add("Name", name);
            
            var rawUser = _db.QueryFirstOrDefault(sql, parameters);

            return (UserBase)rawUser;

            //using (var cn = new SQLiteConnection(cnStr))
            //{

        //    var rawUser = cn.QueryFirstOrDefault<UserBase>(sql, parameters);

        //    if (rawUser != null)
        //    {
        //        return rawUser;
        //    }
        //    else
        //    {
        //        return null;
        //    }

        //}
        }

        public static bool InsertUser(UserBase user)
        {
            var sql =
                @"
                    INSERT INTO User
                    (
                        [Name]
                    )
                    VALUES
                    (
                        @Name
                    );

                ";

            var result = _db.Execute(sql, user);
            return true;

            //using (var cn = new SQLiteConnection(cnStr))
            //{

            //    var result = cn.Execute(sql, user);
            //    return result > 0;
            //}
        }

        public static void REGISTER_USER(string name)
        {
            if (GetUser(name) == null)
            {

                UserBase newUser = new UserBase(name);
                if (InsertUser(newUser))
                {
                    Console.WriteLine("Success");
                }
            }
            else
            {
                Console.WriteLine("Error - user already existing");
            }
        }


    }

