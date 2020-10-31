using KuberaManager.Models.Data;
using KuberaManager.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Logic.ScenarioLogic.Scenarios
{
    public class Quest : ScenarioBase
    {
        public Quest(int varp, string questName, bool isFreeToPlay, CompletionDataDefinition def) 
            : base()
        {
            ScenarioName = "Quest"; // For scenario
            QuestName = questName; // assigns to Argument
            Varp = varp;
            IsFreeToPlay = isFreeToPlay;
            CompletionDefinition = def;

            // Quests should always run until completion
            AlwaysRunsUntilComplete = true;
        }

        public virtual string QuestName
        {
            get { return ScenarioArgument; }
            set { ScenarioArgument = value; }
        }
        public bool IsFreeToPlay { get; set; }
        public int Varp { get; set; }
        public CompletionDataDefinition CompletionDefinition { get; set; }

        public bool IsCompletedByAccount(Account acc)
        {
            return acc.HasDefinition(CompletionDefinition);
        }
    }
}
