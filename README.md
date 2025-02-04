# My Ledger API



##  📋 Pre-Requirements


The tools required to develop on this project are:

* [.NET 8](https://dotnet.microsoft.com/download) 
* [SQL Server Developer Free edition](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
* [SQL Server Management Studio](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16#download-ssms)
* [Visual Studio Community](https://visualstudio.microsoft.com/downloads/)

## 🔌 Configurations

### SQL Server Management Studio

To connect to your local server in the credentials input the following data:

  * Server name: (LocalDB)\MSSQLLocalDB
  * Authentication: Windows Authentication
  * Trust Server Certificate: ✅

Click on the Connect button and you should be able to see all your local databases.

### Creating local database for local testing and development
Inside Microsoft SQL Server Management Studio:

* Right-click on the **Databases** folder and select the **New Database...** option
* Give it the following name: **MyLedgerDB**
* Click **OK**

Now from the root folder of the application:

* Navigate to the **Scripts** folder
* Copy the contents of **Schema.sql**

Back on Microsoft SQL Server Management Studio:
* Right-click the newly created Database and select the **New Query** option
* Paste the contents of **Schema.sql**
* Click on the **Execute** button

### Populate local database with sample data

* Copy the contents of **Data.sql** from the **Scripts** folder onto a new Query on the database and execute it.

### Export Models

If you want to export your models in the database to your models in the code use the following command:

```dotnet ef dbcontext scaffold "Server=(localDBServerName)\mssqllocaldb;Database=MyLedgerDB;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models```

But if you want to update your models in your code with recently updated database changes use the following command:

```dotnet ef dbcontext scaffold "Server=(localDBServerName)\mssqllocaldb;Database=MyLedgerDB;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models -f```

Where (localdb) is the name of your local SQL server

**At the moment of updating the models, the constructor and the onConfiguring method will be reset, so save it in a notepad and put it back to the MyLedgerDbContext.cs file**

**Warning Note:**
If, at the moment of running one of the mentioned commands you have a login error, run the following query in your database. This command will change the owner from the database to your local Windows user: 

```sh
Use <database_name>
GO
sp_changedbowner '<user_name>'
GO
```

To see your user name press the view connection properties in the connection.

## 💿 Install Dependencies

From the powershell you need to use the following command to install the Entity Framework Core tools:

```dotnet tool install --global dotnet-ef```

## 🔨 Build Project

To build the project you need to use the following command in the root folder of the project:

```dotnet build```

## 💻 Run Project

There are two ways to start the project, from the Visual Studio IDE or CLI.

* Visual Studio 
  * If you want to run the program from IDE, you have to situate in the task bar and in the play button options change it to Development, press the button and the program will start.
* CLI
  * If you want to run the program from CLI, you have to use the following command in the Presentation folder of the project: ```dotnet run --profile="Development"```
  
 ## Connection String LocalDB

```sh
"ServiceDatabase": "Server=(localDBServerName);Database=MyLedgerDB;Trusted_Connection=True;TrustServerCertificate=True;Integrated Security=True"
```


## Other required settings on AppSettings files

* "**LedgerSQLDatabase**" => Connection string to the SQL Database for the appropriate environment.
* "**MainOrganizerRoleId**" and "**StaffRoleId**" => id's in the Database of the Main Organizer and Staff organization roles, in the OrganizationRole table. 
* "**AccessExpireMinutes**" => How long a users session can last without any activity


## 🔼 Deployment to Azure

This API currently hosted on Microsoft Azure, and can be accessed via the following link: https://my-ledger-api.azurewebsites.net

If you want to deploy the project as a static-web-page on Azure, you can follow the [official documentation](https://learn.microsoft.com/en-us/azure/static-web-apps/get-started-portal?tabs=vanilla-javascript&pivots=github)
