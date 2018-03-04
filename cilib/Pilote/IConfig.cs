

using System.Net;

public interface IConfig
{
    string DomainName {get;}
    IPAddress PiloteIp { get; }
    string PiloteRepositoryPort { get; }
}