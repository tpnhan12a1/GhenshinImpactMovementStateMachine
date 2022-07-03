using UnityEngine;

namespace GhenshinImpactMovement
{
    public abstract class PlayerMovingState : PlayerGroundedState
    {
        public override void OnEnterState()
        {
            base.OnEnterState();
            StartAnimation(((PlayerMovementStateMachine)_stateMachine).player.AnimationData.MovingParameterHash);
        }
        public override void OnExitState()
        {
            base.OnExitState();
            StopAnimation(((PlayerMovementStateMachine)_stateMachine).player.AnimationData.MovingParameterHash);
        }
    }
}
