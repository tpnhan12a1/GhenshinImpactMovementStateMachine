using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GhenshinImpactMovement
{
    public abstract class PlayerMovementState : MonoBehaviour, IState
    {
        protected StateMachine _stateMachine;
        protected PlayerGroundedData movementData;
        protected PlayerAirBorneData airBorneData;
        public abstract StateType GetStateType();
        public virtual void SetStateMachine(StateMachine stateMachine) { _stateMachine = stateMachine; }
        
        public void SetBaseCamreraRecenteringData()
        {
            ((PlayerMovementStateMachine)_stateMachine).ReusableData.BackwardsCameraRecenteringData = movementData.BackwardsCameraRecenteringData;
            ((PlayerMovementStateMachine)_stateMachine).ReusableData.SidewaysCameraRecenteringData= movementData.SidewaysCameraRecenteringData;
        }    
       
        protected void StartAnimation(int animationHash)
        {
            ((PlayerMovementStateMachine)_stateMachine).player.Animator.SetBool(animationHash, true);
        }
        protected void StopAnimation(int animationHash)
        {
            ((PlayerMovementStateMachine)_stateMachine).player.Animator.SetBool(animationHash, false);
        }

        #region IState Methods
        public virtual void OnEnterState()
        {
            Debug.Log("Entering " + GetType().Name);
            AddInputActionsCallbacks();
        }
        public virtual void OnExitState()
        {
            RemoveInputActionsCallbacks();
        }
       
        public virtual void OnHandleInput()
        {
            ReadMovementInput();
        }


        public virtual void OnUpdate()
        {
           
        }
        public virtual void OnPhysicsUpdate()
        {
            Move();

        }
        public virtual void OnAnimationEnterEvent()
        {
            
        }

        public virtual void OnAnimationExitsEvent()
        {
            
        }

        public virtual void OnAnimationTransitionEvent()
        {
            
        }
        public virtual void OnTriggerEnter(Collider collider)
        {
            if (((PlayerMovementStateMachine)_stateMachine).player.LayerData.IsGroundLayer(collider.gameObject.layer))
            {
                OnContactWithGound(collider);
            } 
                
        }
        public virtual void OnTriggerExit(Collider collider)
        {
            if(((PlayerMovementStateMachine)_stateMachine).player.LayerData.IsGroundLayer(collider.gameObject.layer))
            {
                OnContactWithGroundExited(collider);
                return;
            }    
        }

       
        #endregion
        #region Main Methods
        protected virtual void OnContactWithGound(Collider collider)
        {
           
        }
        public void SetBaseRotatonData()
        {
            ((PlayerMovementStateMachine)_stateMachine).ReusableData.RotationData = movementData.BaseRotationData;
            ((PlayerMovementStateMachine)_stateMachine).ReusableData.TimeReachTargetRotation = ((PlayerMovementStateMachine)_stateMachine).ReusableData.RotationData.TargetRotationReachTime;
        }
        private void ReadMovementInput()
        {
            ((PlayerMovementStateMachine) _stateMachine).ReusableData.MovementInput = ((PlayerMovementStateMachine)_stateMachine).player.Input.PlayerActions.Movement.ReadValue<Vector2>();
        }

        private void Move()
        {
            if (((PlayerMovementStateMachine) _stateMachine).ReusableData.MovementInput == Vector2.zero ||((PlayerMovementStateMachine) _stateMachine).ReusableData.MovementSpeedModifier == 0f) return;
            Vector3 movementDirection = GetMovementDirection();
            float targetRotationYAngle = Rotate(movementDirection);
            Vector3 targetRotationDirection = GetTargetRotaionDirection(targetRotationYAngle);
            

            float movementSpeed = GetMovementSpeed();
            Vector3 currentPlayerVelocity = GetPlayerHorizontalVelocity();
            
            ((PlayerMovementStateMachine)_stateMachine).player.Rigidbody.AddForce(targetRotationDirection * movementSpeed - currentPlayerVelocity, ForceMode.VelocityChange);
        }



        private float Rotate(Vector3 direction)
        {
            float directionAngle = UpdateTargetRotation(direction);
            RotateTowardsTargetRotation();
            return directionAngle;
        }

        protected float UpdateTargetRotation(Vector3 direction, bool shouldConsiderCameraRoatation = true)
        {
            float directionAngle = GetDirectionAngle(direction);

            if(shouldConsiderCameraRoatation)
            {
                directionAngle = AddCameraRotationToAngle(directionAngle);
            }    
            if (directionAngle !=   ((PlayerMovementStateMachine) _stateMachine).ReusableData.CurrentTargetRotation.y)
            {
                UpdateTargetRotationData(directionAngle);
            }

            return directionAngle;
        }
        private float AddCameraRotationToAngle(float angle)
        {
            angle += ((PlayerMovementStateMachine)_stateMachine).player.MainCameraTransform.eulerAngles.y;

            if (angle > 360) angle -= 360f;
            return angle;
        }

        protected void UpdateTargetRotationData(float targetAngle)
        {
             ((PlayerMovementStateMachine) _stateMachine).ReusableData.CurrentTargetRotation.y = targetAngle;
            ((PlayerMovementStateMachine) _stateMachine).ReusableData.DamgedTargetRotationPassedTime.y = 0f;
            
        }
        protected Vector3 GetPlayerVerticalVelocity()
        {
            return new Vector3(0, ((PlayerMovementStateMachine) _stateMachine).player.Rigidbody.velocity.y, 0);
        }    
        protected void RotateTowardsTargetRotation()
        {
            float currentYAngle = ((PlayerMovementStateMachine)_stateMachine).player.Rigidbody.rotation.eulerAngles.y;
            
            if (currentYAngle ==   ((PlayerMovementStateMachine) _stateMachine).ReusableData.CurrentTargetRotation.y) return;
            float smoothYAngle = Mathf.SmoothDampAngle(currentYAngle,   ((PlayerMovementStateMachine) _stateMachine).ReusableData.CurrentTargetRotation.y,
                ref   ((PlayerMovementStateMachine) _stateMachine).ReusableData.DamgedTargetRotationCurrentVelocity.y,
                ((PlayerMovementStateMachine) _stateMachine).ReusableData.TimeReachTargetRotation.y - ((PlayerMovementStateMachine) _stateMachine).ReusableData.DamgedTargetRotationPassedTime.y);
            ((PlayerMovementStateMachine) _stateMachine).ReusableData.DamgedTargetRotationPassedTime.y += Time.deltaTime;
            Quaternion targetRotation = Quaternion.Euler(0f, smoothYAngle, 0f);
            //if(Mathf.Abs(smoothYAngle) > 5f)
                ((PlayerMovementStateMachine)_stateMachine).player.Rigidbody.MoveRotation(targetRotation);
        }

        protected Vector3 GetPlayerHorizontalVelocity()
        {
            Vector3 playerHorizontalVelocity = ((PlayerMovementStateMachine)_stateMachine).player.Rigidbody.velocity;
            playerHorizontalVelocity.y = 0f;
            return playerHorizontalVelocity;
        }
        #endregion
        #region Reusable Methods
        public void SetMovementData(PlayerGroundedData movementData)
        {
            this.movementData = movementData;

        }
        public void SetStateMachine(PlayerMovementStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }
        public void SetAirBorneData(PlayerAirBorneData airBorneData)
        {
            this.airBorneData = airBorneData;
        }
        protected Vector3 GetTargetRotaionDirection(float targetAngle)
        {
            return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }
        protected Vector3 GetMovementDirection()
        {
            return new Vector3(((PlayerMovementStateMachine) _stateMachine).ReusableData.MovementInput.x, 0f, ((PlayerMovementStateMachine) _stateMachine).ReusableData.MovementInput.y);
        }
        protected float GetMovementSpeed(bool shouldConsiderSlope = true)
        {
            float movementSpeed = movementData.BaseSpeed * ((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementSpeedModifier *
                ((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementOnSlopeSpeedModifier;
            if(shouldConsiderSlope)
            {
                movementSpeed *= ((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementOnSlopeSpeedModifier;
            }
            return movementSpeed;
        }
        
        private float GetDirectionAngle(Vector3 direction)
        {
            float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            if (directionAngle < 0f) directionAngle += 360f;
            return directionAngle;
        }

        protected void ResetVelocity()
        {
            ((PlayerMovementStateMachine)_stateMachine).player.Rigidbody.velocity = Vector3.zero;
        }
        protected void ResetVerticalVelocity()
        {
            Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();
            ((PlayerMovementStateMachine)_stateMachine).player.Rigidbody.velocity = playerHorizontalVelocity;
        }
    
        protected virtual void AddInputActionsCallbacks()
        {
            ((PlayerMovementStateMachine)_stateMachine).player.Input.PlayerActions.WalkToggle.started += OnWalkToggleStarted;

            ((PlayerMovementStateMachine)_stateMachine).player.Input.PlayerActions.Look.started += OnMouseMovementStarted;

            ((PlayerMovementStateMachine)_stateMachine).player.Input.PlayerActions.Movement.performed += OnMovementPerformed;

            ((PlayerMovementStateMachine)_stateMachine).player.Input.PlayerActions.Movement.canceled += OnMovementCanceled;
        }

        protected virtual void RemoveInputActionsCallbacks()
        {
            ((PlayerMovementStateMachine)_stateMachine).player.Input.PlayerActions.WalkToggle.started -= OnWalkToggleStarted;

            ((PlayerMovementStateMachine)_stateMachine).player.Input.PlayerActions.Look.started -= OnMouseMovementStarted;

            ((PlayerMovementStateMachine)_stateMachine).player.Input.PlayerActions.Movement.performed -= OnMovementPerformed;
            
            ((PlayerMovementStateMachine)_stateMachine).player.Input.PlayerActions.Movement.canceled -= OnMovementCanceled;
        }
        public void DecelerateHorizontally()
        {
            Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();
            ((PlayerMovementStateMachine)_stateMachine).player.Rigidbody.AddForce(-playerHorizontalVelocity*
              ((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementDecelerationForce, ForceMode.Acceleration);

        }
        public void DecelerateVertical()
        {
            Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();
            ((PlayerMovementStateMachine)_stateMachine).player.Rigidbody.AddForce(-playerVerticalVelocity *
              ((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementDecelerationForce, ForceMode.Acceleration);

        }
        protected bool IsMovingHorizontally(float minimumMagnitude = 0.1f)
        {
            Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();
            Vector2 playerHorizontalMovement = new Vector2(playerHorizontalVelocity.x, playerHorizontalVelocity.z);
            return playerHorizontalMovement.magnitude > minimumMagnitude;
        }    
        protected bool IsMovingUp(float minumVelocity = 0.1f)
        {
            return GetPlayerVerticalVelocity().y > minumVelocity;
        }
        protected bool IsMovingDown(float minumVelocity = 0.1f)
        {
            return GetPlayerVerticalVelocity().y < -minumVelocity;
        }
        protected virtual void OnContactWithGroundExited(Collider collider)
        {
            
        }
        protected void UpdateCameraRecenteringState(Vector2 movementInput)
        {
            if (movementInput == Vector2.zero) return;
            if(movementInput == Vector2.up)
            {
                DisableCameraRecentering();
                return;
            }
            float cameraVerticalAngle = ((PlayerMovementStateMachine)_stateMachine).player.MainCameraTransform.eulerAngles.x;
            if(cameraVerticalAngle >= 270)
            {
                cameraVerticalAngle -= 360f;    
            }    
            cameraVerticalAngle = Mathf.Abs(cameraVerticalAngle);

            if (movementInput == Vector2.down)
            {
                SetCameraRecenteringState(cameraVerticalAngle, ((PlayerMovementStateMachine)_stateMachine).ReusableData.BackwardsCameraRecenteringData);
                return;
            }
            SetCameraRecenteringState(cameraVerticalAngle, movementData.SidewaysCameraRecenteringData);
            
        }

        protected void SetCameraRecenteringState(float cameraVerticalAngle, System.Collections.Generic.List<PlayerCameraRecenteringData> cameraRecenteringData)
        {
            foreach (PlayerCameraRecenteringData recenteringData in cameraRecenteringData)
            {
                if (!recenteringData.IsWithinRange(cameraVerticalAngle))
                {
                    continue;
                }
                EnableCameraRecentering(recenteringData.WaitTime, recenteringData.RecenteringTime);
                return;

            }
            DisableCameraRecentering();
        }

        protected void EnableCameraRecentering(float waitTime =-1f, float recenteringTine = -1f)
        {
            float movementSpeed = GetMovementSpeed();
            if(movementSpeed == 0f)
            {
                movementSpeed = movementData.BaseSpeed;
            }    
            ((PlayerMovementStateMachine)_stateMachine).player.CameraUtility.EnableCentering(waitTime, recenteringTine,movementData.BaseSpeed, movementSpeed);
        }  
        protected void DisableCameraRecentering()
        {
            ((PlayerMovementStateMachine)_stateMachine).player.CameraUtility.DisableRecentering();
        }    
        #endregion
        #region Input Methods
        protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            ((PlayerMovementStateMachine) _stateMachine).ReusableData.ShouldWalk = !((PlayerMovementStateMachine) _stateMachine).ReusableData.ShouldWalk;
        }
        protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
        {
            DisableCameraRecentering();
        }
        protected virtual void OnMovementPerformed(InputAction.CallbackContext context)
        {
            UpdateCameraRecenteringState(context.ReadValue<Vector2>());
        }

        protected virtual void OnMouseMovementStarted(InputAction.CallbackContext context)
        {
            UpdateCameraRecenteringState(((PlayerMovementStateMachine)_stateMachine).ReusableData.MovementInput);
        }
      
        #endregion
    }
}
