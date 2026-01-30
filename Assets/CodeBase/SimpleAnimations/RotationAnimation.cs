using UnityEngine;

namespace CodeBase.SimpleAnimations
{
    public class RotationAnimation : MonoBehaviour
    {
        [SerializeField] private Vector3 _rotationSpeed;

        private void Update()
        {
            transform.localRotation = 
                Quaternion.Euler(transform.localRotation.eulerAngles + _rotationSpeed * Time.deltaTime);
        }
    }
}