using System.Collections;
using System.Collections.Generic;
namespace GhenshinImpactMovement
{
    public enum StateType { Idling, Walking, Running, Sprinting, Dashing, HardStopping, MediumStopping, LightStopping, Jumping,
        Falling,
        LightLanding,
        Rolling,
        HardLanding
    }
    public class PlayerMovementStateMachine : StateMachine
    {
        
        private Dictionary<StateType, IState> _states = new Dictionary<StateType, IState>();
        private Player _player;
        public PlayerStateReusableData ReusableData { get; set; }
        public Dictionary<StateType, IState> states
        {
            get { return _states; }
            set { _states = value; }
        }
        public Player player
        {
            get { return _player; }
            set { _player = value; }
        }
    }
}
