using System;
using CryptoAnalyzer.Data.Enums;

namespace CryptoAnalyzer.WebApi.JwtHelper
{
    public class JwtDto
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string SecretKey { get; set; }

        public UserType UserType { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int ExpireMinutes { get; set; }

    }
}

