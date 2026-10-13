using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ClinicManagement.Data.Interface;
using ClinicManagement.Model.Entity;
using ClinicManagement.Model.Helper;
using ClinicManagement.Model.Option;
using ClinicManagement.Model.Request;
using ClinicManagement.Model.Response;
using ClinicManagement.Services.Interface;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace ClinicManagement.Services.Implement
{
    public class LoginService(ITokenFactoryService tokenFactory, IJwtFactoryService jwtFactory, IOptions<JwtIssuerOptions> jwtOptions
        , IUnitOfWork unitOfWork, IOptions<AppSettingHelper> options, IUserRoleService userRoleService) : ILoginService
    {
        private readonly ITokenFactoryService _tokenFactory = tokenFactory;
        private readonly IJwtFactoryService _jwtFactory = jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions = jwtOptions.Value;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly AppSettingHelper _appSettingHelper = options.Value;
        private readonly IUserRoleService _userRoleService = userRoleService;

        public async Task<Response<AuthenticateResponse>> Authenticate(LoginRequest request)
        {
            var pass = EncriptFunctionsHelper.GeneratePassword(request.PassWord ?? string.Empty);
            List<Claim> claims = [];
            request.UserName = request.UserName?.ToLower();
            var user = await _unitOfWork.UserRepository.GetSingleAsync(x => x.UserName == request.UserName && x.PassWord == pass);
            if (user == null)
            {
                return new Response<AuthenticateResponse>
                {
                    Code = Convert.ToInt32(HttpStatusCode.BadRequest),
                    Message = ConstantHelper.Message.UserInActiveMessage
                };
            }
            if (user.IsDeleted)
            {
                return new Response<AuthenticateResponse>
                {
                    Code = Convert.ToInt32(HttpStatusCode.InternalServerError),
                    Message = ConstantHelper.Message.AccountDeleted
                };
            }
            if (!user.IsActive)
            {
                return new Response<AuthenticateResponse>
                {
                    Code = Convert.ToInt32(HttpStatusCode.InternalServerError),
                    Message = ConstantHelper.Message.AccountInActive
                };
            }
            var id = user.Id;
            var userName = user.UserName ?? string.Empty;
            var person = await _unitOfWork.PersonRepository.GetSingleAsync(x => x.Id == user.PersonId);
            var fullName = person.FullName ?? string.Empty;
            var avatar = !string.IsNullOrEmpty(person.Avatar) ? Path.Combine(_appSettingHelper.DomainFile ?? string.Empty, ConstantHelper.Import, ConstantHelper.FolderName.PERSON_IMAGE, person.Avatar) : string.Empty;
            List<string> listUserGroupId = [.. _unitOfWork.UserClaimRepository.GetAll().Where(x => x.UserId == id && !x.IsDeleted).Select(x => x.UserGroupId.ToString())];
            var lstUserGroupId = listUserGroupId.Count != 0 ? string.Join(ConstantHelper.Comma, listUserGroupId) : string.Empty;
            List<string> listUserGroupName = [.. _unitOfWork.UserGroupRepository.GetAll().Where(x => listUserGroupId.Contains(x.Id.ToString()) && !x.IsDeleted).Select(x => x.Name)];
            var userGroupName = listUserGroupName.Count != 0 ? string.Join(ConstantHelper.CommaSpace, listUserGroupName) : string.Empty;
            var pcName = Dns.GetHostName();
            var iPv4 = ConstantHelper.NotFound;
            var isAdmin = user.IsAdmin;
            using (HttpClient client = new())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HttpResponseMessage responseV4 = await client.GetAsync("http://ipv4.iplocation.net");
                HttpResponseMessage responseV4 = await client.GetAsync("https://api.iplocation.net/v2/get-ip");
                if (responseV4.IsSuccessStatusCode)
                {
                    var getIpV4 = await responseV4.Content.ReadAsStringAsync();
                    var resultIpV4 = JsonConvert.DeserializeObject<IpLocationResponse>(getIpV4);
                    if (resultIpV4?.response_code == 200)
                    {
                        iPv4 = resultIpV4.ip;
                    }
                }
            }
            var listMenu = (await _userRoleService.GetByUserGroupId(lstUserGroupId, false)).Data?.ListMenu;
            ClaimsIdentity identity = await GetClaimsIdentity(request);
            claims.Add(new Claim(ClaimTypes.Role, userName));
            claims.Add(new Claim(ClaimTypes.Role, fullName));
            claims.Add(new Claim(ClaimTypes.Role, avatar));
            claims.Add(new Claim(ClaimTypes.Role, lstUserGroupId));
            claims.Add(new Claim(ClaimTypes.Role, userGroupName));
            claims.Add(new Claim(ClaimTypes.Role, pcName));
            claims.Add(new Claim(ClaimTypes.Role, iPv4 ?? ConstantHelper.NotFound));
            claims.Add(new Claim(ClaimTypes.Role, isAdmin.ToString()));
            claims.Add(new Claim(ClaimTypes.Role, string.Join(ConstantHelper.Comma, listMenu?.Select(x => x.ObjectMenuName).ToList() ?? [])));
            var jwt = await _tokenFactory.GenerateJwt(identity,
                                                      _jwtFactory,
                                                      _jwtOptions,
                                                      new JsonSerializerSettings { Formatting = Formatting.Indented },
                                                      id,
                                                      userName,
                                                      fullName,
                                                      avatar,
                                                      userGroupName,
                                                      isAdmin,
                                                      listMenu ?? [],
                                                      claims);
            var jwtObject = JsonConvert.DeserializeObject<AuthenticateResponse>(jwt);
            LocationTracking insertLocationTracking = new()
            {
                IPv4 = iPv4,
                PcName = pcName,
                CreatedBy = id,
                CreatedDate = DateTime.Now
            };
            _unitOfWork.LocationTrackingRepository.Add(insertLocationTracking);
            _unitOfWork.LocationTrackingRepository.Commit();
            return new Response<AuthenticateResponse>
            {
                Code = Convert.ToInt32(HttpStatusCode.OK),
                Message = ConstantHelper.Message.Successfully,
                Data = jwtObject
            };
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.PassWord))
            {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                return await Task.FromResult<ClaimsIdentity>(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            }

            // get the user to verifty
            var userToVerify = _unitOfWork.UserRepository.FindBy(x => x.UserName == request.UserName && x.IsActive == true && !x.IsDeleted).ToList();

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            if (userToVerify.Count == 0)
            {
                return await Task.FromResult<ClaimsIdentity>(null);
            }
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            // check the credentials
            if (EncriptFunctionsHelper.GeneratePassword(request.PassWord) == userToVerify.FirstOrDefault()?.PassWord)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(request.UserName, userToVerify.FirstOrDefault().Id));
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }

            // Credentials are invalid, or account doesn't exist
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            return await Task.FromResult<ClaimsIdentity>(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }
    }
}
