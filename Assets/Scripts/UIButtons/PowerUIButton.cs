using UnityEngine;
using Knights.Characters.Players;

public class PowerUIButton : UIButton
{
    [HideInInspector]
    public int powerIndex;

    protected override void Start()
    {
        base.Start();
    }

    public override void RecieveClickEvent(Collider2D collider)
    {
        if(Collider2d == collider)
        {
            Player player = GetComponentInParent<Player>();
            if(player != null)
            {
                player.SelectPower(powerIndex);
            }
        }
    }
}
