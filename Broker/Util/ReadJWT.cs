using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.;


namespace ClassLibrary_SEP3.RabbitMQ
{
    public class ReadJwt
    {
        public static string ReadUsernameFromSubInJWTToken(HttpContext httpContext)
        {
            var Token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var token = Token;
            var parts = token.Split('.');
            var payload = parts[1];
            var payloadJson = Encoding.UTF8.GetString(ParseBase64WithoutPadding(payload));
            var payloadData = JsonSerializer.Deserialize<Dictionary<string, object>>(payloadJson);
            var sub = payloadData["sub"].ToString();
            return sub;
        }
        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2:
                    base64 += "==";
                    break;
                case 3:
                    base64 += "=";
                    break;
            }

            return Convert.FromBase64String(base64);
        }
    }

}
