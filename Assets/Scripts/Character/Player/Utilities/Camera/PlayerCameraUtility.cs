using System;
using UnityEngine;
using Cinemachine;

namespace GhenshinImpactMovement
{
    [Serializable]
    public class PlayerCameraUtility
    {
        [field :SerializeField] public CinemachineVirtualCamera VirtualCamera { get; private set; }
        [field: SerializeField] public float DefaultHorizontalWaitTime { get; private set; } = 0f;
        [field: SerializeField] public float DefaultHorizontalRecenteringTime { get; private set; } = 4f;
        private CinemachinePOV cinemachinePOV;
        public void Initialize()
        {
            cinemachinePOV = VirtualCamera.GetCinemachineComponent<CinemachinePOV>();
        }
        public void EnableCentering(float waitime = -1f, float recenteringTime = -1f,float baseMovemmentSpeed= 1f, float movementSpeed  =1f )
        {
            cinemachinePOV.m_HorizontalRecentering.m_enabled = true;
            cinemachinePOV.m_HorizontalRecentering.CancelRecentering();
            if (waitime == -1f)
                waitime = DefaultHorizontalWaitTime;
            if(recenteringTime == -1f)
                recenteringTime = DefaultHorizontalRecenteringTime;

            recenteringTime = recenteringTime * baseMovemmentSpeed / movementSpeed; //tam su?t
            cinemachinePOV.m_HorizontalRecentering.m_WaitTime = waitime;
            cinemachinePOV.m_HorizontalRecentering.m_RecenteringTime = recenteringTime;
        }
        public void DisableRecentering()
        {
            cinemachinePOV.m_HorizontalRecentering.m_enabled = true;
        }
    }
}
