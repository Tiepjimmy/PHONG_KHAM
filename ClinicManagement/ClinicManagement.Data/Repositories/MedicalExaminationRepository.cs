using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;

namespace ClinicManagement.Data.Repositories
{
	public class MedicalExaminationRepository(IUnitOfWork unitOfWork) : Repository<MedicalExamination>(unitOfWork), IMedicalExaminationRepository
    {
	}
}
