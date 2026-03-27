# 🩺 DiaCare - Smart Support System for Diabetes Patients

DiaCare is a smart healthcare system designed to support diabetes patients by tracking health data, predicting health risks using AI, and providing educational resources to improve lifestyle awareness.

---

## 📊 | Features

### Currently Implemented

- **Secure Authentication**  
  Full Identity system with JWT-based authentication for Register & Login.

- **Smart User Profile**  
  Manage and retrieve personal health data (Name, Age, Gender) securely using Bearer Tokens.

- **AI Prediction Engine**  
  Users can input health metrics (glucose level, blood pressure, BMI, etc.) to predict risk of chronic diseases using an AI model.

- **Prediction History**  
  Store and track all previous predictions linked to user profiles.

- **Health Articles**  
  Educational content to increase awareness about diabetes management and healthy living.

- **Infrastructure Ready**  
  Built with Clean Architecture using Generic Repository and Unit of Work patterns.

---

###  In Progress

- **📈 Progress Tracking**  
  Visual charts and analytics to monitor health trends over time.

---

###  Planned / Future Features

- **Health Tips Section**  
  Personalized health recommendations based on user data and AI predictions.

- **Notification System**  
  Alerts for reminders, updates, and health insights.

- **General Feedback Page**  
  Public feedback system for user suggestions and experience ratings.

- **Gamification System**  
  Points, badges, and achievements to encourage engagement.

- **AI Chatbot Integration**  
  Smart assistant for instant health-related guidance.

- **Adaptive Feedback System**  
  Periodic insights based on user behavior and data trends.

- **Smart Diet Suggestions**  
  Rule-based → later AI-powered dietary recommendations.

- **Leaderboard**  
  Compare user engagement and health improvement scores.

- **Community / Support Section**  
  Space for users to share experiences and support each other.

---

## 🏗️ Architecture

The project follows **Clean Architecture** principles:

- **Domain** → Entities & Core Interfaces  
- **Application** → Business Logic, Services, DTOs  
- **Infrastructure** → Data Access, Repositories  
- **WebAPI** → Controllers & Endpoints  

###  Benefits
Scalability • Maintainability • Testability • Separation of Concerns

---

## 🧩 Design Patterns Implemented

- **Repository Pattern (Generic)** → abstracts data access layer  
- **Unit of Work** → manages transactions across repositories  
- **Dependency Injection (DI)** → reduces coupling between layers  
- **Facade Pattern** → service layer simplifies business logic access  
- **DTO Pattern** → structured request/response models  
- **Adapter Pattern** → used for AI model integration  
- **Singleton Pattern** → configuration & caching services  

---

## 🛠️ Tech Stack

- **Backend:** .NET 8 Web API  
- **Database:** SQL Server (LocalDB)  
- **ORM:** Entity Framework Core  
- **Authentication:** ASP.NET Core Identity + JWT  
- **Mapping:** AutoMapper  
- **AI Integration:** External ML model via HTTP API  

---
