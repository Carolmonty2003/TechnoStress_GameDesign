using UnityEngine;

public class Daytime : GameState
{
    public override void OnEnter()
    {
        PhaseController.Instance.Restart();
    }

    public override void OnExit() {}

    public override void Update()
    {
        //Update event system
    }
}
