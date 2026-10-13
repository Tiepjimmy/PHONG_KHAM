using AutoMapper;
using ClinicManagement.Data.Interface;
using ClinicManagement.Data.Repositories;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Helper;
using ClinicManagement.Model.Request;
using ClinicManagement.Model.Response;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels;
using ClinicManagement.Services.Interface;
using Dapper;
using GemBox.Document;
using Serilog;
using System.Data;
using System.Net;

namespace ClinicManagement.Services.Implement
{
    public class MedicineWarehouseService(IUnitOfWork unitOfWork, IDapperRepository dapperRepository, IMapper mapper
        , IAuditChangeService auditChangeService) : IMedicineWarehouseService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IDapperRepository _dapperRepository = dapperRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IAuditChangeService _auditChangeService = auditChangeService;

        public async Task<IResults<MedicineWarehouseViewModel>> GetAll(MedicineWarehouseSearchRequest request)
        {
            try
            {
                DynamicParameters dynamicParameters = new();
                dynamicParameters.Add("@SearchText", request.SearchText);
                dynamicParameters.Add("@SupplierId", request.SupplierId);
                dynamicParameters.Add("@Quantity", request.Quantity);
                dynamicParameters.Add("@IsMain", request.IsMain ? ConstantHelper.One : ConstantHelper.Zero);
                dynamicParameters.Add(ConstantHelper.Page, request.Page);
                dynamicParameters.Add(ConstantHelper.PageSize, request.PageSize);
                dynamicParameters.Add(ConstantHelper.SortField, request.SortField);
                dynamicParameters.Add(ConstantHelper.SortOrder, request.SortOrder);
                var list = (await _dapperRepository.QueryMultipleUsingStoreProcAsync<MedicineWarehouseViewModel>("uspMedicineWarehouse_GetAll", dynamicParameters)).ToList();
                if (list.Count != 0)
                {
                    int i = 0;
                    list.ForEach(x =>
                    {
                        i++;
                        x.STT = (request.Page - 1) * request.PageSize + i;
                    });
                }
                return new Results<MedicineWarehouseViewModel>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                    ListData = list,
                    TotalRecords = list.FirstOrDefault()?.TotalRow ?? 0
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetAll: ", ex.ToString()));
                throw;
            }
        }

        public IResults<MedicineWarehouse> GetAllData()
        {
            try
            {
                var list = _unitOfWork.MedicineWarehouseRepository.GetAll().Where(x => !x.IsDeleted).ToList();
                return new Results<MedicineWarehouse>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                    ListData = list,
                    TotalRecords = list.Count
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetAllData: ", ex.ToString()));
                throw;
            }
        }

