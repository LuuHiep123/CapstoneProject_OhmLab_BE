# 🧪 HỆ THỐNG QUẢN LÝ LAB - OHMLAB_BE

## 📋 TỔNG QUAN

Hệ thống quản lý lab được thiết kế theo **Functional Requirements** với phân quyền rõ ràng giữa các vai trò:

- **Head of Department**: Thiết lập quy định thực hành và tạo bài lab mẫu
- **Lecturer**: Nhận lớp được phân công và thực hiện lab theo quy định
- **Admin**: Quản lý toàn bộ hệ thống

## 🏗️ KIẾN TRÚC HỆ THỐNG

### **Entities (Giữ nguyên database hiện tại):**
- `Lab`: Bài lab mẫu (Head of Department tạo)
- `Class`: Lớp học với LecturerId
- `Schedule`: Lịch thực hành lab
- `User`: Người dùng với role cụ thể

### **Workflow:**
```
Head of Department → Tạo Lab Mẫu → Lecturer xem Lab → Tạo Lịch → Thực hiện Lab
```

## 🔐 PHÂN QUYỀN CHI TIẾT

### **1. Head of Department:**
- ✅ **Tạo bài lab mẫu** với quy định thực hành
- ✅ **Xem tất cả lab** trong hệ thống
- ✅ **Cập nhật và xóa lab**
- ✅ **Phân công giảng viên** cho lớp học

### **2. Lecturer (Lab Technician):**
- ✅ **Xem lab** cho các lớp mình phụ trách
- ✅ **Tạo lịch lab** cho lớp
- ✅ **Thực hiện lab** theo quy định đã thiết lập
- ✅ **Chấm điểm và phản hồi**

### **3. Admin:**
- ✅ **Toàn quyền** quản lý hệ thống
- ✅ **Xem, tạo, sửa, xóa** tất cả lab
- ✅ **Quản lý người dùng** và phân quyền

## 🚀 API ENDPOINTS

### **Lab Management:**

#### **POST /api/labs** - Tạo bài lab mẫu
```http
POST /api/labs
Authorization: Bearer {token}
Content-Type: application/json

{
  "subjectId": 1,
  "labName": "Đo điện trở",
  "labRequest": "Sinh viên thực hiện đo điện trở theo quy trình...",
  "labTarget": "Hiểu được nguyên lý đo điện trở",
  "requiredSlots": 2,
  "maxStudentsPerLab": 30,
  "estimatedDuration": 120,
  "labDifficulty": "Basic",
  "assignmentType": "Individual",
  "assignmentCount": 1,
  "gradingCriteria": "Chính xác 80%, báo cáo 20%",
  "labStatus": "Active",
  "requiredEquipments": [
    {"equipmentTypeId": "RES001"}
  ],
  "requiredKits": [
    {"kitTemplateId": "KIT001"}
  ]
}
```
**Quyền:** Head of Department

#### **GET /api/labs/my-classes** - Xem lab cho lớp mình phụ trách
```http
GET /api/labs/my-classes
Authorization: Bearer {token}
```
**Quyền:** Lecturer

#### **POST /api/labs/{labId}/schedule** - Tạo lịch lab cho lớp
```http
POST /api/labs/1/schedule
Authorization: Bearer {token}
Content-Type: application/json

{
  "classId": 1,
  "scheduledDate": "2024-01-15T08:00:00",
  "slotId": 1,
  "scheduleDescription": "Lab đo điện trở - Lớp A",
  "maxStudentsPerSession": 30,
  "lecturerNotes": "Chuẩn bị đồng hồ đo"
}
```
**Quyền:** Lecturer

#### **GET /api/labs** - Xem tất cả lab
```http
GET /api/labs
Authorization: Bearer {token}
```
**Quyền:** Admin, Head of Department

#### **GET /api/labs/{id}** - Xem chi tiết lab
```http
GET /api/labs/1
Authorization: Bearer {token}
```
**Quyền:** Admin, Head of Department, Lecturer

