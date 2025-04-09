using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace YH
{
    [DisallowMultipleComponent]
    public class Popup : MonoBehaviour
    {
        public bool scriptAttached = false;
    }
}
