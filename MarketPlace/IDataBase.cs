using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Summary description for Class1
/// </summary>


	 interface IDataBase
	{
		object Execute(string sql, object payload);
		List<object> Query(string sql);
		List<object> Query(string sql, DynamicParameters parameters);
		object QueryFirstOrDefault(string sql);
		object QueryFirstOrDefault(string sql, DynamicParameters parameters);

    }
