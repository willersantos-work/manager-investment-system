namespace InvestManagerSystem.Global.Helpers.Hash
{
    public static class HashHelper
    {
        public static string Hash(this string text)
        {
            return BCrypt.Net.BCrypt.HashPassword(text);
        }
        
        public static bool Verify(this string text, string hashText)
        {
            return BCrypt.Net.BCrypt.Verify(text, hashText);
        }
    }
}
