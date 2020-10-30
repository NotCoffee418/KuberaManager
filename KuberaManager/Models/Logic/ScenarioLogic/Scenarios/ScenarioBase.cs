﻿using KuberaManager.Models.Database;
using KuberaManager.Models.Logic.ScenarioLogic.Requirements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Logic.ScenarioLogic.Scenarios
{
    public abstract class ScenarioBase
    {
        public string Name { get; set; }
        public string Argument { get; set; }
        public List<IRequirement> Requirements { get; set; }
    }
}