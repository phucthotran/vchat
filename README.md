###### [Rule mới (14.04)](#nhng-iu-lu-)
###### [Yêu cầu hiện tại > click](TASK.md)

## Update Log
-------------

#### Update 04.05 1:30 PM
- Cách lấy IP của một username:

```c#
	// Đầu tiên phải binding command đến một method response
	this.Get<Client>().CommandBinding(CommandType.CheckIP, res => 
	{
		// parameter đầu tiên trong res là RemoteEndPoint của username cần check
		EndPoint ep = (EndPoint)res.Params[0];
		// do work here
	});
	// Hoặc có thể binding vào một method nào đó
	this.Get<Client>().CommandBinding(CommandType.CheckIP, OpenVoice);
	public void OpenVoice(CommandResponse res)
	{
		EndPoint ep = (EndPoint)res.Params[0];
	}
	
	// Sau đó cần gửi command lên server để lấy IP về
	this.Get<Client>().SendCommand(CommandType.CheckIP, "SERVER", "username cần check");
	
	// nhớ là CommandBinding phải chạy trước thì SendCommand khi gửi lên mới có response được ...
```

#### Update 04.05 6:30 AM
- Chỉnh sửa lại cách thức gửi và nhận tin nhắn
- Thêm tính năng hiển thị thời gian gửi và nhận tin nhắn bằng cách nhấn nút `F2`
- Thêm tính năng dùng lại tin nhắn đã gửi bằng cách nhấn phím Up hoặc Down
- Thêm tính năng hiển thị popup góc dưới bên phải màn hình. Nhấn vào sẽ focus vào khung chat tương ứng

#### Update 02.05 3:07 AM
- Thêm tính năng nhấp nháy Window khi không focus
- Sửa lỗi các cửa sổ thuộc `MainWindow` không tắt khi nhấn đăng xuất
- Thêm tính năng hỏi người dùng khi nhấn tắt `MainWindow` hoặc nhấn Esc
- Thêm icon vào trong các button của `Chat` module
- Thêm âm thanh khi có tin nhắn gửi đến

#### Update 01.05 6:10 PM
- Tái cấu trúc solution **Core**, loại bỏ BackgroundWorker mà thay vào bằng `Task` và áp dụng `Async Socket`
- Tái cấu trúc lại cách thức binding command từ client
- Tạo `MainWindowListener` dùng để xử lý các `Command` được server gửi đến
- Tạo `SendFilePanel` dùng để hiển thị những file được client gửi đến
- Fix lỗi khiến cho server chủ động disconnect client.

#### Update 26.04 4:40 PM
- Thêm module `UploadImage` dùng cho upload ảnh đại diện (profile picture)
- Thêm method `ChangeProfilePicture` (`vChat.WCF`): update ảnh đại diện cho người dùng
- Resize ảnh và chuyển sang byte để lưu xuống database cho `UploadImage` module
- Hiển thị ảnh đại diện trên **FriendList**
- Fix lỗi khi đăng nhập cùng lúc (**thử lại lần nữa cho chắc ăn nha**)
- Kiểm tra lỗi **Yêu Cầu Kết Bạn**: Không có lỗi
- Catch những exception không thể kiểm soát (Unhandle exception) trong `vChat.Data`

#### Update 23.04 7:10 PM
- Tối ưu code `FriendList`, `AddFriend` module
- Tối ưu code `vChatServices`: **vChat.Data**, **vChat.Business**
- Thêm field **Picture** vào entity `Users` (`vChat.Model`)
- Chỉnh lại giao diện cho `SignUp` module: căn giữa theo chiều ngang và dọc, chỉnh lại kích thước
- [Task mới](TASK.md)

#### Update 21.04 3:56 PM
- Thêm method mới vào `vChat.WCF: **GroupInfo**, **RemoveGroup**
- Hoàn Thiện **Yêu Cầu Kết Bạn**: update yêu cầu kết bạn theo thời gian thực
- Thêm context menu cho FriendList: **Thêm Bạn** (đã hoàn thiện), **Xóa Nhóm** (hoàn thiện), **Xem Chi Tiết**
- Chỉnh event click cho FriendList thành double click
- Thêm module `RemoveGroup` dùng cho thao tác xóa nhóm trên FriendList

#### Update 21.04 2:06 AM
- Sửa lỗi đăng nhập không được sau khi đăng xuất
- Chỉnh sửa cách thức hiển thị của MainWindow (MinWidth = 300, Height = chiều cao tối đa của màn hình)
- Chỉnh sửa ChatWindow

#### Update 20.04 3:55 PM
- Tái tổ chức solution **Core**
- Ứng dụng queue vào việc gửi và nhận command trong client
- Thêm chức năng chat giữa các client
- Thêm method `this.InitTheme()` để khởi tạo theme cho window
- [Task mới](TASK.md)

#### Update 18.04 11:20 PM
- Thêm method `AddGroup` dùng để tạo group mới nếu người dùng mong muốn
- Thêm danh sách **Yêu Cầu Kết Bạn** để người dùng có thể quản lý các yêu cầu kết bạn của mình
- Thêm method mới vào `vChat.WCF`: **AcceptFriendRequest**, **IgnoreFriendRequest**, **FriendRequests**, **MoveContact**, **RemoveContact**
- Hoàn thiện chức năng **Chỉnh Sửa Bạn Bè**
- Thêm method mới vào `vChat.Module` trong class Helper **ShowMessage**: dùng cho hiển thị các kết quả trả về từ server (qua class `MethodInvokeResult`), tạm thời hiển thị qua MessageBox. Sử dụng:

```c#
	Helper.ShowMessage(DoiTuongMethodInvokeResult);
