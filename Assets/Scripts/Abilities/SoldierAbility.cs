using UnityEngine;
using System.Collections;

public class SoldierAbility : MonoBehaviour
{
    public float cooldown;
    public int speedBoost;

    private float cooldownTimer;
    private bool isBoosted = false;
    private UnitController unitController;
    private GameController gameController;

    public void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        unitController = GetComponent<UnitController>();
    }

    public void Update()
    {
        if (gameController.gameState != GameState.Playing)
            return;

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (cooldownTimer <= 0f && unitController.GetTargetObject() != null)
        {
            unitController.SetWalkAnimationSpeed(3f);
            unitController.speed += speedBoost;
            isBoosted = true;
            cooldownTimer = cooldown;
        }
    }

    public void OnAttack(GameObject target)
    {
        if (isBoosted)
        {
            target.SendMessage("Stun", 2f);
            unitController.SetWalkAnimationSpeed(1f);
            unitController.speed -= speedBoost;
            isBoosted = false;
        }
    }
}
