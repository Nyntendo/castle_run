using UnityEngine;
using System.Collections;

public class SimpleEnemyAI : MonoBehaviour
{
    public BuildSpotController[] buildSpots;
    public TeamController team;
    public int[] buildQueue;
    
    private int buildQueueIndex = 0;
    private int buildSpotIndex = 0;

    public void Update()
    {
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
