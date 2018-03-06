using System.Collections.Generic;
using UnityEngine;
using Knights.Characters;
using Knights.QuestSystem;

public class Conversation : ScriptableObject
{
    [SerializeField]
    private string conversationName;
    [SerializeField]
    private List<Character> actorPrefabs;
    [SerializeField]
    private List<SpeechBubble> speechBubbles;
    [SerializeField]
    private List<Dialogue> dialogues;
    [SerializeField]
    private List<Quest> quests;

    public string ConversationName { get { return conversationName; } private set { } }
    public List<Character> ActorPrefabs { get { return actorPrefabs; } private set { } }
    public List<SpeechBubble> SpeechBubbles { get { return speechBubbles; } private set { } }
    public List<Dialogue> Dialogues { get { return dialogues; } private set { } }

    public Conversation Init(string _name)
    {
        conversationName = _name;
        dialogues = new List<Dialogue>();
        actorPrefabs = new List<Character>();
        speechBubbles = new List<SpeechBubble>();
        quests = new List<Quest>();
        return this;
    }

    public void EditConversationName(string _name)
    {
        conversationName = _name;
    }

    public void AddDialogue(Dialogue _selectedDialogue, Dialogue _addDialogue)
    {
        _selectedDialogue.AddDialogueIDs(_addDialogue.DialogueID);
        dialogues.Add(_addDialogue);
    }

    public Dialogue FindDialogueWithID(int _id)
    {
        foreach (var item in dialogues)
        {
            if(item.DialogueID == _id)
            {
                return item;
            }
        }

        return null;
    }

    public SpeechBubble FindSpeechBubbleWithName(string _name)
    {
        foreach (var item in speechBubbles)
        {
            if(item.SpeechBubbleName == _name)
            {
                return item;
            }
        }

        return null;
    }

    public int GetTotalDialogueCount()
    {
        return dialogues.Count;
    }

    public void AddActorPrefabs(Character _actor)
    {
        actorPrefabs.Add(_actor);
    }

    public void AddSpeechBubble(SpeechBubble _bubble)
    {
        speechBubbles.Add(_bubble);
    }

    public void RemoveDialogue(Dialogue dialogue)
    {
        dialogues.Remove(dialogue);
    }

#region QuestMethods
    public void AddQuest(Quest _quest)
    {
        quests.Add(_quest);
    }

    public void RemoveQuest(int index)
    {
        if (index >= quests.Count)
            return;

        quests.RemoveAt(index);
    }

    public List<Quest> GetAllQuest()
    {
        return quests;
    }
#endregion QuestMethods
}

[System.Serializable]
public class Dialogue
{
    public enum DialogueType
    {
        Simple,
        Response,
    }

    [SerializeField]
    public Vector2 Position;
    [SerializeField]
    private int dialogueID;
    [SerializeField]
    private string characterName;
    [SerializeField]
    private string speechBubbleName;
    [SerializeField]
    private string sentence;
    [SerializeField]
    private List<int> childDialogueIDs = new List<int>();

    public string CharacterName { get { return characterName; } private set { } }
    public int DialogueID { get { return dialogueID; } private set { } }
    public string Sentence { get { return sentence; } private set { } }
    public string SpeechBubbleName { get { return speechBubbleName; } private set { } }
    public List<int> ChildDialogueIDs { get { return childDialogueIDs; } private set { } }

    public Dialogue(int _id, string _characterName, string _sentence, string _speechBubbleName)
    {
        dialogueID = _id;
        characterName = _characterName;
        sentence = _sentence;
        speechBubbleName = _speechBubbleName;
    }

    public DialogueType GetDialogueType()
    {
        if(childDialogueIDs.Count > 1)
        {
            return DialogueType.Response;
        }

        return DialogueType.Simple;
    }

    public void EditSentence(string newSentence)
    {
        sentence = newSentence;
    }

    public void AddDialogueIDs(int _dialogueID)
    {
        childDialogueIDs.Add(_dialogueID);
    }

    public void RemoveChildID(int childID)
    {
        childDialogueIDs.Remove(childID);
    }
}

