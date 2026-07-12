// OpenTK 4
#if OpenTK_4//C# 7
namespace BuslikDrev.Physics {
    // 1. Структура для быстрого луча с полными путями
    public struct FastRay {
        public readonly OpenTK.Mathematics.Vector3 Origin;
        public readonly OpenTK.Mathematics.Vector3 Direction;
        public readonly OpenTK.Mathematics.Vector3 InvDir;

        public FastRay(OpenTK.Mathematics.Vector3 origin, OpenTK.Mathematics.Vector3 direction) {
            this.Origin = origin;
            this.Direction = OpenTK.Mathematics.Vector3.Normalize(direction);
            
            // Предварительный расчет для исключения деления в циклах
            this.InvDir = new OpenTK.Mathematics.Vector3(
                1.0f / this.Direction.X, 
                1.0f / this.Direction.Y, 
                1.0f / this.Direction.Z
            );
        }
    }

    // 2. Структура AABB с полными путями
    public struct AABB {
        public OpenTK.Mathematics.Vector3 Min;
        public OpenTK.Mathematics.Vector3 Max;

        public AABB(OpenTK.Mathematics.Vector3 min, OpenTK.Mathematics.Vector3 max) {
            this.Min = min;
            this.Max = max;
        }

        // Высокопроизводительный расчет пересечения
        public bool Intersect(BuslikDrev.Physics.FastRay ray, out float distance) {
            float t1 = (this.Min.X - ray.Origin.X) * ray.InvDir.X;
            float t2 = (this.Max.X - ray.Origin.X) * ray.InvDir.X;
            float tmin = System.MathF.Min(t1, t2);
            float tmax = System.MathF.Max(t1, t2);

            float t3 = (this.Min.Y - ray.Origin.Y) * ray.InvDir.Y;
            float t4 = (this.Max.Y - ray.Origin.Y) * ray.InvDir.Y;
            tmin = System.MathF.Max(tmin, System.MathF.Min(t3, t4));
            tmax = System.MathF.Min(tmax, System.MathF.Max(t3, t4));

            float t5 = (this.Min.Z - ray.Origin.Z) * ray.InvDir.Z;
            float t6 = (this.Max.Z - ray.Origin.Z) * ray.InvDir.Z;
            tmin = System.MathF.Max(tmin, System.MathF.Min(t5, t6));
            tmax = System.MathF.Min(tmax, System.MathF.Max(t5, t6));

            distance = tmin;

            return tmax >= System.MathF.Max(0, tmin);
        }
    }

    // 3. Класс обработки нахождения расстояний
    public static class ObjectPicker {
        public static BuslikDrev.Physics.FastRay GetMouseRay(
            float mouseX, 
            float mouseY, 
            int screenWidth, 
            int screenHeight, 
            OpenTK.Mathematics.Matrix4 projection, 
            OpenTK.Mathematics.Matrix4 view) {
            float x = (2.0f * mouseX) / screenWidth - 1.0f;
            float y = 1.0f - (2.0f * mouseY) / screenHeight;

            OpenTK.Mathematics.Vector4 rayClip = new OpenTK.Mathematics.Vector4(x, y, -1.0f, 1.0f);
            
            OpenTK.Mathematics.Matrix4 invProjection = OpenTK.Mathematics.Matrix4.Invert(projection);
            OpenTK.Mathematics.Vector4 rayEye = OpenTK.Mathematics.Vector4.TransformRow(rayClip, invProjection);
            rayEye = new OpenTK.Mathematics.Vector4(rayEye.X, rayEye.Y, -1.0f, 0.0f);
            
            OpenTK.Mathematics.Matrix4 invView = OpenTK.Mathematics.Matrix4.Invert(view);
            OpenTK.Mathematics.Vector3 rayWorld = OpenTK.Mathematics.Vector4.TransformRow(rayEye, invView).Xyz;
            
            // Позиция камеры напрямую из матрицы вида
            OpenTK.Mathematics.Vector3 cameraPos = invView.ExtractTranslation();
            
            return new BuslikDrev.Physics.FastRay(cameraPos, rayWorld);
        }

