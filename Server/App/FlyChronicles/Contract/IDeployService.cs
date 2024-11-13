using System.Threading.Tasks;

namespace FlyCronicles.Contract
{
    public interface IDeployService
    {
        Task Deploy(int? num = null);
    }
}