using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Knights.Characters.Players;
using Knights.Characters.NPC;
using Knights.Characters.NPC.Enemies;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Transform PlayerSpawnPosition;
    public Transform EnemiesSpawnPosition;

    public float MaxDistance = 10f;
    public float Space = 5f;

    public int MaxEnemy = 5;
    public GameObject FightScene;
    public bool IsFightCurrentlyRunning = false;
    public bool IsCurrentlyQuestActive = false;
    public List<NPC> Peoples = new List<NPC>();
    public List<Player> Players = new List<Player>();
    public List<Enemy> Enemies = new List<Enemy>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        IsFightCurrentlyRunning = false;
    }

    public void StartFight()
    {
        FightScene.SetActive(true);
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].gameObject.SetActive(true);
            Players[i].transform.position = -Vector3.up * MaxDistance + PlayerSpawnPosition.position + Vector3.up * Space * i;
        }

        for (int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].gameObject.SetActive(true);
            Enemies[i].transform.position = -Vector3.up * MaxDistance + EnemiesSpawnPosition.position + Vector3.up * Space * i;
        }
    }

    public void AddPlayer(Player character)
    {
        Players.Add(character);
    }

    public void AddEnemy(Enemy character)
    {
        Enemies.Add(character);
    }
}