```

- Tái tổ chức và hoàn thiện chức năng cho `AddFriend` module. Có 2 sự kiện cần lưu ý là: **OnAddFriendSuccess** và **OnAddFriendError**. VD:

```c#
	private void AddFriend_OnAddFriendSuccess(object sender, AddFriendArgs e)
	{
		MessageBox.Show(String.Format("Add Success - FriendName: {0}, GroupName: {1}", e.FriendName, e.GroupName));
	}
	
	private void AddFriend_OnAddFriendError(object sender, AddFriendArgs e)
	{
		MessageBox.Show(String.Format("Add Error - FriendName: {0}, GroupName: {1}", e.FriendName, e.GroupName ?? "NULL"));
	}
```
- Thêm 3 event cho `FriendList` module: **OnFriendClick** và **OnGroupClick**, **OnAddFriendClick**. VD:

```c#

	private void FriendList_OnAddFriendClick(object sender, RoutedEventArgs e)
	{
		Window AddFriendWind = new Window {
			Content = _addFriendModule, 
			Title = "Add Friend", 
			SizeToContent = System.Windows.SizeToContent.WidthAndHeight, 
			WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner 
		};
		AddFriendWind.ShowDialog();
	}

	private void FriendList_OnGroupClick(object sender, GroupArgs e)
	{
		MessageBox.Show(String.Format("ID: {0}, Name: {1}", e.GroupID, e.Name));
	}

	private void FriendList_OnFriendClick(object sender, FriendArgs e)
	{
		MessageBox.Show(String.Format("ID: {0}, Username: {1}, FirstName: {2}, LastName: {3}", e.UserID, e.Username, e.FirstName, e.LastName));
	}
```

#### Update 17.04 11:42 AM
- Fix lỗi bể layout của FriendList

#### Update 17.04 10:06 AM
- Cho phép chỉnh sửa Friend List bằng cách đánh check vào những contact cần chỉnh sửa (Sellect All, Deselect All) (Chưa có code)
- Thêm control `ImageButton` (`vChat.Control`). Cách sử dụng:

```c#
	<ImageButton Name="btnTest" Text="Test" Image="Image/test.png" Command="{Binding TestCommand}" Click="btnTest_Click"/>
