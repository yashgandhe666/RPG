using Knights.Characters.NPC;
using UnityEngine;

public class NPCGameManager : NPC
{
    private GameObject uiPanel;

    protected override void Start()
    {
        uiPanel = gameObject.transform.Find("UIPanel").gameObject;
        uiPanel.SetActive(false);
        base.Start();
    }

    public override void RecieveClickEvent(Collider2D collider)
    {
        if(FightManager.Instance != null)
        {
            return;
        }

        if (Collider2d == collider)
        {
            uiPanel.SetActive(true);
            Debug.Log("I touch " + Collider2d.gameObject.name);
        }
        else
        {
            uiPanel.SetActive(false);
        }
    }
}
