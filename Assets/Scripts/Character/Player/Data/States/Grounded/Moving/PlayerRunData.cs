using System;
using UnityEngine;

namespace GhenshinImpactMovement
{
    [Serializable]
    public class PlayerRunData
    {
        [field: SerializeField][field: Range(1, 2f)] public float SpeedModifier { get; private set; } = 0.5f;
    }
}
