import { Component, OnInit } from '@angular/core';
import {Map, View} from 'ol';
import TileLayer from 'ol/layer/Tile';
import XYZSource from 'ol/source/XYZ';
import {fromLonLat} from 'ol/proj';




@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css']
})
export class MapComponent implements OnInit {
  
  //map = Map;
  constructor() { }


  ngOnInit() {

    const card =  new Map({
      target: 'map',
      layers: [
        new TileLayer({
          source: new XYZSource({
            url: 'http://tile.stamen.com/terrain/{z}/{x}/{y}.jpg'
          })
        })
      ],
      view: new View({
        center: fromLonLat([ 5.866667, 51.833333 ]),
        zoom: 12
      
      })
    });
  }
}
