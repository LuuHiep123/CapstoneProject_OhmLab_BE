# Hướng dẫn Import Users từ Excel

## API Endpoint
```
POST /api/ClassUser/import-excel?classId={classId}
Content-Type: multipart/form-data
```

## Cách sử dụng

### 1. Chuẩn bị file Excel
File Excel phải có định dạng như sau:

| MSSV | Họ và Tên | Email |
|------|-----------|-------|
| 20110001 | Nguyễn Văn A | nguyenvana@email.com |
| 20110002 | Trần Thị B | tranthib@email.com |

### 2. Yêu cầu về dữ liệu
- **MSSV**: Bắt buộc, không được trống (sẽ được lưu vào UserRollNumber và UserNumberCode, hiển thị trong response là userNumberCode)
- **Họ và Tên**: Bắt buộc, không được trống (lưu vào UserFullName)
- **Email**: Bắt buộc, phải đúng định dạng email (lưu vào UserEmail)
- **Số điện thoại**: Không cần thiết (đã loại bỏ khỏi quá trình import)

### 3. Gọi API
```javascript
const formData = new FormData();
formData.append('file', excelFile);

fetch('/api/ClassUser/import-excel?classId=1', {
    method: 'POST',
    headers: {
        'Authorization': 'Bearer ' + token
    },
    body: formData
})
.then(response => response.json())
.then(data => {
    console.log('Import result:', data);
});
```

## Response Format

### Success Response
```json
{
    "code": 200,
    "success": true,
    "message": "Import hoàn tất. Thành công: 5, Lỗi: 2",
    "data": {
        "totalRows": 7,
        "successCount": 5,
        "failedCount": 2,
        "successItems": [
            {
                "rowNumber": 2,
                "userNumberCode": "20110001",
                "fullName": "Nguyễn Văn A",
                "message": "Thêm thành công"
            }
        ],
        "errorItems": [
            {
                "rowNumber": 3,
                "userNumberCode": "20110002",
                "fullName": "Trần Thị B",
                "errorMessage": "Email không đúng định dạng"
            }
        ]
    }
}
```

### Error Response
```json
{
    "code": 400,
    "success": false,
    "message": "Chỉ chấp nhận file Excel (.xlsx, .xls)!"
}
```

## Các lỗi thường gặp

1. **File không đúng định dạng**: Chỉ chấp nhận .xlsx, .xls
2. **Thiếu thông tin bắt buộc**: MSSV, Họ tên, Email không được trống
3. **Email không đúng định dạng**: Email phải có @ và domain hợp lệ
4. **Sinh viên đã có trong lớp**: Không thể thêm sinh viên đã tồn tại
5. **Lớp học không tồn tại**: ClassId không hợp lệ

## Lưu ý

- API sẽ tự động tạo user mới nếu email chưa tồn tại
- Role mặc định cho user mới là "Student"
- Status mặc định là "Active"
- Chỉ Admin, HeadOfDepartment, Lecturer mới có quyền import
