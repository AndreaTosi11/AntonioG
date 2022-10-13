
"use strict";
function initMap() {
    const myLatlng = { lat: 41.902782, lng: 12.496365 };
    const map = new google.maps.Map(document.getElementById("map"), {
        zoom: 12,
        center: myLatlng,
    });

    let infoWindow = new google.maps.InfoWindow({
        /*content: "Click the map to get Lat/Lng!",*/
        /*position: myLatlng,*/
    });
    
    let marker = new google.maps.Marker({
        /*position: myLatlng,*/
        map,
    });
/*    marker.open(map);*/

    map.addListener("click", (mapsMouseEvent) => {

        infoWindow.close();

        infoWindow = new google.maps.InfoWindow({
            position: mapsMouseEvent.latLng,
        });
        infoWindow.open({
            anchor: marker,
            map,
        });

        //marker.setContent(JSON.stringify(mapsMouseEvent.latLng.toJSON(), null, 2));
        //marker.open(map);
        var Codify = JSON.stringify(mapsMouseEvent.latLng.toJSON());
        var Decodify = JSON.parse(Codify);
       /* alert(Codify)*/

        const myElement = document.getElementById('Latid');
        myElement.value = Decodify.lat;
        const myElementt = document.getElementById('Lngid');
        myElementt.value = Decodify.lng;

        //document.getElementById('Latid').value = Decodify.lat;
        //document.getElementById('Lngid').value = Decodify.lng;


    });
}
function Latt() {
    const Lat = JSON.parse(pippo);
    console.log(Lat);
}
function Lngg() {
    const Lng = JSON.parse(data);
    console.log(Lng);
}

window.initMap = initMap;
export { };