using UnityEngine;
using System.Collections;


public class GameController : MonoBehaviour
{
    public LayerMask buildSpotLayerMask;
    public GUISkin guiSkin;
    public Texture2D coinTexture;

    public TeamController playerTeam;
    public TeamController enemyTeam;

    private BuildSpotController selectedBuildSpot;
    public int playerStartCoins;
    public int enemyStartCoins;
    public float sellRefund;
    public int unitCap;

    public void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        playerTeam.coins = playerStartCoins;
        enemyTeam.coins = enemyStartCoins;
    }

    public void IncrementUnits(Team team)
    {
        if (team == Team.Enemy)
            enemyTeam.currentNumberOfUnits++;
        else
            playerTeam.currentNumberOfUnits++;
    }

    public void DecrementUnits(Team team)
    {
        if (team == Team.Enemy)
            enemyTeam.currentNumberOfUnits--;
        else
            playerTeam.currentNumberOfUnits--;
    }

    public bool CanSpawn(Team team)
    {
        if (team == Team.Enemy)
            return enemyTeam.currentNumberOfUnits < unitCap;
        else
            return playerTeam.currentNumberOfUnits < unitCap;
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

    public TeamController GetTeamController(Team team)
    {
        if (team == Team.Player)
            return playerTeam;
        else
            return enemyTeam;
    }

    public void OnGUI()
    {
        GUI.skin = guiSkin;
        GUI.Label(new Rect(10, 10, 150, 50),
                new GUIContent(string.Format(" x {0}", playerTeam.coins), coinTexture));

        if (selectedBuildSpot != null)
        {
            if (selectedBuildSpot.spawner == null)
            {
                for (int i = 0; i < playerTeam.buildings.Length; i++)
                {
                    GUI.Label(new Rect(100 + i * 200, Screen.height - 50, 150, 50),
                            new GUIContent(string.Format(" x {0}", playerTeam.buildingCosts[i]), coinTexture));
                    if (GUI.Button(new Rect(100 + i * 200, Screen.height - 200, 150, 150),
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

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && GUIUtility.hotControl == 0)
        {
            var ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
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
