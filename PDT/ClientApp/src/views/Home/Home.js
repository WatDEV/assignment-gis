import React, { Component } from 'react';
import { Map, TileLayer, Marker, Popup, Tooltip, Polygon, Polyline, GeoJSON } from 'react-leaflet';
import 'leaflet/dist/leaflet.css';
import L from 'leaflet';
import Select from 'react-select';
import hash from 'object-hash';

delete L.Icon.Default.prototype._getIconUrl;

L.Icon.Default.mergeOptions({
	iconRetinaUrl: require('leaflet/dist/images/marker-icon-2x.png'),
	iconUrl: require('leaflet/dist/images/marker-icon.png')
});

let LeafletIcon = L.Icon.extend({
	options: {
		iconSize: [30, 47],
		iconAnchor: [16, 46],
		popupAnchor: [0, -45],
		tooltipAnchor: [0, -45]
	}
});

const shopMarker = new LeafletIcon({
	iconUrl: require('../../icons/ShopIcon.png')
});

const barMarker = new LeafletIcon({
	iconUrl: require('../../icons/PubIcon.png')
});

const userMarker = new LeafletIcon({
	iconUrl: require('../../icons/UserIcon.png')
});

export default class Home extends Component {

	constructor(...props) {
		super(...props);

		this.state = {
			lat: 48.148598,
			lon: 17.107748,
			zoom: 13,
			bars: [],
			radius: 5000,
			shopRadius: 500,
			parkAreas: [],
			cityParts: [],
			cityPartsSelected: [],
			nearbyShops: [],
			streets: [],
			streetsSelected: [],
			streetsOnMap: [],
			mapEntities: []
		};
		this.getMyLocation = this.getMyLocation.bind(this);
		this.selectCityPart = this.selectCityPart.bind(this);
		this.getNearbyShops = this.getNearbyShops.bind(this);
		this.getNearbyBarsInRadius = this.getNearbyBarsInRadius.bind(this);
		this.handleInputChange = this.handleInputChange.bind(this);
		this.getCityParts = this.getCityParts.bind(this);
		this.getNearbyStreetsInRadius = this.getNearbyStreetsInRadius.bind(this);
		this.getStreetsInCityParts = this.getStreetsInCityParts.bind(this);
		this.selectStreet = this.selectStreet.bind(this);
		this.getBarsInCityParts = this.getBarsInCityParts.bind(this);
		this.getParksInCityPart = this.getParksInCityPart.bind(this);
		this.getBarsOnStreet = this.getStreetsWithBars.bind(this);
		this.getStreetsWithBars = this.getStreetsWithBars.bind(this);
		this.pointToLayer = this.pointToLayer.bind(this);
		this.onEachFeature = this.onEachFeature.bind(this);
		this.style = this.style.bind(this);
	}

	componentDidMount() {
		this.getCityParts();
		this.getNearbyStreetsInRadius();
	}

	getMyLocation() {
		if ("geolocation" in navigator) {

			navigator.geolocation.getCurrentPosition(
				(position) => {
					this.setState({
						...this.state,
						lat: position.coords.latitude,
						lon: position.coords.longitude,
					});
				},
				(error_message) => {
					alert('An error has occured' + error_message)
				}
			);
		}
		else {
			alert("Geolocation is not supported by your browser");
		}
	}

	getCityParts() {
		fetch(`api/Bars/GetCityParts`)
			.then(response => response.json())
			.then(response => {
				this.setState({
					...this.state,
					cityParts: response.map((r) => { return { label: r, value: r } })
				})
			}, error => {
				console.log(error);
			});
	}

	getNearbyBarsInRadius() {
		fetch(`api/Bars/GetBars?centerLat=${this.state.lat}&centerLon=${this.state.lon}&radius=${this.state.radius}`)
			.then(response => response.json())
			.then(response => {
				this.setState({
					...this.state,
					mapEntities: response
				})
			}, error => {
				console.log(error);
			});
	}

	getNearbyStreetsInRadius() {
		fetch(`api/Bars/GetNearbyStreets?centerLat=${this.state.lat}&centerLon=${this.state.lon}&radius=${this.state.radius}`)
			.then(response => response.json())
			.then(response => {
				this.setState({
					...this.state,
					streets: response.map((r) => { return { label: r, value: r } })
				})
			}, error => {
				console.log(error);
			});
	}

	getStreetsInCityParts(x) {
		fetch(`api/Bars/GetStreetsInCityParts`,
			{
				method: 'POST',
				body: JSON.stringify(x),
				headers:
				{
					"Content-Type": "application/json"
				}
			})
			.then(response => response.json())
			.then(response => {
				this.setState({
					...this.state,
					streets: response.map((r) => { return { label: r, value: r } })
				})
			}, error => {
				console.log(error);
			});
	}

	getBarsInCityParts() {
		let x = this.state.cityPartsSelected.map((p) => { return p.value });
		fetch(`api/Bars/GetBars`,
			{
				method: 'POST',
				body: JSON.stringify(x),
				headers:
				{
					"Content-Type": "application/json"
				}
			})
			.then(response => response.json())
			.then(response => {
				this.setState({
					...this.state,
					mapEntities: response
				})
			}, error => {
				console.log(error);
			});
	}

