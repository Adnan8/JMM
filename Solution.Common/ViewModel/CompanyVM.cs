using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Solution.Common.ViewModel
{
    public class CompanyVM
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Company name is required")]
        [StringLength(100, ErrorMessage = "Company name must not exceed 100 characters")]
        public string? Compname { get; set; }
        [Required(ErrorMessage = "ABN is required")]

        [StringLength(20, ErrorMessage = "ABN must not exceed 20 characters")]
        public string? Abn { get; set; }
        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, ErrorMessage = "Address 1 must not exceed 200 characters")]
        public string? Addr1 { get; set; }

        [StringLength(200, ErrorMessage = "Address 2 must not exceed 200 characters")]
        public string? Addr2 { get; set; }

        [StringLength(50, ErrorMessage = "City must not exceed 50 characters")]
        public string? City { get; set; }

        [StringLength(50, ErrorMessage = "State must not exceed 50 characters")]
        public string? State { get; set; }

        [StringLength(10, ErrorMessage = "Postal code must not exceed 10 characters")]
        public string? Pcode { get; set; }

        [StringLength(50, ErrorMessage = "Country must not exceed 50 characters")]
        public string? Country { get; set; }

        [StringLength(100, ErrorMessage = "Website address must not exceed 100 characters")]
        [Url(ErrorMessage = "Invalid URL")]
        public string? Webaddr { get; set; }

        public int? Invoicedays { get; set; }

        [StringLength(200, ErrorMessage = "Invoice terms must not exceed 200 characters")]
        public string? Invoiceterms { get; set; }

        public string? Logo { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DteCreated { get; set; }

        public DateTime? DteUpdated { get; set; }

        public int? SecModel { get; set; }

        public Guid? CompGuid { get; set; }

        [StringLength(50, ErrorMessage = "URCode must not exceed 50 characters")]
        public string? URCode { get; set; }
        public bool? DefaultPermissions { get; set; }
    }
}
