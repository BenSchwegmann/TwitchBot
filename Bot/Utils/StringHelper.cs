namespace ApuDoingStuff.Utils
{
    public static class StringHelper
    {
        public static string Antiping(this string input)
        {
            return input.Insert(2, "󠀀");
        }
    }
}
