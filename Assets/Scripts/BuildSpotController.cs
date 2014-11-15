using UnityEngine;
using System.Collections;

public class BuildSpotController : MonoBehaviour
{
    public GameObject selection;
    public Transform spawnPoint;
    public GameObject spawner;

    public void Select()
    {
        selection.SetActive(true);
    }

    public void Deselect()
    {
        selection.SetActive(false);
    }

    public void PlaceSpawner(GameObject spawner)
    {
        this.spawner = spawner;
        this.spawner.GetComponent<SpawnerController>().spawnPoint = spawnPoint.position;
    }
}
