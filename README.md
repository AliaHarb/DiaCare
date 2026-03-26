# 🩺 DiaCare - Smart Support System for Diabetes Patients

DiaCare is a system to support diabetes patients by tracking health data, predicting risks using AI, and providing educational resources.

---

## Features

### Currently Implemented
- **Secure Authentication:** Full Identity system with JWT for Register and Login.
- **Smart User Profile:** Fetch and manage personal health data (Name, Age, Gender) securely using bearer tokens.
- **Infrastructure Ready:** Structured Clean Architecture with Generic Repository and Unit of Work patterns.

### In Progress
- **AI Prediction Engine:** Users can input health data (glucose level, blood pressure, BMI) to predict risk of chronic diseases.

### Planned / Future Features
- **Progress Tracking:** Monitor health trends over time with visual charts and summary statistics.
- **Health Tips Section:** Provide personalized health tips based on user data and predictions.
- **Notification System:** Notify users about important updates, reminders, and health insights.
- **General Feedback Page:** Public feedback form for comments and experience rating.
- **Gamification Basics:** Award points or badges for completing activities and achieving goals.
- **AI Chatbot Integration:** External AI chatbot for instant answers and health guidance.
- **Adaptive Feedback System:** Generate periodic feedback based on user activity and data trends.
- **Smart Diet Suggestions:** Provide dietary recommendations, starting rule-based and expanding to AI-based.
- **Leaderboard:** Compare user achievements or health engagement scores.
- **Community / Support Section:** Forum for users to share experiences and tips.
  
---
## 🏗️ Architecture

The project follows **Clean Architecture**:

- **Domain** → Entities & Interfaces  
- **Application** → Services, Business Logic, DTOs  
- **Infrastructure** → Data Access, Repositories  
- **WebAPI** → Controllers & Endpoints

**Benefits:** Scalability, maintainability, testability, separation of concerns

---

## 🧩 Design Patterns Implemented

- **Repository Pattern (Generic)** → abstracts data access  
- **Unit of Work** → coordinates multiple repositories  
- **Dependency Injection (DI)** → decouples layers & manages lifetimes  
- **Facade Pattern** → Service layer (AuthService, ProfileService) provides simple interface to controllers  
- **DTO Pattern** → RegisterDto, UserProfileDto, PredictionInputDto  
- **Adapter Pattern** → planned for AI model integration  
- **Singleton** → Configuration (JWT settings), caching services  

---

## 🛠️ Tech Stack

- **Backend:** .NET 8 Web API  
- **Database:** SQL Server (LocalDB)  
- **ORM:** Entity Framework Core  
- **Authentication:** ASP.NET Core Identity + JWT  
- **Mapping:** AutoMapper  

---

##  Future Work

- Integrate AI Prediction Model  
- Add Role-based Authorization  
- Add Health Articles & Tips  
- Potential mobile app integration  

---


