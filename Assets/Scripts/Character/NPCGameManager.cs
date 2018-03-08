using Knights.Characters.NPC;
using UnityEngine;

public class NPCGameManager : NPC
{
    private GameObject uiPanel;

    protected override void Start()
    {
        if (gameObject.transform.Find("UIPanel"))
            uiPanel = gameObject.transform.Find("UIPanel").gameObject;
        if (uiPanel != null)
            uiPanel.SetActive(false);
        base.Start();
    }

    public override void RecieveClickEvent(Collider2D collider)
    {
        if (Collider2d == collider)
        {
            CanvasUIHandler.Instance.DisbleSelectedPlayerStats();
        }

        if (uiPanel == null)
        {
            return;
        }

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
