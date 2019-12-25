using MongoDB.Driver;
using System;

namespace apigateway
{
    public static class AuthTokenUpdateDefinitions
    {
        public static UpdateDefinition<AuthToken> ExpirationTime(DateTime dateTime) =>
            Builders<AuthToken>.Update.Set(e => e.Expires, dateTime);
    }
}