# Kubera Manager
![Build status](https://github.com/notcoffee418/KuberaManager/workflows/KuberaManager/badge.svg)  
KuberaManager is a back-end and task assignment system for a multi-account bot system, running on multiple devices.  
It schedules which bot runs when with which task. Assignment is random from predefined tasks.  
This application is not fully complete and needs to be connected to a bot system to work at all.

### Web UI Preview
![KuberaManager preview](https://user-images.githubusercontent.com/9306304/157995489-f236f0a0-52af-41c8-b6d6-37b2351816fa.png)

### Automatic Session Management
Conditions for automatic sessions to fire:
- Computer: 
  - IsEnabled
  - Free client slot
- Account: 
  - IsEnabled, 
  - Start/stopTimeOfDay set to 0 & 23
- Config: 
  - BrainEnabled: True
  - AllowRspeerApiCalls: True,
  - MaxHoursPerDay: enough
- Sessions: 
  - Account must not exceed Config.MaxHoursPerDay

One client will open every 60 seconds if the conditions are met

**WARN**: Launching through localhost might work but only on non-ssl and with an -apiUrl override (defined in config).
