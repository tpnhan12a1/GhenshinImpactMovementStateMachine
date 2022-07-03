using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhenshinImpactMovement
{
    public class PlayerFallingState : PlayerAirBorneState
    {
        private PlayerFallData fallData = new PlayerFallData();
        private Vector3 playerPositionOnEnter;
        public override StateType GetStateType()
        {
            return StateType.Falling;
        }
        public void SetFallData()
        {
            fallData = airBorneData.FallData;
        }    
        #region IStateMethods
        public override void OnEnterState()
        {
            base.OnEnterState();
            StartAnimation(((PlayerMovementStateMachine)_stateMachine).player.AnimationData.FallParameterHash);
            playerPositionOnEnter = ((PlayerMovementStateMachine)_stateMachine).player.transform.position;
            ((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementSpeedModifier = 0f;
            ResetVerticalVelocity();
        }
        public override void OnExitState()
        {
            base.OnExitState();
            StopAnimation(((PlayerMovementStateMachine)_stateMachine).player.AnimationData.FallParameterHash);
        }
        public override void OnPhysicsUpdate()
        {
            base.OnPhysicsUpdate();
            LimitVerticalVelocity();
        }
        #endregion
        #region Reusable Methods
        protected override void ResetSprintState()
        { 
        }
        #endregion
        #region Main Methods
        protected override void OnContactWithGound(Collider collider)
        {
            float fallDistance = playerPositionOnEnter.y - ((PlayerMovementStateMachine)_stateMachine).player.transform.position.y;
            if (fallDistance < fallData.MinimumDisstanceToBeConsideredHardFall)
            {
                _stateMachine.ChangeState(((PlayerMovementStateMachine)_stateMachine).states[StateType.LightLanding]);
                return;
            }
            if (((PlayerMovementStateMachine)_stateMachine).ReusableData.ShouldWalk &&
                !((PlayerMovementStateMachine)_stateMachine).ReusableData.ShouldSprint ||
                ((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementInput == Vector2.zero)
            {
                _stateMachine.ChangeState(((PlayerMovementStateMachine)_stateMachine).states[StateType.HardLanding]);
                return;
            }
            _stateMachine.ChangeState(((PlayerMovementStateMachine)_stateMachine).states[StateType.Rolling]);

        }
        private void LimitVerticalVelocity()
        {
            Vector3 playerVeticalVelocity = GetPlayerVerticalVelocity();
            if(playerVeticalVelocity.y >= -fallData.FallSpeedLimit)
            {
                return;
            }
            Vector3 limitedVelocity = new Vector3(0, -fallData.FallSpeedLimit - playerVeticalVelocity.y, 0f);
            ((PlayerMovementStateMachine)_stateMachine).player.Rigidbody.AddForce(limitedVelocity,ForceMode.VelocityChange);

        }
        #endregion
    }
}
