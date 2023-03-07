using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEvent : MonoBehaviour
{
    [SerializeField]
    protected GameObject m_SpawnableObject;
    [SerializeField]
    protected bool m_DestroyMySelf;

    public void Spawn(Vector3 position)
    {
        if (m_SpawnableObject != null)
            Instantiate(m_SpawnableObject, position, Quaternion.identity);

        if (m_DestroyMySelf)
            Destroy(this);
    }
}
