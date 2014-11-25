using UnityEngine;
using System.Collections;

public enum UnitType
{
    Ground = 0x01,
    Flying = 0x02,
    Both = 0x03
}

public class Attackable : MonoBehaviour
{
    public int maxHealth;
    public int health = 0;
    public float hitRange;
    public Team team;
    public bool dead = false;
    public UnitType unitType;
    public UnitType canAttack;

    public void Start()
    {
        health = maxHealth;
        dead = false;
    }

    public void Hit(int damage)
    {
        if (dead)
            return;

        health -= damage;

        if (health <= 0)
        {
            dead = true;
            gameObject.SendMessage("OnDeath");
        }
    }

    public void SetTeam(Team team)
    {
        this.team = team;
        if (gameObject.tag == "Unit")
            GameObject.FindWithTag("GameController").SendMessage("IncrementUnits", team);
    }
}
