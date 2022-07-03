using UnityEngine;

namespace GhenshinImpactMovement
{
    [CreateAssetMenu(fileName ="Player", menuName ="Custom/Characters/Player")]
    public class PlayerSO : ScriptableObject
    {
        [field:SerializeField] public PlayerGroundedData GroundedData { get; private set; }
        [field:SerializeField] public PlayerAirBorneData AirBorneData { get; private set; }
        
    }
}
