using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUIHandler : MonoBehaviour
{
    public static CanvasUIHandler Instance { get; private set; }

    [Header("UI Panels")]
    public GameObject QuestUIPanel;
    public GameObject FightUIPanel;
    public GameObject FightStatusUIPanel;
    public GameObject SelectedCharacterStatsInfoPanel;

    [Header("Sub UI Panel")]
    public GameObject QuestPresentPanel;
    public GameObject CurrentQuestDecriptionPanel;

    [Header("UI Containers")]
    public GameObject QuestInformationContainer;
    public GameObject ObjectiveContainer;
    public GameObject CharacterStatsInfoContainer;

    [Header("Prefabs")]
    public GameObject QuestInformationPrefab;
    public GameObject ObjectDescriptionPrefab;
    public GameObject CharacterStatsTextPrefabs;

    private List<GameObject> prefabsCloneOfContainer = new List<GameObject>();
    private List<Button> currentlyAvailableQuestButtons = new List<Button>();
    private GameObject currentlyActiveUIPanel;
    private string selectedQuestText;
    private Button startQuestButton;
    private Button continueGameplayButton;

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
        QuestUIPanel.SetActive(false);
        FightUIPanel.SetActive(false);
        FightStatusUIPanel.SetActive(false);
        currentlyActiveUIPanel = null;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(currentlyActiveUIPanel != null)
            {
                currentlyActiveUIPanel.SetActive(false);
                currentlyActiveUIPanel = null;
            }
        }

        if(currentlyActiveUIPanel == null)
        {
            InputManager.Instance.CanTouch = true;
        }
        else
        {
            InputManager.Instance.CanTouch = false;
        }

        if(startQuestButton != null)
        {
            if(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == null)
            {
                startQuestButton.interactable = false;
            }
        }
    }

    public void ShowFightStatusPanel(bool isWin)
    {
        FightStatusUIPanel.SetActive(true);
        Text status = FightStatusUIPanel.transform.Find("Descriptions").Find("Status").GetComponent<Text>();
        continueGameplayButton = FightStatusUIPanel.transform.Find("ContinueButton").GetComponent<Button>();
        continueGameplayButton.onClick.AddListener(ContinueGameplay);
        currentlyActiveUIPanel = FightStatusUIPanel;

        if (isWin)
        {
            status.text = "You Win!";
        }
        else
        {
            status.text = "You Loss!";
        }
    }

    public void ContinueGameplay()
    {
        Debug.Log("Continue");
        currentlyActiveUIPanel.SetActive(false);
        currentlyActiveUIPanel = null;
        Destroy(FightManager.Instance.gameObject);
    }

    public void ShowCurrentlyAvilableQuests()
    {
        DestroyAllClonePrefabsOfContainer();

        QuestUIPanel.SetActive(true);
        QuestPresentPanel.SetActive(true);
        CurrentQuestDecriptionPanel.SetActive(false);
        currentlyActiveUIPanel = QuestUIPanel;
        startQuestButton = QuestUIPanel.transform.Find("StartButton").GetComponent<Button>();
        startQuestButton.onClick.AddListener(StartQuest);
        foreach (var item in QuestManager.Instance.GetQuestList())
        {
            GameObject gm = Instantiate(QuestInformationPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            gm.transform.SetParent(QuestInformationContainer.transform);
            prefabsCloneOfContainer.Add(gm);
            Text text = gm.GetComponentInChildren<Text>();
            Button button = gm.GetComponent<Button>();
            button.onClick.AddListener(delegate { OnClickButton(text.text); });
            currentlyAvailableQuestButtons.Add(button);
            text.text = item.QuestName;
        }
    }

    private void OnClickButton(string text)
    {
        if(startQuestButton != null)
        {
            startQuestButton.interactable = true;
        }

        Debug.Log("Quest is selected...");
        selectedQuestText = text;
    }

    private void StartQuest()
    {
        Debug.Log("Quest is started...");
        QuestManager.Instance.StartQuest(selectedQuestText);
        if (currentlyActiveUIPanel != null)
            currentlyActiveUIPanel.SetActive(false);
        currentlyActiveUIPanel = null;
    }

    public void ShowCurrentQuestDescription()
    {
        DestroyAllClonePrefabsOfContainer();

        QuestUIPanel.SetActive(true);
        CurrentQuestDecriptionPanel.SetActive(true);
        QuestPresentPanel.SetActive(false);

        currentlyActiveUIPanel = QuestUIPanel;

        foreach (var item in QuestManager.Instance.CurrentlyActiveQuest.objectives)
        {
            GameObject gm = Instantiate(ObjectDescriptionPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            gm.transform.SetParent(ObjectiveContainer.transform);
            prefabsCloneOfContainer.Add(gm);
            Text questName = CurrentQuestDecriptionPanel.transform.Find("QuestName").Find("Text").GetComponent<Text>();
            questName.text = QuestManager.Instance.CurrentlyActiveQuest.QuestName;
            Text text = gm.GetComponentInChildren<Text>();
            text.text = item.objectiveName;
        }
    }

    public void ShowSelectedCharacterStats(CharacterStats characterStats)
    {
        DestroyAllClonePrefabsOfContainer();
        SelectedCharacterStatsInfoPanel.SetActive(true);
        System.Reflection.FieldInfo[] fields = typeof(CharacterStats).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        //Debug.Log("SHOW, Length : " + fields.Length);
        
        foreach (var item in fields)
        {
            if (item.FieldType == typeof(Sprite))
            {
                GameObject iconGO = SelectedCharacterStatsInfoPanel.transform.Find("Icon").gameObject;
                Image iconImage = iconGO.GetComponent<Image>();
                if(iconImage != null)
                {
                    iconImage.sprite = (Sprite)item.GetValue(characterStats);
                }
            }
            else
            {
                GameObject gm = Instantiate(CharacterStatsTextPrefabs, Vector3.zero, Quaternion.identity);
                gm.transform.SetParent(CharacterStatsInfoContainer.transform);
                gm.GetComponent<Text>().text = item.Name + " : " + item.GetValue(characterStats).ToString();
                prefabsCloneOfContainer.Add(gm);
            }
        }
    }

    private void DestroyAllClonePrefabsOfContainer()
    {
        foreach (var item in prefabsCloneOfContainer)
        {
            Destroy(item);
        }
        prefabsCloneOfContainer.Clear();
    }
}
