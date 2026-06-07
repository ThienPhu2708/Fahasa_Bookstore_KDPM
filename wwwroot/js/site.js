// Function để hiện Pop-up
function openModal() {
    const modal = document.getElementById('promoModal');
    if (modal) {
        modal.classList.add('active');
    }
}

function closeModal() {
    const modal = document.getElementById('promoModal');
    if (modal) {
        modal.classList.remove('active');

        localStorage.setItem('modalShown', 'true');
    }
}

document.addEventListener('DOMContentLoaded', () => {
    const modalShown = localStorage.getItem('modalShown');

    if (!modalShown) { 
    setTimeout(openModal, 2000); 
    }
});

document.addEventListener('DOMContentLoaded', () => {

    const modalShown = localStorage.getItem('modalShown');

    const promoForm = document.querySelector('.modal-form');
    if (promoForm) {
        promoForm.addEventListener('submit', (e) => {
            e.preventDefault(); 

            alert("Cảm ơn bạn đã đăng ký! Mã giảm giá sẽ được gửi qua email.");

            closeModal();
        });
    }

    if (!modalShown) {
        setTimeout(openModal, 2000);
    }

    // Xử lý nút Thêm vào giỏ hàng bằng AJAX
    const addCartForms = document.querySelectorAll('.ajax-add-to-cart');
    addCartForms.forEach(form => {
        form.addEventListener('submit', async (e) => {
            e.preventDefault();
            const formData = new FormData(form);
            const sanPhamId = formData.get('sanPhamId');
            
            try {
                const response = await fetch('/Cart/ApiAddToCart', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ sanPhamId: parseInt(sanPhamId), quantity: 1 })
                });
                
                const result = await response.json();
                if (result.success) {
                    alert("Đã thêm vào giỏ hàng thành công!");
                } else {
                    alert(result.message || "Có lỗi xảy ra!");
                }
            } catch (error) {
                console.error(error);
                alert("Không thể kết nối đến máy chủ.");
            }
        });
    });
});