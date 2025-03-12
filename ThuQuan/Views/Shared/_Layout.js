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
