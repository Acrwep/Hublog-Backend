using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
namespace EMP
{
    public class Dapperr 
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Execute(string query)
        {
            GetDbconnection().Execute(query);
        }

        public object ExecuteScalar(string query)
        {
            return GetDbconnection().ExecuteScalar(query);
        }

        public T Get<T>(string query)
        {
            return GetDbconnection().Query<T>(query).FirstOrDefault();
        }

        public List<T> GetAll<T>(string query)
        {
           
            return GetDbconnection().Query<T>(query).ToList();

        }

        SqlConnection sq;

        public IDbConnection GetDbconnection()
        {
            if (sq == null)
            {
                sq = new SqlConnection(ConfigurationManager.ConnectionStrings["EMBContext"].ConnectionString);
            }
            return sq;
        }

     
    }
}