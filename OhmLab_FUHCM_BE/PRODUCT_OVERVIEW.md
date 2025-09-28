# Product Overview - OhmLab Management System

## 🎯 Product Overview
OhmLab là hệ thống quản lý phòng thí nghiệm điện tử toàn diện, giúp giảng viên và sinh viên quản lý lịch thực hành, chấm điểm, và báo cáo một cách hiệu quả.

## 📱 User Interfaces
```
┌─────────────────────────────────────────────────────────────┐
│                    USER INTERFACES                         │
├─────────────────────┬───────────────────────────────────────┤
│   WEB PORTAL        │      MOBILE APP                      │
│   (NextJS)          │      (React Native)                 │
│                     │                                      │
│  ┌─────────────┐    │  ┌─────────────┐  ┌─────────────┐    │
│  │  Lecturer   │    │  │  Lecturer   │  │   Student   │    │
│  │  Dashboard  │    │  │   Mobile    │  │    Mobile   │    │
│  └─────────────┘    │  └─────────────┘  └─────────────┘    │
│  ┌─────────────┐    │                                      │
│  │   Student   │    │                                      │
│  │  Portal     │    │                                      │
│  └─────────────┘    │                                      │
└─────────────────────┴───────────────────────────────────────┘
```

## 🏗️ System Architecture (Tinh gọn)
```
┌─────────────────────────────────────────────────────────────┐
│                    CLIENT LAYER                            │
│              (NextJS + React Native)                      │
└─────────────────────────────────────────────────────────────┘
                                │
                                │ HTTPS/REST API
                                │ JWT Auth
                                │
┌─────────────────────────────────────────────────────────────┐
│                   BACKEND LAYER                             │
│                (C# .NET Web API)                           │
│                                                             │
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐       │
│  │ Controllers │  │  Services   │  │ Repositories│       │
│  │   (APIs)    │  │ (Business)  │  │  (Data)     │       │
│  └─────────────┘  └─────────────┘  └─────────────┘       │
└─────────────────────────────────────────────────────────────┘
                                │
                                │ Entity Framework
                                │
┌─────────────────────────────────────────────────────────────┐
│                  DATABASE LAYER                             │
│                 (SQL Server - SmarterASP)                  │
│                                                             │
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐       │
│  │    Users    │  │   Classes   │  │    Labs     │       │
│  │   Classes   │  │  Schedules  │  │   Grades    │       │
│  │   Reports   │  │   Teams     │  │ ScheduleTypes│      │
│  └─────────────┘  └─────────────┘  └─────────────┘       │
└─────────────────────────────────────────────────────────────┘
```

## 🔄 Core Business Flows

### 1. 🔐 Authentication & Authorization Flow
```
User Login → Validate Credentials → Generate JWT Token → 
Store Token → Access Protected Resources → Role-based Authorization
```

### 2. 📅 Lab Scheduling Flow (Giảng viên)
```
Lecturer → Select Class → Select Lab → Choose Date/Time → 
Conflict Check → Create Schedule → Notify Students
```
**Validation Steps:**
- ✅ Lecturer owns the class
- ✅ Lab belongs to class subject
- ✅ No scheduling conflicts
- ✅ Schedule type exists

### 3. 📊 Grade Management Flow
```
Lecturer → Select Class → View Students → Input Grades → 
Save Grades → Students View Grades → Generate Reports
```
**Features:**
- Team-based grading
- Individual grading override
- Grade statistics
- Export functionality

### 4. 📝 Report Creation Flow (Sinh viên/Giảng viên)
```
User → Select Today's Schedule → Create Report → 
Add Content/Images → Submit → Lecturer Review → Approve/Reject
```
**Access Control:**
- Students: Only enrolled classes
- Lecturers: Only teaching classes
- Today's schedules only

### 5. 👥 Team Management Flow
```
Lecturer → Create Teams → Assign Students → Set Team Leaders → 
Team Collaboration → Grade by Team
```

## 🎯 Key Features by User Role

### 👨‍🏫 Lecturer Features
- **Lịch Lab:** Tạo và quản lý lịch thực hành
- **Chấm điểm:** Chấm điểm cá nhân/nhóm
- **Quản lý lớp:** Phân công sinh viên vào nhóm
- **Xem báo cáo:** Duyệt và phản hồi báo cáo
- **Thống kê:** Xem báo cáo tổng kết

### 👨‍🎓 Student Features
- **Xem lịch:** Lịch học cá nhân và nhóm
- **Xem điểm:** Điểm các môn học
- **Tạo báo cáo:** Nộp báo cáo thực hành
- **Nhóm làm việc:** Thông tin nhóm và thành viên
- **Thông báo:** Nhận thông báo mới

## 🔒 Security & Access Control
```
┌─────────────────────────────────────────────────────────────┐
│                    SECURITY LAYER                           │
│                                                             │
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐       │
│  │  JWT Auth   │  │   Role-Based│  │   Resource  │       │
│  │             │  │  Access Ctrl│  │  Ownership  │       │
│  └─────────────┘  └─────────────┘  └─────────────┘       │
│                                                             │
│  Lecturer → Only own classes                                │
│  Student  → Only enrolled classes                           │
│  Admin    → Full system access                              │
└─────────────────────────────────────────────────────────────┘
```

## 📊 Data Flow Summary
```
User Input → API Gateway → Authentication → Authorization → 
Business Logic → Data Validation → Database Operations → 
Response → Notification → User Interface Update
```

## 🚀 Technology Stack
| Component | Technology | Purpose |
|-----------|------------|---------|
| **Frontend Web** | NextJS, React | Lecturer/Student Web Portal |
| **Frontend Mobile** | React Native | Cross-platform Mobile App |
| **Backend API** | C#, .NET 8 | Business Logic & APIs |
| **Database** | SQL Server | Data Persistence (SmarterASP) |
| **Authentication** | JWT Tokens | Secure User Authentication |
| **Deployment** | Azure/SmarterASP | Cloud Hosting & Deployment |

## 🎯 Product Value Proposition

### For Lecturers:
- **Tiết kiệm thời gian:** Tự động hóa quản lý lịch và chấm điểm
- **Tổ chức hiệu quả:** Quản lý nhiều lớp và nhóm dễ dàng
- **Minh bạch:** Theo dõi tiến độ sinh viên real-time

### For Students:
- **Tiện lợi:** Truy cập lịch và điểm mọi lúc mọi nơi
- **Hợp tác:** Làm việc nhóm hiệu quả
- **Phản hồi nhanh:** Nhận điểm và phản hồi kịp thời

### For Institution:
- **Quản lý tập trung:** Toàn bộ hệ thống lab trên một nền tảng
- **Báo cáo tự động:** Thống kê và báo cáo tổng kết
- **Mở rộng dễ dàng:** Architecture hỗ trợ scaling

---
*OhmLab - Nền tảng quản lý phòng thí nghiệm thông minh cho giáo dục đại học*
