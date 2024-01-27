using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private float time = 1f;

    [SerializeField] private bool deactivateOnly;
    [SerializeField] private GameObject _target;

    private GameObject Target => _target ?? gameObject;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Complete", time);
    }

    public void Complete()
    {
        if (deactivateOnly)
        {
            Target.SetActive(false);
        }
        else
        {
            Destroy(Target);
        }
    }

}
