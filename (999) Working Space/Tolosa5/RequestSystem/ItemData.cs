using System;
using System.Collections;
using System.Collections.Generic;
using _999__Working_Space.Tolosa5.Inventory.Model;
using UnityEngine;

[CreateAssetMenu(fileName = "RequestData", menuName = "RequestSystem/RequestData")]
public class ItemData : ScriptableObject
{
    public readonly int Index = Guid.NewGuid().GetHashCode();
    
    public ObjectType itemType;
    public Sprite requestedSprite;
    
    //public Sprite[] possibleSprites;
}
