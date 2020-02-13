import { Component, OnInit, Input, Output, ViewChild, ElementRef, EventEmitter } from '@angular/core';
import * as D3 from "d3";
import { ChartService } from '../../../services/chart.service';

interface DataType{
  x: number,
  y: number
}

@Component({
  selector: 'app-line-chart',
  templateUrl: './line-chart.component.html',
  styleUrls: ['./line-chart.component.scss']
})
export class LineChartComponent implements OnInit {

  constructor(private chartService: ChartService) {
  }

  @ViewChild("containerLineChart") element: ElementRef;

  @Input() public chartlabel;
  @Input() public chartLabelColor = '#000';
  @Input()
  set dataset(value: DataType[]) {
    this._dataset = value;
  }

  @Output() state = new EventEmitter<any>();

  private _dataset: DataType[];
  public svg;
  public xScale;
  public yScale;
  public height;
  public width;
  private _disableChart = true;
  private _selectedArea;

  get disableChart(): boolean {
    return this._disableChart;
  }

  set disableChart(value: boolean) {
    this._disableChart = value;
    this.updateState();
  }

  get selectedArea() {
    return this._selectedArea;
  }

  set selectedArea(value) {
    this._selectedArea = value;
    this.updateState();
  }

  updateState(){
    this.state.emit({
      selectedArea: this.selectedArea,
      isEnabled: this.disableChart
    })
  }

  isDisabled(id) {
    const isVisible = this.disableChart = !this.disableChart;

    this.chartService.setChartVisibility({id: id, isVisible: isVisible});
    return isVisible;
  }

  get dataset(): DataType[] {
    return this._dataset;
  }

  updateChart(value) {
    this.drawChart(value);
  }

