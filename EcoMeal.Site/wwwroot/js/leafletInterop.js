window.leafletInterop = {
    map: null,
    marker: null,

    initLeafletMap: function (elementId, lat, lng, dotNetRef) {
        if (!lat || !lng) {
            lat = 44.3167;
            lng = 23.8000;
        }

        this.map = L.map(elementId).setView([lat, lng], 13);

        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '© OpenStreetMap contributors'
        }).addTo(this.map);

        this.marker = L.marker([lat, lng], { draggable: true }).addTo(this.map);

        const updateCoordinates = (latLng) => {
            dotNetRef.invokeMethodAsync('OnMapClicked', latLng.lat, latLng.lng);
        };

        this.marker.on('dragend', function (e) {
            updateCoordinates(e.target.getLatLng());
        });

        this.map.on('click', (e) => {
            this.marker.setLatLng(e.latlng);
            updateCoordinates(e.latlng);
        });
    },

    updateMarkerPosition: function (elementId, lat, lng) {
        if (this.marker && lat && lng) {
            this.marker.setLatLng([lat, lng]);
            this.map.setView([lat, lng]);
        }
    },

    destroyLeafletMap: function (elementId) {
        if (this.map) {
            this.map.remove();
            this.map = null;
            this.marker = null;
        }
    },

    getUserLocation: function () {
        return new Promise((resolve, reject) => {
            if (!navigator.geolocation) {
                reject("Geolocation is not supported by your browser");
                return;
            }
            navigator.geolocation.getCurrentPosition(
                (position) => {
                    resolve({
                        latitude: position.coords.latitude,
                        longitude: position.coords.longitude
                    });
                },
                (error) => {
                    reject(error.message);
                }
            );
        });
    }
};
