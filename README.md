Phần 2: Dành cho các thành viên trong nhóm (Quy trình làm việc hàng ngày) Mỗi khi một thành viên muốn viết code mới, họ sẽ làm theo các bước sau:

Bước 1: Lấy code mới nhất về máy

Đầu tiên, hãy đảm bảo bạn đang ở trên nhánh testcode và lấy về những thay đổi mới nhất mà người khác có thể đã đẩy lên.

Bash

Chuyển sang nhánh testcode (nếu bạn chưa ở đó)
git checkout testcode

Lấy code mới nhất từ nhánh testcode trên GitHub về
git pull origin testcode Bước 2: Viết code và commit

Bây giờ, bạn có thể thoải mái sửa code, thêm chức năng mới. Sau khi hoàn thành một phần công việc, hãy commit như bình thường.

Bash

Thêm các tệp đã thay đổi
git add .

Ghi lại thay đổi với một tin nhắn
git commit -m "Them chuc nang XYZ" Bước 3: Đẩy code lên nhánh testcode

Thay vì đẩy lên master, bây giờ bạn sẽ đẩy code của mình lên nhánh testcode để mọi người cùng xem và kiểm thử.

Bash

git push origin testcode
