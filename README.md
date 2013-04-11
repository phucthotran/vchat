###### [Nhấn vào đây để xem rule](#nhng-iu-lu-)
###### [Yêu cầu hiện tại > click](TASK.md)

## Update Log
-------------

#### Update 26.03 3:53 PM
- Hiệu chỉnh chức năng validate, cho phép xem tất cả các lỗi xảy ra trong quá trình validate (InvokeMethodResult.Errors)

#### Update 22.03 00:37 AM
- Đơn giản hóa các kiểu dữ liệu trả về của các method trên WCF service, lược bỏ những dữ liệu trong entity không cần serialize (tránh tình trạng bị lỗi không serialize được dữ liệu trước khi gửi về client)

#### Update 20.03 10:43 PM
- Fix lỗi SignUp
- Chỉnh lại MethodInvokeResult
- Thêm entity mới (FriendMap) và cập nhật entity User, tạo sẵn mẫu câu hỏi bí mật (entity Question)

#### Update 19.03 4:15 PM
- Thay đổi kiểu trả về của các method (method trả về các đối tượng) thành XmlTextObject, muốn parse về đối tượng ban đầu (ở client) dùng ObjectSerialize<Kieu>.ParseToObject(DoiTuong)
- Thêm class ValidationWithStruct và ValidatExtenderWithStuct hỗ trợ cho validate (các đối tượng dạng struct: int, DateTime) (vChat.Business)

#### Update 18.03 3:45 PM
- Thêm entity mới FriendGroup, chỉnh lại entity Users
- Thêm chức năng validate dữ liệu cho vChat.Data layer (có thể bật/tắt thông qua config trong web.config (vChat.WCF))
- Thêm method FriendList() vào UserService.svc: lấy ra friend list của user

#### Update 17.03 7:45 PM
- Thêm class MethodInvokeResult (thay thế enum Status): chứa kết quả trả về khi thực thi các method trên WCF Service
- Sửa lại interface IUserNoticeService: đối tượng trả về của các method -> MethodInvokeResult

#### Update 17.03 5:00 PM
- Thêm class ObjectSerialize (vChat.Lib) (sử dụng cho WCF Service)
    - Để chuyển 1 đối tượng thành xml text: ObjectSerialize<TenClass>.ParseToXml(ClassDaKhoiTao);
    - Để chuyển đổi từ xml text thành object: Users user = DoiTuongObjectSerialize.ParseToObject();
- Chỉnh sửa lại model (không ảnh hưởng đến csdl hiện tại)
- Thêm một số method hỗ trợ cho việc load dữ liệu (chưa sử dụng được do chưa tạo trên WCF Service)
    - List<Users> GetAll(), Conversation Get(Conversation ID), List<Conversation> GetAll(), List<Question> GetAll()
- Xóa bớt vài thứ không cần thiết

#### Update 17.03 0:18 AM
- Chỉnh lại code, cho phép sử dụng ConnectionString linh động hơn (tạo file Connect.txt (ổ C) và điền ConnectionString)

#### Update 11.04 9:18 PM
- Update một số model và thay đổi kiểu trả về của method FriendList (WCF)

--------------------------
###### Những điều lưu ý

- **Về header cho từng update:**
    1. Dùng kiểu H4 `####`
    2. Thời gian update định dạng theo kiểu `dd.MM hh:mm TT`
    3. Update gần nhất sẽ được đưa lên đầu trang
- **Về cách thức viết code cho từng user control (UC) hiện tại:**
    1. Vào thư mục **View** > **Controls** tạo một `UserControl(WPF)`
    2. Trong file `.xaml` thêm dòng `xmlns:v="clr-namespace:vChat.Templates"` vào proprety của tag `<UserControl ...>`
    3. Sửa tag `UserControl` thành `v:vChatController` (sửa luôn cả tag đóng)
    4. Trong file `.xaml.cs` sửa lớp kế thừa `UserControl` thành `vChatController`
    5. Để tạo controller cho UC ta vào thư mục **Controllers** tạo 1 file `.cs` đặt tên theo cú pháp: `<TênClassUserControl>Controller.cs`
        - Ví dụ: class của UC là `Login` thì file controller sẽ đặt tên là `LoginController.cs`
        - Controller phải thuộc `namespace vChat.Controllers`
        - **Không nhất thiết phải tạo file controller**, nếu không có nhu cầu tách biệt methods và dồn vào controller thì khỏi tạo cũng được.
    6. Để sử dụng controller trong user control ta gọi `this.Controller`
        - `this.Controller` thuộc kiểu **dynamic** nên khi gọi các methods trong controller sẽ không autocomplete. Nếu cần thì ép kiểu sang class controller mong muốn.
- **Về yêu cầu khác:**
    1. `MainWindow` sẽ xử lý chính cho các UC được tạo
    2. Nếu cần truyền các tác vụ từ bên trong UC ra cho MainWindow xử lý thì ta tạo event và delegate method bên trong UC đó
    3. Code tham khảo như UC `Login` hay `SignUp`

###### Nếu có gì bất tiện thì cùng nhau góp ý =]

--------------------------
