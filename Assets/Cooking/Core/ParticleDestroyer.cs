using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class ParticleDestroyer : MonoBehaviour
    {
        public float duration;
        private void Start()
        {
            StartCoroutine(DestroyParticle(duration));
        }

        private IEnumerator DestroyParticle(float duration)
        {
            yield return new WaitForSeconds(duration);
            ObjectPool.Instance.UnSpawn(gameObject);
        }
    }
}
