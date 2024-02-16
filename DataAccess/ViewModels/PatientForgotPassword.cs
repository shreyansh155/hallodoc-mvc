using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class PatientForgotPassword
    {
        [Required]
        public string Email { get; set; }

    }
}
