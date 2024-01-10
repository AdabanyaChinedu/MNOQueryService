using MediatR;
using Microsoft.AspNetCore.Mvc;
using MNOQueryService.Application.UseCases.MNOQuerys.Queries;

namespace MNOQueryService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MNOController : ControllerBase
    {        
        private readonly ILogger<MNOController> logger;
        private readonly ISender sender;

        public MNOController(ILogger<MNOController> logger, ISender sender)
        {
            this.logger = logger;
            this.sender = sender;
        }

        [HttpGet(Name = "GetMNODetails")]
        public async Task<IActionResult> GetOperatorCountryDetails([FromQuery] string phoneNumber)
        {
            var response = await sender.Send(new OperatorDetail.Query(phoneNumber));
            return Ok(response);
        }
    }
}