using UnityEngine;
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
        GUI.Box(new Rect(center.x - 200, center.y - 200, 400, 400), "Tower Wars");

        if (state == MainMenuState.Init)
        {
            if (GUI.Button(new Rect(center.x - 100, center.y - 100, 200, 50), "Play Demo"))
            {
                state = MainMenuState.SetupLevel;
            }
        }
        else if (state == MainMenuState.SetupLevel)
        {
            level = GUI.Toolbar(new Rect(center.x - 150, center.y - 150, 300, 50), level, levelStrings);

            playerRace = GUI.Toolbar(new Rect(center.x - 150, center.y - 75, 300, 50), playerRace, raceStrings);

            enemyRace = GUI.Toolbar(new Rect(center.x - 150, center.y, 300, 50), enemyRace, raceStrings);

            if (GUI.Button(new Rect(center.x - 100, center.y + 100, 200, 50), "Start Game"))
            {
                matchController.playerRace = (Race)playerRace;
                matchController.enemyRace = (Race)enemyRace;
                Application.LoadLevel(level + 1);
            }
        }
    }
}
