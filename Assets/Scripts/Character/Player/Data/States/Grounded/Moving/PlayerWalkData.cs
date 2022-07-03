using System;
using UnityEngine;
using System.Collections.Generic;
namespace GhenshinImpactMovement
{
    [Serializable]
    public class PlayerWalkData
    {
        [field: SerializeField][field: Range(0, 1f)] public float SpeedModifier { get; private set; } = 0.225f;
        [field: SerializeField] public List<PlayerCameraRecenteringData> BackwardCameraRecenteringData { get; private set; }
    }
}
