using UnityEditor;
using UnityEngine;

namespace Base.ModularUI
{
    public class UISnapPoint : MonoBehaviour
    {
#if DEBUG
        private void OnDrawGizmos()
        {
            Gizmos.DrawCube(gameObject.transform.position, new Vector3(8, 8, 8));
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(gameObject.transform.position, new Vector3(8, 8, 8));
        }
#endif
    }
}