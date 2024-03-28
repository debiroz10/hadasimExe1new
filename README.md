The Corona Foundation Management System is a project designed for a large hospital fund to manage member data and central information related to the COVID-19 epidemic. The system enables the management of member records, including editing and deletion, and stores essential information such as vaccination records, dates of illness and dates of recovery in a SQL database.
Prerequisites

Before running the app, make sure you have the following installed:
NET Core SDK
SQL Server (or other compatible SQL database)

Installation steps
Clone the repository: Open the GitHub repository link in your browser and clone the repository to your local computer using Git or GitHub Desktop.
Open Visual Studio: Start the Visual Studio IDE on your computer.
Open a project: In Visual Studio, navigate to File > Open > Project/Solution, and select the solution file (.sln) from the cloned repository.
  Build the solution by pressing Ctrl + Shift + B, and then run the application by pressing F5 or clicking the Start button in Visual Studio.
Installing a migration to update your current database:

Run the following commands in the Package Manager Console to initialize the database:

Add - Migration InitialCreate
Update-Database


This project relies on the following external services:

Microsoft.AspNetCore.Mvc
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools
Microsoft.Extensions.DependencyInjection
Discounts


The system assumes a simplistic model where each member has a single record for vaccination, illness and recovery dates.
Currently the number of vaccinations is limited to a maximum of four vaccinations per person.
I assumed that the way the vaccines are inserted is according to the order they were received,
 so tests were added during the creation of the vaccine if the current date is not earlier than the existing date.

 
client side

When the site is activated, you will be presented with the main window where there are buttons for the different windows


![image](https://github.com/debiroz10/hadasimExe1new/assets/117022114/d520ce6e-53b7-4cad-a934-6b792bdfc27c)

By clicking on the CLIENT button, a list of all members registered in the HMO will be displayed.
New patients can be manually entered into the system. And an option to upload a photo for a customer.


![image](https://github.com/debiroz10/hadasimExe1new/assets/117022114/869e6e8b-6789-4a58-a831-94ea70c4fce6)


Clicking on a member will open his profile, and will display details such as vaccination records, dates of illness and dates of recovery.
 In addition, each patient will be given the option to add a vaccine to the system and edit the vaccines that have already been entered previously.

 
![image](https://github.com/debiroz10/hadasimExe1new/assets/117022114/6e9cb921-d683-46f1-be50-2d9209635566)


Adding a vaccine:


![image](https://github.com/debiroz10/hadasimExe1new/assets/117022114/f5145e10-9c3e-4a39-aa07-4e5746c7f76f)


All vaccinations of the same client


![image](https://github.com/debiroz10/hadasimExe1new/assets/117022114/494150b1-10e0-4aab-b6b0-7f48de29fd4b)



In addition, there is an option to display a summary of active patients every day of the last month, presented as 
a graph, as well as patients who have not been vaccinated.


![image](https://github.com/debiroz10/hadasimExe1new/assets/117022114/99ed42c3-e1e1-44e7-b652-98b8b366bca0)
![image](https://github.com/debiroz10/hadasimExe1new/assets/117022114/9e5e4bd3-143d-46fa-94d0-f5345b120150)






