using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class HeroData
{
    public PowerUpID currentPowerUpID;
    public int powerUpAmount;
    public int coinsAmount;
    public int health;
    public float lastPositionX;
    public float lastPositionY;
    public float lastPositionZ;
    //public float[] lastPosition;

    /*public HeroData()
    {
        lastPosition = new float[3];
        lastPosition[0] = 0;
        lastPosition[1] = 0;
        lastPosition[2] = 0;
    }*/

    /*public HeroData(HeroController player)
    {
        powerUpAmount = player.PowerUpAmount;
        health = player.Health;

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }*/
}
