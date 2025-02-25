# Youtube Overview:
[![IMAGE ALT TEXT HERE](https://img.youtube.com/vi/fYVWTa-BLrk/0.jpg)](https://www.youtube.com/watch?v=fYVWTa-BLrk)


## The UserSecrets Example:

```json
{
  "UserSecrets": {
    "Email": {
      "Address": "googlepidori@yander.ru",
      "Password": "email_password"
    },
    "RedisConnectionStr": "redis:6379",
    "PostgresConnectionStr": "Server=postgres;Database=marketplace;Port=5432;User Id = postgres;Password=pg_password;Pooling=true",
    "MongoDb": {
      "ConnectionString": "mongodb://mongodb:27010/",
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

I think this project doesn't need a Cqrs/Mediator. Why is this? I think my project isn't so big to it has cqrs(or I'm really lazy)

By default, The EF Core will add 4 delivery companies.

In secrets, UserSecrets.Email.Password have to be a yandex's email. Because the fckg google thought that I'm Russia citizen.
