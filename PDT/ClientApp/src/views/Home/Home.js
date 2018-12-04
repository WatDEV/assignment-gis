import React, { Component } from 'react';
import { Map, TileLayer, Marker, Tooltip, GeoJSON } from 'react-leaflet';
import { TabContent, TabPane, Nav, NavItem, NavLink, Button, Row, Col, Input, Label, FormGroup } from 'reactstrap';
import classnames from 'classnames';
import 'leaflet/dist/leaflet.css';
import L from 'leaflet';
import Select from 'react-select';
import hash from 'object-hash';
import StreetMapboxLayer from './StreetMapboxLayer';
import SateliteMapboxLayer from './SateliteMapboxLayer';

delete L.Icon.Default.prototype._getIconUrl;

L.Icon.Default.mergeOptions({
	iconRetinaUrl: require('leaflet/dist/images/marker-icon-2x.png'),
	iconUrl: require('leaflet/dist/images/marker-icon.png')
});

let LeafletIcon = L.Icon.extend({
	options: {
		iconSize: [30, 47],
		iconAnchor: [15, 47],
		popupAnchor: [0, -47],
		tooltipAnchor: [0, -47]
	}
});

let ShopIcon = L.Icon.extend({
	options: {
		iconSize: [45, 45],
		iconAnchor: [22, 45],
		popupAnchor: [0, -45],
		tooltipAnchor: [0, -45]
	}
});

const alcoholMarker = new ShopIcon({
	iconUrl: require('../../icons/AlcoholIcon.png')
});
const wineMarker = new ShopIcon({
	iconUrl: require('../../icons/WineIcon.png')
});
const coffeeMarker = new ShopIcon({
	iconUrl: require('../../icons/TobaccoIcon.png')
});
const shopMarker = new ShopIcon({
	iconUrl: require('../../icons/ShopIcon.png')
});

const barMarker = new ShopIcon({
	iconUrl: require('../../icons/BarIcon.png')
});

