using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ClinicManagement.Data.Interface;
using ClinicManagement.Data.Repositories;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Helper;
using ClinicManagement.Model.Request;
using ClinicManagement.Model.Response;
using ClinicManagement.Model.ViewModels;
using ClinicManagement.Services.Interface;
using Serilog;
using System.Net;
using System.Net.Http.Headers;

namespace ClinicManagement.Services.Implement
{
    public class ImportService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment) : IImportService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly string _webRootPath = Path.Combine(webHostEnvironment.WebRootPath, ConstantHelper.Import);

        public async Task<IResult<bool>> Save(ImportExcelRequest request)
        {
            var filePath = string.Empty;
            try
            {
                if (request.File == null)
                {
                    return new Result<bool>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.InternalServerError),
                        ResponseMessage = ConstantHelper.Message.FileRequired,
                        Data = false
                    };
                }
                var getExtension = Path.GetExtension(ContentDispositionHeaderValue.Parse(request.File.ContentDisposition).FileName?.Trim('"'));
                if (getExtension != ConstantHelper.xlsx && getExtension != ConstantHelper.xls)
                {
                    return new Result<bool>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.InternalServerError),
                        ResponseMessage = ConstantHelper.Message.FileCorrectFormat,
                        Data = false
                    };
                }

                var fileName = request.File.FileName.Replace(getExtension, string.Empty);
                var fileNameHas = string.Concat(request.File.FileName.Replace(getExtension, string.Empty), ConstantHelper.Underlined, MurmurHashHelper.Hash(request.File.FileName + Guid.NewGuid()), getExtension);

                // Path Folder
                if (!Directory.Exists(_webRootPath))
                {
                    Directory.CreateDirectory(_webRootPath);
                }
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), _webRootPath);
                var fullPath = Path.Combine(pathToSave, fileNameHas);

                using (FileStream fileStream = new(fullPath, FileMode.Create))
                {
                    request.File.CopyTo(fileStream);
                }

                filePath = Path.Combine(_webRootPath, fileNameHas);
                var dataTable = DataTableHelper.ConvertExeltoDataTable(fullPath);
                var result = string.Empty;
                switch (request.TableName)
                {
                    case nameof(TestDemo):
                        var testDemo = DataTableHelper.ConvertDataTable<TestDemo>(dataTable);
                        if (testDemo.Count == 0)
                        {
                            return new Result<bool>
                            {
                                ResponseCode = Convert.ToInt32(HttpStatusCode.InternalServerError),
                                ResponseMessage = ConstantHelper.Message.ImportFileNull,
                                Data = false
                            };
                        }
                        await ImportTestDemo(testDemo);
                        break;
                    default:
                        break;
                }
                if (!string.IsNullOrEmpty(result))
                {
                    DeleteFile(filePath);
                    return new Result<bool>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.InternalServerError),
                        ResponseMessage = result,
                        Data = false
                    };
                }
                return new Result<bool>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.ImportSuccess,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                DeleteFile(filePath);
                Log.Error(string.Concat("[", GetType().Name, "]", " Save: ", ex.ToString()));
                throw;
            }
        }

        private async Task ImportTestDemo(List<TestDemo> listData)
        {
            if (listData.Count != 0)
            {
                foreach (var item in listData)
                {
                    _unitOfWork.TestDemoRepository.Add(item);
                }
                await _unitOfWork.TestDemoRepository.CommitAsync(ConstantHelper.TypeSave.Insert);
            }
        }

        private static void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public InformationFileViewModel GetInformationFile(IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                return new();
            }
            else
            {
                var getExtension = Path.GetExtension(ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName?.Trim('"')) ?? string.Empty;
                var fileName = file.FileName.Replace(getExtension, string.Empty);
                var guid = Guid.NewGuid().ToString();
                var has = MurmurHashHelper.Hash(string.Concat(file.FileName, guid).ToString()).ToString();
                var fileNameHas = string.Concat(fileName, ConstantHelper.Underlined, has, getExtension);
                var fileSize = Convert.ToInt32(file?.Length ?? 0);
                return new()
                {
                    FileName = fileName,
                    FileSize = fileSize,
                    FileType = getExtension,
                    FileNameHas = fileNameHas
                };
            }
        }

        public bool UploadFile(List<FileRequest> listFile, string type)
        {
            try
            {
                if (listFile.Count == 0)
                {
                    return false;
                }
                if (!Directory.Exists(_webRootPath))
                {
                    Directory.CreateDirectory(_webRootPath);
                }
                var url = string.Empty;
                switch (type)
                {
                    case ConstantHelper.FolderName.PERSON_IMAGE:
                        url = Path.Combine(_webRootPath, ConstantHelper.FolderName.PERSON_IMAGE);
                        break;
                    case ConstantHelper.FolderName.TEST_DEMO_UPLOAD:
                        url = Path.Combine(_webRootPath, ConstantHelper.FolderName.TEST_DEMO_UPLOAD);
                        break;
                    default:
                        break;
                }
                foreach (var item in listFile)
                {
                    if (string.IsNullOrEmpty(item.FileName) || item.File == null)
                    {
                        return false;
                    }
                    if (!Directory.Exists(url))
                    {
                        Directory.CreateDirectory(url);
                    }
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), url);
                    var fullPath = Path.Combine(pathToSave, item.FileName);
                    using FileStream fileStream = new(fullPath, FileMode.Create);
                    item.File.CopyTo(fileStream);
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " UploadFile: ", ex.ToString()));
                return false;
            }
        }

        public UploadFileResponse UploadFileForInsertUpdate(List<IFormFile> files, string type)
        {
            List<FileRequest> listFileUpload = [];
            List<InformationFileViewModel> listFile = [];
            foreach (var item in files)
            {
                var fileInfo = GetInformationFile(item);
                listFile.Add(new() { FileName = fileInfo.FileName, FileSize = fileInfo.FileSize, FileType = fileInfo.FileType, FileNameHas = fileInfo.FileNameHas });
                listFileUpload.Add(new() { FileName = fileInfo.FileNameHas ?? string.Empty, File = item });
            }
            return new()
            {
                Result = UploadFile(listFileUpload, type),
                ListFile = listFile
            };
        }
    }
}
