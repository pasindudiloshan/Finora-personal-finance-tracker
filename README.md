# Finora 🏦💰

FinoraTracker is a desktop application built with C# (.NET Framework / WinForms) designed to help users efficiently manage their personal finances. The application allows users to track income and expenses, monitor portfolio performance, and gain insights through analytics and reporting features. It also includes an AI-powered chatbot that provides personalized financial advice and suggests investment strategies, enabling users to make smarter financial decisions.

![FinoraTracker Screenshot](https://github.com/pasindudiloshan/Finora-personal-finance-tracker/blob/335e87a6f495493720cd998632a6022970a915ce/FioraMockUpCover.drawio.png)
## 📄 Research Proposal

This application was developed based on academic research aimed at promoting financial literacy among students. The project is grounded in the research proposal:

**“Designing a Personal Finance Tracker to Promote Financial Literacy and Savings Among Students”**

[View System Report](https://drive.google.com/file/d/1ctIrLO8Sdetqt19TBmPSnWDUzFy1XO44/view?usp=sharing)

The research highlights the lack of tailored financial management tools for students, who often face irregular income, limited financial knowledge, and difficulty managing expenses. FinoraTracker was designed to:

- Encourage better budgeting and savings habits  
- Provide AI-driven insights for smarter financial decisions  
- Use gamification and educational features to make financial management engaging  
- Contribute to improving financial literacy in Sri Lanka and beyond  

---

## 🚀 Features

👤 **User Authentication** – Signup, Signin, and Signout functionality  

💵 **Income Management** – CRUD operations for income records with category tracking  

💸 **Expenses Management** – CRUD operations for expense records with category tracking  

📈 **Portfolio Tracker** – Manage multiple investment portfolios, monitor portfolio performance, and calculate profits  

🤖 **AI Chatbot** – Provides market updates, savings strategy recommendations, investment tips, and beginner-friendly YouTube tutorial collections for each investment market  

📊 **Reports & Charts** –  
- Pie charts for spending habits  
- Line charts for selected period income vs expenses  
- Statistical cards for saving goals, overall wealth gain, and investment profit

---

## 🛠️ Technologies Used  

- **Framework:** .NET Framework / WinForms  
- **Programming Language:** C# 9.0+  
- **Architecture:** MVC (Models, Views, Controllers)  
- **UI:** Flat UI Framework (modern, clean design)  
- **Dependencies:** System.Windows.Forms.DataVisualization.Charting (data visualization)  
- **Database:** MySQL  
- **Testing:** NUnit, Moq  
- **IDE:** Visual Studio 2022  

---

## 📥 Installation & Setup  

Follow these steps to run **FinoraTracker** locally on your machine:  

1. **Clone the repository**  
   ```bash
   git clone https://github.com/pasindudiloshan/Finora-personal-finance-tracker.git
   cd Finora-personal-finance-tracker
   
2. **Open in Visual Studio**

- Launch Visual Studio 2022 (or later)
- Open the solution file:
   ```bash
   FinoraTracker.sln

3. **Restore NuGet Packages**

- Go to Tools > NuGet Package Manager > Manage NuGet Packages for Solution

- Restore all dependencies (NUnit, Moq, System.Windows.Forms.DataVisualization, etc.)

4. **Setup Database (MySQL)**

- Install MySQL Server & Workbench if not already installed
- Create a database
- Import the provided schema file:
   ```bash
   /Database/schema.sql
   
5. **Configure Connection String**

- In app.config (or Program.cs depending on your setup), update the connection string with your MySQL username, password, and database name:
   ```bash
   <connectionStrings>
    <add name="FinoraDB" 
         connectionString="Server=localhost;Database=finora_db;User Id=root;Password=yourpassword;" 
         providerName="MySql.Data.MySqlClient" />
</connectionStrings>

6. **Build & Run the Application**

- Press F5 or click Start Debugging in Visual Studio
- The FinoraTracker dashboard will launch

---

## 📂 Project Structure

   ```bash
├─ FinoraTracker.sln 
├─ FinoraTracker/ 
│ ├─ Program.cs 
│ ├─ App.config # 
│ │
│ ├─ Properties/ 
│ │ └─ AssemblyInfo.cs
│ │
│ ├─ Resources/ 
│ │ ├─ logo.png
│ │ └─ ...
│ │
│ ├─ Forms/ 
│ │ ├─ Login.cs / 
│ │ ├─ Register.cs / 
│ │ ├─ Dashboard.cs / 
│ │ ├─ ViewExpenses.cs / 
│ │ ├─ AddExpense.cs / 
│ │ ├─ Reports.cs / 
│ │ └─ ...
│ │
│ ├─ Models/ 
│ │ ├─ User.cs
│ │ ├─ Expense.cs
│ │ └─ Income.cs
│ │ └─ ...
│ │
│ ├─ DAOs/
│ │ ├─ UserDAO.cs 
│ │ └─ ExpenseDAO.cs
│ │ └─ ...
│ │
│ ├─ Utils/
│ │ ├─ DBConnection.cs 
│ │ └─ Helpers.cs
│ │
│ ├─ Controls/ 
│ │ ├─ ExpenseItemControl.cs
│ │ └─ ...
│ │
│ ├─ Tests/ 
│ │ ├─ UserDAOTest
│ │ ├─ ExpenseDAOTests
│ │ └─ ...

│ └─ Program.cs
