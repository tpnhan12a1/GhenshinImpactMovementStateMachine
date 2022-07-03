using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GhenshinImpactMovement
{
    public class PlayerDashingState : PlayerGroundedState
    {
        private PlayerDashData dashData = new PlayerDashData();
        private float startTime;
        private int cosecutiveDashesUsed;

        private bool shouldKeepRotating;
        public void SetDashData()
        {
            this.dashData = movementData.DashData;
        }    
        public override StateType GetStateType()
        {
            return StateType.Dashing;
        }
        public override void SetStateMachine(StateMachine stateMachine)
        {
            if (stateMachine.GetType() == typeof(PlayerMovementStateMachine))
            {
                base.SetStateMachine(stateMachine);
                _stateMachine = (PlayerMovementStateMachine)stateMachine;
            }
        }
        #region IState Methods
        public override void OnEnterState()
        {
            ((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementSpeedModifier = dashData.SpeedModifier;
            base.OnEnterState();
            StartAnimation(((PlayerMovementStateMachine)_stateMachine).player.AnimationData.DashParameterHash);
            ((PlayerMovementStateMachine)_stateMachine).ReusableData.RotationData = dashData.RotationData;
            ((PlayerMovementStateMachine)_stateMachine).ReusableData.CurrentJumpForce = airBorneData.JumpData.StrongForce;

            Dash();

            shouldKeepRotating = ((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementInput != Vector2.zero;
            UpdateConsecutiveDashes();

            startTime = Time.time;

        }
        public override void OnPhysicsUpdate()
        {
            base.OnPhysicsUpdate();
            if (!shouldKeepRotating) return;
            
            RotateTowardsTargetRotation();
        }

        public override void OnAnimationTransitionEvent()
        {
            base.OnAnimationTransitionEvent();
            if(((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementInput == Vector2.zero)
            {
                _stateMachine.ChangeState(((PlayerMovementStateMachine)_stateMachine).states[StateType.Idling]);
                return;
            }
            _stateMachine.ChangeState(((PlayerMovementStateMachine)_stateMachine).states[StateType.Sprinting]);

        }
        public override void OnUpdate()
        {
            base.OnUpdate();

        }
        public override void OnExitState()
        {
            base.OnExitState();
            StopAnimation(((PlayerMovementStateMachine)_stateMachine).player.AnimationData.DashParameterHash);
            SetBaseRotatonData();
        }
        #endregion
        #region Main Methods
        private void UpdateConsecutiveDashes()
        {
            if(!IsConsecutive())
            {
                cosecutiveDashesUsed = 0;
            }
            ++cosecutiveDashesUsed;
            if(cosecutiveDashesUsed == dashData.ConsecutiveDashesLimitAmount)
            {
                cosecutiveDashesUsed = 0;
                ((PlayerMovementStateMachine)_stateMachine).player.Input.DisableActionFor(((PlayerMovementStateMachine)_stateMachine).player.Input.PlayerActions.Dash,dashData.DashLimitReachedCooldown);
            }    
        }

        private bool IsConsecutive()
        {
            return Time.time < startTime + dashData.TimeToColliderConsecutive;
        }
        private void Dash()
        {
            Vector3 dashDirection = ((PlayerMovementStateMachine)_stateMachine).player.transform.forward;
            dashDirection.y = 0f;
            UpdateTargetRotation(dashDirection, false);
            if (((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementInput != Vector2.zero)
            {
                UpdateTargetRotation(GetMovementDirection());
                dashDirection = GetTargetRotaionDirection(((PlayerMovementStateMachine)_stateMachine).ReusableData.CurrentTargetRotation.y);
            }
            ((PlayerMovementStateMachine)_stateMachine).player.Rigidbody.velocity = dashDirection*GetMovementSpeed(false);
        }
        #region Reusable Methods
        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();
            ((PlayerMovementStateMachine)_stateMachine).player.Input.PlayerActions.Movement.performed += OnMovemmentPerformed;
        }

        
        #endregion
        #endregion
        #region Input Methods
     
        private void OnMovemmentPerformed(InputAction.CallbackContext context)
        {
            shouldKeepRotating = true;
        }
        protected override void OnDashStarted(InputAction.CallbackContext context)
        {
            
        }
        #endregion
    }
}
