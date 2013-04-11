### SYMACI đã đái ở đây .....................

vChat
=====
Small chat application using TCP/IP and WCF Services, LINQ, Entity Framework CodeFirst (C#)

Update Log
======

Update 17.03 00:18
-----
+ Chỉnh lại code, cho phép sử dụng ConnectionString linh động hơn (tạo file Connect.txt (ổ C) và điền ConnectionString)

Update 17.03 17:00
-----
- Thêm class ObjectSerialize (vChat.Lib) (sử dụng cho WCF Service)
#1 Để chuyển 1 đối tượng thành xml text: ObjectSerialize<TenClass>.ParseToXml(ClassDaKhoiTao);
#2 Để chuyển đổi từ xml text thành object: Users user = DoiTuongObjectSerialize.ParseToObject();
- Chỉnh sửa lại model (không ảnh hưởng đến csdl hiện tại)
- Thêm một số method hỗ trợ cho việc load dữ liệu (chưa sử dụng được do chưa tạo trên WCF Service)
#1 List<Users> GetAll(), Conversation Get(Conversation ID), List<Conversation> GetAll(), List<Question> GetAll()
- Xóa bớt vài thứ không cần thiết

Update 17.03 19:45
-----
- Thêm class MethodInvokeResult (thay thế enum Status): chứa kết quả trả về khi thực thi các method trên WCF Service
- Sửa lại interface IUserNoticeService: đối tượng trả về của các method -> MethodInvokeResult

Update 18.03 15:45
-----
- Thêm entity mới FriendGroup, chỉnh lại entity Users
- Thêm chức năng validate dữ liệu cho vChat.Data layer (có thể bật/tắt thông qua config trong web.config (vChat.WCF))
- Thêm method FriendList() vào UserService.svc: lấy ra friend list của user

Update 19.03 16:15
-----
- Thay đổi kiểu trả về của các method (method trả về các đối tượng) thành XmlTextObject, muốn parse về đối tượng ban đầu (ở client) dùng ObjectSerialize<Kieu>.ParseToObject(DoiTuong)
- Thêm class ValidationWithStruct và ValidatExtenderWithStuct hỗ trợ cho validate (các đối tượng dạng struct: int, DateTime) (vChat.Business)

Update 20.03 22:43
-----
- Fix lỗi SignUp
- Chỉnh lại MethodInvokeResult
- Thêm entity mới (FriendMap) và cập nhật entity User, tạo sẵn mẫu câu hỏi bí mật (entity Question)

Update 22.03 00:37
-----
- Đơn giản hóa các kiểu dữ liệu trả về của các method trên WCF service, lược bỏ những dữ liệu trong entity không cần serialize (tránh tình trạng bị lỗi không serialize được dữ liệu trước khi gửi về client)

Update 26.03 15:53
-----
- Hiệu chỉnh chức năng validate, cho phép xem tất cả các lỗi xảy ra trong quá trình validate (InvokeMethodResult.Errors)

Update 11.04 21:18
-----
- Update một số model và thay đổi kiểu trả về của method FriendList (WCF)