using System;
namespace CryptoAnalyzer.Business.DataProtection
{
	public interface IDataProtection
	{
		string Protect(string text);
		string UnProtect(string protectedText);
	}
}

