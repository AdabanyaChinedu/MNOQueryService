namespace MNOQueryService.Domain.Entities
{
    public class Country : BaseEntity<int>
    {
        public Country(string name, string countryCode, string countryIso)
        {
            Name = name;
            CountryCode = countryCode;
            CountryIso = countryIso;
            Operators = new List<NetworkOperator>();
        }

        public Country(int id, string name, string countryCode, string countryIso)
            : this(name, countryCode, countryIso)
        {
            Id = id;
        }

        protected Country()
        {
        }

        public string Name { get; protected set; }
        public string CountryCode { get; protected set; }
        public string CountryIso { get; protected set; }
        public ICollection<NetworkOperator> Operators { get; protected set; }


    }
}
