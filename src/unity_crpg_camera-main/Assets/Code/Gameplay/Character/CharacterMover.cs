using Code.Gameplay.Character.Settings;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Gameplay.Character
{
    public class CharacterMover
    {
        private readonly NavMeshAgent _navMeshAgent;
        private readonly MovementSettings _movementSettings;

        public CharacterMover(NavMeshAgent navMeshAgent, MovementSettings movementSettings)
        {
            _navMeshAgent = navMeshAgent;
            _movementSettings = movementSettings;
            ApplyConfiguration();
        }

        public bool IsMoving
        {
            get
            {
                if (_navMeshAgent == null) return false;
                if (!_navMeshAgent.hasPath) return false;

                var stoppingDist = _movementSettings?.StoppingDistance ?? _navMeshAgent.stoppingDistance;
                return !(_navMeshAgent.remainingDistance <= stoppingDist);
            }
        }

        public void SetDestination(Vector3 destination)
        {
            if (_navMeshAgent == null)
            {
                Debug.LogError("NavMeshAgent is not assigned!");
                return;
            }

            if (NavMesh.SamplePosition(destination, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
            {
                _navMeshAgent.SetDestination(hit.position);
            }
        }

        public void Stop()
        {
            if (_navMeshAgent == null)
                return;

            _navMeshAgent.ResetPath();
        }

        private void ApplyConfiguration()
        {
            _navMeshAgent.updateRotation = true;

            if (_movementSettings == null)
            {
                Debug.LogError("Cannot apply null movement settings!");
                return;
            }

            _movementSettings.ApplyToAgent(_navMeshAgent);
        }
    }
}
