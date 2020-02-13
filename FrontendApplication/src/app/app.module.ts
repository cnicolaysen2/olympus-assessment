import {BrowserModule} from '@angular/platform-browser';
import {CUSTOM_ELEMENTS_SCHEMA, NgModule, NO_ERRORS_SCHEMA} from '@angular/core';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {en_US, NZ_I18N} from 'ng-zorro-antd';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {registerLocaleData} from '@angular/common';
import en from '@angular/common/locales/en';
import {SharedModule} from './shared/shared.module';
import {GlobalPubSubService} from './shared/global-pub-sub.service';
import {NavigationService} from "./router-components/navigation/navigation.service";
import {NavigationComponent} from "./router-components/navigation/navigation.component";
import {MainThemeComponent} from "./router-components/main-theme/main-theme.component";
import {NavigationViewComponent} from "./router-components/navigation-view-component/navigation-view.component";
import {ChartService} from "./services/chart.service";
import {GridsterDragndropService} from "./services/gridster-dragndrop.service";
import { ZoomElementDirective } from './directives/zoom-element.directive';

registerLocaleData(en);

const ROUTER_COMPONENT = [
  MainThemeComponent,
  NavigationComponent,
  NavigationViewComponent
];

@NgModule({
  declarations: [
    AppComponent,
    ...ROUTER_COMPONENT
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    SharedModule,
  ],
  providers: [
    GlobalPubSubService,
    NavigationService,
    ChartService,
    GridsterDragndropService,
    { provide: NZ_I18N, useValue: en_US }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
