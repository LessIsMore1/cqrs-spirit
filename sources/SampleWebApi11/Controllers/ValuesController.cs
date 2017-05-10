using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CqrsSpirit;
using Microsoft.AspNetCore.Mvc;
using SampleWebApi11.Commands;
using SampleWebApi11.Datasource;
using SampleWebApi11.Queries;
using SampleWebApi11.Workflows;

namespace SampleWebApi11.Controllers
{
    //[Route("api/[controller]")]
    public class ValuesController : Controller
    {
        protected readonly IGetValuesQuery GetValuesQuery;

        protected readonly IMakeEvenItemsLowercaseWorkflow MakeEvenItemsLowercaseWorkflow;

        protected readonly ICommandsDispatcher CommandsDispatcher;

        public ValuesController(IGetValuesQuery getValuesQuery,
            IMakeEvenItemsLowercaseWorkflow makeEvenItemsLowercaseWorkflow,
            ICommandsDispatcher commandsDispatcher)
        {
            GetValuesQuery = getValuesQuery;
            MakeEvenItemsLowercaseWorkflow = makeEvenItemsLowercaseWorkflow;
            CommandsDispatcher = commandsDispatcher;
        }

        [HttpGet]
        [Route("api/values")]
        public async Task<IEnumerable<Item>> Get(string filter = null)
        {
            var result = await GetValuesQuery.ExecuteAsync(filter);
            return result;
        }

        [HttpPost]
        [Route("api/values/update")]
        public async Task Update(int id, string value)
        {
            await CommandsDispatcher.ExecuteAsync(new UpdateItemCommand(id, value));
        }

        [HttpPost]
        [Route("api/values/dosomethings")]
        public async Task Workflow()
        {
            await MakeEvenItemsLowercaseWorkflow.ExecuteAsync();
        }
				
        // GET api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
