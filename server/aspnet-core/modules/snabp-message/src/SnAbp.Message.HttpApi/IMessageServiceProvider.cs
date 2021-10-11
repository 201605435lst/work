using System.Threading.Tasks;

namespace SnAbp.Message
{
    public interface IMessageServiceProvider
    {
        Task SendMessage(byte[] data);
    }
}