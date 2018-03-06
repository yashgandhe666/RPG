using UnityEngine;

namespace Knights.Items
{
    public abstract class Item : ScriptableObject
    {
        public string itemName;
        public Sprite itemSprite;

        public Item(string _name, Sprite _sprite)
        {
            itemName = _name;
            itemSprite = _sprite;
        }
    }
}