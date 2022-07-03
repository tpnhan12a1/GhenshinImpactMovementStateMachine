using UnityEngine.InputSystem;

namespace GhenshinImpactMovement
{
    public abstract class PlayerStoppingState : PlayerGroundedState
    {
        #region IState Methods
        public override void OnEnterState()
        {
            ((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementSpeedModifier = 0f;
            SetBaseCamreraRecenteringData();
            base.OnEnterState();
            StartAnimation(((PlayerMovementStateMachine)_stateMachine).player.AnimationData.StoppingParameterHash);
        }
        public override void OnExitState()
        {
            base.OnExitState();
            StopAnimation(((PlayerMovementStateMachine)_stateMachine).player.AnimationData.StoppingParameterHash);
        }
        public override void OnPhysicsUpdate()
        {
            base.OnPhysicsUpdate();
            RotateTowardsTargetRotation();

            if (!IsMovingHorizontally())
            {
                return;
            }
            DecelerateHorizontally();
        }
        public override void OnAnimationTransitionEvent()
        { 
            _stateMachine.ChangeState(((PlayerMovementStateMachine)_stateMachine).states[StateType.Idling]);
        }
        #endregion
        #region Reusable Methods
        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();
           ( (PlayerMovementStateMachine)_stateMachine).player.Input.PlayerActions.Movement.started += OnMovementStarted;
        }

       
        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();
            ((PlayerMovementStateMachine)_stateMachine).player.Input.PlayerActions.Movement.started -= OnMovementStarted;
        }
        #endregion
        #region Input Methods
        private void OnMovementStarted(InputAction.CallbackContext context)
        {
            OnMove();
        }
        #endregion
        #region Reusable Methods
        protected override void OnMove()
        {
            if (((PlayerMovementStateMachine)_stateMachine).ReusableData.ShouldWalk)
            {
                _stateMachine.ChangeState(((PlayerMovementStateMachine)_stateMachine).states[StateType.Walking]);
                return;
            } 
                
            _stateMachine.ChangeState(((PlayerMovementStateMachine)_stateMachine).states[StateType.Running]);
        }
        #endregion
    }
}
