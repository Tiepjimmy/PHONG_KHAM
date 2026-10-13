using Microsoft.Extensions.Options;
using ClinicManagement.Data.Interface;
using ClinicManagement.Data.Repositories;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Helper;
using ClinicManagement.Model.Model;
using ClinicManagement.Services.Interface;
using Serilog;
using System.Net;

namespace ClinicManagement.Services.Implement
{
    public class DocumentFileService(IUnitOfWork unitOfWork, IOptions<AppSettingHelper> options) : IDocumentFileService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly AppSettingHelper _appSettingHelper = options.Value;

        public async Task<IResult<bool>> Delete(Guid id)
        {
            using var tran = await _unitOfWork.OpenTransaction();
            try
            {
                var result = string.Empty;
                if (string.IsNullOrEmpty(id.ToString()))
                {
                    result = ConstantHelper.Message.RecordNotFoundMessage;
                }
                var delete = _unitOfWork.DocumentFileRepository.GetSingle(x => x.Id == id && !x.IsDeleted);
                if (delete == null)
                {
                    result = ConstantHelper.Message.RecordNotFoundMessage;
                }
                if (!string.IsNullOrEmpty(result))
                {
                    _unitOfWork.CommitTransaction();
                    return new Result<bool>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.BadRequest),
                        ResponseMessage = result,
                        Data = false
                    };
                }
                if (delete != null)
                {
                    _unitOfWork.DocumentFileRepository.Update(delete);
                    await _unitOfWork.DocumentFileRepository.CommitAsync(ConstantHelper.TypeSave.Delete);
                }
                _unitOfWork.CommitTransaction();
                return new Result<bool>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.DeleteSuccess,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " Delete: ", ex.ToString()));
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public IResults<DocumentFile> GetAllData()
        {
            try
            {
                var list = _unitOfWork.DocumentFileRepository.GetAll().Where(x => !x.IsDeleted).ToList();
                return new Results<DocumentFile>
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

        public async Task<IResult<bool>> BulkInsert(List<DocumentFile> listDocumentFile)
        {
            using var tran = await _unitOfWork.OpenTransaction();
            try
            {
                if (listDocumentFile.Count != 0)
                {
                    foreach (var item in listDocumentFile)
                    {
                        _unitOfWork.DocumentFileRepository.Add(item);
                    }
                    await _unitOfWork.DocumentFileRepository.CommitAsync(ConstantHelper.TypeSave.Insert);
                }
                _unitOfWork.CommitTransaction();
                return new Result<bool>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.TypeSave.Delete,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " BulkInsert: ", ex.ToString()));
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task<IResult<bool>> BulkUpdate(List<DocumentFile> listDocumentFile)
        {
            using var tran = await _unitOfWork.OpenTransaction();
            try
            {
                if (listDocumentFile.Count != 0)
                {
                    foreach (var item in listDocumentFile)
                    {
                        _unitOfWork.DocumentFileRepository.Update(item);
                    }
                    await _unitOfWork.DocumentFileRepository.CommitAsync(ConstantHelper.TypeSave.Update);
                }
                _unitOfWork.CommitTransaction();
                return new Result<bool>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.TypeSave.Delete,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " BulkUpdate: ", ex.ToString()));
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task<IResult<bool>> DeleteFileById(Guid? id, string tableName)
        {
            using var tran = await _unitOfWork.OpenTransaction();
            try
            {
                var list = GetAllData().ListData;
                var folderName = string.Empty;
                switch (tableName)
                {
                    case nameof(TestDemo):
                        list = list.Where(x => x.TestDemoId == id).ToList();
                        break;
                    default:
                        break;
                }
                if (list.Count != 0)
                {
                    foreach (var item in list)
                    {
                        _unitOfWork.DocumentFileRepository.Update(item);
                    }
                    await _unitOfWork.DocumentFileRepository.CommitAsync(ConstantHelper.TypeSave.Delete);
                }
                _unitOfWork.CommitTransaction();
                return new Result<bool>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.TypeSave.Delete,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " DeleteFileById: ", ex.ToString()));
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public IResults<DocumentFileModel> GetAllFileById(Guid id, string tableName)
        {
            try
            {
                var list = GetAllData().ListData.OrderByDescending(x => x.CreatedDate).ToList();
                var folderName = string.Empty;
                switch (tableName)
                {
                    case nameof(TestDemo):
                        folderName = ConstantHelper.FolderName.TEST_DEMO_UPLOAD;
                        list = list.Where(x => x.TestDemoId == id).ToList();
                        break;
                    default:
                        break;
                }
                var listData = list.Select(x => new DocumentFileModel
                {
                    Id = x.Id,
                    TestDemoId = x.TestDemoId,
                    FileName = x.FileName,
                    FileSize = x.FileSize,
                    FileType = x.FileType,
                    FileNameHas = x.FileNameHas,
                    FileNameUrl = !string.IsNullOrEmpty(x.FileNameHas) ? Path.Combine(_appSettingHelper.DomainFile ?? string.Empty, ConstantHelper.Import, folderName, x.FileNameHas) : string.Empty,
                }).ToList();
                return new Results<DocumentFileModel>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                    ListData = listData,
                    TotalRecords = listData.Count
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetAllFileById: ", ex.ToString()));
                throw;
            }
        }
    }
}
