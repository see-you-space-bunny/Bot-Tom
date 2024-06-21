using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using org.mariuszgromada.math.mxparser;
using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace BotTom;

public class Program
{
	private static IConfiguration _configuration;
	private static IServiceProvider _services;

	private static readonly DiscordSocketConfig _socketConfig = new()
	{
		GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers,
		AlwaysDownloadUsers = true,
	};

	private static readonly InteractionServiceConfig _interactionServiceConfig = new()
	{
		LocalizationManager = new ResxLocalizationManager("BotTom.Resources.CommandLocales", Assembly.GetEntryAssembly(),
			new CultureInfo("en-US"), new CultureInfo("ru"))
	};

	public static async Task Main(string[] args)
	{
		ControlPanel.LoadFileConfig();
		Envy.Envy.Load(Path.Combine(Environment.CurrentDirectory, ".env"));
		
		InitMxParser();

		_configuration = new ConfigurationBuilder()
				.AddEnvironmentVariables(prefix: "DISCORD_")
				.AddJsonFile("appsettings.json", optional: true)
				.Build();
		
		_services = new ServiceCollection()
				.AddSingleton(_configuration)
				.AddSingleton(_socketConfig)
				.AddSingleton<DiscordSocketClient>()
				.AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>(), _interactionServiceConfig))
				.AddSingleton<InteractionHandler>()
				.BuildServiceProvider();

		var client = _services.GetRequiredService<DiscordSocketClient>();

		client.Log += Log;

		// Here we can initialize the service that will register and execute our commands
		await _services.GetRequiredService<InteractionHandler>()
				.InitializeAsync();

		// Bot token can be provided from the Configuration object we set up earlier
		await client.LoginAsync(TokenType.Bot, _configuration["TOKEN"]);
		await client.StartAsync();

		// Never quit the program until manually forced to.
		await Task.Delay(Timeout.Infinite);
	}

	#region Log
	private static Task Log(LogMessage msg)
	{
		switch (msg.Severity)
		{
			case LogSeverity.Critical:
			case LogSeverity.Error:
				Console.ForegroundColor = ConsoleColor.Red;
				break;
			case LogSeverity.Warning:
				Console.ForegroundColor = ConsoleColor.Yellow;
				break;
			case LogSeverity.Info:
				Console.ForegroundColor = ConsoleColor.White;
				break;
			case LogSeverity.Verbose:
			case LogSeverity.Debug:
				Console.ForegroundColor = ConsoleColor.DarkGray;
				break;
		}
		Console.WriteLine($"{DateTime.Now,-19} [{msg.Severity,8}] {msg.Source}: {msg.Message} {msg.Exception}");
		Console.ResetColor();
		
		// If you get an error saying 'CompletedTask' doesn't exist,
		// your project is targeting .NET 4.5.2 or lower. You'll need
		// to adjust your project's target framework to 4.6 or higher
		// (instructions for this are easily Googled).
		// If you *need* to run on .NET 4.5 for compat/other reasons,
		// the alternative is to 'return Task.Delay(0);' instead.
		return Task.CompletedTask;
	}
	#endregion

	#region InitMxParser
	private static void InitMxParser()
	{
		// ----------------------------------------------------------------------------------
		// mXparser required
		// Non-Commercial Use Confirmation
		bool isCallSuccessful = License.iConfirmNonCommercialUse(Environment.GetEnvironmentVariable("USER_NAME"));

		// Verification if use type has been already confirmed
		bool isConfirmed = License.checkIfUseTypeConfirmed();

		// Checking use type confirmation message
		String message = License.getUseTypeConfirmationMessage();

		// ----------------------------------------------------------------------------------
		Console.WriteLine("isCallSuccessful = " + isCallSuccessful);
		Console.WriteLine("isConfirmed = " + isConfirmed);
		Console.WriteLine("message = " + message);
		// ----------------------------------------------------------------------------------
	}
	#endregion
}