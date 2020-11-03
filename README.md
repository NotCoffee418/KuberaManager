# Kubera Manager
![Build status](https://github.com/notcoffee418/KuberaManager/workflows/KuberaManager/badge.svg)

### Automatic Session Management
Conditions for automatic sessions to fire:
- Computer: 
  - IsEnabled
  - Free client slot
- Account: 
  - IsEnabled, 
  - Start/stopTimeOfDay set to 0 & 23 (Needs testing, hours seem broken)
- Config: 
  - BrainEnabled: True
  - AllowRspeerApiCalls: True,
  - MaxHoursPerDay: enough
- Sessions: 
  - Account must not exceed Config.MaxHoursPerDay

One client will open every 60 seconds if the conditions are met

**WARN**: Launching through localhost might work but only on non-ssl and with an -apiUrl override (defined in config).
