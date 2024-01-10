namespace MNOQueryService.Application.UseCases.MNOQuerys.ViewModels
{
    public class OperatorCountryResponse
    {

        public string CountryCode { get; set; } = default!;

        public string Name { get; set; } = default!;

        public string CountryIso { get; set; } = default!;

        public List<OperatorCountryDetailsResponse> CountryDetails { get; set; } = default!;

    }
}
