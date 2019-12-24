using MongoDB.Driver;

namespace apigateway
{
    internal static class CollationHelper
    {
        public static Collation EnUsInvariantCase => new Collation("en_US", strength: CollationStrength.Secondary);
    }
}
