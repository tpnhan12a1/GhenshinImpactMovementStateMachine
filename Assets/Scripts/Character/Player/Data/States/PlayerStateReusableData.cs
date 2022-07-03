using UnityEngine;
using System.Collections.Generic;
namespace GhenshinImpactMovement
{
    public class PlayerStateReusableData
    {
       public Vector2 MovementInput { get; set; }
        public float MovementSpeedModifier { get; set; } = 1f;
        public float MovementOnSlopeSpeedModifier { get; set; } = 1f;
        public float MovementDecelerationForce { get; set; } = 1f;
        public List<PlayerCameraRecenteringData> BackwardsCameraRecenteringData { get;set; }
        public List<PlayerCameraRecenteringData> SidewaysCameraRecenteringData { get;set; }
        public bool ShouldWalk { get; set; }
        public bool ShouldSprint { get; set; } 
        public PlayerRotationData RotationData { get;set; }
        private Vector3 currentTargetRotation;
        private Vector3 timeReachTargetRotation;
        private Vector3 damgedTargetRotationCurrentVelocity;
        private Vector3 damgedTargetRotationPassedTime;
        public ref Vector3 CurrentTargetRotation
        {
            get { return ref currentTargetRotation; }
        }
        public ref Vector3 TimeReachTargetRotation
        {
            get { return ref timeReachTargetRotation; }
        }
        public ref Vector3 DamgedTargetRotationCurrentVelocity
        {
            get { return ref damgedTargetRotationCurrentVelocity; }
        }
        public ref Vector3 DamgedTargetRotationPassedTime
        {
            get { return ref damgedTargetRotationPassedTime; }
        }

        public Vector3 CurrentJumpForce { get; set; }
       
        
    }
}
