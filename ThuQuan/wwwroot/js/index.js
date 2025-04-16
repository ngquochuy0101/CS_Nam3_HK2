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
function handleSave() {
  alert("Đã thay đổi thông tin"); // Hiển thị thông báo
  window.location.href = '/'; // Chuyển hướng trang
}
