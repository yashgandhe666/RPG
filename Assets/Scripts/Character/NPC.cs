using UnityEngine;

namespace Knights.Characters.NPC
{
    public class NPC : Character
    {
        protected virtual void Start()
        {
            GameManager.Instance.Peoples.Add(this);
            Initialize();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void Die()
        {
            QuestManager questManager = QuestManager.Instance.GetComponent<QuestManager>();
            if(questManager != null)
            {
                if (questManager.IsCurrentlyQuestActivate)
                {
                    questManager.CompareObject(this);
                }
            }
            
            base.Die();
        }

        public override void RecieveClickEvent(Collider2D collider)
        {
            if (Collider2d == collider)
            {
                CanvasUIHandler.Instance.DisbleSelectedPlayerStats();
            }
        }
    }
}
