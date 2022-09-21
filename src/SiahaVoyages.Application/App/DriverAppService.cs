using SiahaVoyages.App.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Account;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.Users;

namespace SiahaVoyages.App
{
    public class DriverAppService : SiahaVoyagesAppService, IDriverAppService
    {
        IRepository<Driver, Guid> _driverRepository;

        IAccountAppService _accountAppService;

        IRepository<IdentityUser, Guid> _userRepository;

        ICurrentUser _currentUser;

        IdentityUserManager UserManager { get; }

        IRepository<IdentityRole, Guid> _roleRepository;

        public DriverAppService(IRepository<Driver, Guid> driverRepository, IAccountAppService accountAppService,
            IRepository<IdentityUser, Guid> userRepository, IRepository<IdentityRole, Guid> roleRepository,
            IdentityUserManager userManager, ICurrentUser currentUser)
        {
            _driverRepository = driverRepository;
            _accountAppService = accountAppService;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            UserManager = userManager;
            _currentUser = currentUser;
        }

        public async Task<DriverDto> GetAsync(Guid id)
        {
            var driver = (await _driverRepository.WithDetailsAsync(d => d.User)).FirstOrDefault(d => d.Id == id);
            return ObjectMapper.Map<Driver, DriverDto>(driver);
        }

        public async Task<ListResultDto<DriverDto>> GetAvailablesAsync()
        {
            var drivers = (await _driverRepository.WithDetailsAsync(d => d.User))
                .OrderByDescending(d => d.Available)
                .ToList();

            return new ListResultDto<DriverDto>(
                ObjectMapper.Map<List<Driver>, List<DriverDto>>(drivers)
            );
        }

        public async Task<PagedResultDto<DriverDto>> GetListAsync(GetDriverListDto input)
        {
            var query = await _driverRepository.WithDetailsAsync(d => d.User);

            var drivers = query.WhereIf(!string.IsNullOrEmpty(input.Filter), d => (d.User.Name + d.User.Surname).Contains(input.Filter)
                                    || (d.User.Surname + d.User.Name).Contains(input.Filter)
                                    || d.User.Surname.Contains(input.Filter))
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .OrderByDescending(d => d.LastModificationTime != null ? d.LastModificationTime : d.CreationTime)
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

        public async Task<DriverDto> UpdateAsync(Guid DriverId, UpdateDriverDto input)
        {
            var driver = (await _driverRepository.WithDetailsAsync(d => d.User))
                .FirstOrDefault(d => d.Id == DriverId);

            driver.User.Name = input.Name;
            driver.User.Surname = input.Surname;
            driver.User.SetPhoneNumber(input.PhoneNumber ?? "", true);
            driver.ProfilePicture = input.ProfilePicture;
            driver = await _driverRepository.UpdateAsync(driver);

            var user = await _userRepository.GetAsync(u => u.Id == driver.UserId);

            var changeEmailToken = await UserManager.GenerateChangeEmailTokenAsync(user, input.Email);

            await UserManager.ChangeEmailAsync(user, input.Email, changeEmailToken);
            await UserManager.ConfirmEmailAsync(user, changeEmailToken);
            await UserManager.SetEmailAsync(user, input.Email);

            await UserManager.SetUserNameAsync(user, input.UserName);
            if (!string.IsNullOrEmpty(input.Password))
            {
                await EditPassword(DriverId, input.Password);
            }

            return ObjectMapper.Map<Driver, DriverDto>(driver);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _driverRepository.DeleteAsync(id);
        }

        public async Task<DriverDto> EditPassword(Guid driverId, string newPassword)
        {
            var userId = _currentUser.Id.Value;
            var user = (await _userRepository.WithDetailsAsync()).FirstOrDefault(u => u.Id == userId);

            var resetToken = await UserManager.GeneratePasswordResetTokenAsync(user);

            await UserManager.ResetPasswordAsync(user, resetToken, newPassword);

            var driver = await _driverRepository.GetAsync(d => d.Id == driverId);

            return ObjectMapper.Map<Driver, DriverDto>(driver);
        }
    }
}
