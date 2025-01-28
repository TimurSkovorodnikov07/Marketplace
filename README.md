### UserSecrets: 
```json
{
  "UserSecrets": {
    "Email": {
      "Address": "googlepidori@yander.ru",
      "Password": "secretpassword"
    },
    "RedisConnectionStr": "127.0.0.1:6379",
    "PostgresConnectionStr": "Database=marketplacedb;Server=localhost;Port=5432;User Id = postgres;Password=pgpassword;Pooling=true",
    "MongoDb": {
      "ConnectionString": "mongodb://localhost:27017/",
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
