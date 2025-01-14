﻿namespace Kaalcharakk.Dtos.AdressDtos
{
    public class ViewAddressDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string? LandMark { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
    }
}
