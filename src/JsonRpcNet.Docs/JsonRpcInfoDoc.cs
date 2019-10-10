namespace JsonRpcNet.Docs
{
    public class JsonRpcInfoDoc
    {
        public string Description { get; set; }= string.Empty;
        public string Version { get; set; }= string.Empty;
        public string Title { get; set; }= string.Empty;

        public ContactDoc Contact { get; set; } = new ContactDoc();

        public string JsonRpcApiEndpoint { get; set; } = "/jsonrpc";
    }
}