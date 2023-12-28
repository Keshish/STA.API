using Microsoft.AspNetCore.Identity;
using STA.API.Dtos.Parent;
using STA.API.Models.Authentication;
using STA.API.Models.DbContext;
using STA.API.Models.Users;
using STA.API.ViewModels;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using STA.API.Services.Abstractions;

namespace STA.API.Services.Implementations
{
    public class ParentService : IParentService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly BaseDataContext _dbContext;
        private readonly IMapper _mapper;

        private readonly ILogger<ParentService> _logger;


        public ParentService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, BaseDataContext dbContex, IMapper mapper, ILogger<ParentService> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _dbContext = dbContex ?? throw new ArgumentNullException(nameof(dbContex));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<List<ParentVM>> GetParentsAsync()
        {
            _logger.LogInformation("GetParentsAsync called.");
            var parents = await _dbContext.Parents.Include(p => p.User).ToListAsync();

            return _mapper.Map<List<ParentVM>>(parents);
        }

        public async Task<ParentVM> GetParentWithIdAsync(int Id)
        {
            _logger.LogInformation("GetParentWithIdAsync called.");
            var parent = await _dbContext.Parents.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == Id);

            if (parent == null)
            {
                throw new ApplicationException("Parent not found.");
            }
            return _mapper.Map<ParentVM>(parent);
        }
        public async Task<ParentVM> GetParentWithUserIdAsync(string userId)
        {
            _logger.LogInformation("GetParentWithUserIdAsync called.");
            var parent = await _dbContext.Parents.Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == userId);
            if (parent == null)
            {
                   throw new ApplicationException("Parent not found.");
            }
            return _mapper.Map<ParentVM>(parent);
        }


        public async Task<bool> DeleteParentWithParentIdAsync(int Id)
        {
            _logger.LogInformation("DeleteParentWithParentIdAsync called.");
            var parent = await _dbContext.Parents.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == Id);
            if (parent == null)
            {
                throw new ApplicationException("Parent not found.");
            }
            _dbContext.Parents.Remove(parent);
            _dbContext.Users.Remove(parent.User);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RegisterParentAsync(ParentRegisterDto parentRegisterDto)
        {
            _logger.LogInformation("RegisterParentAsync called.");
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

                if (!string.IsNullOrEmpty("Parent"))
                {
                    if (await _roleManager.RoleExistsAsync("Parent"))
                    {
                        await _userManager.AddToRoleAsync(newUser, "Parent");
                    }
                    else
                    {
                        throw new ApplicationException("Role Parent does not exist.");
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

        public async Task<bool> UpdateParentAsync(ParentUpdateDto parentUpdateDto)
        {
            _logger.LogInformation("UpdateParentAsync called.");
            var parent = _dbContext.Parents.Include(x => x.User).FirstOrDefault(x => x.Id == parentUpdateDto.ParentId);
            if (parent == null)
            {
                throw new ApplicationException("Parent not found.");
            }
            parent.User.UserName = parentUpdateDto.UserName;
            parent.User.Email = parentUpdateDto.Email;
            parent.User.NormalizedUserName = parentUpdateDto.UserName.ToUpper();
            _dbContext.SaveChanges();
            return true;
        }

    }
}
