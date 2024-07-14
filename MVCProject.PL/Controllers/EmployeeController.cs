using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCProject.PL.Helper;
using MVCProject.PL.ViewModels;
using Project.BLL.Interfaces;
using Project.BLL.Repositories;
using Project.DAL.Models;


namespace MVCProject.PL.Controllers
{
	[Authorize]
	public class EmployeeController : Controller
    {
        //private readonly IEmployeeRepository _employeeRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

        //public readonly IDepartmentRepository _departmentRepository;

        public EmployeeController(/*IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository*/
			IUnitOfWork UnitOfWork
            ,IMapper mapper)
        {
			_unitOfWork = UnitOfWork;
			_mapper = mapper;
            //_employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository;
        }


		public async Task<IActionResult> Index(string SearchValue)
		{
			IEnumerable<Employee> employee;

			if (string.IsNullOrEmpty(SearchValue))
				employee = await _unitOfWork._employeeRepository.GetAllAsync();
			else
				employee = _unitOfWork._employeeRepository.GetEmployeeByName(SearchValue);
			var EmpVm = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employee);
			return View(EmpVm);
		}

		public IActionResult Create()
        {
            //ViewBag.departments = _departmentRepository.GetAll();
            return View();
        }

		public async Task<IActionResult> Details(int? id, string view = "Details")
        {
            if (id == null)
                return BadRequest();
            var employee = await _unitOfWork._employeeRepository.GetByIdAsync(id.Value);
            if(employee == null)
                return NotFound();
            var EmpVm = _mapper.Map<Employee, EmployeeViewModel>(employee);
            return View(view, EmpVm);
        }

        public async Task<IActionResult> Edit(int? id)
        {
			//ViewBag.departments = _departmentRepository.GetAll();
			return await Details(id, "Edit");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
		{
			if (ModelState.IsValid)
			{
                employeeVM.ImgName = DocumentSettings.UploadFile(employeeVM.Image, "Images");

                var Emp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                await _unitOfWork._employeeRepository.AddAsync(Emp);

				if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    TempData["Message"] = "Added successfully";
					return RedirectToAction(nameof(Index));
                }
			}
			return View(employeeVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(EmployeeViewModel employeeVM, [FromRoute] int id)
        {
            if(employeeVM.Id !=  id)
                return BadRequest();

			if (ModelState.IsValid)
			{
				try
				{
                    if(employeeVM.Image is not null)
						employeeVM.ImgName = DocumentSettings.UploadFile(employeeVM.Image, "Images");

                    var Emp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                    _unitOfWork._employeeRepository.Update(Emp);

					if (await _unitOfWork.SaveChangeAsync() > 0)
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
            return View(employeeVM);
        }

        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(EmployeeViewModel employeeVM, [FromRoute] int id)
        {
            if (id != employeeVM.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var Emp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                    _unitOfWork._employeeRepository.Delete(Emp);

					if (await _unitOfWork.SaveChangeAsync() > 0 && employeeVM.ImgName is not null)
					{
						DocumentSettings.DeleteFile("Images",employeeVM.ImgName);
					}
					TempData["Message"] = "Deleted successfully";

					return RedirectToAction(nameof(Index));
				}
				catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(employeeVM);
        }
    }
}
