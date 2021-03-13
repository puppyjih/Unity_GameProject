using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioClip upgradeSound0;
    private AudioClip upgradeSound1;
    private AudioClip upgradeSound2;
    private AudioClip upgradeSound3;

    private AudioClip healthUpSound0;
    //private AudioClip healthUpSound1;

    private AudioClip laserSound0;
    private AudioClip laserSound1;
    private AudioClip laserSound2;
    //private AudioClip laserSound3;
    //private AudioClip laserSound4;
    //private AudioClip laserSound5;
    //private AudioClip laserSound6;
    //private AudioClip laserSound7;

    private AudioClip obstacleSound0;
    //private AudioClip obstacleSound1;

    private AudioSource audioSource;

    private string laserClip;
    private int laserClipNum;

    private void Start()
    {
        upgradeSound0 = Resources.Load<AudioClip>("Temp/Pop_0");
        upgradeSound1 = Resources.Load<AudioClip>("Temp/Pop_1");
        upgradeSound2 = Resources.Load<AudioClip>("Temp/Pop_2");
        upgradeSound3 = Resources.Load<AudioClip>("Temp/Upgrade_Stack");

        healthUpSound0 = Resources.Load<AudioClip>("Temp/GrowUp_01");
        //healthUpSound0 = Resources.Load<AudioClip>("HealthUp/HealthUp_0");
        //healthUpSound1 = Resources.Load<AudioClip>("HealthUp/HealthUp_1");

        laserSound0 = Resources.Load<AudioClip>("Temp/Button3");
        //laserSound1 = Resources.Load<AudioClip>("Temp/Button3");
        //laserSound2 = Resources.Load<AudioClip>("Temp/Button3");
        //laserSound2 = Resources.Load<AudioClip>("Laser/Laser_Clean_1");
        //laserSound3 = Resources.Load<AudioClip>("Laser/Laser_Cute_0");
        //laserSound4 = Resources.Load<AudioClip>("Laser/Laser_Long_0");
        //laserSound5 = Resources.Load<AudioClip>("Laser/Laser_Long_1");
        //laserSound6 = Resources.Load<AudioClip>("Laser/Laser_Long_2");
        //laserSound7 = Resources.Load<AudioClip>("Laser/Laser_Strong_0");

        obstacleSound0 = Resources.Load<AudioClip>("Temp/Boom_00");
        //obstacleSound0 = Resources.Load<AudioClip>("Obstacle/Obstacle_Short_0");
        //obstacleSound1 = Resources.Load<AudioClip>("Obstacle/Obstacle_Low_0");

        audioSource = GetComponent<AudioSource>();

        //laserClip = "Temp/l_";
        //laserClipNum = 0;
    }

    public void PlaySound(string clip)
    {
        switch(clip)
        {
            case "upgrade0":
                audioSource.PlayOneShot(upgradeSound0);
                break;
            case "upgrade1":
                audioSource.PlayOneShot(upgradeSound1);
                break;
            case "upgrade2":
                audioSource.PlayOneShot(upgradeSound2);
                break;
            case "upgrade3":
                audioSource.PlayOneShot(upgradeSound3);
                break;
            case "healthUp0":
                audioSource.PlayOneShot(healthUpSound0);
                break;
            //case "healthUp1":
            //    audioSource.PlayOneShot(healthUpSound1);
            //    break;
            case "laser0":
                audioSource.PlayOneShot(laserSound0, 0.5f);
                //AudioClip audioClip = Resources.Load<AudioClip>(laserClip + laserClipNum.ToString());
                //audioSource.PlayOneShot(audioClip);
                //laserClipNum = (laserClipNum + 1) % 10;
                break;
            //case "laser1":
            //    audioSource.PlayOneShot(laserSound1, 0.5f);
            //    break;
            //case "laser2":
            //    audioSource.PlayOneShot(laserSound2, 0.5f);
            //    break;
            //case "laser3":
            //    audioSource.PlayOneShot(laserSound3);
            //    break;
            //case "laser4":
            //    audioSource.PlayOneShot(laserSound4);
            //    break;
            //case "laser5":
            //    audioSource.PlayOneShot(laserSound5);
            //    break;
            //case "laser6":
            //    audioSource.PlayOneShot(laserSound6);
            //    break;
            //case "laser7":
            //    audioSource.PlayOneShot(laserSound7);
            //    break;
            case "obstacle0":
                audioSource.PlayOneShot(obstacleSound0);
                break;
            //case "obstacle1":
            //    audioSource.PlayOneShot(obstacleSound1);
            //    break;
        }
    }
}
