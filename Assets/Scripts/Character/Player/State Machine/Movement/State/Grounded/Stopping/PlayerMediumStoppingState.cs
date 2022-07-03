using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhenshinImpactMovement
{
    public class PlayerMediumStoppingState : PlayerStoppingState
    {
        public override StateType GetStateType()
        {
            return StateType.MediumStopping;
        }
        #region IState Methods
        public override void OnEnterState()
        {
            base.OnEnterState();
            StartAnimation(((PlayerMovementStateMachine)_stateMachine).player.AnimationData.MediumStopParameterHash);
            ((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementDecelerationForce = movementData.StopData.MediumDecelerationForce;
        }
        public override void OnExitState()
        {
            base.OnExitState();
            StopAnimation(((PlayerMovementStateMachine)_stateMachine).player.AnimationData.MediumStopParameterHash);
        }
        #endregion
    }
}
