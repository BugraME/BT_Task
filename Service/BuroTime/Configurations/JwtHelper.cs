using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BuroTime.Configurations;

public static class JwtHelper {
	public static string GenerateToken() {
		SymmetricSecurityKey symmetricSecurityKey = new(Encoding.UTF8.GetBytes("ED236AE5-1841-4FFC-9D61-228B3A08336D"));
		JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
		SecurityTokenDescriptor securityTokenDescriptor = new() {
			Issuer = "burotime",
			Expires = DateTime.Now.AddMinutes(30),
			SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
		};
		SecurityToken token = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
		return jwtSecurityTokenHandler.WriteToken(token);
	}
}