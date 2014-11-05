using UnityEngine;
using System.Collections;

public enum Team
{
    Pink,
    Yellow
}

public class SpawnerController : MonoBehaviour
{
    public GameObject unitToSpawn;
    public float spawnDelay;
    public Team team;

    private float spawnTimer;
    private GameObject target;

    public void Awake()
    {
        if (team == Team.Pink)
        {
            target = GameObject.FindWithTag("YellowBase");
        }
        else
        {
            target = GameObject.FindWithTag("PinkBase");
        }
    }

    public void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnDelay)
        {
            var unit = Instantiate(
                    unitToSpawn,
                    transform.position + Vector3.forward * 2 * ((team == Team.Pink)?-1:1),
                    Quaternion.identity) as GameObject;

            if (target != null)
                unit.SendMessage("SetTarget", target.transform);

            unit.SendMessage("SetTeam", (int)team);

            spawnTimer = 0f;
        }
    }
}
