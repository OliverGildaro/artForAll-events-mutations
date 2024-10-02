using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtForAll.Events.Presentation.DTOs.Events
{
    public class AddressRequest
    {
        public string City { get;  set; }
        public string Country { get; set; }
        public string Number { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }

        public void Deconstruct(out string city, out string country, out string street, out string number, out string zipCode)
        {
            city = City;
            country = Country;
            street = Street;
            number = Number;
            zipCode = ZipCode;
        }
    }
}
