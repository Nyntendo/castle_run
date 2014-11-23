using UnityEngine;
using System.Collections;

public class TeamController : MonoBehaviour
{
    public int coins = 0;
    public GameObject mainBuilding;
    public GameObject[] buildings;
    public int[] buildingCosts;
    public int income;
    public float incomeDelay;
    public int currentNumberOfUnits = 0;

    private float incomeTimer = 0f;
    private GameController gameController;

    public void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    public void Update()
    {
        if (gameController.gameState != GameState.Playing)
            return;

        incomeTimer += Time.deltaTime;

        if (incomeTimer >= incomeDelay)
        {
            coins += income;
            incomeTimer = 0f;
        }
    }
}
