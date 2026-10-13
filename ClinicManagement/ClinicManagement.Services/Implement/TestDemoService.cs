using AutoMapper;
using Dapper;
using GemBox.Document;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using ClinicManagement.Data.Interface;
using ClinicManagement.Data.Repositories;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Enum;
using ClinicManagement.Model.Helper;
using ClinicManagement.Model.Model;
using ClinicManagement.Model.Request.Custom;
using ClinicManagement.Model.Response;
using ClinicManagement.Model.SearchRequest;
using ClinicManagement.Model.ViewModels;
using ClinicManagement.Services.Interface;
using Serilog;
using System.Data;
using System.Net;

namespace ClinicManagement.Services.Implement
{
    public class TestDemoService(IUnitOfWork unitOfWork, IDapperRepository dapperRepository, IMapper mapper
        , IOptions<AppSettingHelper> options, IImportService importService, IDocumentFileService documentFileService
        , IAuditChangeService auditChangeService) : ITestDemoService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IDapperRepository _dapperRepository = dapperRepository;
        private readonly IMapper _mapper = mapper;
        private readonly AppSettingHelper _appSettingHelper = options.Value;
        private readonly IImportService _importService = importService;
        private readonly IDocumentFileService _documentFileService = documentFileService;
        private readonly IAuditChangeService _auditChangeService = auditChangeService;

