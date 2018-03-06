using UnityEditor;
using Knights.QuestSystem;

[CustomEditor(typeof(Quest))]
public class RenameQuestObject : Editor
{
    private Quest quest;

    private void OnEnable()
    {
        quest = target as Quest;
    }

    private void OnDisable()
    {
        string path = AssetDatabase.GetAssetPath(quest);
        AssetDatabase.RenameAsset(path, quest.QuestName + ".asset");
    }
}