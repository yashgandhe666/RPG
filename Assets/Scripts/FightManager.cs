using System.Collections.Generic;
using UnityEngine;
using Knights.Characters;
using Knights.Characters.Players;
using Knights.Characters.NPC.Enemies;
using UnityEngine.UI;
using Knights.Enums;

public class FightManager : MonoBehaviour
{
    public static FightManager Instance { get; private set; }

    public enum WhoseTurn
    {
        None,
        Player,
        Enemy,
    }

    public WhoseTurn whoseAttackingTurn;
    public List<Player> currentActivePlayers = new List<Player>();
    public List<Enemy> currentAciveEnemies = new List<Enemy>();

    private List<IFighter> attackersInOrder = new List<IFighter>();
    private Button addEnemyButton, attackButton, cancelButton;
    private List<GameObject> enemiesPrefabs = new List<GameObject>();
    private int currentAttackerIndex;
    private float delayTime = 2f;
    private float delayInAttackTimer;
    private bool isFightStart;

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
        currentAttackerIndex = 0;
        whoseAttackingTurn = WhoseTurn.None;
        isFightStart = false;
        delayTime = 1f;
        delayInAttackTimer = delayTime;
        addEnemyButton = CanvasUIHandler.Instance.FightUIPanel.transform.Find("AddButton").GetComponent<Button>();
        attackButton = CanvasUIHandler.Instance.FightUIPanel.transform.Find("AttackButton").GetComponent<Button>();
        cancelButton = CanvasUIHandler.Instance.FightUIPanel.transform.Find("NameAndIcon").Find("CancelButton").GetComponent<Button>();
        attackButton.interactable = false;
        addEnemyButton.interactable = true;
        addEnemyButton.onClick.AddListener(AddEnemyOnScene);
        attackButton.onClick.AddListener(ArrangeAttackersInOrder);
        cancelButton.onClick.AddListener(CancelAttack);
        Enemy[] enemy = Resources.LoadAll<Enemy>("Prefabs/Characters/");
        foreach(var e in enemy)
        {
            enemiesPrefabs.Add(e.gameObject);
        }
        Debug.Log("enemies prefabs count : " + enemiesPrefabs.Count);

        foreach (Player item in GameManager.Instance.Players)
        {
            if(item.gameObject.activeSelf)
            {
                currentActivePlayers.Add(item);
            }
        }
    }

    private void Update()
    {
        if (!isFightStart)
            return;

        if (whoseAttackingTurn == WhoseTurn.None)
        {
            if(currentActivePlayers.Count <= 0 || currentAciveEnemies.Count <= 0)
            {
                CheckForWiningTeam();
            }
            else
            {
                delayInAttackTimer -= Time.deltaTime;
                if (delayInAttackTimer <= 0f)
                {
                    IFighter fighter = WhoseTurnToAttack();
                    if (fighter != null)
                    {
                        fighter.IsItMyTurn = true;
                        delayInAttackTimer = delayTime;
                        //Debug.Log("current fighter is : " + fighter);
                    }
                    else
                    {
                        //Debug.Log("fighter is null");
                    }
                }
            }
        }
    }

    public void AddEnemyOnScene()
    {
        attackButton.interactable = true;
        int rand = Random.Range(0, enemiesPrefabs.Count);
        GameObject gm = Instantiate(enemiesPrefabs[rand], transform.position + Vector3.right * 2f + new Vector3(transform.position.x + 2f, transform.position.y + Random.Range(-3, 3), 0f), Quaternion.identity);
        Enemy enemy = gm.GetComponent<Enemy>();
        gm.SetActive(false);
        currentAciveEnemies.Add(enemy);
        if(currentAciveEnemies.Count >= GameManager.Instance.MaxEnemy)
        {
            addEnemyButton.interactable = false;
        }
    }

    public void RemoveCharacter(Character character, TypeOfFighter type)
    {
        attackersInOrder.Remove((IFighter)character);
        if (type == TypeOfFighter.Player)
        {
            currentActivePlayers.Remove((Player)character);
        }
        else
        {
            currentAciveEnemies.Remove((Enemy)character);
            Destroy(character.gameObject);
        }        
    }

    private void CheckForWiningTeam()
    {
        if (currentActivePlayers.Count <= 0)
        {
            CanvasUIHandler.Instance.ShowFightStatusPanel(false);
        }
        else if (currentAciveEnemies.Count <= 0)
        {
            CanvasUIHandler.Instance.ShowFightStatusPanel(true);
        }

        isFightStart = false;
    }

    private void ArrangeAttackersInOrder()
    {
        GameManager.Instance.IsFightCurrentlyRunning = true;
        GameManager.Instance.StartFight();
        CanvasUIHandler.Instance.FightUIPanel.SetActive(false);
        Debug.Log("Attackers are arranged in ascending order...");
        attackersInOrder.Clear();
        List<Character> currentAttackers = new List<Character>();
        foreach (Character item in currentActivePlayers)
        {
            currentAttackers.Add(item);
        }

        foreach (Character item in currentAciveEnemies)
        {
            currentAttackers.Add(item);
        }

        int index = 0;
        int length = currentAttackers.Count;
        for (int i = 0; i < length; i++)
        {
            index = 0;
            for (int j = 0; j < currentAttackers.Count; j++)
            {
                if (currentAttackers[j].CharacterStats.Initiative < currentAttackers[index].CharacterStats.Initiative)
                {
                    index = j;
                }
            }

            attackersInOrder.Add((IFighter)currentAttackers[index]);
            currentAttackers.RemoveAt(index);
        }

        isFightStart = true;
    }

    private void CancelAttack()
    {
        GameManager.Instance.IsFightCurrentlyRunning = false;
        CanvasUIHandler.Instance.FightUIPanel.SetActive(false);
        Destroy(gameObject, 0.1f);
    }

    private void OnDestroy()
    {
        GameManager.Instance.IsFightCurrentlyRunning = false;
        foreach (Enemy item in currentAciveEnemies)
        {
            Destroy(item.gameObject);
        }
        currentActivePlayers.Clear();
        currentAciveEnemies.Clear();
        attackersInOrder.Clear();
    }

    public IFighter WhoseTurnToAttack()
    {
        if (attackersInOrder.Count == 0)
        {
            whoseAttackingTurn = WhoseTurn.None;
            return null;
        }

        currentAttackerIndex = (currentAttackerIndex + 1) % attackersInOrder.Count;

        IFighter fighter = attackersInOrder[currentAttackerIndex];

        if (fighter.typeOfFighter == TypeOfFighter.Player)
        {
            whoseAttackingTurn = WhoseTurn.Player;
        }
        else
        {
            whoseAttackingTurn = WhoseTurn.Enemy;
        }

        return fighter;
    }
}
