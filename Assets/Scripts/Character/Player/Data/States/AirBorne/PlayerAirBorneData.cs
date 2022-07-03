using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhenshinImpactMovement
{
    [Serializable]
    public class PlayerAirBorneData 
    {
       [field:SerializeField] public PlayerJumpData JumpData { get; set; }  
        [field:SerializeField] public PlayerFallData FallData { get; set; }
    }
}
