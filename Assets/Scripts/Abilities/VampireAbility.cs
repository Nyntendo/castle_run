using UnityEngine;
using System.Collections;

public class VampireAbility : MonoBehaviour
{
    public float cooldown;

    private Attackable attackable;
    private float cooldownTimer = 0f;

    public void Start()
    {
        attackable = GetComponent<Attackable>();
    }

    public void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
        else if (!attackable.stealthed)
        {
            attackable.SetStealthed(true);
        }
    }

    public void OnAttack(GameObject target)
    {
        if (attackable.stealthed)
        {
            attackable.SetStealthed(false);
        }
        cooldownTimer = cooldown;
    }
}
