import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import * as mapboxgl from 'mapbox-gl';
import {ContourLayer, GeoJsonLayer, HexagonLayer, ScatterplotLayer, ArcLayer} from '@deck.gl/layers';
import {MapboxLayer} from '@deck.gl/mapbox';
import {Deck, MapView, OrthographicView, WebMercatorViewport} from '@deck.gl/core';

@Component({
  selector: 'app-deckgl-layers',
  templateUrl: './deckgl-layers.component.html',
  styleUrls: ['./deckgl-layers.component.scss']
})
export class DeckglLayersComponent implements OnInit {


    constructor() { }

    @ViewChild("map") map: ElementRef;
    @ViewChild("deckCanvas") deckCanvas: ElementRef;


    colorToRGBArray(colour){

    }

    ngOnInit() {

        const AIR_PORTS =
            'https://d2ad6b4ur7yvpq.cloudfront.net/naturalearth-3.3.0/ne_10m_airports.geojson';

        const INITIAL_VIEW_STATE = {
            latitude: 51.47,
            longitude: 0.45,
            zoom: 4,
            bearing: 0,
            pitch: 30
        };

        const mapContainer = this.map.nativeElement;
        const deckCanvas = this.deckCanvas.nativeElement;
        // Any type because it's bug from types or set globally
        (<any>mapboxgl).accessToken = 'pk.eyJ1IjoidmljdGVyIiwiYSI6ImNqcGVpdXZqdTAwcHUzcHMwNnBpbGhodGkifQ._oepIpI1VgVhRVETpv5Cxw';
        console.log(mapContainer);
        const map = new mapboxgl.Map({
            container: mapContainer,
            style: 'mapbox://styles/victer/cju9v3xiq1n0b1fmlrdxb789h',
            // Note: deck.gl will be in charge of interaction and event handling
            interactive: false,
            center: [INITIAL_VIEW_STATE.longitude, INITIAL_VIEW_STATE.latitude],
            zoom: INITIAL_VIEW_STATE.zoom,
            bearing: INITIAL_VIEW_STATE.bearing,
            pitch: INITIAL_VIEW_STATE.pitch
        });


// // Set your mapbox token here
        const deck = new Deck(<any>{
            container: deckCanvas,
            width: '100%',
            height: '100%',
            initialViewState: INITIAL_VIEW_STATE,
            zoom: 8,
            controller: true,
            onViewStateChange: ({viewState}) => {
                map.jumpTo({
                    center: [viewState.longitude, viewState.latitude],
                    zoom: viewState.zoom,
                    bearing: viewState.bearing,
                    pitch: viewState.pitch
                });
            },
            layers: [
                <any>new GeoJsonLayer({
                    id: 'airports',
                    data: AIR_PORTS,
                    // Styles
                    filled: true,
                    pointRadiusMinPixels: 2,
                    opacity: 1,
                    pointRadiusScale: 2000,
                    getRadius: f => 11 - f.properties.scalerank,
                    getFillColor: [200, 0, 80, 180],
                    // Interactive props
                    pickable: true,
                    autoHighlight: true,
                    onClick: info =>
                        // eslint-disable-next-line
                        info.object && alert(`${info.object.properties.name} (${info.object.properties.abbrev})`)
                }),
                <any>new ArcLayer({
                    id: 'arcs',
                    data: AIR_PORTS,
                    dataTransform: d => d.features.filter(f => f.properties.scalerank < 4),
                    // Styles
                    getSourcePosition: f => [-0.4531566, 51.4709959], // London
                    getTargetPosition: f => f.geometry.coordinates,
                    getSourceColor: [0, 128, 200],
                    getTargetColor: [200, 0, 80],
                    getWidth: 1
                })
            ],

        });


        console.log(map)
        // const deck = new Deck({
        //     canvas: 'deck-canvas',
        //     width: '100%',
        //     height: '100%',
        //     initialViewState: INITIAL_VIEW_STATE,
        //     controller: true,
        //     onViewStateChange: ({viewState}) => {
        //         map.jumpTo({
        //             center: [viewState.longitude, viewState.latitude],
        //             zoom: viewState.zoom,
        //             bearing: viewState.bearing,
        //             pitch: viewState.pitch
        //         });
        //     },
        //     layers: [
        //         new GeoJsonLayer({
        //             data: US_MAP_GEOJSON,
        //             stroked: true,
        //             filled: true,
        //             lineWidthMinPixels: 2,
        //             opacity: 0.4,
        //             getLineColor: [255, 100, 100],
        //             getFillColor: [200, 160, 0, 180]
        //         })
        //     ]
        // });
    }
}
