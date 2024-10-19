namespace Messenger.Interfaces;

public interface IPasswordHash
{
    string Generate(string password);
    bool Verify(string password, string passwordHash);
}