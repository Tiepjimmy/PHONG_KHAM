using System.Data;
using System.Data.SqlClient;
using Dapper;
using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Common;
using ClinicManagement.Model.Enum;
using ClinicManagement.Model.Helper;
using Serilog;

namespace ClinicManagement.Data.Repositories
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "<Pending>")]
    public class DapperRepository : IDapperRepository
    {
        public static string connectionString = ConfigurationCommon.Instance.GetConfig<string>(ConstantHelper.ConnectionStrings, ConstantHelper.MainConnectionString);

        public IEnumerable<T> QueryMultipleWithParam<T>(string queryString, object prm)
        {
            try
            {
                using SqlConnection connection = new(connectionString);
                connection.Open();
                using var multi = connection.QueryMultiple(queryString, prm);
                var result = multi.Read<T>();
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " QueryMultipleWithParam: ", ex.ToString()));
                throw new DbExceptionHelper(ex.ToString());
            }
        }

        public IEnumerable<T> QueryMultiple<T>(string queryString, int commandTimeOut = (int)CommandEnum.CommainTimeOut)
        {
            try
            {
                using SqlConnection connection = new(connectionString);
                connection.Open();
                using var multi = connection.QueryMultiple(queryString, commandTimeout: commandTimeOut);
                var result = multi.Read<T>();
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " QueryMultiple: ", ex.ToString()));
                throw new DbExceptionHelper(ex.ToString());
            }

        }

        public async Task<IEnumerable<T>> QueryMultipleUsingStoreProcAsync<T>(string spName, object? parms, int commandTimeOut = (int)CommandEnum.CommainTimeOut)
        {
            try
            {
                using SqlConnection connection = new(connectionString);
                connection.Open();
                using var multi = await connection.QueryMultipleAsync(spName, parms, commandTimeout: commandTimeOut, commandType: CommandType.StoredProcedure);
                var result = multi.Read<T>();
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " QueryMultipleUsingStoreProcAsync: ", ex.ToString()));
                throw new DbExceptionHelper(ex.ToString());
            }
        }

        public List<T> GetBySql<T>(string sql)
        {
            using IDbConnection db = new SqlConnection(connectionString);
            return db.Query<T>(sql).ToList();
        }

        public T QueryFirstOrDefault<T>(string connectionString, string queryString, int commandTimeOut = (int)CommandEnum.CommainTimeOut)
        {
            try
            {
                using SqlConnection connection = new(connectionString);
                return connection.QueryFirstOrDefault<T>(queryString, commandTimeout: commandTimeOut);
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " QueryFirstOrDefault: ", ex.ToString()));
                throw new DbExceptionHelper(ex.ToString());
            }
        }

        public T QueryFirstOrDefault<T>(string queryString, int commandTimeOut = (int)CommandEnum.CommainTimeOut)
        {
            try
            {
                using SqlConnection connection = new(connectionString);
                return connection.QueryFirstOrDefault<T>(queryString, commandTimeout: commandTimeOut);
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " QueryFirstOrDefault: ", ex.ToString()));
                throw new DbExceptionHelper(ex.ToString());
            }
        }

        public T QueryFirstOrDefaultUsingStoreProc<T>(string spName, object parms)
        {
            try
            {
                using SqlConnection connection = new(connectionString);
                return connection.QueryFirstOrDefault<T>(spName, parms, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " QueryFirstOrDefaultUsingStoreProc: ", ex.ToString()));
                throw new DbExceptionHelper(ex.ToString());
            }
        }

        public T ExecuteScalar<T>(string queryString, int commandTimeOut = (int)CommandEnum.CommainTimeOut)
        {
            try
            {
                using SqlConnection connection = new(connectionString);
                return connection.ExecuteScalar<T>(queryString, commandTimeout: commandTimeOut);
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " ExecuteScalar: ", ex.ToString()));
                throw new DbExceptionHelper(ex.ToString());
            }
        }

        public T ExecuteScalar<T>(string connectionString, string queryString, int commandTimeOut = (int)CommandEnum.CommainTimeOut)
        {
            try
            {
                using SqlConnection connection = new(connectionString);
                return connection.ExecuteScalar<T>(queryString, commandTimeout: commandTimeOut);
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " ExecuteScalar: ", ex.ToString()));
                throw new DbExceptionHelper(ex.ToString());
            }
        }

        public T ExecuteScalarUsingStoreProc<T>(string spName, object parms, int commandTimeOut = (int)CommandEnum.CommainTimeOut)
        {
            try
            {
                using SqlConnection connection = new(connectionString);
                return connection.ExecuteScalar<T>(spName, parms, commandTimeout: commandTimeOut, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " ExecuteScalarUsingStoreProc: ", ex.ToString()));
                throw new DbExceptionHelper(ex.ToString());
            }
        }

        public T InsertOrUpdateUsingStoreProc<T>(string spName, object parms, int commandTimeOut = (int)CommandEnum.CommainTimeOut)
        {
            try
            {
                using SqlConnection connection = new(connectionString);
                connection.Open();
#pragma warning disable CS8603 // Possible null reference return.
                return connection.Query<T>(spName, parms, commandTimeout: commandTimeOut, commandType: CommandType.StoredProcedure).FirstOrDefault();
#pragma warning restore CS8603 // Possible null reference return.
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " InsertOrUpdateUsingStoreProc: ", ex.ToString()));
                throw new DbExceptionHelper(ex.ToString());
            }
        }

        public async Task UpdateMultiple(string connectionString, string executeString, List<dynamic> listDataUpDate, int commandTimeOut = (int)CommandEnum.CommainTimeOut)
        {
            try
            {
                using SqlConnection connection = new(connectionString);
                await connection.ExecuteAsync(executeString, listDataUpDate, commandTimeout: commandTimeOut);
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " UpdateMultiple: ", ex.ToString()));
                throw new DbExceptionHelper(ex.ToString());
            }
        }

        public DataTable ExecuteToDataTable(string queryString, int commandTimeOut = 30)
        {
            try
            {
                using SqlConnection connection = new(connectionString);
                var reader = connection.ExecuteReader(queryString, commandTimeout: commandTimeOut);
                DataTable dataTable = new();
                dataTable.Load(reader);
                return dataTable;
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " ExecuteToDataTable: ", ex.ToString()));
                throw new DbExceptionHelper(ex.ToString());
            }
        }

        public DataTable ExecuteToDataTable(string connectionString, string queryString, int commandTimeOut = 30)
        {
            try
            {
                using SqlConnection connection = new(connectionString);
                var reader = connection.ExecuteReader(queryString, commandTimeout: commandTimeOut);
                DataTable dataTable = new();
                dataTable.Load(reader);
                return dataTable;
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " ExecuteToDataTable: ", ex.ToString()));
                throw new DbExceptionHelper(ex.ToString());
            }
        }

        public async Task DataTableBulkInsert(string connectionString, DataTable dataTable, int bulkCopyTimeOut = (int)SqlBulkCopyEnum.TimeOut, int batchSize = (int)SqlBulkCopyEnum.BatchSize, bool isKeepIdentity = false)
        {
            try
            {
                using SqlBulkCopy sqlBulkCopy = new(connectionString, isKeepIdentity == true ? SqlBulkCopyOptions.KeepIdentity : SqlBulkCopyOptions.Default);
                sqlBulkCopy.DestinationTableName = dataTable.TableName;
                sqlBulkCopy.BulkCopyTimeout = bulkCopyTimeOut;
                sqlBulkCopy.BatchSize = batchSize;
                MapColumns(dataTable, sqlBulkCopy);
                await sqlBulkCopy.WriteToServerAsync(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " DataTableBulkInsert: ", ex.ToString()));
                throw new DbExceptionHelper(ex.ToString());
            }
        }

        public async Task DataReaderBulkInsert(string connectionString, IDataReader dataReader, int bulkCopyTimeOut = (int)SqlBulkCopyEnum.TimeOut, int batchSize = (int)SqlBulkCopyEnum.BatchSize)
        {
            try
            {
                using SqlConnection connection = new(connectionString);
                await connection.OpenAsync();
                using SqlBulkCopy sqlBulkCopy = new(connection);
                sqlBulkCopy.DestinationTableName = dataReader?.GetSchemaTable()?.TableName;
                sqlBulkCopy.BulkCopyTimeout = bulkCopyTimeOut;
                sqlBulkCopy.BatchSize = batchSize;
                await sqlBulkCopy.WriteToServerAsync(dataReader);
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " DataReaderBulkInsert: ", ex.ToString()));
                throw new DbExceptionHelper(ex.ToString());
            }
        }

        public async Task UpdateUsingQuery(string executeString, object parms)
        {
            try
            {
                using SqlConnection connection = new(connectionString);
                await connection.ExecuteAsync(executeString, parms, commandTimeout: (int)CommandEnum.CommainTimeOut).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " UpdateUsingQuery: ", ex.ToString()));
                throw new DbExceptionHelper(ex.ToString());
            }
        }

        private static SqlBulkCopy MapColumns(DataTable infoTable, SqlBulkCopy bulkCopy)
        {
            foreach (DataColumn dc in infoTable.Columns)
            {
                bulkCopy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
            }
            return bulkCopy;
        }
    }
}
