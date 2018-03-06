using UnityEngine;
using System.Collections.Generic;
using Knights.Items;

namespace Knights.QuestSystem
{
    [CreateAssetMenu(fileName = "Collect", menuName = "QuestObjectives/Collect", order = -5)]
    public class CollectObjective : Objective
    {
        public int requiredNumberOfCollection;
        public int currentCollectedItem;
        public List<Item> collectObjects;

        public CollectObjective(string objName, string descript, int objID)
        {
            objName = objectiveName;
            descript = objectiveDescription;
            objectiveID = objID;
        }

        public override void CompareWithRequiredObject(Object _object)
        {
            throw new System.Exception("Not implemented");
        }

        public override bool IsCompleted
        {
            get
            {
                return false;
            }
            protected set { }
        }
    }
}