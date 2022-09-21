using AutoMapper;
using SiahaVoyages.App;
using SiahaVoyages.App.Dtos;

namespace SiahaVoyages;

public class SiahaVoyagesApplicationAutoMapperProfile : Profile
{
    public SiahaVoyagesApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Client, ClientDto>();
        CreateMap<CreateClientDto, Client>();
        CreateMap<UpdateClientDto, Client>();

        CreateMap<Driver, DriverDto>();
        CreateMap<CreateDriverDto, Driver>();
        CreateMap<UpdateDriverDto, Driver>();

        CreateMap<Transfer, TransferDto>()
            .ForMember(dest => dest.PassengersNames, options => options.MapFrom(src => src.PassengersArray));
        CreateMap<CreateTransferDto, Transfer>()
            .ForMember(dest => dest.Passengers, options => options.MapFrom(src => src.PassengersNamesString));
        CreateMap<UpdateTransferDto, Transfer>()
            .ForMember(dest => dest.Passengers, options => options.MapFrom(src => src.PassengersNamesString));

        CreateMap<Voucher, VoucherDto>();
        CreateMap<CreateVoucherDto, Voucher>();
        CreateMap<UpdateVoucherDto, Voucher>();

        CreateMap<Invoice, InvoiceDto>();
        CreateMap<CreateInvoiceDto, Invoice>();
        CreateMap<UpdateInvoiceDto, Invoice>();
    }
}
