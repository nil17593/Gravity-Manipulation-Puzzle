using UnityEngine;

namespace Parodystudios.GravityModificationPuzzle
{
    public class CollectableCubes : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<CharacterMovement>() != null)
            {
                gameObject.SetActive(false);
            }
        }
    }
}