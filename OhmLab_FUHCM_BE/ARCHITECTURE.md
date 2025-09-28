# System Architecture - OhmLab Management System

## Tổng quan
Hệ thống quản lý Lab OhmLab được xây dựng theo kiến trúc đa tầng với frontend sử dụng NextJS (web) và React Native (mobile), backend C# (.NET), và database SmarterASP.

## Sơ đồ kiến trúc tổng thể

```
┌─────────────────────────────────────────────────────────────────────────────────┐
│                                  CLIENT LAYER                                   │
├─────────────────────────────────┬───────────────────────────────────────────────┤
│          WEB CLIENT             │            MOBILE CLIENT                     │
│        (NextJS Application)     │        (React Native Application)            │
│                                 │                                               │
│  ┌─────────────┐  ┌───────────┐ │  ┌─────────────┐  ┌─────────────────────┐   │
│  │   Lecturer  │  │  Student  │ │  │   Lecturer  │  │      Student        │   │
│  │   Portal    │  │  Portal   │ │  │   Mobile    │  │      Mobile         │   │
│  └─────────────┘  └───────────┘ │  └─────────────┘  └─────────────────────┘   │
└─────────────────────────────────┴───────────────────────────────────────────────┘
                                         │
                                         │ HTTPS/REST API
                                         │ JWT Authentication
                                         │
┌─────────────────────────────────────────────────────────────────────────────────┐
│                                 API GATEWAY                                     │
│                                  (Optional)                                     │
│                     Load Balancing & Rate Limiting                             │
└─────────────────────────────────────────────────────────────────────────────────┘
                                         │
                                         │
┌─────────────────────────────────────────────────────────────────────────────────┐
│                               BACKEND LAYER                                    │
│                      (C# .NET Web API Application)                            │
│                                                                                 │
│  ┌─────────────────────────────────────────────────────────────────────────┐   │
│  │                        CONTROLLER LAYER                                │   │
│  │  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐  ┌───────────────┐ │   │
│  │  │LabController│  │GradeController│ │ReportController│ │ClassController│ │   │
│  │  └─────────────┘  └─────────────┘  └─────────────┘  └───────────────┘ │   │
│  │  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐  ┌───────────────┐ │   │
│  │  │UserController│ │ScheduleController│ │EquipmentController│ │AuthController │ │   │
│  │  └─────────────┘  └─────────────┘  └─────────────┘  └───────────────┘ │   │
│  └─────────────────────────────────────────────────────────────────────────┘   │
│                                         │                                     │
│  ┌─────────────────────────────────────────────────────────────────────────┐   │
│  │                        SERVICE LAYER                                   │   │
│  │  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐  ┌───────────────┐ │   │
│  │  │  LabService │  │ GradeService│  │ReportService │  │  UserService  │ │   │
│  │  └─────────────┘  └─────────────┘  └─────────────┘  └───────────────┘ │   │
│  │  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐  ┌───────────────┐ │   │
│  │  │ClassService │  │ScheduleService│ │AuthService  │  │EquipmentService│ │   │
│  │  └─────────────┘  └─────────────┘  └─────────────┘  └───────────────┘ │   │
│  └─────────────────────────────────────────────────────────────────────────┘   │
│                                         │                                     │
│  ┌─────────────────────────────────────────────────────────────────────────┐   │
│  │                      REPOSITORY LAYER                                  │   │
│  │  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐  ┌───────────────┐ │   │
│  │  │LabRepository│  │GradeRepository│ │ReportRepository│ │UserRepository │ │   │
│  │  └─────────────┘  └─────────────┘  └─────────────┘  └───────────────┘ │   │
│  │  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐  ┌───────────────┐ │   │
│  │  │ClassRepository│ │ScheduleRepository│ │AuthRepository│ │EquipmentRepository│ │   │
│  │  └─────────────┘  └─────────────┘  └─────────────┘  └───────────────┘ │   │
│  └─────────────────────────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────────────────────────┘
                                         │
                                         │ Entity Framework Core
                                         │ Connection String
                                         │
┌─────────────────────────────────────────────────────────────────────────────────┐
│                              DATABASE LAYER                                     │
│                           (SmarterASP Hosting)                                 │
│                                                                                 │
│  ┌─────────────────────────────────────────────────────────────────────────┐   │
│  │                          DATABASE SCHEMA                                │   │
│  │                                                                         │   │
│  │  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐  ┌───────────────┐ │   │
│  │  │    Users    │  │   Classes   │  │    Labs     │  │   Subjects    │ │   │
│  │  └─────────────┘  └─────────────┘  └─────────────┘  └───────────────┘ │   │
│  │       │              │              │              │                    │   │
│  │  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐  ┌───────────────┐ │   │
│  │  │ ClassUsers  │  │  Schedules  │  │   Grades    │  │  Reports      │ │   │
│  │  └─────────────┘  └─────────────┘  └─────────────┘  └───────────────┘ │   │
│  │       │              │              │              │                    │   │
│  │  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐  ┌───────────────┐ │   │
│  │  │ScheduleTypes│  │    Slots    │  │   Teams     │  │  TeamUsers    │ │   │
│  │  └─────────────┘  └─────────────┘  └─────────────┘  └───────────────┘ │   │
│  │                                                                         │   │
│  └─────────────────────────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────────────────────────┘
```

