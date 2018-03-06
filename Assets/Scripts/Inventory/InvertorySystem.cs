using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertorySystem : MonoBehaviour
{
    public static InvertorySystem Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void AddItem()
    {

    }
}
