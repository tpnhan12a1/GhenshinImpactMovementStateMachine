using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhenshinImpactMovement
{
    public class PlayerLightStoppingState : PlayerStoppingState
    {
        public override StateType GetStateType()
        {
            return StateType.LightStopping;
        }
        #region IState Methods
        public override void OnEnterState()
        {
            base.OnEnterState();
            ((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementDecelerationForce = movementData.StopData.LightDecelerationForce;
        }
        #endregion
    }
}
