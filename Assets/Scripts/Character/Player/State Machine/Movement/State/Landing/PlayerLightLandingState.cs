using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhenshinImpactMovement
{
    public class PlayerLightLandingState : PlayerLandingState
    {
        public override StateType GetStateType()
        {
            return StateType.LightLanding;
        }
        #region IState Methods
        public override void OnEnterState()
        {
            ((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementSpeedModifier = 0f;
            base.OnEnterState();
           
           ( (PlayerMovementStateMachine)_stateMachine).ReusableData.CurrentJumpForce = airBorneData.JumpData.StationaryForce;
            ResetVelocity();
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            if(((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementInput == Vector2.zero)
            {
                return;
            }  
            OnMove();
        }
        public override void OnPhysicsUpdate()
        {
            base.OnPhysicsUpdate();
            if (!IsMovingHorizontally()) return;
            ResetVelocity();
        }
        public override void OnAnimationTransitionEvent()
        {
            base.OnAnimationTransitionEvent();
            _stateMachine.ChangeState(((PlayerMovementStateMachine)_stateMachine).states[StateType.Idling]);
        }
        #endregion
    }
}
