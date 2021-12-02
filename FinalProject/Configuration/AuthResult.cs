using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Configuration
{
    public class AuthResult
    {
        public string Token { get; set; }

        public bool Success { get; set; }

        public List<string> Errors { get; set; }
        // Menambahkan Properti untuk Refresh Token

        public string RefreshToken { get; set; }
    }
}
