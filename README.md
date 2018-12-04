# General course assignment

Build a map-based application, which lets the user see geo-based data on a map and filter/search through it in a meaningfull way. Specify the details and build it in your language of choice. The application should have 3 components:

1. Custom-styled background map, ideally built with [mapbox](http://mapbox.com). Hard-core mode: you can also serve the map tiles yourself using [mapnik](http://mapnik.org/) or similar tool.
2. Local server with [PostGIS](http://postgis.net/) and an API layer that exposes data in a [geojson format](http://geojson.org/).
3. The user-facing application (web, android, ios, your choice..) which calls the API and lets the user see and navigate in the map and shows the geodata. You can (and should) use existing components, such as the Mapbox SDK, or [Leaflet](http://leafletjs.com/).

## Example projects

- Showing nearby landmarks as colored circles, each type of landmark has different circle color and the more interesting the landmark is, the bigger the circle. Landmarks are sorted in a sidebar by distance to the user. It is possible to filter only certain landmark types (e.g., castles).

- Showing bicykle roads on a map. The roads are color-coded based on the road difficulty. The user can see various lists which help her choose an appropriate road, e.g. roads that cross a river, roads that are nearby lakes, roads that pass through multiple countries, etc.

## Data sources

- [Open Street Maps](https://www.openstreetmap.org/)

## My project

Made by: Matúš Buzássy

Fill in (either in English, or in Slovak):

**Application description**: 

Web application written in C# on backend and React.js on frontend. BE is exposed via REST API that outputs GeoJSON feature collections. Data are retrieved from PostGis database. On Frontend a Leaflet.js is used for displaying map data. Frontend displays information about bars, pubs, shops, parks and user location. 

Three base scenarios handled by this application:
1. Searching for bars and pubs in radius or in selected city district
2. Searching for parks in radius or city district
	2a. Searching for shops in close proximity of a park (shops that can be found: tobacco, alcohol, wine stores or supermarkets)
3. Searching for bars and pubs on selected streets in radius or in city district

**Data source**: 

- [Open Street Maps](https://www.openstreetmap.org/)

**Technologies used**: 

Frontend
- React.js
- Leaflet.js
- Mapbox

Backend
- C# 
- ASP.NET Core (Web server shell)
- PostGIS (Database)
- EntityFramework Core(Database orm on c# side)
- NetTopologySuite (Nuget for transforming post gis functions for entity framework)
- GeoJSON.Net (GeoJSON parser)





