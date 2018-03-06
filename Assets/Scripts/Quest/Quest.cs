using System.Collections.Generic;
using UnityEngine;
using Knights.Enums;

namespace Knights.QuestSystem
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Quest", order = -6)]
    public class Quest : ScriptableObject
    {
        public string QuestName;
        public QuestTypes QuestType;
		public List<Objective> objectives;

        public Quest(Objective _objective)
        {
            objectives = new List<Objective>();
            objectives.Add(_objective);
        }

        public Quest(List<Objective> _objectives)
        {
            objectives = new List<Objective>();
            objectives.AddRange(_objectives);
        }
    }
}