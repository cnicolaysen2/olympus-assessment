import {AfterViewInit, ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {ScriptLoaderService} from './script-loader.service';
import {NzNotificationService} from 'ng-zorro-antd';
import {GlobalPubSubService} from '../../shared/global-pub-sub.service';

import * as d3 from 'd3';
import {NavigationService} from "../../router-components/navigation/navigation.service";
import {json} from "d3-fetch";

declare let UnityLoader;
declare let UnityProgress;


// TODO: export interface to another file
export interface Notification {
  type:  'success' | 'info' | 'warning' | 'error' | 'blank' | string,
  title: string,
  description: string,
  options?: any
}

export interface SendMessageModel {
  objectName: string,
  methodName: string,
  value: string
}

export interface DataDistribution {
  x: number;
  y: number;
}

export interface LineChartEventJson {
  LayerID: string;
  DataDistribution: DataDistribution[];
}

@Component({
  selector: 'app-unity-view',
  templateUrl: './unity-view.component.html',
  styleUrls: ['./unity-view.component.scss']
})
export class UnityViewComponent implements OnInit, AfterViewInit {

  @ViewChild('gameContainer') gameContainer: ElementRef;

  unityLoaded = false;

  constructor(
    public scriptLoaderService: ScriptLoaderService,
    private nzNotificationService: NzNotificationService,
    private globalPubSubService: GlobalPubSubService,
    public navigationService: NavigationService,
    private cdr: ChangeDetectorRef
    ) {
  }

  gameInstance: {
    SendMessage: any,
    Module: any,
  };
  functionModelValue = {
    name: '',
    value: ''
  };
  sendMessageModel: SendMessageModel = {
    objectName: '',
    methodName: '',
    value: ''
  };

  public lineChartEventJson: LineChartEventJson[] = [];


  // TODO: вынести нотифкейшины в отдельный сервис, ибо нужна прослойку между ними и юнити. Либо юнити в отдельную компоненту

  notificationHistories: Notification[] = [];


  createNotification(notification: Notification): void {
    this.nzNotificationService.create(
      notification.type,
      notification.title,
      notification.description,
      notification.options
    );

    this.notificationHistories.push(notification);
  }

  ngOnInit() {

    this.globalPubSubService.subscribe('NotificationEvent', (event) => {
      // console.info(event);
      return this.createNotification(event);
    });

    this.globalPubSubService.subscribe('PieChartEvent', (event) => {
      // console.info(event);
    });

    this.globalPubSubService.subscribe('LineChartEvent', (event) => {
      // console.info(event);
      if (event !== 'line chart data test'){
        this.lineChartEventJson.push(JSON.parse(event));
        console.log(this.lineChartEventJson);
      }



      //
      // d3.select("#chart svg").remove();
      // //CHART, DELETE IT AFTER PRESENTATION OR FULL REWORK
      //
      // let dataStr: string = '1,1;2,2;3,3;4,4;5,5;6,5;7,5';
      // let data: {x: number, y: number}[] = [];
      //
      // dataStr.split(';').forEach(row => {
      //   data.push({
      //       x: parseInt(row.split(',')[0]),
      //       y: parseInt(row.split(',')[1])
      //   })
      // });
      //
      // console.log(data);
      //
      // let margin = {
      //   top: 30,
      //   right: 20,
      //   bottom: 30,
      //   left: 50
      // };
      // let width = 600 - margin.left - margin.right;
      // let height = 270 - margin.top - margin.bottom;
      //
      //
      // let x = d3.scaleLinear().range([0, width]);
      // let y = d3.scaleLinear().range([height, 0]);
      //
      // let xAxis = d3.axisBottom(x).ticks(5);
      //
      // let yAxis = d3.axisLeft(y).ticks(5);
      //
      // let valueline = d3.line()
      //   .x((d: any) => {
      //     return x(d.x);
      //   })
      //   .y((d: any) => {
      //     return y(d.y);
      //   });
      //
      // let svg = d3.select("#chart")
      //   .append("svg")
      //   .attr("width", width + margin.left + margin.right)
      //   .attr("height", height + margin.top + margin.bottom)
      //   .append("g")
      //   .attr("transform", "translate(" + margin.left + "," + margin.top + ")");
      //
      //
      //
      // // Scale the range of the data
      // x.domain(d3.extent(data, function (d) {
      //   return d.x;
      // }));
      // y.domain([0, d3.max(data, function (d) {
      //   return d.y;
      // })]);
      //
      // svg.append("path") // Add the valueline path.
      //   .attr("d", valueline(<any>data));
      //
      // svg.append("g") // Add the X Axis
      //   .attr("class", "x axis")
      //   .attr("transform", "translate(0," + height + ")")
      //   .call(xAxis);
      //
      // svg.append("g") // Add the Y Axis
      //   .attr("class", "y axis")
      //   .call(yAxis);

    });

    this.scriptLoaderService.load('UnityProgress', 'UnityLoader').then(data => {
      let UnityProgress = (gameInstance, progress) => {
        if (progress === 1){
          this.unityLoaded = true;
          // console.log('unityLoaded');
          this.cdr.markForCheck();
          setTimeout(() => {
            this.cdr.markForCheck();
            // console.log('unityLoaded');
          }, 1000)
        }
      };

      this.gameInstance = UnityLoader.instantiate("gameContainer", "/assets/UnityBuild/Build/UnityBuild.json", {onProgress: UnityProgress});
    })
    .catch(error => console.log(error));





  }

  selectedArea(event, layer){
    let layVal = JSON.stringify({"LayerID": layer, "Min": event.selectedArea[0], "Max": event.selectedArea[1], IsEnabled: !event.isEnabled});
    console.log(event);
    this.sendMessage({
      objectName: 'JSBridge',
      methodName: 'LineChartFilter',
      value: layVal
    });
  }

  ngAfterViewInit() {
    console.log(this.gameContainer.nativeElement);


    let recaptureInputAndFocus = () => {
      let canvas = this.gameContainer.nativeElement;
      if(canvas) {
        canvas.setAttribute("tabindex", "1");
        canvas.focus();
      } else
        setTimeout(recaptureInputAndFocus, 100);
    };

    recaptureInputAndFocus();

  }

  sendMessage(sendMessageModel: SendMessageModel){
    console.log(`sendModel ${sendMessageModel}`);
    this.gameInstance.SendMessage(sendMessageModel.objectName, sendMessageModel.methodName, sendMessageModel.value);
  }

  callFunctionFromUnity(functionModelValue: {name: string, value: string}){
    // console.log(name)
    console.log(this.gameInstance)
    console.log(this.gameInstance.Module)
    console.log(this.gameInstance.Module['asmLibraryArg'])
    console.log(this.gameInstance.Module['asmLibraryArg'][`_${functionModelValue.name}`])
    if (this.gameInstance.Module['asmLibraryArg'][`_${functionModelValue.name}`]){
      this.gameInstance.Module['asmLibraryArg'][`_${functionModelValue.name}`](functionModelValue.value);
    }
  }

}
