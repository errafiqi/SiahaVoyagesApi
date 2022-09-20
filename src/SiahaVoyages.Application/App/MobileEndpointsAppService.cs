﻿using Nito.AsyncEx;
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
using Volo.Abp.ObjectMapping;
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
            IRepository<IdentityUser, Guid> userRepository,
            IdentityUserManager userManager)
        {
            _driverRepository = driverRepository;
            _transferRepository = transferRepository;
            _currentUser = currentUser;
            _userRepository = userRepository;
            UserManager = userManager;
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

        public async Task<TransferDto> CompleteMission(Guid MissionId, string DeliveryPoint, string DriverReview, DateTime DeliveryDate)
        {
            var mission = (await _transferRepository.WithDetailsAsync(t => t.Driver, t => t.Client, t => t.Client.User))
                .FirstOrDefault(t => t.Id == MissionId);

            mission.State = TransferStateEnum.Closed;
            mission.DeliveryDate = DeliveryDate;
            mission.DeliveryPoint = DeliveryPoint;
            mission.DriverReview = DriverReview;
            mission.Driver.Available = true;
            mission = await _transferRepository.UpdateAsync(mission);

            return ObjectMapper.Map<Transfer, TransferDto>(mission);
        }

        public async Task<DriverDto> EditProfileInfos(EditProfileInfosDto infos)
        {
            var driver = await _driverRepository.GetAsync(d => d.Id == infos.DriverId);

            driver = (await _driverRepository.WithDetailsAsync(d => d.User))
                .FirstOrDefault(d => d.Id == infos.DriverId);

            driver.User.Name = infos.Name;
            driver.User.Surname = infos.Surname;
            driver.User.SetPhoneNumber(infos.PhoneNumber ?? "", true);
            driver.ProfilePicture = infos.ProfilePicture;
            driver = await _driverRepository.UpdateAsync(driver);

            var user = await _userRepository.GetAsync(u => u.Id == driver.UserId);

            var changeEmailToken = await UserManager.GenerateChangeEmailTokenAsync(user, infos.Email);

            await UserManager.ChangeEmailAsync(user, infos.Email, changeEmailToken);
            await UserManager.ConfirmEmailAsync(user, changeEmailToken);
            await UserManager.SetEmailAsync(user, infos.Email);

            await UserManager.SetUserNameAsync(user, infos.Username);

            return ObjectMapper.Map<Driver, DriverDto>(driver);
        }

        public async Task<ListResultDto<TransferDto>> GetAffectedAndOnGoingMissions(Guid DriverId)
        {
            var missions = (await _transferRepository.WithDetailsAsync(t => t.Driver, t => t.Driver.User, t => t.Client, t => t.Client.User))
                .Where(t => t.DriverId == DriverId && (t.State == TransferStateEnum.Affected
                                                       || t.State == TransferStateEnum.OnGoing
                                                       || t.State == TransferStateEnum.Closed))
                .OrderByDescending(t => t.PickupDate)
                .OrderByDescending(t => t.LastModificationTime != null ? t.LastModificationTime : t.CreationTime)
                .ToList();

            var result = ObjectMapper.Map<List<Transfer>, List<TransferDto>>(missions);

            return new ListResultDto<TransferDto>(result);
        }

        public async Task<ListResultDto<TransferDto>> GetCompletedMissionsByDateRange(Guid DriverId, DateTime? From, DateTime? to)
        {
            var missions = (await _transferRepository.WithDetailsAsync(t => t.Driver, t => t.Driver.User, t => t.Client, t => t.Client.User))
                .Where(t => t.DriverId == DriverId && t.State != TransferStateEnum.Requested)
                .WhereIf(From != null, t => t.PickupDate.Date.CompareTo(From.Value.Date) >= 0)
                .WhereIf(to != null, t => t.PickupDate.Date.CompareTo(to.Value.Date) <= 0)
                .OrderByDescending(t => t.PickupDate)
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
                .OrderByDescending(t => t.LastModificationTime != null ? t.LastModificationTime : t.CreationTime)
                .Take(5)
                .ToList();

            var result = ObjectMapper.Map<List<Transfer>, List<TransferDto>>(missions);

            return new ListResultDto<TransferDto>(result);
        }

        public async Task<MissionsCountsTodayDto> GetMissionsCountsToday(Guid DriverId)
        {
            var missionsCountsTodayDto = new MissionsCountsTodayDto();
            var today = DateTime.Now.Date;

            var query = (await _transferRepository.WithDetailsAsync()).Where(t => t.DriverId == DriverId && today.Equals(t.PickupDate.Date));

            missionsCountsTodayDto.AffectedMissionsCount = query.Count(t => t.State == TransferStateEnum.Affected);
            missionsCountsTodayDto.CompletedMissionsCount = query.Count(t => t.State == TransferStateEnum.Closed);
            missionsCountsTodayDto.InProgressMissionsCount = query.Count(t => t.State == TransferStateEnum.OnGoing);

            return missionsCountsTodayDto;
        }

        public async Task<ListResultDto<MissionsCountThisWeekPerDayDto>> GetMissionsCountThisWeekPerDay(Guid DriverId)
        {
            var today = DateTime.Now.Date;
            var list = (await _transferRepository.WithDetailsAsync(t => t.Driver, t => t.Driver.User, t => t.Client, t => t.Client.User))
                .Where(t => t.DriverId == DriverId &&
                                (t.State == TransferStateEnum.Affected
                                    || t.State == TransferStateEnum.OnGoing
                                    || t.State == TransferStateEnum.Closed
                                )
                       )
                .Where(t => t.PickupDate.Date.CompareTo(today) <= 0 && t.PickupDate.Date.CompareTo(today) > -7).ToList();
            var groupedList = list.GroupBy(t => t.PickupDate.DayOfWeek)
                .Select(t => new MissionsCountThisWeekPerDayDto { DayOfWeek = t.Key, Count = t.Count() });
            var missions = groupedList.ToList();

            return new ListResultDto<MissionsCountThisWeekPerDayDto>(missions);
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

        public async Task<DriverDto> EditDriverPassword(Guid driverId, string newPassword)
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
