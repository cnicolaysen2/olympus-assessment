import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {StratigraphyConvertor} from "./stratigraphy-convertor";
import * as d3 from "d3";

@Component({
  selector: 'app-stratigraphy-chart',
  templateUrl: './stratigraphy-chart.component.html',
  styleUrls: ['./stratigraphy-chart.component.scss']
})
export class StratigraphyChartComponent implements OnInit {

  @ViewChild("chart") chart: ElementRef;

  constructor() { }

  ngOnInit() {
    let data = StratigraphyConvertor.jsonToHierarchyJson();

    let root = d3.hierarchy(data)
      .count();

    let w = 3000,
    h = 1000;

    let partition = d3.partition()
      .size([w, h]);

    partition(root);

    let g = d3.select(this.chart.nativeElement)
      .selectAll('rect.node')
      .data(root.descendants())
      .enter()

    g.append('svg:rect')
      .classed('node', true)
      .attr('y', (d: any) => d.x0)
      .attr('x', (d: any) => d.y0)
      .attr('height',(d: any) => d.x1 - d.x0)
      .attr('width', (d: any) => {
        if (d.data.children) {
          return d.y1 - d.y0;
        } else {
          return 2*(d.y1 - d.y0);
        }
      });

    g.append("svg:text")
      .attr("transform", transform)
      .attr("dy", ".35em")
      .text((d) => {
        if (d.data.children) {
          return d.data.name;
        } else {
          return `Ma: ${(<any>d.data).Ma} Approx ${(<any>d.data).Approx} Uncretainty ${(<any>d.data).Uncertainty}`;
        }
      });


    function transform(d) {
      return `translate(${d.y0 + 10}, ${d.x0 + 10})`;
    }
  }

}
