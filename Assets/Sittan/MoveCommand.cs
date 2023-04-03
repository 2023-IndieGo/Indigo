using System.Collections;
using UnityEngine;

public class MoveCommand : Command
{
    GameObject target;
    Vector2 previousPosition;
    Vector2 purposePosition;

    public MoveCommand(GameObject target, Vector2 purposePosition)
    {
        this.target = target;
        this.purposePosition = purposePosition;

        this.previousPosition = target.transform.position;
    }

    public override void Do()
    {
        target.transform.position = purposePosition;
    }

    public override void UnDo()
    {
        target.transform.position = previousPosition;
    }
}