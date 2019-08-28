namespace JsonRpcNet.Authentication
{
	public interface ITokenReader
	{
		AuthenticatedIdentity Read(string token);
	}
}