using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterEffect : MonoBehaviour
{
    void Start()
    {
        // ��ƼŬ �ý����� ���� �ð��� ����ϴ�.
        ParticleSystem ps = GetComponent<ParticleSystem>();
        if (ps != null)
        {
            // ��ƼŬ �ý����� ���� �ð� �Ŀ� GameObject�� �ı��մϴ�.
            Destroy(gameObject, ps.main.duration);
        }
    }

}
