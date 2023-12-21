using Microsoft.AspNetCore.Identity;
using STA.API.Dtos.Parent;
using STA.API.Models.Authentication;
using STA.API.Models.DbContext;
using STA.API.Models.Users;
using STA.API.ViewModels;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace STA.API.Services
{
    public class ParentService : IParentService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly BaseDataContext _dbContext;
        private readonly IMapper _mapper;

        private readonly ILogger<ParentService> _logger;


        public ParentService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, BaseDataContext dbContex, IMapper mapper)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _dbContext = dbContex ?? throw new ArgumentNullException(nameof(dbContex));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<List<ParentVM>> GetParentsAsync()
        {
            var parents = await _dbContext.Parents.Include(p => p.User).ToListAsync();

            return _mapper.Map<List<ParentVM>>(parents);
        }

        public Task<ParentVM> GetParentAsync(int Id)
        {
            throw new NotImplementedException();
        }


        public async Task<bool> RegisterParentAsync(ParentRegisterDto parentRegisterDto)
        {
            try
            {
                var existingUser = await _userManager.FindByNameAsync(parentRegisterDto.UserName);
                if (existingUser != null)
                {
                    throw new ApplicationException("User creation failed. There is a user with the same name.");
                }

                var newUser = new ApplicationUser
                {
                    Email = parentRegisterDto.Email,
                    UserName = parentRegisterDto.UserName,
                    SecurityStamp = Guid.NewGuid().ToString(),
                };

                var result = await _userManager.CreateAsync(newUser, parentRegisterDto.Password);
                if (!result.Succeeded)
                {
                    throw new ApplicationException($"Error during user creation: {string.Join(", ", result.Errors)}");
                }

                if (!string.IsNullOrEmpty(parentRegisterDto.Role))
                {
                    if (await _roleManager.RoleExistsAsync(parentRegisterDto.Role))
                    {
                        await _userManager.AddToRoleAsync(newUser, parentRegisterDto.Role);
                    }
                    else
                    {
                        throw new ApplicationException($"Role '{parentRegisterDto.Role}' does not exist.");
                    }
                }

                var newParent = new Parent
                {
                    UserId = newUser.Id,
                };

                _dbContext.Parents.Add(newParent);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred during parent registration.");
                throw;
            }
        }


    }
}
