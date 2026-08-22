using UnityEngine;

namespace Planets
{
    public class Sun : CelestialBody
    {
        // It could have init function to create a MonoBehaviour Sphere planet named SunVisual

        public Sun(CelestialData data, Material material)
            : base(data, material)
        {
            Debug.Log("Sun Created");
        }
    }
}