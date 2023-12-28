using AutoMapper;
using Microsoft.EntityFrameworkCore;
using STA.API.Dtos.Student;
using STA.API.Models;
using STA.API.Models.DbContext;
using STA.API.Services.Abstractions;
using STA.API.ViewModels;

namespace STA.API.Services.Implementations
{
    public class StudentService : IStudentService
    {

        private readonly BaseDataContext _dbContext;

        private readonly IMapper _mapper;
        private readonly ILogger<StudentService> _logger;
        public StudentService(BaseDataContext dbContext, ILogger<StudentService> logger, IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<StudentVM>> GetStudentsWithParentIdAsync(int parentId)
        {
            _logger.LogInformation("GetStudentsAsync called.");
            if (parentId == -1)
            {
                throw new ApplicationException("Parent not found.");
            }
            var students = await _dbContext.Students.Include(p => p.Parent).Where(s => s.ParentId == parentId).ToListAsync();

            return _mapper.Map<List<StudentVM>>(students);
        }

        public async Task<StudentVM> GetStudentWithIdAsync(int Id)
        {
            _logger.LogInformation("GetStudentAsync called.");
            var student = await _dbContext.Students.Include(p => p.Parent).FirstOrDefaultAsync(x => x.Id == Id);
            if (student == null)
            {
                throw new ApplicationException("Student not found.");
            }
            return _mapper.Map<StudentVM>(student);
        }

        public async Task<bool> RegisterStudenAsync(StudentRegisterDto studentRegisterDto)
        {
            _logger.LogInformation("RegisterStudenAsync called.");

            var student = _mapper.Map<Student>(studentRegisterDto);

            _dbContext.Students.Add(student);
            _dbContext.SaveChanges();

            return true;
        }
    }
}
