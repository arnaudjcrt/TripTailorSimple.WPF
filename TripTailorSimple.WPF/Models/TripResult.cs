using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripTailorSimple.WPF.Models
{
    public class TripResult
    {
        public string City { get; set; } = "";
        public string Country { get; set; } = "";
        public string Region { get; set; } = "";
        public string Description { get; set; } = "";
        public int EstimatedPrice { get; set; }
        public double AverageTemperature { get; set; }

        public string CityCountry => $"{City}, {Country}";
        public string PriceDisplay => $"{EstimatedPrice} €";
        public string TemperatureDisplay => $"{AverageTemperature} °C";
    }
}
