using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace GhenshinImpactMovement
{
    public class PlayerRunningState : PlayerMovingState
    {
        private PlayerSprintData sprintData = new PlayerSprintData();
        private float startTime;
        public override StateType GetStateType()
        {
            return StateType.Running;
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
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            _stateMachine.ChangeState(((PlayerMovementStateMachine)_stateMachine).states[StateType.MediumStopping]);
        }
        public override void OnEnterState()
        {
            ((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementSpeedModifier = movementData.RunData.SpeedModifier;
            base.OnEnterState();
            StartAnimation(((PlayerMovementStateMachine)_stateMachine).player.AnimationData.RunParameterHash);

            ((PlayerMovementStateMachine)_stateMachine).ReusableData.CurrentJumpForce = airBorneData.JumpData.MediumForce ;
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            if (!((PlayerMovementStateMachine)_stateMachine).ReusableData.ShouldWalk) return;
            if (Time.time < startTime + sprintData.RunToWalkTime) return;
            StopRunning();

        }
        public override void OnExitState()
        {
            base.OnExitState();
            StopAnimation(((PlayerMovementStateMachine)_stateMachine).player.AnimationData.RunParameterHash);

        }
        #region Main Methods
        public  void SetSprintData()
        {
            sprintData = movementData.SprintData;
        }
        private void StopRunning()
        {
            if(((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementInput ==Vector2.zero)
            {
                _stateMachine.ChangeState(((PlayerMovementStateMachine)_stateMachine).states[StateType.Idling]);
                return;
            }
            _stateMachine.ChangeState(((PlayerMovementStateMachine)_stateMachine).states[StateType.Walking]);
        }
        #endregion
        #endregion
        #region Input Methods
        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);
            _stateMachine.ChangeState(((PlayerMovementStateMachine)_stateMachine).states[StateType.Walking]);

        }

       
        #endregion

    }
}
