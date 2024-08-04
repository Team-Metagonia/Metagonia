using UnityEngine;

namespace CartoonEffects
{
    public class AutoMove : MonoBehaviour
    {
        public float speed = 5f; // 移动速度
        public Vector3 direction = Vector3.forward; // 移动方向

        void Update()
        {
            // 计算物体的位移
            Vector3 movement = direction.normalized * speed * Time.deltaTime;

            // 将位移应用到物体的位置
            transform.Translate(movement);
        }
    }
}
