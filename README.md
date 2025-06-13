# How to run

1. Clone the repo
2. Install .NET 8 SDK - https://dotnet.microsoft.com/en-us/download/dotnet/8.0
3. Use dotnet run commmand. If you have Visual Studio installed with the version specified above you can run the app from the VS directly.
4. The default N of stories is 5. Feel free to call the ednpoint from browser, postman etc using the folling URL - https://localhost:7203/api/stories?count={NUMBER}.

# Performance considerations

1. To avoid overload of the HackerNews API, a memory caching was added with the duration of 5 minutes per story.
2. To improve response time of the implemented endpoint a parallel processing was added.

# What can be added

1. Logging
2. Caching time may be adjusted
