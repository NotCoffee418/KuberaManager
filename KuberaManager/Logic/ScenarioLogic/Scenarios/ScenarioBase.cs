using KuberaManager.Models.Database;
using KuberaManager.Logic.ScenarioLogic.Requirements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Logic.ScenarioLogic.Scenarios
{
    public abstract class ScenarioBase
    {
        // Properties
        public string Identifier { get { return $"{ScenarioName}.{ScenarioArgument}".Replace(" ", "_"); } }
        public string ScenarioName { get; set; }
        public string ScenarioArgument { get; set; }

        // These requirements must be met for the scenario to be able to run
        public List<IRequirement> Requirements { get; set; }
            = new List<IRequirement>() // Base requirements for any other task
            {
                new TutorialComplete(), // careful that it doesn't set itself as a requirmeent!

            };

        // Determines if a scenario should run until it is complete
        public bool AlwaysRunsUntilComplete { get; set; } = false;

        // Nullable. Determines the minimum and maximum time a scenario should run.
        // Will be selected at random based on these values.
        // WARN: Can be overridden by AlwaysRunsUntilComplete
        public TimeSpan MinimumRunTime { get; set; } = TimeSpan.FromMinutes(20);
        public TimeSpan MaximumRunTime { get; set; } = TimeSpan.FromMinutes(60);

        // Methods
        public override string ToString()
        {
            return Identifier;
        }
    }
}
