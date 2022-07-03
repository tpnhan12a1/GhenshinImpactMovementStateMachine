using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhenshinImpactMovement
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerIdlingState))]
    public class Player : MonoBehaviour
    {
        [field:Header("References")]
        [field:SerializeField] public PlayerSO Data { get; private set; }

        [field: Header("Colisions")]
        [field:SerializeField] public PlayerCapsuleColliderUtility ColliderUtility { get; private set; }
        [field:SerializeField] public PlayerLayerData LayerData { get; private set; }
        [field:Header("Cameras")]
        [field:SerializeField] public PlayerCameraUtility CameraUtility { get; private set; }
        [field:Header("Animations")]
        [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }
        public Rigidbody Rigidbody { get; private set; }
        public Animator Animator { get; private set; }
        public PlayerInput Input { get; private set; }

        public Transform MainCameraTransform { get; private set; }
        private PlayerMovementStateMachine _movementStateMachine;

        public PlayerMovementStateMachine movementStateMachine
        {
            get { return _movementStateMachine; }
        }

        private void Awake()
        {
            _movementStateMachine = new PlayerMovementStateMachine();
            _movementStateMachine.ReusableData = new PlayerStateReusableData();
            ColliderUtility.Initialize(gameObject);
            ColliderUtility.CapsuleCollierDimensions();
            CameraUtility.Initialize();
            AnimationData.Initialize();
            
            IState[] states = GetComponents<IState>();
            foreach (IState state in states)
            {
                if (state != null && !_movementStateMachine.states.ContainsKey(state.GetStateType()))
                {
                    _movementStateMachine.states.Add(state.GetStateType(), state);
                    state.SetStateMachine(_movementStateMachine);
                
                    ((PlayerMovementState)state).SetMovementData(Data.GroundedData);
                    ((PlayerMovementState)state).SetAirBorneData(Data.AirBorneData);
                    ((PlayerMovementState)state).SetBaseRotatonData();
                    ((PlayerMovementState)state).SetBaseCamreraRecenteringData();
                    if(state.GetType() == typeof(PlayerIdlingState))
                        ((PlayerIdlingState)state).SetIdleData();
                    if (state.GetType() == typeof(PlayerGroundedState))
                        ((PlayerGroundedState)state).SetSlopeData(ColliderUtility.SlopeData);
                    if (state.GetType() == typeof(PlayerWalkingState))
                        ((PlayerWalkingState)state).SetWalkData();
                    if (state.GetType() == typeof(PlayerDashingState))
                        ((PlayerDashingState)state).SetDashData();
                    if(state.GetType() == typeof(PlayerSprintingState))
                        ((PlayerSprintingState)state).SetSprintData();
                    if(state.GetType() == typeof(PlayerRunningState))
                        ((PlayerRunningState)state).SetSprintData();
                    if(state.GetType() == typeof(PlayerJumpingState))
                        ((PlayerJumpingState)state).SetJumpData();
                    if (state.GetType() == typeof(PlayerFallingState))
                        ((PlayerFallingState)state).SetFallData();
                    if(state.GetType()== typeof(PlayerRollingState))
                        ((PlayerRollingState)state).SetRollData();


                }
            } 
            _movementStateMachine.player = this;  
        }
        private void OnValidate()
        {
            if(ColliderUtility != null)
            {
                ColliderUtility.Initialize(gameObject);
                ColliderUtility.CapsuleCollierDimensions();
            }    
        }
        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();
            Animator = GetComponentInChildren<Animator>();
            Input = GetComponent<PlayerInput>();
            MainCameraTransform = Camera.main.transform;
            
            if(movementStateMachine != null)
            {
                IState newState = null;
                if( _movementStateMachine.states.TryGetValue(StateType.Idling, out newState))
                {
                    _movementStateMachine.curentState = newState;
                    _movementStateMachine.currentStateType = StateType.Idling;
                    _movementStateMachine.ChangeState(newState);
                }    
            }
        }
        private void Update()
        {
            _movementStateMachine.HandleInput();
            _movementStateMachine.Update();
        }
        private void FixedUpdate()
        {
            _movementStateMachine.PhysicsUpdate();
        }
        private void OnTriggerEnter(Collider collider)
        {
            movementStateMachine.OnTriggerEnter(collider);
        }
        private void OnTriggerExit(Collider collider)
        {
            movementStateMachine?.OnTriggerExit(collider);
        }
        public void OnMovementStateAnimationEnterEvent()
        {
            _movementStateMachine?.OnAnimationEnterEvent();
        }    
        public void OnMovementStateAnimationExitEvent()
        {
            _movementStateMachine?.OnAnimationExitsEvent();
        }   
        public void OnMovementStateAnimationTransitionEvent()
        {
            _movementStateMachine.OnAnimationTransitionEvent();
        }    
    }
}
