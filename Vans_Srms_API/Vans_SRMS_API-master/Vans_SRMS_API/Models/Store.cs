using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vans_SRMS_API.Models
{
    public class Store
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StoreId { get; set; }
        public string StoreNumber { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Phone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Fax { get; set; }
        public string DistrictNumber { get; set; }
        public string DistrictManager { get; set; }
        public string Region { get; set; }
        public string RegionalDirector { get; set; }
        public bool IsDefault { get; set; }
        public DateTime LastUpdate { get; set; }

        public List<Location> Locations { get; set; }
    }
}
