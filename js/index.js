document.addEventListener("DOMContentLoaded", function () {
  const userBtn = document.getElementById("userBtn");
  const dropdownMenu = document.getElementById("dropdownMenu");

  // Toggle menu khi bấm nút
  userBtn.addEventListener("click", function (event) {
    dropdownMenu.style.display =
      dropdownMenu.style.display === "block" ? "none" : "block";
    event.stopPropagation(); // Ngăn chặn sự kiện lan ra ngoài
  });

  // Ẩn menu khi click ra ngoài
  document.addEventListener("click", function () {
    dropdownMenu.style.display = "none";
  });

  // Ngăn chặn menu bị ẩn khi click bên trong menu
  dropdownMenu.addEventListener("click", function (event) {
    event.stopPropagation();
  });
});
// Gắn sự kiện click cho nút "Thông tin"
document.getElementById("btn-info").addEventListener("click", function () {
  // Hiển thị phần display (nếu đang ẩn)
  const displaySection = document.querySelector(".display");
  displaySection.style.display = "block";

  // Cập nhật nội dung bên trong phần display
  displaySection.innerHTML = `
      <h2 style="margin-top:10px">Thông tin cá nhân</h2>
      <div class="container">
        <img id="preview" src="img/download (3) (1).png" alt="Ảnh đại diện" class="profile-img">
        <div class="info-group">
          <label>Họ và tên:</label>
          <span>Nguyễn Quốc Huy</span>
        </div>
        <div class="info-group">
          <label>Email:</label>
          <span>ngquochuy4002@gmail.com</span>
        </div>
        <div class="info-group">
          <label>SĐT:</label>
          <span>0878955654</span>
        </div>
        <button onclick=edit_info() id="btn-edit-info">Sửa thông tin</button>
      </div>
    `;
});
// Gắn sự kiện click cho nút "Thay đổi Thông tin"
function edit_info() {
  // Hiển thị phần display (nếu đang ẩn)
  const displaySection = document.querySelector(".display");
  displaySection.style.display = "block";

  // Cập nhật nội dung bên trong phần display
  displaySection.innerHTML = `<div class="container">
    <h2>Thay đổi thông tin</h2>
    <form>
      <!-- Ảnh đại diện -->
      <div class="edit_info_form-group">
        <img id="edit_info_preview" src="img/download (3) (1).png" alt="Ảnh đại diện" class="edit_info_profile-img">
      </div>
      <div class="edit_info_form-group">
        <label for="edit_info_image">Chọn ảnh đại diện mới:</label>
        <input type="file" name="image" id="edit_info_image" accept="image/*" onchange="previewImage(event)">
      </div>
      <!-- Thông tin cá nhân -->
      <div class="edit_info_form-group">
        <label for="edit_info_name">Họ và tên:</label>
        <input type="text" id="edit_info_name" name="name" value="Nguyễn Quốc Huy" required>
      </div>
      <div class="edit_info_form-group">
        <label for="edit_info_email">Email:</label>
        <input type="email" id="edit_info_email" name="email" value="ngquochuy4002@gmail.com" required>
      </div>
      <div class="edit_info_form-group">
        <label for="edit_info_sdt">SĐT:</label>
        <input type="tel" id="edit_info_sdt" name="sdt" value="0878955654" required>
      </div>
      <div class="edit_info_form-group">
        <label for="edit_info_password">Mật khẩu:</label>
        <input type="password" id="edit_info_password" name="password" placeholder="Nhập mật khẩu mới nếu muốn thay đổi">
      </div>
      <button id="edit_info_btn-save" onclick=showAlert("Đã lưu thay đổi")>Lưu thay đổi</button>
    </form>
  </div>
    `;
}
function showAlert(message) {
  alert(message);
}
function device() {
  // Hiển thị phần display (nếu đang ẩn)
  const displaySection = document.querySelector(".display");
  displaySection.style.display = "block";

  // Cập nhật nội dung bên trong phần display
  displaySection.innerHTML = `<h2 style="margin-top:10px"">Thuê thiết bị</h2>

    <div class="content">
        <div class="container">
        <div class="service-container">
        
            <div class="service">
                <label>
                    <input type="checkbox" class="service-checkbox" data-price="100"> 
                    <img src="./img/may-chieu-epson-ebe10-3600-ansilumen-i9j4QY.png" alt="Máy chiếu"> Máy chiếu (100.000 VNĐ)
                </label>
            </div>
            <div class="service">
                <label>
                    <input type="checkbox" class="service-checkbox" data-price="200"> 
                    <img src="./img/1653979960-920-o-dien-da-nang-shoptida-cubez-k4u-24m-chiu-tai-2500w-cam-noi-chien-noi-lau-an-toan-copy-1-removebg-preview.png" alt="Dịch vụ B"> Dịch vụ B (200.000 VNĐ)
                </label>
            </div>
            
        </div>
        <div class="service-container">
            <div class="service">
                <label>
                    <input type="checkbox" class="service-checkbox" data-price="100"> 
                    <img src="./img/may-chieu-epson-ebe10-3600-ansilumen-i9j4QY.png" alt="Máy chiếu"> Máy chiếu (100.000 VNĐ)
                </label>
            </div>
            <div class="service">
                <label>
                    <input type="checkbox" class="service-checkbox" data-price="200"> 
                    <img src="./img/1653979960-920-o-dien-da-nang-shoptida-cubez-k4u-24m-chiu-tai-2500w-cam-noi-chien-noi-lau-an-toan-copy-1-removebg-preview.png" alt="Dịch vụ B"> Dịch vụ B (200.000 VNĐ)
                </label>
            </div>
            
        </div>
        <div style="display: flex; background-color: #f0f0f0; padding: 10px; border-radius: 5px;">
            

        <div>Số thiết bị đã chọn: <span id="totalDevice">0</span></div>
        <div style="position: relative; margin-left: auto;">
            <div class="total">Tổng tiền: <span id="totalPrice">0</span> VNĐ</div>
            <button onclick="window.location.href='index.html';showAlert('Chọn thiết bị thành công!')"  class="btn-primary btn-result">Hoàn thành</button>
        </div>

        </div>
    </div>
    </div>
        `;
}
