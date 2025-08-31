using Code.Gameplay.Character.Settings;
using UnityEngine;

namespace Code.Infrastructure.Services.Camera
{
    public class CameraProvider
    {
        private readonly CameraSettings _cameraSettings;
        private Transform _cameraTransform;
        private Transform _playerTransform;
        
        private Vector3 _targetPosition;
        private float _targetHeight;
        private Vector3 _currentVelocity;
        private float _currentHeightVelocity;
        
        public bool IsMoving => Vector3.Distance(_cameraTransform.position, _targetPosition) > 0.1f;

        public CameraProvider(CameraSettings cameraSettings)
        {
            _cameraSettings = cameraSettings;
            _targetPosition = Vector3.zero;
            _targetHeight = 0f;
        }

        public void SetCameraTransform(Transform cameraTransform)
        {
            _cameraTransform = cameraTransform;
            if (_cameraTransform != null)
            {
                _targetPosition = _cameraTransform.position;
                _targetHeight = _cameraTransform.position.y;
            }
        }

        public void SetPlayerTransform(Transform playerTransform)
        {
            _playerTransform = playerTransform;
        }

        public void MoveCamera(Vector3 direction)
        {
            if (_cameraTransform == null) return;

            Vector3 movement = direction.normalized * _cameraSettings.MovementSpeed * Time.deltaTime;
            Vector3 worldMovement = _cameraTransform.TransformDirection(movement);
            worldMovement.y = 0; // Игнорируем вертикальное движение
            
            // Применяем ограничение дистанции от персонажа
            if (_cameraSettings.EnableDistanceConstraint && _playerTransform != null)
            {
                Vector3 newPosition = _targetPosition + worldMovement;
                Vector3 playerPosition = _playerTransform.position;
                float distanceToPlayer = Vector3.Distance(newPosition, playerPosition);
                
                if (distanceToPlayer < _cameraSettings.MinDistanceFromPlayer)
                {
                    Vector3 directionToPlayer = (newPosition - playerPosition).normalized;
                    newPosition = playerPosition + directionToPlayer * _cameraSettings.MinDistanceFromPlayer;
                }
                else if (distanceToPlayer > _cameraSettings.MaxDistanceFromPlayer)
                {
                    Vector3 directionToPlayer = (newPosition - playerPosition).normalized;
                    newPosition = playerPosition + directionToPlayer * _cameraSettings.MaxDistanceFromPlayer;
                }
                
                _targetPosition = newPosition;
            }
            else
            {
                _targetPosition += worldMovement;
            }
        }

        public void ZoomCamera(float zoomDelta)
        {
            if (_cameraTransform == null) return;

            // Приближение/отдаление изменяет высоту камеры
            float heightChange = -zoomDelta * _cameraSettings.ZoomSpeed;
            _targetHeight += heightChange;

            // Ограничиваем минимальную и максимальную высоту
            _targetHeight = Mathf.Clamp(_targetHeight,
                _cameraSettings.MinZoomDistanceFromPlayer + _playerTransform.position.y,
                _cameraSettings.MaxZoomDistanceFromPlayer + _playerTransform.position.y);
        }

        public void UpdateCameraPosition()
        {
            if (_cameraTransform == null) return;

            // Плавно перемещаем камеру к целевой позиции
            Vector3 targetPos = new Vector3(_targetPosition.x, _targetHeight, _targetPosition.z);
            Vector3 smoothedPosition = Vector3.SmoothDamp(
                _cameraTransform.position, 
                targetPos, 
                ref _currentVelocity, 
                _cameraSettings.MovementSmoothness);

            // Применяем ограничение дистанции от персонажа
            if (_cameraSettings.EnableDistanceConstraint && _playerTransform != null)
            {
                Vector3 playerPosition = _playerTransform.position;
                float distanceToPlayer = Vector3.Distance(smoothedPosition, playerPosition);
                
                if (distanceToPlayer < _cameraSettings.MinDistanceFromPlayer)
                {
                    Vector3 directionToPlayer = (smoothedPosition - playerPosition).normalized;
                    smoothedPosition = playerPosition + directionToPlayer * _cameraSettings.MinDistanceFromPlayer;
                }
                else if (distanceToPlayer > _cameraSettings.MaxDistanceFromPlayer)
                {
                    Vector3 directionToPlayer = (smoothedPosition - playerPosition).normalized;
                    smoothedPosition = playerPosition + directionToPlayer * _cameraSettings.MaxDistanceFromPlayer;
                }
            }

            // Рассчитываем оптимальную высоту с учетом перепадов высот
            float optimalHeight = CalculateOptimalHeight(smoothedPosition);
            smoothedPosition.y = Mathf.SmoothDamp(
                smoothedPosition.y, 
                optimalHeight, 
                ref _currentHeightVelocity, 
                _cameraSettings.HeightSmoothness);

            // Применяем позицию
            _cameraTransform.position = smoothedPosition;
            
            // Обновляем целевые значения
            _targetPosition = new Vector3(smoothedPosition.x, _targetPosition.y, smoothedPosition.z);
            _targetHeight = smoothedPosition.y;
        }

        private float CalculateOptimalHeight(Vector3 position)
        {
            float maxHeight = position.y;
            
            // Проверяем высоту в радиусе вокруг камеры
            for (int i = 0; i < 8; i++)
            {
                float angle = i * 45f * Mathf.Deg2Rad;
                Vector3 checkPosition = position + new Vector3(
                    Mathf.Cos(angle) * _cameraSettings.HeightCheckRadius,
                    0,
                    Mathf.Sin(angle) * _cameraSettings.HeightCheckRadius
                );
                
                if (Physics.Raycast(checkPosition + Vector3.up * 100f, Vector3.down, out RaycastHit hit, 200f))
                {
                    float groundHeight = hit.point.y;
                    float requiredHeight = groundHeight + _cameraSettings.MinHeightAboveGround + _cameraSettings.HeightOffset;
                    maxHeight = Mathf.Max(maxHeight, requiredHeight);
                }
            }
            
            return maxHeight;
        }
    }
}
