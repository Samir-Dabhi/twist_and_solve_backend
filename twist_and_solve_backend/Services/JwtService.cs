using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace twist_and_solve_backend.Services
{
    public class JwtService
    {
        private readonly string _secret;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expiryMinutes;

        // Simulated storage for refresh tokens (Use a database in production)
        private static Dictionary<string, string> _refreshTokens = new Dictionary<string, string>();

        public JwtService(IConfiguration config)
        {
            _secret = config["JwtSettings:Secret"] ?? throw new ArgumentNullException("JWT Secret is missing in configuration.");
            _issuer = config["JwtSettings:Issuer"] ?? "defaultIssuer";
            _audience = config["JwtSettings:Audience"] ?? "defaultAudience";
            _expiryMinutes = int.TryParse(config["JwtSettings:ExpiryMinutes"], out int expiry) ? expiry : 60 * 24 * 60;
        }

        public TokenResponse GenerateToken(string userId, string role)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID cannot be null or empty.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                _issuer,
                _audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(_expiryMinutes), // Access token expiry
                signingCredentials: credentials
            );

            // Generate a Refresh Token
            var refreshToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            // Store Refresh Token (Ideally, use a database)
            _refreshTokens[userId] = refreshToken;

            return new TokenResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken
            };
        }

        public TokenResponse? RefreshToken(string refreshToken)
        {
            // Find user associated with the given refresh token
            var user = _refreshTokens.FirstOrDefault(rt => rt.Value == refreshToken);

            if (string.IsNullOrEmpty(user.Key))
                return null; // Invalid refresh token

            // Generate new tokens
            return GenerateToken(user.Key, "User");
        }
    }

    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
