using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Data
{
    // WARNING: Hardcoded, existing IDs should not be changed
    // Check for duplicates before adding stuff
    // These are used by Database AccountCompletionData and Scenario Requirements
    public enum CompletionDataDefinition
    {
        Invalid = 0,
        TutorialComplete = 1,
        TwentyHoursPlaytime = 2,
        HunderedSkillPoints = 3,
    }
}
