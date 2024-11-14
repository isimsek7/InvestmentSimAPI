﻿using System;
namespace CryptoAnalyzer.Business.Operations.User.Dtos
{
	public class AddUserDto
	{
		public string Email { get; set; }

		public string Password { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public DateTime BirthDate { get; set; }

		public decimal Deposit { get; set; } = 10000;
	}
}
