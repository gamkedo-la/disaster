using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Terrain))]
public class TerrainDeformationManager : MonoBehaviour
{
    [SerializeField] float m_erosionRate = 1f;
    [SerializeField] int m_errosionChunkDivider = 8;

    private Terrain m_terrain;
    private float[,] m_originalHeights;
    private int m_width;
    private int m_height;


    void Awake()
    {
        m_terrain = GetComponent<Terrain>();

        m_width = m_terrain.terrainData.heightmapWidth;
        m_height = m_terrain.terrainData.heightmapHeight;
    }


    void Start()
    {
        m_originalHeights = m_terrain.terrainData.GetHeights(0, 0, m_width, m_height);

        StartCoroutine(Erode());
    }


    private IEnumerator Erode()
    {
        int chunkWidth = (m_width - 1) / m_errosionChunkDivider;
        int chunkHeight = (m_height - 1) / m_errosionChunkDivider;

        int jBlock = 0;
        int iBlock = 0;

        while (true)
        {
            int width = jBlock == m_errosionChunkDivider - 1 ? chunkWidth + 1 : chunkWidth;
            int height = iBlock == m_errosionChunkDivider - 1 ? chunkHeight + 1 : chunkHeight;

            int jStart = jBlock * chunkHeight;
            int iStart = iBlock * chunkWidth;

            var currentHeights = m_terrain.terrainData.GetHeights(jStart, iStart, width, height);

            var newHeights = new float[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    float currentHeight = currentHeights[i, j];
                    float originalHeight = m_originalHeights[i + iStart, j + jStart];

                    newHeights[i, j] = Mathf.Lerp(
                        currentHeight, 
                        originalHeight,
                        Time.deltaTime * m_erosionRate);
                }
            }

            m_terrain.terrainData.SetHeights(jStart, iStart, newHeights);

            iBlock++;
            iBlock = iBlock % m_errosionChunkDivider;

            if (iBlock == 0)
            {
                jBlock++;
                jBlock = jBlock % m_errosionChunkDivider;
            }

            yield return null;
        }
    }


    void OnCollisionEnter(Collision col)
    {
        var terrainDeformer = col.gameObject.GetComponent<ITerrainDeformer>();

        if (terrainDeformer != null)
        {
            terrainDeformer.DeformTerrain(m_terrain, col.contacts[0].point);
        }
    }


    void OnTriggerEnter(Collider other)
    {
        var terrainDeformer = other.gameObject.GetComponent<ITerrainDeformer>();

        if (terrainDeformer != null)
        {
            terrainDeformer.DeformTerrain(m_terrain, other.transform.position);
        }
    }


    private void OnDestroy()
    {
        m_terrain.terrainData.SetHeights(0, 0, m_originalHeights);
    }
}
