using Code.Gameplay.Character.Settings;
using UnityEngine;

namespace Code.Infrastructure.Services.Camera
{
    public class CameraService : ICameraService
    {
        public UnityEngine.Camera MainCamera { get; private set; }

        private readonly CameraProvider _cameraProvider;

        public CameraService(CameraSettings cameraSettings)
        {
            _cameraProvider = new CameraProvider(cameraSettings);
        }
        public void SetMainCamera(UnityEngine.Camera camera)
        {
            MainCamera = camera;
            _cameraProvider?.SetCameraTransform(MainCamera.transform);
        }

        public void SetPlayerTransform(Transform playerTransform)
        {
            _cameraProvider?.SetPlayerTransform(playerTransform);
        }

        public void UpdateCamera()
        {
            _cameraProvider?.UpdateCameraPosition();
        }

        public void MoveCamera(Vector3 direction)
        {
            _cameraProvider?.MoveCamera(direction);
        }

        public void ZoomCamera(float zoomDelta)
        {
            _cameraProvider?.ZoomCamera(zoomDelta);
        }

        public bool IsCameraMoving => _cameraProvider?.IsMoving ?? false;
    }
}
