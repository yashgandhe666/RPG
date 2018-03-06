using UnityEngine;
using System.Collections;
using Knights.Characters;

namespace Knights.Powers
{
    [CreateAssetMenu(fileName = "Default", menuName = "Power", order = -9)]
    public class Power : ScriptableObject
    {
        [SerializeField]
        public string PowerName;
        [SerializeField]
        public Sprite Icon;
        [SerializeField]
        public int DamageAmount, EnergyRequired;
        [SerializeField]
        public bool IsSpecialEffectOn;
        [SerializeField]
        public GameObject DefaultPowerEffect;
        [SerializeField]
        public GameObject SpecialPowerEffect;

        public void DefaultAttack(GameObject owner,Character whomToAttack)
        {
            whomToAttack.TakeDamage(DamageAmount);
            Instantiate(DefaultPowerEffect, whomToAttack.transform.position, Quaternion.identity);
        }

        public IEnumerator SpecialPowerTravel(GameObject owner, Character whomToAttack)
        {
            int i = 0;

            if (SpecialPowerEffect == null)
            {
                Debug.Log("Special power is null...");
                yield return null;
            }
            else
            {
                GameObject effectGO = Instantiate(SpecialPowerEffect, owner.transform.position, Quaternion.identity);
                effectGO.transform.LookAt(whomToAttack.transform);
                effectGO.SetActive(true);
                while (true)
                {
                    i++;
                    if (i > 10000)
                    {
                        break;
                    }

                    effectGO.transform.position += effectGO.transform.forward * Time.deltaTime * 10f;

                    if (Vector3.Distance(effectGO.transform.position, whomToAttack.transform.position) < 0.1f)
                    {
                        break;
                    }

                    yield return null;
                }

                whomToAttack.TakeDamage(DamageAmount);
                Destroy(effectGO);
                if (FightManager.Instance != null)
                {
                    FightManager.Instance.whoseAttackingTurn = FightManager.WhoseTurn.None;
                }
                yield return null;
            }
        }
    }
}
