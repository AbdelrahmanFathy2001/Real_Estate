


namespace Project.API.Controllers.Account
{
    [Authorize(Roles = "Admin")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IOfficesServices _officesServices;
        private readonly IGenericRepository<ApplicationUser> _userRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public AuthorizationController(IAuthorizationService authorizationService,
            IOfficesServices officesServices, IGenericRepository<ApplicationUser> userRepo,
            UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _authorizationService = authorizationService;
            _officesServices = officesServices;
            _userRepo = userRepo;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet(Router.AuthorizationRouting.GetAllRoles)]
        public async Task<ActionResult<List<GetAllRoles>>> GetAllRoles()
        {
            var roles = await _authorizationService.GetRolesAsync();
            var mappedRoles = roles.Select(x => new GetAllRoles { RoleName = x.Name, Id = x.Id }).ToList();
            return Ok(mappedRoles);
        }

        [HttpGet(Router.AccountRouting.List)]
        public async Task<ActionResult<Pagination<GetAllUsersDto>>> GetAllUsers(UserSpecParams userSpec)
        {
            var spec = new UserSpecification(userSpec);

            var users = await _userRepo.GetAllWithSpecAsync(spec);

            var Data = _mapper.Map<IReadOnlyList<GetAllUsersDto>>(users);

            var countSpec = new UserWithFiltersForCountSpecification(userSpec);

            var Count = await _userRepo.GetCountAsync(countSpec);

            return Ok(new Pagination<GetAllUsersDto>(userSpec.PageIndex, userSpec.PageSize, Count, Data));


        }


        [HttpPost(Router.AuthorizationRouting.AddRole)]
        public async Task<IActionResult> CreateRoleAsync([FromBody] AddRoleDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var roleResult = await _authorizationService.AddRoleAsync(model.RoleName);

            return roleResult switch
            {
                "RoleIsExist" => BadRequest("Role already exists."),
                "Created" => Ok(new { mess = "Role created successfully." }),
                _ => BadRequest(new { mess = "Failed to create role. Try again." })
            };
        }

        [HttpPut(Router.AuthorizationRouting.UpdateRole)]
        public async Task<IActionResult> UpdateRoleAsync([FromBody] EditRoleDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var roleResult = await _authorizationService.UpdateRoleAsync(model.RoleName, model.Id);

            return roleResult switch
            {
                "NotFound" => NotFound(new { mess = "Role not found. Please try again." }),
                "RoleExist" => BadRequest(new { mess = "Role already exists." }),
                "Failure" => StatusCode(500, "An error occurred while updating the role. Please try again."),
                _ => Ok(new { mess = "Role updated successfully." })
            };
        }

        [HttpDelete(Router.AuthorizationRouting.DeleteRole)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var roleResult = await _authorizationService.DeleteRoleAsync(id);

            return roleResult switch
            {
                "notfound" => NotFound(new { mess = "Role not found." }),
                "Delete" => Ok(new { mess = "Role deleted successfully." }),
                _ => BadRequest(new { mess = "Failed to delete role." })
            };
        }

        [HttpGet(Router.AuthorizationRouting.GetUsersByRole)]
        public async Task<ActionResult<IReadOnlyList<ManageUsers>>> GetUsersByRole([FromQuery] string? roleName)
        {
            try
            {
                var users = await _authorizationService.GetUsersByRoleAsync(roleName);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet(Router.AuthorizationRouting.ManageUserRole)]
        public async Task<ActionResult<ManageUserRoles>> ManageUserRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var userManage = await _authorizationService.GetManageUserRoleWithRoles(user);

            if (userManage == null)
            {
                return BadRequest(new { mess = "Failed to manage user roles." });
            }

            return Ok(userManage);
        }

        [HttpPost(Router.AuthorizationRouting.UpdateUserRole)]
        [AllowAnonymous]
        public async Task<ActionResult<UpdateUserRole>> UpdateUserRole([FromBody] UpdateUserRole userRole)
        {
            var userRolesResult = await _authorizationService.UpdateUserRoleWithRoles(userRole);

            return userRolesResult switch
            {
                "notfound" => NotFound(new { mess = "User not found." }),
                "FailedRemoveOldRoles" => BadRequest(new { mess = "Failed to remove old roles." }),
                "FailedAddNewRoles" => BadRequest(new { mess = "Failed to add new roles." }),
                "Success" => Ok(new { mess = "User roles updated successfully." }),
                _ => BadRequest(new { mess = "An error occurred. Please try again." })
            };
        }

    }

}
