﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public int Age { get; set; }

        public string Address { get; set; }

        public decimal Salary { get; set; }

        public bool IsActive { get; set; }

        
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string ImgName { get; set; }

        public DateTime HiringDate { get; set; }
        
        public DateTime CreationDate { get; set; } = DateTime.Now;


        [ForeignKey("Department")]
        public int? DeptId { get; set; }


        [InverseProperty("Employees")]
        public Department Department { get; set; }
    }
}