        public static T FindClosestObject<T>(BuslikDrev.Physics.FastRay ray, System.Collections.Generic.IEnumerable<T> objects, System.Func<T, BuslikDrev.Physics.AABB> getAABB) where T : class {
            T closestObject = null;
            float minDistance = float.MaxValue;

            foreach (var obj in objects)
            {
                BuslikDrev.Physics.AABB worldBox = getAABB(obj);
                
                if (worldBox.Intersect(ray, out float distance))
                {
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestObject = obj;
                    }
                }
            }
            return closestObject;
        }
    }
}
#endif

#if OpenTK_3 //C# 5
namespace BuslikDrev.Physics
{
    // 1. Структура для быстрого луча
    public struct FastRay {
        public OpenTK.Vector3 Origin;
        public OpenTK.Vector3 Direction;
        public OpenTK.Vector3 InvDir;

        public FastRay(OpenTK.Vector3 origin, OpenTK.Vector3 direction) {
            this.Origin = origin;
            // В OpenTK 3.3 метод Normalize возвращает нормализованную копию
            this.Direction = OpenTK.Vector3.Normalize(direction);
            
            // Предварительный расчет для исключения деления в циклах
            // В C# 5 Math.Min/Max работают с double, поэтому приводим к float
            this.InvDir = new OpenTK.Vector3(
                1.0f / this.Direction.X, 
                1.0f / this.Direction.Y, 
                1.0f / this.Direction.Z
            );
        }
    }

    // 2. Структура AABB
    public struct AABB {
        public OpenTK.Vector3 Min;
        public OpenTK.Vector3 Max;

        public AABB(OpenTK.Vector3 min, OpenTK.Vector3 max) {
            this.Min = min;
            this.Max = max;
        }

        public bool Intersect(BuslikDrev.Physics.FastRay ray, out float distance) {
            float t1 = (this.Min.X - ray.Origin.X) * ray.InvDir.X;
            float t2 = (this.Max.X - ray.Origin.X) * ray.InvDir.X;
            float tmin = (float)System.Math.Min((double)t1, (double)t2);
            float tmax = (float)System.Math.Max((double)t1, (double)t2);

            float t3 = (this.Min.Y - ray.Origin.Y) * ray.InvDir.Y;
            float t4 = (this.Max.Y - ray.Origin.Y) * ray.InvDir.Y;
            tmin = (float)System.Math.Max((double)tmin, (double)System.Math.Min((double)t3, (double)t4));
            tmax = (float)System.Math.Min((double)tmax, (double)System.Math.Max((double)t3, (double)t4));

            float t5 = (this.Min.Z - ray.Origin.Z) * ray.InvDir.Z;
            float t6 = (this.Max.Z - ray.Origin.Z) * ray.InvDir.Z;
            tmin = (float)System.Math.Max((double)tmin, (double)System.Math.Min((double)t5, (double)t6));
            tmax = (float)System.Math.Min((double)tmax, (double)System.Math.Max((double)t5, (double)t6));

            distance = tmin;

            // Сравнение для определения попадания
            return tmax >= (float)System.Math.Max(0.0, (double)tmin);
        }
    }

    // 3. Класс обработки нахождения расстояний
    public static class ObjectPicker {
        public static BuslikDrev.Physics.FastRay GetMouseRay(
            float mouseX, 
            float mouseY, 
            int screenWidth, 
            int screenHeight, 
            OpenTK.Matrix4 projection, 
            OpenTK.Matrix4 view)
        {
            float x = (2.0f * mouseX) / (float)screenWidth - 1.0f;
            float y = 1.0f - (2.0f * mouseY) / (float)screenHeight;

            OpenTK.Vector4 rayClip = new OpenTK.Vector4(x, y, -1.0f, 1.0f);
            
            OpenTK.Matrix4 invProjection = OpenTK.Matrix4.Invert(projection);
            
            // В OpenTK 3.3 Vector4.Transform принимает вектор и матрицу
            OpenTK.Vector4 rayEye = OpenTK.Vector4.Transform(rayClip, invProjection);
            rayEye = new OpenTK.Vector4(rayEye.X, rayEye.Y, -1.0f, 0.0f);
            
            OpenTK.Matrix4 invView = OpenTK.Matrix4.Invert(view);
            OpenTK.Vector3 rayWorld = OpenTK.Vector4.Transform(rayEye, invView).Xyz;
            
            // Извлечение позиции камеры из инвертированной матрицы вида
            OpenTK.Vector3 cameraPos = invView.ExtractTranslation();
            
            return new BuslikDrev.Physics.FastRay(cameraPos, rayWorld);
        }

