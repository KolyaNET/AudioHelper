using System.Text.RegularExpressions;

namespace AudioHelperLib
{
    public static class Helpers
    {
        public static string ReplaceWrongs(this string str)
        {
            var wrongs = new Regex("[\\\\" + "\\/" + "\\:" + "\\*" + "\\?" + "\\<" + "\\>" + "\\|" + "\\+]");
            return wrongs.Replace(str, " ");
        }

        public static string MergeSpaces(this string str)
        {
            return new Regex("\\s\\s+").Replace(str, " ");
        }

        public static bool IsMp3(this string str)
        {
            return Regex.IsMatch(str, "\\.mp3$");
        }
    }
}
