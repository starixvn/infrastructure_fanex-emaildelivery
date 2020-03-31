***********************
Mail Station 2.0
***********************

Installation
#############

`Fanex.MailStation <http://nuget.nexdev.net/packages/Fanex.MailStation>`_

Configuration
##############

.. code-block:: xml

	<Fanex.MailDelivery.Client applicationSendMailId="68" siteId="97" rabbitMQConnectionString="host=ha.nexdev.net:5672;virtualHost=/MailStationHost;username=rabbitclient;timeout=120">
	  <Functions>
	    <!--<add id="1" name="YourFunction" />-->
	  </Functions>
	  <Schedules>
	    <Schedule id="313" name="SendMailJob" job="MailStationClient.AutoJobDemo.SendMailJob, MailStationClient.AutoJobDemo" />
	  </Schedules>
	</Fanex.MailDelivery.Client>

*Attributes:*

.. glossary::

	applicationSendMailId
		It's required.
		The #Id of application to send mail (configured in mSite).
	
	siteId
		It's required.
		The #Id of Nap site.
		
	rabbitMQConnectionString
		It's required.
		The connection string of RabbitMQ.
		
	requestRetryLimit
		It's optional.
		The maximum number of times to retry request to server if connection failed.
		Default value is 20 times.
		
	requestRetryIntervalInSeconds
		It's optional.
		The waiting interval in seconds to retry request if failed.
		Default value is 120 seconds.
