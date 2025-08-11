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

    public enum EventType
    {
        look,       //Makes the player look around. Vector3 params for rotation.
        walk,       //Let's the player proceed to next node. float params for player speed.
        stop,       //Stops the player in the current node. No params.
        hold        //Makes the player hold their fire. No params.
    }
}
