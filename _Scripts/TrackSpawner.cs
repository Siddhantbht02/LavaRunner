using System.Collections.Generic;
using UnityEngine;

public class TrackSpawner : MonoBehaviour
{
    public GameObject[] trackPrefabs;
    public GameObject player;
    public float trackLength = 50f;
    public int numberOfTracks = 5; // How many tracks to keep active at once

    private List<GameObject> activeTracks = new List<GameObject>();
    private int nextTrackIndex = 0;

    void Start()
    {
        // Spawn initial tracks
        for (int i = 0; i < numberOfTracks; i++)
        {
            SpawnTrack(i);
        }
    }

    void Update()
    {
        // Check if we need to spawn a new track and delete an old one
        if (player.transform.position.z > (nextTrackIndex - numberOfTracks + 1) * trackLength)
        {
            SpawnTrack(nextTrackIndex);
            DeleteOldestTrack();
        }
    }

    private void SpawnTrack(int index)
    {
        // Choose a random track prefab
        int prefabIndex = Random.Range(0, trackPrefabs.Length);

        // Calculate the spawn position using an integer multiple to avoid floating-point errors
        Vector3 spawnPosition = new Vector3(0, 0, index * trackLength);

        // Instantiate the track
        GameObject newTrack = Instantiate(trackPrefabs[prefabIndex], spawnPosition, Quaternion.identity);
        activeTracks.Add(newTrack);
        nextTrackIndex++;
    }

    private void DeleteOldestTrack()
    {
        if (activeTracks.Count > numberOfTracks)
        {
            Destroy(activeTracks[0]);
            activeTracks.RemoveAt(0);
        }
    }
}

