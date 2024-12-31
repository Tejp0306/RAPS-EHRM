using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ServiceLayer.Helpers
{
    public static class JwtSessionHelper
    {
        private static string SecretKey = "your-very-long-and-strong-secret-key-here-with-at-least-32-bytes"; // Replace with a more secure key

        // Generate JWT token
        public static string GenerateJwtToken(string userId, string email, string userName, int? roleId)
        {
            try
            {
                // Validate input parameters
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(userName))
                {
                    throw new ArgumentException("Invalid input parameters.");
                }

                // Create claims
                var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, roleId?.ToString() ?? "0") // Default to 0 if roleId is null
            };

                // Define the signing key (ensure SecretKey is at least 256 bits or 32 bytes long)
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)); // Make sure SecretKey is at least 32 bytes
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // Generate the JWT token
                var token = new JwtSecurityToken(
                    issuer: "https://localhost:7075/",  // Use your actual API URL here
                    audience: "https://localhost:7075/", // Use your actual frontend URL here
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30), // Token expiration time
                    signingCredentials: creds
                );

                // Return the serialized token
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (ArgumentException ex)
            {
                // Log specific invalid argument exceptions (e.g., null or empty parameters)
                throw new InvalidOperationException("Input validation failed.", ex);
            }
            catch (Exception ex)
            {
                // Catch other exceptions and log them
                throw new InvalidOperationException("An error occurred while generating the JWT token.", ex);
            }
        }

        // Extract session data from JWT token
        public static (string userId, string userName, string email, int roleId) ExtractSessionData(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var userId = jwtToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                var userName = jwtToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                var email = jwtToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var roleId = int.Parse(jwtToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "0");

                return (userId, userName, email, roleId);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error extracting session data from the token.", ex);
            }
        }
    }


}
