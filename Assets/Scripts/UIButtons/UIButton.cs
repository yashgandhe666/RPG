using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class UIButton : MonoBehaviour, IClickable
{
    [HideInInspector]
    public Collider2D Collider2d { get; set; }

    protected virtual void Start()
    {
        Collider2d = GetComponent<Collider2D>();
        Collider2d.isTrigger = true;
        InputManager.Instance.OnClickOnScreen += RecieveClickEvent;
    }

    public abstract void RecieveClickEvent(Collider2D collider);

    private void OnDestroy()
    {
        InputManager.Instance.OnClickOnScreen -= RecieveClickEvent;
    }
}
