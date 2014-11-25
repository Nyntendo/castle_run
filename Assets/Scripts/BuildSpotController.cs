using UnityEngine;
using System.Collections;

public class BuildSpotController : MonoBehaviour
{
    public GameObject selection;
    public Transform spawnPoint;
    public GameObject spawner;
    public int spawnerType;
    public Team team;
    public GameObject model;

    public void Select()
    {
        selection.SetActive(true);
    }

    public void Deselect()
    {
        selection.SetActive(false);
    }

    public void PlaceSpawner(GameObject spawner, int spawnerType)
    {
        model.SetActive(false);
        this.spawnerType = spawnerType;
        this.spawner = spawner;
        this.spawner.GetComponent<SpawnerController>().spawnPoint = spawnPoint.position;
        this.spawner.SendMessage("SetTeam", team);
    }

    public void RemoveSpawner()
    {
        model.SetActive(true);
        this.spawnerType = -1;
        this.spawner = null;
    }
}
