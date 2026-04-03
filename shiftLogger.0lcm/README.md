# Shift Logger
This project was made under the requirements set out [here](https://www.thecsharpacademy.com/project/17/shifts-logger).  
The project works as a functional way to upload work shifts to the database, which then can be assigned to a registered employee, viewed, deleted, edited, marked as completed, and more,
this makes it great for keeping track of what shifts are taken, which ones are available, which shifts are assigned to which employees, as well as the shift's dates, time span, and
other qualities.  

# Features
* You can not only log shifts, you can also log employees, allowing you to keep track of everyone from one application.  
* A simple interface allows for easy access to shift and employee details.  
  ![image of an employee's details](https://i.imgur.com/ZkVkX6K.png)  
  ![image of a shift's details](https://i.imgur.com/CLbzFP1.png)  
* All data is stored locally on an SQLite database.  
* Extensive viewing options allow you to view shifts based on a variety of requirements.  
  ![image of the shift viewing menu](https://i.imgur.com/t1kCx13.png)  
* A list of possible actions makes it easy to find what you need.  
  ![image of the shift menu](https://i.imgur.com/Zy23nu8.png)  
  ![image of the employee menu](https://i.imgur.com/CZbsMlQ.png)  

  # Setup
  To use the application, you simply need to clone the repo and run it on your machine, using a multi launch profile to launch both shiftLogger.0lcm Ui project, and the shiftLogger.0lcm.Api
  project.  

  # Resources used
  [.NET (10.0)](https://learn.microsoft.com/en-us/dotnet/)  
  [Spectre.Console (0.54.0)](https://spectreconsole.net/cli)  
  [Microsoft.Extensions.Hosting (10.0.5)](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting?view=net-10.0-pp)  
  [Microsoft.Extensions.Http (10.0.5)](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.http?view=net-10.0-pp)  
  [System.Net.Http.Json (10.0.5)](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.json?view=net-10.0)  
  [Microsoft.AspNetCore.OpenApi (10.0.3)](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/overview?view=aspnetcore-10.0)  
  [Microsoft.EntityFrameworkCore (10.0.5)](https://learn.microsoft.com/en-us/ef/)  
  [Microsoft.EntityFrameworkCore.Design (10.0.5)](https://learn.microsoft.com/en-us/ef/core/cli/services)  
  [Microsoft.EntityFrameworkCore.Sqlite (10.0.5)](https://learn.microsoft.com/en-us/ef/core/providers/sqlite/?tabs=dotnet-core-cli)  
  [Swashbuckle.AspNetCore (10.1.5)](https://www.nuget.org/packages/swashbuckle.aspnetcore)  
  [Swashbuckle.AspNetCore.Swagger (10.1.5)](https://www.nuget.org/packages/swashbuckle.aspnetcore.swagger/)  
  [Swashbuckle.AspNetCore.SwaggerGen (10.1.5)](https://www.nuget.org/packages/swashbuckle.aspnetcore.swaggergen/)  
  [Swashbuckle.AspNetCore.SwaggerUi (10.1.5)](https://www.nuget.org/packages/swashbuckle.aspnetcore.swaggerui/)  

  # Personal Thoughts
  It was interesting to build an API for the first time, while I definitly had some trouble with it at the beginning, it worked out well in the end. I think I procrastinated a little
  bit more than I should have during this project, but at least it got done, and I can move onto the next project. I liked building up two seperate projects and seeing how they
  connect with each other, and how the UI depends on the API project, and it was definitly cool to see it connecting to something I built myself, rather than something someone else built,
  even if what I built isn't as good as those professional APIs out there. I'm excited to finally be done with this project, even though I could have done it quicker if I focused more,
  and I'm very excited to see what project I'll be making next, and what I'll be able to learn during the next project.
