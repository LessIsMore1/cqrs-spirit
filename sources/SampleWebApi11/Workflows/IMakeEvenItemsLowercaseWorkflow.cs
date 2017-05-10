using System.Linq;
using System.Threading.Tasks;
using CqrsSpirit;
using SampleWebApi11.Commands;
using SampleWebApi11.Datasource;
using SampleWebApi11.Queries;

namespace SampleWebApi11.Workflows
{
    public interface IMakeEvenItemsLowercaseWorkflow : IWorkflow
    {
        Task ExecuteAsync();
    }

    public class MakeEvenItemsLowercaseWorkflow : IMakeEvenItemsLowercaseWorkflow
    {
        protected readonly IGetValuesQuery GetValuesQuery;

        protected readonly ICommandsDispatcher CommandsDispatcher;

        public MakeEvenItemsLowercaseWorkflow(IGetValuesQuery getValuesQuery, ICommandsDispatcher commandsDispatcher)
        {
            GetValuesQuery = getValuesQuery;
            CommandsDispatcher = commandsDispatcher;
        }

        public async Task ExecuteAsync()
        {
            var items = await GetValuesQuery.ExecuteAsync();

            var itemsToUpdate = items.Select((Value, Index) => new { Index, Value })
                .Where(x => (x.Index % 2) != 0)
                .Select(x => x.Value);

            foreach (Item item in itemsToUpdate)
            {
                await CommandsDispatcher.ExecuteAsync(new UpdateItemCommand(item.Id, item.Value?.ToLowerInvariant()));
            }
        }
    }
}