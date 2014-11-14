using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    private int pinkTeamCoins = 0;
    private int yellowTeamCoins = 0;

    public void AddCoins(Team team, int coins)
    {
        if (team == Team.Yellow)
            yellowTeamCoins += coins;
        else
            pinkTeamCoins += coins;
    }

    public void AddCoinsToOtherTeam(Team team, int coins)
    {
        if (team == Team.Pink)
            yellowTeamCoins += coins;
        else
            pinkTeamCoins += coins;
    }

    public void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 30), string.Format("{0} coins", pinkTeamCoins));
        GUI.Label(new Rect(Screen.width - 110, 10, 100, 30), string.Format("{0} coins", yellowTeamCoins));
    }
}
