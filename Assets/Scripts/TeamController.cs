using UnityEngine;
using System.Collections;

public class TeamController : MonoBehaviour
{
    public int coins = 0;
    public GameObject[] buildings;
    public int[] buildingCosts;
    public int income;
    public float incomeDelay;

    private float incomeTimer = 0f;

    public void Update()
    {
        incomeTimer += Time.deltaTime;

        if (incomeTimer >= incomeDelay)
        {
            coins += income;
            incomeTimer = 0f;
        }
    }
}
