using KuberaManager.Models.Database;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Data
{
    public class AdminPassword
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password cannot be empty.")]
        [StringLength(30, MinimumLength = 8)]
        public string AdminPassPlain { get; set; }


        private PasswordHasher<string> pw = new PasswordHasher<string>();

        public string GetPasswordHash()
        {
            return BCrypt.Net.BCrypt.HashPassword(AdminPassPlain);
        }

        public bool IsCorrectPassword()
        {
            string passwordHash = Config.Get<string>("AdminPassHash");
            if (passwordHash == null)
                return false;
            else return BCrypt.Net.BCrypt.Verify("Pa$$w0rd", passwordHash);
        }
    }
}
