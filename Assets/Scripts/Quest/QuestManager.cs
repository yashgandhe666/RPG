using System.Collections.Generic;
using UnityEngine;
using Knights.QuestSystem;
using Knights.Characters.NPC.Enemies;
using Knights.Items;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    public bool IsCurrentlyQuestActivate { get; private set; }
    public Quest CurrentlyActiveQuest { get; private set; }

    public GameObject Gm;

    [SerializeField]
    private List<Quest> currentlyAvailableQuest = new List<Quest>();    

    public void Awake()
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
        CurrentlyActiveQuest = null;
        IsCurrentlyQuestActivate = false;
	}

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            if (Gm.GetComponent<Enemy>() != null)
            {
                Debug.Log("Enemy Script Found...");
                CompareObject(Gm.GetComponent<Enemy>());
            }
            else
            {
                Debug.Log("Enemy Script not Found...");
                CompareObject(Gm);
            }
        }
    }

    private void CheckProgressionOfQuest()
    {
        foreach (var item in CurrentlyActiveQuest.objectives)
        {
            if(item.IsCompleted)
            {
                Debug.Log(item + " is completed...");
            }
            Debug.Log("Checking");
        }
    }

    public void StartQuest(string _questName)
    {
        IsCurrentlyQuestActivate = true;
        string path = "Quests/" + _questName;
        Quest quest = Resources.Load<Quest>(path);
        if(quest == null)
        {
            Debug.Log("Can't find asset in path : " + path);
            return;
        }
        CurrentlyActiveQuest = quest;
    }

    public void AddQuestToList(Quest _quest)
    {
        currentlyAvailableQuest.Add(_quest);
    }

    public void AddQuestToList(List<Quest> _quests)
    {
        Debug.Log("Quest is Added");
        foreach (var item in _quests)
        {
            AddQuestToList(item);
        }
    }

    public List<Quest> GetQuestList()
    {
        return currentlyAvailableQuest;
    }

    public void CompareObject(Object _object)
    {
        foreach (var item in CurrentlyActiveQuest.objectives)
        {
            item.CompareWithRequiredObject(_object);
        }
        CheckProgressionOfQuest();
    }
}
