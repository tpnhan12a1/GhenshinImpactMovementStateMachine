using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace GhenshinImpactMovement
{
    public abstract class PlayerGroundedState : PlayerMovementState
    {
        private SlopeData slopeData= new SlopeData();
        public void SetSlopeData(SlopeData slopeData)
        {
            this.slopeData = slopeData;
        }
        #region IState Methods
        public override void OnEnterState()
        {
            base.OnEnterState();
            StartAnimation(((PlayerMovementStateMachine)_stateMachine).player.AnimationData.GroundedParameterHash);
            UpdateShouldSprintState();
            UpdateCameraRecenteringState(((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementInput);
        }
        public override void OnExitState()
        {
            base.OnExitState();
            StopAnimation(((PlayerMovementStateMachine)_stateMachine).player.AnimationData.GroundedParameterHash);
        }
        private void UpdateShouldSprintState()
        {
            if(!((PlayerMovementStateMachine)_stateMachine).ReusableData.ShouldSprint)
            {
                return;
            }  
            if(((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementInput != Vector2.zero)
            {
                return;
            }
            ((PlayerMovementStateMachine)_stateMachine).ReusableData.ShouldSprint = false;
        }

        public override void OnPhysicsUpdate()
        {
            base.OnPhysicsUpdate();
            Float();
        }

        
        private void Float()
        {
            Vector3 CapsuleColliderCenterInSpace = ((PlayerMovementStateMachine)_stateMachine).player.ColliderUtility.CapsuleColliderData.Collider.bounds.center;
            Ray downwardRayFromCapsuleCenter = new Ray(CapsuleColliderCenterInSpace, Vector3.down);
            if(Physics.Raycast(downwardRayFromCapsuleCenter, out RaycastHit hit, slopeData.FloatRayDistance,
               ((PlayerMovementStateMachine)_stateMachine).player.LayerData.GroundedLayer,QueryTriggerInteraction.Ignore))
            {
                float groundAngle = Vector3.Angle(hit.normal,-downwardRayFromCapsuleCenter.direction);
                float slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);

                if (slopeSpeedModifier == 0f) return;

                float distanceToFloatingPoint = ((PlayerMovementStateMachine)_stateMachine).player.ColliderUtility.CapsuleColliderData.ColliderCenterInLocalSpace.y * ((PlayerMovementStateMachine)_stateMachine).player.transform.localScale.y - hit.distance;
                if (distanceToFloatingPoint == 0) return;
                float amountToLift = distanceToFloatingPoint * slopeData.StepReachForce - GetPlayerVerticalVelocity().y;
                Vector3 liftForce = new Vector3(0,amountToLift, 0);
               ( (PlayerMovementStateMachine)_stateMachine).player.Rigidbody.AddForce(liftForce,ForceMode.VelocityChange);
            }    
            
        }


        #endregion
        #region Resusable Methods
        protected float SetSlopeSpeedModifierOnAngle(float angle)
        {
            float slopeSpeedModifier = movementData.SlopeSpeedAngle.Evaluate(angle);

            if(((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementOnSlopeSpeedModifier != slopeSpeedModifier)
            {
                ((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementOnSlopeSpeedModifier = slopeSpeedModifier;
                UpdateCameraRecenteringState(((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementInput);
            }    
            return slopeSpeedModifier;
        }
        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();
            
            ((PlayerMovementStateMachine)_stateMachine).player.Input.PlayerActions.Dash.started += OnDashStarted;
            ((PlayerMovementStateMachine)_stateMachine).player.Input.PlayerActions.Jump.started += OnJumpStarted;

        }
        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();
           
            ((PlayerMovementStateMachine)_stateMachine).player.Input.PlayerActions.Jump.started -= OnJumpStarted;
        }
        protected virtual void OnMove()
        {
            if(((PlayerMovementStateMachine)_stateMachine).ReusableData.ShouldSprint)
            {
                _stateMachine.ChangeState(((PlayerMovementStateMachine)_stateMachine).states[StateType.Sprinting]);
                return;
            }    
            if (((PlayerMovementStateMachine) _stateMachine).ReusableData.ShouldWalk)
            {
                _stateMachine.ChangeState(((PlayerMovementStateMachine)_stateMachine).states[StateType.Walking]);
                return;
            }
            _stateMachine.ChangeState(((PlayerMovementStateMachine)_stateMachine).states[StateType.Running]);
        }
        protected override void OnContactWithGroundExited(Collider collider)
        {
            base.OnContactWithGound(collider);
            if (IsThereGroundUnderneath()) return;

            Vector3 capsuleColloderCenterInWorld = ((PlayerMovementStateMachine)_stateMachine).player.ColliderUtility.CapsuleColliderData.Collider.bounds.center;
            Ray downwardRayFromCapsuleBottom = new Ray(capsuleColloderCenterInWorld - ((PlayerMovementStateMachine)_stateMachine).player.ColliderUtility.CapsuleColliderData.ColliderVerticalExtents, Vector3.down);

            if(!Physics.Raycast(downwardRayFromCapsuleBottom,out _, movementData.GroundToFallDistance,
                ((PlayerMovementStateMachine)_stateMachine).player.LayerData.GroundedLayer,
                QueryTriggerInteraction.Ignore))
            {
                OnFall();
                
            }
        }
        protected virtual void OnFall()
        {
            _stateMachine.ChangeState(((PlayerMovementStateMachine)_stateMachine).states[StateType.Falling]);
        }
        #endregion
        #region Main Methods
        private bool IsThereGroundUnderneath()
        {
            BoxCollider groundCheckCollider = ((PlayerMovementStateMachine)_stateMachine).player.ColliderUtility.TriggerColliderData.GroundCheckCollider;
            if(groundCheckCollider != null)
            {
                Vector3 groundColloderCenterInWorld = groundCheckCollider.bounds.center;
                Collider[] overlappedGroundColliders = Physics.OverlapBox(groundColloderCenterInWorld,
                ((PlayerMovementStateMachine)_stateMachine).player.ColliderUtility.TriggerColliderData.GroundCheckColliderExtents, groundCheckCollider.transform.rotation,
                (((PlayerMovementStateMachine)_stateMachine).player.LayerData.GroundedLayer), QueryTriggerInteraction.Ignore);
                return overlappedGroundColliders.Length > 0;
            }
            return false;
            
            
        }
        #endregion
        #region Input Methods
        protected virtual void OnDashStarted(InputAction.CallbackContext context)
        {
            _stateMachine.ChangeState(((PlayerMovementStateMachine)_stateMachine).states[StateType.Dashing]);
        }

        protected virtual void OnJumpStarted(InputAction.CallbackContext context)
        {
            _stateMachine.ChangeState(((PlayerMovementStateMachine)_stateMachine).states[StateType.Jumping]);
        }
       
        #endregion
    }
}
