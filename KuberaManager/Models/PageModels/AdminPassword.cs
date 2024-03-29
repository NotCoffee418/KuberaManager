﻿using KuberaManager.Logic;
using KuberaManager.Models.Database;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KuberaManager.Models.PageModels
{
    public class AdminPassword
    {
        [Display(Name = "Administrator Password")]
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
            ConfigHelper ch = new ConfigHelper();
            string passwordHash = ch.Get<string>("AdminPassHash");
            if (passwordHash == null)
                return false;
            else return BCrypt.Net.BCrypt.Verify(AdminPassPlain, passwordHash);
        }
    }
}
