using UnityEngine;
namespace GhenshinImpactMovement
{
    public class StateMachine
    {
        private IState _currentState;
        private StateType _currentStateType;

        public IState curentState { get { return _currentState; } set { _currentState = value; } }
        public StateType currentStateType { get { return _currentStateType; } set { _currentStateType = value; } }

        public void ChangeState(IState newState)
        {
            _currentState?.OnExitState();
            _currentState = newState;
            _currentStateType = newState.GetStateType();
            _currentState?.OnEnterState();
        }

        public void HandleInput()
        {
            _currentState?.OnHandleInput();
        }
        public void Update()
        {
            _currentState.OnUpdate();
        }
        public void PhysicsUpdate()
        {
            _currentState?.OnPhysicsUpdate();
        }
        public void OnAnimationEnterEvent()
        {
            _currentState?.OnAnimationEnterEvent();
        }
        public void OnAnimationExitsEvent()
        {
            _currentState?.OnAnimationExitsEvent();
        }
        public void OnAnimationTransitionEvent()
        {
            _currentState?.OnAnimationTransitionEvent();
        }
        public void OnTriggerEnter(Collider collider)
        {
            _currentState?.OnTriggerEnter(collider);
        }
        public void OnTriggerExit(Collider collider)
        {
            _currentState?.OnTriggerExit(collider);
        }
    }
}
