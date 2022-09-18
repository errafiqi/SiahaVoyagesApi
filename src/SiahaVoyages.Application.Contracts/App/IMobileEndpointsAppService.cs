using SiahaVoyages.App.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SiahaVoyages.App
{
    public interface IMobileEndpointsAppService : IApplicationService
    {
        public Task<TransferDto> RejectMission(Guid MissionId);

        public Task<TransferDto> AccepteMission(Guid MissionId);

        public Task<TransferDto> CompleteMission(Guid MissionId);

        public Task<DriverDto> EditProfileInfos(UpdateDriverDto DriverInfos);

        public Task<ListResultDto<TransferDto>> GetAffectedAndOnGoingMissions(Guid DriverId);

        public Task<ListResultDto<TransferDto>> GetCompletedMissionsByDateRange(Guid DriverId, DateTime From, DateTime to);

        public Task<ListResultDto<TransferDto>> GetLastFiveMissions(Guid DriverId);

        public Task<MissionsCountsTodayDto> GetMissionsCountsToday(Guid DriverId);

        public Task<ListResultDto<MissionsCountThisWeekPerDayDto>> GetMissionsCountThisWeekPerDay(Guid DriverId);

        public Task<DriverDto> GetCurrentDriver();

        public Task<DriverDto> GetCurrentDriverWithDetails();

        public Task EditPassword(string newPassword);
    }
}
