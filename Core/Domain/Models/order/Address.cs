namespace Domain.Models.order
{
    public class Address
    {
        public Address()
        {
            
        }

        public Address(string firstName, string lastName, string streate, string city, string country)
        {
            FirstName = firstName;
            LastName = lastName;
            Streate = streate;
            City = city;
            Country = country;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Streate { get; set; }
        public string City { get; set; }
        public string Country { get; set; }


    }
}