## Chi tiết các tầng kiến trúc

### 1. Client Layer

#### Web Client (NextJS)
- **Lecturer Portal**: Quản lý lịch lab, chấm điểm, xem báo cáo
- **Student Portal**: Xem lịch học, xem điểm, tạo báo cáo
- **Admin Portal**: Quản lý hệ thống (nếu có)
- **Features**:
  - Authentication & Authorization
  - Responsive Design
  - Real-time updates (SignalR)
  - File upload/download
  - Data visualization (charts, tables)

#### Mobile Client (React Native)
- **Lecturer Mobile**: Quản lý lịch di động, chấm điểm nhanh
- **Student Mobile**: Xem lịch, nhận thông báo, tạo báo cáo
- **Features**:
  - Cross-platform (iOS/Android)
  - Push notifications
  - Offline mode support
  - Camera integration (cho báo cáo)
  - GPS/location services

### 2. Backend Layer (C# .NET Web API)

#### Controller Layer
- **LabController**: Quản lý lịch lab, tạo lịch, kiểm tra xung đột
- **GradeController**: Quản lý điểm số, API lấy điểm lớp
- **ReportController**: Quản lý báo cáo với access control nghiêm ngặt
- **ClassController**: Quản lý lớp học, sinh viên
- **UserController**: Quản lý người dùng, authentication
- **ScheduleController**: Quản lý lịch học, schedule types
- **EquipmentController**: Quản lý thiết bị lab
- **AuthController**: Xử lý đăng nhập, JWT token

#### Service Layer (Business Logic)
- **LabService**: Logic tạo lịch lab, validation xung đột
- **GradeService**: Logic chấm điểm, tính toán điểm trung bình
- **ReportService**: Logic tạo báo cáo, validation quyền truy cập
- **ClassService**: Logic quản lý lớp, phân công giảng dạy
- **UserService**: Logic quản lý user, role-based access
- **ScheduleService**: Logic quản lý schedule types, slots
- **AuthService**: Logic authentication, authorization
- **EquipmentService**: Logic quản lý thiết bị

#### Repository Layer (Data Access)
- **Repository Pattern**: Trừu tượng hóa data access
- **Unit of Work**: Quản lý transactions
- **Entity Framework Core**: ORM framework
- **Connection Management**: Quản lý kết nối database

### 3. Database Layer (SmarterASP)

#### Core Tables
- **Users**: Thông tin người dùng (Lecturer, Student, Admin)
- **Classes**: Thông tin lớp học
- **Subjects**: Thông tin môn học
- **Labs**: Thông tin lab thực hành