        public static T FindClosestObject<T>(
            BuslikDrev.Physics.FastRay ray, 
            System.Collections.Generic.IEnumerable<T> objects, 
            System.Func<T, BuslikDrev.Physics.AABB> getAABB) where T : class
        {
            T closestObject = null;
            float minDistance = float.MaxValue;

            foreach (T obj in objects) {
                BuslikDrev.Physics.AABB worldBox = getAABB(obj);
                float distance;
                
                if (worldBox.Intersect(ray, out distance))
                {
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestObject = obj;
                    }
                }
            }
            return closestObject;
        }
    }
}

namespace BuslikDrev.Physics
{
    // 1. Структура для быстрого луча
    public struct FastRay
    {
        public OpenTK.Vector3 Origin;
        public OpenTK.Vector3 Direction;
        public OpenTK.Vector3 InvDir;

        public FastRay(OpenTK.Vector3 origin, OpenTK.Vector3 direction)
        {
            this.Origin = origin;
            // Нормализация направления
            this.Direction = OpenTK.Vector3.Normalize(direction);
            
            // Предварительный расчет обратного направления для ускорения пересечений.
            // float.Epsilon используется для предотвращения деления на чистый ноль.
            this.InvDir = new OpenTK.Vector3(
                1.0f / (System.Math.Abs(this.Direction.X) < float.Epsilon ? float.Epsilon : this.Direction.X),
                1.0f / (System.Math.Abs(this.Direction.Y) < float.Epsilon ? float.Epsilon : this.Direction.Y),
                1.0f / (System.Math.Abs(this.Direction.Z) < float.Epsilon ? float.Epsilon : this.Direction.Z)
            );
        }
    }

    // 2. Структура AABB (Axis-Aligned Bounding Box)
    public struct AABB {
        public OpenTK.Vector3 Min;
        public OpenTK.Vector3 Max;

        public AABB(OpenTK.Vector3 min, OpenTK.Vector3 max) {
            this.Min = min;
            this.Max = max;
        }

        public bool Intersect(BuslikDrev.Physics.FastRay ray, out float distance) {
            float t1 = (this.Min.X - ray.Origin.X) * ray.InvDir.X;
            float t2 = (this.Max.X - ray.Origin.X) * ray.InvDir.X;
            float t3 = (this.Min.Y - ray.Origin.Y) * ray.InvDir.Y;
            float t4 = (this.Max.Y - ray.Origin.Y) * ray.InvDir.Y;
            float t5 = (this.Min.Z - ray.Origin.Z) * ray.InvDir.Z;
            float t6 = (this.Max.Z - ray.Origin.Z) * ray.InvDir.Z;

            float tmin = (float)System.Math.Max(System.Math.Max(System.Math.Min((double)t1, (double)t2), System.Math.Min((double)t3, (double)t4)), System.Math.Min((double)t5, (double)t6));
            float tmax = (float)System.Math.Min(System.Math.Min(System.Math.Max((double)t1, (double)t2), System.Math.Max((double)t3, (double)t4)), System.Math.Max((double)t5, (double)t6));

            distance = tmin;

            // Если tmax < 0, объект находится позади луча. 
            // Если tmin > tmax, луч проходит мимо.
            return tmax >= (float)System.Math.Max(0.0, (double)tmin);
        }
    }

