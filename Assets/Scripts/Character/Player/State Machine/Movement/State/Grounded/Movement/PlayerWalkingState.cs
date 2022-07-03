using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GhenshinImpactMovement
{
    public class PlayerWalkingState : PlayerMovingState
    {
        private PlayerWalkData walkData = new PlayerWalkData();
        public override StateType GetStateType()
        {
            return StateType.Walking;
        }
        public void SetWalkData()
        {
            walkData = movementData.WalkData;
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
            ((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementSpeedModifier = movementData.WalkData.SpeedModifier;
            ((PlayerMovementStateMachine)_stateMachine).ReusableData.BackwardsCameraRecenteringData = walkData.BackwardCameraRecenteringData;
            base.OnEnterState();
            StartAnimation(((PlayerMovementStateMachine)_stateMachine).player.AnimationData.WalkParameterHash);
            ((PlayerMovementStateMachine)_stateMachine).ReusableData.CurrentJumpForce = airBorneData.JumpData.WeakForce;
        }
        public override void OnUpdate()
        {
            base.OnUpdate();

        }
        public override void OnExitState()
        {
            base.OnExitState();
            StopAnimation(((PlayerMovementStateMachine)_stateMachine).player.AnimationData.WalkParameterHash);
            SetBaseCamreraRecenteringData();
        }
        #endregion
        #region Input Methods
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
           _stateMachine.ChangeState(((PlayerMovementStateMachine)_stateMachine).states[StateType.LightStopping]);
            base.OnMovementCanceled(context);
        }
        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);
            _stateMachine.ChangeState(((PlayerMovementStateMachine)_stateMachine).states[StateType.Running]);

        }
        #endregion
    }
}
