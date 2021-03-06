﻿using UnityEngine;
using System.Collections;

public enum MainMenuState
{
    Init,
    SetupLevel
}

public class MainMenuController : MonoBehaviour
{
    public GUISkin skin;
    private Vector2 center;
    private MainMenuState state = MainMenuState.Init;

    private int level = 0;
    private string[] levelStrings = {"Pine Forest", "Frozen Plateau"};

    private int playerRace = 0;
    private int enemyRace = 1;
    private string[] raceStrings = {"Human", "Undead"};

    public MatchController matchController;

    public void Start()
    {
        center = new Vector2(Screen.width / 2, Screen.height / 2);
    }

    public void OnGUI()
    {
        GUI.skin = skin;

        GUI.Label(new Rect(center.x - 200, center.y - 100, 400, 400),"");

        if (state == MainMenuState.Init)
        {
			if (GUI.Button(new Rect(center.x - 100, center.y, 250, 100), "Play Demo"))
            {
                state = MainMenuState.SetupLevel;
            }
        }
        else if (state == MainMenuState.SetupLevel)
        {
			level = GUI.Toolbar(new Rect(center.x - 150, center.y - 50, 350, 100), level, levelStrings);

			playerRace = GUI.Toolbar(new Rect(center.x - 150, center.y + 25, 350, 100), playerRace, raceStrings);

			enemyRace = GUI.Toolbar(new Rect(center.x - 150, center.y + 100, 350, 100), enemyRace, raceStrings);


            if (GUI.Button(new Rect(center.x - 100, center.y + 175, 250, 100), "Start Game"))
            {
                matchController.playerRace = (Race)playerRace;
                matchController.enemyRace = (Race)enemyRace;
                Application.LoadLevel(level + 1);
            }
        }
    }
}
