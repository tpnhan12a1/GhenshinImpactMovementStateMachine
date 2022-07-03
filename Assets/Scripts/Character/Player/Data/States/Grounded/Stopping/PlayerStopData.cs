using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GhenshinImpactMovement
{
    [Serializable]
    public class PlayerStopData
    {
        [field: SerializeField][field: Range(0f, 5f)] public float LightDecelerationForce { get; private set; } = 5f;
        [field: SerializeField][field: Range(0f, 5f)] public float MediumDecelerationForce { get; private set; } = 6.5f;
        [field: SerializeField][field: Range(0f, 5f)] public float HardDecelerationForce { get; private set; } = 5f;
    }
}