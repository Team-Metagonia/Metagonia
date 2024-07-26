using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SelectionMode
{
    Sex, Style
}

public enum Sex
{
    Male, Female
}

public class CustomizationInfo
{
    // Sex Info
    public Sex sex;

    // Style Info
    public Color skinColor;
    public Color hairColor;
    public Color furColor;

    public Mesh hairStyle; 
    public Mesh beardStyle;
    public Mesh moustacheStyle;

    public CustomizationInfo()
    {
        sex = Sex.Male;
        skinColor = default(Color);
        hairColor = default(Color);
        furColor = default(Color);
        hairStyle = null;
        beardStyle = null;
        moustacheStyle = null;
    }

    public CustomizationInfo(
        Sex _sex,
        Color _skinColor, Color _hairColor, Color _furColor,
        Mesh _hairStyle, Mesh _beardStyle, Mesh _moustacheStyle
    )
    {   
        this.sex = _sex;
        
        this.skinColor = _skinColor;
        this.hairColor = _hairColor;
        this.furColor = _furColor;
        
        this.hairStyle = _hairStyle;
        this.beardStyle = _beardStyle;
        this.moustacheStyle = _moustacheStyle;
    }
}
