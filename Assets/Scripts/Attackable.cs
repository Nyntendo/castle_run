﻿using UnityEngine;
using System.Collections;

public class Attackable : MonoBehaviour
{
    public int maxHealth;
    public int health = 0;
    public float hitRange;
    public Team team;
    public bool dead = false;

    public void Start()
    {
        health = maxHealth;
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
        GameObject.FindWithTag("GameController").SendMessage("IncrementUnits", team);
    }
}
