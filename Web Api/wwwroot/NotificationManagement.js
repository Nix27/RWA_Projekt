window.onload = () => {
    let btnSend = document.querySelector('#btnSend');
    let pResponse = document.querySelector('#pResponse');

    getNumberOfUnsentNotifications();
    getNotifications();
    loadNotifications();
    
    btnSend.addEventListener('click', () => {
        fetch('api/Notifications/SendAllNotifications', {
            method: 'post',
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(() => {
                pResponse.innerHTML = 'Notifications sent successfully';
                getNumberOfUnsentNotifications();
            })
            .catch(error => console.log(error))
    });
};

function getNumberOfUnsentNotifications() {
    let spanElement = document.querySelector('span');
    fetch('api/Notifications/GetNumberOfUnsentNotifications', {
        method: 'get',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json())
        .then(data => spanElement.innerText = data)
        .catch(error => console.log(error));
}

function getNotifications() {
    fetch('api/Notifications', {
        method: 'get',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json())
        .then(data => {
            if (data != null) {
                localStorage.setItem('notifications', JSON.stringify(data));
                console.log('Notifications successfully saved to local storage');
            } else {
                console.log('Data is empty');
            }
        })
        .catch(error => console.log(error));
}

function loadNotifications() {
    let notifications = JSON.parse(localStorage.getItem('notifications'));
    $('#tblNotifications').DataTable({
        data: notifications,
        columns: [
            { data: "id", width: "15%" },
            { data: "receiverEmail", width: "15%" },
            { data: "subject", width: "15%" },
            { data: "body", width: "15%" }
        ]
    });
}