        public async Task<IResults<TestDemoViewModel>> GetAll(TestDemoSearchRequest request)
        {
            try
            {
                DynamicParameters dynamicParameters = new();
                dynamicParameters.Add("@SearchText", request.SearchText);
                dynamicParameters.Add(ConstantHelper.Page, request.Page);
                dynamicParameters.Add(ConstantHelper.PageSize, request.PageSize);
                dynamicParameters.Add(ConstantHelper.SortField, request.SortField);
                dynamicParameters.Add(ConstantHelper.SortOrder, request.SortOrder);
                var list = (await _dapperRepository.QueryMultipleUsingStoreProcAsync<TestDemoViewModel>("uspTestDemo_GetAll", dynamicParameters)).ToList();
                if (list.Count != 0)
                {
                    int i = 0;
                    list.ForEach(x =>
                    {
                        i++;
                        x.STT = (request.Page - 1) * request.PageSize + i;
                    });
                }
                return new Results<TestDemoViewModel>
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

        public IResults<TestDemo> GetAllData()
        {
            try
            {
                var list = _unitOfWork.TestDemoRepository.GetAll().Where(x => !x.IsDeleted).ToList();
                return new Results<TestDemo>
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

        public async Task<IResult<bool>> Save(TestDemoCustomRequest request, string typeSave)
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

        private async Task<string> Insert(TestDemoCustomRequest request, string typeSave)
        {
            var checkDuplicate = _unitOfWork.TestDemoRepository.GetSingle(x => x.FieldString == request.FieldString && !x.IsDeleted);
            if (checkDuplicate != null)
            {
                return string.Format(ConstantHelper.Message.NameExist, request.FieldString);
            }
            var insert = _mapper.Map<TestDemo>(request);
            if (request.File != null)
            {
                List<IFormFile> listFile =
                [
                    request.File
                ];
                var uploadFileResponse = _importService.UploadFileForInsertUpdate(listFile, ConstantHelper.FolderName.TEST_DEMO_UPLOAD);
                if (!uploadFileResponse.Result)
                {
                    return ConstantHelper.Message.UploadFileFail;
                }
                insert.FieldImageSingle = uploadFileResponse.ListFile?.FirstOrDefault()?.FileNameHas;
            }
            _unitOfWork.TestDemoRepository.Add(insert);
            await _unitOfWork.TestDemoRepository.CommitAsync(typeSave);
            //return string.Empty;
            return await SaveListDocumentFile(request.Files, insert.Id);
        }

        private async Task<string> Update(TestDemoCustomRequest request, string typeSave)
        {
            var exist = _unitOfWork.TestDemoRepository.GetSingle(x => x.Id == request.Id && !x.IsDeleted);
            if (string.IsNullOrEmpty(request.Id.ToString()) || exist == null)
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            var checkDuplicate = _unitOfWork.TestDemoRepository.GetSingle(x => x.Id != request.Id && x.FieldString == request.FieldString && !x.IsDeleted);
            if (checkDuplicate != null)
            {
                return string.Format(ConstantHelper.Message.NameExist, request.FieldString);
            }
            var update = _mapper.Map(request, exist);
            if (request.File != null)
            {
                List<IFormFile> listFile =
                [
                    request.File
                ];
                var uploadFileResponse = _importService.UploadFileForInsertUpdate(listFile, ConstantHelper.FolderName.TEST_DEMO_UPLOAD);
                if (uploadFileResponse.Result)
                {
                    if (uploadFileResponse.ListFile != null)
                    {
                        update.FieldImageSingle = uploadFileResponse.ListFile?.FirstOrDefault()?.FileNameHas;
                    }
                }
                else
                {
                    return ConstantHelper.Message.UploadFileFail;
                }
            }
            _unitOfWork.TestDemoRepository.Update(update);
            await _unitOfWork.TestDemoRepository.CommitAsync(typeSave);
            //return string.Empty;
            return await SaveListDocumentFile(request.Files, update.Id);
        }

        private async Task<string> Delete(Guid? id, string typeSave)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            var checkForeignKey = (await _auditChangeService.GetColumnNameForeignKey()).ListData.Where(x => (x.ColumnName ?? string.Empty).Contains(nameof(TestDemo))).ToList();
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
            var delete = _unitOfWork.TestDemoRepository.GetSingle(x => x.Id == id && !x.IsDeleted);
            if (delete == null)
            {
                return ConstantHelper.Message.RecordNotFoundMessage;
            }
            _unitOfWork.TestDemoRepository.Update(delete);
            await _unitOfWork.TestDemoRepository.CommitAsync(typeSave);
            await _documentFileService.DeleteFileById(id, nameof(TestDemo));
            return string.Empty;
        }

        public async Task<IResult<TestDemoModel>> GetById(Guid id)
        {
            try
            {
                var data = await _unitOfWork.TestDemoRepository.GetSingleAsync(x => x.Id == id && !x.IsDeleted);
                if (data == null)
                {
                    return new Result<TestDemoModel>
                    {
                        ResponseCode = Convert.ToInt32(HttpStatusCode.BadRequest),
                        ResponseMessage = ConstantHelper.Message.RecordNotFoundMessage,
                        Data = null,
                    };
                }
                data.FieldImageSingle = !string.IsNullOrEmpty(data.FieldImageSingle) ? Path.Combine(_appSettingHelper.DomainFile ?? string.Empty, ConstantHelper.Import, ConstantHelper.FolderName.TEST_DEMO_UPLOAD, data.FieldImageSingle) : string.Empty;
                var mapData = _mapper.Map<TestDemoModel>(data);
                mapData.ListDocumentFile = [.. _documentFileService.GetAllFileById(data.Id, nameof(TestDemo)).ListData];
                return new Result<TestDemoModel>
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.GetAllSuccessfully,
                    Data = mapData,
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " GetById: ", ex.ToString()));
                throw;
            }
        }

        private async Task<string> SaveListDocumentFile(List<IFormFile>? files, Guid id)
        {
            if (files != null)
            {
                var uploadFileResponse = _importService.UploadFileForInsertUpdate(files, ConstantHelper.FolderName.TEST_DEMO_UPLOAD);
                if (!uploadFileResponse.Result)
                {
                    return ConstantHelper.Message.UploadFileFail;
                }
                if (uploadFileResponse.ListFile != null)
                {
                    List<DocumentFile> listDocumentFile = [];
                    foreach (var item in uploadFileResponse.ListFile)
                    {
                        DocumentFile documentFile = new()
                        {
                            Id = Guid.NewGuid(),
                            TestDemoId = id,
                            FileName = item.FileName,
                            FileSize = item.FileSize,
                            FileType = item.FileType,
                            FileNameHas = item.FileNameHas,
                        };
                        listDocumentFile.Add(documentFile);
                    }
                    await _documentFileService.BulkInsert(listDocumentFile);
                }
            }
            return string.Empty;
        }

        public async Task<ExportToExcelResponse> ExportToExcel(TestDemoSearchRequest request)
        {
            try
            {
                request.Page = 1;
                request.PageSize = 1000000000;
                var listData = (await GetAll(request)).ListData;
                using ExcelPackage excelPackage = new();
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                List<ExportToExcelModel> listExportToExcelModel =
                [
                    new ExportToExcelModel() { ColunmName = ColunmNameExcelEnum.A.ToString(), ColunmTitle = "STT" },
                    new ExportToExcelModel() { ColunmName = ColunmNameExcelEnum.B.ToString(), ColunmTitle = "String" },
                    new ExportToExcelModel() { ColunmName = ColunmNameExcelEnum.C.ToString(), ColunmTitle = "DateTime" },
                    new ExportToExcelModel() { ColunmName = ColunmNameExcelEnum.D.ToString(), ColunmTitle = "Dropdown" },
                    new ExportToExcelModel() { ColunmName = ColunmNameExcelEnum.E.ToString(), ColunmTitle = "Number" },
                    new ExportToExcelModel() { ColunmName = ColunmNameExcelEnum.F.ToString(), ColunmTitle = "TextArea" },
                    new ExportToExcelModel() { ColunmName = ColunmNameExcelEnum.G.ToString(), ColunmTitle = "Image" },
                    new ExportToExcelModel() { ColunmName = ColunmNameExcelEnum.H.ToString(), ColunmTitle = "CheckBox" },
                    new ExportToExcelModel() { ColunmName = ColunmNameExcelEnum.I.ToString(), ColunmTitle = "Switch" }
                ];

                //DrawContent
                int row = 1;
                for (var i = 0; i < listData.Count; i++)
                {
                    var data = listData[i];
                    //index
                    row++;

                    //Set value content
                    worksheet.Cells[string.Concat(ColunmNameExcelEnum.A.ToString(), row)].Value = data.STT.ToString();
                    worksheet.Cells[string.Concat(ColunmNameExcelEnum.B.ToString(), row)].Value = data.FieldString;
                    worksheet.Cells[string.Concat(ColunmNameExcelEnum.C.ToString(), row)].Value = data.FieldDateTime?.ToString(ConstantHelper.FormatDate_ddMMyyyy);
                    worksheet.Cells[string.Concat(ColunmNameExcelEnum.D.ToString(), row)].Value = data.FieldDropdownName;
                    worksheet.Cells[string.Concat(ColunmNameExcelEnum.E.ToString(), row)].Value = data.FieldNumber;
                    worksheet.Cells[string.Concat(ColunmNameExcelEnum.F.ToString(), row)].Value = data.FieldTextArea;
                    worksheet.Cells[string.Concat(ColunmNameExcelEnum.G.ToString(), row)].Value = data.FieldImageSingle;
                    worksheet.Cells[string.Concat(ColunmNameExcelEnum.H.ToString(), row)].Value = data.FieldCheckBox;
                    worksheet.Cells[string.Concat(ColunmNameExcelEnum.I.ToString(), row)].Value = data.FieldSwitch;
                }

                //DrawTemplate
                var result = ExportToExcelHelper.DrawTemplate(excelPackage, worksheet, listExportToExcelModel, row);

                var fileName = string.Concat("chuc-nang-demo-", DateTime.Now.ToString(ConstantHelper.FormatDate_ddMMyyyyhmmss), ConstantHelper.xlsx);
                return new ExportToExcelResponse
                {
                    ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                    ResponseMessage = ConstantHelper.Message.Successfully,
                    FileContent = result,
                    FileName = fileName,
                    ContentType = ConstantHelper.DownloadExcel
                };
            }
            catch (Exception ex)
            {
                Log.Error(string.Concat("[", GetType().Name, "]", " ExportToExcel: ", ex.ToString()));
                throw;
            }
        }

        public ExportToExcelResponse DownloadToWord(DocumentModel document)
        {
            //data reven
            var table = new DataTable() { TableName = "Table" };
            table.Columns.Add("Colunm1", typeof(string));
            table.Columns.Add("Colunm2", typeof(string));
            table.Columns.Add("Colunm3", typeof(string));
            table.Columns.Add("Colunm4", typeof(string));
            table.Rows.Add("Colunm1", "Colunm1", "Colunm1", "Colunm1");
            table.Rows.Add("Colunm2", "Colunm2", "Colunm2", "Colunm2");
            table.Rows.Add("Colunm3", "Colunm3", "Colunm3", "Colunm3");
            table.Rows.Add("Colunm4", "Colunm4", "Colunm4", "Colunm4");
            document.MailMerge.FieldMerging += (sender, e) =>
            {
                if (e.IsValueFound && e.Value != null)
                {
                    switch (e.FieldName)
                    {
                        //case "QrCode":
                        //    var imagePath = e.Value.ToString();
                        //    e.Inline = new Picture(e.Document, imagePath, 78, 78);
                        //    break;
                        default:
                            break;
                    }
                }
            };
            document.MailMerge.Execute(table);
            using MemoryStream stream = new();
            document.Save(stream, SaveOptions.DocxDefault);
            byte[] result = stream.ToArray();
            return new ExportToExcelResponse
            {
                ResponseCode = Convert.ToInt32(HttpStatusCode.OK),
                ResponseMessage = ConstantHelper.Message.Successfully,
                FileContent = result,
                FileName = string.Concat("DownloadToWord_", DateTime.Now.ToString(ConstantHelper.FormatDate_ddMMyyyyhmmss), ConstantHelper.docx)
            };
        }
    }
}
