# HotelManagement
## Description: 
A simple hotel management application that has two portals:

**Web portal** - the web portal allows people to book an available room through the browser. 

**Desktop portal** - the desktop portal allows people to check in once they have booked a room. It will look up the guest information and check them in based on that. 

I have used **ASP.NET Core Razor Pages** for the web portal and **WPF Core** for the desktop portal. The two portals are connected to **SQL Server** database which stores the guest data. 

## Application Design:
So, the applications consists of three different layers:

Database (SQL Server) <-->  Class Library (Data Access) <--> User Interfaces (Web portal and Desktop portal) 

• First Layer - Database. I used SQL Server for storing and managing the data. Later, I added SQLite option. The program picks which database to use based on the connection string provided.


## Reminder: 
This application is a minimum viable produvt (MVP) which means that there can/will be updates in the future! 
