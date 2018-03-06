using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knights.QuestSystem
{
    public abstract class Objective : ScriptableObject
    {
        [SerializeField]
        public string objectiveName;
        [SerializeField]
        protected string objectiveDescription;
        [SerializeField]
        protected int objectiveID;

        public abstract void CompareWithRequiredObject(Object _object);

        public abstract bool IsCompleted { get; protected set; }
    }
}
