﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarPool.Models
{
    [CustomValidation(typeof(Request), "ValidateEndDate")]
    public class Request
    {
        public int Id { get; set; }

        public string requestor { get; set; }

        [DisplayName("Accept status")]
        public Boolean isAccepted { get; set; }

        public string acceptor { get; set; }

        [Required]
        [DisplayName("From Address")]
        public string fromAddress { get; set; }

        [Required]
        [DisplayName("From ZipCode")]
        [RegularExpression(@"^(?!00000)[0-9]{5,5}$", ErrorMessage = "Zip Code should be of 5 digits")]
        public string fromZip { get; set; }

        [Required]
        [DisplayName("To Address")]
        public string toAddress { get; set; }

        [Required]
        [DisplayName("To ZipCode")]
        [RegularExpression(@"^(?!00000)[0-9]{5,5}$", ErrorMessage = "Zip Code should be of 5 digits")]
        public string toZip { get; set; }

        [DisplayName("Pool Type - Daily")]
        public Boolean isDaily { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Remote("IsValidStartDate", "Validation", HttpMethod = "POST", ErrorMessage = "Please provide a valid start date.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Pool Start Date")]
        public DateTime startDate { get; set; }


        [DataType(DataType.Date)]
        [Remote("IsValidEndDate", "Validation", HttpMethod = "POST", ErrorMessage = "Please provide a valid end date.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Pool End Date")]
        public DateTime endDate { get; set; }

        [Required]
        [DisplayName("Pool Start Time/24 hr format")]
        [Range(0, 23, ErrorMessage = "Please enter between 0 and 23")]
        public int startTime { get; set; }

        public static ValidationResult ValidateEndDate(Request request, ValidationContext context)
        {
            if (!request.isDaily)
                return ValidationResult.Success;
            if (request.endDate < request.startDate)
                return new ValidationResult("End date cannot be less than start date");
            return ValidationResult.Success;
        }
    }
}