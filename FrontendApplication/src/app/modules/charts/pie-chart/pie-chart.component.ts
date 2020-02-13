import {Component, OnInit, ViewChild, ElementRef, Input} from '@angular/core';
import * as D3 from "d3";
//import {GlobalPubSubService} from '../../../shared/global-pub-sub.service';
//import {string} from "prop-types";


@Component({
  selector: 'app-pie-chart',
  templateUrl: 'pie-chart.component.html',
  styleUrls: ['pie-chart.component.scss']
})
export class PieChartComponent implements OnInit {
  @ViewChild("containerPieChart") element: ElementRef;
  @Input() public event: number;
  @Input() public chartlabel;

  public data;

  //constructor(private globalPubSubService: GlobalPubSubService) { }

  drawPie(data, text, width, height, radius, element) {
    const arc = D3.arc()
        .innerRadius(radius - 15)
        .outerRadius(radius - 25)
        .cornerRadius(20);
    //30,10,20
    const color = D3.scaleOrdinal(["none","#0E77B1"]);

    var pie = D3.pie()
        .padAngle(.02);

    let svgContainer = D3.select(element)
        .append("svg")
        .attr("width", width)
        .attr("height", height)
        .append("g")
        .attr("transform", "translate(" + width / 2 + "," + height / 2 + ")");

    //Draw the Circle
    svgContainer.append("circle")
        .style("fill", "none")
        .style("stroke", "#3C424E")
        .style("stroke-width", 2)
        .attr("r", radius - 20);

    svgContainer.append("text")
        .text((d) => { return `${text}%`; })
        .attr("x", -15)
        .attr("y", 5)
        .attr("font-family", "sans-serif")
        .attr("font-size", "20px")
        .attr("fill", "white");

    svgContainer.selectAll("path")
        .data(pie(data))
        .enter().append("path")
        .style("fill-opacity", 1)
        .style("fill", function(d, i) { return color(<any>i); })
        .attr("d", <any>arc);
  }

  ngOnInit() {
    /*this.globalPubSubService.subscribe('LineChartEvent', (event) => {
      this.data = [100 - event, event];
      this.drawPie(this.data, event, 200, 200, 100, this.element.nativeElement);
    });
    */

    this.data = [100 - this.event, this.event];
    this.drawPie(this.data, this.event, 200, 200, 100, this.element.nativeElement);
  }
}
