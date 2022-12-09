using CreditClearance.Domain.DTOs.InputDTOs;
using CreditClearance.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace CreditClearance.API.Controllers
{
    [Route("api/main")]
    public class MainController : Controller
    {
        #region Properties
        private readonly ICreditService _creditService;
        #endregion

        #region Constructors
        public MainController(ICreditService creditService)
        {
            _creditService = creditService;
        }
        #endregion

        #region Methods
        [HttpPost("credit-analysis")]
        public IActionResult CreditAnalysis([FromBody] CreditClearanceInputDTO request)
        {
            try
            {
                var response = _creditService.CreditClearanceAnalysis(request);

                return Ok(response);
            }
            catch(Exception e) 
            {
                while (e.InnerException != null) e = e.InnerException;
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("")]
        public IActionResult GetAllCreditTypes()
        {
            try
            {
                var response = _creditService.GetAllCreditTypes();

                return Ok(response);
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                return StatusCode(500, e.Message);
            }
        }
        #endregion
    }
}
