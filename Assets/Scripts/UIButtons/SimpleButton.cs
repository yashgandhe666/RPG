using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleButton : UIButton
{
    public GameObject ActivateScene;
    public GameObject DeactivateScene;

    public string toScenename;

    protected override void Start()
    {
        base.Start();
    }

    public override void RecieveClickEvent(Collider2D collider)
    {
        if(collider == Collider2d)
        {
            ActivateScene.SetActive(true);
            CanvasButtonManager.Instance.currentlyActiveScene = ActivateScene;
            if (toScenename == "House")
            {
                CanvasButtonManager.Instance.FightButton.gameObject.SetActive(false);
                CanvasButtonManager.Instance.ToMapButton.gameObject.SetActive(false);
                CanvasButtonManager.Instance.ToVillageButton.gameObject.SetActive(true);
            }
            else if(toScenename == "Village")
            {
                CanvasButtonManager.Instance.currentlyActiveVillage = ActivateScene;
                CanvasButtonManager.Instance.FightButton.gameObject.SetActive(true);
                CanvasButtonManager.Instance.ToMapButton.gameObject.SetActive(true);
                CanvasButtonManager.Instance.ToVillageButton.gameObject.SetActive(false);
            }
            else if(toScenename == "Map")
            {
                CanvasButtonManager.Instance.FightButton.gameObject.SetActive(false);
                CanvasButtonManager.Instance.ToMapButton.gameObject.SetActive(false);
                CanvasButtonManager.Instance.ToVillageButton.gameObject.SetActive(true);
            }
            else
            {
                CanvasButtonManager.Instance.FightButton.gameObject.SetActive(false);
                CanvasButtonManager.Instance.ToMapButton.gameObject.SetActive(false);
                CanvasButtonManager.Instance.ToVillageButton.gameObject.SetActive(false);
            }

            DeactivateScene.SetActive(false);
        }
    }
}
