import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {NavigationComponent} from './router-components/navigation/navigation.component';
import {MainThemeComponent} from './router-components/main-theme/main-theme.component';
import {NavigationViewComponent} from './router-components/navigation-view-component/navigation-view.component';
import { LoginGuard } from './login-guard';

const routes: Routes = [
  {
    path: 'login',
    loadChildren: './modules/login/login.module#LoginModule'
  },
  {
    path: 'main',
    component: MainThemeComponent,
    children: [
      {
        path: '',
        component: NavigationViewComponent,
        children: [
          {
            path: 'my-view',
            loadChildren: './modules/my-view/my-view.module#MyViewModule',
            component: MainThemeComponent
          },
          {
            path: 'team-view',
            loadChildren: './modules/team-view/team-view.module#TeamViewModule',
            component: MainThemeComponent
          },
        ]
      }
    ]
  },
  {
    path: 'dashboard',
    loadChildren: './modules/dashboard/dashboard.module#DashboardModule',
    component: NavigationComponent
  },
  {
    path: 'grid',
    loadChildren: './modules/grid/grid.module#GridModule',
    component: NavigationComponent,
    canActivate: [LoginGuard]
  },
  {
    path: 'grid2',
    loadChildren: './modules/grid/grid.module#GridModule',
    component: NavigationComponent,
    canActivate: [LoginGuard]
  },
  {
    path: 'unity-view',
    loadChildren: './modules/unity-view/unity-view.module#UnityViewModule',
  },
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes,
    {enableTracing: false}
    )],
  exports: [RouterModule]
})
export class AppRoutingModule { }
