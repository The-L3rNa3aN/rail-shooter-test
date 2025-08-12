public static class Util
{
    public static string ReturnNodeSuffix(int number)
    {
        return (number / 10) switch
        {
            < 1 => "00" + number,
            < 10 => "0" + number,
            _ => number.ToString()
        };
    }

    /* EVENT NAME           PARAMS          DURATION
       ==========           ==========      ==========
       look                 Vector3         yes
       walk                 float           no
       stop                 nil             yes
       hold                 nil             yes         */
    public enum EventType
    {
        look,       //Makes the player look around.
        walk,       //Let's the player proceed to next node.
        stop,       //Stops the player in the current node.
        hold        //Makes the player hold their fire.
    }
}
