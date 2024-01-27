using JetBrains.Annotations;
using UnityEngine;

public interface IDamageDetailed
{
    public void Damage(MonoBehaviour cause, RaycastHit hit);
    public void Damage(MonoBehaviour cause, Vector3 position, Vector3 surfaceNormal, [CanBeNull] Collider collider);
}