    // 3. Дополнительные тесты пересечений
    public static class IntersectionTests {
        // Алгоритм Моллера — Трумбора для пересечения луча с треугольником
        public static bool RayTriangle(BuslikDrev.Physics.FastRay ray, OpenTK.Vector3 v0, OpenTK.Vector3 v1, OpenTK.Vector3 v2, out float t)
        {
            t = 0;
            OpenTK.Vector3 edge1 = v1 - v0;
            OpenTK.Vector3 edge2 = v2 - v0;
            OpenTK.Vector3 h = OpenTK.Vector3.Cross(ray.Direction, edge2);
            float a = OpenTK.Vector3.Dot(edge1, h);

            if (a > -float.Epsilon && a < float.Epsilon) return false;

            float f = 1.0f / a;
            OpenTK.Vector3 s = ray.Origin - v0;
            float u = f * OpenTK.Vector3.Dot(s, h);
            if (u < 0.0f || u > 1.0f) return false;

            OpenTK.Vector3 q = OpenTK.Vector3.Cross(s, edge1);
            float v = f * OpenTK.Vector3.Dot(ray.Direction, q);
            if (v < 0.0f || u + v > 1.0f) return false;

            t = f * OpenTK.Vector3.Dot(edge2, q);
            return t > float.Epsilon;
        }
    }

    // 4. Основной класс для работы с мышью и объектами
    public static class ObjectPicker
    {
        public static BuslikDrev.Physics.FastRay GetMouseRay(
            float mouseX, 
            float mouseY, 
            int screenWidth, 
            int screenHeight, 
            OpenTK.Matrix4 projection, 
            OpenTK.Matrix4 view)
        {
            // Перевод координат мыши в Normalized Device Coordinates (NDC)
            float x = (2.0f * mouseX) / (float)screenWidth - 1.0f;
            float y = 1.0f - (2.0f * mouseY) / (float)screenHeight;

            OpenTK.Vector4 rayClip = new OpenTK.Vector4(x, y, -1.0f, 1.0f);
            
            OpenTK.Matrix4 invProjection = OpenTK.Matrix4.Invert(projection);
            OpenTK.Vector4 rayEye = OpenTK.Vector4.Transform(rayClip, invProjection);
            rayEye = new OpenTK.Vector4(rayEye.X, rayEye.Y, -1.0f, 0.0f);
            
            OpenTK.Matrix4 invView = OpenTK.Matrix4.Invert(view);
            OpenTK.Vector3 rayWorld = OpenTK.Vector4.Transform(rayEye, invView).Xyz;
            
            // Позиция камеры (начало луча)
            OpenTK.Vector3 cameraPos = invView.ExtractTranslation();
            
            return new BuslikDrev.Physics.FastRay(cameraPos, rayWorld);
        }

        public static T FindClosestObject<T>(
            BuslikDrev.Physics.FastRay ray, 
            System.Collections.Generic.IEnumerable<T> objects, 
            System.Func<T, BuslikDrev.Physics.AABB> getAABB) where T : class
        {
            T closestObject = null;
            float minDistance = float.MaxValue;

            foreach (T obj in objects)
            {
                BuslikDrev.Physics.AABB worldBox = getAABB(obj);
                float distance;
                
                if (worldBox.Intersect(ray, out distance))
                {
                    // Проверка, что объект перед камерой и ближе всех найденных
                    if (distance >= 0 && distance < minDistance)
                    {
                        minDistance = distance;
                        closestObject = obj;
                    }
                }
            }
            return closestObject;
        }
    }
}
#endif


namespace BuslikDrev.Physics {
    // 1. Структура для быстрого луча
    public struct FastRay
    {
        public OpenTK.Vector3 Origin;
        public OpenTK.Vector3 Direction;
        public OpenTK.Vector3 InvDir;

        public FastRay(OpenTK.Vector3 origin, OpenTK.Vector3 direction)
        {
            this.Origin = origin;
            // Нормализация направления
            this.Direction = OpenTK.Vector3.Normalize(direction);
            
            // Предварительный расчет обратного направления для ускорения пересечений.
            // float.Epsilon используется для предотвращения деления на чистый ноль.
            this.InvDir = new OpenTK.Vector3(
                1.0f / (System.Math.Abs(this.Direction.X) < float.Epsilon ? float.Epsilon : this.Direction.X),
                1.0f / (System.Math.Abs(this.Direction.Y) < float.Epsilon ? float.Epsilon : this.Direction.Y),
                1.0f / (System.Math.Abs(this.Direction.Z) < float.Epsilon ? float.Epsilon : this.Direction.Z)
            );
        }
    }

