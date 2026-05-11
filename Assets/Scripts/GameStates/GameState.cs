using UnityEngine;

public abstract class GameState
{
    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void Update();
}
