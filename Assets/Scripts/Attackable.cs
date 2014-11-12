using UnityEngine;
using System.Collections;

public class Attackable : MonoBehaviour
{
    public int maxHealth;
    public int health = 0;
    public float hitRange;
    public Team team;

    public void Start()
    {
        health = maxHealth;
    }

    public void Hit(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            gameObject.SendMessage("OnDeath");
        }
    }

    public void SetTeam(int team)
    {
        this.team = (Team)team;
    }
}
