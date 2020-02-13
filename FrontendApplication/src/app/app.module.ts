import {BrowserModule} from '@angular/platform-browser';
import {CUSTOM_ELEMENTS_SCHEMA, NgModule, NO_ERRORS_SCHEMA} from '@angular/core';
import {
  MatBottomSheetModule,
  MatButtonModule,
  MatButtonToggleModule,
  MatCardModule,
  MatCheckboxModule,
  MatChipsModule,
  MatDatepickerModule,
  MatDialogModule,
  MatDividerModule,
  MatExpansionModule,
  MatGridListModule,
  MatIconModule,
  MatInputModule,
  MatListModule,
  MatMenuModule,
  MatNativeDateModule,
  MatPaginatorModule,
  MatProgressBarModule,
  MatProgressSpinnerModule,
  MatRadioModule,
  MatRippleModule,
  MatSelectModule,
  MatSidenavModule,
  MatSliderModule,
  MatSlideToggleModule,
  MatSortModule,
  MatStepperModule,
  MatTableModule,
  MatTabsModule,
  MatToolbarModule,
  MatTooltipModule,
  MatTreeModule,
  MatFormFieldModule,
  MatOptionModule,
  MatSnackBarModule,
} from '@angular/material';

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
import { LoginGuard } from './login-guard';
import { StoreModule } from '@ngrx/store';
import { reducers } from './ngrx/reducers';
import { EffectsModule } from '@ngrx/effects';
import { OlympusStoreEffects } from './ngrx/effects';
import { OlympusStoreService } from './services/firestore.service';
import { AngularFireModule } from '@angular/fire';
import { AngularFirestoreModule } from '@angular/fire/firestore';
import { AngularFireAuthModule } from '@angular/fire/auth';
import { environment } from '../environments/environment';

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
    MatSnackBarModule,
    MatDialogModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    SharedModule,
    StoreModule.forRoot(reducers),
    EffectsModule.forRoot([OlympusStoreEffects]),
    AngularFireModule.initializeApp(environment.firebase),
    AngularFirestoreModule, // imports firebase/firestore, only needed for database features
    AngularFireAuthModule, // imports firebase/auth, only needed for auth features,
   ],
  providers: [
    LoginGuard,
    OlympusStoreService,
    GlobalPubSubService,
    NavigationService,
    ChartService,
    GridsterDragndropService,
    { provide: NZ_I18N, useValue: en_US }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
