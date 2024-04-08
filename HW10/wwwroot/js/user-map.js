var userMarker

var map = L.map('map').setView([50.44866, 30.5250106], 13);
L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
    maxZoom: 19,
    attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
}).addTo(map);

function loadMarker() {
    var userId = document.getElementById("UserId").value;

    fetch("/api/map/userMarker/" + userId)
        .then(response => {
            if (!response.ok) {
                throw new Error('Ошибка сети или сервера');
            }
            return response.json();
        })
        .then(data => {
            addToMap(data);
        })
        .catch(error => {
            console.error('Ошибка при выполнении запроса:', error);
        });

}

function addToMap(markerData) {
    console.log(markerData)
    var marker = L.marker([markerData.lat, markerData.lng])
        .bindPopup(`<h2>${markerData.title}</h2><div><button data-id='${markerData.id}' class='btn btn-danger delete-marker' type='button'>Delete</button></div>`);


    marker.id = markerData.id
    userMarker = marker
    marker.addTo(map);
}

function removeFromMap() {
    map.removeLayer(userMarker)
    userMarker = null;
}

function onMapClick(e) {
    let username = document.getElementById("loginInput")


    if (username != null) {
        if (username.value == '') {
            window.alert("Input your email to add marker");
            return;
        }
    }

    if (userMarker != null) {
        fetch("/api/map/userMarker", {
            method: "DELETE",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ id: userMarker.id }),
        })
        removeFromMap()
    }
    let data = {
        Title: username.value,
        Lat: e.latlng.lat,
        Lng: e.latlng.lng,
    }

    fetch("/api/map/userMarker", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(data),
    }).then(r => r.json()).then(response => {
        if (response.ok) {
            document.getElementById('MapMarkerId').value = response.marker.id
            console.log(response)
            addToMap(response.marker)
        } else {
            Swal.fire("Error!", "Marker not added", "error")
        }
    })

}

map.on('click', onMapClick);

loadMarker()


$(document).on('click', 'button.delete-marker', e => {
    let id = Number($(e.target).data('id'))

    fetch("api/map/userMarker", {
        method: "DELETE",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ id: id }),
    }).then(r => r.json()).then(response => {
        if (response.ok) {
            removeFromMap()
        } else {
            Swal.fire("Error!", "Marker not deleted", "error")
        }
    })
})