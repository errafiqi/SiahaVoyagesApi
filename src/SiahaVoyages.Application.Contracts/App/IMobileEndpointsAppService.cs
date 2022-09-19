﻿using SiahaVoyages.App.Dtos;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SiahaVoyages.App
{
    public interface IMobileEndpointsAppService : IApplicationService
    {
        public Task<TransferDto> RejectMission(Guid MissionId);

        public Task<TransferDto> AccepteMission(Guid MissionId);

        public Task<TransferDto> CompleteMission(Guid MissionId, string DeliveryPoint, string DriverReview, DateTime DeliveryDate);

        public Task<DriverDto> EditProfileInfos(Guid DriverId, string Username, string Name, string Surname, string Email, string PhoneNumber, string ProfilePicture);

        public Task<ListResultDto<TransferDto>> GetAffectedAndOnGoingMissions(Guid DriverId);

        public Task<ListResultDto<TransferDto>> GetCompletedMissionsByDateRange(Guid DriverId, DateTime? From, DateTime? to);

        public Task<ListResultDto<TransferDto>> GetLastFiveMissions(Guid DriverId);

        public Task<MissionsCountsTodayDto> GetMissionsCountsToday(Guid DriverId);

        public Task<ListResultDto<MissionsCountThisWeekPerDayDto>> GetMissionsCountThisWeekPerDay(Guid DriverId);

        public Task<DriverDto> GetCurrentDriver();

        public Task<DriverDto> GetCurrentDriverWithDetails();

        public Task EditPassword(string newPassword);
    }
}
