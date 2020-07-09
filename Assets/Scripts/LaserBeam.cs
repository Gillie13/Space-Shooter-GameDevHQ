using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [SerializeField]
    private bool _spawnLaser = false;

    public SpriteRenderer laserBeamLeft, laserBeamRight, laserBeamCentre;
    public SpriteRenderer laserSpawnLeft, laserSpawnRight, laserSpawnCentre;


    public void FireLaser()
    {
        _spawnLaser = true;
        StartCoroutine(FireLaserBeam());

    }

    IEnumerator FireLaserBeam()
    {
        while (_spawnLaser == true)
        {
            laserSpawnLeft.gameObject.SetActive(true);
            laserBeamLeft.gameObject.SetActive(true);
            laserSpawnRight.gameObject.SetActive(true);
            laserBeamRight.gameObject.SetActive(true);
            laserSpawnCentre.gameObject.SetActive(true);
            laserBeamCentre.gameObject.SetActive(true);
            yield return new WaitForSeconds(4f);
            laserSpawnLeft.gameObject.SetActive(false);
            laserBeamLeft.gameObject.SetActive(false);
            laserSpawnRight.gameObject.SetActive(false);
            laserBeamRight.gameObject.SetActive(false);
            laserSpawnCentre.gameObject.SetActive(false);
            laserBeamCentre.gameObject.SetActive(false);
            _spawnLaser = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
        }
    }
}
