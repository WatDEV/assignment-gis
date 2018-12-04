import L from "leaflet";
import {} from "mapbox-gl-leaflet";
import PropTypes from "prop-types";
import { GridLayer, withLeaflet } from "react-leaflet";

class SateliteMapboxLayer extends GridLayer {
	createLeafletElement(props) {
		return L.mapboxGL(props);
	}
}

SateliteMapboxLayer.propTypes = {
	accessToken: PropTypes.string,
	style: PropTypes.string
};

SateliteMapboxLayer.defaultProps = {
	accessToken: "pk.eyJ1IjoiYnV6YXNzeSIsImEiOiJjanA5Zzk3Z3UwN3UzM3FwbHJ3ZnN3aDlnIn0.jg2ByjvaE1MP9ujMmCHuYw",
	style: "mapbox://styles/buzassy/cjp9gb7h40rlq2rqp95e4f1az"
};

export default withLeaflet(SateliteMapboxLayer); 