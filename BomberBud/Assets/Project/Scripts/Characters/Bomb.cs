using System.Collections;
using UnityEngine;

namespace Project.Scripts.Characters
{
    public class Bomb : Content
    {

        private IEnumerator Explode(float time)
        {
            yield return new WaitForSeconds(time);
        }
    }
}