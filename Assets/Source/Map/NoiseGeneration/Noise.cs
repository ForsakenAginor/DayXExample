using UnityEngine;

namespace ProceduralNoiseProject
{
    public abstract class Noise
    {
        public float Frequency { get; set; }

        public float Amplitude { get; set; }

        public Vector3 Offset { get; set; }

		public abstract float Sample1D(float x);

		public abstract float Sample2D(float x, float y);

		public abstract float Sample3D(float x, float y, float z);

        public abstract void UpdateSeed(int seed);
    }
}
