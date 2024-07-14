using System;
using System.ComponentModel.DataAnnotations;

namespace MVCProject.PL.ViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }

        //public RoleViewModel()
        //{
        //    Id = Guid.NewGuid().ToString();
        //}
    }
}
