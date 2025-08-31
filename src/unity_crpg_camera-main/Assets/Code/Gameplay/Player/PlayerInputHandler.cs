using System;
using Code.Infrastructure.Services;
using Code.Infrastructure.Services.Camera;
using UnityEngine;

namespace Code.Gameplay.Player
{
    public class PlayerInputHandler
    {
        private readonly ICameraService _cameraService = ServiceLocator.Instance.GetService<ICameraService>();

        public event Action<Vector3> OnDestinationSelected;

        public void HandleInput()
        {
            if (!Input.GetMouseButtonDown(0))
                return;

            var camera = _cameraService?.MainCamera;
            if (camera == null)
            {
                return;
            }

            var ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit))
            {
                OnDestinationSelected?.Invoke(hit.point);
            }
        }
    }
}
