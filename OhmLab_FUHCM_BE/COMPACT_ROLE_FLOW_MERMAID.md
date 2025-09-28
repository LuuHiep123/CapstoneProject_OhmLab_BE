# OhmLab System Flow - Compact Role Version

```mermaid
graph LR
    %% User Roles
    Admin[🔧 Admin]
    HeadOfDept[👔 Head of Dept]
    Lecturer[👨‍🏫 Lecturer]
    Student[👨‍🎓 Student]
    
    %% System
    OhmLab[💻 OhmLab System]
    
    %% Admin Functions
    A1[Create Admin Account]
    A2[Block/Unblock User]
    A3[Full System Analytics]
    
    %% Head of Department Functions
    H1[Create Lab Template]
    H2[Create Head of Dept]
    H3[Create Student Account]
    H4[Create/Update Class]
    H5[Manage Subject]
    H6[Department Analytics]
    
    %% Lecturer Functions
    L1[Booking Lab Schedule]
    L2[Grade Team Lab]
    L3[Grade Individual]
    L4[Create Report]
    L5[Create/Update Team]
    L6[View Class Labs]
    L7[View Personal Analytics]
    
    %% Student Functions
    S1[View Dashboard]
    S2[Check Schedule]
    S3[Check Grades]
    S4[Submit Report]
    S5[View Team Info]
    S6[View Lab Instructions]
    
    %% Connections
    Admin --> OhmLab
    HeadOfDept --> OhmLab
    Lecturer --> OhmLab
    Student --> OhmLab
    
    %% Admin Flow
    Admin --> A1
    Admin --> A2
    Admin --> A3
    
    %% Head of Department Flow
    HeadOfDept --> H1
    HeadOfDept --> H2
    HeadOfDept --> H3
    HeadOfDept --> H4
    HeadOfDept --> H5
    HeadOfDept --> H6
    
    %% Lecturer Flow
    Lecturer --> L1
    Lecturer --> L2
    Lecturer --> L3
    Lecturer --> L4
    Lecturer --> L5
    Lecturer --> L6
    Lecturer --> L7
    
    %% Student Flow
    Student --> S1
    Student --> S2
    Student --> S3
    Student --> S4
    Student --> S5
    Student --> S6
    
    %% All functions connect to system
    A1 --> OhmLab
    A2 --> OhmLab
    A3 --> OhmLab
    H1 --> OhmLab
    H2 --> OhmLab
    H3 --> OhmLab
    H4 --> OhmLab
    H5 --> OhmLab
    H6 --> OhmLab
    L1 --> OhmLab
    L2 --> OhmLab
    L3 --> OhmLab
    L4 --> OhmLab
    L5 --> OhmLab
    L6 --> OhmLab
    L7 --> OhmLab
    S1 --> OhmLab
    S2 --> OhmLab
    S3 --> OhmLab
    S4 --> OhmLab
    S5 --> OhmLab
    S6 --> OhmLab
    
    %% Styling
    classDef roles fill:#e8f5e8,stroke:#1b5e20,stroke-width:3px,font-size:14px
    classDef system fill:#e1f5fe,stroke:#01579b,stroke-width:3px,font-size:16px
    classDef admin fill:#fff3e0,stroke:#e65100,stroke-width:2px,font-size:12px
    classDef headofdept fill:#f1f8e9,stroke:#558b2f,stroke-width:2px,font-size:12px
    classDef lecturer fill:#f3e5f5,stroke:#4a148c,stroke-width:2px,font-size:12px
    classDef student fill:#e8f5fd,stroke:#0277bd,stroke-width:2px,font-size:12px
    
    class Admin,HeadOfDept,Lecturer,Student roles
    class OhmLab system
    class A1,A2,A3 admin
    class H1,H2,H3,H4,H5,H6 headofdept
    class L1,L2,L3,L4,L5,L6,L7 lecturer
    class S1,S2,S3,S4,S5,S6 student
```

---

**Cách sử dụng:** Copy code mermaid và paste vào công cụ hỗ trợ mermaid.

**Đặc điểm:**
- Compact layout dễ chụp hình
- Icons trực quan cho từng role
- Font size optimized cho document
- Flow gọn gàng, không rườm rà
- Phân quyền rõ ràng theo codebase thực tế
