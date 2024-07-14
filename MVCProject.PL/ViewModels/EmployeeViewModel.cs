using Project.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace MVCProject.PL.ViewModels
{
	public class EmployeeViewModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Name Is Required :(")]
		[MaxLength(50, ErrorMessage = "Max Length Of Name Is 50 Chars")]
		[MinLength(3, ErrorMessage = "Min Length Of Name Is 3 Chars")]
		public string Name { get; set; }

		[Range(22, 30, ErrorMessage = "Age must be between 22 and 30")]
		public int Age { get; set; }

		[RegularExpression("^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$",
			ErrorMessage = "Address must be like => 123-Street-City-Country")]
		public string Address { get; set; }

		[DataType(DataType.Currency)]
		public decimal Salary { get; set; }

		public bool IsActive { get; set; }

		[EmailAddress]
		public string Email { get; set; }

		[Phone]
		public string PhoneNumber { get; set; }
        public string ImgName { get; set; }
        public IFormFile Image { get; set; }
        public DateTime HiringDate { get; set; }


		[ForeignKey("Department")]
		public int? DeptId { get; set; }

		[InverseProperty("Employees")]
		public Department Department { get; set; }
	}
}
