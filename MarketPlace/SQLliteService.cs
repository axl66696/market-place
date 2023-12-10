using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Xml.Linq;

/// <summary>
/// Summary description for Class1
/// </summary>
/// 

public class SQLliteService : IDataBase
    {

		private string _cnStr;
		private SQLiteConnection _cn;
		
		public SQLliteService(string dbPath)
		{
			_cnStr = "data source=" + dbPath;
			_cn = new SQLiteConnection(_cnStr);

        }

		public object Execute(string sql, object payload)
		{
			var result = _cn.Execute(sql, payload);
            return result;

        }

    public List<object> Query(string sql)
		{
			var result = _cn.Query<object>(sql).ToList();

            if (result != null)
            {
                return result;
            }
            else
            {
                return null;
            }
        }
    public List<object> Query(string sql, DynamicParameters parameters)
    {
        var rawListing = _cn.Query<object>(sql, parameters).ToList();
        return rawListing;
    }
    public object QueryFirstOrDefault(string sql, DynamicParameters parameters)
        {
            var result = _cn.QueryFirstOrDefault<object>(sql, parameters);

            if (result != null)
            {
                return result;
            }
            else
            {
                return null;
            }


        }

        public object QueryFirstOrDefault(string sql)
        {
            var result = _cn.QueryFirstOrDefault<object>(sql);
            return result;


    }
}
