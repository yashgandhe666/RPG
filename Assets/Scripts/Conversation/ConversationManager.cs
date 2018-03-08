using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Knights.Enums;

public sealed class ConversationManager : MonoBehaviour
{
    public static ConversationManager Instance { get; private set; }
    public bool IsConversationRunning { get; private set; }

    private List<GameObject> dialogueTextGameObjectList;
    private Conversation currentConversation;
    private Dialogue currentDialogue;
    private bool isShowingAnswers;

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
        dialogueTextGameObjectList = new List<GameObject>();
        InputManager.Instance.OnClickOnScreenContConv += ContinueConversation;
        isShowingAnswers = false;
        //LoadWelcomeConversation();
    }

    [ContextMenu("Load Welcome")]
    private void LoadWelcomeConversation()
    {
        if (Application.isPlaying)
            LoadConversation("Welcome");
    }

    public void LoadConversation(string conversationName)
    {
        currentConversation = Resources.Load<Conversation>("Conversations/" + conversationName);
        if(currentConversation == null)
        {
            Debug.LogWarning("There is no conversation with " + conversationName + " in Resources/Conversations folder...");
            return;
        }
        
        ActivateConversation();
    }

    private void ActivateConversation()
    {
        currentDialogue = currentConversation.Dialogues[0];
        if (currentDialogue == null)
        {
            Debug.Log("There is dialogues in current ...");
            return;
        }

        IsConversationRunning = true;
        CanvasUIHandler.Instance.DialogueUIPanel.SetActive(true);

        GameObject gm = CreateDialogueTextGameObject(currentDialogue);
        if (gm != null)
        {
            dialogueTextGameObjectList.Add(gm);
        }
    }

    // Call by ClickOnScreen events
    public void ContinueConversation()
    {
        ShowDialogues();
    }

    private void ShowDialogues()
    {
        if(currentDialogue.ChildDialogueIDs.Count <= 0)
        {
            CompleteConversation();
            return;
        }

        if(isShowingAnswers)
        {
            Debug.Log("Resposing....");
            return;
        }

        DestroyListDialogueTextGameObject();

        if (currentDialogue.TypeOfDialogue == DialogueType.Question)
        {
            foreach (var childID in currentDialogue.ChildDialogueIDs)
            {
                Dialogue dialogue = currentConversation.FindDialogueWithID(childID);
                if (dialogue == null)
                {
                    continue;
                }
                GameObject gm = CreateDialogueTextGameObject(dialogue);
                Button bt = gm.AddComponent<Button>();
                bt.onClick.AddListener(delegate { NextDiaolugeButton(childID); });
                if (gm != null)
                {
                    dialogueTextGameObjectList.Add(gm);
                }
            }
            isShowingAnswers = true;
        }
        else
        {
            currentDialogue = currentConversation.FindDialogueWithID(currentDialogue.ChildDialogueIDs[0]);
            GameObject gm = CreateDialogueTextGameObject(currentDialogue);
            if (gm != null)
            {
                dialogueTextGameObjectList.Add(gm);
            }
        }
    }

    private GameObject CreateDialogueTextGameObject(Dialogue dialogue)
    {
        if(dialogue == null)
        {
            CompleteConversation();
            return null;
        }

        SpeechBubble bubble = currentConversation.SpeechBubbles[dialogue.SpeechBubbleIndex];
        if (bubble != null)
        {
            GameObject gm = bubble.gameObject;
            if (gm != null)
            {
                gm = Instantiate(gm, transform.position, Quaternion.identity);
                gm.transform.SetParent(CanvasUIHandler.Instance.DialogueContainer.transform);
            }

            Text dialogueText = gm.GetComponentInChildren<Text>();
            dialogueText.text = dialogue.Sentence;
            return gm;
        }
        return null;
    }

    public void NextDiaolugeButton(int childID)
    {
        Debug.Log("Button : " + childID);
        currentDialogue = currentConversation.FindDialogueWithID(childID);
        isShowingAnswers = false;
        ShowDialogues();
    }

    private void DestroyListDialogueTextGameObject()
    {
        foreach (var item in dialogueTextGameObjectList)
        {
            Destroy(item);
        }
        dialogueTextGameObjectList.Clear();
    }

    private void CompleteConversation()
    {
        DestroyListDialogueTextGameObject();
        IsConversationRunning = false;
        CanvasUIHandler.Instance.DialogueUIPanel.SetActive(false);
        if (currentConversation.GetAllQuest().Count > 0)
        {
            QuestManager.Instance.AddQuestToList(currentConversation.GetAllQuest());
        }

        currentConversation = null;
    }
}
