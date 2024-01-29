using SFX;
using UnityEngine;

namespace DefaultNamespace
{
    public class Table : MonoBehaviour, IDamageDetailed
    {
        public void Damage(MonoBehaviour cause, RaycastHit hit)
        {
            SourcePlayerEvents.instance.InvokeEvent("shootTable", hit.point, hit.normal, transform.gameObject);
        }

        public void Damage(MonoBehaviour cause, Vector3 position, Vector3 surfaceNormal, Collider collider)
        {
            throw new System.NotImplementedException();
        }
    }
}