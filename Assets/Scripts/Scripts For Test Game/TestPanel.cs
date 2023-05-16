using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPanel : BasePanel
{
    public override void Enter()
    {
        base.Enter();
        Debug.Log("TestPanel Push in Stack");
    }
}
