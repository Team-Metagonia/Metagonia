using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttachable
{
    public void Attach(Item item1, Item item2);

    public void ShowAttachableArea(bool isActive);
    
    
}
