## Yêu cầu hiện tại
-------------------

- Trước mắt kết hợp cái hiển thị bạn bè sau khi login thành công.
- Login thành công sẽ chạy event `Login_OnLoginSuccess` trong `MainWindow`
- ``` csharp
private void Login_OnLoginSuccess(string userLogged)
{
  MessageBox.Show("ok");
}
```
- Giao diện từ từ, chưa cần đẹp, hiển thị bạn bè rồi cung cấp event khi người dùng click vào từng ô friend là được.
