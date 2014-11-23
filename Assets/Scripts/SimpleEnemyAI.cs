using UnityEngine;
using System.Collections;

public class SimpleEnemyAI : MonoBehaviour
{
    public BuildSpotController[] buildSpots;
    public int[] buildQueue;

    private TeamController team;
    private int buildQueueIndex = 0;
    private int buildSpotIndex = 0;
    private GameController gameController;

    public void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        this.team = gameController.enemyTeam;
    }

    public void Update()
    {
        if (gameController.gameState != GameState.Playing)
            return;

        if (buildSpotIndex >= buildSpots.Length)
        {
            return;
        }

        var buildingIndex = buildQueue[buildQueueIndex];
        if (team.coins >= team.buildingCosts[buildingIndex])
        {
            Build(buildingIndex);
        }
    }

    private void Build(int i)
    {
        var buildSpot = buildSpots[buildSpotIndex];
        team.coins -= team.buildingCosts[i];
        var building = Instantiate(team.buildings[i], buildSpot.transform.position, Quaternion.identity) as GameObject;
        buildSpot.PlaceSpawner(building, i);
        buildSpotIndex++;
        buildQueueIndex++;
    }
}
