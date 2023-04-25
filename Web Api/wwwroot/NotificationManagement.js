window.onload = () => {
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