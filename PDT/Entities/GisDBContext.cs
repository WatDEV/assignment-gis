using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PDT.Entities
{
    public partial class GisDBContext : DbContext
    {
        public GisDBContext()
        {
        }

        public GisDBContext(DbContextOptions<GisDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PlanetOsmLine> PlanetOsmLine { get; set; }
        public virtual DbSet<PlanetOsmPoint> PlanetOsmPoint { get; set; }
        public virtual DbSet<PlanetOsmPolygon> PlanetOsmPolygon { get; set; }
        public virtual DbSet<PlanetOsmRoads> PlanetOsmRoads { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Database=gisdb;Username=postgres;Password=postgres");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("address_standardizer")
                .HasPostgresExtension("address_standardizer_data_us")
                .HasPostgresExtension("fuzzystrmatch")
                .HasPostgresExtension("hstore")
                .HasPostgresExtension("ogr_fdw")
                .HasPostgresExtension("pgrouting")
                .HasPostgresExtension("postgis")
                .HasPostgresExtension("postgis_sfcgal")
                .HasPostgresExtension("postgis_tiger_geocoder")
                .HasPostgresExtension("postgis_topology");

            modelBuilder.Entity<PlanetOsmLine>(entity =>
            {
                entity.ToTable("planet_osm_line");

                entity.HasIndex(e => e.Tags)
                    .HasName("idx_planet_osm_line_tags")
                    .ForNpgsqlHasMethod("gist");

                entity.HasIndex(e => e.Way)
                    .HasName("planet_osm_line_index")
                    .ForNpgsqlHasMethod("gist");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Access).HasColumnName("access");

                entity.Property(e => e.AddrHousename).HasColumnName("addr:housename");

                entity.Property(e => e.AddrHousenumber).HasColumnName("addr:housenumber");

                entity.Property(e => e.AddrInterpolation).HasColumnName("addr:interpolation");

                entity.Property(e => e.AdminLevel).HasColumnName("admin_level");

                entity.Property(e => e.Aerialway).HasColumnName("aerialway");

                entity.Property(e => e.Aeroway).HasColumnName("aeroway");

                entity.Property(e => e.Amenity).HasColumnName("amenity");

                entity.Property(e => e.Area).HasColumnName("area");

                entity.Property(e => e.Barrier).HasColumnName("barrier");

                entity.Property(e => e.Bicycle).HasColumnName("bicycle");

                entity.Property(e => e.Boundary).HasColumnName("boundary");

                entity.Property(e => e.Brand).HasColumnName("brand");

                entity.Property(e => e.Bridge).HasColumnName("bridge");

                entity.Property(e => e.Building).HasColumnName("building");

                entity.Property(e => e.Construction).HasColumnName("construction");

                entity.Property(e => e.Covered).HasColumnName("covered");

                entity.Property(e => e.Culvert).HasColumnName("culvert");

                entity.Property(e => e.Cutting).HasColumnName("cutting");

                entity.Property(e => e.Denomination).HasColumnName("denomination");

                entity.Property(e => e.Disused).HasColumnName("disused");

                entity.Property(e => e.Embankment).HasColumnName("embankment");

                entity.Property(e => e.Foot).HasColumnName("foot");

                entity.Property(e => e.GeneratorSource).HasColumnName("generator:source");

                entity.Property(e => e.Harbour).HasColumnName("harbour");

                entity.Property(e => e.Highway).HasColumnName("highway");

                entity.Property(e => e.Historic).HasColumnName("historic");

                entity.Property(e => e.Horse).HasColumnName("horse");

                entity.Property(e => e.Intermittent).HasColumnName("intermittent");

                entity.Property(e => e.Junction).HasColumnName("junction");

                entity.Property(e => e.Landuse).HasColumnName("landuse");

                entity.Property(e => e.Layer).HasColumnName("layer");

                entity.Property(e => e.Leisure).HasColumnName("leisure");

                entity.Property(e => e.Lock).HasColumnName("lock");

                entity.Property(e => e.ManMade).HasColumnName("man_made");

                entity.Property(e => e.Military).HasColumnName("military");

                entity.Property(e => e.Motorcar).HasColumnName("motorcar");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Natural).HasColumnName("natural");

                entity.Property(e => e.Office).HasColumnName("office");

                entity.Property(e => e.Oneway).HasColumnName("oneway");

                entity.Property(e => e.Operator).HasColumnName("operator");

                entity.Property(e => e.OsmId).HasColumnName("osm_id");

                entity.Property(e => e.Place).HasColumnName("place");

                entity.Property(e => e.Population).HasColumnName("population");

                entity.Property(e => e.Power).HasColumnName("power");

                entity.Property(e => e.PowerSource).HasColumnName("power_source");

                entity.Property(e => e.PublicTransport).HasColumnName("public_transport");

                entity.Property(e => e.Railway).HasColumnName("railway");

                entity.Property(e => e.Ref).HasColumnName("ref");

                entity.Property(e => e.Religion).HasColumnName("religion");

                entity.Property(e => e.Route).HasColumnName("route");

                entity.Property(e => e.Service).HasColumnName("service");

                entity.Property(e => e.Shop).HasColumnName("shop");

                entity.Property(e => e.Sport).HasColumnName("sport");

                entity.Property(e => e.Surface).HasColumnName("surface");

                entity.Property(e => e.Tags).HasColumnName("tags");

                entity.Property(e => e.Toll).HasColumnName("toll");

                entity.Property(e => e.Tourism).HasColumnName("tourism");

                entity.Property(e => e.TowerType).HasColumnName("tower:type");

                entity.Property(e => e.Tracktype).HasColumnName("tracktype");

                entity.Property(e => e.Tunnel).HasColumnName("tunnel");

                entity.Property(e => e.Water).HasColumnName("water");

                entity.Property(e => e.Waterway).HasColumnName("waterway");

                entity.Property(e => e.Way)
                    .HasColumnName("way")
                    .HasColumnType("geometry(LineString,3857)");

                entity.Property(e => e.WayArea).HasColumnName("way_area");

                entity.Property(e => e.Wetland).HasColumnName("wetland");

                entity.Property(e => e.Width).HasColumnName("width");

                entity.Property(e => e.Wood).HasColumnName("wood");

                entity.Property(e => e.ZOrder).HasColumnName("z_order");
            });

            modelBuilder.Entity<PlanetOsmPoint>(entity =>
            {
                entity.ToTable("planet_osm_point");

                entity.HasIndex(e => e.Tags)
                    .HasName("idx_planet_osm_point_tags")
                    .ForNpgsqlHasMethod("gist");

                entity.HasIndex(e => e.Way)
                    .HasName("planet_osm_point_index")
                    .ForNpgsqlHasMethod("gist");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Access).HasColumnName("access");

                entity.Property(e => e.AddrHousename).HasColumnName("addr:housename");

                entity.Property(e => e.AddrHousenumber).HasColumnName("addr:housenumber");

                entity.Property(e => e.AddrInterpolation).HasColumnName("addr:interpolation");

                entity.Property(e => e.AdminLevel).HasColumnName("admin_level");

                entity.Property(e => e.Aerialway).HasColumnName("aerialway");

                entity.Property(e => e.Aeroway).HasColumnName("aeroway");

                entity.Property(e => e.Amenity).HasColumnName("amenity");

                entity.Property(e => e.Area).HasColumnName("area");

                entity.Property(e => e.Barrier).HasColumnName("barrier");

                entity.Property(e => e.Bicycle).HasColumnName("bicycle");

                entity.Property(e => e.Boundary).HasColumnName("boundary");

                entity.Property(e => e.Brand).HasColumnName("brand");

                entity.Property(e => e.Bridge).HasColumnName("bridge");

                entity.Property(e => e.Building).HasColumnName("building");

                entity.Property(e => e.Capital).HasColumnName("capital");

                entity.Property(e => e.Construction).HasColumnName("construction");

                entity.Property(e => e.Covered).HasColumnName("covered");

                entity.Property(e => e.Culvert).HasColumnName("culvert");

                entity.Property(e => e.Cutting).HasColumnName("cutting");

                entity.Property(e => e.Denomination).HasColumnName("denomination");

                entity.Property(e => e.Disused).HasColumnName("disused");

                entity.Property(e => e.Ele).HasColumnName("ele");

                entity.Property(e => e.Embankment).HasColumnName("embankment");

                entity.Property(e => e.Foot).HasColumnName("foot");

                entity.Property(e => e.GeneratorSource).HasColumnName("generator:source");

                entity.Property(e => e.Harbour).HasColumnName("harbour");

                entity.Property(e => e.Highway).HasColumnName("highway");

                entity.Property(e => e.Historic).HasColumnName("historic");

                entity.Property(e => e.Horse).HasColumnName("horse");

                entity.Property(e => e.Intermittent).HasColumnName("intermittent");

                entity.Property(e => e.Junction).HasColumnName("junction");

                entity.Property(e => e.Landuse).HasColumnName("landuse");

                entity.Property(e => e.Layer).HasColumnName("layer");

                entity.Property(e => e.Leisure).HasColumnName("leisure");

                entity.Property(e => e.Lock).HasColumnName("lock");

                entity.Property(e => e.ManMade).HasColumnName("man_made");

                entity.Property(e => e.Military).HasColumnName("military");

                entity.Property(e => e.Motorcar).HasColumnName("motorcar");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Natural).HasColumnName("natural");

                entity.Property(e => e.Office).HasColumnName("office");

                entity.Property(e => e.Oneway).HasColumnName("oneway");

                entity.Property(e => e.Operator).HasColumnName("operator");

                entity.Property(e => e.OsmId).HasColumnName("osm_id");

                entity.Property(e => e.Place).HasColumnName("place");

                entity.Property(e => e.Population).HasColumnName("population");

                entity.Property(e => e.Power).HasColumnName("power");

                entity.Property(e => e.PowerSource).HasColumnName("power_source");

                entity.Property(e => e.PublicTransport).HasColumnName("public_transport");

                entity.Property(e => e.Railway).HasColumnName("railway");

                entity.Property(e => e.Ref).HasColumnName("ref");

                entity.Property(e => e.Religion).HasColumnName("religion");

                entity.Property(e => e.Route).HasColumnName("route");

                entity.Property(e => e.Service).HasColumnName("service");

                entity.Property(e => e.Shop).HasColumnName("shop");

                entity.Property(e => e.Sport).HasColumnName("sport");

                entity.Property(e => e.Surface).HasColumnName("surface");

                entity.Property(e => e.Tags).HasColumnName("tags");

                entity.Property(e => e.Toll).HasColumnName("toll");

                entity.Property(e => e.Tourism).HasColumnName("tourism");

                entity.Property(e => e.TowerType).HasColumnName("tower:type");

                entity.Property(e => e.Tunnel).HasColumnName("tunnel");

                entity.Property(e => e.Water).HasColumnName("water");

                entity.Property(e => e.Waterway).HasColumnName("waterway");

                entity.Property(e => e.Way)
                    .HasColumnName("way")
                    .HasColumnType("geometry(Point,3857)");

                entity.Property(e => e.Wetland).HasColumnName("wetland");

                entity.Property(e => e.Width).HasColumnName("width");

                entity.Property(e => e.Wood).HasColumnName("wood");

                entity.Property(e => e.ZOrder).HasColumnName("z_order");
            });

            modelBuilder.Entity<PlanetOsmPolygon>(entity =>
            {
                entity.ToTable("planet_osm_polygon");

                entity.HasIndex(e => e.Tags)
                    .HasName("idx_planet_osm_polygon_tags")
                    .ForNpgsqlHasMethod("gist");

                entity.HasIndex(e => e.Way)
                    .HasName("planet_osm_polygon_index")
                    .ForNpgsqlHasMethod("gist");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Access).HasColumnName("access");

                entity.Property(e => e.AddrHousename).HasColumnName("addr:housename");

                entity.Property(e => e.AddrHousenumber).HasColumnName("addr:housenumber");

                entity.Property(e => e.AddrInterpolation).HasColumnName("addr:interpolation");

                entity.Property(e => e.AdminLevel).HasColumnName("admin_level");

                entity.Property(e => e.Aerialway).HasColumnName("aerialway");

                entity.Property(e => e.Aeroway).HasColumnName("aeroway");

                entity.Property(e => e.Amenity).HasColumnName("amenity");

                entity.Property(e => e.Area).HasColumnName("area");

                entity.Property(e => e.Barrier).HasColumnName("barrier");

                entity.Property(e => e.Bicycle).HasColumnName("bicycle");

                entity.Property(e => e.Boundary).HasColumnName("boundary");

                entity.Property(e => e.Brand).HasColumnName("brand");

                entity.Property(e => e.Bridge).HasColumnName("bridge");

                entity.Property(e => e.Building).HasColumnName("building");

                entity.Property(e => e.Construction).HasColumnName("construction");

                entity.Property(e => e.Covered).HasColumnName("covered");

                entity.Property(e => e.Culvert).HasColumnName("culvert");

                entity.Property(e => e.Cutting).HasColumnName("cutting");

                entity.Property(e => e.Denomination).HasColumnName("denomination");

                entity.Property(e => e.Disused).HasColumnName("disused");

                entity.Property(e => e.Embankment).HasColumnName("embankment");

                entity.Property(e => e.Foot).HasColumnName("foot");

                entity.Property(e => e.GeneratorSource).HasColumnName("generator:source");

                entity.Property(e => e.Harbour).HasColumnName("harbour");

                entity.Property(e => e.Highway).HasColumnName("highway");

                entity.Property(e => e.Historic).HasColumnName("historic");

                entity.Property(e => e.Horse).HasColumnName("horse");

                entity.Property(e => e.Intermittent).HasColumnName("intermittent");

                entity.Property(e => e.Junction).HasColumnName("junction");

                entity.Property(e => e.Landuse).HasColumnName("landuse");

                entity.Property(e => e.Layer).HasColumnName("layer");

                entity.Property(e => e.Leisure).HasColumnName("leisure");

                entity.Property(e => e.Lock).HasColumnName("lock");

                entity.Property(e => e.ManMade).HasColumnName("man_made");

                entity.Property(e => e.Military).HasColumnName("military");

                entity.Property(e => e.Motorcar).HasColumnName("motorcar");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Natural).HasColumnName("natural");

                entity.Property(e => e.Office).HasColumnName("office");

                entity.Property(e => e.Oneway).HasColumnName("oneway");

                entity.Property(e => e.Operator).HasColumnName("operator");

                entity.Property(e => e.OsmId).HasColumnName("osm_id");

                entity.Property(e => e.Place).HasColumnName("place");

                entity.Property(e => e.Population).HasColumnName("population");

                entity.Property(e => e.Power).HasColumnName("power");

                entity.Property(e => e.PowerSource).HasColumnName("power_source");

                entity.Property(e => e.PublicTransport).HasColumnName("public_transport");

                entity.Property(e => e.Railway).HasColumnName("railway");

                entity.Property(e => e.Ref).HasColumnName("ref");

                entity.Property(e => e.Religion).HasColumnName("religion");

                entity.Property(e => e.Route).HasColumnName("route");

                entity.Property(e => e.Service).HasColumnName("service");

                entity.Property(e => e.Shop).HasColumnName("shop");

                entity.Property(e => e.Sport).HasColumnName("sport");

                entity.Property(e => e.Surface).HasColumnName("surface");

                entity.Property(e => e.Tags).HasColumnName("tags");

                entity.Property(e => e.Toll).HasColumnName("toll");

                entity.Property(e => e.Tourism).HasColumnName("tourism");

                entity.Property(e => e.TowerType).HasColumnName("tower:type");

                entity.Property(e => e.Tracktype).HasColumnName("tracktype");

                entity.Property(e => e.Tunnel).HasColumnName("tunnel");

                entity.Property(e => e.Water).HasColumnName("water");

                entity.Property(e => e.Waterway).HasColumnName("waterway");

                entity.Property(e => e.Way)
                    .HasColumnName("way")
                    .HasColumnType("geometry(Geometry,3857)");

                entity.Property(e => e.WayArea).HasColumnName("way_area");

                entity.Property(e => e.Wetland).HasColumnName("wetland");

                entity.Property(e => e.Width).HasColumnName("width");

                entity.Property(e => e.Wood).HasColumnName("wood");

                entity.Property(e => e.ZOrder).HasColumnName("z_order");
            });

            modelBuilder.Entity<PlanetOsmRoads>(entity =>
            {
                entity.ToTable("planet_osm_roads");

                entity.HasIndex(e => e.Way)
                    .HasName("planet_osm_roads_index")
                    .ForNpgsqlHasMethod("gist");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Access).HasColumnName("access");

                entity.Property(e => e.AddrHousename).HasColumnName("addr:housename");

                entity.Property(e => e.AddrHousenumber).HasColumnName("addr:housenumber");

                entity.Property(e => e.AddrInterpolation).HasColumnName("addr:interpolation");

                entity.Property(e => e.AdminLevel).HasColumnName("admin_level");

                entity.Property(e => e.Aerialway).HasColumnName("aerialway");

                entity.Property(e => e.Aeroway).HasColumnName("aeroway");

                entity.Property(e => e.Amenity).HasColumnName("amenity");

                entity.Property(e => e.Area).HasColumnName("area");

                entity.Property(e => e.Barrier).HasColumnName("barrier");

                entity.Property(e => e.Bicycle).HasColumnName("bicycle");

                entity.Property(e => e.Boundary).HasColumnName("boundary");

                entity.Property(e => e.Brand).HasColumnName("brand");

                entity.Property(e => e.Bridge).HasColumnName("bridge");

                entity.Property(e => e.Building).HasColumnName("building");

                entity.Property(e => e.Construction).HasColumnName("construction");

                entity.Property(e => e.Covered).HasColumnName("covered");

                entity.Property(e => e.Culvert).HasColumnName("culvert");

                entity.Property(e => e.Cutting).HasColumnName("cutting");

                entity.Property(e => e.Denomination).HasColumnName("denomination");

                entity.Property(e => e.Disused).HasColumnName("disused");

                entity.Property(e => e.Embankment).HasColumnName("embankment");

                entity.Property(e => e.Foot).HasColumnName("foot");

                entity.Property(e => e.GeneratorSource).HasColumnName("generator:source");

                entity.Property(e => e.Harbour).HasColumnName("harbour");

                entity.Property(e => e.Highway).HasColumnName("highway");

                entity.Property(e => e.Historic).HasColumnName("historic");

                entity.Property(e => e.Horse).HasColumnName("horse");

                entity.Property(e => e.Intermittent).HasColumnName("intermittent");

                entity.Property(e => e.Junction).HasColumnName("junction");

                entity.Property(e => e.Landuse).HasColumnName("landuse");

                entity.Property(e => e.Layer).HasColumnName("layer");

                entity.Property(e => e.Leisure).HasColumnName("leisure");

                entity.Property(e => e.Lock).HasColumnName("lock");

                entity.Property(e => e.ManMade).HasColumnName("man_made");

                entity.Property(e => e.Military).HasColumnName("military");

                entity.Property(e => e.Motorcar).HasColumnName("motorcar");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Natural).HasColumnName("natural");

                entity.Property(e => e.Office).HasColumnName("office");

                entity.Property(e => e.Oneway).HasColumnName("oneway");

                entity.Property(e => e.Operator).HasColumnName("operator");

                entity.Property(e => e.OsmId).HasColumnName("osm_id");

                entity.Property(e => e.Place).HasColumnName("place");

                entity.Property(e => e.Population).HasColumnName("population");

                entity.Property(e => e.Power).HasColumnName("power");

                entity.Property(e => e.PowerSource).HasColumnName("power_source");

                entity.Property(e => e.PublicTransport).HasColumnName("public_transport");

                entity.Property(e => e.Railway).HasColumnName("railway");

                entity.Property(e => e.Ref).HasColumnName("ref");

                entity.Property(e => e.Religion).HasColumnName("religion");

                entity.Property(e => e.Route).HasColumnName("route");

                entity.Property(e => e.Service).HasColumnName("service");

                entity.Property(e => e.Shop).HasColumnName("shop");

                entity.Property(e => e.Sport).HasColumnName("sport");

                entity.Property(e => e.Surface).HasColumnName("surface");

                entity.Property(e => e.Tags).HasColumnName("tags");

                entity.Property(e => e.Toll).HasColumnName("toll");

                entity.Property(e => e.Tourism).HasColumnName("tourism");

                entity.Property(e => e.TowerType).HasColumnName("tower:type");

                entity.Property(e => e.Tracktype).HasColumnName("tracktype");

                entity.Property(e => e.Tunnel).HasColumnName("tunnel");

                entity.Property(e => e.Water).HasColumnName("water");

                entity.Property(e => e.Waterway).HasColumnName("waterway");

                entity.Property(e => e.Way)
                    .HasColumnName("way")
                    .HasColumnType("geometry(LineString,3857)");

                entity.Property(e => e.WayArea).HasColumnName("way_area");

                entity.Property(e => e.Wetland).HasColumnName("wetland");

                entity.Property(e => e.Width).HasColumnName("width");

                entity.Property(e => e.Wood).HasColumnName("wood");

                entity.Property(e => e.ZOrder).HasColumnName("z_order");
            });
        }
    }
}
