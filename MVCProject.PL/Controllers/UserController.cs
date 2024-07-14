﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCProject.PL.ViewModels;
using Project.DAL.Models;

namespace MVCProject.PL.Controllers
{
	[Authorize]
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IMapper _mapper;

		public UserController(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
			_userManager = userManager;
			_mapper = mapper;
		}


        public async Task<IActionResult> Index(string SearchValue)
		{
			if (string.IsNullOrEmpty(SearchValue))
			{
				var users = await _userManager.Users.Select(
					u => new UserViewModel()
					{
						Id = u.Id,
						FName = u.FName,
						LName = u.LName,
						PhoneNumber = u.PhoneNumber,
						Email = u.Email,
						Roles = _userManager.GetRolesAsync(u).Result,
					}).ToListAsync();
				return View(users);
			}
			//Pa$$w0rd
			else
			{
				var user = await _userManager.FindByEmailAsync(SearchValue);
				if (user is not null)
				{
					var MappedUser = new UserViewModel()
					{
						Id = user.Id,
						FName = user.FName,
						LName = user.LName,
						PhoneNumber = user.PhoneNumber,
						Email = user.Email,
						Roles = _userManager.GetRolesAsync(user).Result,
					};
					return View(new List<UserViewModel> { MappedUser });
				}
				else 
					return NotFound();
			}
		}

		public async Task<IActionResult> Details(string id, string ViewName = "Details")
		{
			if (id is null)
				return BadRequest();
			var user = await _userManager.FindByIdAsync(id);
			if (user is null)
				return NotFound();
			var MappedUser = _mapper.Map<ApplicationUser, UserViewModel>(user);
			return View(ViewName, MappedUser);
		}

		public async Task <IActionResult> Edit(string id)
		{
			return await Details(id, "Edit");
		}

		[HttpPost]
		public async Task<IActionResult> Edit([FromRoute] string id, UserViewModel model)
		{
			if(id != model.Id)
				return BadRequest();
			if (ModelState.IsValid)
			{
				try
				{
					var user = await _userManager.FindByIdAsync(id);
					user.FName = model.FName;
					user.LName = model.LName;
					user.PhoneNumber = model.PhoneNumber;
					await _userManager.UpdateAsync(user);

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
                var user = await _userManager.FindByIdAsync(id);
                await _userManager.DeleteAsync(user);

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
