using UnityEngine;
using System.Collections;

public enum Team
{
    Player,
    Enemy
}

public class SpawnerController : MonoBehaviour
{
    public GameObject unitToSpawn;
    public float spawnDelay;
    public Team team;
    public Vector3 spawnPoint;

    private float spawnTimer;
    private GameController gameController;
    private StatisticsController stats;

    public void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        stats = GameObject.FindWithTag("StatisticsController").GetComponent<StatisticsController>();
    }

    public void SetTeam(Team team)
    {
        this.team = team;
    }

    public void Update()
    {
        if (gameController.gameState != GameState.Playing)
            return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnDelay && gameController.CanSpawn(team))
        {
            stats.LogSpawn(team, unitToSpawn.name);
            var unit = Instantiate(
                    unitToSpawn,
                    spawnPoint,
                    Quaternion.identity) as GameObject;

            unit.SendMessage("SetTeam", team);

            spawnTimer = 0f;
        }
    }
}
