using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightUIButton : UIButton
{
    protected override void Start()
    {
        base.Start();
        CanvasUIHandler.Instance.FightUIPanel.SetActive(false);
    }

    public override void RecieveClickEvent(Collider2D collider)
    {
        if (Collider2d == collider)
        {
            if (GameManager.Instance.GetComponentInChildren<FightManager>() == null)
            {
                CreateNewFight();
            }
            
            Debug.Log("I touch " + Collider2d.gameObject.name);
        }
    }

    public static void CreateNewFight()
    {
        GameObject gm = new GameObject();
        gm.transform.SetParent(GameManager.Instance.gameObject.transform);
        gm.name = "FigthingManager";
        gm.AddComponent<FightManager>();
        CanvasUIHandler.Instance.FightUIPanel.gameObject.SetActive(true);
    }
}
