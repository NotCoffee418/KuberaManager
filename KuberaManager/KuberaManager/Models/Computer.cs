using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models
{
    public class Computer
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Hostname{ get; set; }

        [Required]
        [DefaultValue(3)]
        [Display(Name = "Max Clients", Description = "0 for unlimited")]
        public int MaxClients { get; set; }

        [DefaultValue(true)]
        public bool LowCpuMode { get; set; }

        [DefaultValue(true)]
        public bool SuperLowCpuMode { get; set; }

        [DefaultValue(true)]
        public bool DisableModelRendering { get; set; }

        [DefaultValue(true)]
        public bool DisableSceneRendering { get; set; }
    }
}
