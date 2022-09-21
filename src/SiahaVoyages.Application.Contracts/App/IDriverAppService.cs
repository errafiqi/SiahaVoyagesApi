﻿using SiahaVoyages.App.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SiahaVoyages.App
{
    public interface IDriverAppService : IApplicationService
    {
        Task<DriverDto> GetAsync(Guid id);

        Task<ListResultDto<DriverDto>> GetAvailablesAsync();

        Task<PagedResultDto<DriverDto>> GetListAsync(GetDriverListDto input);

        Task<DriverDto> CreateAsync(CreateDriverDto input);

        Task<DriverDto> UpdateAsync(Guid DriverId, UpdateDriverDto input);

        Task DeleteAsync(Guid id);
    }
}
