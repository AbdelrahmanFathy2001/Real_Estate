using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.Dtos.Office;
using System.Security.Claims;

namespace Project.API.Controllers
{
    [Authorize(Roles = "REOffice,REOStaff")]
    public class OfficeClientManagementController : ControllerBase
    {
        private readonly IRealEstateAdService _service;
        private readonly IGenericRepository<ClientInfo> _clientRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public OfficeClientManagementController(IRealEstateAdService service,
            IGenericRepository<ClientInfo> clientRepo, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _service = service;
            _clientRepo = clientRepo;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet(Router.ClientRouting.List)]
        public async Task<ActionResult<IReadOnlyList<clientDto>>> GetAllClient()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole("REOStaff"))
            {
                var REOStaff = await _userManager.FindByIdAsync(currentUserId);
                currentUserId = REOStaff.SupervisorId;
            }

            var clients = await _clientRepo.GetAllAsync();
            var filteredClients = clients.Where(c => c.ApplicationUserId == currentUserId).ToList();
            var Data = _mapper.Map<IReadOnlyList<clientDto>>(filteredClients);

            return Ok(Data);
        }

        [HttpGet(Router.ClientRouting.GetById)]
        public async Task<IActionResult> GetById(int id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole("REOStaff"))
            {
                var REOStaff = await _userManager.FindByIdAsync(currentUserId);
                currentUserId = REOStaff.SupervisorId;
            }
            var client = await _clientRepo.GetByIdAsync(id);
            if (client == null) return NotFound();
            if (client.ApplicationUserId != currentUserId) return NotFound();
            var clientDto = new clientDto
            {
                Id = client.Id,
                Name = client.Name,
                ApplicationUserId = client.ApplicationUserId

            };
            return Ok(clientDto);
        }

        [HttpPost(Router.ClientRouting.Create)]
        public async Task<ActionResult> AddClient([FromForm] clientCreateDto clientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole("REOStaff"))
            {
                var REOStaff = await _userManager.FindByIdAsync(currentUserId);
                currentUserId = REOStaff.SupervisorId;
            }
            var client = new ClientInfo()
            {
                Name = clientDto.Name,
                Phone = clientDto.Phone,
                Email = clientDto.Email,
                Country = clientDto.Country,
                City = clientDto.City,
                Street = clientDto.Street,
                ApplicationUserId = currentUserId
            };
            await _clientRepo.AddAsync(client);
            return Ok(client);
        }


        [HttpPost(Router.ClientRouting.Edit)]
        public async Task<ActionResult> EditClient([FromRoute] int? id, [FromForm] clientDto clientDto)
        {
            if (id != clientDto.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    if (User.IsInRole("REOStaff"))
                    {
                        var REOStaff = await _userManager.FindByIdAsync(currentUserId);
                        currentUserId = REOStaff.SupervisorId;
                    }
                    var client = new ClientInfo()
                    {
                        Id = clientDto.Id,
                        Name = clientDto.Name,
                        Phone = clientDto.Phone,
                        Email = clientDto.Email,
                        Country = clientDto.Country,
                        City = clientDto.City,
                        Street = clientDto.Street,
                        ApplicationUserId = currentUserId
                    };
                    await _clientRepo.UpdateAsync(client);
                    return Ok(clientDto);
                }
                catch
                {
                    return Ok(clientDto);
                }
            }
            return BadRequest(ModelState);

        }

        [HttpPost(Router.ClientRouting.Delete)]
        public async Task<ActionResult> DeleteClient([FromRoute] int? id, [FromForm] clientDto clientDto)
        {
            if (id != clientDto.Id)
                return BadRequest();
            try
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (User.IsInRole("REOStaff"))
                {
                    var REOStaff = await _userManager.FindByIdAsync(currentUserId);
                    currentUserId = REOStaff.SupervisorId;
                }
                var client = new ClientInfo()
                {
                    Id = clientDto.Id,
                    Name = clientDto.Name,
                    ApplicationUserId = currentUserId
                };
                await _clientRepo.DeleteAsync(client);
                return Ok(clientDto);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }

        //[HttpPost]
        //public async Task<IActionResult> Create(int clientId ,[FromBody] RealEstateAdDto adDto)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);
        //    var client = await _clientRepo.GetByIdAsync(clientId);
        //    if (client == null)
        //    {
        //        ModelState.AddModelError("Client", "The specified client does not exist or is invalid.");
        //        return BadRequest(ModelState);
        //    }
        //    // تحويل DTO إلى الكيان إذا لزم الأمر
        //    var ad = new RealEstateAd
        //    {
        //        Region = adDto.Region,
        //        City = adDto.City,
        //        Neighborhood = adDto.Neighborhood,
        //        DealType = (DealType)Enum.Parse(typeof(DealType), adDto.DealType),
        //        UnitType = (UnitType)Enum.Parse(typeof(UnitType), adDto.UnitType),
        //        UnitValue = adDto.UnitValue,
        //        UnitLocation = adDto.UnitLocation,
        //        Longitude = adDto.Longitude,
        //        Latetude = adDto.Latitude,
        //        RequestType = (RequestType)Enum.Parse(typeof(RequestType), adDto.RequestType),
        //        ClientId = clientId
        //    };
        //    await _service.AddAsync(ad);
        //    return CreatedAtAction(nameof(GetById), new { id = ad.Id }, adDto);
        //}

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(int id, [FromBody] RealEstateAdDto adDto)
        //{
        //    if (id != adDto.Id) return BadRequest("ID mismatch");
        //    var existingAd = await _service.GetByIdAsync(id);
        //    if (existingAd == null) return NotFound();
        //    var ad = new RealEstateAd
        //    {
        //        Id = adDto.Id,
        //        Region = adDto.Region,
        //        City = adDto.City,
        //        Neighborhood = adDto.Neighborhood,
        //        DealType = (DealType)Enum.Parse(typeof(DealType), adDto.DealType),
        //        UnitType = (UnitType)Enum.Parse(typeof(UnitType), adDto.UnitType),
        //        UnitValue = adDto.UnitValue,
        //        UnitLocation = adDto.UnitLocation,
        //        Longitude = adDto.Longitude,
        //        Latetude = adDto.Latitude,
        //        RequestType = (RequestType)Enum.Parse(typeof(RequestType), adDto.RequestType),
        //    };
        //    await _service.UpdateAsync(ad);
        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var existingAd = await _service.GetByIdAsync(id);
        //    if (existingAd == null) return NotFound();
        //    await _service.DeleteAsync(id);
        //    return NoContent();
        //}
    }
}
