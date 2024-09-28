Реализация JWT-авторизации с Refresh Token в .NET
Давайте рассмотрим, как это можно реализовать в ASP.NET Core.

1. Настройка аутентификации и генерация токенов
Настройка конфигурации JWT:

В appsettings.json добавьте секцию с настройками:

json
Копировать код
{
    "JwtSettings": {
        "SecretKey": "your_secret_key_here",
        "Issuer": "your_issuer",
        "Audience": "your_audience",
        "AccessTokenExpiration": 15,
        "RefreshTokenExpiration": 1440
    }
}
Создание метода для генерации токенов:

В сервисе аутентификации создайте метод для генерации Access Token и Refresh Token:

csharp
Копировать код
public class TokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var tokenOptions = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:AccessTokenExpiration"])),
            signingCredentials: signinCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
Сохранение рефреш токена:

Рефреш токен нужно сохранить на сервере (например, в базе данных) вместе с идентификатором пользователя и сроком действия.

2. Аутентификация и генерация токенов
Когда пользователь аутентифицируется, сервер возвращает Access Token и Refresh Token:

csharp
Копировать код
[HttpPost("login")]
public IActionResult Login([FromBody] LoginModel model)
{
    // Проверка учетных данных пользователя
    var user = _userService.Authenticate(model.Username, model.Password);
    if (user == null)
        return Unauthorized();

    var tokenService = new TokenService(_configuration);

    var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Username),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

    var accessToken = tokenService.GenerateAccessToken(claims);
    var refreshToken = tokenService.GenerateRefreshToken();

    // Сохранение refresh token в базе данных
    _userService.SaveRefreshToken(user.Id, refreshToken);

    return Ok(new
    {
        AccessToken = accessToken,
        RefreshToken = refreshToken
    });
}
3. Обновление токена доступа
Когда токен доступа истекает, клиент отправляет запрос на сервер с Refresh Token:

csharp
Копировать код
[HttpPost("refresh")]
public IActionResult Refresh([FromBody] TokenModel tokenModel)
{
    var principal = GetPrincipalFromExpiredToken(tokenModel.AccessToken);
    var username = principal.Identity.Name;
    
    // Проверка refresh token в базе данных
    var savedRefreshToken = _userService.GetRefreshToken(username);
    if (savedRefreshToken != tokenModel.RefreshToken)
        return Unauthorized();

    var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
    var newRefreshToken = _tokenService.GenerateRefreshToken();

    // Обновление refresh token в базе данных
    _userService.SaveRefreshToken(username, newRefreshToken);

    return Ok(new
    {
        AccessToken = newAccessToken,
        RefreshToken = newRefreshToken
    });
}

private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
{
    var tokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false, // Не проверяем время жизни, так как токен уже истек
        ValidateIssuerSigningKey = true,
        ValidIssuer = _configuration["JwtSettings:Issuer"],
        ValidAudience = _configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]))
    };

    var tokenHandler = new JwtSecurityTokenHandler();
    SecurityToken securityToken;
    var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

    var jwtSecurityToken = securityToken as JwtSecurityToken;
    if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        throw new SecurityTokenException("Invalid token");

    return principal;
}
4. Использование токенов в запросах
Клиент отправляет Access Token в заголовке каждого запроса:

http
Копировать код
GET /api/securedata
Authorization: Bearer <access_token>
5. Безопасность и лучшие практики
Безопасность Refresh Token:

Храните Refresh Token в HTTP-only и Secure cookies или в базе данных на сервере.
Refresh Token должен быть длиннее и безопаснее Access Token.
Регулярная ротация токенов:

Регулярно ротация Refresh Token после нескольких обновлений токенов.
Отзыв токенов:

Реализуйте механизм отзыва токенов, чтобы можно было немедленно отменить доступ при подозрении на компрометацию.
Кратковременность Access Token:

Устанавливайте короткий срок жизни для Access Token (например, 15 минут), чтобы минимизировать ущерб в случае его компрометации.
Заключение
Использование JWT с Bearer-авторизацией и Refresh Token обеспечивает безопасный и удобный механизм аутентификации и авторизации в веб-приложениях. Он позволяет ограничить срок действия токенов доступа и обеспечить возможность безопасного обновления сессий без необходимости повторной аутентификации пользователя.
