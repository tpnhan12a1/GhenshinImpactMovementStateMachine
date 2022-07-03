using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GhenshinImpactMovement
{
    public abstract class PlayerLandingState : PlayerGroundedState
    {
        public override void OnEnterState()
        {
            base.OnEnterState();
            StartAnimation(((PlayerMovementStateMachine)_stateMachine).player.AnimationData.LandingParameterHash);
        }
        public override void OnExitState()
        {
            base.OnExitState();
            StopAnimation(((PlayerMovementStateMachine)_stateMachine).player.AnimationData.LandingParameterHash);
        }
    }
}
