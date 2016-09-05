using UnityEngine;
using System.Collections;
using System;

public class TerrainDeformer : MonoBehaviour, ITerrainDeformer
{
    [SerializeField] float m_radiusWorldUnits = 5f;
    [SerializeField] float m_heightWorldUnits = 3f;
    [SerializeField] AnimationCurve m_profile;
    [SerializeField] float m_deformationDuration = 0f;

    private bool m_deformed = false;
    private Rigidbody m_rigidbody;
    private Terrain m_terrain;
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

        m_terrain = terrain;

        int heightMapWidth = terrain.terrainData.heightmapWidth;
        int heightMapHeight = terrain.terrainData.heightmapHeight;

        //print(string.Format("height: {0}, width: {1}", heightMapWidth, heightMapHeight));

        int radius = Mathf.CeilToInt(m_radiusWorldUnits / terrain.terrainData.heightmapScale.x);
        float height = m_heightWorldUnits / terrain.terrainData.heightmapScale.y;

        // get the normalized position of this game object relative to the terrain
        Vector3 coord = (position - terrain.gameObject.transform.position);
        coord.x = coord.x / terrain.terrainData.size.x;
        coord.y = coord.y / terrain.terrainData.size.y;
        coord.z = coord.z / terrain.terrainData.size.z;

        // get the position of the terrain heightmap where the collision happened
        float xPos = coord.x * heightMapWidth;
        float yPos = coord.z * heightMapHeight;

        //print("xPos: " + xPos + ", yPos: " + yPos);

        int posXInTerrain = (int) xPos;
        int posYInTerrain = (int) yPos;

        int xMin = Math.Max(0, posXInTerrain - radius);
        int xMax = Math.Min(heightMapWidth , posXInTerrain + radius);
        int yMin = Math.Max(0, posYInTerrain - radius);
        int yMax = Math.Min(heightMapHeight, posYInTerrain + radius);

        //print(string.Format("xMin: {0}, xMax: {1}, yMin: {2}, yMax: {3}", xMin, xMax, yMin, yMax));

        int xMinToCentre = posXInTerrain - xMin;
        int yMinToCentre = posYInTerrain - yMin;

        m_xSize = xMax - xMin;
        m_ySize = yMax - yMin;

        m_xBase = xMin;
        m_yBase = yMin;

        float[,] sampleHeights = new float[m_xSize, m_ySize];

        for (int i = 0; i < m_xSize; i++)
        {
            float sampleXPos = i - xMinToCentre + posXInTerrain;

            for (int j = 0; j < m_ySize; j++)
            {
                float sampleYPos = j - yMinToCentre + posYInTerrain;
                float xDiff = sampleXPos - xPos;
                float yDiff = sampleYPos - yPos;
                float dist = Mathf.Sqrt(xDiff * xDiff + yDiff * yDiff) * terrain.terrainData.heightmapScale.x / m_radiusWorldUnits;
                sampleHeights[i, j] = height * m_profile.Evaluate(dist);
            }
        }

        if (m_deformationDuration < 0.01f)
        {
            var heights = m_terrain.terrainData.GetHeights(m_xBase, m_yBase, m_xSize, m_ySize);
            terrain.terrainData.SetHeights(m_xBase, m_yBase, AddHeights(heights, sampleHeights, 1f));

            Destroy(gameObject);
            //m_rigidbody.isKinematic = true;
        }
        else
        {
            StartCoroutine(AddHeightsIncrementally(sampleHeights));
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


    private IEnumerator AddHeightsIncrementally(float[,] sampleHeights)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;

        float startTime = Time.time;
        float duration = 0;

        while (duration <= m_deformationDuration)
        {
            duration = Time.time - startTime;
            float frac = Time.deltaTime / m_deformationDuration;

            var heights = m_terrain.terrainData.GetHeights(m_xBase, m_yBase, m_xSize, m_ySize);
            m_terrain.terrainData.SetHeights(m_xBase, m_yBase, AddHeights(heights, sampleHeights, frac));

            yield return null;
        }

        Destroy(gameObject);
    }
}
