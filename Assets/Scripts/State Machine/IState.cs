using System.Collections;
using UnityEngine;
namespace GhenshinImpactMovement
{
    public interface IState
    {
       
        public void OnEnterState();
        public void OnExitState();
        public void OnHandleInput();
        public void OnUpdate();
        public void OnPhysicsUpdate();
        public void OnAnimationEnterEvent();
        public void OnAnimationExitsEvent();

        public void OnAnimationTransitionEvent();
        public abstract StateType GetStateType();
        public void OnTriggerEnter(Collider collider);
        public void OnTriggerExit(Collider collider);
        void SetStateMachine(PlayerMovementStateMachine stateMachine);
    }
}
