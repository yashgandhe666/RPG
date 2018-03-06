using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClickable
{
    Collider2D Collider2d { get; set; }
    void RecieveClickEvent(Collider2D collider2d);
}
