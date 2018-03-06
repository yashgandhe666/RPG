using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "CharacterStats", order = -10)]
public class CharacterStatsWithSO : ScriptableObject
{
    public Sprite CharIcon;

    [SerializeField]
    public int Health;
    [SerializeField]
    public int Energy;
    [SerializeField]
    public int Initiative;
    [SerializeField]
    public int Threat;
    [SerializeField]
    public int ShieldPower;

    public CharacterStats Copy(CharacterStats characterStats)
    {
        characterStats.CharIcon = CharIcon;
        characterStats.Health = Health;
        characterStats.Energy = Energy;
        characterStats.Initiative = Initiative;
        characterStats.Threat = Threat;
        characterStats.ShieldPower = ShieldPower;
        return characterStats;
    }
}
