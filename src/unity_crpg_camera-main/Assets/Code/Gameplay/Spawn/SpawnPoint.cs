using UnityEngine;

namespace Code.Gameplay.Spawn
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private string _spawnPointId = "Player";
        [SerializeField] private Color _gizmoColor = Color.green;
        [SerializeField] private float _gizmoSize = 1f;

        public string Id => _spawnPointId;
        public Vector3 Position => transform.position;
        public Quaternion Rotation => transform.rotation;

        private void OnDrawGizmos()
        {
            Gizmos.color = _gizmoColor;
            Gizmos.DrawWireCube(transform.position, Vector3.one * _gizmoSize);
            
            // Draw arrow indicating forward direction
            var forward = transform.forward * _gizmoSize;
            Gizmos.DrawLine(transform.position, transform.position + forward);
            
            // Draw arrow head
            var arrowHead1 = transform.position + forward + (-transform.forward + transform.right) * 0.3f * _gizmoSize;
            var arrowHead2 = transform.position + forward + (-transform.forward - transform.right) * 0.3f * _gizmoSize;
            Gizmos.DrawLine(transform.position + forward, arrowHead1);
            Gizmos.DrawLine(transform.position + forward, arrowHead2);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, Vector3.one * (_gizmoSize + 0.2f));
        }
    }
}