```

#### Update 17.04 3:28 AM
- Cập nhật tính năng phân luồng để hiện lên progress khi đang login
- Cải thiện thông báo lỗi khi login không đúng hoặc khi cookie bị sửa đổi
- Tối ưu phần **core**

#### Update 16.04 9:21 PM
- Sửa lại phần layout, bỏ StackPanel và chuyển lại Grid như cũ (cái này muốn add cùng lúc 2 control lên thì tạo StackPanel và phân panel ra)
- Sau khi login thành công, **ID** và **Name** của **User** sẽ được lưu trong client. Cách sử dụng:

```c#
// lấy ra id hiện hành
int userID = this.Get<Client>().ID;
// lấy ra username hiện hành
int userName = this.Get<Client>().Name;
``` 

#### Update 15.04 3:20 AM
- Thêm method `LoginHash` trong **vChatService** dùng để xác nhận login hợp lệ của 1 tài khoản với tham số password truyền vào đã được mã hoá MD5 sẵn (tạo ra cái này để làm cookie)
- Thêm extension method `Panel.LoadModule(params object[] args)` dùng để tạo 1 module và truyền tham số tương ứng vào constructor nếu có
- Chuyển đổi giao diện ứng dụng
- Thêm tính năng **cookie** khi login
- Thêm tính năng **đăng xuất** (nằm ở trên title bar)

#### Update 14.04 1:38 PM
- Sửa **Assembly Name** của project `vChat.Control` từ **vChat** thành **vChat.Control**
- Xoá menu context của MainWindow
- Thêm thẻ **\<runtime>** trong `app.config` nhằm mục đích có thể load được những file dll cần thiết trong các thư mục con: bin;bin\client;bin\core

#### Update 14.04 5:50 AM
- Tái tổ chức hệ thống module (xem [rule mới](#nhng-iu-lu-))
- Chỉnh sửa [yêu cầu hiện tại](TASK.md)
- Bỏ các extension không cần thiết, thêm 2 extension method mới: `Panel.LoadModule<T>()` và `UserControl.Get<T>()`
	- `Panel.LoadModule<T>()`: load một module nào đó dựa vào kiểu **T** truyền vào. **Panel** sẽ xoá module hiện tại đang chứa tại panel đó và thêm module **T** vào. Ví dụ: `Grid.LoadModule<Login>();`
	- `UserControl.Get<T>()`: **UserControl** ở đây là một module. Method này dùng để trả về kiểu **T** của một key resource được định nghĩa trong **Application** (vì module được tách ra làm 1 project riêng nên project module không thể add reference đến project chính được, do đó phải dùng method này để lấy giá trị static trong Application). Ví dụ: `this.Get<UserServiceClient>();` (phải dùng `this` mới gọi hàm này được)

#### Update 12.04 6:30 PM
- Fix lỗi kết quả trả về của method `FriendList` (`vChat.WCF`)
- Thêm 2 module mới `AddFriend`, `FriendList`

#### Update 11.04 9:18 PM
- Update một số model và thay đổi kiểu trả về của method `FriendList` (`vChat.WCF`)

#### Update 26.03 3:53 PM
- Hiệu chỉnh chức năng validate, cho phép xem tất cả các lỗi xảy ra trong quá trình validate `InvokeMethodResult.Errors`

#### Update 22.03 00:37 AM
- Đơn giản hóa các kiểu dữ liệu trả về của các method trên WCF service, lược bỏ những dữ liệu trong entity không cần serialize (tránh tình trạng bị lỗi không serialize được dữ liệu trước khi gửi về client)

#### Update 20.03 10:43 PM
- Fix lỗi SignUp
- Chỉnh lại `MethodInvokeResult`
- Thêm entity mới `FriendMap` và cập nhật entity User, tạo sẵn mẫu câu hỏi bí mật (entity `Question`)

#### Update 19.03 4:15 PM
- Thay đổi kiểu trả về của các method (method trả về các đối tượng) thành `XmlTextObject`, muốn parse về đối tượng ban đầu (ở client) dùng `ObjectSerialize<Kieu>.ParseToObject(DoiTuong)`
- Thêm class `ValidationWithStruct` và `ValidatExtenderWithStuct` hỗ trợ cho validate (các đối tượng dạng struct: `int`, `DateTime`) (`vChat.Business`)

#### Update 18.03 3:45 PM
- Thêm entity mới `FriendGroup`, chỉnh lại entity `Users`
- Thêm chức năng validate dữ liệu cho `vChat.Data` layer (có thể bật/tắt thông qua config trong `web.config`) (`vChat.WCF`))
- Thêm method `FriendList()` vào `UserService.svc`: lấy ra friend list của user

#### Update 17.03 7:45 PM
- Thêm class `MethodInvokeResult` (thay thế enum `Status`): chứa kết quả trả về khi thực thi các method trên WCF Service
- Sửa lại interface `IUserNoticeService`: đối tượng trả về của các method -> `MethodInvokeResult`

#### Update 17.03 5:00 PM
- Thêm class `ObjectSerialize` (`vChat.Lib`) (sử dụng cho WCF Service)
    - Để chuyển 1 đối tượng thành xml text: `ObjectSerialize<TenClass>.ParseToXml(ClassDaKhoiTao)`;
    - Để chuyển đổi từ xml text thành object: `c# Users user = DoiTuongObjectSerialize.ParseToObject()`;
- Chỉnh sửa lại model (không ảnh hưởng đến csdl hiện tại)
- Thêm một số method hỗ trợ cho việc load dữ liệu (chưa sử dụng được do chưa tạo trên WCF Service)
    - `List<Users> GetAll()`, `Conversation Get(Conversation ID)`, `List<Conversation> GetAll()`, `List<Question> GetAll()`
- Xóa bớt vài thứ không cần thiết

#### Update 17.03 0:18 AM
- Chỉnh lại code, cho phép sử dụng `ConnectionString` linh động hơn (tạo file `Connect.txt` (ổ `C`) và điền ConnectionString)

--------------------------
###### Những điều lưu ý

- **Về mô hình module:**
	- `MainWindow` sẽ chỉ đảm nhiệm công việc load và bố trí vị trí cho các module. (tối giản hơn trước, chi tiết ở code mẫu bên dưới)
	- Mọi hoạt động của module đều tự túc.
	- Các custom control muốn xây dựng để sử dụng lại sẽ được tạo trong project `vChat.Control`
	- Các module sẽ cần phải nằm trong một thư mục mới trong project `vChat.Module`
	- Project `vChat.Service` chỉ dùng để reference đến các Service. (mục đích tách ra là để project chính và module reference đến được và không bị xung đột)
- **Cách thức sử dụng module trong project chính:**

```c#
// load Login vào grid và chạy event, event đơn giản cũng chỉ là để load 1 module nào khác mà thôi
Login loginMdl = Grid.LoadModule<Login>();
loginMdl.OnLoginSuccess += new LoginSuccessHandler(delegate {
	Grid.LoadModule<FriendList>();
});
```

- **Cách thức gọi và sử dụng các global resource trong App:**

```c#
// giả sử trong App ta thêm resource là - Resources.Add("UserServiceClient", new UserServiceClient());
public partial Login : UserControl
{
	UserServiceClient _UserService;
	public Login()
	{
		// key resource khi add phải đặt trùng với tên class của đối tượng cần lưu giữ thì mới lấy ra được
		_UserService = this.Get<UserServiceClient>();
	}
}
```

--------------------------