    // 3. Дополнительные тесты пересечений
    public static class IntersectionTests
    {
        // Алгоритм Моллера — Трумбора для пересечения луча с треугольником
        public static bool RayTriangle(BuslikDrev.Physics.FastRay ray, OpenTK.Vector3 v0, OpenTK.Vector3 v1, OpenTK.Vector3 v2, out float t)
        {
            t = 0;
            OpenTK.Vector3 edge1 = v1 - v0;
            OpenTK.Vector3 edge2 = v2 - v0;
            OpenTK.Vector3 h = OpenTK.Vector3.Cross(ray.Direction, edge2);
            float a = OpenTK.Vector3.Dot(edge1, h);

            if (a > -float.Epsilon && a < float.Epsilon) return false;

            float f = 1.0f / a;
            OpenTK.Vector3 s = ray.Origin - v0;
            float u = f * OpenTK.Vector3.Dot(s, h);
            if (u < 0.0f || u > 1.0f) return false;

            OpenTK.Vector3 q = OpenTK.Vector3.Cross(s, edge1);
            float v = f * OpenTK.Vector3.Dot(ray.Direction, q);
            if (v < 0.0f || u + v > 1.0f) return false;

            t = f * OpenTK.Vector3.Dot(edge2, q);
            return t > float.Epsilon;
        }
    }

    // 4. Основной класс для работы с мышью и объектами
    public static class ObjectPicker
    {
        public static BuslikDrev.Physics.FastRay GetMouseRay(
            float mouseX, 
            float mouseY, 
            int screenWidth, 
            int screenHeight, 
            OpenTK.Matrix4 projection, 
            OpenTK.Matrix4 view)
        {
            // Перевод координат мыши в Normalized Device Coordinates (NDC)
            float x = (2.0f * mouseX) / (float)screenWidth - 1.0f;
            float y = 1.0f - (2.0f * mouseY) / (float)screenHeight;

            OpenTK.Vector4 rayClip = new OpenTK.Vector4(x, y, -1.0f, 1.0f);
            
            OpenTK.Matrix4 invProjection = OpenTK.Matrix4.Invert(projection);
            OpenTK.Vector4 rayEye = OpenTK.Vector4.Transform(rayClip, invProjection);
            rayEye = new OpenTK.Vector4(rayEye.X, rayEye.Y, -1.0f, 0.0f);
            
            OpenTK.Matrix4 invView = OpenTK.Matrix4.Invert(view);
            OpenTK.Vector3 rayWorld = OpenTK.Vector4.Transform(rayEye, invView).Xyz;
            
            // Позиция камеры (начало луча)
            OpenTK.Vector3 cameraPos = invView.ExtractTranslation();
            
            return new BuslikDrev.Physics.FastRay(cameraPos, rayWorld);
        }

        public static T FindClosestObject<T>(
            BuslikDrev.Physics.FastRay ray, 
            System.Collections.Generic.IEnumerable<T> objects, 
            System.Func<T, BuslikDrev.Physics.AABB> getAABB) where T : class
        {
            T closestObject = null;
            float minDistance = float.MaxValue;

            foreach (T obj in objects)
            {
                BuslikDrev.Physics.AABB worldBox = getAABB(obj);
                float distance;
                
                if (worldBox.Intersect(ray, out distance))
                {
                    // Проверка, что объект перед камерой и ближе всех найденных
                    if (distance >= 0 && distance < minDistance) {
                        minDistance = distance;
                        closestObject = obj;
                    }
                }
            }
            return closestObject;
        }
    }
}

/* public class MyEntity 
{
    public OpenTK.Vector3 Position;
    public float Scale = 1.0f;
    // Локальные границы модели (например, от -1 до 1)
    public BuslikDrev.Physics.AABB LocalAABB; 

    // Метод получения мирового AABB
    public BuslikDrev.Physics.AABB GetWorldAABB() 
    {
        return new BuslikDrev.Physics.AABB(
            (LocalAABB.Min * Scale) + Position,
            (LocalAABB.Max * Scale) + Position
        );
    }
}

// ГДЕ-ТО В ГЛАВНОМ ЦИКЛЕ ИЛИ ОБРАБОТЧИКЕ СОБЫТИЙ:
void OnMouseClick(float mouseX, float mouseY) 
{
    // 1. Генерируем луч
    BuslikDrev.Physics.FastRay ray = BuslikDrev.Physics.ObjectPicker.GetMouseRay(
        mouseX, mouseY, WindowWidth, WindowHeight, ProjectionMatrix, ViewMatrix
    );

    // 2. Ищем ближайший объект среди списка Entities
    MyEntity selected = BuslikDrev.Physics.ObjectPicker.FindClosestObject(
        ray, 
        allEntities, 
        entity => entity.GetWorldAABB()
    );

    if (selected != null) {
        System.Console.WriteLine("Объект выбран!");
    }
} */

