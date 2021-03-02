# Mail Station #

### Installation ###

Install nuget package: [Fanex.MailStation](http://nuget.nexdev.net/packages/Fanex.MailStation)


## v2.1.0 Changes ##

- Support .Net Core

### App Setting (appsettings.json) ###

	{
	  "Fanex.MailDelivery.Client": {
	    "ApplicationSendMailId": 1,
	    "SiteId": 1,
	    "RabbitMQConnectionString": "host=ha.nexdev.net:5672;virtualHost=/MailStationHost;username=rabbitclient;timeout=120",
	
	    "Functions": [
	      //{
	      //  "Id": 1,
	      //  "Name": "YourFunction"
	      //}
	    ],
	
	    "Schedules": [
	      //{
	      //  "Id": 1,
	      //  "Name": "YourSchedule",
	      //  "Job": "YourProject.Namespace.Job, YourProject"
	      //}
	    ]
	  }
	}

### Code Sample: ###

- Use default AppSetting (appsettings.json):     
    
	`await MailClientHandler.StartAsync();`

- Use custom AppSetting (custom-appsettings.json):
    
	`await MailClientHandler.StartAsync("custom-appsettings.json");`



## v2.0 Changes ##

* Fix bugs

* Improvements

* Add detail logs

* Update libraries

* .Net Framework 4.6.1

* Remove attribute **messageBrokerLogCategory**


### Configuration ###

	<Fanex.MailDelivery.Client applicationSendMailId="68" siteId="97" rabbitMQConnectionString="host=ha.nexdev.net:5672;virtualHost=/MailStationHost;username=rabbitclient;timeout=120">
	  <Functions>
	    <!--<add id="1" name="YourFunction" />-->
	  </Functions>
	  <Schedules>
	    <Schedule id="313" name="SendMailJob" job="MailStationClient.AutoJobDemo.SendMailJob, MailStationClient.AutoJobDemo" />
	  </Schedules>
	</Fanex.MailDelivery.Client>

``Attributes:``

	applicationSendMailId
		The #Id of an application to send mail (configured in mSite).
	
		It's an integer and **required** attribute.
	
	siteId
		The #Id of a Nap site.
		
		It's an integer and **required** attribute.
		
	rabbitMQConnectionString
		The connection string of RabbitMQ server.
		
		It's a string and **required** attribute.
		
	requestRetryLimit
		The limited times to retry a request to server if connection failed.
		
		It's an integer and *optional* attribute.
		*Default value is 20 times.*
		
	requestRetryIntervalInSeconds
		The waiting interval in seconds to retry a request if failed.
		
		It's an integer and *optional* attribute.
		*Default value is 120 seconds.*

``Elements:``

	Functions
		It contains the configurations of function mails. You can create function mails on *mSite* and configure them right here.
		
	Schedules
		It contains the configurations of scheduled mails (auto). You can create scheduled mails on *mSite* and configure them right here.
		
		
### Function Mails ###

**App.config**

	<Fanex.MailDelivery.Client applicationSendMailId="68" siteId="97" rabbitMQConnectionString="host=ha.nexdev.net:5672;virtualHost=/MailStationHost;username=rabbitclient;timeout=120">
	  <Functions>
	    <add id="316" name="FunctionMail" />
	  </Functions>
	  <Schedules>
	    <!--<Schedule id="2" name="YourSchedule" job="YourProject.Namespace.Job, YourProject" />-->
	  </Schedules>
	</Fanex.MailDelivery.Client>

**Code Sample**

	class Program
	{
	    static void Main(string[] args)
	    {
	        try
	        {
	            MailClientHandler.Start();
    
	            var mail = new Mail
	            {
	                Subject = "Mail Station Client - Function mail demo",
	                Title = "Function mail demo",
	                Body = "Function mail Body",
	                MailingListId = AppSettings.GetFunctionId("FunctionMail")
	            };
    
	            if (mail.MailingListId > 0)
	            {
	                MailClientHandler.SendFunctionMail(mail);
	            }
	        }
	        finally
	        {
	            MailClientHandler.Stop();
	        }
	    }
	}

### Scheduled Mails (auto) ###

**Create job class to generate mail**

	namespace MailStationClient.AutoJobDemo
	{
	    public class SendMailJob : JobBase
	    {
	        protected override JobMessage Run(Schedule schedule, IJobExecutionContext jobContext)
	        {
	            // DO NOT wrap your code in try catch block because it's handled in the base class to retry running if there is any error.
    
	            // Generate mail content
	            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "attachment.txt");
	            var mail = new Mail
	            {
	                Subject = "Mail Station Client - Auto job demo",
	                Title = "Auto job demo",
	                Body = "Auto job Body",
	                Attachments = new List<MailAttachment> {
	                    new MailAttachment(filePath, FileExtension.TEXT)
	                }
	            };
    
	            return new JobMessage
	            {
	                Mail = mail,
	                Schedule = schedule,
	                JobStatus = JobStatus.Success
	            };
	        }
	    }
	}


**App.config**

	<Fanex.MailDelivery.Client applicationSendMailId="68" siteId="97" rabbitMQConnectionString="host=ha.nexdev.net:5672;virtualHost=/MailStationHost;username=rabbitclient;timeout=120">
	  <Functions>
	    <!--<add id="1" name="YourFunction" />-->
	  </Functions>
	  <Schedules>
	    <Schedule id="313" name="SendMailJob" job="MailStationClient.AutoJobDemo.SendMailJob, MailStationClient.AutoJobDemo" />
	  </Schedules>
	</Fanex.MailDelivery.Client>
	
	
**Windows Service**

    public partial class AutoJobService : ServiceBase
    {
        protected override void OnStart(string[] args)
        {
            MailClientHandler.Start();
        }

        protected override void OnStop()
        {
            MailClientHandler.Stop();
        }
    }
