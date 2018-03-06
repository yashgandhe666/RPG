using UnityEngine;
using System.Collections.Generic;
using Knights.Characters;

namespace Knights.QuestSystem
{
    [CreateAssetMenu(fileName = "KillQuest", menuName = "QuestObjectives/kill", order = -5)]
    public class KillObjective : Objective
    {
        public int requiredNumberHaveToKilled;
        public int currentKilledEnemy;
        public List<Character> killedObjects;

        public KillObjective(string objName, string descript, int objID)
        {
            objName = objectiveName;
            descript = objectiveDescription;
            objectiveID = objID;
        }

        public override void CompareWithRequiredObject(Object _object)
        {
            foreach (var item in killedObjects)
            {
                Debug.Log("Compare : " + _object.GetType() + "\t Originally : " + item.GetType() + "\t" + _object.Equals(item));
                if (_object.GetType() == item.GetType())
                {
                    Debug.Log("Object is same...");
                    currentKilledEnemy++;
                }
                else
                {
                    Debug.Log("Object is not same...");
                }
            }
        }

        public override bool IsCompleted
        {
            get
            {
                return currentKilledEnemy >= requiredNumberHaveToKilled;
            }
            protected set { }
        }
    }
}