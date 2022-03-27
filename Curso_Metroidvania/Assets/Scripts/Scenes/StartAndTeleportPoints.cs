using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contiene los diferentes puntos de inicio y teletransporte de una escena a otra,
/// esto para mejor comodidad al ponerlos en el inspector de Unity
/// </summary>
public enum StartAndTeleportPointsID
{
    nothing,
    gameStart,

    // Level1_1
    level1_1FromLevel1_2,
    level1_1FromLevel1_4,
    level1_1FromLevel1_5,

    // Level1_2
    level1_2FromLevel1_1,
    level1_2FromLevel1_3,
    level1_2FromCheck1_1,

    // Level1_3
    level1_3FromLevel1_2,
    level1_3FromCheck1_2,
    level1_3FromCheck1_5,

    // Level1_4
    level1_4FromLevel1_1,

    // Level1_5
    level1_5FromLevel1_1,
    level1_5FromLevel1_3,

    // Checks
    check1_1FromLevel1_2,
    check1_2FromLevel1_3
}
