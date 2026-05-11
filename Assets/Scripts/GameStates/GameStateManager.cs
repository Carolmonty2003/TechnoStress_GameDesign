using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    private GameState gameState;
    private Dictionary<string, GameState> gameStates = new Dictionary<string, GameState>
    {
        { "Menu", new Menu() },
        { "Daytime", new Daytime() },
        { "Result", new Result() },
        { "Summary", new Summary() },
        { "GameEnd", new GameEnd() }
    };

    void Start()
    {
        SetState("Daytime");
    }

    void Update()
    {
        gameState?.Update();
    }

    public void SetState(string state)
    {
        gameState?.OnExit();
        gameState = gameStates[state];
        gameState?.OnEnter();
    }
}
