using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasButtonManager : MonoBehaviour
{
    public static CanvasButtonManager Instance { get { return instance; } private set { } }

    public Button FightButton;
    public Button ToMapButton;
    public Button ToVillageButton;
    public GameObject MapGameObject;
    
    public GameObject currentlyActiveScene;
    public GameObject currentlyActiveVillage;

    private static CanvasButtonManager instance;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        if(FightButton == null)
        {
            Debug.LogWarning("Fight Button is null...");
        }
        FightButton.onClick.AddListener(delegate { FightUIButton.CreateNewFight(); });
        ToVillageButton.onClick.AddListener(delegate { ToVillage(currentlyActiveVillage, currentlyActiveScene); });
        ToMapButton.onClick.AddListener(delegate { ToMap(MapGameObject, currentlyActiveScene); });
    }

    private void ToVillage(GameObject enabledGm, GameObject disabledGm)
    {
        currentlyActiveScene = enabledGm;
        FightButton.gameObject.SetActive(true);
        ToMapButton.gameObject.SetActive(true);
        ToVillageButton.gameObject.SetActive(false);
        enabledGm.SetActive(true);
        disabledGm.SetActive(false);
    }

    private void ToMap(GameObject enabledGm, GameObject disabledGm)
    {
        currentlyActiveScene = enabledGm;
        FightButton.gameObject.SetActive(false);
        ToMapButton.gameObject.SetActive(false);
        ToVillageButton.gameObject.SetActive(false);
        enabledGm.SetActive(true);
        disabledGm.SetActive(false);
    }

    private void Fight()
    {

    }
}
