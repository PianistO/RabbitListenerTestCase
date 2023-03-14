using MassTransit;
using RabbitListener.Common.Helpers.LogHelper;
using RabbitListener.Common.Helpers.RequestHelper;
using RabbitListener.Core.Commands;
using RabbitListener.Core.Configurations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsumerWorkerService.Consumers
{
    public class UrlCommandConsumer : IConsumer<UrlCommand>
    {
        IConfiguration _configuration;
        public UrlCommandConsumer(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task Consume(ConsumeContext<UrlCommand> context)
        {
                var projectConfiguration = _configuration.GetSection("ProjectConfig").Get<ProjectConfiguration>();
                var serviceName = projectConfiguration.ServiceName;
                var statusCode = RequestHelper.GetStatusCode(context.Message.Url);
                LogModel logModel = (new LogModel
                {
                    StatusCode = statusCode,
                    ServiceName = serviceName,
                    Url = context.Message.Url,
                });
                LogHelper.WriteLog(logModel);
        }
    }
}
