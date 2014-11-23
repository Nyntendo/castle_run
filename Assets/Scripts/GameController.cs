using UnityEngine;
using System.Collections;

public enum GameState
{
    Paused,
    Playing,
    Finished
}

public class GameController : MonoBehaviour
{
    public GameState gameState = GameState.Playing;
    public LayerMask buildSpotLayerMask;
    public GUISkin guiSkin;
    public Texture2D coinTexture;
	public Texture2D optionsTexture;

    public TeamController playerTeam;
    public TeamController enemyTeam;

    public Transform playerMainBuildingSpot;
    public Transform enemyMainBuildingSpot;

    private BuildSpotController selectedBuildSpot;
    public int playerStartCoins;
    public int enemyStartCoins;
    public float sellRefund;
    public int unitCap;
    public GameObject corpsePrefab;
    public float corpseDespawnTime;

    private Attackable playerMainBuildingAttackable;
    private Attackable enemyMainBuildingAttackable;

    private Team loosingTeam;

    public void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        playerTeam.coins = playerStartCoins;
        enemyTeam.coins = enemyStartCoins;

        var playerMainBuilding = Instantiate(
                playerTeam.mainBuilding,
                playerMainBuildingSpot.position,
                playerMainBuildingSpot.rotation) as GameObject;
        playerMainBuilding.SendMessage("SetTeam", Team.Player);
        playerMainBuildingAttackable = playerMainBuilding.GetComponent<Attackable>();

        var enemyMainBuilding = Instantiate(enemyTeam.mainBuilding,
                enemyMainBuildingSpot.position,
                enemyMainBuildingSpot.rotation) as GameObject;
        enemyMainBuilding.SendMessage("SetTeam", Team.Enemy);
        enemyMainBuildingAttackable = enemyMainBuilding.GetComponent<Attackable>();
    }

    public Transform GetTarget(Team team)
    {
        if (team == Team.Enemy)
            return playerMainBuildingSpot;
        else
            return enemyMainBuildingSpot;
    }

    public void PutCorpseAt(Vector3 position)
    {
        var corpse = Instantiate(corpsePrefab, position, Quaternion.identity) as GameObject;
        Destroy(corpse, corpseDespawnTime);
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

        GUI.Label(new Rect(160, 10, 150, 50),
                string.Format("{0}/{1} units", playerTeam.currentNumberOfUnits, unitCap));

        var playerMainBuildingHealth = (int)Mathf.Floor(
                (float)playerMainBuildingAttackable.health / (float)playerMainBuildingAttackable.maxHealth * 100f);
        var enemyMainBuildingHealth = (int)Mathf.Floor(
                (float)enemyMainBuildingAttackable.health / (float)enemyMainBuildingAttackable.maxHealth * 100f);
        GUI.Label(new Rect(Screen.width / 2 - 100, 10, 200, 50),
                string.Format("{0}% - {1}%", playerMainBuildingHealth, enemyMainBuildingHealth));

        if (gameState == GameState.Finished)
        {
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 200,  50), 
                    (loosingTeam == Team.Enemy)?"You win!":"You loose!");
        }

        if (selectedBuildSpot != null && gameState == GameState.Playing)
        {
            if (selectedBuildSpot.spawner == null)
            {
                for (int i = 0; i < playerTeam.buildings.Length; i++)
                {
                    GUI.Label(new Rect(120 + i * 200, Screen.height - 50, 150, 50),
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
                if (GUI.Button(new Rect(100, Screen.height - 200, 150, 150), "Sell"))
                {
                    var cost = playerTeam.buildingCosts[selectedBuildSpot.spawnerType];
                    playerTeam.coins += (int)Mathf.Floor(cost * sellRefund);
                    Destroy(selectedBuildSpot.spawner.gameObject);
                    selectedBuildSpot.RemoveSpawner();
                }
            }
        }

		if (GUI.Button(new Rect(Screen.width - 100, 10, 70, 70), new GUIContent(optionsTexture), GUIStyle.none))
        {
            if (gameState == GameState.Playing)
                Pause();
            else
                UnPause();
        }
    }

    public void MainBuildingDestroyed(Team team)
    {
        loosingTeam = team;
        gameState = GameState.Finished;
    }

    public void Pause()
    {
        gameState = GameState.Paused;
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        gameState = GameState.Playing;
        Time.timeScale = 1;
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
        if (gameState != GameState.Playing)
            return;

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
