namespace TrabalhoBackEnd.Models
{
    public enum SortDir
    {
        ASC,
        DESC
    }

    public static class SortDirExtensions
    {
        public static SortDir? GetByName(string sortDir)
        {
            if (Enum.TryParse<SortDir>(sortDir, true, out var result))
            {
                return result;
            }
            return null;
        }
    }
}
