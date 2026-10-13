using ClinicManagement.Model.ViewModels;

namespace ClinicManagement.Data.Share
{
    public interface IAuthenticatedService
    {
        public Guid Id { get; }
        public UserInfo UserInfo { get; }
    }
}
