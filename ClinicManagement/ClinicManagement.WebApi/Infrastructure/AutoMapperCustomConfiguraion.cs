using AutoMapper;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Model;
using ClinicManagement.Model.Request.Custom;

namespace ClinicManagement.WebApi.Infrastructure
{
    public class AutoMapperCustomConfiguraion : Profile
    {
        public AutoMapperCustomConfiguraion()
        {
            CreateMap<TestDemo, TestDemoModel>();
            CreateMap<TestDemoCustomRequest, TestDemo>();
            CreateMap<PersonCustomRequest, Person>();
            CreateMap<PatientCustomRequest, Patient>();
        }
    }
}
