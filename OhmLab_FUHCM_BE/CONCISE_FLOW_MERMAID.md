# OhmLab System Flow - Concise Version

```mermaid
graph TD
    %% Users & Interfaces
    Students["Students"]
    Lecturers["Lecturers"]
    MobileApp["Mobile App"]
    WebPortal["Web Portal"]
    
    %% Core System
    OhmLab["OhmLab System"]
    
    %% Main Services
    AuthSvc["Authentication"]
    LabSvc["Lab Scheduling"]
    GradeSvc["Grade Management"]
    ReportSvc["Report Management"]
    TeamSvc["Team Management"]
    
    %% Key Processes
    ScheduleCreate["Create Schedule"]
    GradeSave["Save Grade"]
    ReportCreate["Create Report"]
    TeamCreate["Create Team"]
    
    %% Storage
    Database["Database"]
    
    %% External Services
    EmailService["Email Service"]
    
    %% Main Flow
    Students --> MobileApp
    Lecturers --> WebPortal
    MobileApp --> OhmLab
    WebPortal --> OhmLab
    
    OhmLab --> AuthSvc
    AuthSvc --> Database
    
    %% Lab Scheduling Flow
    OhmLab --> LabSvc
    LabSvc --> ScheduleCreate
    ScheduleCreate --> Database
    
    %% Grade Management Flow
    OhmLab --> GradeSvc
    GradeSvc --> GradeSave
    GradeSave --> Database
    
    %% Report Management Flow
    OhmLab --> ReportSvc
    ReportSvc --> ReportCreate
    ReportCreate --> Database
    ReportCreate --> EmailService
    
    %% Team Management Flow
    OhmLab --> TeamSvc
    TeamSvc --> TeamCreate
    TeamCreate --> Database
    
    %% Styling
    classDef users fill:#e8f5e8,stroke:#1b5e20,stroke-width:2px
    classDef interfaces fill:#f3e5f5,stroke:#4a148c,stroke-width:2px
    classDef system fill:#e1f5fe,stroke:#01579b,stroke-width:3px
    classDef services fill:#e3f2fd,stroke:#1565c0,stroke-width:2px
    classDef processes fill:#f1f8e9,stroke:#33691e,stroke-width:2px
    classDef storage fill:#e0f2f1,stroke:#004d40,stroke-width:2px
    classDef external fill:#fce4ec,stroke:#880e4f,stroke-width:2px
    
    class Students,Lecturers users
    class MobileApp,WebPortal interfaces
    class OhmLab system
    class AuthSvc,LabSvc,GradeSvc,ReportSvc,TeamSvc services
    class ScheduleCreate,GradeSave,ReportCreate,TeamCreate processes
    class Database storage
    class EmailService external
```

---

**Cách sử dụng:** Copy code mermaid và paste vào công cụ hỗ trợ mermaid (GitHub, GitLab, Notion, VS Code với extension mermaid).

**Đặc điểm:**
- Flow gọn gàng, tập trung vào các thành phần chính
- Màu sắc phân biệt rõ ràng
- Dễ theo dõi luồng xử lý
- Bao gồm 4 module chính: Lab Scheduling, Grade Management, Report Management, Team Management
