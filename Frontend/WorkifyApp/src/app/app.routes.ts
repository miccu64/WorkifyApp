import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { ExercisePreviewComponent } from './exercises/exercise-preview/exercise-preview.component';
import { StatsResolver } from './exercises/exercise-preview/stats.resolver';
import { ExercisesListComponent } from './exercises/exercises-list/exercises-list.component';
import { BaseComponent } from './layout/base/base.component';
import { PlanPreviewComponent } from './plans/plan-preview/plan-preview.component';
import { PlansListComponent } from './plans/plans-list/plans-list.component';
import { SettingsComponent } from './settings/settings.component';
import { canActivateByJwt } from './utils/can-activate-by-jwt';

export const routes: Routes = [
  {
    path: 'auth',
    children: [
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent }
    ]
  },
  {
    path: 'app',
    component: BaseComponent,
    children: [
      {
        path: 'plans',
        children: [
          {
            path: 'list',
            component: PlansListComponent
          },
          { path: 'preview/:planId', component: PlanPreviewComponent }
        ]
      },
      {
        path: 'exercises',
        children: [
          {
            path: 'list',
            component: ExercisesListComponent
          },
          { path: 'preview/:exerciseId', component: ExercisePreviewComponent, resolve: { stats: StatsResolver } }
        ]
      },
      {
        path: 'settings',
        component: SettingsComponent
      }
    ],
    canActivate: [canActivateByJwt]
  },
  { path: '**', redirectTo: 'auth/login' }
];
