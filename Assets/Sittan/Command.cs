using System.Collections;
using UnityEngine;

public abstract class Command
{
    public Command() { }
    public abstract void Do();
    public abstract void UnDo();

}