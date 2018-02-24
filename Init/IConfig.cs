

using System.Net;

public interface IConfig 
{
    string DomainName { get; }
    string PiloteDomainName { get; }
    IPAddress PiloteIp { get; }
    string PiloteRepositoryPort { get; }
}