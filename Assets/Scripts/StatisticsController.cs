using UnityEngine;
using System.Collections;

public enum StatisticsEntryType
{
    UnitSpawned,
    UnitKilled,
    DamageDone
}

public class StatisticsEntry
{
    public StatisticsEntryType type;
    public string unitType;
    public Team team;
    public string targetUnitType;
    public int damage;
}

public class StatisticsController : MonoBehaviour
{
    private List<StatisticsEntry> statistics;

    public void Start()
    {
        statistics = new List<StatisticsEntry>();
    }

    public void LogSpawn(Team team, string unitType)
    {
        statistics.Add(new StatisticsEntry(){
                type = StatisticsEntryType.UnitSpawned,
                unitType = unitType,
                team = team
                });
    }

    public void LogDeath(Team team, string unitType)
    {
        statistics.Add(new StatisticsEntry(){
                type = StatisticsEntryType.UnitKilled,
                unitType = unitType,
                team = team
                });
    }

    public void LogDamage(Team team, string unitType, string targetUnitType, int damage)
    {
        statistics.Add(new StatisticsEntry(){
                type = StatisticsEntryType.DamageDone,
                unitType = unitType,
                team = team,
                targetUnitType = targetUnitType,
                damage = damage;
                });
    }

    public void PrintReport()
    {
        var playerUnitsSpawned = statistics.Select(s => s.type == StatisticsEntryType.UnitSpawned && s.team == Team.Player).Count;
    }
}
