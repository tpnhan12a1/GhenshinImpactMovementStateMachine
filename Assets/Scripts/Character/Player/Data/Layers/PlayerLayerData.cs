using System;
using UnityEngine;

namespace GhenshinImpactMovement
{
    [Serializable]
    public class PlayerLayerData
    {
        [field:SerializeField] public LayerMask GroundedLayer { get; private set; }
        public bool ContainsLayer(LayerMask layerMark, int layer)
        {
            return (1 << layer & layerMark) != 0; // shilft right 1 by layer bit to compare with layer mark
        }
        public bool IsGroundLayer(int layer)
        {
            return ContainsLayer(GroundedLayer, layer);
        }
    }
}
