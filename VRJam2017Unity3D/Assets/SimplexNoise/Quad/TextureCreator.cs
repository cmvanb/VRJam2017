using UnityEngine;

namespace CatlikeCoding.SimplexNoise
{
    public class TextureCreator : MonoBehaviour
    {
        [Range(2, 512)]
        public int resolution = 256;

        public float frequency = 1f;

        [Range(1, 8)]
        public int octaves = 1;

        [Range(1f, 4f)]
        public float lacunarity = 2f;

        [Range(0f, 1f)]
        public float persistence = 0.5f;

        [Range(1, 3)]
        public int dimensions = 3;

        public NoiseMethodType type;

        public Gradient coloring;

        private Texture2D texture;

        private void OnEnable()
        {
            if (texture == null)
            {
                texture = new Texture2D(resolution, resolution, TextureFormat.RGB24, true);
                texture.name = "Procedural Texture";
                texture.wrapMode = TextureWrapMode.Clamp;
                texture.filterMode = FilterMode.Trilinear;
                texture.anisoLevel = 9;
                GetComponent<MeshRenderer>().material.mainTexture = texture;
            }

            UpdateTexture();
        }

        private void Update()
        {
            if (transform.hasChanged)
            {
                transform.hasChanged = false;
                UpdateTexture();
            }
        }

        public void UpdateTexture()
        {
            FillTextureWithNoise(
                this.texture,
                this.resolution,
                this.frequency,
                this.octaves,
                this.lacunarity,
                this.persistence,
                this.dimensions,
                this.type,
                this.coloring);
        }

        public static void FillTextureWithNoise(
            Texture2D texture,
            int resolution = 256, 
            float frequency = 1f, 
            int octaves = 1, 
            float lacunarity = 2f, 
            float persistence = 0.5f,
            int dimensions = 3,
            NoiseMethodType type = NoiseMethodType.Perlin,
            Gradient coloring = null)
        {
            if (texture.width != resolution)
            {
                texture.Resize(resolution, resolution);
            }

            Vector3 point00 = new Vector3(-0.5f, -0.5f);
            Vector3 point10 = new Vector3(0.5f, -0.5f);
            Vector3 point01 = new Vector3(-0.5f, 0.5f);
            Vector3 point11 = new Vector3(0.5f, 0.5f);

            // NOTE: Removed conversion from local to world space, as it appears to be unnecessary. -Casper 2017-09-14
            /*
            Vector3 point00 = transform.TransformPoint(new Vector3(-0.5f, -0.5f));
            Vector3 point10 = transform.TransformPoint(new Vector3(0.5f, -0.5f));
            Vector3 point01 = transform.TransformPoint(new Vector3(-0.5f, 0.5f));
            Vector3 point11 = transform.TransformPoint(new Vector3(0.5f, 0.5f));
            */

            NoiseMethod method = Noise.methods[(int)type][dimensions - 1];
            float stepSize = 1f / resolution;

            for (int y = 0; y < resolution; y++)
            {
                Vector3 point0 = Vector3.Lerp(point00, point01, (y + 0.5f) * stepSize);
                Vector3 point1 = Vector3.Lerp(point10, point11, (y + 0.5f) * stepSize);

                for (int x = 0; x < resolution; x++)
                {
                    Vector3 point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSize);
                    float sample = Noise.Sum(method, point, frequency, octaves, lacunarity, persistence).value;
                    sample = sample * 0.5f + 0.5f;
                    texture.SetPixel(x, y, coloring.Evaluate(sample));
                }
            }

            texture.Apply();
        }

        public static Texture2D Create(
            int resolution = 256, 
            float frequency = 6f, 
            int octaves = 6, 
            float lacunarity = 2f, 
            float persistence = 0.5f,
            int dimensions = 2,
            NoiseMethodType type = NoiseMethodType.Perlin,
            Gradient coloring = null)
        {
            // This is the default gradient if the user doesn't provide one.
            if (coloring == null)
            {
                // From black to white.
                GradientColorKey[] gradientColorKeys = new GradientColorKey[2];
                gradientColorKeys[0].color = Color.black;
                gradientColorKeys[0].time = 0f;
                gradientColorKeys[1].color = Color.white;
                gradientColorKeys[1].time = 1f;

                // Full alpha.
                GradientAlphaKey[] gradientAlphaKeys = new GradientAlphaKey[2];
                gradientAlphaKeys[0].alpha = 1f;
                gradientAlphaKeys[0].time = 0f;
                gradientAlphaKeys[1].alpha = 1f;
                gradientAlphaKeys[1].time = 1f;

                coloring = new Gradient();
                coloring.SetKeys(gradientColorKeys, gradientAlphaKeys);
            }

            Texture2D result = new Texture2D(resolution, resolution, TextureFormat.RGB24, true);
            result.name = "Procedural Texture";
            result.wrapMode = TextureWrapMode.Clamp;
            result.filterMode = FilterMode.Trilinear;
            result.anisoLevel = 9;

            FillTextureWithNoise(
                result,
                resolution,
                frequency,
                octaves,
                lacunarity,
                persistence,
                dimensions,
                type,
                coloring);

            return result;
        }
    }
}
