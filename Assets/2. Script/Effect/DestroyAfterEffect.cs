using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterEffect : MonoBehaviour
{
    void Start()
    {
        // 파티클 시스템의 지속 시간을 얻습니다.
        ParticleSystem ps = GetComponent<ParticleSystem>();
        if (ps != null)
        {
            // 파티클 시스템의 지속 시간 후에 GameObject를 파괴합니다.
            Destroy(gameObject, ps.main.duration);
        }
    }

}
