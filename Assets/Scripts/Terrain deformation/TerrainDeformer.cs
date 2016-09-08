using UnityEngine;
using System.Collections;

public class TerrainDeformer : MonoBehaviour, ITerrainDeformer
{
    [Header("Deformation options")]
    [SerializeField] float m_radiusWorldUnits = 5f;
    [SerializeField] float m_heightWorldUnits = 3f;
    [SerializeField] AnimationCurve m_profile;
    [SerializeField] float m_deformationDuration = 0f;

    [Header("Bumpiness options")]
    [SerializeField] float m_bumpScale = 5f;
    [SerializeField] float m_bumpHeightWorldUnits = 0.3f;
    [SerializeField] AnimationCurve m_bumpBlend;

    [Header("Scar options")]
    [SerializeField] int m_scarTextureIndex = 0;
    [SerializeField] AnimationCurve m_scarBlend;

    private bool m_deformed = false;
    private Rigidbody m_rigidbody;
    private TerrainData m_terrainData;
    private int m_xBase;
    private int m_yBase;
    private int m_xSize;
    private int m_ySize;


    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }


    public void DeformTerrain(Terrain terrain, Vector3 position)
    {
        if (m_deformed)
            return;

        m_deformed = true;

        m_terrainData = terrain.terrainData;

        int heightMapWidth = m_terrainData.heightmapWidth;
        int heightMapHeight = m_terrainData.heightmapHeight;

        //print(string.Format("height: {0}, width: {1}", heightMapWidth, heightMapHeight));

        int radius = Mathf.CeilToInt(m_radiusWorldUnits / m_terrainData.heightmapScale.x);
        float height = m_heightWorldUnits / m_terrainData.heightmapScale.y;
        float bumpHeight = m_bumpHeightWorldUnits / m_terrainData.heightmapScale.y;

        // get the normalized position of this game object relative to the terrain
        Vector3 coord = (position - terrain.gameObject.transform.position);
        coord.x = coord.x / m_terrainData.size.x;
        coord.y = coord.y / m_terrainData.size.y;
        coord.z = coord.z / m_terrainData.size.z;

        // get the position of the terrain heightmap where the collision happened
        float xPos = coord.x * heightMapWidth;
        float yPos = coord.z * heightMapHeight;

        //print("xPos: " + xPos + ", yPos: " + yPos);

        int posXInTerrain = (int) xPos;
        int posYInTerrain = (int) yPos;

        int xMin = Mathf.Max(0, posXInTerrain - radius);
        int xMax = Mathf.Min(heightMapWidth , posXInTerrain + radius);
        int yMin = Mathf.Max(0, posYInTerrain - radius);
        int yMax = Mathf.Min(heightMapHeight, posYInTerrain + radius);

        //print(string.Format("xMin: {0}, xMax: {1}, yMin: {2}, yMax: {3}", xMin, xMax, yMin, yMax));

        int xMinToCentre = posXInTerrain - xMin;
        int yMinToCentre = posYInTerrain - yMin;

        m_xSize = xMax - xMin;
        m_ySize = yMax - yMin;

        m_xBase = xMin;
        m_yBase = yMin;

        float[,] sampleHeights = new float[m_xSize, m_ySize];
        float[,] sampleScarBlend = new float[m_xSize - 1, m_ySize - 1];

        float offsetX = Random.Range(0f, 10000f);
        float offsetY = Random.Range(0f, 10000f);

        for (int i = 0; i < m_xSize; i++)
        {
            float sampleXPos = i - xMinToCentre + posXInTerrain;

            for (int j = 0; j < m_ySize; j++)
            {
                float sampleYPos = j - yMinToCentre + posYInTerrain;
                float xDiff = sampleXPos - xPos;
                float yDiff = sampleYPos - yPos;
                float dist = Mathf.Sqrt(xDiff * xDiff + yDiff * yDiff) * m_terrainData.heightmapScale.x / m_radiusWorldUnits;

                float bump = Mathf.PerlinNoise(offsetX + i / m_bumpScale, offsetY + j / m_bumpScale) * 2f - 1f;
                //print(string.Format("{0}, {1}, {2}", i * m_bumpinessScale, j * m_bumpinessScale, bump));
                bump *= bumpHeight * m_bumpBlend.Evaluate(dist);

                sampleHeights[i, j] = height * m_profile.Evaluate(dist) + bump;

                if (i < m_xSize - 1 && j < m_ySize - 1)
                    sampleScarBlend[i, j] = m_scarBlend.Evaluate(dist);
            }
        }

        if (m_deformationDuration < 0.01f)
        {
            var heights = m_terrainData.GetHeights(m_xBase, m_yBase, m_xSize, m_ySize);
            m_terrainData.SetHeights(m_xBase, m_yBase, AddHeights(heights, sampleHeights, 1f));

            var maps = m_terrainData.GetAlphamaps(m_xBase, m_yBase, m_xSize - 1, m_ySize - 1);
            m_terrainData.SetAlphamaps(m_xBase, m_yBase, AddScar(maps, sampleScarBlend, 1f));

            Destroy(gameObject);
            //m_rigidbody.isKinematic = true;
        }
        else
        {
            StartCoroutine(DeformIncrementally(sampleHeights, sampleScarBlend));
        }
    }


    private float[,] AddHeights(float[,] heights, float[,] sampleHeights, float frac)
    {
        //print(string.Format("heights: [{0},{1}], sampleHeights: [{2},{3}]", 
        //    heights.GetLength(0), heights.GetLength(1), sampleHeights.GetLength(0), sampleHeights.GetLength(1)));
        //print(string.Format("xSize: {0}, ySize: {1}", m_xSize, m_ySize));

        var sumHeights = new float[m_ySize, m_xSize];

        for (int i = 0; i < m_xSize; i++)
        {
            for (int j = 0; j < m_ySize; j++)
            {
                sumHeights[j, i] = heights[j, i] + frac * sampleHeights[i, j];
            }
        }

        return sumHeights;
    }


    private float[,,] AddScar(float[,,] maps, float[,] sampleScarBlend, float frac)
    {
        int textures = maps.GetLength(2);

        for (int i = 0; i < m_xSize - 1; i++)
        {
            for (int j = 0; j < m_ySize - 1; j++)
            {
                float blend = frac * sampleScarBlend[i, j];
    
                for (int k = 0; k < textures; k++)
                {
                    float existing = maps[j, i, k];
                    float existingScar = maps[j, i, m_scarTextureIndex];

                    float newValue = k == m_scarTextureIndex
                        ? existing + blend
                        : existingScar < 1f
                            ? existing * (1f - (blend / (1f - existingScar)))
                            : 0f;

                    maps[j, i, k] = newValue;
                }
            }
        }

        return maps;
    }


    private IEnumerator DeformIncrementally(float[,] sampleHeights, float[,] sampleScarBlend)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;

        float startTime = Time.time;
        float duration = 0;
        float totalFrac = 0;

        while (duration <= m_deformationDuration)
        {
            duration = Time.time - startTime;
            float frac = Mathf.Min(1f - totalFrac, Time.deltaTime / m_deformationDuration);
            totalFrac += frac;

            var heights = m_terrainData.GetHeights(m_xBase, m_yBase, m_xSize, m_ySize);
            m_terrainData.SetHeights(m_xBase, m_yBase, AddHeights(heights, sampleHeights, frac));

            var maps = m_terrainData.GetAlphamaps(m_xBase, m_yBase, m_xSize - 1, m_ySize - 1);
            m_terrainData.SetAlphamaps(m_xBase, m_yBase, AddScar(maps, sampleScarBlend, frac));

            yield return null;
        }

        Destroy(gameObject);
    }
}
