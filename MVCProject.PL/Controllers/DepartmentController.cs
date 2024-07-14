using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.Interfaces;
using Project.BLL.Repositories;
using Project.DAL.Models;

namespace MVCProject.PL.Controllers
{
	[Authorize]
	public class DepartmentController : Controller
    {
		private readonly IUnitOfWork _unitOfWork;

		//private readonly IDepartmentRepository _departmentRepository; // Composition relationship (HAS A) must not be NULL 
		public DepartmentController(/*IDepartmentRepository departmentRepository,*/ IUnitOfWork unitOfWork)
        {
			this._unitOfWork = unitOfWork;
			//_departmentRepository = departmentRepository;
		}

        public async Task<IActionResult> Index()
        {
            var department = await _unitOfWork._departmentRepository.GetAllAsync();
            return View(department);
        }

        public IActionResult Create()
		{
			return View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            if(ModelState.IsValid)
            {

				await _unitOfWork._departmentRepository.AddAsync(department);
				if (await _unitOfWork.SaveChangeAsync() > 0)
				{
					TempData["Message"] = "Added successfully";
					return RedirectToAction(nameof(Index));
				}
			}

            return View(department);
        }

        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var department = await _unitOfWork._departmentRepository.GetByIdAsync(id.Value);
            if(department == null)
                return NotFound();

            return View(ViewName,department);
        }

        public async Task<IActionResult> Edit(int? id)
		{
            return await Details(id,"Edit");
			//if (id is null)
			//	return BadRequest();
			//var department = _departmentRepository.GetById(id.Value);
			//if (department == null)
			//	return NotFound();
			//return View(department);
		}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Department department,[FromRoute] int id)
        {
            if(id!=department.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {

					_unitOfWork._departmentRepository.Update(department);
					if ( await _unitOfWork.SaveChangeAsync() > 0)
					{
						TempData["Message"] = "Edited successfully";
						return RedirectToAction(nameof(Index));

					}

				}
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(department);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(Department department, [FromRoute] int id)
		{
			if (id != department.Id)
				return BadRequest();

			if (ModelState.IsValid)
			{
				try
				{

					_unitOfWork._departmentRepository.Delete(department);
					if (await _unitOfWork.SaveChangeAsync() > 0)
                    {
						TempData["Message"] = "Deleted successfully";

						return RedirectToAction(nameof(Index));
                    }

				}
				catch (Exception ex)
				{
					ModelState.AddModelError(string.Empty, ex.Message);
				}
			}

			return View(department);
		}

	}
}
