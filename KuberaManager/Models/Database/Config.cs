using KuberaManager.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Database
{
    public class Config
    {

        [Key]
        public string ConfKey { get; set; }

        [AllowNull]
        public string ConfValue { get; set; }


        // See Logic.ConfigHelper for methods
    }
}
