using System;
using UnityEngine;

namespace GhenshinImpactMovement
{
    [Serializable]
    public class PlayerFallData
    {
        [field: SerializeField][field: Range(1f, 15f)] public float FallSpeedLimit { get;private set; } = 15f;
        [field: SerializeField][field: Range(1f, 100f)] public float MinimumDisstanceToBeConsideredHardFall { get; private set; } = 3f;
    }
}
