using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

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
                unitType = Clean(unitType),
                team = team
                });
    }

    public void LogDeath(Team team, string unitType)
    {
        statistics.Add(new StatisticsEntry(){
                type = StatisticsEntryType.UnitKilled,
                unitType = Clean(unitType),
                team = team
                });
    }

    public void LogDamage(Team team, string unitType, string targetUnitType, int damage)
    {
        statistics.Add(new StatisticsEntry(){
                type = StatisticsEntryType.DamageDone,
                unitType = Clean(unitType),
                team = team,
                targetUnitType = Clean(targetUnitType),
                damage = damage
                });
    }

    private string Clean(string name)
    {
        return name.Replace("(Clone)", "");
    }

    public string[,] GetStatistics()
    {
        var results = new string[,] {
            {"Player", "Team", "Units spawned", "Units lost", "Damage done"},
            {"","","","",""},
            {"","","","",""}
        };

        return results;
    }

    public void PrintReport()
    {
        var playerUnitsSpawned = statistics.Count(s => s.type == StatisticsEntryType.UnitSpawned && s.team == Team.Player);
        var playerUnitsLost = statistics.Count(s => s.type == StatisticsEntryType.UnitKilled && s.team == Team.Player);
        var enemyUnitsSpawned = statistics.Count(s => s.type == StatisticsEntryType.UnitSpawned && s.team == Team.Enemy);
        var enemyUnitsLost = statistics.Count(s => s.type == StatisticsEntryType.UnitKilled && s.team == Team.Enemy);
        var playerDamageDone = statistics.Where(s => s.type == StatisticsEntryType.DamageDone && s.team == Team.Player).Sum(s => s.damage);
        var enemyDamageDone = statistics.Where(s => s.type == StatisticsEntryType.DamageDone && s.team == Team.Enemy).Sum(s => s.damage);
        Debug.Log(string.Format("Player - Units spawned: {0}", playerUnitsSpawned));
        Debug.Log(string.Format("Player - Units lost: {0}", playerUnitsLost));
        Debug.Log(string.Format("Player - Damage done: {0}", playerDamageDone));
        Debug.Log(string.Format("Enemy - Units spawned: {0}", enemyUnitsSpawned));
        Debug.Log(string.Format("Enemy - Units lost: {0}", enemyUnitsLost));
        Debug.Log(string.Format("Enemy - Damage done: {0}", enemyDamageDone));

        SaveLogToFile();
    }

    public void SaveLogToFile()
    {
        var file = File.CreateText("statistics.txt");
        file.WriteLine("Type;Unit;Team;Target;Damage");
        foreach (var s in statistics)
        {
            file.WriteLine(string.Format("{0};{1};{2};{3};{4}", s.type.ToString(), s.unitType, s.team.ToString(), s.targetUnitType, s.damage));
        }
        file.Close();
    }
}
