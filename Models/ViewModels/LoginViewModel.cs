using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public partial class LoginViewModel
    {
        private bool? rememberMe;

        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Display(Name = "Remember me?")]
        public bool? RememberMe
        {
            get => rememberMe;
            set => rememberMe = value ?? false;
        }
      
    }
}
