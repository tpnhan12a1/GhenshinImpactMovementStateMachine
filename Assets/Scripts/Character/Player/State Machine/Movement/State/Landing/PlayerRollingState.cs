using UnityEngine;
using UnityEngine.InputSystem;

namespace GhenshinImpactMovement
{
    public class PlayerRollingState : PlayerLandingState
    {
        private PlayerRollData rollData = new PlayerRollData();
        public void SetRollData()
        {
            rollData = movementData.RollData;
        }    
        public override StateType GetStateType()
        {
            return StateType.Rolling;
        }
        #region IState Methods
        public override void OnEnterState()
        {
            ((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementSpeedModifier = rollData.SpeedModifier;
            base.OnEnterState();
            StartAnimation(((PlayerMovementStateMachine)_stateMachine).player.AnimationData.RollParameterHash);

            ((PlayerMovementStateMachine)_stateMachine).ReusableData.ShouldSprint = false;
        }
        public override void OnExitState()
        {
            base.OnExitState();
            StopAnimation(((PlayerMovementStateMachine)_stateMachine).player.AnimationData.RollParameterHash);
        }
        public override void OnPhysicsUpdate()
        {
            base.OnPhysicsUpdate();
           
            if (((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementInput == Vector2.zero)
                return;

            RotateTowardsTargetRotation();

        }
        public override void OnAnimationTransitionEvent()
        {
            if(((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementInput == Vector2.zero)
            {
                _stateMachine.ChangeState(((PlayerMovementStateMachine)_stateMachine).states[StateType.MediumStopping]);
                return;
            }
            OnMove();
        }
        #endregion
        #region Input Methods
        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
            
        }
        #endregion
    }
}
