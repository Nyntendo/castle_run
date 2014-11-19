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
    private GameObject target;

    public void SetTeam(Team team)
    {
        this.team = team;

        if (team == Team.Player)
        {
            target = GameObject.FindWithTag("EnemyBase");
        }
        else
        {
            target = GameObject.FindWithTag("PlayerBase");
        }
    }

    public void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnDelay)
        {
            var unit = Instantiate(
                    unitToSpawn,
                    spawnPoint,
                    Quaternion.identity) as GameObject;

            if (target != null)
                unit.SendMessage("SetTarget", target.transform);

            unit.SendMessage("SetTeam", team);

            spawnTimer = 0f;
        }
    }
}
