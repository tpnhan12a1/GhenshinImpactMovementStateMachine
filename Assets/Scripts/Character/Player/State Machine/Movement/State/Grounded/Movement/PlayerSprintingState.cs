using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GhenshinImpactMovement
{
    public class PlayerSprintingState : PlayerMovingState
    {
        private PlayerSprintData sprintData = new PlayerSprintData();
        private bool keepSprinting;
        private float startTime;
        private bool shouldResetSprintState;
        public override StateType GetStateType()
        {
            return StateType.Sprinting;
        }
        public void SetSprintData()
        {
            sprintData = movementData.SprintData;
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
            ((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementSpeedModifier = sprintData.SpeedModifier;
            base.OnEnterState();

            StartAnimation(((PlayerMovementStateMachine)_stateMachine).player.AnimationData.SprintParameterHash);

            ((PlayerMovementStateMachine)_stateMachine).ReusableData.CurrentJumpForce = airBorneData.JumpData.StrongForce;
            startTime = Time.time;
            shouldResetSprintState = true;
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            if (keepSprinting) return;

            if (Time.time < startTime + sprintData.SprintToRunTime) return;

            StopSprinting();     
        }
        public override void OnExitState()
        {
            base.OnExitState();
            StopAnimation(((PlayerMovementStateMachine)_stateMachine).player.AnimationData.SprintParameterHash);
            keepSprinting = false;
            if(shouldResetSprintState)
            shouldResetSprintState = false;
            ((PlayerMovementStateMachine)_stateMachine).ReusableData.ShouldSprint = false;
        }
        #endregion
        #region Main Methods
        private void StopSprinting()
        {
            if(((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementInput ==Vector2.zero)
            {
                ((PlayerMovementStateMachine)_stateMachine).ChangeState(((PlayerMovementStateMachine)_stateMachine).states[StateType.Idling]);
                return;            
            }
            ((PlayerMovementStateMachine)_stateMachine).ChangeState(((PlayerMovementStateMachine)_stateMachine).states[StateType.Running]);
        }
        #endregion
        #region Reusable Methods
        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();
            ((PlayerMovementStateMachine)_stateMachine).player.Input.PlayerActions.Sprint.performed += OnSprintPerformed;
        }
        protected override void OnFall()
        {
            shouldResetSprintState = false;
            base.OnFall();
        }
        #endregion
        #region Input Methods
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            _stateMachine.ChangeState(((PlayerMovementStateMachine)_stateMachine).states[StateType.HardStopping]);
            base.OnMovementCanceled(context);
        }
        private void OnSprintPerformed(InputAction.CallbackContext context)
        {
            keepSprinting = true;
            ((PlayerMovementStateMachine)_stateMachine).ReusableData.ShouldSprint = true;
        }
        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
            shouldResetSprintState = false;
            base.OnJumpStarted(context);
        }
        #endregion
    }
}
