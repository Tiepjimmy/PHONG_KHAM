using ClinicManagement.Model.ViewModels;

namespace ClinicManagement.Model.Response
{
    public class UploadFileResponse
    {
        public bool Result { get; set; }
        public List<InformationFileViewModel>? ListFile { get; set; }
    }
}