const pubMarker = new ShopIcon({
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
			radius: 5000,
			shopRadius: 500,
			cityParts: [],
			cityPartsSelected: [],
			nearbyShops: [],
			streets: [],
			streetsSelected: [],
			mapEntities: [],
			activeTab: "1",
			isStreetView: true
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
		this.getNearbyParks = this.getNearbyParks.bind(this);
		this.getBarsOnStreet = this.getStreetsWithBars.bind(this);
		this.getStreetsWithBars = this.getStreetsWithBars.bind(this);
		this.pointToLayer = this.pointToLayer.bind(this);
		this.onEachFeature = this.onEachFeature.bind(this);
		this.style = this.style.bind(this);
		this.toggle = this.toggle.bind(this);
	}

	componentDidMount() {
		this.getCityParts();
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

	getNearbyParks() {
		fetch(`api/Bars/GetParks?centerLat=${this.state.lat}&centerLon=${this.state.lon}&radius=${this.state.radius}`)
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
		fetch(`api/Bars/GetNearbyShops?parkId=${id}&shopRadius=${this.state.shopRadius}&centerLat=${this.state.lat}&centerLon=${this.state.lon}&radius=${this.state.radius}`,
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
				if (feature.properties.ShopType === "alcohol") {
					return L.marker(featureLayer, {
						icon: alcoholMarker,
						...feature.properties
					}).bindPopup(feature.properties.Name + "<br/>" + feature.properties.ShopType)

				}
				else if (feature.properties.ShopType === "wine") {
					return L.marker(featureLayer, {
						icon: wineMarker,
						...feature.properties
					}).bindPopup(feature.properties.Name + "<br/>" + feature.properties.ShopType)

				}
				else if (feature.properties.ShopType === "tobacco" || feature.properties.ShopType === "coffee") {
					return L.marker(featureLayer, {
						icon: coffeeMarker,
						...feature.properties
					}).bindPopup(feature.properties.Name + "<br/>" + feature.properties.ShopType)

				}
				return L.marker(featureLayer, {
					icon: shopMarker,
					...feature.properties
				}).bindPopup(feature.properties.Name + "<br/>" + feature.properties.ShopType)
			}
			else if (feature.properties.hasOwnProperty("BarType")) {
				if (feature.properties.BarType === "bar") {
					return L.marker(featureLayer, {
						icon: barMarker,
						...feature.properties
					}).bindPopup(feature.properties.Name + "<br/>" + feature.properties.BarType)
				}
				return L.marker(featureLayer, {
					icon: pubMarker,
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
		else if (feature.geometry.type == "LineString") {
			return {
				weight: 4,
				opacity: 1,
				color: 'green',
			}
		}
	}

	toggle(tab) {
		if (this.state.activeTab !== tab) {
			this.setState({
				...this.state,
				activeTab: tab
			});
		}
	}

	render() {

		const position = [this.state.lat, this.state.lon];

		return (
			<div>
				<Map center={position} zoom={this.state.zoom} ref={(map) => { this.state.mapRef = map; }}>
					{this.state.isStreetView && <StreetMapboxLayer />}
					{!this.state.isStreetView && <SateliteMapboxLayer />}					
					<TileLayer
						attribution="&amp;copy <a href=&quot;http://osm.org/copyright&quot;>OpenStreetMap</a> contributors"
						url=""
					/>
					<Marker position={position} icon={userMarker}>
						<Tooltip direction={"top"} permanent>Your location</Tooltip>
					</Marker>
					<GeoJSON key={hash(this.state.mapEntities)} data={this.state.mapEntities} onEachFeature={this.onEachFeature} pointToLayer={this.pointToLayer} style={this.style}></GeoJSON>
				</Map>
				<div className="custom-tabs">
					<Nav tabs>
						<NavItem>
							<NavLink
								className={classnames({ active: this.state.activeTab === '1' })}
								onClick={() => { this.toggle('1'); }}
							>
								Nearby entities
							</NavLink>
						</NavItem>
						<NavItem>
							<NavLink
								className={classnames({ active: this.state.activeTab === '2' })}
								onClick={() => { this.toggle('2'); }}
							>
								City districts
							</NavLink>
						</NavItem>
					</Nav>
					<TabContent activeTab={this.state.activeTab}>
						<TabPane tabId="1">
							<Row>
								<Col sm="12">
									<FormGroup>
										<Label>Choose streets:</Label>
										<Select value={this.state.streetsSelected} options={this.state.streets} onChange={this.selectStreet} isMulti placeholder="Choose streets: e.g. Obchodná" />
									</FormGroup>
									<FormGroup>
										<Label>Choose desired distance for nearby elements:</Label>
										<Input name="radius" type="number" value={this.state.radius} onChange={this.handleInputChange} />
									</FormGroup>
									<FormGroup>
										<Label for="streetView1">Display street view: </Label>
										<Input id="streetView1" type="checkbox" name="isStreetView" checked={this.state.isStreetView} onChange={this.handleInputChange}
										/>
									</FormGroup>
									<FormGroup>
										<Button onClick={this.getNearbyBarsInRadius} color="primary">Find nearby bars in radius</Button>
										<Button onClick={this.getNearbyParks} color="primary">Find nearby parks</Button>
										<Button onClick={this.getNearbyStreetsInRadius} color="primary">Find nearby streets</Button>
										<Button onClick={this.getStreetsWithBars} color="primary">Find bars on selected streets</Button>
									</FormGroup>
								</Col>
							</Row>
						</TabPane>
						<TabPane tabId="2">
							<Row>
								<Col sm="12">
									<Row>
										<Col sm="6">
											<FormGroup>
												<Label>Choose city districts:</Label>
												<Select value={this.state.cityPartsSelected} options={this.state.cityParts} onChange={this.selectCityPart} isMulti placeholder="Choose city districts: e.g. Rača" />
											</FormGroup>
										</Col>
										<Col sm="6">
											<FormGroup>
												<Label>Choose streets:</Label>
												<Select value={this.state.streetsSelected} options={this.state.streets} onChange={this.selectStreet} isMulti placeholder="Choose streets: e.g. Obchodná" />
											</FormGroup>
										</Col>
									</Row>
									<FormGroup>
										<Label>Choose desired distance between parks and shops:</Label>
										<Input name="shopRadius" type="number" value={this.state.shopRadius} onChange={this.handleInputChange} />
									</FormGroup>
									<FormGroup>
										<Label for="streetView2">Display street view: </Label>
										<Input id="streetView2" type="checkbox" name="isStreetView" checked={this.state.isStreetView} onChange={this.handleInputChange}
										/>
									</FormGroup>
									<Button onClick={this.getMyLocation} color="primary">Get current geolocation</Button>
									<Button onClick={this.getBarsInCityParts} color="primary">Find bars in selected city districts</Button>
									<Button onClick={this.getParksInCityPart} color="primary">Find parks in selected city districts</Button>
									<Button onClick={this.getStreetsWithBars} color="primary">Find bars on selected streets</Button>
								</Col>
							</Row>
						</TabPane>
					</TabContent>
				</div>
			</div>
		)
	}
}
