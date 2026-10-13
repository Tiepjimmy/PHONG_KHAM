Print("----------ClinicManagement.Data----------")
copy ..\ClinicManagement.Generator\ClinicManagement.Data.Connection\*.cs ..\ClinicManagement.Data\Connection
copy ..\ClinicManagement.Generator\ClinicManagement.Data.Interface\*.cs ..\ClinicManagement.Data\Interface
copy ..\ClinicManagement.Generator\ClinicManagement.Data.Repositories\*.cs ..\ClinicManagement.Data\Repositories
copy ..\ClinicManagement.Generator\ClinicManagement.Data.Interface.UnitOfWork\*.cs ..\ClinicManagement.Data\Interface
copy ..\ClinicManagement.Generator\ClinicManagement.Data.Repositories.UnitOfWork\*.cs ..\ClinicManagement.Data\Repositories


Print("----------ClinicManagement.Model----------")
copy ..\ClinicManagement.Generator\ClinicManagement.Model.Entity\*.cs ..\ClinicManagement.Model\Entity
copy ..\ClinicManagement.Generator\ClinicManagement.Model.Request\*.cs ..\ClinicManagement.Model\Request
for /r %%F in ("..\ClinicManagement.Generator\ClinicManagement.Model.SearchRequest\*.cs") do if not exist "..\ClinicManagement.Model\SearchRequest\%%~nxF" copy %%F ..\ClinicManagement.Model\SearchRequest
copy ..\ClinicManagement.Generator\ClinicManagement.Model.ViewModels\*.cs ..\ClinicManagement.Model\ViewModels


Print("----------ClinicManagement.Services----------")
for /r %%F in ("..\ClinicManagement.Generator\ClinicManagement.Services.Interface\*.cs") do if not exist "..\ClinicManagement.Services\Interface\%%~nxF" copy %%F ..\ClinicManagement.Services\Interface
for /r %%F in ("..\ClinicManagement.Generator\ClinicManagement.Services.Implement\*.cs") do if not exist "..\ClinicManagement.Services\Implement\%%~nxF" copy %%F ..\ClinicManagement.Services\Implement

Print("----------ClinicManagement.WebApi----------")
for /r %%F in ("..\ClinicManagement.Generator\ClinicManagement.WebApi.Controllers\*.cs") do if not exist "..\ClinicManagement.WebApi\Controllers\%%~nxF" copy %%F ..\ClinicManagement.WebApi\Controllers
copy ..\ClinicManagement.Generator\ClinicManagement.WebApi.Infrastructure\*.cs ..\ClinicManagement.WebApi\Infrastructure


Print("----------Delete All File Copy----------")
del "..\ClinicManagement.Data\Connection\DomainGenerator.cs"
del "..\ClinicManagement.Data\Interface\DomainGenerator.cs"
del "..\ClinicManagement.Data\Repositories\DomainGenerator.cs"
del "..\ClinicManagement.Model\Entity\DomainGenerator.cs"
del "..\ClinicManagement.Model\Request\DomainGenerator.cs"
del "..\ClinicManagement.Model\SearchRequest\DomainGenerator.cs"
del "..\ClinicManagement.Model\ViewModels\DomainGenerator.cs"
del "..\ClinicManagement.Services\Implement\DomainGenerator.cs"
del "..\ClinicManagement.Services\Interface\DomainGenerator.cs"
del "..\ClinicManagement.WebApi\Controllers\DomainGenerator.cs"
del "..\ClinicManagement.WebApi\Infrastructure\DomainGenerator.cs"
del "..\ClinicManagement.Generator\ClinicManagement.Data.Connection\*.cs"
del "..\ClinicManagement.Generator\ClinicManagement.Data.Interface\*.cs"
del "..\ClinicManagement.Generator\ClinicManagement.Data.Interface.UnitOfWork\*.cs"
del "..\ClinicManagement.Generator\ClinicManagement.Data.Repositories\*.cs"
del "..\ClinicManagement.Generator\ClinicManagement.Data.Repositories.UnitOfWork\*.cs"
del "..\ClinicManagement.Generator\ClinicManagement.Model.Entity\*.cs"
del "..\ClinicManagement.Generator\ClinicManagement.Model.Request\*.cs"
del "..\ClinicManagement.Generator\ClinicManagement.Model.SearchRequest\*.cs"
del "..\ClinicManagement.Generator\ClinicManagement.Model.ViewModels\*.cs"
del "..\ClinicManagement.Generator\ClinicManagement.Services.Implement\*.cs"
del "..\ClinicManagement.Generator\ClinicManagement.Services.Interface\*.cs"
del "..\ClinicManagement.Generator\ClinicManagement.WebApi.Controllers\*.cs"
del "..\ClinicManagement.Generator\ClinicManagement.WebApi.Infrastructure\*.cs"