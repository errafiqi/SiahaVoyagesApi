using SiahaVoyages.App.Enums;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SiahaVoyages.App.Dtos
{
    public class TransferDto : AuditedEntityDto<Guid>
    {
        public Guid ClientId { get; set; }
        public ClientDto Client { get; set; }

        public string[] PassengersNames { get; set; }

        public string PassengersPhone { get; set; }

        public string FMNO { get; set; }

        public string ChargeCode { get; set; }

        public DateTime PickupDate { get; set; }

        public string PickupPoint { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public string DeliveryPoint { get; set; }

        public string DriverReview { get; set; }

        public float Rate { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public bool IsAirportTransfert { get; set; }

        public string FlightDetails { get; set; }

        public float Price { get; set; }

        public TransferStateEnum State { get; set; }

        public Guid? DriverId { get; set; }
        public DriverDto Driver { get; set; }
    }
}
