using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.Dtos.Office;
using Stripe;
using System.Security.Claims;

namespace Project.API.Controllers
{
    [Authorize(Roles = "Admin,REOffice,REOStaff")]
    public class RealEstateAdsController : ControllerBase
    {
        private readonly IRealEstateAdService _service;
        private readonly IGenericRepository<RealEstateAd> _realEstateAdRepo;

        public RealEstateAdsController(IRealEstateAdService service , 
            IGenericRepository<RealEstateAd> RealEstateAdRepo)
        {
            _service = service;
            _realEstateAdRepo = RealEstateAdRepo;
        }

        [HttpGet(Router.RealEstateAd.List)]
        public async Task<ActionResult<Pagination<RealEstateAdDto>>> GetRealEstateAds([FromQuery] RealEstateAdPublicSpecParams specParams)
        {
            var spec = new RealEstateAdSpecification(specParams);

            var Data = await _service.GetAllWithSpecAsync(spec);

            var countSpec = new RealEstateAdWithFiltersForCountSpecification(specParams);

            var Count = await _realEstateAdRepo.GetCountAsync(countSpec);

            return Ok(new Pagination<RealEstateAdDto>(specParams.PageIndex, specParams.PageSize, Count, Data));
        }

        [HttpGet(Router.RealEstateAd.GetById)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RealEstateAdDto>> GetRealEstateAdById(int id)
        {
            var spec = new RealEstateAdSpecification(id);
            var data = await _service.GetByIdWithSpecAsync(spec);
            if (data == null && data.Ispublic == false) return NotFound(new ApiResponse(404));
            return Ok(data);
        }
    }
}
