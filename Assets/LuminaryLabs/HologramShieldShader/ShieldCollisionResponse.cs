using System.Collections.Generic;
using UnityEngine;

namespace LuminaryLabs.HologramShieldShader
{
    public class ShieldCollisionResponse : MonoBehaviour
    {
        // Ссылка на рендерер щита
        private Renderer shieldRenderer;
        // Уникальный экземпляр материала для щита
        private Material shieldMaterial;

        // Параметры эффекта коллизии
        public float effectDuration = 1f;
        public float collisionRadius = 0.5f;
        public float collisionIntensity = 1f;

        // Максимальное число коллизий, поддерживаемое шейдером
        private const int MAX_COLLISIONS = 4;

        // Структура данных для одной коллизии
        private class CollisionData
        {
            public Vector3 point;
            public float radius;
            public float intensity;
            public float startTime;
        }

        // Список активных коллизий
        private List<CollisionData> activeCollisions = new List<CollisionData>();

        void Awake()
        {
            shieldRenderer = GetComponent<Renderer>();
            shieldMaterial = shieldRenderer.material; // Клонирование материала
        }

        void Update()
        {
            float currentTime = Time.time;

            // Удаление устаревших коллизий
            activeCollisions.RemoveAll(c => (currentTime - c.startTime) > effectDuration);

            // Подготовка массивов для шейдера
            Vector4[] collisionPoints = new Vector4[MAX_COLLISIONS];
            float[] collisionRadii = new float[MAX_COLLISIONS];
            float[] collisionIntensities = new float[MAX_COLLISIONS];
            float[] collisionStartTimes = new float[MAX_COLLISIONS];

            int count = Mathf.Min(activeCollisions.Count, MAX_COLLISIONS);
            for (int i = 0; i < count; i++)
            {
                CollisionData col = activeCollisions[i];
                float fade = Mathf.Clamp01(1f - ((currentTime - col.startTime) / effectDuration));
                collisionPoints[i] = new Vector4(col.point.x, col.point.y, col.point.z, 0f);
                collisionRadii[i] = col.radius;
                collisionIntensities[i] = col.intensity * fade;
                collisionStartTimes[i] = col.startTime;
            }

            for (int i = count; i < MAX_COLLISIONS; i++)
            {
                collisionPoints[i] = Vector4.zero;
                collisionRadii[i] = 0f;
                collisionIntensities[i] = 0f;
                collisionStartTimes[i] = 0f;
            }

            // Обновление параметров в шейдере
            shieldMaterial.SetVectorArray("_CollisionPoints", collisionPoints);
            shieldMaterial.SetFloatArray("_CollisionRadii", collisionRadii);
            shieldMaterial.SetFloatArray("_CollisionIntensities", collisionIntensities);
            shieldMaterial.SetFloatArray("_CollisionStartTimes", collisionStartTimes);
            shieldMaterial.SetInt("_NumCollisions", count);
            shieldMaterial.SetFloat("_EffectDuration", effectDuration);
        }

        // Обработка входа в триггер
        private void OnTriggerEnter(Collider other)
        {
            // Получаем точку ближайшую к поверхности щита
            Vector3 collisionPoint = other.ClosestPoint(transform.position);

            CollisionData newCollision = new CollisionData
            {
                point = collisionPoint,
                radius = collisionRadius,
                intensity = collisionIntensity,
                startTime = Time.time
            };

            if (activeCollisions.Count < MAX_COLLISIONS)
            {
                activeCollisions.Add(newCollision);
            }
            else
            {
                float minRemaining = float.MaxValue;
                int replaceIndex = 0;
                float currentTime = Time.time;
                for (int i = 0; i < activeCollisions.Count; i++)
                {
                    float remaining = effectDuration - (currentTime - activeCollisions[i].startTime);
                    if (remaining < minRemaining)
                    {
                        minRemaining = remaining;
                        replaceIndex = i;
                    }
                }
                activeCollisions[replaceIndex] = newCollision;
            }
        }
    }
}
