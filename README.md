[![Deployment_Actions](https://github.com/Tejp0306/RAPS-EHRM/actions/workflows/dotnet.yml/badge.svg?branch=actions-runner)](https://github.com/Tejp0306/RAPS-EHRM/actions/workflows/dotnet.yml)

# E-HRM Application  
**Version:** 1.0  
**Technologies:** ASP.NET Core 8.0, Entity Framework Core, MS SQL Server  

## 📌 Overview  
The **E-HRM (Electronic Human Resource Management)** system is a web-based application designed to streamline HR processes such as employee management, leave tracking, probation evaluation, and timesheet management. It provides an intuitive and secure platform for HR professionals to manage workforce operations efficiently.  

## 🚀 Features  
- **Employee Management** – Add, update, and manage employee records.  
- **Attendance & Leave Tracking** – Request, approve, and manage employee leave.  
- **Holidays & Leave Calendar** – Maintain a structured calendar for holidays and employee leaves.  
- **Probation Evaluation** – Assess and manage employee probationary periods.  
- **Timesheet Management** – Track and log employee working hours and productivity.  
- **User Roles & Permissions** – Secure role-based access control (Admin, HR, Employee).  
- **Reports & Analytics** – Generate HR reports and insights.  
- **Authentication & Authorization** – Secure login with ASP.NET Identity.  

## 🛠 Tech Stack  
- **Backend:** ASP.NET Core 8.0 (C#)  
- **Frontend:** Razor Pages / MVC  
- **Database:** Microsoft SQL Server (Entity Framework Core)  
- **Authentication:** ASP.NET Identity / JWT  
- **Deployment:** IIS / Azure / Docker 

## 🔧 Installation & Setup  

### **Prerequisites**  
Ensure you have the following installed:  
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
- [SQL Server](https://www.microsoft.com/en-us/sql-server)  
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) (or VS Code with C# Extension)  

### **Step 1: Clone the Repository**  
```
git clone https://github.com/your-repo/E-HRM.git
cd RAPS-EHRM
```

## Step 2: Configure the Database  
### Update Connection String  
Open the `appsettings.json` file and update the database connection string with your SQL Server details:  

```
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=EHRM_DB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

### Apply Database Migrations  
Open a terminal or command prompt in the project root folder and run:  

```
dotnet ef database update
```

This will create the required tables in the database.  

### Verify Database Setup  
- Open **SQL Server Management Studio (SSMS)** or another database tool.  
- Connect to your SQL Server and ensure that the `EHRM_DB` database has been created with the necessary tables.  

## Step 3: Run the Application  
```
dotnet run
```
OR use Visual Studio’s **Run** button.  

## 🚀 Deployment  
For deployment, configure **IIS, Azure, or Docker** as per your hosting preference.  

## 🤝 Contributing  
Feel free to fork the repo and submit pull requests!  

## 📜 License  
MIT License (or specify your license).
