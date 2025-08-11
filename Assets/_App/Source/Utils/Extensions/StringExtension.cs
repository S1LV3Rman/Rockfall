namespace S1LV3Rman.RockFall
{
    public static class StringExtension
    {
        public static bool IsNullOrWhiteSpace(this string value) =>
            string.IsNullOrWhiteSpace(value);

        public static bool IsNullOrEmpty(this string value) =>
            string.IsNullOrEmpty(value);
    }
}