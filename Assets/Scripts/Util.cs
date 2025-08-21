using UnityEngine;

public static class Util
{
    public static float CamGroundDist = 1.77f;

    public static string ReturnNodeSuffix(int number)
    {
        return (number / 10) switch
        {
            < 1 => "00" + number,
            < 10 => "0" + number,
            _ => number.ToString()
        };
    }

    public static Vector3 QuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 point = (uu * p0) + (2 * u * t * p1) + (tt * p2);
        return point;
    }

    /* EVENT NAME           PARAMS          DURATION
       ==========           ==========      ==========
       look                 Vector3         yes
       walk                 float           no
       stop                 nil             no
       hold                 nil             no
       open                 nil             no
       wait                 nil             yes
       jump                 float           yes
       duck                 nil             no
       rise                 nil             no        */
    public enum EventType
    {
        look,       //Makes the player look around.
        walk,       //Let's the player proceed to next node.
        stop,       //Stops the player in the current node.
        hold,       //Makes the player hold their fire.
        open,       //Allows the player to resume fire.
        wait,       //Delay's operations for a specified duration.
        jump,       //Jumps with a specified height and duration.
        duck,       //Make's the player crouch. WILL NOT AFFECT PLAYER SPEED.
        rise        //Make's the player stand up.
    }
}
