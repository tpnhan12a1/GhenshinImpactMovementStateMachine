using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhenshinImpactMovement
{
    public class PlayerHardStoppingState : PlayerStoppingState
    {
        public override StateType GetStateType()
        {
            return StateType.HardStopping;
        }
        #region IState Methods
        public override void OnEnterState()
        {
            base.OnEnterState();
            StartAnimation(((PlayerMovementStateMachine)_stateMachine).player.AnimationData.HardStopParameterHash);
            ((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementDecelerationForce = movementData.StopData.HardDecelerationForce;
        }
        public override void OnExitState()
        {
            base.OnExitState();
            StopAnimation(((PlayerMovementStateMachine)_stateMachine).player.AnimationData.HardStopParameterHash);
        }
        #endregion
    }
}
