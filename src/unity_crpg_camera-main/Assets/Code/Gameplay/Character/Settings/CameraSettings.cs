using System;
using UnityEngine;

namespace Code.Gameplay.Character.Settings
{
    [CreateAssetMenu(fileName = "CameraSettings", menuName = "Settings/CameraSettings")]
    public class CameraSettings : ScriptableObject
    {
        [Header("Movement Settings")]
        [Tooltip("Скорость движения камеры (5-15)")]
        [SerializeField] private float _movementSpeed = 10f;
        
        [Tooltip("Скорость приближения/отдаления камеры (2-8)")]
        [SerializeField] private float _zoomSpeed = 5f;
        
        [Header("Height Calculation")]
        [Tooltip("Минимальная высота камеры над землей (2-5)")]
        [SerializeField] private float _minHeightAboveGround = 3.0f;
        
        [Tooltip("Дополнительная высота для перепадов высот (1-3)")]
        [SerializeField] private float _heightOffset = 2f;
        
        [Tooltip("Радиус проверки высоты вокруг камеры (5-15)")]
        [SerializeField] private float _heightCheckRadius = 10f;
        
        [Header("Distance Constraints")]
        [Tooltip("Минимальное расстояние от персонажа")]
        [SerializeField] private float _minDistanceFromPlayer = 5f;
        
        [Tooltip("Максимальное расстояние от персонажа")]
        [SerializeField] private float _maxDistanceFromPlayer = 25f;

        [Tooltip("Включить ограничение дистанции при движении WASD")]
        [SerializeField] private bool _enableDistanceConstraint = true;

        [Tooltip("Минимальное расстояние от персонажа при зуме")]
        [SerializeField] private float _minZoomDistanceFromPlayer = 1f;

        [Tooltip("Максимальное расстояние от персонажа при зуме")]
        [SerializeField] private float _maxZoomDistanceFromPlayer = 70f;

        [Header("Smooth Movement")]
        [Tooltip("Плавность движения камеры (0.1-0.3)")]
        [SerializeField] private float _movementSmoothness = 0.15f;
        
        [Tooltip("Плавность изменения высоты (0.1-0.3)")]
        [SerializeField] private float _heightSmoothness = 0.2f;

        public float MovementSpeed => _movementSpeed;
        public float ZoomSpeed => _zoomSpeed;
        public float MinHeightAboveGround => _minHeightAboveGround;
        public float HeightOffset => _heightOffset;
        public float HeightCheckRadius => _heightCheckRadius;
        public float MinDistanceFromPlayer => _minDistanceFromPlayer;
        public float MaxDistanceFromPlayer => _maxDistanceFromPlayer;
        public float MinZoomDistanceFromPlayer => _minDistanceFromPlayer;
        public float MaxZoomDistanceFromPlayer => _maxZoomDistanceFromPlayer;
        public bool EnableDistanceConstraint => _enableDistanceConstraint;
        public float MovementSmoothness => _movementSmoothness;
        public float HeightSmoothness => _heightSmoothness;
    }
}
