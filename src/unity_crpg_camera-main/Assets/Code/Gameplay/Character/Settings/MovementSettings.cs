using System;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Gameplay.Character.Settings
{
    [Serializable]
    public class MovementSettings
    {
        [Header("Speed Settings")]
        [Tooltip("Скорость бега (6.0-8.0)")]
        [SerializeField] private float _runSpeed = 7.0f;
        
        [Header("Pathfinding Settings")]
        [Tooltip("Радиус агента - размер персонажа (0.4-0.6)")]
        [SerializeField] private float _agentRadius = 0.5f;
        
        [Tooltip("Высота агента (1.6-2.0)")]
        [SerializeField] private float _agentHeight = 1.8f;
        
        [Header("Movement Responsiveness")]
        [Tooltip("Ускорение - как быстро набирает скорость (12-20 для отзывчивости)")]
        [SerializeField] private float _acceleration = 16.0f;
        
        [Tooltip("Торможение - как быстро останавливается (12-20 для плавности)")]
        [SerializeField] private float _stoppingDistance = 0.1f;
        
        [Header("Rotation Settings")]
        [Tooltip("Скорость поворота (300-540 degrees/sec)")]
        [SerializeField] private float _angularSpeed = 360.0f;
        
        [Header("Pathfinding Quality")]
        [Tooltip("Качество обхода препятствий (0-4, где 4 самое высокое)")]
        [SerializeField] private int _obstacleAvoidanceType = 3;

        [Header("Advanced Settings")]
        [Tooltip("Автоматическое торможение у цели")]
        [SerializeField] private bool _autoBraking = true;
        
        [Tooltip("Автоматическое переназначение пути при препятствиях")]
        [SerializeField] private bool _autoRepath = true;
        
        [Tooltip("Приоритет агента (50 = средний, выше = важнее)")]
        [SerializeField] private int _priority = 50;

        public float StoppingDistance => _stoppingDistance;


        public void ApplyToAgent(NavMeshAgent agent)
        {
            if (agent == null) return;

            // Основные параметры движения
            agent.speed = _runSpeed;
            agent.acceleration = _acceleration;
            agent.angularSpeed = _angularSpeed;
            agent.stoppingDistance = _stoppingDistance;
            
            // Размеры агента
            agent.radius = _agentRadius;
            agent.height = _agentHeight;
            
            // Поведение
            agent.autoBraking = _autoBraking;
            agent.autoRepath = _autoRepath;
            agent.avoidancePriority = _priority;
            
            // Качество обхода препятствий (0-4, где 4 = самое высокое)
            agent.obstacleAvoidanceType = (ObstacleAvoidanceType)_obstacleAvoidanceType;
        }

        public static MovementSettings CreatePathfinderStyle()
        {
            return new MovementSettings
            {
                _runSpeed = 7.0f,
                _agentRadius = 0.5f,
                _agentHeight = 1.8f,
                _acceleration = 16.0f,
                _stoppingDistance = 0.1f,
                _angularSpeed = 360.0f,
                _obstacleAvoidanceType = 3,
                _autoBraking = true,
                _autoRepath = true,
                _priority = 50
            };
        }
    }
}
