using UnityEngine;
using System.Collections.Generic;
using Knights.Characters.Players;
using Knights.Powers;
using Knights.Enums;

namespace Knights.Characters.NPC.Enemies
{
    public class Enemy : NPC, IFighter
    {
        public bool IsItMyTurn { get; set; }
        public TypeOfFighter typeOfFighter { get; set; }

        public Power selectedPower;
        public List<Power> Powers = new List<Power>();

        private float delayInAttack;

        protected override void Start()
        {
            IsItMyTurn = false;
            delayInAttack = 1f;
            typeOfFighter = TypeOfFighter.Enemy;
            Initialize();
        }

        private void Update()
        {
            if(IsItMyTurn)
            {
                delayInAttack -= Time.deltaTime;
                if (delayInAttack <= 0f)
                {
                    Player whomToAttack = CalculateWhomToAttack();
                    if (whomToAttack != null)
                    {
                        Attack(whomToAttack);
                        IsItMyTurn = false;
                        delayInAttack = 1f;
                    }
                }
            }
        }

        private Player CalculateWhomToAttack()
        {
            if (FightManager.Instance == null)
            {
                return null;
            }

            int rand = Random.Range(0, 10);

            Player currentPlayer = null;

            foreach (Player item in FightManager.Instance.currentActivePlayers)
            {
                if(item.CharacterStats.Threat >= rand)
                {
                    currentPlayer = item;
                    break;
                }
            }

            if (currentPlayer == null)
            {
                currentPlayer = FightManager.Instance.currentActivePlayers[Random.Range(0, FightManager.Instance.currentActivePlayers.Count - 1)];
            }

            return currentPlayer;
        }

        public void Attack(Character whomToAttack)
        {
            if (!selectedPower.IsSpecialEffectOn)
            {
                //Debug.Log(charName + " attack " + whomToAttack.charName);
                selectedPower.DefaultAttack(gameObject, whomToAttack);
            }
            else
            {
                StartCoroutine(selectedPower.SpecialPowerTravel(gameObject, whomToAttack));
            }

            if (FightManager.Instance != null)
            {
                FightManager.Instance.whoseAttackingTurn = FightManager.WhoseTurn.None;
            }
        }

        public override void TakeDamage(int damageAmount)
        {
            base.TakeDamage(damageAmount);
            if (CharacterStats.Health <= 0)
            {
                if (FightManager.Instance != null)
                {
                    FightManager.Instance.RemoveCharacter(this, TypeOfFighter.Enemy);
                }

                if(QuestManager.Instance.IsCurrentlyQuestActivate)
                {
                    Debug.Log("Died and quest is activated...");
                    if (QuestManager.Instance.CurrentlyActiveQuest.QuestType == QuestTypes.Kill)
                    {
                        QuestManager.Instance.CompareObject(this);
                    }
                }
            }
        }
    }
}