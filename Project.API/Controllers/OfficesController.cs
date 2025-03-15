using Project.BLL.Attributes;
using Project.BLL.Dtos.Office;

namespace Project.API.Controllers
{
    [Authorize(Roles = "Admin,REOffice,REOStaff")]
    public class OfficesController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOfficesServices _officesServices;
        private readonly IRealEstateAdService _realEstateAdService;
        private readonly IMapper _mapper;

        public OfficesController(UserManager<ApplicationUser> userManager,
            IOfficesServices officesServices , IRealEstateAdService realEstateAdService ,
            IMapper mapper)
        {
            _userManager = userManager;
            _officesServices = officesServices;
            _realEstateAdService = realEstateAdService;
            _mapper = mapper;
        }


        [HttpGet(Router.OfficeRouting.List)]
        public async Task<ActionResult<Pagination<GetAllUsersDto>>> GetUsersByRole(OfficeSpecParams specParams)
        {
            try
            {
                var roleName = "REOffice";
                var users = await _officesServices.GetUsersByRoleAsync(roleName);

                if (!string.IsNullOrWhiteSpace(specParams.Search))
                {
                    users = users.Where(user => user.UserName.Contains(specParams.Search, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                // Apply pagination
                var totalUsers = users.Count;
                var paginatedUsers = users.Skip((specParams.PageIndex - 1) * specParams.PageSize).Take(specParams.PageSize).ToList();

                // Map the data
                var data = _mapper.Map<IReadOnlyList<GetAllUsersDto>>(paginatedUsers);

                return Ok(new Pagination<GetAllUsersDto>(specParams.PageIndex, specParams.PageSize, totalUsers, paginatedUsers));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet(Router.OfficeRouting.GetById)]
        public async Task<IActionResult> GetUserByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            // Query to load RealEstateAds with IsPublic = true and include Images
            var userWithRealEstateAds = await _userManager.Users
                .Where(u => u.Id == id)
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.PhoneNumber,
                    u.Email,
                    RealEstateAds = u.ClientInfos
                        .SelectMany(ci => ci.RealEstateAds)
                        .Where(ad => ad.Ispublic) // Filter ads where IsPublic = true
                        .Select(ad => new
                        {
                            Ad = ad,
                            Images = ad.Images // Include associated images
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (userWithRealEstateAds == null)
                return NotFound(new { mess = $"ID={id} is not found", info = "Try Again" });

            // Map the result to DTO
            var result = new OfficeToReturnDto
            {
                Id = userWithRealEstateAds.Id,
                UserName = userWithRealEstateAds.UserName,
                Phone = userWithRealEstateAds.PhoneNumber,
                Email = userWithRealEstateAds.Email,
                RealEstateAds = userWithRealEstateAds.RealEstateAds
                    .Select(ad => ConvertToDto(ad.Ad)) // Convert ads to DTO
                    .ToList()
            };

            return Ok(result);
        }

        public static RealEstateAdDto ConvertToDto(RealEstateAd ad)
        {
            var apiUrl = "https://localhost:7276/images/products/"; // Base API URL

            return new RealEstateAdDto
            {
                Id = ad.Id,
                Region = ad.Region,
                City = ad.City,
                Neighborhood = ad.Neighborhood,
                DealType = EnumTranslations.GetTranslation(ad.DealType),
                UnitType = EnumTranslations.GetTranslation(ad.UnitType),
                UnitValue = ad.UnitValue,
                UnitLocation = ad.UnitLocation,
                Longitude = ad.Longitude,
                Latitude = ad.Latetude,
                RequestType = EnumTranslations.GetTranslation(ad.RequestType),
                Images = ad.Images?.Select(img => new ImageToReturnDto
                {
                    Id = img.Id,
                    Name = new Uri(new Uri(apiUrl), img.PictureUrl).ToString() // Full URL for image
                }).ToList() ?? new List<ImageToReturnDto>(),
            };
        }



    }
}
