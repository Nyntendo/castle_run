using UnityEngine;
using System.Collections;


public class GameController : MonoBehaviour
{
    public LayerMask buildSpotLayerMask;

    public TeamController playerTeam;
    public TeamController enemyTeam;

    private BuildSpotController selectedBuildSpot;
    public int playerStartCoins;
    public int enemyStartCoins;
    public float sellRefund;

    public void Start()
    {
        playerTeam.coins = playerStartCoins;
        enemyTeam.coins = enemyStartCoins;
    }

    public void AddCoins(Team team, int coins)
    {
        if (team == Team.Enemy)
            enemyTeam.coins += coins;
        else
            playerTeam.coins += coins;
    }

    public void AddCoinsToOtherTeam(Team team, int coins)
    {
        if (team == Team.Player)
            enemyTeam.coins += coins;
        else
            playerTeam.coins += coins;
    }

    public void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 30), string.Format("{0} coins", playerTeam.coins));
        GUI.Label(new Rect(Screen.width - 110, 10, 100, 30), string.Format("{0} coins", enemyTeam.coins));

        if (selectedBuildSpot != null)
        {
            if (selectedBuildSpot.spawner == null)
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
            else
            {
                if (GUI.Button(new Rect(100, Screen.height - 100, 130, 30), "Sell"))
                {
                    var cost = playerTeam.buildingCosts[selectedBuildSpot.spawnerType];
                    playerTeam.coins += (int)Mathf.Floor(cost * sellRefund);
                    Destroy(selectedBuildSpot.spawner.gameObject);
                    selectedBuildSpot.RemoveSpawner();
                }
            }
        }
    }

    private void Build(int i)
    {
        var cost = playerTeam.buildingCosts[i];
        if (playerTeam.coins >= cost)
        {
            playerTeam.coins -= cost;
            var building = Instantiate(playerTeam.buildings[i], selectedBuildSpot.transform.position, Quaternion.identity) as GameObject;
            selectedBuildSpot.PlaceSpawner(building, i);
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
