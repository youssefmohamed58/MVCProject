using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MVCProject.PL.MappingProfiles;
using Project.BLL.Interfaces;
using Project.BLL.Repositories;
using Project.DAL.Contexts;
using Project.DAL.Models;

namespace MVCProject.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
			// CreateHostBuilder(args).Build().Run();

			var Builder = WebApplication.CreateBuilder(args);

			#region ConfigureServices (Dependancy Injection)

			Builder.Services.AddControllersWithViews();

			Builder.Services.AddDbContext<MVCDbContext>(option =>
			{
				option.UseSqlServer(Builder.Configuration.GetConnectionString("DefaultConnection"));
			}); // Allow Dependancy Injection into MVCDbContext Objects

			Builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

			//services.AddScoped<IEmployeeRepository, EmployeeRepository>();

			Builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

			Builder.Services.AddAutoMapper(M => M.AddProfiles(new List<Profile>() { new EmployeeProfile(), new UserProfile(), new RoleProfile() }));

			Builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
			{
				options.Password.RequireNonAlphanumeric = true;
				options.Password.RequireUppercase = true;
				options.Password.RequireLowercase = true;
				options.Password.RequireDigit = true;
			})
				.AddEntityFrameworkStores<MVCDbContext>()
				.AddDefaultTokenProviders();
			// P@ssww0rd
			Builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
			{
				options.AccessDeniedPath = "Home/Error";
				options.LoginPath = "Account/Login";
			});

			#endregion

			var app = Builder.Build();

			#region Configure

			if (app.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Account}/{action=Login}/{id?}");
			});

			#endregion

			app.Run();
		}


	}
}
