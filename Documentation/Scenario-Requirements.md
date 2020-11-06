If you haven't already, you're better off reading [Adding Scenarios](https://github.com/NotCoffee418/KuberaManager/blob/main/Documentation/Adding-Scenarios.md) first.

### Implemented Requirements you can use
There are currently only a few [Requirement classes](https://github.com/NotCoffee418/KuberaManager/tree/main/KuberaManager/Logic/ScenarioLogic/Requirements) and they all enforce the interface [IRequirement](https://github.com/NotCoffee418/KuberaManager/blob/main/KuberaManager/Logic/ScenarioLogic/Requirements/IRequirement.cs).  
They can be dynamic but aren't necessarily dynamic.  
Here is a list of all current requirements and their accepted parameters.
- **SkillRequirement(Skill skill, int requiredLevel)**  
Requires that you have a specified skill at a certain level or will train it until it is that level.
- **TutorialComplete()**  
- **TwentyHoursPlayed()**  
Will do random F2P quests until the threshhold is reached.
- **HasHunderedSkillPoints()**  
Will do random F2p quests until threshhold is reached (might be changed, doesn't seem optimal...)

- **QuestComplete(QuestScenario quest)**  
Requires that the quest is complete or go do it.

### Adding existing requirements
Requirements are preferably added in the `C` function of the relevant [Assigner class](https://github.com/NotCoffee418/KuberaManager/tree/main/KuberaManager/Logic/ScenarioLogic/Scenarios/Assigners).

To add a requirement to a scenario it should be added to the Requirements list (defined in ScenarioBase) like so:  
`theScenario.Requirements.Add(new SkillRequirement(Skill.Woodcutting, 15));`

That code would mean that the scenario will not run unless our woodcutting is level 15 or higher.  
But instead, we would train woodcutting until we reached level 15.

##### Example
```
private static SkillScenario C(Skill skill, string argument, int minLevel = 0)
{
    SkillScenario ss = new SkillScenario(skill, argument, minLevel);

    switch (argument)
    {
        case "BarbLogs":
            ss.Requirements.Add(new HasHunderedSkillPoints());
            break;
        case "LumbridgeCopper":
            AlwaysRunUntilComplete = true;
            break;
    }

    return ss;
}
```

Make sure the Switch statement targets the variable you're trying to assess.

### Creating a new requirement
Create a class in the Requirements directory.  
Make it enforce IRequirement and use the following information to implement it.  
But feel free to ask for help on this one because it can get tricky.

- **IsPriority**  
When true it will always attempt to fulfill this requirement first.  
When false, requirements are shuffled if there are multiple.  
- **bool DoesMeetCondition(Account acc)**  
This function is called automatically to determine if we're allowed to run the scenario.
- **ScenarioBase GetFulfillScenario(Account acc)**  
This returns the scenario that should be run instead if `DoesMeetCondition()` returns false.
