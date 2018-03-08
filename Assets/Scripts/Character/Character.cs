using UnityEngine;

namespace Knights.Characters
{
    public abstract class Character : MonoBehaviour, IClickable
    {
        public string CharacterName;
        public CharacterStatsWithSO CharacterStatsWithSO;

        [HideInInspector]
        public CharacterStats CharacterStats;

        public Collider2D Collider2d { get; set; }

        protected virtual void Initialize()
        {
            Collider2d = GetComponent<Collider2D>();
            InputManager.Instance.OnClickOnScreen += RecieveClickEvent;

            if(CharacterStatsWithSO == null)
            {
                // Debug.Log("Character Stats with ScriptableObject is null...");
                return;
            }

            CharacterStats = CharacterStatsWithSO.Copy(new CharacterStats());
        }

        public virtual void TakeDamage(int damageAmount)
        {
            //Debug.Log("I, " + charName + " am hurt... and " + damageAmount + " amount take damage...");
            CharacterStats.Health -= damageAmount;
            if(CharacterStats.Health <= 0)
            {
                //Debug.Log("I," + charName + " died..");
                Die();
            }
        }

        protected virtual void Die()
        {
            gameObject.SetActive(false);
        }

        protected virtual void OnDestroy()
        {
            InputManager.Instance.OnClickOnScreen -= RecieveClickEvent;
        }

        public abstract void RecieveClickEvent(Collider2D collider);
    }
}
