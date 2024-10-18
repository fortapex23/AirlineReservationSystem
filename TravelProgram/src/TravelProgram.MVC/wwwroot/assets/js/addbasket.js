//document.addEventListener('DOMContentLoaded', function () {
//    const basketButtons = document.querySelectorAll('.add-to-basket-btn');

//    basketButtons.forEach(button => {
//        button.addEventListener('click', function (e) {
//            const flightId = this.closest('form').getAttribute('data-flight-id');

//            console.log(`Flight ID: ${flightId}`);

//            fetch('/Home/AddToBasket', {
//                method: 'POST',
//                headers: {
//                    'Content-Type': 'application/json',
//                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
//                },
//                body: JSON.stringify({ flightId: flightId })
//            })
//                .then(response => response.json())
//                .then(data => {
//                    console.log(`Server Response:`, data);

//                    if (data.success) {
//                        alert('Flight added to the basket successfully!');
//                    } else {
//                        alert(`Failed to add flight to the basket. Message: ${data.message}`);
//                    }
//                })
//                .catch(error => {
//                    console.error('Error occurred while adding the flight to the basket:', error);
//                    alert('An error occurred while adding the flight to the basket.');
//                });
//        });
//    });
//});
