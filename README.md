### The UserSecrets Example:

```json
{
  "UserSecrets": {
    "Email": {
      "Address": "googlepidori@yander.ru",
      "Password": "secretpassword"
    },
    "RedisConnectionStr": "127.0.0.1:6370",
    "PostgresConnectionStr": "Server=postgres;Database=marketplacedb;Port=5432;User Id = postgres;Password=pgpassword;Pooling=true",
    "MongoDb": {
      "ConnectionString": "mongodb://localhost:27010/",
      "DatabaseName": "marketplacemongodb",
      "ImagesCollectionName": "images"
    },
    "Jwt": {
      "Issuer": "localhost",
      "AlgorithmForAccessToken": "HS256",
      "AccessTokenExpiresMinutes": 15,
      "AccessTokenNameInCookies": "jwtToken",
      "AccessTokenSecretKey": "accesstokensecretkeyaccesstokensecretkeyaccesstokensecretkeyaccesstokensecretkey",
      "AlgorithmForRefreshToken": "RS256",
      "RefreshTokenExpiresDays": 10,
      "RefreshTokenNameInCookies": "jwtToken",
      "RefreshTokenSecretKey": "secretKeysecretKeysecretKeysecretKeysecretKeysecretKeysecretKeyvsecretKeysecretKeysecretKey"
    }
  }
}
```

I think this project doesn't need a Cqrs/Mediator. Why is this? I think my project isn't so big to it has cqrs(if I'm
lazy)