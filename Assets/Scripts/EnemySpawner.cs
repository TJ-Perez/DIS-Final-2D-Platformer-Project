using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{

    public int enemyCount;

    public int maxEnemies;

    public float spawnTimer;
    public float timer;

    public Tilemap tilemap;

    public GameObject skeleton;

    List<Vector3> eligibleTiles;

    [SerializeField] private GameControler gameControler;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        timer = spawnTimer;


    }

    void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

            if(enemyCount < maxEnemies)
            {
                SpawnEnemy();

            }
            timer = spawnTimer;
        }

        if(gameControler.timePlayed > 10)
        {
            spawnTimer = 4 - (Mathf.Floor(gameControler.timePlayed / 10) * .1f);
        }
    }

    //for enemy spawning, the tilemap is looked at through the x and y coordinates.
    //Enemies will be randomnly spawned on the highest y tile for each x coordinate that a tile exists
    void SpawnEnemy()
    {
        eligibleTiles = new List<Vector3>();

        for (int n = tilemap.cellBounds.xMin; n < tilemap.cellBounds.xMax; n++)
        {
            int highestTile = -9999;

            for (int p = tilemap.cellBounds.yMin; p < tilemap.cellBounds.yMax; p++)
            {

                Vector3Int localPlace = (new Vector3Int(n, p, (int)tilemap.transform.position.y));
                Vector3 place = tilemap.CellToWorld(localPlace);
                if (tilemap.HasTile(localPlace) && p > highestTile)
                {

                    highestTile = p;
                }

            }
            if (highestTile != -9999)
            {
                Vector3Int highTile = new Vector3Int(n, highestTile, 0);

                eligibleTiles.Add(highTile);
            }

        }

        Vector3 finalTile = eligibleTiles[Random.Range(0, eligibleTiles.Count)];

        finalTile.y += 2f;
        finalTile.x += .5f;

        Instantiate(skeleton,finalTile , skeleton.transform.rotation);


    }
}
