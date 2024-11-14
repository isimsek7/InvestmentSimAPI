using System;
using System.ComponentModel.DataAnnotations;

namespace CryptoAnalyzer.WebApi.Models
{
	public class LoginRequest
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }

	}
}

