using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class GameManager
{
    public void Init()
    {
        UnityEngine.Debug.LogWarning("GameManager Init");
    }

    public void StartGame()
    {
        MonoManager.Managers.ChangeState("Game Start!!");
        MonoManager.Managers.UI.StartGameViewer();
    }

    public void StartTurn()
    {
        MonoManager.Managers.ChangeState("Turn Start!!");
        UnityEngine.Debug.LogWarning("Before");
        MonoManager.Managers.Map.Debug();
        var chip = MonoManager.Managers.Map.MakeChip();
        MonoManager.Managers.Map.GetTile(0, 0).Push(chip);
        MonoManager.Managers.UI.UpdateViewer();
        UnityEngine.Debug.LogWarning("After");
        MonoManager.Managers.Map.Debug();
    }
    
    public void EndTurn()
    {
        MonoManager.Managers.ChangeState("Turn End!!");
    }

    public void EndGame()
    {
        MonoManager.Managers.ChangeState("Game End");
    }
}
