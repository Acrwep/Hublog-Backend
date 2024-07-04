using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace EMP
{
    public class Dapperr : IDisposable
    {
        private SqlConnection sq;

        public void Dispose()
        {
            if (sq != null)
            {
                sq.Dispose();
                sq = null;
            }
        }

        public int Execute(string query, object parameters = null)
        {
            return GetDbconnection().Execute(query, parameters);
        }

        public object ExecuteScalar(string query)
        {
            return GetDbconnection().ExecuteScalar(query);
        }

        public T Get<T>(string query)
        {
            return GetDbconnection().Query<T>(query).FirstOrDefault();
        }

        public List<T> GetAll<T>(string query, object parameters = null)
        {
            return GetDbconnection().Query<T>(query, parameters).ToList();
        }

        public async Task<int> ExecuteAsync(string query, object parameters = null)
        {
            return await GetDbconnection().ExecuteAsync(query, parameters);
        }

        public async Task<int> ExecuteAsync(string query, object parameters, CommandType commandType)
        {
            return await GetDbconnection().ExecuteAsync(query, parameters, commandType: commandType);
        }

        public async Task<T> GetAsync<T>(string query, object parameters = null, CommandType commandType = CommandType.Text)
        {
            var result = await GetDbconnection().QueryAsync<T>(query, parameters, commandType: commandType);
            return result.FirstOrDefault();
        }

        private IDbConnection GetDbconnection()
        {
            if (sq == null)
            {
                sq = new SqlConnection(ConfigurationManager.ConnectionStrings["EMBContext"].ConnectionString);
            }
            return sq;
        }
    }
}
