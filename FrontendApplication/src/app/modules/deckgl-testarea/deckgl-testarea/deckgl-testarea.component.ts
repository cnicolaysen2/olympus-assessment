import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';

import * as mapboxgl from 'mapbox-gl';
import {HexagonLayer, ScatterplotLayer} from '@deck.gl/layers';
import {MapboxLayer} from '@deck.gl/mapbox';


@Component({
    selector: 'app-deckgl-testarea',
    templateUrl: './deckgl-testarea.component.html',
    styleUrls: ['./deckgl-testarea.component.scss']
})
export class DeckglTestareaComponent implements OnInit {

    constructor() { }

    @ViewChild("map") map: ElementRef;

    ngOnInit() {
        const INITIAL_VIEW_STATE = {
            longitude: -122.42177834,
            latitude: 37.78346622,
            zoom: 15.5,
            bearing: -17.6,
            pitch: 45
        };

        const mapContainer = this.map.nativeElement;
        // Any type because it's bug from types or set globally
        (<any>mapboxgl).accessToken = 'pk.eyJ1Ijoid2FobGJlcmdydSIsImEiOiJjanU0MTk0bWowcWlmNDRwNGlwbDhjNmVyIn0.dl0m5I8jdwJ5b0oSdwOTtw';
        console.log(mapContainer);
// // Set your mapbox token here
        const map = new mapboxgl.Map({
            container: mapContainer,
            style: 'mapbox://styles/mapbox/light-v9',
            // Note: deck.gl will be in charge of interaction and event handling
            interactive: false,
            center: [INITIAL_VIEW_STATE.longitude, INITIAL_VIEW_STATE.latitude],
            zoom: INITIAL_VIEW_STATE.zoom,
            bearing: INITIAL_VIEW_STATE.bearing,
            pitch: INITIAL_VIEW_STATE.pitch
        });


        // The 'building' layer in the mapbox-streets vector source contains building-height
        // data from OpenStreetMap.
        map.on('load', () => {
            // Insert the layer beneath any symbol layer.
            const layers = map.getStyle().layers;

            let labelLayerId;
            for (let i = 0; i < layers.length; i++) {
                if (layers[i].type === 'symbol' && layers[i].layout['text-field']) {
                    labelLayerId = layers[i].id;
                    break;
                }
            }

            map.addLayer({
                'id': '3d-buildings',
                'source': 'composite',
                'source-layer': 'building',
                'filter': ['==', 'extrude', 'true'],
                'type': 'fill-extrusion',
                'minzoom': 15,
                'paint': {
                    'fill-extrusion-color': '#aaa',
                    // use an 'interpolate' expression to add a smooth transition effect to the
                    // buildings as the user zooms in
                    'fill-extrusion-height': [
                        "interpolate", ["linear"], ["zoom"],
                        15, 0,
                        15.05, ["get", "height"]
                    ],
                    'fill-extrusion-base': [
                        "interpolate", ["linear"], ["zoom"],
                        15, 0,
                        15.05, ["get", "min_height"]
                    ],
                    'fill-extrusion-opacity': .6
                }
            }, labelLayerId);


            const myScatterplotLayer = new MapboxLayer({
                id: 'my-scatterplot',
                type: ScatterplotLayer,
                data: [
                    {position: [INITIAL_VIEW_STATE.longitude, INITIAL_VIEW_STATE.latitude], size: 100}
                ],
                getPosition: d => d.position,
                getRadius: d => d.size,
                getColor: [255, 0, 0]
            });

            map.addLayer(myScatterplotLayer);

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
