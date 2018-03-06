using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Knights.Powers;

[CustomEditor(typeof(Power))]
public class PowerCustomInspector : Editor
{
    Power power;

    private void OnEnable()
    {
        power = target as Power;
    }

    public override void OnInspectorGUI()
    {
        if (power == null)
        {
            Debug.Log("Power object selected is null...");
            return;
        }
        GUILayout.BeginHorizontal();
        GUILayout.Label("Power Name : ", GUILayout.Width(Screen.width / 2));
        power.PowerName = EditorGUILayout.TextArea(power.PowerName);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Icon : ", GUILayout.Width(Screen.width / 4f));
        power.Icon = (Sprite)EditorGUILayout.ObjectField(power.Icon, typeof(Sprite), false, GUILayout.Width(Screen.width / 4f));
        if (power.Icon != null)
        {
            GUILayout.Box(power.Icon.texture, GUILayout.Width(Screen.width / 3), GUILayout.Height(Screen.width / 3));
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("DamageAmount : ");
        power.DamageAmount = EditorGUILayout.IntSlider(power.DamageAmount, 0, 100);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("EnergyRequired : ");
        power.EnergyRequired = EditorGUILayout.IntSlider(power.EnergyRequired, 0, 100);
        GUILayout.EndHorizontal();

        power.IsSpecialEffectOn = EditorGUILayout.BeginToggleGroup("Is Special Power : ", power.IsSpecialEffectOn);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Special Power Effect : ", GUILayout.Width(Screen.width / 2.5f));
        power.SpecialPowerEffect = (GameObject)EditorGUILayout.ObjectField(power.SpecialPowerEffect, typeof(GameObject), false);
        GUILayout.EndHorizontal();
        EditorGUILayout.EndToggleGroup();

        power.IsSpecialEffectOn = !EditorGUILayout.BeginToggleGroup("Is Default Power : ", !power.IsSpecialEffectOn);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Default Power Effect : ", GUILayout.Width(Screen.width / 2.5f));
        power.DefaultPowerEffect = (GameObject)EditorGUILayout.ObjectField(power.DefaultPowerEffect, typeof(GameObject), false);
        GUILayout.EndHorizontal();
        EditorGUILayout.EndToggleGroup();
        GUILayout.Button("ADDD");
    }
}
