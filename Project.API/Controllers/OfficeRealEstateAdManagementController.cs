using Project.BLL.Attributes;
using Project.BLL.Dtos.Office;
using System.Security.Claims;

namespace Project.API.Controllers
{
    [Authorize(Roles = "REOffice,REOStaff")]
    public class OfficeRealEstateAdManagementController : ControllerBase
    {
        private readonly IRealEstateAdService _service;
        private readonly IGenericRepository<ClientInfo> _clientRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenericRepository<RealEstateAd> _realEstateAdRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Image> _repoImage;

        public OfficeRealEstateAdManagementController(IRealEstateAdService service,
            IGenericRepository<ClientInfo> clientRepo, UserManager<ApplicationUser> userManager, 
            IGenericRepository<RealEstateAd> RealEstateAdRepo, IMapper mapper, IGenericRepository<Image> repoImage)
        {
            _service = service;
            _clientRepo = clientRepo;
            _userManager = userManager;
            _realEstateAdRepo = RealEstateAdRepo;
            _mapper = mapper;
            _repoImage = repoImage;
        }

        [HttpGet(Router.RealEstateAdRouting.List)]
        public async Task<ActionResult<Pagination<RealEstateAdDto>>> GetRealEstateAds([FromQuery] RealEstateAdSpecParams specParams)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole("REOStaff"))
            {
                var REOStaff = await _userManager.FindByIdAsync(currentUserId);
                currentUserId = REOStaff.SupervisorId;
            }
            var client = await _clientRepo.GetByIdAsync(specParams.ClientId);

            if (client == null) return NotFound();
            if (client.ApplicationUserId != currentUserId) return NotFound();


            var spec = new RealEstateAdSpecification(specParams);

            var Data = await _service.GetAllWithSpecAsync(spec);

            var countSpec = new RealEstateAdWithFiltersForCountSpecification(specParams);

            var Count = await _realEstateAdRepo.GetCountAsync(countSpec);

            return Ok(new Pagination<RealEstateAdDto>(specParams.PageIndex, specParams.PageSize, Count, Data));
        }

        [HttpGet(Router.RealEstateAdRouting.GetById)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RealEstateAdDto>> GetRealEstateAdById(int id)
        {
            var spec = new RealEstateAdSpecification(id);
            var data = await _service.GetByIdWithSpecAsync(spec);
            if (data == null) return NotFound(new ApiResponse(404));
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole("REOStaff"))
            {
                var REOStaff = await _userManager.FindByIdAsync(currentUserId);
                currentUserId = REOStaff.SupervisorId;
            }
            if (data.office.Id != currentUserId) return NotFound();

            return Ok(data);
        }

        [HttpPost(Router.RealEstateAdRouting.Create)]
        public async Task<IActionResult> Create(int clientId, [FromForm] RealEstateAdAddOrEditDto adDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var client = await _clientRepo.GetByIdAsync(clientId);
            if (client == null) return NotFound(new { mess = "هذا العميل غير موجود!" });

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole("REOStaff"))
            {
                var REOStaff = await _userManager.FindByIdAsync(currentUserId);
                currentUserId = REOStaff.SupervisorId;
            }
            if (client.ApplicationUserId != currentUserId) return NotFound(new { mess = "ليس لديك الصلاحية لإضافة إعلان عقاري لهذا العميل." });

            try
            {
                // تحويل النصوص العربية إلى قيم enum
                var dealType = EnumTranslations.GetEnumFromTranslation<DealType>(adDto.DealType);
                var unitType = EnumTranslations.GetEnumFromTranslation<UnitType>(adDto.UnitType);
                var requestType = EnumTranslations.GetEnumFromTranslation<RequestType>(adDto.RequestType);


                var ad = new RealEstateAd
                {
                    Region = adDto.Region,
                    City = adDto.City,
                    Description = adDto.Description,
                    Neighborhood = adDto.Neighborhood,
                    DealType = dealType,
                    UnitType = unitType,
                    UnitValue = adDto.UnitValue,
                    UnitLocation = adDto.UnitLocation,
                    Longitude = adDto.Longitude,
                    Latetude = adDto.Latitude,
                    RequestType = requestType,
                    ClientId = clientId
                };


                await _service.AddAsync(ad);

                if (adDto.ImagesUrl != null && adDto.ImagesUrl.Count > 0)
                {
                    var validImages = adDto.ImagesUrl.Take(4).ToList();

                    foreach (var file in validImages)
                    {
                        if (file.Length > 5 * 1024 * 1024)
                        {
                            return BadRequest(new { message = "Each image must not exceed 5MB." });
                        }

                        var fileName = DocumentSettings.UploadFile(file);

                        var image = new Image
                        {
                            PictureUrl = fileName,
                            RealEstateAdId = ad.Id
                        };

                        await _repoImage.AddAsync(image);
                    }
                }
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mess = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mess = $"خطأ داخلي: {ex.Message}" });
            }
        }

        [HttpPut(Router.RealEstateAdRouting.Edit)]
        public async Task<IActionResult> Update(int id, [FromForm] RealEstateAdDto adDto)
        {
            if (id != adDto.Id) return BadRequest("ID mismatch");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole("REOStaff"))
            {
                var REOStaff = await _userManager.FindByIdAsync(currentUserId);
                currentUserId = REOStaff.SupervisorId;
            }
            if (adDto.office.Id != currentUserId) return NotFound(new { mess = "ليس لديك الصلاحية لتعديل هذا الاعلان لهذا العميل." });

            var spec = new RealEstateAdSpecification(id);
            //var data = await _service.GetByIdWithSpecAsync(spec);
            var data = await _realEstateAdRepo.GetByIdWithSpecAsync(spec);
            if (data == null) return NotFound();


            var dealType = EnumTranslations.GetEnumFromTranslation<DealType>(adDto.DealType);
            var unitType = EnumTranslations.GetEnumFromTranslation<UnitType>(adDto.UnitType);
            var requestType = EnumTranslations.GetEnumFromTranslation<RequestType>(adDto.RequestType);

            data.Region = adDto.Region;
            data.City = adDto.City;
            data.Neighborhood = adDto.Neighborhood;
            data.DealType = dealType;
            data.UnitType = unitType;
            data.UnitValue = adDto.UnitValue;
            data.UnitLocation = adDto.UnitLocation;
            data.Longitude = adDto.Longitude;
            data.Latetude = adDto.Latitude;
            data.RequestType = requestType;

            // إذا كنت تستخدم EF Core، قم بتحديث الكائن بدلاً من إضافة كائن جديد بنفس الـ ID.
            await _service.UpdateAsync(data);
            return NoContent();
        }

        [HttpDelete(Router.RealEstateAdRouting.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            var spec = new RealEstateAdSpecification(id);
            var data = await _service.GetByIdWithSpecAsync(spec);            
            if (data == null) return NotFound();
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole("REOStaff"))
            {
                var REOStaff = await _userManager.FindByIdAsync(currentUserId);
                currentUserId = REOStaff.SupervisorId;
            }
            if (data.office.Id != currentUserId) return NotFound(new { mess = "ليس لديك الصلاحية لتعديل هذا الاعلان لهذا العميل." });
            foreach (var file in data.Images)
            {
                DocumentSettings.DeleteFile(file.Name);
            }
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