  drawChart(value) {
    this.selectedArea = D3.extent(this._dataset, (d) => d.x);

    let initalSelection = () =>
    {
      drawArea(this.selectedArea[0], this. selectedArea[1]);
      D3.select(this.element.nativeElement).selectAll('.handle--custom').attr("transform", (d, i) => {
        return `translate(${this.xScale(this.selectedArea[i])}, ${this.yScale(D3.max(this._dataset, d => (<any>d).y))})`
      });
      handle.attr("display", null);

      brush.move(D3.select(this.element.nativeElement).select('.brush'), this.selectedArea.map(xval => this.xScale(xval)));
    };

    let brushed = () => {

      let s = D3.event.selection;

      const xVals = this._dataset.map(d => d.x);
      const closest = (numbers, goal) => numbers.reduce(function(prev, curr) {
        return (Math.abs(curr - goal) < Math.abs(prev - goal) ? curr : prev);
      });

      if (!D3.event.sourceEvent || D3.event.sourceEvent.type === "brush" || D3.event.sourceEvent.which !== 1) return;

      if (s === null) {
        D3.select(this.element.nativeElement).select(".brush").call(D3.event.target.move, this.selectedArea.map(xval => this.xScale(xval)));
        return;
      } else if ( s[0] === s[1]) {
        //TODO: draw a simple line
        return;
      }

      let snapXpoints = s.map(val => this.xScale.invert(val));
      let bisectXindex = snapXpoints.map(sx => closest(xVals, sx));
      let snapX = [bisectXindex[0], bisectXindex[1]];

      this.selectedArea = snapX;

      drawArea(snapX[0], snapX[1]);

      if (D3.event.type === 'end') {
        handle.attr("display", null);
      }

      D3.select(this.element.nativeElement).selectAll('.handle--custom').attr("transform", (d, i) => {
        return `translate(${this.xScale(snapX[i])}, ${this.yScale(D3.max(this._dataset, d => (<any>d).y))})` });

      D3.select(this.element.nativeElement).select('.brush').call(D3.event.target.move, snapX.map(xval => this.xScale(xval)));
    };

    this._dataset.sort((a, b) => {
      if (a.x < b.x)
        return -1;
      if (a.x > b.x)
        return 1;
      return 0;
    });

    D3.select(this.element.nativeElement).select('svg').remove();
    const margin = {
      top: 50,
      right: 50,
      bottom: 50,
      left: 50
    };
    this.width = 400;
    this.height = 200;

// 5. X scale will use the index of our data
    this.xScale = D3.scaleLinear()
        .domain(D3.extent(this._dataset, (d) => d.x ))
        .range([margin.left, this.width - margin.right]); // output

// 6. Y scale will use the randomly generate number

    const [yMin,yMax] = D3.extent(this._dataset, (d) => d.y );

    this.yScale = D3.scaleLinear()
        .domain(D3.extent(this._dataset, (d) => d.y )) // input
        .range([this.height - margin.bottom, margin.top]); // output

// 7. D3's line generator
    let line = D3.line<DataType>()
        .x((d, i) => {
          return this.xScale(d.x);
        }) // set the x values for the line generator
        .y((d) => {
          return this.yScale(d.y);
        })
        .curve(D3.curveMonotoneX); // set the y values for the line generator

// 1. Add the SVG to the page and employ #2
    this.svg = D3.select(this.element.nativeElement).append("svg")
        .attr("width", this.width)
        .attr("height", this.height)
        .attr("class","svg-cont")
        .append("g");

// 3. Call the x axis in a group tag
    let xAxis = this.svg.append("g")
        .attr("class", "x axis")
        .attr("transform", "translate(0," + (this.height-margin.bottom) + ")")
        .call(D3.axisBottom(this.xScale));

    xAxis.selectAll("text")
        .style("color", this.chartLabelColor);

    xAxis.selectAll("path")
        .style("color", this.chartLabelColor);

    xAxis.selectAll("line")
        .style("color", this.chartLabelColor);

// 4. Call the y axis in a group tag
    let yAxis = this.svg.append("g")
        .attr("class", "y axis")
        .attr("transform", `translate(${margin.left},0)`)
        .call(D3.axisLeft(this.yScale)); // Create an axis component with D3.axisLeft

    yAxis.selectAll("text")
        .style("color", this.chartLabelColor);

    yAxis.selectAll("path")
        .style("color", this.chartLabelColor);

    yAxis.selectAll("line")
        .style("color", this.chartLabelColor);

// 9. Append the path, bind the data, and call the line generator
    this.svg.append("path")
        .datum(this._dataset) // 10. Binds data to the line
        .attr("class", "line") // Assign a class for styling
        .attr("d", line); // 11. Calls the line generator

    this.svg.append("text")
        .text((d) => { return this.chartlabel; })
        .style("text-anchor", "middle")
        .attr("x", this.width/2)
        .attr("y", margin.top-10)
        .attr("font-family", "sans-serif")
        .attr("font-size", "20px")
        .attr("fill", this.chartLabelColor);

    const brush = D3.brushX()
        .extent( [[margin.left, margin.top], [this.width- margin.right, this.height- margin.bottom]])
        .on("start brush end", brushed);

    const brushG =  this.svg.append("g")
        .attr("class", "brush")
        .call(brush);

    brushG.selectAll("rect")
        .attr("y", -6)
        .attr("height", this.height);

    brushG.select('.selection')
        .attr('fill', 'none')
        .attr('stroke','none');

    brushG.selectAll('.handle--w, .handle--e')
        .attr('fill', '#666');

    const handle = brushG.selectAll(".handle--custom")
        .data([{type: "w"}, {type: "e"}])
        .enter().append("circle")
        .attr("class", "handle--custom")
        .attr("fill", "#666")
        .attr("stroke", "#000")
        .attr("cursor", "ew-resize")
        .attr('display', 'none')
        .style("pointer-events", 'none')
        .attr("r", "10");


    let area = D3.area<DataType>()
        .x((d, i) => {
          return this.xScale(d.x);
        })
        .y0(this.yScale((D3.min(this._dataset, d=> d.y))))
        .y1((d) => {
          return this.yScale(d.y);
        })
        .defined((d) => {
          return <any>d.y;
        })
        .curve(D3.curveMonotoneX);

    const drawArea = (min, max) => {
      let filteredData = this._dataset.map((d,i) => {
        if (Math.abs(d.x) < Math.abs(max) || Math.abs(d.x) > Math.abs(min)) {
          return {x: d.x, y: null}
        }
        return d;
      });
      let svg = D3.select('svg.svg-cont');

      D3.selectAll('.selected-area').remove();

      svg.append("path")
          .attr('class', 'selected-area')
          .datum(filteredData)
          .attr("fill", "steelblue")
          .attr("d", area);
    }
    initalSelection();
  }

  ngOnInit() {
    this.drawChart({value: 0.2, highValue: 0.5});
  }
}
