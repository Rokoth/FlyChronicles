using System.Threading.Tasks;

namespace FlyCronicles.Common.Options
{
    public interface IErrorNotifyService
    {
        Task Send(string message, MessageLevelEnum level = MessageLevelEnum.Error, string title = null);
    }

    public enum MessageLevelEnum
    {
        Issue = 0,
        Warning = 1,
        Error = 10
    }
}
