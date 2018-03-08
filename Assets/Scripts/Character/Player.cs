using System.Collections.Generic;
using Knights.Characters.NPC.Enemies;
using Knights.Powers;
using UnityEngine;
using Knights.Enums;

namespace Knights.Characters.Players
{
    public class Player : Character, IFighter
	{
        public bool IsItMyTurn { get; set; }
        public TypeOfFighter typeOfFighter { get; set; }

        [SerializeField]
        private GameObject uiContainer;
        [SerializeField]
        private GameObject powerUI;
        [SerializeField]
        private float size;
        [SerializeField]
        private float gap;
        [SerializeField]
        private Power selectedPower;
        [SerializeField]
        private List<Power> powers = new List<Power>();

        private List<GameObject> powerUIGameObjects = new List<GameObject>();

        private void Start()
        {
            if (powers.Count <= 0)
            {
                selectedPower = Resources.Load<Power>("Power/" + "Default");
            }

            IsItMyTurn = false;
            typeOfFighter = TypeOfFighter.Player;
            //GameManager.Instance.AddPlayer(this);
            Initialize();
        }

        public void SelectPower(int index)
        {
            selectedPower = powers[index];
        }

        public void Attack(Character whomToAttack)
        {
            //Debug.Log("I," + charName + " attack " + whomToAttack.charName);
            if (selectedPower.IsSpecialEffectOn)
            {
                StartCoroutine(selectedPower.SpecialPowerTravel(gameObject, whomToAttack));
            }
            else
            {
                selectedPower.DefaultAttack(gameObject, whomToAttack);
                if (FightManager.Instance != null)
                {
                    FightManager.Instance.whoseAttackingTurn = FightManager.WhoseTurn.None;
                }
            }

            IsItMyTurn = false;
            CharacterStats.Energy -= selectedPower.EnergyRequired;
        }

        public override void TakeDamage(int damageAmount)
        {
            base.TakeDamage(damageAmount);
            if (CharacterStats.Health <= 0)
            {
                if (FightManager.Instance != null)
                {
                    FightManager.Instance.RemoveCharacter(this, TypeOfFighter.Player);
                }
            }
        }

        private void ShowPowersUI()
        {
            DestroyPowerUI();
            float startX_pos = 0f;
            int length = powers.Count;

            if (length%2 == 0)
            {
                startX_pos = (length / 2 - 1) * 2 * size + size + (length / 2 - 1) * gap + gap / 2.0f;
            }
            else
            {
                int n = length / 2;
                startX_pos = (length - 1) * size + n * gap;
            }

            float x_pos = startX_pos;

            for (int i = 0; i < length; i++)
            {
                GameObject gm = Instantiate(powerUI, Vector3.zero, Quaternion.identity);
                powerUIGameObjects.Add(gm);
                gm.transform.SetParent(uiContainer.transform);
                gm.transform.position = new Vector3(uiContainer.transform.position.x - x_pos, uiContainer.transform.position.y, 0f);
                gm.transform.localScale = Vector3.one;
                gm.transform.Find("Icon").GetComponent<SpriteRenderer>().sprite = powers[i].Icon;
                PowerUIButton pwButton = gm.AddComponent<PowerUIButton>();
                pwButton.powerIndex = i;
                x_pos -= (2 * size + gap);
            }
        }

        private void DestroyPowerUI()
        {
            foreach (var item in powerUIGameObjects)
            {
                Destroy(item);
            }
            powerUIGameObjects.Clear();
        }

        public override void RecieveClickEvent(Collider2D collider)
        {
            if (!gameObject.activeSelf)
                return;

            if (Collider2d == collider)
            {
                CanvasUIHandler.Instance.ShowSelectedPlayerStats(CharacterStats);
                ShowPowersUI();
            }
            else
            {
                DestroyPowerUI();
            }

            if (IsItMyTurn && FightManager.Instance != null)
            {
                if (collider != null)
                {
                    if (selectedPower.EnergyRequired > CharacterStats.Energy)
                        return;

                    Enemy enemy = collider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        Attack(enemy);
                    }
                }
            }
        }
    }
}