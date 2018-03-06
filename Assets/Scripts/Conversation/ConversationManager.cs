using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public sealed class ConversationManager : MonoBehaviour
{
    public static ConversationManager Instance { get; private set; }
    public bool IsConversationRunning { get; private set; }

    public GameObject ResponseContent;
    public GameObject ResponseGameObject;

    private List<GameObject> responseGameObjectList;
    private Conversation currentConversation;
    private Dialogue currentDialogue;

    private GameObject bubbleCanvas;

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
        responseGameObjectList = new List<GameObject>();
        InputManager.Instance.OnClickOnScreenContConv += ContinueConversation;
        LoadWelcomeConversation();
    }

    private void LoadWelcomeConversation()
    {
        LoadConversation("Welcome");
    }

    public void LoadConversation(string conversationName)
    {
        currentConversation = Resources.Load<Conversation>("Conversations/" + conversationName);
        if(currentConversation == null)
        {
            Debug.LogWarning("There is no conversation with " + conversationName + " in Resources/Conversations folder...");
        }

        ActivateConversation();
    }

    public void ActivateConversation()
    {
        IsConversationRunning = true;
        currentDialogue = currentConversation.Dialogues[0];
        if (currentDialogue == null)
        {
            Debug.Log("There is dialogues in current ...");
            return;
        }
        ShowDialogue();
    }

    public void ContinueConversation()
    {
        if (bubbleCanvas != null)
        {
            Destroy(bubbleCanvas);
            bubbleCanvas = null;
        }

        foreach (var item in responseGameObjectList)
        {
            Destroy(item);
        }

        responseGameObjectList.Clear();

        if (currentDialogue.ChildDialogueIDs.Count > 0)
        {
            if (currentDialogue.GetDialogueType() == Dialogue.DialogueType.Simple)
            {
                currentDialogue = currentConversation.FindDialogueWithID(currentDialogue.ChildDialogueIDs[0]);

                if (currentConversation != null)
                {
                    ShowDialogue();
                }
                else
                {
                    CompleteConversation();
                }
            }
            else
            {
                foreach (var dialogueID in currentDialogue.ChildDialogueIDs)
                {
                    GameObject gm = Instantiate(ResponseGameObject, transform.position, Quaternion.identity);
                    gm.transform.SetParent(ResponseContent.transform);
                    Text text = gm.GetComponentInChildren<Text>();
                    Dialogue response = currentConversation.FindDialogueWithID(dialogueID);
                    if (text != null && response != null)
                    {
                        text.text = response.Sentence;
                        responseGameObjectList.Add(gm);
                    }
                    else if(response == null)
                    {
                        Debug.LogWarning("Response is null");
                        Destroy(gm);
                    }                    
                }
            }
        }
        else
        {
            CompleteConversation();
        }
    }

    private void ShowDialogue()
    {
        if (bubbleCanvas != null)
        {
            Destroy(bubbleCanvas);
        }

        SpeechBubble bubble = currentConversation.FindSpeechBubbleWithName(currentDialogue.SpeechBubbleName);
        if (bubble != null)
        {
            GameObject gm = bubble.gameObject;

            if (gm != null)
            {
                bubbleCanvas = Instantiate(gm, transform.position, Quaternion.identity);
            }

            Text dialogueText = bubbleCanvas.GetComponentInChildren<Text>();

            dialogueText.text = currentDialogue.Sentence;

            Debug.Log("Dialogue is shown...");
        }
        else
        {
            Debug.Log("Speech bubble is null");
        }
    }

    private void CompleteConversation()
    {
        IsConversationRunning = false;
        if (currentConversation.GetAllQuest().Count > 0)
        {
            QuestManager.Instance.AddQuestToList(currentConversation.GetAllQuest());
        }

        currentConversation = null;
    }
}
