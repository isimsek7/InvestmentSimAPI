using System;
namespace CryptoAnalyzer.Business.Types
{
	public class ServiceMessage
	{
		public bool IsSucceeded { get; set; }

		public string Message { get; set; }
	}

	public class ServiceMessage<T>
	{
        public bool IsSucceeded { get; set; }

        public string Message { get; set; }

		public T? Data { get; set; }
    }
}

