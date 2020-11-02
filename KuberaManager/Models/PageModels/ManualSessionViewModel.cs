using KuberaManager.Models.Database;
using KuberaManager.Models.Logic.ScenarioLogic.Scenarios;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.PageModels
{
    public class ManualSession
    {
        [Key]
        public int AccountId { get; set; }
        public int ComputerId { get; set; }
        public string SelectedScenario { get; set; }
    }
}
