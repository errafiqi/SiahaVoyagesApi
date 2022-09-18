using Nito.AsyncEx;
using SiahaVoyages.App.Dtos;
using SiahaVoyages.App.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Account;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace SiahaVoyages.App
{
    public class MobileEndpointsAppService : ApplicationService, IMobileEndpointsAppService
    {
        IRepository<Driver, Guid> _driverRepository;

        IRepository<Transfer, Guid> _transferRepository;

        ICurrentUser _currentUser;

        IRepository<IdentityUser, Guid> _userRepository;

        IdentityUserManager UserManager { get; }

        public MobileEndpointsAppService(IRepository<Driver, Guid> driverRepository,
            IRepository<Transfer, Guid> transferRepository,
            ICurrentUser currentUser,
            IRepository<IdentityUser, Guid> userRepository)
        {
            _driverRepository = driverRepository;
            _transferRepository = transferRepository;
            _currentUser = currentUser;
            _userRepository = userRepository;
        }

        public async Task<TransferDto> RejectMission(Guid MissionId)
        {
            var mission = (await _transferRepository.WithDetailsAsync(t => t.Driver, t => t.Client, t => t.Client.User))
                .FirstOrDefault(t => t.Id == MissionId);

            mission.State = TransferStateEnum.Rejected;
            mission.Driver.Available = true;
            mission = await _transferRepository.UpdateAsync(mission);

            return ObjectMapper.Map<Transfer, TransferDto>(mission);
        }

        public async Task<TransferDto> AccepteMission(Guid MissionId)
        {
            var mission = (await _transferRepository.WithDetailsAsync(t => t.Driver, t => t.Client, t => t.Client.User))
                .FirstOrDefault(t => t.Id == MissionId);

            mission.State = TransferStateEnum.OnGoing;
            mission.Driver.Available = false;
            mission = await _transferRepository.UpdateAsync(mission);

            return ObjectMapper.Map<Transfer, TransferDto>(mission);
        }

        public async Task<TransferDto> CompleteMission(Guid MissionId)
        {
            var mission = (await _transferRepository.WithDetailsAsync(t => t.Driver, t => t.Client, t => t.Client.User))
                .FirstOrDefault(t => t.Id == MissionId);

            mission.State = TransferStateEnum.Closed;
            mission.Driver.Available = true;
            mission = await _transferRepository.UpdateAsync(mission);

            return ObjectMapper.Map<Transfer, TransferDto>(mission);
        }

        public async Task<DriverDto> EditProfileInfos(UpdateDriverDto DriverInfos)
        {
            var driver = ObjectMapper.Map<UpdateDriverDto, Driver>(DriverInfos);
            driver = await _driverRepository.UpdateAsync(driver);

            return ObjectMapper.Map<Driver, DriverDto>(driver);
        }

        public async Task<ListResultDto<TransferDto>> GetAffectedAndOnGoingMissions(Guid DriverId)
        {
            var missions = (await _transferRepository.WithDetailsAsync(t => t.Driver, t => t.Client, t => t.Client.User))
                .Where(t => t.DriverId == DriverId && (t.State == TransferStateEnum.Affected || t.State == TransferStateEnum.OnGoing))
                .OrderBy(t => t.PickupDate)
                .OrderBy(t => t.LastModificationTime != null ? t.LastModificationTime : t.CreationTime)
                .ToList();

            var result = ObjectMapper.Map<List<Transfer>, List<TransferDto>>(missions);

            return new ListResultDto<TransferDto>(result);
        }

        public async Task<ListResultDto<TransferDto>> GetCompletedMissionsByDateRange(Guid DriverId, DateTime From, DateTime to)
        {
            var missions = (await _transferRepository.WithDetailsAsync(t => t.Driver, t => t.Driver.User, t => t.Client, t => t.Client.User))
                .Where(t => t.DriverId == DriverId && t.State == TransferStateEnum.Closed)
                .Where(t => t.PickupDate.CompareTo(From) >= 0 && t.PickupDate.CompareTo(to) <= 0)
                .OrderBy(t => t.LastModificationTime)
                .ToList();

            var result = ObjectMapper.Map<List<Transfer>, List<TransferDto>>(missions);

            return new ListResultDto<TransferDto>(result);
        }

        public async Task<ListResultDto<TransferDto>> GetLastFiveMissions(Guid DriverId)
        {
            var missions = (await _transferRepository.WithDetailsAsync(t => t.Driver, t => t.Driver.User, t => t.Client, t => t.Client.User))
                .Where(t => t.DriverId == DriverId &&
                                (t.State == TransferStateEnum.Affected
                                    || t.State == TransferStateEnum.OnGoing
                                    || t.State == TransferStateEnum.Closed
                                )
                       )
                .OrderBy(t => t.LastModificationTime != null ? t.LastModificationTime : t.CreationTime)
                .Take(5)
                .ToList();

            var result = ObjectMapper.Map<List<Transfer>, List<TransferDto>>(missions);

            return new ListResultDto<TransferDto>(result);
        }

        public async Task<MissionsCountsTodayDto> GetMissionsCountsToday(Guid DriverId)
        {
            var missionsCountsTodayDto = new MissionsCountsTodayDto();
            var today = DateTime.Now.Date;

            var query = (await _transferRepository.WithDetailsAsync()).Where(t => t.DriverId == DriverId && today.Equals(t.PickupDate));

            missionsCountsTodayDto.AffectedMissionsCount = query.Count(t => t.State == TransferStateEnum.Affected);
            missionsCountsTodayDto.CompletedMissionsCount = query.Count(t => t.State == TransferStateEnum.Affected);
            missionsCountsTodayDto.InProgressMissionsCount = query.Count(t => t.State == TransferStateEnum.Affected);

            return missionsCountsTodayDto;
        }

        public Task<MissionsCountThisWeekPerDayDto> GetMissionsCountThisWeekPerDay(Guid DriverId)
        {
            throw new NotImplementedException();
        }

        public async Task<DriverDto> GetCurrentDriverWithDetails()
        {
            var userId = _currentUser.Id;

            var driver = (await _driverRepository.WithDetailsAsync(d => d.User)).FirstOrDefault(d => d.UserId == userId);

            return ObjectMapper.Map<Driver, DriverDto>(driver);
        }

        public async Task<DriverDto> GetCurrentDriver()
        {
            var userId = _currentUser.Id;

            var driver = await _driverRepository.GetAsync(d => d.UserId == userId);
            driver.User = await _userRepository.GetAsync(u => u.Id == userId);

            return ObjectMapper.Map<Driver, DriverDto>(driver);
        }

        public async Task EditPassword(string newPassword)
        {
            var userId = _currentUser.Id.Value;
            var user = (await _userRepository.WithDetailsAsync()).FirstOrDefault(u => u.Id == userId);

            var resetToken = await UserManager.GeneratePasswordResetTokenAsync(user);

            await UserManager.ResetPasswordAsync(user, resetToken, newPassword);
        }
    }
}
