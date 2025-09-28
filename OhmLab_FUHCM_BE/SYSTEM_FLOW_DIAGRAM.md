# System Flow Diagram - OhmLab Management System

## 🔄 System Flow Overview

```
                    ┌─────────────────────────────────────────────────┐
                    │              EXTERNAL SYSTEMS                   │
                    │                                                 │
                    │  ┌─────────────┐      ┌─────────────┐          │
                    │  │   Email     │      │  File       │          │
                    │  │  Service    │◄────►│  Storage    │          │
                    │  └─────────────┘      │  (Azure/S3) │          │
                    │                      └─────────────┘          │
                    └─────────────────────────────────────────────────┘
                                    ▲               ▲
                                    │               │
                                    │               │
                    ┌───────────────┼───────────────┼───────────────┐
                    │               │               │               │
                    │               │               │               │
        ┌───────────▼──────┐       │       ┌───────▼───────────┐   │
        │                   │       │       │                   │   │
        │  ┌─────────────┐  │       │       │  ┌─────────────┐  │   │
        │  │   Mobile    │  │       │       │  │   Web       │  │   │
        │  │   App       │  │       │       │  │  Portal     │  │   │
        │  │(React Native│  │       │       │  │  (NextJS)   │  │   │
        │  │   iOS/Android│ │       │       │  │             │  │   │
        │  └─────────────┘  │       │       │  └─────────────┘  │   │
        │                   │       │       │                   │   │
        │    STUDENTS       │       │       │     LECTURERS     │   │
        │    & LECTURERS    │       │       │                   │   │
        └───────────────────┘       │       └───────────────────┘   │
                    │               │               │               │
                    │               │               │               │
                    │    HTTPS/REST API              │               │
                    │    JWT Authentication          │               │
                    │               │               │               │
                    └───────────────┼───────────────┼───────────────┘
                                    │               │
                                    │               │
                    ┌───────────────▼───────────────▼───────────────┐
                    │                                               │
                    │            ┌─────────────────┐                │
                    │            │                 │                │
                    │            │   O H M L A B   │                │
                    │            │    S Y S T E M  │                │
                    │            │                 │                │
                    │            │                 │                │
                    │            │  ┌───────────┐  │                │
                    │            │  │  C# .NET  │  │                │
                    │            │  │  Web API  │  │                │
                    │            │  └───────────┘  │                │
                    │            │                 │                │
                    │            └─────────────────┘                │
                    │                                               │
                    └───────────────┬───────────────┬───────────────┘
                                    │               │
                                    │               │
                    ┌───────────────▼───────────────▼───────────────┐
                    │                                               │
                    │              BUSINESS LOGIC                    │
                    │                                               │
                    │  ┌─────────────┐  ┌─────────────┐  ┌───────┐ │
                    │  │   Auth &    │  │   Lab       │  │ Grade │ │
                    │  │   Authz     │  │ Scheduling  │  │ Mgmt  │ │
                    │  └─────────────┘  └─────────────┘  └───────┘ │
                    │                                               │
                    │  ┌─────────────┐  ┌─────────────┐  ┌───────┐ │
                    │  │   Report    │  │   Team      │  │ Class │ │
                    │  │  Management │  │ Management  │  │ Mgmt  │ │
                    │  └─────────────┘  └─────────────┘  └───────┘ │
                    │                                               │
                    └───────────────┬───────────────┬───────────────┘
                                    │               │
                                    │               │
                    ┌───────────────▼───────────────▼───────────────┐
                    │                                               │
                    │              DATA LAYER                       │
                    │                                               │
                    │  ┌─────────────┐  ┌─────────────┐  ┌───────┐ │
                    │  │   Entity    │  │   Repos     │  │ Unit  │ │
                    │  │ Framework   │  │ Pattern     │  │ of    │ │
                    │  │   Core      │  │             │  │ Work  │ │
                    │  └─────────────┘  └─────────────┘  └───────┘ │
                    │                                               │
                    └───────────────┬───────────────┬───────────────┘
                                    │               │
                                    │               │
                    ┌───────────────▼───────────────▼───────────────┐
                    │                                               │
                    │              DATABASE                         │
                    │         (SQL Server - SmarterASP)            │
                    │                                               │
                    │  ┌─────────────┐  ┌─────────────┐  ┌───────┐ │
                    │  │    Users    │  │   Classes   │  │ Labs  │ │
                    │  └─────────────┘  └─────────────┘  └───────┘ │
                    │                                               │
                    │  ┌─────────────┐  ┌─────────────┐  ┌───────┐ │
                    │  │  Schedules  │  │   Grades    │  │ Teams │ │
                    │  └─────────────┘  └─────────────┘  └───────┘ │
                    │                                               │
                    │  ┌─────────────┐  ┌─────────────┐  ┌───────┐ │
                    │  │   Reports   │  │ScheduleTypes│  │ Slots │ │
                    │  └─────────────┘  └─────────────┘  └───────┘ │
                    │                                               │
                    └───────────────────────────────────────────────┘
```

