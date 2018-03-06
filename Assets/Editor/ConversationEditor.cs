using Knights.Characters;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConversationEditor : EditorWindow
{
    #region Variables
    public static Texture2D headerTexture;
    public static Conversation selectedConversation = null;
    public static List<Conversation> conversations = new List<Conversation>();

    private const float nodeHeight = 40f, nodeWidth = 200f, nodeVerSpace = 40f, nodeHorSpace = 10f;
    private static float headerHeight = 50f;
    private static float SideWidth = 300f;
    private static Texture2D leftTexture;
    private static Texture2D rightTexture;
    private static Texture2D selectedScreenTexture;
    private static int selectedTabInt;

    private static bool isAddingDialogue;
    private static string sentence, characterName, bubbleTalkName;
    private static int selectedActorNameIndex;
    private static int selectedBubbleTalkNameIndex;
    private static float bottomHeight = 120f;
    private static Dialogue selectedDialogue;
    private static Vector2 DialogueViewRectOffset;

    private GUIStyle normalStyle;
    private Vector2 scrollPosOfConversations;
    private Vector2 scrollPosOfRightView;
    private string[] toolBars = new string[4] { "Actor", "BubbleTalk", "Dialogue", "Quest" };
    #endregion Variables

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/Conversation Editor")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        ConversationEditor window = (ConversationEditor)GetWindow(typeof(ConversationEditor));
        window.Show();
        window.minSize = new Vector2(450, 450);
        GUIContent content = new GUIContent("Editor");
        window.titleContent = content;
        conversations.Clear();

        Conversation[] conver = Resources.LoadAll<Conversation>("Conversations");
        foreach (var item in conver)
        {
            string[] str = AssetDatabase.GetAssetPath(item).Split(new char[] { '/', '.' });
            conversations.Add(item);
            item.EditConversationName(str[str.Length - 2]);
        }

        if (conversations.Count > 0 && selectedConversation == null)
        {
            selectedConversation = conversations[0];
            CreateTreeStructure.AssignPositionToNodes(selectedConversation, nodeWidth, nodeHeight, nodeHorSpace, nodeVerSpace);
        }

        isAddingDialogue = false;

        InitializeTexture();
    }

    private void OnFocus()
    {
        Init();
    }

    private static void InitializeTexture()
    {
        headerTexture = new Texture2D(1, 1);
        leftTexture = new Texture2D(1, 1);
        rightTexture = new Texture2D(1, 1);
        selectedScreenTexture = Resources.Load<Texture2D>("UI/background");

        headerTexture.SetPixel(0, 0, Color.red);
        headerTexture.Apply();

        leftTexture.SetPixel(0, 0, Color.magenta);
        leftTexture.Apply();

        rightTexture.SetPixel(0, 0, Color.white);
        rightTexture.Apply();
    }

    void OnGUI()
    {
        normalStyle = new GUIStyle()
        {
            fontStyle = FontStyle.BoldAndItalic,
            fontSize = 15,
            alignment = TextAnchor.MiddleLeft
        };

        normalStyle.normal.textColor = Color.black;

        #region Header View
        Rect rect = new Rect(0, 0, Screen.width, headerHeight);
        GUILayout.BeginArea(rect);
        GUI.DrawTexture(rect, headerTexture);
        GUIStyle style = new GUIStyle()
        {
            font = EditorStyles.boldFont,
            alignment = TextAnchor.MiddleCenter,
            fixedHeight = 50,
            fontSize = 25
        };
        GUILayout.Label(new GUIContent("Conversation Editor"), style);
        GUILayout.EndArea();
        #endregion End Header View

        #region Left Side View
        SideWidth = Screen.width / 3f;
        rect = new Rect(0, headerHeight, SideWidth, Screen.height);
        GUI.DrawTexture(rect, leftTexture);
        GUILayout.BeginArea(rect);
        scrollPosOfConversations = EditorGUILayout.BeginScrollView(scrollPosOfConversations, GUILayout.Width(SideWidth), GUILayout.Height(Screen.height - 100f));
        foreach (var item in conversations)
        {
            if (item == selectedConversation)
            {
                Texture2D tex = new Texture2D(1, 1);
                tex.SetPixel(0, 0, Color.blue);
                tex.Apply();
                style.normal.background = tex;
                GUILayout.BeginHorizontal(style);
            }
            else
            {
                GUILayout.BeginHorizontal();
            }

            GUILayout.Label(item.ConversationName, normalStyle, GUILayout.Width((Screen.width / 3.0f) * 0.70f));
            if (GUILayout.Button("Edit"))
            {
                selectedConversation = item;
                CreateTreeStructure.AssignPositionToNodes(selectedConversation, nodeWidth, nodeHeight, nodeHorSpace, nodeVerSpace);
            }
            if (GUILayout.Button("delete"))
            {
                conversations.Remove(selectedConversation);
                string path = AssetDatabase.GetAssetPath(selectedConversation);
                AssetDatabase.DeleteAsset(path);
                selectedConversation = null;
            }
            GUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Add New Conversation", GUILayout.Width(200f)))
        {
            AddConversation();
        }
        GUILayout.EndArea();
        #endregion End Left Side View

        #region Right Side View
        rect = new Rect(SideWidth, headerHeight, Screen.width - SideWidth - 10f, Screen.height - headerHeight - 30f);

        if (selectedConversation == null)
        {
            GUI.DrawTexture(rect, rightTexture);
        }
        else
        {
            GUILayout.BeginArea(rect);
            selectedTabInt = GUILayout.Toolbar(selectedTabInt, toolBars);
            EditorGUI.BeginChangeCheck();
            if (EditorGUI.EndChangeCheck())
            {
                scrollPosOfRightView = Vector2.zero;
            }

            if (selectedTabInt == 0)
            {
                GUI.DrawTexture(new Rect(0, 20f, rect.width, rect.height - headerHeight), selectedScreenTexture);
                ShowActors(rect);
            }
            else if (selectedTabInt == 1)
            {
                GUI.DrawTexture(new Rect(0, 20f, rect.width, rect.height - headerHeight), selectedScreenTexture);
                ShowBubbleTalks(rect);
            }
            else if (selectedTabInt == 2)
            {
                GUI.DrawTexture(new Rect(0, 20f, rect.width, rect.height - bottomHeight), selectedScreenTexture);
                if (selectedConversation.SpeechBubbles.Count > 0 && selectedConversation.ActorPrefabs.Count > 0)
                {
                    Event e = Event.current;
                    if (e.button == 1 && e.isMouse)
                    {
                        selectedDialogue = GetSelectedDialogue(e.mousePosition);
                        if(!isAddingDialogue && selectedDialogue != null)
                        {
                            RightClickMenuOfDialogues();
                        }
                    }

                    ShowDialogueScreen(rect);
                }
                else if (selectedConversation.SpeechBubbles.Count <= 0)
                {
                    GUILayout.Label("Add Speech Bubbles in the list", normalStyle);
                }
                else if (selectedConversation.ActorPrefabs.Count <= 0)
                {
                    GUILayout.Label("Add Actor Bubbles in the list", normalStyle);
                }
            }
            else if (selectedTabInt == 3)
            {
                GUI.DrawTexture(new Rect(0, 20f, rect.width, rect.height - headerHeight), selectedScreenTexture);
                ShowQuest(rect);
            }
            GUILayout.EndArea();
        }

        #endregion End Right Side View
    }

    private void ShowActors(Rect rect)
    {
        scrollPosOfRightView = EditorGUILayout.BeginScrollView(scrollPosOfRightView, GUILayout.Width(rect.width), GUILayout.Height(rect.height - headerHeight));
        GUILayout.BeginHorizontal();
        int k = 5;
        for (int i = 0; i < selectedConversation.ActorPrefabs.Count; i++)
        {
            if (i % k == 0)
            {
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
            }

            GUILayout.BeginVertical();
            if (selectedConversation.ActorPrefabs[i] != null)
            {
                Sprite icon = selectedConversation.ActorPrefabs[i].CharacterStatsWithSO.CharIcon;
                if (icon != null)
                {
                    GUILayout.Box(icon.texture, GUILayout.Width(100f), GUILayout.Height(100f));
                }

                GUILayout.Label(selectedConversation.ActorPrefabs[i].CharacterName, normalStyle, GUILayout.Width(150f));
            }
            else
            {
                selectedConversation.ActorPrefabs[i] = (Character)EditorGUILayout.ObjectField(selectedConversation.ActorPrefabs[i], typeof(Character), false, GUILayout.Width(100f));
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("-", GUILayout.Width(40f)))
            {
                selectedConversation.ActorPrefabs.RemoveAt(i);
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();
        EditorGUILayout.EndScrollView();

        GUILayout.BeginHorizontal();
        var style = new GUIStyle(normalStyle)
        {
            alignment = TextAnchor.UpperCenter
        };

        GUILayout.Label("Add Actor", style, GUILayout.Width(100f), GUILayout.Height(40f));

        if (GUILayout.Button("+", GUILayout.Width(40f)))
        {
            selectedConversation.AddActorPrefabs(null);
        }
    }

    private void ShowBubbleTalks(Rect rect)
    {
        scrollPosOfRightView = EditorGUILayout.BeginScrollView(scrollPosOfRightView, GUILayout.Width(rect.width), GUILayout.Height(rect.height - headerHeight));
        GUILayout.BeginHorizontal();
        int k = 5;
        for (int i = 0; i < selectedConversation.SpeechBubbles.Count; i++)
        {
            if (i % k == 0)
            {
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
            }

            GUILayout.BeginVertical();
            if (selectedConversation.SpeechBubbles[i] != null)
            {
                Sprite icon = selectedConversation.SpeechBubbles[i].GetImage().sprite;
                if (icon != null)
                {
                    GUILayout.Box(icon.texture, GUILayout.Width(100f), GUILayout.Height(100f));
                }
            }
            else
            {
                selectedConversation.SpeechBubbles[i] = (SpeechBubble)EditorGUILayout.ObjectField(selectedConversation.SpeechBubbles[i], typeof(SpeechBubble), true, GUILayout.Width(100f));
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("-", GUILayout.Width(40f)))
            {
                selectedConversation.SpeechBubbles.RemoveAt(i);
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();
        EditorGUILayout.EndScrollView();

        GUILayout.BeginHorizontal();
        var style = new GUIStyle(normalStyle)
        {
            alignment = TextAnchor.UpperCenter
        };

        GUILayout.Label("Add SpeechBubble", style, GUILayout.Width(150f), GUILayout.Height(40f));

        if (GUILayout.Button("+", GUILayout.Width(40f)))
        {
            selectedConversation.AddSpeechBubble(null);
        }
    }

    private void ShowDialogueScreen(Rect rect)
    {
        scrollPosOfRightView = EditorGUILayout.BeginScrollView(scrollPosOfRightView, GUILayout.Width(rect.width), GUILayout.Height(rect.height - bottomHeight));
        GUILayout.BeginArea(new Rect(0, 0, rect.width, rect.height));
        GUILayout.BeginArea(new Rect(DialogueViewRectOffset, new Vector2(Mathf.Infinity, Mathf.Infinity)));
        ShowTree.Show(selectedConversation);
        GUILayout.EndArea();
        GUILayout.EndArea();
        EditorGUILayout.EndScrollView();

        if (isAddingDialogue && selectedDialogue != null)
        {
            string[] actorsName = new string[selectedConversation.ActorPrefabs.Count];
            string[] bubbleTalksName = new string[selectedConversation.SpeechBubbles.Count];

            for (int i = 0; i < selectedConversation.ActorPrefabs.Count; i++)
            {
                if (selectedConversation.ActorPrefabs[i] != null)
                {
                    actorsName[i] = selectedConversation.ActorPrefabs[i].CharacterName;
                }
                else
                {
                    actorsName[i] = "No Gameobject";
                }
            }

            for (int i = 0; i < selectedConversation.SpeechBubbles.Count; i++)
            {
                if (selectedConversation.SpeechBubbles[i] != null)
                {
                    bubbleTalksName[i] = selectedConversation.SpeechBubbles[i].SpeechBubbleName;
                }
                else
                {
                    bubbleTalksName[i] = "No GameObject";
                }
            }

            GUILayout.Label("SelectedNodeIndex : " + selectedDialogue.DialogueID);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Sentence :", normalStyle, GUILayout.Width(150f));
            sentence = EditorGUILayout.TextArea(sentence);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Select Actor :", normalStyle, GUILayout.Width(150f));
            selectedActorNameIndex = EditorGUILayout.Popup(selectedActorNameIndex, actorsName);
            characterName = actorsName[selectedActorNameIndex];
            GUILayout.Label("Select Bubble :", normalStyle, GUILayout.Width(150f));
            selectedBubbleTalkNameIndex = EditorGUILayout.Popup(selectedBubbleTalkNameIndex, bubbleTalksName);
            bubbleTalkName = bubbleTalksName[selectedBubbleTalkNameIndex];
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Add"))
            {
                AddDialogue();
            }

            if (GUILayout.Button("Cancel Dialogue"))
            {
                isAddingDialogue = false;
            }
        }

        if (Event.current.type == EventType.MouseDrag)
        {
            DialogueViewRectOffset += Event.current.delta;
            DialogueViewRectOffset.x = Mathf.Clamp(DialogueViewRectOffset.x, -Mathf.Infinity, 10);
            DialogueViewRectOffset.y = Mathf.Clamp(DialogueViewRectOffset.y, -Mathf.Infinity, 10);
            Repaint();
        }
    }

    private void ShowQuest(Rect rect)
    {
        scrollPosOfRightView = EditorGUILayout.BeginScrollView(scrollPosOfRightView, GUILayout.Width(rect.width), GUILayout.Height(rect.height - headerHeight));
        GUILayout.BeginHorizontal();
        int k = 5;
        for (int i = 0; i < selectedConversation.GetAllQuest().Count; i++)
        {
            if (i % k == 0)
            {
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
            }

            GUILayout.BeginVertical();
            if (selectedConversation.GetAllQuest()[i] != null)
            {
                GUILayout.Box(selectedConversation.GetAllQuest()[0].QuestName, GUILayout.Width(100f), GUILayout.Height(100f));
            }
            else
            {
                selectedConversation.GetAllQuest()[i] = (Knights.QuestSystem.Quest)EditorGUILayout.ObjectField(selectedConversation.GetAllQuest()[i], typeof(Knights.QuestSystem.Quest), true, GUILayout.Width(100f));
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("-", GUILayout.Width(40f)))
            {
                selectedConversation.GetAllQuest().RemoveAt(i);
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();
        EditorGUILayout.EndScrollView();

        GUILayout.BeginHorizontal();
        var style = new GUIStyle(normalStyle)
        {
            alignment = TextAnchor.UpperCenter
        };

        GUILayout.Label("Add Quest", style, GUILayout.Width(150f), GUILayout.Height(40f));

        if (GUILayout.Button("+", GUILayout.Width(40f)))
        {
            selectedConversation.AddQuest(null);
        }
    }

    private void AddConversation()
    {
        ConversationCreator.Init();
        Init();
    }

    private void RightClickMenuOfDialogues()
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("Add Dialogue"), false, AddOrDeleteDialogue, "Add");
        menu.AddItem(new GUIContent("Remove Dialogue Children"), false, AddOrDeleteDialogue, "Remove");

        menu.ShowAsContext();
    }

    private void AddOrDeleteDialogue(object id)
    {
        if((string)id == "Add")
        {
            isAddingDialogue = true;
        }
        else if ((string)id == "Remove")
        {
            RemoveNodeFromTree.AssigningNodesToStack(selectedConversation, selectedDialogue);
            RemoveNodeFromTree.RemoveNodes(selectedConversation);
        }
    }
    
    private void AddDialogue()
    {
        Dialogue newDialogue = new Dialogue(selectedConversation.Dialogues[selectedConversation.Dialogues.Count - 1].DialogueID + 1, characterName, sentence, bubbleTalkName);

        if (selectedDialogue == null)
        {
            Debug.Log("selectedNode is null...");
        }

        Debug.Log("Dialogue is added to DialogueID" + selectedDialogue.DialogueID + ", sentence : " + sentence);

        selectedConversation.AddDialogue(selectedDialogue, newDialogue);
        isAddingDialogue = false;
        CreateTreeStructure.AssignPositionToNodes(selectedConversation, nodeWidth, nodeHeight, nodeHorSpace, nodeVerSpace);
    }

    private Dialogue GetSelectedDialogue(Vector2 mousePosition)
    {
        foreach (var dialogue in selectedConversation.Dialogues)
        {
            Rect rect = new Rect(dialogue.Position + new Vector2(10, 30) + DialogueViewRectOffset, new Vector2(CreateTreeStructure.NodeWidth, CreateTreeStructure.NodeHeight));
            if(rect.Contains(mousePosition))
            {
                return dialogue;
            }
        }

        return null;
    }
}

public class ConversationCreator : EditorWindow
{
    static ConversationCreator window;
    string text;

    public static void Init()
    {
        window = (ConversationCreator)GetWindow(typeof(ConversationCreator));
        window.minSize = new Vector2(200f, 200f);
        window.maxSize = new Vector2(200f, 200f);
        window.titleContent = new GUIContent("ConversationEditor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Conversation Name : ");
        text = GUILayout.TextField(text);
        if (GUILayout.Button("Create"))
        {
            Conversation conversation = CreateInstance<Conversation>().Init(text);
            ConversationEditor.conversations.Add(conversation);
            ConversationEditor.selectedConversation = conversation;
            Debug.Log(conversation.ConversationName);
            AssetDatabase.CreateAsset(conversation, "Assets/Resources/Conversations/" + conversation.ConversationName + ".asset");
            AssetDatabase.SaveAssets();
            window.Close();
        }

        if(GUILayout.Button("Cancel"))
        {
            window.Close();
        }
    }
}

public sealed class CreateTreeStructure
{
    public static float ThresholdX = 10f;
    public static float ThresholdY = 10f;
    public static float NodeWidth = 100f;
    public static float NodeHeight = 100f;

    private static Conversation conversation;
    private static List<int> horDepthAtParticularLevel;
    private static float verticalSpace = 10f;
    private static float horizontalSpace = 20f;
    private static int breakCount = 0;

    public static void AssignPositionToNodes(Conversation _conversation, float _nodeWidth, float _nodeHeight, float _horizontalSpace, float _verticalSpace)
    {
        breakCount = 0;
        if (_conversation == null)
        {
            Debug.Log("Conversation is null...");
            return;
        }

        if(_conversation.Dialogues.Count <= 0)
        {
            Debug.Log("Conversation dialogue length is zero...");
            return;
        }

        conversation = _conversation;
        horDepthAtParticularLevel = new List<int>();

        NodeWidth = _nodeWidth;
        NodeHeight = _nodeHeight;
        horizontalSpace = _horizontalSpace;
        verticalSpace = _verticalSpace;

        Dialogue currentDialogue = _conversation.FindDialogueWithID(_conversation.Dialogues[0].DialogueID);
        AssignPosition(currentDialogue, 0);
    }

    private static void AssignPosition(Dialogue currentDialogue, int verDepth)
    {
        breakCount++;
        if (currentDialogue == null || breakCount > 200)
        {
            return;
        }

        if (verDepth >= horDepthAtParticularLevel.Count)
        {
            horDepthAtParticularLevel.Add(0);
        }
        else
        {
            horDepthAtParticularLevel[verDepth]++;
        }

        Debug.Log("Current node id : " + currentDialogue.DialogueID + " and VerDepth : " + verDepth + " and count : " + horDepthAtParticularLevel.Count);
        Debug.Log("Current level hor depth : " + horDepthAtParticularLevel[verDepth]);

        currentDialogue.Position = new Vector2(ThresholdX + horDepthAtParticularLevel[verDepth] * (horizontalSpace + NodeWidth), ThresholdY + verDepth * (verticalSpace + NodeHeight));

        foreach (int id in currentDialogue.ChildDialogueIDs)
        {
            Dialogue node = conversation.FindDialogueWithID(id);
            if (node == null)
            {
                currentDialogue.ChildDialogueIDs.Remove(id);
            }

            AssignPosition(node, verDepth + 1);
        }
    }
}

public sealed class ShowTree
{
    public static void Show(Conversation conversation)
    {
        foreach (Dialogue dialogue in conversation.Dialogues)
        {
            Rect rect = new Rect(dialogue.Position, new Vector2(CreateTreeStructure.NodeWidth, CreateTreeStructure.NodeHeight));
            EditorGUI.DrawRect(rect, Color.red);
            GUILayout.BeginArea(rect);
            GUILayout.BeginHorizontal();
            GUILayout.Label("ID :");
            GUILayout.Label(dialogue.DialogueID.ToString());
            GUILayout.Label("Name :");
            GUILayout.Label(dialogue.CharacterName);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Sentence : ");
            dialogue.EditSentence(EditorGUILayout.TextField(dialogue.Sentence));
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
            Handles.color = Color.black;
            foreach (var childID in dialogue.ChildDialogueIDs)
            {
                Dialogue childNode = conversation.FindDialogueWithID(childID);
                if (childNode != null)
                {
                    Handles.DrawLine(dialogue.Position + new Vector2(CreateTreeStructure.NodeWidth / 2f, CreateTreeStructure.NodeHeight), childNode.Position + new Vector2(CreateTreeStructure.NodeWidth / 2f, 0f));
                }
            }
        }
    }
}

public sealed class RemoveNodeFromTree
{
    private static Stack<Dialogue> nodeStack = new Stack<Dialogue>();

    public static void AssigningNodesToStack(Conversation conversation, Dialogue currentNode)
    {
        if (conversation == null || currentNode == null)
        {
            return;
        }

        nodeStack.Push(currentNode);

        foreach (var childID in currentNode.ChildDialogueIDs)
        {
            Dialogue dialogue = conversation.FindDialogueWithID(childID);
            if (dialogue != null)
            {
                AssigningNodesToStack(conversation, dialogue);
            }
        }
    }

    public static void RemoveNodes(Conversation nodeSystem)
    {
        int length = nodeStack.Count;

        for (int i = 0; i < length - 1; i++)
        {
            nodeSystem.RemoveDialogue(nodeStack.Pop());
        }

        Dialogue dialogue = nodeStack.Pop();
        for (int i = dialogue.ChildDialogueIDs.Count - 1; i >= 0; i--)
        {
            dialogue.RemoveChildID(dialogue.ChildDialogueIDs[i]);
        }
    }
}
