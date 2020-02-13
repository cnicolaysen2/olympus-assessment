var JSBrigde = 
{
	$ConsoleStringCallback: {},

	SetConsoleStringCallback: function(obj)
	{
        ConsoleStringCallback.callback = obj;
    },
	
    LogMessageUnity: function(message)
	{	
		  if (ConsoleStringCallback.callback) 
		  {
		  	if(typeof message === "number")
			{
		  	    var messageString = message.toString();
		  	    Runtime.dynCall('vi', ConsoleStringCallback.callback, [allocate(intArrayFromString(messageString), 'i8', ALLOC_STACK)]);
		  	}

		  	if(typeof message === "string") 
			{
		  		  Runtime.dynCall('vi', ConsoleStringCallback.callback, [allocate(intArrayFromString(message), 'i8', ALLOC_STACK)]);
			}
		  }
    },

	LogMessageBrowser: function(message)
	{
        window.fireAngularEvent('NotificationEvent', [{type: 'success', title: Pointer_stringify(message), description: Pointer_stringify(message)}]);
    },

    LineChart: function(message) 
	{
        window.fireAngularEvent('LineChartEvent',[Pointer_stringify(message)]);
    },

	PieChart: function(message) 
	{
        window.fireAngularEvent('PieChartEvent',[Pointer_stringify(message)]);
    },
};

autoAddDeps(JSBrigde, '$ConsoleStringCallback');
mergeInto(LibraryManager.library, JSBrigde);
