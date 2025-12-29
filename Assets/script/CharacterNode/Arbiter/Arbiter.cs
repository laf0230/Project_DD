using UnityEngine;
using System.Collections.Generic;
using System;

namespace BlackboardSystem
{
 public class Arbiter
    {
        readonly List<IExpert> experts = new();

        public void RegisterExpert(IExpert expert)
        {
            experts.Add(expert);
        }

        public void DeRegisterExpert(IExpert exepert)
        {
            experts.Remove(exepert);
        }

        public List<Action> BlackboardIteration(Blackboard blackboard)
        {
            IExpert bestExpert = null;
            int hightestInsistence = 0;

            foreach (IExpert expert in experts)
            {
                int insistence = expert.GetInsistence(blackboard);
                if (insistence > hightestInsistence)
                {
                    hightestInsistence = insistence;
                    bestExpert = expert;
                }
            }

            if (bestExpert != null)
            {
                Debug.Log($"[Arbiter] Winner: {bestExpert.GetType().Name} with score {hightestInsistence}");
                bestExpert.Execute(blackboard);
            }
            bestExpert?.Execute(blackboard);

            var actions = new List<Action>(blackboard.PassedActions);
            blackboard.ClearActions();

            // Return or execute the actions here
            return actions;
        }
    }
 }
