using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utilities
{
    public class JwtSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessTokenExpiration { get; set; }
        public int RefreshTokenExpiration { get; set; }
        public string SecretKey { get; set; }
        public string EncryptKey { get; set; }
        public double NotBeforeMinutes { get; set; }
        public double ExpirationMinutes { get; set; }
    }
}
