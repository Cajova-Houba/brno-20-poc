﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class ObstacleSpawner : MonoBehaviour
    {
        /// <summary>
        /// How many canals to spawn.
        /// </summary>
        public int canalCount;

        /// <summary>
        /// How many trash cans to spawn.
        /// </summary>
        public int trashCanCount;

        /// <summary>
        /// Canal prefab.
        /// </summary>
        public GameObject canal;

        /// <summary>
        /// Trash can prefab.
        /// </summary>
        public GameObject trashCan;

        /// <summary>
        /// Tag used to collect all obstacle spawnpoints.
        /// </summary>
        public string obstacleSpawnpointTag;

        /// <summary>
        /// All spawn points that can be used for obstacles.
        /// </summary>
        protected GameObject[] spawnPoints;

        protected List<Transform> availableSpawnPoints;

        private System.Random random;

        void Start()
        {
            spawnPoints = GameObject.FindGameObjectsWithTag(obstacleSpawnpointTag);
            availableSpawnPoints = new List<Transform>();
            foreach(GameObject sp in spawnPoints)
            {
                availableSpawnPoints.Add(sp.transform);
            }
            random = new System.Random();

            SpawnTrashCans();
            SpawnCanals();
        }

        private void SpawnCanals()
        {
            for (int i = 0; i < canalCount; i++)
            {
                SpawnObstacleRandomly(canal);
            }
        }

        /// <summary>
        /// Spawns one canal on random position.
        /// </summary>
        private void SpawnTrashCans()
        {
            for (int i = 0; i < trashCanCount; i++)
            {
                SpawnObstacleRandomly(trashCan);
            }
        }

        /// <summary>
        /// Spawns given obstacle on random spawn point.
        /// </summary>
        /// <param name="obstacle"></param>
        private void SpawnObstacleRandomly(GameObject obstacle) {
            Transform spawnPoint = PickRandomSpawnPoint();
            Debug.Log("Spawning "+obstacle.name+": " + spawnPoint.transform.position + " ; " + spawnPoint.rotation);
            Instantiate(obstacle, spawnPoint.position, spawnPoint.rotation);
        }

        private Transform PickRandomSpawnPoint()
        {
            int index = random.Next(availableSpawnPoints.Count);
            Transform t = availableSpawnPoints[index];
            availableSpawnPoints.RemoveAt(index);
            return t;
        }
    }
}
