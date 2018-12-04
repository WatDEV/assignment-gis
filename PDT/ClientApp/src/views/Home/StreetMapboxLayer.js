import L from "leaflet";
import {} from "mapbox-gl-leaflet";
import PropTypes from "prop-types";
import { GridLayer, withLeaflet } from "react-leaflet";

class StreetMapboxLayer extends GridLayer {
	createLeafletElement(props) {
		return L.mapboxGL(props);
	}
}

StreetMapboxLayer.propTypes = {
	accessToken: PropTypes.string,
	style: PropTypes.string
};

StreetMapboxLayer.defaultProps = {
	accessToken: "pk.eyJ1IjoiYnV6YXNzeSIsImEiOiJjanA5Zzk3Z3UwN3UzM3FwbHJ3ZnN3aDlnIn0.jg2ByjvaE1MP9ujMmCHuYw",
	style: "mapbox://styles/buzassy/cjp9gm0050x5i2snv6i3ltq4v"
};

export default withLeaflet(StreetMapboxLayer); 