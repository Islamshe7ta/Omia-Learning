using Microsoft.AspNetCore.Mvc;
using Omia.DAL.Models.Entities;
using Omia.DAL.Models.Enums;
using Omia.PL.Helpers;
using Omia.PL.Middlewares;
using Omia.PL.ViewModels.CentralAdmin;
using Omia.BLL.UnitOfWork;
using AutoMapper;
using Omia.BLL.DTOs.CentralAdmin;
using Microsoft.AspNetCore.Authorization;

namespace Omia.PL.Controllers
{
    [AdminAuthFilter]
    public class CentralAdminController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CentralAdminController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [AllowAnonymousAction]
        [HttpGet]
        public IActionResult Login()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(AdminSessionHelper.AdminIdKey)))
                return RedirectToAction(nameof(Index));

            return View();
        }

        [AllowAnonymousAction]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var admin = await _uow.Admins.GetByUsernameAsync(model.Username);

            if (admin == null || !BCrypt.Net.BCrypt.Verify(model.Password, admin.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "اسم المستخدم أو كلمة المرور غير صحيحة");
                return View(model);
            }

            if (admin.Status != AccountStatus.Active)
            {
                ModelState.AddModelError(string.Empty, "هذا الحساب موقوف. يرجى التواصل مع المسؤول.");
                return View(model);
            }

            HttpContext.Session.SetString(AdminSessionHelper.AdminIdKey, admin.Id.ToString());
            HttpContext.Session.SetString(AdminSessionHelper.AdminNameKey, admin.FullName);
            HttpContext.Session.SetString(AdminSessionHelper.AdminUsernameKey, admin.Username);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var institutes = (await _uow.Institutes.GetAllWithDetailsAsync()).ToList();
            var soloTeachers = (await _uow.Teachers.GetSoloTeachersWithDetailsAsync()).ToList();

            var vm = _mapper.Map<DashboardViewModel>((institutes, soloTeachers));

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Details(DetailsFilterViewModel filter)
        {
            var institutes = await _uow.Institutes.FindAsync(i => !i.IsDeleted);
            var soloTeachers = await _uow.Teachers.FindAsync(t => t.InstituteId == null && !t.IsDeleted);

            var vm = _mapper.Map<DetailsViewModel>(filter);
            vm.Institutes = _mapper.Map<List<SelectOption>>(institutes.OrderBy(i => i.Name));
            vm.SoloTeachers = _mapper.Map<List<SelectOption>>(soloTeachers.OrderBy(t => t.FullName));

            if (filter.InstituteId.HasValue || filter.TeacherId.HasValue)
            {
                var today = DateTime.UtcNow;
                
                if (filter.InstituteId.HasValue)
                {
                    var institute = await _uow.Institutes.GetWithDetailsByIdAsync(filter.InstituteId.Value);
                    if (institute != null)
                    {
                        vm.SelectedEntityName = institute.Name;
                        vm.StudentCount = institute.RegisteredStudents.Count;
                        var courses = institute.Courses.AsQueryable();
                        ApplyCourseFilters(ref courses, filter);
                        vm.CourseRows = _mapper.Map<List<CourseDetailRow>>(courses.ToList());
                    }
                }
                else
                {
                    var teacher = await _uow.Teachers.GetWithDetailsByIdAsync(filter.TeacherId!.Value);
                    if (teacher != null)
                    {
                        vm.SelectedEntityName = teacher.FullName;
                        vm.StudentCount = teacher.RegisteredStudents.Count;
                        var courses = teacher.Courses.AsQueryable();
                        ApplyCourseFilters(ref courses, filter);
                        vm.CourseRows = _mapper.Map<List<CourseDetailRow>>(courses.ToList());
                    }
                }
            }

            return View(vm);
        }

        private void ApplyCourseFilters(ref IQueryable<Course> courses, DetailsFilterViewModel filter)
        {
            if (filter.DateFrom.HasValue)
                courses = courses.Where(c => c.CreatedAt >= filter.DateFrom.Value);

            if (filter.DateTo.HasValue)
                courses = courses.Where(c => c.CreatedAt <= filter.DateTo.Value.AddDays(1));

            if (filter.CourseId.HasValue)
                courses = courses.Where(c => c.Id == filter.CourseId.Value);
        }

        [HttpGet]
        public async Task<IActionResult> Institutes()
        {
            var institutes = await _uow.Institutes.GetAllWithDetailsAsync();
            var vm = _mapper.Map<List<InstituteListViewModel>>(institutes);
            return View(vm);
        }

        [HttpGet]
        public IActionResult InstituteCreate()
        {
            return View("InstituteForm", new InstituteFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InstituteCreate(InstituteFormViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Password))
                ModelState.AddModelError(nameof(model.Password), "كلمة المرور مطلوبة عند الإنشاء");

            if (await _uow.Institutes.IsUsernameUsedAsync(model.Username))
                ModelState.AddModelError(nameof(model.Username), "اسم المستخدم مستخدم بالفعل");

            if (!ModelState.IsValid) return View("InstituteForm", model);

            var entity = _mapper.Map<Institute>(model);
            entity.FullName = model.Name;
            entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password!);
            entity.InitPassword = model.Password;
            entity.Status = AccountStatus.Active;

            await _uow.Institutes.AddAsync(entity);
            await _uow.CommitAsync();

            TempData["Success"] = "تم إنشاء المعهد بنجاح";
            return RedirectToAction(nameof(Institutes));
        }

        [HttpGet]
        public async Task<IActionResult> InstituteEdit(Guid id)
        {
            var entity = await _uow.Institutes.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted) return NotFound();

            var model = _mapper.Map<InstituteFormViewModel>(entity);
            return View("InstituteForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InstituteEdit(InstituteFormViewModel model)
        {
            if (await _uow.Institutes.IsUsernameUsedAsync(model.Username, model.Id))
                ModelState.AddModelError(nameof(model.Username), "اسم المستخدم مستخدم بالفعل");

            if (!ModelState.IsValid) return View("InstituteForm", model);

            var entity = await _uow.Institutes.GetByIdAsync(model.Id!.Value);
            if (entity == null || entity.IsDeleted) return NotFound();

            _mapper.Map(model, entity);
            entity.FullName = model.Name;

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
                entity.InitPassword = model.Password;
            }

            _uow.Institutes.Update(entity);
            await _uow.CommitAsync();

            TempData["Success"] = "تم تحديث بيانات المعهد بنجاح";
            return RedirectToAction(nameof(Institutes));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InstituteToggleStatus(Guid id)
        {
            var entity = await _uow.Institutes.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted) return NotFound();

            entity.Status = entity.Status == AccountStatus.Active ? AccountStatus.Paused : AccountStatus.Active;
            _uow.Institutes.Update(entity);
            await _uow.CommitAsync();

            TempData["Success"] = entity.Status == AccountStatus.Active ? "تم تفعيل المعهد" : "تم إيقاف المعهد";
            return RedirectToAction(nameof(Institutes));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InstituteDelete(Guid id)
        {
            var entity = await _uow.Institutes.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted) return NotFound();

            _uow.Institutes.Delete(entity);
            await _uow.CommitAsync();

            TempData["Success"] = "تم حذف المعهد";
            return RedirectToAction(nameof(Institutes));
        }

        [HttpGet]
        public async Task<IActionResult> Teachers()
        {
            var teachers = await _uow.Teachers.GetSoloTeachersWithDetailsAsync();
            var vm = _mapper.Map<List<TeacherListViewModel>>(teachers);
            return View(vm);
        }

        [HttpGet]
        public IActionResult TeacherCreate()
        {
            return View("TeacherForm", new TeacherFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TeacherCreate(TeacherFormViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Password))
                ModelState.AddModelError(nameof(model.Password), "كلمة المرور مطلوبة عند الإنشاء");

            if (await _uow.Teachers.IsUsernameUsedAsync(model.Username))
                ModelState.AddModelError(nameof(model.Username), "اسم المستخدم مستخدم بالفعل");

            if (!ModelState.IsValid) return View("TeacherForm", model);

            var entity = _mapper.Map<Teacher>(model);
            entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password!);
            entity.InitPassword = model.Password;
            entity.Status = AccountStatus.Active;

            await _uow.Teachers.AddAsync(entity);
            await _uow.CommitAsync();

            TempData["Success"] = "تم إنشاء حساب المدرس بنجاح";
            return RedirectToAction(nameof(Teachers));
        }

        [HttpGet]
        public async Task<IActionResult> TeacherEdit(Guid id)
        {
            var entity = await _uow.Teachers.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted) return NotFound();

            var model = _mapper.Map<TeacherFormViewModel>(entity);
            return View("TeacherForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TeacherEdit(TeacherFormViewModel model)
        {
            if (await _uow.Teachers.IsUsernameUsedAsync(model.Username, model.Id))
                ModelState.AddModelError(nameof(model.Username), "اسم المستخدم مستخدم بالفعل");

            if (!ModelState.IsValid) return View("TeacherForm", model);

            var entity = await _uow.Teachers.GetByIdAsync(model.Id!.Value);
            if (entity == null || entity.IsDeleted) return NotFound();

            _mapper.Map(model, entity);

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
                entity.InitPassword = model.Password;
            }

            _uow.Teachers.Update(entity);
            await _uow.CommitAsync();

            TempData["Success"] = "تم تحديث بيانات المدرس بنجاح";
            return RedirectToAction(nameof(Teachers));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TeacherToggleStatus(Guid id)
        {
            var entity = await _uow.Teachers.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted) return NotFound();

            entity.Status = entity.Status == AccountStatus.Active ? AccountStatus.Paused : AccountStatus.Active;
            _uow.Teachers.Update(entity);
            await _uow.CommitAsync();

            TempData["Success"] = entity.Status == AccountStatus.Active ? "تم تفعيل المدرس" : "تم إيقاف المدرس";
            return RedirectToAction(nameof(Teachers));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TeacherDelete(Guid id)
        {
            var entity = await _uow.Teachers.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted) return NotFound();

            _uow.Teachers.Delete(entity);
            await _uow.CommitAsync();

            TempData["Success"] = "تم حذف المدرس";
            return RedirectToAction(nameof(Teachers));
        }

        [HttpGet]
        public async Task<IActionResult> Admins()
        {
            var admins = (await _uow.Admins.GetAllAsync()).Where(a => !a.IsDeleted).OrderBy(a => a.FullName);
            var vm = _mapper.Map<List<AdminListViewModel>>(admins);
            return View(vm);
        }

        [HttpGet]
        public IActionResult AdminCreate()
        {
            return View("AdminForm", new AdminFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminCreate(AdminFormViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Password))
                ModelState.AddModelError(nameof(model.Password), "كلمة المرور مطلوبة عند الإنشاء");

            if (await _uow.Admins.IsUsernameUsedAsync(model.Username))
                ModelState.AddModelError(nameof(model.Username), "اسم المستخدم مستخدم بالفعل");

            if (!ModelState.IsValid) return View("AdminForm", model);

            var entity = _mapper.Map<Admin>(model);
            entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password!);
            entity.InitPassword = model.Password;
            entity.Status = AccountStatus.Active;

            await _uow.Admins.AddAsync(entity);
            await _uow.CommitAsync();

            TempData["Success"] = "تم إنشاء حساب المسؤول بنجاح";
            return RedirectToAction(nameof(Admins));
        }

        [HttpGet]
        public async Task<IActionResult> AdminEdit(Guid id)
        {
            var entity = await _uow.Admins.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted) return NotFound();

            var model = _mapper.Map<AdminFormViewModel>(entity);
            return View("AdminForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminEdit(AdminFormViewModel model)
        {
            if (await _uow.Admins.IsUsernameUsedAsync(model.Username, model.Id))
                ModelState.AddModelError(nameof(model.Username), "اسم المستخدم مستخدم بالفعل");

            if (!ModelState.IsValid) return View("AdminForm", model);

            var entity = await _uow.Admins.GetByIdAsync(model.Id!.Value);
            if (entity == null || entity.IsDeleted) return NotFound();

            _mapper.Map(model, entity);

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
                entity.InitPassword = model.Password;
            }

            _uow.Admins.Update(entity);
            await _uow.CommitAsync();

            TempData["Success"] = "تم تحديث بيانات المسؤول بنجاح";
            return RedirectToAction(nameof(Admins));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminToggleStatus(Guid id)
        {
            var currentAdminId = HttpContext.Session.GetString(AdminSessionHelper.AdminIdKey);
            if (id.ToString() == currentAdminId)
            {
                TempData["Error"] = "لا يمكنك إيقاف حسابك الحالي";
                return RedirectToAction(nameof(Admins));
            }

            var entity = await _uow.Admins.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted) return NotFound();

            entity.Status = entity.Status == AccountStatus.Active ? AccountStatus.Paused : AccountStatus.Active;
            _uow.Admins.Update(entity);
            await _uow.CommitAsync();

            TempData["Success"] = entity.Status == AccountStatus.Active ? "تم تفعيل المسؤول" : "تم إيقاف المسؤول";
            return RedirectToAction(nameof(Admins));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminDelete(Guid id)
        {
            var currentAdminId = HttpContext.Session.GetString(AdminSessionHelper.AdminIdKey);
            if (id.ToString() == currentAdminId)
            {
                TempData["Error"] = "لا يمكنك حذف حسابك الحالي";
                return RedirectToAction(nameof(Admins));
            }

            var entity = await _uow.Admins.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted) return NotFound();

            _uow.Admins.Delete(entity);
            await _uow.CommitAsync();

            TempData["Success"] = "تم حذف المسؤول";
            return RedirectToAction(nameof(Admins));
        }
    }
}
