vChat
=====
<<<<<<< HEAD
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