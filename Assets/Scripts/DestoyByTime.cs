using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoyByTime : MonoBehaviour
{
    public float delayInDestroy = 1f;

    void Start()
    {
        Destroy(gameObject, delayInDestroy);
    }
}
