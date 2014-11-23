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

    public void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
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
            var unit = Instantiate(
                    unitToSpawn,
                    spawnPoint,
                    Quaternion.identity) as GameObject;

            unit.SendMessage("SetTeam", team);

            spawnTimer = 0f;
        }
    }
}
