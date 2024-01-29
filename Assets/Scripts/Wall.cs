using SFX;
using UnityEngine;

namespace DefaultNamespace
{
    public class Wall : MonoBehaviour, IDamageDetailed
    {
        public void Damage(MonoBehaviour cause, RaycastHit hit)
        {
            SourcePlayerEvents.instance.InvokeEvent("shootWall", hit.point, hit.normal, transform.gameObject);
        }

        public void Damage(MonoBehaviour cause, Vector3 position, Vector3 surfaceNormal, Collider collider)
        {
            throw new System.NotImplementedException();
        }
    }
}