#### Relationship Tables
- **ClassUsers**: Sinh viên thuộc lớp nào
- **Schedules**: Lịch học của lớp
- **Grades**: Điểm số của sinh viên
- **Reports**: Báo cáo của sinh viên/giảng viên
- **Teams**: Nhóm sinh viên
- **TeamUsers**: Thành viên nhóm

#### Configuration Tables
- **ScheduleTypes**: Loại lịch học (chứa slot info)
- **Slots**: Ca học (sáng, chiều, tối)
- **Roles**: Quyền hạn người dùng

## Data Flow

### 1. Authentication Flow
```
Client → AuthController → AuthService → JWT Token → Client (store token)
```

### 2. Lab Scheduling Flow
```
Lecturer (Web/Mobile) → LabController → LabService → 
Validation (Ownership, Conflict, Compatibility) → 
ScheduleRepository → Database → Response
```

### 3. Grade Management Flow
```
Lecturer → GradeController → GradeService → 
GradeRepository → Database → 
Student (Web/Mobile) → View Grades
```

### 4. Report Creation Flow
```
Student/Lecturer → ReportController → ReportService → 
Access Validation → ReportRepository → Database → 
Notification → Relevant Users
```

## Security Architecture

### 1. Authentication
- JWT Token-based authentication
- Token expiration and refresh
- Secure password hashing

### 2. Authorization
- Role-based access control (RBAC)
- Resource-level permissions
- Ownership validation

### 3. Data Security
- HTTPS encryption
- Input validation and sanitization
- SQL injection prevention
- XSS protection

### 4. Access Control Patterns
- **Lecturer**: Chỉ truy cập lớp mình phụ trách
- **Student**: Chỉ truy cập lớp mình đăng ký
- **Admin**: Full access (nếu có)

## Deployment Architecture

### 1. Frontend Deployment
- **NextJS**: Vercel hoặc Netlify
- **React Native**: App Store, Google Play Store

### 2. Backend Deployment
- **C# .NET**: Azure App Service hoặc SmarterASP hosting
- **IIS Configuration**: Reverse proxy, SSL termination

### 3. Database Deployment
- **SmarterASP**: Managed SQL Server hosting
- **Backup Strategy**: Daily automated backups
- **Monitoring**: Performance monitoring, alerting

## Integration Points

### 1. External Services
- **Email Service**: Gửi thông báo, xác nhận
- **File Storage**: Azure Blob Storage hoặc AWS S3
- **Notification Service**: Push notifications cho mobile

### 2. Third-party APIs
- **Calendar Integration**: Google Calendar, Outlook
- **Payment Gateway** (nếu có tính năng trả phí)

## Monitoring & Logging

### 1. Application Monitoring
- **Application Insights**: Performance monitoring
- **Health Checks**: Service health monitoring
- **Error Tracking**: Exception logging and alerting

### 2. Database Monitoring
- **Query Performance**: Slow query detection
- **Connection Pooling**: Connection monitoring
- **Backup Monitoring**: Backup status tracking

## Scalability Considerations

### 1. Horizontal Scaling
- Load balancing cho API Gateway
- Database read replicas
- CDN cho static assets

### 2. Vertical Scaling
- Server resource optimization
- Database indexing strategy
- Caching implementation (Redis)

## Technology Stack Summary

| Layer | Technology | Purpose |
|-------|------------|---------|
| Frontend Web | NextJS, React | Web application |
| Frontend Mobile | React Native | Mobile application |
| Backend | C#, .NET 6/7/8 | API and business logic |
| ORM | Entity Framework Core | Database abstraction |
| Database | SQL Server (SmarterASP) | Data persistence |
| Authentication | JWT | Security |
| Deployment | Azure/SmarterASP | Hosting |
| Monitoring | Application Insights | Performance tracking |

Sơ đồ này cung cấp cái nhìn tổng quan về kiến trúc hệ thống OhmLab với các thành phần chính và luồng dữ liệu giữa chúng.
