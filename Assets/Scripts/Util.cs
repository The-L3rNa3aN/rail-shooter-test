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
}
