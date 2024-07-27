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

    public string hairStyle; 
    public string beardStyle;
    public string moustacheStyle;

    public CustomizationInfo()
    {
        sex = Sex.Male;
        skinColor = default(Color);
        hairColor = default(Color);
        furColor = default(Color);
        hairStyle = default(string);
        beardStyle = default(string);
        moustacheStyle = default(string);
    }

    public CustomizationInfo(
        Sex _sex,
        Color _skinColor, Color _hairColor, Color _furColor,
        string _hairStyle, string _beardStyle, string _moustacheStyle
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
