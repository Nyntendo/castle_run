using UnityEngine;
using System.Collections;


public class GameController : MonoBehaviour
{
    public LayerMask buildSpotLayerMask;

    public TeamController playerTeam;

    private BuildSpotController selectedBuildSpot;
    private int playerCoins = 100;
    private int enemyCoins = 0;

    public void AddCoins(Team team, int coins)
    {
        if (team == Team.Enemy)
            enemyCoins += coins;
        else
            playerCoins += coins;
    }

    public void AddCoinsToOtherTeam(Team team, int coins)
    {
        if (team == Team.Player)
            enemyCoins += coins;
        else
            playerCoins += coins;
    }

    public void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 30), string.Format("{0} coins", playerCoins));
        GUI.Label(new Rect(Screen.width - 110, 10, 100, 30), string.Format("{0} coins", enemyCoins));

        if (selectedBuildSpot != null && selectedBuildSpot.spawner == null)
        {
            for (int i = 0; i < playerTeam.buildings.Length; i++)
            {
                if (GUI.Button(new Rect(100 + i * 200, Screen.height - 100, 130, 30),
                    playerTeam.buildings[i].name))
                {
                    Build(i);
                }
            }
        }
    }

    private void Build(int i)
    {
        var cost = playerTeam.buildingCosts[i];
        if (playerCoins >= cost)
        {
            playerCoins -= cost;
            var building = Instantiate(playerTeam.buildings[i], selectedBuildSpot.transform.position, Quaternion.identity) as GameObject;
            selectedBuildSpot.PlaceSpawner(building);
            SelectBuildSpot(null);
        }
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && GUIUtility.hotControl == 0)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, buildSpotLayerMask))
            {
                SelectBuildSpot(hit.collider.GetComponent<BuildSpotController>());
            }
            else
            {
                SelectBuildSpot(null);
            }
        }
    }

    private void SelectBuildSpot(BuildSpotController buildSpot)
    {
        if (selectedBuildSpot != null)
        {
            selectedBuildSpot.Deselect();
        }

        selectedBuildSpot = buildSpot;

        if (selectedBuildSpot != null)
        {
            selectedBuildSpot.Select();
        }
    }
}