#### **PUT /api/labs/{id}** - Cập nhật lab
```http
PUT /api/labs/1
Authorization: Bearer {token}
Content-Type: application/json

{
  "labName": "Đo điện trở (Cập nhật)",
  "labRequest": "Quy trình mới...",
  "labTarget": "Mục tiêu mới...",
  "labStatus": "Active"
}
```
**Quyền:** Admin, Head of Department

#### **DELETE /api/labs/{id}** - Xóa lab
```http
DELETE /api/labs/1
Authorization: Bearer {token}
```
**Quyền:** Admin, Head of Department

### **Lab Queries:**

#### **GET /api/labs/subject/{subjectId}** - Xem lab theo môn học
```http
GET /api/labs/subject/1
Authorization: Bearer {token}
```
**Quyền:** Admin, Head of Department, Lecturer

#### **GET /api/labs/class/{classId}** - Xem lab theo lớp
```http
GET /api/labs/class/1
Authorization: Bearer {token}
```
**Quyền:** Admin, Head of Department, Lecturer

## 📊 LUỒNG HOẠT ĐỘNG CHI TIẾT

### **1. Head of Department tạo bài lab mẫu:**
1. Đăng nhập với role "HeadOfDepartment"
2. Gọi API `POST /api/labs` với thông tin lab
3. Hệ thống kiểm tra quyền và tạo lab
4. Lab được lưu với trạng thái "Active"

### **2. Lecturer xem lab cho lớp mình phụ trách:**
1. Đăng nhập với role "Lecturer"
2. Gọi API `GET /api/labs/my-classes`
3. Hệ thống trả về lab của các môn học mà lecturer dạy

### **3. Lecturer tạo lịch lab:**
1. Chọn lab và lớp cụ thể
2. Gọi API `POST /api/labs/{labId}/schedule`
3. Hệ thống kiểm tra quyền và tạo Schedule
4. Lịch lab được tạo thành công

### **4. Thực hiện lab:**
1. Lecturer thực hiện lab theo lịch đã tạo
2. Sinh viên thực hành theo quy định
3. Lecturer chấm điểm và phản hồi

## 🔧 CÀI ĐẶT VÀ SỬ DỤNG

### **1. Build Project:**
```bash
dotnet build
```

### **2. Chạy Migration (nếu cần):**
```bash
dotnet ef database update
```

### **3. Chạy Project:**
```bash
dotnet run
```

### **4. Test API:**
- Sử dụng Swagger UI: `https://localhost:7134/swagger`
- Hoặc Postman với Bearer token

## ⚠️ LƯU Ý QUAN TRỌNG

1. **JWT Token**: Tất cả API đều yêu cầu Bearer token
2. **Role-based Access**: Kiểm tra role trước khi thực hiện thao tác
3. **Validation**: Hệ thống kiểm tra dữ liệu đầu vào nghiêm ngặt
4. **Error Handling**: Xử lý lỗi chi tiết với HTTP status code phù hợp

## 🐛 TROUBLESHOOTING

### **Lỗi thường gặp:**

1. **401 Unauthorized**: Token không hợp lệ hoặc hết hạn
2. **403 Forbidden**: Không có quyền thực hiện thao tác
3. **400 Bad Request**: Dữ liệu đầu vào không hợp lệ
4. **404 Not Found**: Không tìm thấy resource

### **Giải pháp:**
1. Kiểm tra JWT token
2. Kiểm tra role của user
3. Kiểm tra dữ liệu đầu vào
4. Kiểm tra ID resource

## 📞 HỖ TRỢ

Nếu gặp vấn đề, vui lòng:
1. Kiểm tra logs trong console
2. Xem HTTP status code và error message
3. Liên hệ team development

---

**Phiên bản:** 1.0.0  
**Cập nhật:** 2024-01-15  
**Tác giả:** Development Team