	selectCityPart(e) {
		this.setState({ ...this.state, cityPartsSelected: e });
		let x = e.map((p) => { return p.value });
		this.getStreetsInCityParts(x);
	}

	getParksInCityPart() {
		let x = this.state.cityPartsSelected.map((p) => { return p.value });
		fetch(`api/Bars/GetParks`,
			{
				method: 'POST',
				body: JSON.stringify(x),
				headers:
				{
					"Content-Type": "application/json"
				}
			})
			.then(response => response.json())
			.then(response => {
				this.setState({
					...this.state,
					mapEntities: response
				});
			}, error => {
				console.log(error);
			});
	}

	selectStreet(e) {
		this.setState({ ...this.state, streetsSelected: e });
	}

	getStreetsWithBars() {
		let x = this.state.streetsSelected.map((p) => { return p.value });
		fetch(`api/Bars/GetStreetsWithBars`,
			{
				method: 'POST',
				body: JSON.stringify(x),
				headers:
				{
					"Content-Type": "application/json"
				}
			})
			.then(response => response.json())
			.then(response => {
				this.setState({
					...this.state,
					mapEntities: response
				})
			}, error => {
				console.log(error);
			})
	}

	getNearbyShops(id) {
		fetch(`api/Bars/GetNearbyShops?parkId=${id}&radius=${this.state.shopRadius}`,
			{
				method: 'POST',
				body: JSON.stringify( this.state.cityPartsSelected.map((p) => { return p.value })),
				headers:
				{
					"Content-Type": "application/json"
				}
			})
			.then(response => response.json())
			.then(response => {
				this.setState({
					...this.state,
					mapEntities: response
				})
			}, error => {
				console.log(error);
			})
	}

	handleInputChange(event) {
		const target = event.target;
		const value = target.type === 'checkbox' ? target.checked : target.value;
		const name = target.name;

		this.setState({
			...this.state,
			[name]: value
		});
	}

	pointToLayer(feature, featureLayer)
	{
		if (feature.geometry.type === "Point")
		{
			if (feature.properties.hasOwnProperty("ShopType")) {
				return L.marker(featureLayer, {
					icon: shopMarker,
					...feature.properties
				}).bindPopup(feature.properties.Name + "<br/>" + feature.properties.ShopType)
			}
			else if (feature.properties.hasOwnProperty("BarType")) {
				return L.marker(featureLayer, {
					icon: barMarker,
					...feature.properties
				}).bindPopup(feature.properties.Name + "<br/>" + feature.properties.BarType)
			}
		}
	}
	onEachFeature(feature, featureLayer)
	{
		let self = this;
		if (feature.geometry.type === "Polygon") {
			featureLayer.on({
				'click': function (e) {
					console.log(feature.properties);
					self.getNearbyShops(feature.properties.Id);
				}
			});
		}
	}

	style(feature) {
		if (feature.geometry.type === "Polygon") {
			console.log(feature.properties);
			if (feature.properties.HasBenches === true) {
				return {
					fillColor: 'lightblue',
					weight: 2,
					opacity: 1,
					color: 'blue',
					fillOpacity: 0.7
				}
			}
			else {
				return {
					fillColor: 'indianred',
					weight: 2,
					opacity: 1,
					color: 'red',
					fillOpacity: 0.7
				}
			}
		}
	}

	render() {

		const position = [this.state.lat, this.state.lon];

		return (
			<div>
				<label>Choose city districts:</label>
				<Select value={this.state.cityPartsSelected} options={this.state.cityParts} onChange={this.selectCityPart} isMulti placeholder="Choose city districts: e.g. Rača" />
				<label>Choose streets:</label>
				<Select value={this.state.streetsSelected} options={this.state.streets} onChange={this.selectStreet} isMulti placeholder="Choose streets: e.g. Obchodná" />
				<button onClick={this.getMyLocation}>GetGeolocation</button>
				<button onClick={this.getNearbyBarsInRadius}>Get nearby bars in radius</button>
				<button onClick={this.getBarsInCityParts}>Get bars in selected city districts</button>
				<button onClick={this.getParksInCityPart}>Get parks in selected city districts</button>
				<button onClick={this.getStreetsWithBars}>Get bars on selected streets</button>
				<label>Choose desired distance for nearby bars:</label>
				<input name="radius" type="number" value={this.state.radius} onChange={this.handleInputChange} />
				<label>Choose desired distance between parks and shops:</label>
				<input name="shopRadius" type="number" value={this.state.shopRadius} onChange={this.handleInputChange} />
				<Map center={position} zoom={this.state.zoom}>
					<TileLayer
						attribution="&amp;copy <a href=&quot;http://osm.org/copyright&quot;>OpenStreetMap</a> contributors"
						url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
					/>
					<Marker position={position} icon={userMarker}>
						<Tooltip direction={"top"} permanent>Your location</Tooltip>
					</Marker>
					<GeoJSON key={hash(this.state.mapEntities)} data={this.state.mapEntities} onEachFeature={this.onEachFeature} pointToLayer={this.pointToLayer} style={this.style}>
					</GeoJSON>
				</Map>
			</div>
		)
	}
}
