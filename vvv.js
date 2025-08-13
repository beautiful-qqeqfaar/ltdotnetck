// Toggle popup
document.getElementById("registerBtn").onclick = () => {
  document.getElementById("popup").classList.remove("hidden");
};
document.getElementById("closePopup").onclick = () => {
  document.getElementById("popup").classList.add("hidden");
};

// Validate form
document.getElementById("registerForm").onsubmit = (e) => {
  e.preventDefault();
  const email = document.getElementById("email").value;
  const pass = document.getElementById("password").value;
  if (!email.includes("@")) {
    alert("Email phải chứa @");
    return;
  }
  if (pass.length <= 6) {
    alert("Mật khẩu phải > 6 ký tự");
    return;
  }
  alert("Đăng ký thành công!");
  document.getElementById("popup").classList.add("hidden");
};

// Đếm lượt xem
document.querySelectorAll(".title").forEach((title) => {
  title.addEventListener("click", () => {
    const viewsSpan = title.parentElement.querySelector(".views span");
    let views = parseInt(viewsSpan.textContent);
    views++;
    viewsSpan.textContent = views;
  });
});

// Dark mode
const darkBtn = document.getElementById("darkModeToggle");
darkBtn.onclick = () => {
  document.body.classList.toggle("dark-mode");
  localStorage.setItem("darkMode", document.body.classList.contains("dark-mode"));
};
// Load trạng thái dark mode
if (localStorage.getItem("darkMode") === "true") {
  document.body.classList.add("dark-mode");
}

// Đồng hồ
function updateClock() {
  const now = new Date();
  const time = now.toLocaleTimeString("vi-VN", { hour12: false });
  document.getElementById("clock").textContent = time;
}
setInterval(updateClock, 1000);
updateClock();
