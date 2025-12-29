using BlackboardSystem;
using UnityEngine;
using System.Collections.Generic;
using System;

public class HealthExpert : Expert
{
    public override void Execute(Blackboard blackboard)
    {
        base.Execute(blackboard);
    }

    public override int GetInsistence(Blackboard blackboard)
    {
        base.GetInsistence(blackboard);
        return 0;
    }
}
