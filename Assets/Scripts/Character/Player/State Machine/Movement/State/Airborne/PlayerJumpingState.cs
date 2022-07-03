using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GhenshinImpactMovement
{
    public class PlayerJumpingState : PlayerAirBorneState
    {
        private PlayerJumpData jumpData;
        private bool shouldKeepRotating;
        private bool canStartFalling;
        public void SetJumpData()
        {
            jumpData = airBorneData.JumpData;
        }
        public override StateType GetStateType()
        {
            return StateType.Jumping;
        }
        #region IState Methods
        public override void OnEnterState()
        {
            base.OnEnterState();
            ((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementSpeedModifier = 0f;
            ((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementDecelerationForce = jumpData.DecelerationForce;
            shouldKeepRotating = ((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementInput != Vector2.zero;
            Jump();
        }
        #endregion
        public override void OnPhysicsUpdate()
        {
            base.OnPhysicsUpdate();
            if (shouldKeepRotating)
            {
                RotateTowardsTargetRotation();
            }
            if(IsMovingUp())
            {
                DecelerateVertical();
            }    
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            if(!canStartFalling && IsMovingUp(0f))
            {
                canStartFalling = true;
            }    
            if (!canStartFalling || GetPlayerVerticalVelocity().y > 0) return;
            _stateMachine.ChangeState(((PlayerMovementStateMachine)_stateMachine).states[StateType.Falling]);

        }
        public override void OnExitState()
        {
            base.OnExitState();
            SetBaseRotatonData();
            canStartFalling = false;
        }
        #region Reusable Methods
        protected override void ResetSprintState()
        {
            
        }
        #endregion
        #region Main Methods
        protected void Jump()
        {
            Vector3 JumpForce = ((PlayerMovementStateMachine)_stateMachine).ReusableData.CurrentJumpForce;
            Vector3 JumpDirection = ((PlayerMovementStateMachine)_stateMachine).player.transform.forward;

            if(shouldKeepRotating)
            {
                UpdateTargetRotation(GetMovementDirection());
                JumpDirection = GetTargetRotaionDirection(((PlayerMovementStateMachine)_stateMachine).ReusableData.CurrentTargetRotation.y);
            }    
            JumpForce.x*=JumpDirection.x;
            JumpForce.z *= JumpDirection.z;
            Vector3 capsuleCollierCenterInWorld = ((PlayerMovementStateMachine)_stateMachine).player.ColliderUtility.CapsuleColliderData.Collider.bounds.center;
            Ray downwardsRayFromCapsuleCenter = new Ray(capsuleCollierCenterInWorld, Vector3.down);
            if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, jumpData.JumpToGroundRayDistance, ((PlayerMovementStateMachine)_stateMachine).player.LayerData.GroundedLayer, QueryTriggerInteraction.Ignore))
            {
                float groundAngle = Vector2.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);
                if(IsMovingUp())
                {
                    float forceModifier = jumpData.JumpForceModifierOnSlopeUpwards.Evaluate(groundAngle);
                    JumpForce.x *= forceModifier;
                    JumpForce.z *= forceModifier;
                }    
                if(IsMovingDown())
                {
                    float forceModifier = jumpData.JumpForceModifierOnSlopeDownwards.Evaluate(groundAngle);
                    JumpForce.y *= forceModifier;
                }    
            }
             
            ResetVelocity();
            ((PlayerMovementStateMachine)_stateMachine).player.Rigidbody.AddForce(JumpForce,ForceMode.VelocityChange);
        }
        #endregion
        #region Input Methods
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
        }
        #endregion
    }
}
