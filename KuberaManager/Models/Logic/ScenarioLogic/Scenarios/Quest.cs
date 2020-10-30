﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KuberaManager.Models.Logic.ScenarioLogic.Scenarios
{
    public class Quest : ScenarioBase
    {
        public Quest(int varp, string questName, bool isFreeToPlay) 
            : base()
        {
            ScenarioName = "Quest"; // For scenario
            QuestName = questName; // assigns to Argument
            Varp = varp;
            IsFreeToPlay = IsFreeToPlay;
        }

        public virtual string QuestName
        {
            get { return ScenarioArgument; }
            set { ScenarioArgument = value; }
        }
        public bool IsFreeToPlay { get; set; }
        public int Varp { get; set; }
    }
}