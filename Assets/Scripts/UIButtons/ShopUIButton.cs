using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUIButton : UIButton
{
    protected override void Start()
    {
        base.Start();
    }

    public override void RecieveClickEvent(Collider2D collider)
    {
        if (Collider2d == collider)
        {
            Debug.Log("I touch " + Collider2d.gameObject.name);
        }
    }
}
