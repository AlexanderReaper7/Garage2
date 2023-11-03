﻿using System.ComponentModel.DataAnnotations;

namespace Garage2.Models.ViewModels
{
    public class CheckOutVehicleViewModel
    {
        public int Id { get; set; }
        //[Remote()]
        [Display(Name = "Registration Number")]
        public string RegistrationNumber { get; set; }
        [Display(Name = "Arrival Time")]
        public DateTime ArrivalTime { get; set; }
        [Display(Name ="Check Out Time")]
        public DateTime CheckOutTime { get; set; }
		[DisplayFormat(DataFormatString = "{0:N2}")]
		public decimal Price { get; set; }
        [Display(Name ="Total Time")]
        //{0} represents the value of the property, and %d is used to display the days part of the TimeSpan.
        [DisplayFormat(DataFormatString = "{0:%d} days")]
        public TimeSpan TotalTime { get; set; }
    }
}
