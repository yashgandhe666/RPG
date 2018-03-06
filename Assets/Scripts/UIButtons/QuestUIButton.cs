using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUIButton : UIButton
{
    protected override void Start()
    {
        base.Start();
        CanvasUIHandler.Instance.QuestUIPanel.SetActive(false);
    }

    public override void RecieveClickEvent(Collider2D collider)
    {
        if (Collider2d == collider)
        {
            if (!QuestManager.Instance.IsCurrentlyQuestActivate)
            {
                CanvasUIHandler.Instance.ShowCurrentlyAvilableQuests();// QuestUIPanel.SetActive(true);
            }
            else
            {
                CanvasUIHandler.Instance.ShowCurrentQuestDescription();
            }
            Debug.Log("I touch " + Collider2d.gameObject.name);
        }
    }
}
