namespace JN_WEB.Services
{
    public interface IPasswordHelper
    {
        string Encrypt(string texto);
        string Decrypt(string texto);
    }
}
