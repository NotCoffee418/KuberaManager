using KuberaManager.Models.Database;
using KuberaManager.Models.Logic.ScenarioLogic.Requirements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Logic.ScenarioLogic.Scenarios
{
    public abstract class ScenarioBase
    {
        // Properties
        public string Identifier { get { return $"{ScenarioName}.{ScenarioArgument}".Replace(" ", "_"); } }
        public string ScenarioName { get; set; }
        public string ScenarioArgument { get; set; }
        public List<IRequirement> Requirements { get; set; }
            = new List<IRequirement>() // Base requirements for any other task
            {
                new TutorialComplete(), // careful that it doesn't set itself as a requirmeent!

            };
        public bool AlwaysRunsUntilComplete { get; set; } = false;

        // Methods
        public override string ToString()
        {
            return Identifier;
        }
    }
}
