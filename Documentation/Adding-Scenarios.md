# Adding Scenarios
This is a quick explanation on how to define scenarios in KuberaManager so that they can be randomly selected as a moneymaker or used as a requirement.
### tl;dr:
1. Create scenario in the `All*Scenarios` property of the correct [Assigner](https://github.com/NotCoffee418/KuberaManager/tree/main/KuberaManager/Logic/ScenarioLogic/Scenarios/Assigners).
2. (Quests only) Add a [CompletionDataDefinition](https://github.com/NotCoffee418/KuberaManager/blob/main/KuberaManager/Models/Data/CompletionDataDefinition.cs) for the quest.
3. Specify additional [requirements](https://github.com/NotCoffee418/KuberaManager/blob/main/Documentation/Scenario-Requirements.md) if needed.

### Overview
Most of the scenario logic is located at [KuberaManager.Logic.ScenarioLogic](https://github.com/NotCoffee418/KuberaManager/tree/main/KuberaManager/Logic/ScenarioLogic).  

### ScenarioBase
There are different scenario types.

- QuestScenario
- SkillScenario
- (more coming soon)

These are all based on ScenarioBase and inherit it's properties.  
Meaning that you could call `SkillScenario.Identifier]`, despite this property not existing directly in SkillScenario.

These properties exist in every scenario:
- **Identifier** (read-only)  
This is a string formatted as `ScenarioName.ScenarioArgumant`.  
For example: "Quest.TUTORIAL_ISLAND".

- **ScenarioName**  
This is currently called *Scenario* in our jar and defines the type of scenario we run. eg. Quest, Woodcutting, Mining, Fighter.  
This value is automatically set when creating a Type of scenario such as QuestScenario.  

- **ScenarioArgument**  
This value is the *Argument* as it's called in our jar.  
This will usually define the location or specific sub-scenario by name.  
Both ScenarioName and ScenarioArgument must conform to ScenarioManager in our jar.  

- **AlwaysRunsUntilComplete**  
This boolean defines if an account should continue this scenario the next time it logs in if the task is still ongoing while the session ends.  
Usually this should be *false* unless we're dealing with a long, linear series of events such as quests.  


- **Minimum / Maximum RunTime**  
These are set by default in ScenarioBase (currently to 20-60 minutes).
You can override these if it's approperiate for the situation.


### Quests
Example request: **Quest.TUTORIAL_ISLAND**  
As described, this inherits all the properties of ScenarioBase and has the following additional properties:

- **QuestName** (This is the same value as ScenarioArgument)
- **IsFreeToPlay**
- **Varp** The quest VARP as defined by runescape
- **CompletionDefinition** See below for more info


##### Creating a CompletionDataDefinition
Quests rely on these to mark when an account has completed.  
Along with whatever other once-in-an-accounts-lifetime booleans you want to define with them.

The magic happens in [KuberaManager.Models.Data.CompletionDataDefinition](https://github.com/NotCoffee418/KuberaManager/blob/main/KuberaManager/Models/Data/CompletionDataDefinition.cs).
Its just a big enum. You only need to do one thing.
All you need to do is add a line like this at the end of the enum:  
`QUEST_NEW_QUESTS_NAME = xx`  
with xx being one number higher than the previous entry.


##### Adding quests
Navigate to [KuberaManager.Logic.ScenarioLogic.Scenarios.Assigners.QuestAssigner](https://github.com/NotCoffee418/KuberaManager/blob/12db67f27a7e347cec6086e8603e78e24cfe48f0/KuberaManager/Logic/ScenarioLogic/Scenarios/Assigners/QuestAssigner.cs#L22).

All you really need to do is add a line under `AllQuests`
```
public static List<Quest> AllQuests
{
    get
    {
        if (_allQuestIdentifiers == null)
        {
            _allQuestIdentifiers = new List<Quest>()
            {
                // Free to play
                C(281, "TUTORIAL_ISLAND", true, CompletionDataDefinition.TutorialComplete),
                C(130, "BLACK_KNIGHTS_FORTRESS", true, CompletionDataDefinition.QUEST_BLACK_KNIGHTS_FORTRESS),
                ... here ...

                // Members
                ... or here ...
            }
        }
    }
}
```

`C` is really just a shortcut to save a bit of screenspace. The function you call is:  
`C(int varp, string questName, bool isFreeToPlay, CompletionDataDefinition def)`  



### Skills
Example request: **Woodcutting.MapleLogs**

These are simpler to add, but as you may notice are structurally ***different***.  
The request doesn't include "Skill", but directly defines which skill.  
And the argument is used to specify the location or type of log/ore/craft to do.  

They also inherit from ScenarioBase and use the [SkillAssigner](https://github.com/NotCoffee418/KuberaManager/blob/main/KuberaManager/Logic/ScenarioLogic/Scenarios/Assigners/SkillAssigner.cs) class instead to list all scenarios.

New skills need to be manually added to the list like so in :
```
public static List<SkillScenario> AllSkillScenarios
{
    get
    {
        if (_allSkillScenarios == null)
            _allSkillScenarios = new List<SkillScenario>()
            {
                // Woodcutting
                C(Skill.Woodcutting, "BarbLogs", 0), // Skill requirement optional
                
                // Mining
                C(Skill.Mining, "LocationOreName"),
            };
        return _allSkillScenarios;
    }
}
```
C is also a shortcut. it takes arguments like so:  
`C(Skill skill, string argument, int minLevel = 0)`

[Skill](https://github.com/NotCoffee418/KuberaManager/blob/main/KuberaManager/Models/Data/Runescape/Skill.cs) is an enum which lists each runescape skill.

### Further reading
These are the only ones implemented or documented for now but should give an idea.  
You should also read up on [how to add scenario requirements](https://github.com/NotCoffee418/KuberaManager/blob/main/Documentation/Scenario-Requirements.md) if your scenario is depended on a skill being leveled or quest being completed.
