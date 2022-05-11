using ApuDoingStuff.Database;

namespace ApuDoingStuff.Utils
{
    public static class StringHelper
    {

        public static string Antiping(this string input)
        {
            return input.Insert(2, "󠀀");
        }

        public static string ZeroWidth(this char input)
        {
            return $"￼￼￼￼￼￼￼￼￼￼￼￼￼￼￼￼￼￼￼￼￼￼￼￼￼￼￼￼￼￼￼￼￼ {input}";
        }

        public static string Rank(string username)
        {
            string rank = DbController.GetRank(username);
            string acs = DbController.GetAcs(username);
            if (!string.IsNullOrEmpty(rank) && !string.IsNullOrEmpty(acs))
            {
                return $"[ {rank} {acs} ]";
            }
            else if (!string.IsNullOrEmpty(rank))
            {
                return $"[ {rank} ]";
            }
            else
            {
                return string.Empty;
            }
        }

        public static string EquippedRank(string username)
        {
            string rank = DbController.GetRank(username);
            string acs = DbController.GetAcs(username);
            if (!string.IsNullOrEmpty(rank) && !string.IsNullOrEmpty(acs))
            {
                return $"[currently equipped: {rank} {acs} ]";
            }
            else if (!string.IsNullOrEmpty(rank))
            {
                return $"[currently equipped: {rank} {acs} ]";
            }
            else
            {
                return "[currently equipped: - ]";
            }
        }
    }
}
