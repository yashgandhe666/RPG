    using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Knights.Characters.Players;
using Knights.Characters.NPC;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int MaxEnemy = 5;
    public bool IsFightCurrentlyRunning = false;
    public bool IsCurrentlyQuestActive = false;
    public List<NPC> Peoples = new List<NPC>();
    public List<Player> Players = new List<Player>();

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

    public void AddPlayer(Player character)
    {
        Players.Add(character);
    }
}
