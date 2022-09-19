using SiahaVoyages.App.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace SiahaVoyages.App
{
    [Table(SiahaVoyagesConsts.DbTablePrefix + "Transfert")]
    public class Transfer : AuditedAggregateRoot<Guid>
    {
        [ForeignKey("ClientId")]
        public Guid ClientId { get; set; }
        public Client Client { get; set; }

        public string Passengers { get; set; }

        [NotMapped]
        public string[] PassengersArray
        { 
            get 
            {
                return Passengers.Split(";");
            } 
        }

        public string PassengersPhone { get; set; }

        public string FMNO { get; set; }

        public string ChargeCode { get; set; }

        public DateTime PickupDate { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public bool IsAirportTransfert { get; set; }

        public string FlightDetails { get; set; }

        public string PickupPoint { get; set; }

        public string DeliveryPoint { get; set; }

        public string DriverReview { get; set; }

        public float Rate { get; set; }

        public float Price { get; set; }

        public TransferStateEnum State { get; set; } = TransferStateEnum.Requested;

        [ForeignKey("DriverId")]
        public Guid DriverId { get; set; }
        public Driver Driver { get; set; }
    }
}
