using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.CrossPlatformInput
{
    public class ButtonHandler : MonoBehaviour
    {
        private Touch tempTouchs;
        private Vector3 touchedPos;

        public string Name;

        void OnEnable()
        {

        }

        public void SetDownState()
        {
            CrossPlatformInputManager.SetButtonDown(Name);
        }


        public void SetUpState()
        {
            CrossPlatformInputManager.SetButtonUp(Name);
        }


        public void SetAxisPositiveState()
        {
            CrossPlatformInputManager.SetAxisPositive(Name);
        }


        public void SetAxisNeutralState()
        {
            CrossPlatformInputManager.SetAxisZero(Name);
        }


        public void SetAxisNegativeState()
        {
            CrossPlatformInputManager.SetAxisNegative(Name);
        }

        void Update()
        {

        }
    }
}

