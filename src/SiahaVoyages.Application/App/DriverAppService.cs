using SiahaVoyages.App.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Account;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace SiahaVoyages.App
{
    public class DriverAppService : SiahaVoyagesAppService, IDriverAppService
    {
        IRepository<Driver, Guid> _driverRepository;

        IAccountAppService _accountAppService;

        IRepository<IdentityUser, Guid> _userRepository;

        IRepository<IdentityRole, Guid> _roleRepository;

        public DriverAppService(IRepository<Driver, Guid> driverRepository, IAccountAppService accountAppService,
            IRepository<IdentityUser, Guid> userRepository, IRepository<IdentityRole, Guid> roleRepository)
        {
            _driverRepository = driverRepository;
            _accountAppService = accountAppService;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<DriverDto> GetAsync(Guid id)
        {
            var driver = (await _driverRepository.WithDetailsAsync(d => d.User)).FirstOrDefault(d => d.Id == id);
            return ObjectMapper.Map<Driver, DriverDto>(driver);
        }

        public async Task<PagedResultDto<DriverDto>> GetListAsync(GetDriverListDto input)
        {
            var query = await _driverRepository.WithDetailsAsync(d => d.User);

            var drivers = query.WhereIf(!string.IsNullOrEmpty(input.Filter), d => (d.User.Name + d.User.Surname).Contains(input.Filter)
                                    || (d.User.Surname + d.User.Name).Contains(input.Filter)
                                    || d.User.Surname.Contains(input.Filter))
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .OrderBy(d => d.LastModificationTime != null ? d.LastModificationTime : d.CreationTime)
                .ToList();

            var totalCount = drivers.Any() ? drivers.Count() : 0;

            return new PagedResultDto<DriverDto>(
                totalCount,
                ObjectMapper.Map<List<Driver>, List<DriverDto>>(drivers)
            );
        }

        public async Task<DriverDto> CreateAsync(CreateDriverDto input)
        {
            var registerUserDto = new RegisterDto
            {
                UserName = input.UserName,
                EmailAddress = input.Email,
                Password = input.Password,
                AppName = "SiahaVoyages"
            };
            var userDto = await _accountAppService.RegisterAsync(registerUserDto);

            var user = (await _userRepository.WithDetailsAsync()).FirstOrDefault(u => u.Id == userDto.Id);
            var roleId = (await _roleRepository.GetAsync(r => r.Name.Equals("driver"))).Id;
            user.AddRole(roleId);
            user.Name = input.Name ?? "";
            user.Surname = input.Surname ?? "";
            user.SetPhoneNumber(input.PhoneNumber ?? "", true);

            user = await _userRepository.UpdateAsync(user);
            var driver = new Driver
            {
                User = user,
                UserId = user.Id,
                Available = true
            };

            var insertedDriver = await _driverRepository.InsertAsync(driver);

            return ObjectMapper.Map<Driver, DriverDto>(insertedDriver);
        }

        public async Task<DriverDto> UpdateAsync(UpdateDriverDto input)
        {
            var driver = ObjectMapper.Map<UpdateDriverDto, Driver>(input);

            var updatedDriver = await _driverRepository.UpdateAsync(driver);

            return ObjectMapper.Map<Driver, DriverDto>(updatedDriver);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _driverRepository.DeleteAsync(id);
        }
    }
}
