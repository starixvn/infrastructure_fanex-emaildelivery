using Quartz;
using System.Threading.Tasks;

namespace Fanex.MailDelivery.Client.Tests
{
    public class TestJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.CompletedTask;
        }
    }
}
