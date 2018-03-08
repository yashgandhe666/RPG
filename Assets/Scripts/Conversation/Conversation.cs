using System.Collections.Generic;
using UnityEngine;
using Knights.Characters;
using Knights.QuestSystem;
using Knights.Enums;

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
    [SerializeField]
    public Vector2 Position;
    [SerializeField]
    private int dialogueID;
    [SerializeField]
    private int characterID;
    [SerializeField]
    private int speechBubbleIndex;
    [SerializeField]
    private DialogueType dialogueType;
    [SerializeField]
    private string sentence;
    [SerializeField]
    private List<int> childDialogueIDs;

    public int DialogueID { get { return dialogueID; } private set { } }
    public string Sentence { get { return sentence; } private set { } }
    public int CharacterIndex { get { return characterID; } private set { } }
    public int SpeechBubbleIndex { get { return speechBubbleIndex; } private set { } }
    public DialogueType TypeOfDialogue { get { return dialogueType; } private set { } }
    public List<int> ChildDialogueIDs { get { return childDialogueIDs; } private set { } }

    public Dialogue(int _id, DialogueType _dialogueType, string _sentence, int _characterIndex, int _speechBubbleIndex)
    {
        dialogueID = _id;
        dialogueType = _dialogueType;
        if(dialogueType != DialogueType.Answers)
        {
            childDialogueIDs = new List<int>(1);
        }
        else
        {
            childDialogueIDs = new List<int>();
        }

        sentence = _sentence;
        characterID = _characterIndex;
        speechBubbleIndex = _speechBubbleIndex;
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

