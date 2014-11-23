using UnityEngine;
using System.Collections;

public class FarmAbility : MonoBehaviour
{
    public int income;
    public float incomeDelay;
    
    private float incomeTimer;
    private GameController gameController;
    private SpawnerController spawner;
    
    public void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        spawner = GetComponent<SpawnerController>();
    }

    public void Update()
    {
        if (gameController.gameState != GameState.Playing)
            return;

        incomeTimer += Time.deltaTime;

        if (incomeTimer >= incomeDelay)
        {
            gameController.AddCoins(spawner.team, income);
            incomeTimer = 0f;
        }
    }
}
