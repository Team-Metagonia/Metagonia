using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Polyperfect.PrehistoricAnimals
{
    public class PanelControl : MonoBehaviour
    {
        public void PanelToggle()
        {
            if (this.gameObject.activeSelf)
            {
                this.gameObject.SetActive(false);

            }
            else
            {
                this.gameObject.SetActive(true);
            }
        }
    }
}
