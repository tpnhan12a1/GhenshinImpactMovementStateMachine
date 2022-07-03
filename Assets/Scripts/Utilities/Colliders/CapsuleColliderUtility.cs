using System;
using UnityEngine;

namespace GhenshinImpactMovement
{
    [Serializable]
    public class CapsuleColliderUtility
    {
        public CapsuleColliderData CapsuleColliderData { get; set; }
        [field:SerializeField] public DefaultColliderData DefaultColliderData { get; set; }
        [field: SerializeField] public SlopeData SlopeData { get; set; }

        public void Initialize(GameObject gameObject)
        {
            if(CapsuleColliderData!= null)
            {
                return;
            }    
            CapsuleColliderData = new CapsuleColliderData();
            CapsuleColliderData.Initialize(gameObject);
            OnInitialize();
        }
        protected virtual void OnInitialize()
        {

        }
        public void CapsuleCollierDimensions()
        {
            SetCapsuleColliderRadious(DefaultColliderData.Radious);
            SetCapsuleColliderHeight(DefaultColliderData.Height * (1-SlopeData.StepHeightPercentage));
            RecalculateCapsuleColliderCenter(); 
            float halfColliderHeight = DefaultColliderData.Height / 2;
            if(halfColliderHeight <CapsuleColliderData.Collider.radius)
            {
                SetCapsuleColliderRadious(halfColliderHeight);
            }
            CapsuleColliderData.UpdateColliderData();
        }

        public void RecalculateCapsuleColliderCenter()
        {
            float colliderHeightDifference = DefaultColliderData.Height - CapsuleColliderData.Collider.height;
            Vector3 newColliderCenter = new Vector3(0f, DefaultColliderData.CenterY + (colliderHeightDifference / 2), 0f);
            CapsuleColliderData.Collider.center = newColliderCenter;
        }

        public void SetCapsuleColliderRadious(float radious)
        {
            CapsuleColliderData.Collider.radius = radious;
        }
        public void SetCapsuleColliderHeight(float height)
        {
            CapsuleColliderData.Collider.height = height;
        }


    }
}
