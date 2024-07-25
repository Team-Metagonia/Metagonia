using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurMenu : CustomizationMenu
{
    public Transform randomInFurMenu;
    public Transform emptyInFurMenu;
    public Transform generalBeardsInFurMenu;
    public Transform generalMustachesInFurMenu;
    
    private Transform beardMeshTransformInCharacter;
    private Transform mustacheMeshTransformInCharacter;

    public override void Initialize(CharacterCustomization character)
    {
        InjectCustomCharacter(character);
    }


    public Transform randomHairInHairMenu;
    public Transform generalHairsInHairMenu;

}
