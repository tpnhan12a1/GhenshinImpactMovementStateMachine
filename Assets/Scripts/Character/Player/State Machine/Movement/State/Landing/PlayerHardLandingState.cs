using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GhenshinImpactMovement
{
    public class PlayerHardLandingState : PlayerLandingState
    {
        public override StateType GetStateType()
        {
            return StateType.HardLanding;
        }
        #region IState Methods
        public override void OnEnterState()
        {
            ((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementSpeedModifier = 0f;
            base.OnEnterState();
            StartAnimation(((PlayerMovementStateMachine)_stateMachine).player.AnimationData.HardLandParameterHash);
            ((PlayerMovementStateMachine)_stateMachine).player.Input.PlayerActions.Movement.Disable();
            //reference animation
           
            ResetVelocity();
        }
        public override void OnPhysicsUpdate()
        {
            base.OnPhysicsUpdate();
            if (!IsMovingHorizontally()) return;
            ResetVelocity();
        }
        public override void OnExitState()
        {
            base.OnExitState();
            StopAnimation(((PlayerMovementStateMachine)_stateMachine).player.AnimationData.HardLandParameterHash);
            ((PlayerMovementStateMachine)_stateMachine).player.Input.PlayerActions.Movement.Enable();
        }
        public override void OnAnimationExitsEvent()
        {
            ((PlayerMovementStateMachine)_stateMachine).player.Input.PlayerActions.Movement.Enable();
        }
        public override void OnAnimationTransitionEvent()
        {
            _stateMachine.ChangeState(((PlayerMovementStateMachine)_stateMachine).states[StateType.Idling]);

        }
        #endregion
        #region Resuable Methods
        protected override void AddInputActionsCallbacks()
        {
            ((PlayerMovementStateMachine)_stateMachine).player.Input.PlayerActions.Movement.started += OnMovementStarted;
        }
        protected override void OnMove()
        {
            base.OnMove();
            if(((PlayerMovementStateMachine)_stateMachine).ReusableData.ShouldWalk)
            {
                _stateMachine.ChangeState(((PlayerMovementStateMachine)_stateMachine).states[StateType.Walking]);
                return;
            }    
            _stateMachine.ChangeState(((PlayerMovementStateMachine)_stateMachine).states[StateType.Running]);
        }
        #endregion
        #region Input Methods
        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
        }
        protected virtual void OnMovementStarted(InputAction.CallbackContext context)
        {
            OnMove();
        }
        #endregion
    }
}
