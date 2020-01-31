using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FIXME
public class PlayerFreeFall : MonoBehaviour
{
    [SerializeField]
    private GameObject m_PlayerPrefab;

    private GameObject m_Instance;

    void Start()
    {
        InstantiateNew();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        Destroy(m_Instance);
        InstantiateNew();
    }

    // void Update()
    // {
    //     if (m_Instance.transform.localPosition.y < -80)
    //     {
    //         m_PlayerTransform.localPosition = new Vector3(0, 15, 0);
    //         m_RigidBody.velocity = Vector2.zero;
    //         Destroy(m_Instance);
    //         InstantiateNew();
    //     }
    // }

    void InstantiateNew()
    {
        // FIXME: Hardcoded pos
        float posX = Random.Range(2.0f, 16.5f);
        m_Instance = Instantiate(m_PlayerPrefab, new Vector3(posX, 15, 0), Quaternion.identity);
    }
}
