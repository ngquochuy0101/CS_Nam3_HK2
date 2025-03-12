const checkboxes = document.querySelectorAll('.service-checkbox');
const totalPriceElement = document.getElementById('totalPrice');

checkboxes.forEach(checkbox => {
    checkbox.addEventListener('change', function() {
        let total = 0;
        checkboxes.forEach(cb => {
            if (cb.checked) {
                total += parseInt(cb.getAttribute('data-price'));
            }
        });
        totalPriceElement.textContent = total;
    });
});