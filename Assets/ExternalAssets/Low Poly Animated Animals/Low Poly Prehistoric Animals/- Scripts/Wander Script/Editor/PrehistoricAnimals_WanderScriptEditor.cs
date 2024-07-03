using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Polyperfect.Common;

#if UNITY_EDITOR

namespace Polyperfect.PrehistoricAnimals
{
    [CustomEditor(typeof(PrehistoricAnimals_WanderScript))]
    [CanEditMultipleObjects]
    public class PrehistoricAnimals_WanderScriptEditor : Common_WanderScriptEditor { }
}

#endif