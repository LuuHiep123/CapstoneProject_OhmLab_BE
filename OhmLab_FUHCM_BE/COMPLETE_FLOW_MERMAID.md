# Complete OhmLab System Flow - Mermaid Code

```mermaid
graph TD
    %% Users
    Students["Students"]
    Lecturers["Lecturers"]
    
    %% User Interfaces
    MobileApp["Mobile App<br/>(React Native)"]
    WebPortal["Web Portal<br/>(NextJS)"]
    
    %% Central System
    OhmLab[("OhmLab System<br/>C# .NET Web API")]
    
    %% Authentication Flow
    Login["Login Request"]
    AuthSvc["Auth Service"]
    JWT["JWT Token<br/>Generation"]
    TokenStore["Token Storage<br/>(Client)"]
    
    %% Business Logic Modules
    LabScheduling["Lab Scheduling<br/>Service"]
    GradeManagement["Grade Management<br/>Service"]
    ReportManagement["Report Management<br/>Service"]
    TeamManagement["Team Management<br/>Service"]
    
    %% Lab Scheduling Flow Details
    OwnershipCheck["Class Ownership<br/>Validation"]
    LabCheck["Lab-Subject<br/>Compatibility"]
    ConflictCheck["Schedule Conflict<br/>Detection"]
    ScheduleCreate["Schedule Creation"]
    StudentNotify["Student<br/>Notification"]
    
    %% Grade Management Flow Details
    GradeInput["Grade Input<br/>Request"]
    TeamCheck["Team-based<br/>Grading Check"]
    GradeValidation["Grade<br/>Validation"]
    GradeSave["Save Grades<br/>to Database"]
    GradeCalc["Grade<br/>Calculation"]
    StudentView["Student Grade<br/>View"]
    ReportGen["Grade Report<br/>Generation"]
    
    %% Report Management Flow Details
    ReportRequest["Create Report<br/>Request"]
    DateCheck["Today's Schedule<br/>Check"]
    AccessCheck["User Access<br/>Validation"]
    ReportCreate["Report Creation"]
    ContentUpload["Content/Attachment<br/>Upload"]
    LecturerReview["Lecturer<br/>Review"]
    Approval["Approval/<br/>Rejection"]
    ReportNotify["Report<br/>Notification"]
    
    %% Team Management Flow Details
    TeamCreate["Team Creation<br/>Request"]
    StudentAssign["Student<br/>Assignment"]
    LeaderAssign["Team Leader<br/>Assignment"]
    TeamCollab["Team<br/>Collaboration"]
    TeamGrading["Team-based<br/>Grading"]
    TeamAnalytics["Team Performance<br/>Analytics"]
    
    %% Database
    Database["Database<br/>(SQL Server)"]
    
    %% External Systems
    EmailService["Email Service"]
    FileStorage["File Storage<br/>(Azure/S3)"]
    
    %% User Connections
    Students --> MobileApp
    Students --> WebPortal
    Lecturers --> MobileApp
    Lecturers --> WebPortal
    
    %% Authentication Flow
    MobileApp --> Login
    WebPortal --> Login
    Login --> AuthSvc
    AuthSvc --> JWT
    JWT --> TokenStore
    TokenStore -->|"Bearer Token"| OhmLab
    
    %% Main API Connections
    MobileApp -->|"HTTPS/REST API<br/>JWT Auth"| OhmLab
    WebPortal -->|"HTTPS/REST API<br/>JWT Auth"| OhmLab
    
    %% System to Business Logic
    OhmLab --> LabScheduling
    OhmLab --> GradeManagement
    OhmLab --> ReportManagement
    OhmLab --> TeamManagement
    
    %% Lab Scheduling Flow
    Lecturers -->|"Schedule Request"| LabScheduling
    LabScheduling --> OwnershipCheck
    OwnershipCheck -->|"Valid"| LabCheck
    LabCheck -->|"Compatible"| ConflictCheck
    ConflictCheck -->|"No Conflict"| ScheduleCreate
    ScheduleCreate --> StudentNotify
    StudentNotify --> Students
    ScheduleCreate --> Database
    
    %% Lab Scheduling Error Paths
    OwnershipCheck -->|"Invalid: 403 Forbidden"| Lecturers
    LabCheck -->|"Incompatible: 400 Bad Request"| Lecturers
    ConflictCheck -->|"Conflict Found: 400 Bad Request"| Lecturers
    
    %% Grade Management Flow
    Lecturers -->|"Grade Input"| GradeManagement
    GradeManagement --> GradeInput
    GradeInput --> TeamCheck
    TeamCheck -->|"Team Grade"| GradeValidation
    TeamCheck -->|"Individual Grade"| GradeValidation
    GradeValidation -->|"Valid"| GradeSave
    GradeSave --> GradeCalc
    GradeCalc --> StudentView
    StudentView --> Students
    GradeCalc --> ReportGen
    GradeSave --> Database
    
    %% Grade Management Error Path
    GradeValidation -->|"Invalid: 400 Bad Request"| Lecturers
    
    %% Report Management Flow
    Students -->|"Report Request"| ReportManagement
    Lecturers -->|"Report Request"| ReportManagement
    ReportManagement --> ReportRequest
    ReportRequest --> DateCheck
    DateCheck -->|"Today"| AccessCheck
    AccessCheck -->|"Valid Access"| ReportCreate
    ReportCreate --> ContentUpload
    ContentUpload --> FileStorage
    ContentUpload --> LecturerReview
    LecturerReview --> Approval
    Approval --> ReportNotify
    ReportNotify --> EmailService
    ReportCreate --> Database
    
    %% Report Management Error Paths
    DateCheck -->|"Not Today: 400 Bad Request"| Students
    DateCheck -->|"Not Today: 400 Bad Request"| Lecturers
    AccessCheck -->|"Invalid Access: 403 Forbidden"| Students
    AccessCheck -->|"Invalid Access: 403 Forbidden"| Lecturers
    
    %% Team Management Flow
    Lecturers -->|"Team Management"| TeamManagement
    TeamManagement --> TeamCreate
    TeamCreate --> StudentAssign
    StudentAssign --> LeaderAssign
    LeaderAssign --> TeamCollab
    TeamCollab --> TeamGrading
    TeamGrading --> TeamAnalytics
    TeamGrading --> GradeManagement
    StudentAssign --> Database
    TeamCreate --> Database
    
    %% Database Connections
    LabScheduling --> Database
    GradeManagement --> Database
    ReportManagement --> Database
    TeamManagement --> Database
    
    %% External System Connections
    OhmLab --> EmailService
    OhmLab --> FileStorage
    StudentNotify --> EmailService
    ReportNotify --> EmailService
    
    %% Styling
    classDef users fill:#e8f5e8,stroke:#1b5e20,stroke-width:2px
    classDef interfaces fill:#f3e5f5,stroke:#4a148c,stroke-width:2px
    classDef centralSystem fill:#e1f5fe,stroke:#01579b,stroke-width:4px
    classDef auth fill:#fff3e0,stroke:#e65100,stroke-width:2px
    classDef businessLogic fill:#e3f2fd,stroke:#1565c0,stroke-width:2px
    classDef process fill:#f1f8e9,stroke:#33691e,stroke-width:2px
    classDef validation fill:#ffecb3,stroke:#ff6f00,stroke-width:2px
    classDef error fill:#ffebee,stroke:#c62828,stroke-width:2px,stroke-dasharray: 5 5
    classDef database fill:#e0f2f1,stroke:#004d40,stroke-width:2px
    classDef external fill:#fce4ec,stroke:#880e4f,stroke-width:2px
    
    class Students,Lecturers users
    class MobileApp,WebPortal interfaces
    class OhmLab centralSystem
    class AuthSvc,JWT,TokenStore auth
    class LabScheduling,GradeManagement,ReportManagement,TeamManagement businessLogic
    class ScheduleCreate,GradeSave,ReportCreate,TeamCreate,StudentAssign,LeaderAssign process
    class OwnershipCheck,LabCheck,ConflictCheck,GradeValidation,DateCheck,AccessCheck validation
    class Lecturers,Students error
    class Database database
    class EmailService,FileStorage external
```

---

**Cách sử dụng:** Copy toàn bộ code mermaid bên trên và paste vào bất kỳ công cụ nào hỗ trợ mermaid (GitHub, GitLab, Notion, VS Code với extension mermaid, v.v.) để hiển thị sơ đồ flow hoàn chỉnh của hệ thống OhmLab.

**Đặc điểm diagram:**
- Tất cả các luồng đi được tích hợp trong một diagram duy nhất
- Màu sắc phân biệt rõ ràng các loại components
- Luồng success và error được thể hiện riêng biệt
- Các mối quan hệ giữa các modules được hiển thị đầy đủ
