var userMarker = []

var map = L.map('map').setView([50.44866, 30.5250106], 13);
L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
    maxZoom: 19,
    attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
}).addTo(map);

function addToMap(markerData) {
    console.log(markerData)
    var marker = L.marker([markerData.lat, markerData.lng])
        .bindPopup(`<h2>${markerData.title}</h2><div></div>`);


    marker.id = markerData.id
    userMarker = marker
    marker.addTo(map);
}
function loadMarkers() {
    fetch("api/map/markers")
        .then(r => r.json())
        .then(list => {
            list.forEach(i => addToMap(i))
        })
}

loadMarkers()