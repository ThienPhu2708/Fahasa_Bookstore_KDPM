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
});