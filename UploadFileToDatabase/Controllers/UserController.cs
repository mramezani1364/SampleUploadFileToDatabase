
using Microsoft.AspNetCore.Mvc;
using UploadFileToDatabase.Aggregate;
using UploadFileToDatabase.Dtos.UserDto;


namespace UploadFileToDatabase.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region -- Mapper --
        private User Convert(UserAddDto userAddDto)
        {
            User User = new()
            {
                FirstName = userAddDto.FirstName,
                LastName = userAddDto.LastName,
                Email = userAddDto.Email,
                CurriculumVitae = userAddDto.CurriculumVitae
            };
            return User;
        }
        private User Convert(UserEditDto userEditDto)
        {
            User User = new()
            {
                Id = userEditDto.Id,
                FirstName = userEditDto.FirstName,
                LastName = userEditDto.LastName,
                Email = userEditDto.Email,
                CurriculumVitae = userEditDto.CurriculumVitae
            };
            return User;
        }
        private UserEditDto Convert(User user)
        {
            string curriculumVitaeBase64 = string.Empty;

            if (user.CurriculumVitae != null)
            {
                curriculumVitaeBase64 = System.Convert.ToBase64String(user?.CurriculumVitae);
            }

            UserEditDto userEditDto = new()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                CurriculumVitae = user.CurriculumVitae,
                CurriculumVitaeBase64 = curriculumVitaeBase64
            };
            return userEditDto;
        }
        private IEnumerable<UserGetAllDto> Convert(IEnumerable<User> users)
        {
            List<UserGetAllDto> userGetAllDtos = new();

            foreach (var item in users)
            {
                UserGetAllDto userGetAllDto = new()
                {
                    Id = item.Id,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Email = item.Email,
                    CurriculumVitae = item.CurriculumVitae
                };
                userGetAllDtos.Add(userGetAllDto);
            }

            return userGetAllDtos;
        }
        #endregion

        #region -- Post --

        [HttpPost]
        public IActionResult Add(UserAddDto userAddDto)
        {

            try
            {
                if (userAddDto?.FormFile?.ContentType == "application/pdf")
                {

                    using (var memoryStream = new MemoryStream())
                    {
                        if (userAddDto?.FormFile != null)
                        {
                            userAddDto.FormFile.CopyTo(memoryStream);

                            if (memoryStream.Length > 10)
                            {
                                userAddDto.CurriculumVitae = memoryStream.ToArray();
                            }
                        }
                    }
                }

                _context.User.Add(Convert(userAddDto));
                _context.SaveChanges();

                return RedirectToAction("Index", "User");


            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public IActionResult Edit(UserEditDto userEditDto)
        {
            try
            {
                var user = _context.User.SingleOrDefault(i => i.Id == userEditDto.Id);
                bool curriculumVitaeIsModified = false;

                if (userEditDto?.FormFile != null && userEditDto?.FormFile?.ContentType == "application/pdf")
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        if (userEditDto.FormFile != null)
                        {
                            userEditDto.FormFile.CopyTo(memoryStream);

                            if (memoryStream.Length > 0)
                            {
                                userEditDto.CurriculumVitae = memoryStream.ToArray();
                            }
                        }
                    }
                    curriculumVitaeIsModified = true;
                }


                _context.Entry(user).CurrentValues.SetValues(userEditDto);
                _context.Entry(user).Property(p => p.CurriculumVitae).IsModified = curriculumVitaeIsModified;
                _context.SaveChanges();

                return RedirectToAction("Index", "User");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public IActionResult Remove(Guid Id)
        {
            try
            {
                var user = _context.User.SingleOrDefault(i => i.Id == Id);
             
                _context.User.Remove(user);
                _context.SaveChanges();
                return RedirectToAction("Index", "User");
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region -- Get --
        [HttpGet]
        public IActionResult Index()
        {
            var user = _context.User.ToList();
            return View(Convert(user));
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Edit(Guid Id)
        {
            var user = _context.User.Find(Id);
            return View(Convert(user));
        }

        [HttpGet]
        public IActionResult Detail(Guid Id)
        {
            var user = _context.User.Find(Id);
            return View(Convert(user));
        }
        #endregion
    }
}
