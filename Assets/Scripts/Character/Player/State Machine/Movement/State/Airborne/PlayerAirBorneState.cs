using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhenshinImpactMovement
{
    public abstract class PlayerAirBorneState : PlayerMovementState
    {
       
        #region Reusable Methods
        public override void SetStateMachine(StateMachine stateMachine)
        {
            base.SetStateMachine(stateMachine);
        }
        #region IState Methods
        public override void OnEnterState()
        {
            base.OnEnterState();
            StartAnimation(((PlayerMovementStateMachine)_stateMachine).player.AnimationData.AirborneParameterHash);
            ResetSprintState();
        }
        public override void OnExitState()
        {
            base.OnExitState();
            StopAnimation(((PlayerMovementStateMachine)_stateMachine).player.AnimationData.AirborneParameterHash);
        }
        #endregion
        protected override void OnContactWithGound(Collider collider)
        {
            base.OnContactWithGound(collider);
            _stateMachine.ChangeState(((PlayerMovementStateMachine)_stateMachine).states[StateType.LightLanding]);
        }

        protected virtual  void ResetSprintState()
        {
            ((PlayerMovementStateMachine)_stateMachine).ReusableData.ShouldSprint = false;
        }    
        #endregion

    }

}
