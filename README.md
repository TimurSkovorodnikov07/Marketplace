### UserSecrets: 
```json
{
  "UserSecrets": {
    "Email": {
      "Address": "email@mail.kz",
      "Password": "password"
    },
    "RedisConnectionStr": "127.0.0.1:6379",
    "PostgresConnectionStr": "Database=marketplacedb;Server=localhost;Port=5432;User Id =postgres;Password=password;Pooling=true",
    "PathToFiles": "/home/timur/Desktop/Marketplace/files",
    "Jwt": {
      "Issuer": "localhost",
      "AlgorithmForAccessToken": "HS256",
      "AccessTokenExpiresMinutes": 15000,
      "AccessTokenNameInCookies": "jwtToken",
      "AccessTokenSecretKey": ";labkfjlkajdslkfjavslkdfjvdakdjfvlakdjvjsavlkvdjvslkjfaskljfavl;kjdflkvajflkvjdasfvj",
      "AlgorithmForRefreshToken": "RS256",
      "RefreshTokenExpiresDays": 10,
      "RefreshTokenNameInCookies": "jwtToken",
      "RefreshTokenSecretKey": "secretKeysecretKeysecretKeysecrealksdjfl;af;jlaaksdl;fjal;kfjdsjfl;dfjlkasjfl;cretKeysecretKey"
    }
  }
}
```