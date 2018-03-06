using UnityEditor;
using Knights.QuestSystem;

[CustomEditor(typeof(Objective), true)]
public class RenameObjectiveObject : Editor
{
    private Objective objective;

    private void OnEnable()
    {
        UnityEngine.Debug.Log("Enable objective...");
        objective = target as Objective;
    }

    private void OnDisable()
    {
        string path = AssetDatabase.GetAssetPath(objective);
        AssetDatabase.RenameAsset(path, objective.objectiveName + ".asset");
    }
}
