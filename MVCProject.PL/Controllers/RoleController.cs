using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MVCProject.PL.ViewModels;
using Project.DAL.Models;

namespace MVCProject.PL.Controllers
{
	[Authorize]
	public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleController(RoleManager<IdentityRole> roleManager, IMapper mapper )
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string Searchvalue)
        {
            if (string.IsNullOrEmpty(Searchvalue))
            {
                var roles = await _roleManager.Roles.ToListAsync();
                var rolesMapper = _mapper.Map<IEnumerable<IdentityRole>, IEnumerable<RoleViewModel>>(roles);
                return View(rolesMapper);
            }
            else
            {
                var role = await _roleManager.FindByNameAsync(Searchvalue);
                var roleMapper = _mapper.Map<IdentityRole, RoleViewModel>(role);
                return View(new List<RoleViewModel>() { roleMapper });
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
		public async Task<IActionResult> Create(RoleViewModel model)
		{
			if (ModelState.IsValid)
			{
                model.Id = Guid.NewGuid().ToString();
				var RoleMapped = _mapper.Map<RoleViewModel, IdentityRole>(model);
				var result = await _roleManager.CreateAsync(RoleMapped);
				if (result.Succeeded)
					return RedirectToAction(nameof(Index));

				else
					foreach (var error in result.Errors)
						ModelState.AddModelError(string.Empty, error.Description);

			}
			return View(model);
		}


		public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
                return NotFound();
            var MappedRole = _mapper.Map<IdentityRole, RoleViewModel>(role);
            return View(ViewName, MappedRole);
        }

        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModel model)
        {
            if (id != model.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(id);
                    role.Name = model.Name;
                    await _roleManager.UpdateAsync(role);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception error)
                {
                    ModelState.AddModelError(string.Empty, error.Message);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }


        [HttpPost]
        public async Task<IActionResult> Delete(string Id, [FromRoute] string id)
        {
            if (id != Id)
                return BadRequest();

            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                await _roleManager.DeleteAsync(role);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception error)
            {
                ModelState.AddModelError(string.Empty, error.Message);
                return RedirectToAction("Error", "Home");
            }

        }

    }
}
