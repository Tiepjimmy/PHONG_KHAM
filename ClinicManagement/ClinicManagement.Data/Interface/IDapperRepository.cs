using System.Data;
using ClinicManagement.Model.Enum;

namespace ClinicManagement.Data.Interface
{
    public interface IDapperRepository
    {
        public IEnumerable<T> QueryMultipleWithParam<T>(string queryString, object prm);
        public IEnumerable<T> QueryMultiple<T>(string queryString, int commandTimeOut = (int)CommandEnum.CommainTimeOut);
        public Task<IEnumerable<T>> QueryMultipleUsingStoreProcAsync<T>(string spName, object? parms, int commandTimeOut = (int)CommandEnum.CommainTimeOut);
        public List<T> GetBySql<T>(string sql);
        public T QueryFirstOrDefault<T>(string connectionString, string queryString, int commandTimeOut = (int)CommandEnum.CommainTimeOut);
        public T QueryFirstOrDefault<T>(string queryString, int commandTimeOut = (int)CommandEnum.CommainTimeOut);
        public T QueryFirstOrDefaultUsingStoreProc<T>(string spName, object parms);
        public T ExecuteScalar<T>(string queryString, int commandTimeOut = (int)CommandEnum.CommainTimeOut);
        public T ExecuteScalar<T>(string connectionString, string queryString, int commandTimeOut = (int)CommandEnum.CommainTimeOut);
        public T ExecuteScalarUsingStoreProc<T>(string spName, object parms, int commandTimeOut = (int)CommandEnum.CommainTimeOut);
        public T InsertOrUpdateUsingStoreProc<T>(string spName, object parms, int commandTimeOut = (int)CommandEnum.CommainTimeOut);
        public Task UpdateMultiple(string connectionString, string executeString, List<dynamic> listDataUpDate, int commandTimeOut = (int)CommandEnum.CommainTimeOut);
        public DataTable ExecuteToDataTable(string queryString, int commandTimeOut = 30);
        public DataTable ExecuteToDataTable(string connectionString, string queryString, int commandTimeOut = 30);
        public Task DataTableBulkInsert(string connectionString, DataTable dataTable, int bulkCopyTimeOut = (int)SqlBulkCopyEnum.TimeOut, int batchSize = (int)SqlBulkCopyEnum.BatchSize, bool isKeepIdentity = false);
        public Task DataReaderBulkInsert(string connectionString, IDataReader dataReader, int bulkCopyTimeOut = (int)SqlBulkCopyEnum.TimeOut, int batchSize = (int)SqlBulkCopyEnum.BatchSize);
        public Task UpdateUsingQuery(string executeString, object parms);
    }
}
