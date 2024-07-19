using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttachable
{
    public void Attach(item item1, item item2);

    public void ShowAttachableArea(bool isActive);
    
    
}
