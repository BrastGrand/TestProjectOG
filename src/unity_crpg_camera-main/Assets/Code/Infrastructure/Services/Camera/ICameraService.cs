using UnityEngine;

namespace Code.Infrastructure.Services.Camera
{
    public interface ICameraService : IService
    {
        UnityEngine.Camera MainCamera { get; }
        bool IsCameraMoving { get; }
        
        void SetMainCamera(UnityEngine.Camera camera);
        void SetPlayerTransform(Transform playerTransform);
        void UpdateCamera();
        void MoveCamera(Vector3 direction);
        void ZoomCamera(float zoomDelta);
    }
}