namespace BuslikDrev.Physics {
    // 2. Структура AABB (Axis-Aligned Bounding Box)
    public struct AABB {
        public OpenTK.Vector3 Min;
        public OpenTK.Vector3 Max;

        public AABB(OpenTK.Vector3 min, OpenTK.Vector3 max) {
            this.Min = min;
            this.Max = max;
        }

        /// <summary>
        /// Создает мировой AABB из локального, учитывая трансформацию (поворот, масштаб, позицию)
        /// </summary>
        public static AABB Transform(AABB localBox, OpenTK.Matrix4 modelMatrix) {
            // Извлекаем позицию из матрицы (колонна M41, M42, M43)
            OpenTK.Vector3 min = modelMatrix.ExtractTranslation();
            OpenTK.Vector3 max = min;

            // Проходим по осям X, Y, Z и вычисляем вклад каждой оси в итоговый бокс
            // Это эффективный алгоритм Джима Арво (Jim Arvo)
            
            // Ось X
            UpdateAxis(modelMatrix.M11, modelMatrix.M12, modelMatrix.M13, localBox.Min.X, localBox.Max.X, ref min, ref max);
            // Ось Y
            UpdateAxis(modelMatrix.M21, modelMatrix.M22, modelMatrix.M23, localBox.Min.Y, localBox.Max.Y, ref min, ref max);
            // Ось Z
            UpdateAxis(modelMatrix.M31, modelMatrix.M32, modelMatrix.M33, localBox.Min.Z, localBox.Max.Z, ref min, ref max);

            return new AABB(min, max);
        }

        private static void UpdateAxis(float m1, float m2, float m3, float localMin, float localMax, ref OpenTK.Vector3 worldMin, ref OpenTK.Vector3 worldMax) {
            // Считаем вклад компонента X матрицы в новые границы
            float a = m1 * localMin;
            float b = m1 * localMax;
            worldMin.X += System.Math.Min(a, b);
            worldMax.X += System.Math.Max(a, b);

            // Считаем вклад компонента Y
            a = m2 * localMin;
            b = m2 * localMax;
            worldMin.Y += System.Math.Min(a, b);
            worldMax.Y += System.Math.Max(a, b);

            // Считаем вклад компонента Z
            a = m3 * localMin;
            b = m3 * localMax;
            worldMin.Z += System.Math.Min(a, b);
            worldMax.Z += System.Math.Max(a, b);
        }

        public bool Intersect(BuslikDrev.Physics.FastRay ray, out float distance) {
            float t1 = (this.Min.X - ray.Origin.X) * ray.InvDir.X;
            float t2 = (this.Max.X - ray.Origin.X) * ray.InvDir.X;
            float t3 = (this.Min.Y - ray.Origin.Y) * ray.InvDir.Y;
            float t4 = (this.Max.Y - ray.Origin.Y) * ray.InvDir.Y;
            float t5 = (this.Min.Z - ray.Origin.Z) * ray.InvDir.Z;
            float t6 = (this.Max.Z - ray.Origin.Z) * ray.InvDir.Z;

            float tmin = (float)System.Math.Max(System.Math.Max(System.Math.Min((double)t1, (double)t2), System.Math.Min((double)t3, (double)t4)), System.Math.Min((double)t5, (double)t6));
            float tmax = (float)System.Math.Min(System.Math.Min(System.Math.Max((double)t1, (double)t2), System.Math.Max((double)t3, (double)t4)), System.Math.Max((double)t5, (double)t6));

            distance = tmin;

            // Если tmax < 0, объект находится позади луча. 
            // Если tmin > tmax, луч проходит мимо.
            return tmax >= (float)System.Math.Max(0.0, (double)tmin);
        }
    }

