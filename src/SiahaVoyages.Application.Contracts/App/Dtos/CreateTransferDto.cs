using SiahaVoyages.App.Enums;
using System;
using System.Collections.Generic;

namespace SiahaVoyages.App.Dtos
{
    public class CreateTransferDto
    {
        public Guid ClientId { get; set; }

        public string[] PassengersNames{ get; set; }

        public string PassengersNamesString
        {
            get 
            {
                if (PassengersNames != null)
                {
                    string names = "";
                    foreach (var name in PassengersNames)
                    {
                        names += name + ";";
                        names = name.Trim(';');
                    }
                    return names;
                }
                return "";
            }
        }

        public string PassengersPhone { get; set; }

        public string FMNO { get; set; }

        public string ChargeCode { get; set; }

        public DateTime PickupDate { get; set; }

        public string PickupPoint { get; set; }

        public float Rate { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public bool IsAirportTransfert { get; set; }

        public string FlightDetails { get; set; }

        public float Price { get; set; }

        public TransferStateEnum State { get; set; }

        public Guid DriverId { get; set; }
    }
}
