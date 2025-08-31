using Code.Infrastructure.Services;
using Code.Infrastructure.Services.Camera;
using UnityEngine;

namespace Code.Gameplay.Player
{
    public class CameraInputHandler
    {
        private readonly ICameraService _cameraService = ServiceLocator.Instance.GetService<ICameraService>();

        public CameraInputHandler(Transform player)
        {
            _cameraService.SetPlayerTransform(player);
        }

        public void HandleInput()
        {
            HandleMovementInput();
            HandleZoomInput();
        }

        public void UpdateCamera()
        {
            _cameraService.UpdateCamera();
        }

        private void HandleMovementInput()
        {
            Vector3 movementDirection = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
                movementDirection.z += 1f;

            if (Input.GetKey(KeyCode.S))
                movementDirection.z -= 1f;

            if (Input.GetKey(KeyCode.A))
                movementDirection.x -= 1f;

            if (Input.GetKey(KeyCode.D))
                movementDirection.x += 1f;

            // Нормализуем направление для диагонального движения
            if (movementDirection.magnitude > 0f)
            {
                movementDirection.Normalize();
                _cameraService.MoveCamera(movementDirection);
            }
        }

        private void HandleZoomInput()
        {
            float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
            
            if (Mathf.Abs(scrollDelta) > 0.01f)
            {
                _cameraService.ZoomCamera(scrollDelta);
            }
        }
    }
}