    // 5. Структура плоскости для Frustum Culling
    public struct Plane {
        public OpenTK.Vector3 Normal;
        public float Distance;

        public Plane(float a, float b, float c, float d) {
            float length = (float)System.Math.Sqrt((double)(a * a + b * b + c * c));
            this.Normal = new OpenTK.Vector3(a / length, b / length, c / length);
            this.Distance = d / length;
        }

        // Расстояние от точки до плоскости
        public float DotCoordinate(OpenTK.Vector3 point) {
            return OpenTK.Vector3.Dot(this.Normal, point) + this.Distance;
        }
    }

    // 6. Структура пирамиды видимости (Frustum)
    public struct Frustum {
        private BuslikDrev.Physics.Plane[] planes;

        // Создание пирамиды из матрицы View-Projection
        public static BuslikDrev.Physics.Frustum FromMatrix(OpenTK.Matrix4 matrix) {
            BuslikDrev.Physics.Frustum f = new BuslikDrev.Physics.Frustum();
            f.planes = new BuslikDrev.Physics.Plane[6];

            // Правая плоскость
            f.planes[0] = new BuslikDrev.Physics.Plane(matrix.M14 - matrix.M11, matrix.M24 - matrix.M21, matrix.M34 - matrix.M31, matrix.M44 - matrix.M41);
            // Левая плоскость
            f.planes[1] = new BuslikDrev.Physics.Plane(matrix.M14 + matrix.M11, matrix.M24 + matrix.M21, matrix.M34 + matrix.M31, matrix.M44 + matrix.M41);
            // Нижняя плоскость
            f.planes[2] = new BuslikDrev.Physics.Plane(matrix.M14 + matrix.M12, matrix.M24 + matrix.M22, matrix.M34 + matrix.M32, matrix.M44 + matrix.M42);
            // Верхняя плоскость
            f.planes[3] = new BuslikDrev.Physics.Plane(matrix.M14 - matrix.M12, matrix.M24 - matrix.M22, matrix.M34 - matrix.M32, matrix.M44 - matrix.M42);
            // Дальняя плоскость
            f.planes[4] = new BuslikDrev.Physics.Plane(matrix.M14 - matrix.M13, matrix.M24 - matrix.M23, matrix.M34 - matrix.M33, matrix.M44 - matrix.M43);
            // Ближняя плоскость
            f.planes[5] = new BuslikDrev.Physics.Plane(matrix.M14 + matrix.M13, matrix.M24 + matrix.M23, matrix.M34 + matrix.M33, matrix.M44 + matrix.M43);

            return f;
        }

        // Проверка: находится ли AABB внутри или пересекает пирамиду
        public bool Contains(BuslikDrev.Physics.AABB box) {
            for (int i = 0; i < 6; i++) {
                OpenTK.Vector3 p = box.Min;
                if (this.planes[i].Normal.X >= 0) p.X = box.Max.X;
                if (this.planes[i].Normal.Y >= 0) p.Y = box.Max.Y;
                if (this.planes[i].Normal.Z >= 0) p.Z = box.Max.Z;

                if (this.planes[i].DotCoordinate(p) < 0) {
                    return false; // Объект полностью за пределами одной из плоскостей
                }
            }

            return true;
        }
    }
}


/* // 1. Получаем матрицу View-Projection (обязательно перемноженную)
OpenTK.Matrix4 viewProjection = ViewMatrix * ProjectionMatrix;

// 2. Создаем Frustum (можно делать это один раз за кадр)
BuslikDrev.Physics.Frustum cameraFrustum = BuslikDrev.Physics.Frustum.FromMatrix(viewProjection);

// 3. Проходим по списку объектов перед отрисовкой
foreach (var obj in mySceneObjects) {
    BuslikDrev.Physics.AABB worldBox = obj.GetWorldAABB();

    if (cameraFrustum.Contains(worldBox)) {
        // ОБЪЕКТ ВИДЕН — РИСУЕМ
        obj.Draw();
    } else {
        // ОБЪЕКТ ВНЕ КАДРА — ПРОПУСКАЕМ
        // Это экономит ресурсы CPU на вызовах Draw и GPU на расчетах
    }
} */