        public async Task<IResult<bool>> Save(MedicineWarehouseRequest request, string typeSave)
        {
            using var tran = await _unitOfWork.OpenTransaction();
            try
            {
                var result = string.Empty;
                switch (typeSave)
                {
                    case ConstantHelper.TypeSave.Insert:
                        result = await Insert(request, typeSave);
                        break;
                    case ConstantHelper.TypeSave.Update:
                        result = await Update(request, typeSave);
                        break;
                    case ConstantHelper.TypeSave.Delete:
                        result = await Delete(request.Id, typeSave);
                        break;
                    default:
                        break;
                }
                _unitOfWork.CommitTransaction();
                if (!string.IsNullOrEmpty(result))
                {
                    return new Result<bool>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.BadRequest),
                        ResponseMessage = result,
                        Data = false
                    };
                }
                return new Result<bool>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = typeSave == ConstantHelper.TypeSave.Delete ? ConstantHelper.Message.DeleteSuccess : ConstantHelper.Message.SaveSuccess,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " Save: ", ex.ToString()));
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        private async Task<string> Insert(MedicineWarehouseRequest request, string typeSave)
        {
            var insert = _mapper.Map<MedicineWarehouse>(request);
            _unitOfWork.MedicineWarehouseRepository.Add(insert);
            await _unitOfWork.MedicineWarehouseRepository.CommitAsync(typeSave);
            return string.Empty;
        }

        private async Task<string> Update(MedicineWarehouseRequest request, string typeSave)
        {
            var exist = _unitOfWork.MedicineWarehouseRepository.GetSingle(x => x.Id == request.Id && !x.IsDeleted);
            if (string.IsNullOrEmpty(request.Id.ToString()) || exist == null)
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            var update = _mapper.Map(request, exist);
            _unitOfWork.MedicineWarehouseRepository.Update(update);
            await _unitOfWork.MedicineWarehouseRepository.CommitAsync(typeSave);
            return string.Empty;
        }

        private async Task<string> Delete(Guid? id, string typeSave)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            var checkForeignKey = (await _auditChangeService.GetColumnNameForeignKey()).ListData.Where(x => (x.ColumnName ?? string.Empty).Contains(nameof(MedicineWarehouse))).ToList();
            if (checkForeignKey.Count != 0)
            {
                foreach (var item in checkForeignKey)
                {
                    var strSql = string.Concat("SELECT COUNT(*) FROM ", item.TableName, " where ", item.ColumnName, " = '", id, "' AND IsDeleted = 0");
                    DynamicParameters dynamicParameters = new();
                    dynamicParameters.Add("@StrSql", strSql);
                    var checkExist = _dapperRepository.QueryFirstOrDefaultUsingStoreProc<long>("uspSystem_ExecuteQuery", dynamicParameters);
                    if (checkExist > 0)
                    {
                        return string.Format(ConstantHelper.Message.DeleteExistFail);
                    }
                }
            }
            var delete = _unitOfWork.MedicineWarehouseRepository.GetSingle(x => x.Id == id && !x.IsDeleted);
            if (delete == null)
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            _unitOfWork.MedicineWarehouseRepository.Update(delete);
            await _unitOfWork.MedicineWarehouseRepository.CommitAsync(typeSave);
            return string.Empty;
        }

        public async Task<IResult<MedicineWarehouse>> GetById(Guid id)
        {
            try
            {
                var data = await _unitOfWork.MedicineWarehouseRepository.GetSingleAsync(x => x.Id == id && !x.IsDeleted);
                if (data == null)
                {
                    return new Result<MedicineWarehouse>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.BadRequest),
                        ResponseMessage = ConstantHelper.Message.RecordNotFoundMessage,
                        Data = null,
                    };
                }
                return new Result<MedicineWarehouse>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                    Data = data,
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetById: ", ex.ToString()));
                throw;
            }
        }

        public async Task<IResults<MedicineWarehouseViewModel>> GetExpiry(MedicineWarehouseSearchRequest request)
        {
            try
            {
                DynamicParameters dynamicParameters = new();
                dynamicParameters.Add("@Expiry", request.Expiry);
                dynamicParameters.Add("@IsMain", request.IsMain ? ConstantHelper.One : ConstantHelper.Zero);
                dynamicParameters.Add(ConstantHelper.Page, request.Page);
                dynamicParameters.Add(ConstantHelper.PageSize, request.PageSize);
                dynamicParameters.Add(ConstantHelper.SortField, request.SortField);
                dynamicParameters.Add(ConstantHelper.SortOrder, request.SortOrder);
                var list = (await _dapperRepository.QueryMultipleUsingStoreProcAsync<MedicineWarehouseViewModel>("uspMedicineWarehouse_GetExpiry", dynamicParameters)).ToList();
                if (list.Count != 0)
                {
                    int i = 0;
                    list.ForEach(x =>
                    {
                        i++;
                        x.STT = (request.Page - 1) * request.PageSize + i;
                    });
                }
                return new Results<MedicineWarehouseViewModel>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                    ListData = list,
                    TotalRecords = list.FirstOrDefault()?.TotalRow ?? 0
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetExpiry: ", ex.ToString()));
                throw;
            }
        }

        public async Task<ExportToExcelResponse> DownloadExpiry(MedicineWarehouseSearchRequest request, DocumentModel document)
        {
            request.Page = 1;
            request.PageSize = 1000000000;
            var listData = (await GetExpiry(request)).ListData;

            var dataSource = new { Date = DateTime.Now.ToString(ConstantHelper.FormatDate_ddMMyyyy), ExpiryDate = request.Expiry };
            document.MailMerge.Execute(dataSource);

            //data for TableExpiry
            var table = new DataTable() { TableName = "TableExpiry" };
            table.Columns.Add("Stt", typeof(string));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("UnitMeasureName", typeof(string));
            table.Columns.Add("Quantity", typeof(string));
            table.Columns.Add("Price", typeof(string));
            table.Columns.Add("Expiry", typeof(string));
            foreach (var item in listData)
            {
                table.Rows.Add(item.STT, item.Name, item.UnitMeasureName, item.Quantity, string.Concat(item.Price.ToString("N0"), " vnđ"), item.Expiry?.ToString(ConstantHelper.FormatDate_ddMMyyyy));
            }
            document.MailMerge.Execute(table);
            using MemoryStream stream = new();
            document.Save(stream, SaveOptions.DocxDefault);
            byte[] result = stream.ToArray();
            return new ExportToExcelResponse
            {
                ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                ResponseMessage = ConstantHelper.Message.Successfully,
                FileContent = result,
                FileName = string.Concat("DownloadExpiry_", request.Expiry, ConstantHelper.Underlined, DateTime.Now.ToString(ConstantHelper.FormatDate_ddMMyyyyhmmss), ConstantHelper.docx)
            };
        }
    }
}
