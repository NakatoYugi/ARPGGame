using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Pause() { }
    public virtual void Resume() { }
}
