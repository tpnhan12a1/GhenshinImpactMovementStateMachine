using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhenshinImpactMovement
{
    public class PlayerAnimationEventTrigger : MonoBehaviour
    {
        private Player player;
        private void Awake()
        {
            player = GetComponentInParent<Player>();
        }
        public void TriggerOnMovemetStateEnterEvent()
        {
            player?.OnMovementStateAnimationEnterEvent();
        }    
        public void TriggerOnMovemetStateExitEvent()
        {
            player?.OnMovementStateAnimationExitEvent();
        } 
        public void TriggerOnMovemetAnimationTransitionEvent()
        {
            player?.OnMovementStateAnimationTransitionEvent();
        }    
    }
}
