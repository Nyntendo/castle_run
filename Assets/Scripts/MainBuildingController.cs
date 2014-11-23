using UnityEngine;
using System.Collections;

public class MainBuildingController : MonoBehaviour
{
    private GameController gameController;
    private Attackable attackable;

    public void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        attackable = GetComponent<Attackable>();
    }

    public void OnDeath()
    {
        gameController.MainBuildingDestroyed(attackable.team);
    }
}
