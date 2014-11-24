using UnityEngine;
using System.Collections;

public enum Race
{
    Human,
    Undead
}

public class MatchController : MonoBehaviour
{
    public Race playerRace;
    public Race enemyRace;

    public GameObject humanTeamPrefab;
    public GameObject undeadTeamPrefab;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public GameObject CreatePlayerTeam()
    {
        GameObject prefab = null;

        if (playerRace == Race.Human)
        {
            prefab = humanTeamPrefab;
        }
        else
        {
            prefab = undeadTeamPrefab;
        }

        return Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
    }

    public GameObject CreateEnemyTeam()
    {
        GameObject prefab = null;

        if (enemyRace == Race.Human)
        {
            prefab = humanTeamPrefab;
        }
        else
        {
            prefab = undeadTeamPrefab;
        }

        return Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
    }
}
