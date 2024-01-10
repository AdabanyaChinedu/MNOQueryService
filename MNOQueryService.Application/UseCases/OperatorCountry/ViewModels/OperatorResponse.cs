namespace MNOQueryService.Application.UseCases.MNOQuerys.ViewModels
{
    public class OperatorResponse
    {
        public string Number { get; set; } = default!;

        public OperatorCountryResponse Country { get; set; } = default!;
       
    }
}
