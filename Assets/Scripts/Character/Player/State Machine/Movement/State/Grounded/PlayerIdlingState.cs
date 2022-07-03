using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhenshinImpactMovement
{
    public class PlayerIdlingState : PlayerGroundedState
    {
        private PlayerIdleData idleData = new PlayerIdleData();
        #region IState Methods
        public override StateType GetStateType()
        {
            return StateType.Idling;
        }
        public void SetIdleData()
        {
            idleData = movementData.IdleData;
        }    
        public override void SetStateMachine(StateMachine stateMachine)
        {
            if (stateMachine.GetType() == typeof(PlayerMovementStateMachine))
            {
                base.SetStateMachine(stateMachine);
                _stateMachine = (PlayerMovementStateMachine)stateMachine;
            }
        }
        public override void OnEnterState()
        {
            ((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementSpeedModifier = 0f;
            ((PlayerMovementStateMachine)_stateMachine).ReusableData.BackwardsCameraRecenteringData = idleData.BackwardsCameraRecenteringData;
            
            base.OnEnterState();
            StartAnimation(((PlayerMovementStateMachine)_stateMachine).player.AnimationData.IdleParameterHash);
            ((PlayerMovementStateMachine)_stateMachine).ReusableData.CurrentJumpForce = airBorneData.JumpData.StationaryForce;
            ResetVelocity();
        }
        public override void OnExitState()
        {
            base.OnExitState();
            StopAnimation(((PlayerMovementStateMachine)_stateMachine).player.AnimationData.IdleParameterHash);
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            if (((PlayerMovementStateMachine) _stateMachine).ReusableData.MovementInput == Vector2.zero) return;
            OnMove();
        }
        public override void OnPhysicsUpdate()
        {
            base.OnPhysicsUpdate();
            if (!IsMovingHorizontally()) return;
            ResetVelocity();
        }
        #endregion
    }
}