## 🔄 Detailed Flow Arrows

### 1. **User Interaction Flow**
```
Students/Lecturers 
        ↓
Mobile App/Web Portal
        ↓
[HTTPS/REST API + JWT Auth]
        ↓
OhmLab System (C# .NET Web API)
```

### 2. **Authentication Flow**
```
User Login 
        ↓
Credentials Validation
        ↓
JWT Token Generation
        ↓
Token Storage (Client)
        ↓
Protected Resource Access
        ↓
Role-based Authorization
```

### 3. **Lab Scheduling Flow**
```
Lecturer Request
        ↓
Class Ownership Check
        ↓
Lab-Subject Compatibility Check
        ↓
Schedule Conflict Detection
        ↓
Schedule Creation
        ↓
Student Notification
        ↓
Calendar Update
```

### 4. **Grade Management Flow**
```
Lecturer Grade Input
        ↓
Team/Individual Grade Logic
        ↓
Grade Validation
        ↓
Database Storage
        ↓
Grade Calculation
        ↓
Student Grade View
        ↓
Report Generation
```

### 5. **Report Creation Flow**
```
Student/Lecturer Request
        ↓
Today's Schedule Access Check
        ↓
User Class Enrollment Check
        ↓
Report Creation
        ↓
Content/Attachment Upload
        ↓
Lecturer Review
        ↓
Approval/Rejection
        ↓
Notification System
```

### 6. **Team Management Flow**
```
Lecturer Team Creation
        ↓
Student Assignment
        ↓
Team Leader Assignment
        ↓
Team Collaboration
        ↓
Team-based Grading
        ↓
Team Performance Analytics
```

## 🎯 Key Integration Points

### **API Gateway (Central Hub)**
```
┌─────────────────────────────────────────────────┐
│              API GATEWAY                        │
│                                                 │
│  ┌─────────────┐  ┌─────────────┐  ┌─────────┐ │
│  │   Mobile    │  │    Web      │  │ External│ │
│  │    API      │  │   Portal    │  │ Systems │ │
│  └─────────────┘  └─────────────┘  └─────────┘ │
│                                                 │
│  ┌─────────────┐  ┌─────────────┐  ┌─────────┐ │
│  │   Auth      │  │   Business  │  │   Data   │ │
│  │  Service    │  │   Logic     │  │  Access  │ │
│  └─────────────┘  └─────────────┘  └─────────┘ │
└─────────────────────────────────────────────────┘
```

### **Data Flow Direction**
```
┌─────────────┐    Request     ┌─────────────┐    Process    ┌─────────────┐
│             │ ──────────────→ │             │ ─────────────→ │             │
│   Client    │                │  OhmLab     │                │  Business   │
│   Apps      │                │  System     │                │   Logic     │
│             │ ◀────────────── │             │ ◀───────────── │             │
└─────────────┘    Response     └─────────────┘    Result     └─────────────┘
                                                        │
                                                        │ Data
                                                        ▼
                                                ┌─────────────┐
                                                │             │
                                                │  Database   │
                                                │             │
                                                └─────────────┘
```

## 🔒 Security Flow

```
┌─────────────┐    Credentials    ┌─────────────┐    JWT Token   ┌─────────────┐
│             │ ─────────────────→ │             │ ─────────────→ │             │
│   User      │                │   Auth       │                │   Protected │
│   Login     │                │   Service    │                │   Resource  │
│             │ ◀────────────── │             │ ◀───────────── │             │
└─────────────┘    JWT Token     └─────────────┘    Validation  └─────────────┘
```

---
*OhmLab System Flow - Centralized architecture with clear data flow and security boundaries*
