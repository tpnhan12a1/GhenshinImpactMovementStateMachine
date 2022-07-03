using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhenshinImpactMovement
{
    [Serializable]
    public class PlayerAnimationData
    {
        [Header("State Group Parameter Names")]
        [SerializeField] private string grouderParametername = "Grounded";
        [SerializeField] private string movingParametername = "Moving";
        [SerializeField] private string stoppingParametername = "Stopping";
        [SerializeField] private string landingParametername = "Landing";
        [SerializeField] private string airborneParametername = "Airborne";
        
        [Header("Grounded Parameter Name")]
        [SerializeField] private string idleParametername = "IsIdling";
        [SerializeField] private string dashParametername = "IsDashing";
        [SerializeField] private string walkParametername = "IsWalking";
        [SerializeField] private string runParametername = "IsRunning";
        [SerializeField] private string sprintParametername = "IsSprinting";
        [SerializeField] private string mediumStopParametername = "IsMediumStopping";
        [SerializeField] private string hardStopParametername = "IsHardStopping";
        [SerializeField] private string rollParametername = "IsRolling";
        [SerializeField] private string hardLandParametername = "IsHardLanding";

        [Header("Airborne Parameter Name")]
        [SerializeField] private string fallParametername = "IsFalling";

        public int GroundedParameterHash { get; private set; }
        public int MovingParameterHash { get; private set; }
        public int StoppingParameterHash { get; private set; }
        public int LandingParameterHash { get; private set; }
        public int AirborneParameterHash { get; private set; }
        public int IdleParameterHash { get; private set; }
        public int DashParameterHash { get; private set; }
        public int WalkParameterHash { get; private set; }
        public int RunParameterHash { get; private set; }
        public int SprintParameterHash { get; private set; }
        public int MediumStopParameterHash { get; private set; }
        public int HardStopParameterHash { get; private set; }
        public int RollParameterHash { get; private set; }
        public int HardLandParameterHash { get; private set; }
        public int FallParameterHash { get; private set; }

        public void Initialize()
        {
            GroundedParameterHash = Animator.StringToHash(grouderParametername);
            MovingParameterHash = Animator.StringToHash(movingParametername);
            StoppingParameterHash = Animator.StringToHash(stoppingParametername);
            LandingParameterHash = Animator.StringToHash(landingParametername);
            AirborneParameterHash = Animator.StringToHash(airborneParametername);

            IdleParameterHash = Animator.StringToHash(idleParametername);
            DashParameterHash = Animator.StringToHash(dashParametername);
            WalkParameterHash = Animator.StringToHash(walkParametername);
            RunParameterHash = Animator.StringToHash(runParametername);
            SprintParameterHash = Animator.StringToHash(sprintParametername);
            MediumStopParameterHash = Animator.StringToHash(mediumStopParametername);
            HardStopParameterHash = Animator.StringToHash(hardStopParametername);
            RollParameterHash = Animator.StringToHash(rollParametername);
            HardLandParameterHash = Animator.StringToHash(hardLandParametername);

            FallParameterHash = Animator.StringToHash(fallParametername);
        }
    }
}
