import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { PlansListComponent } from './plans/plans-list/plans-list.component';
import { canActivateByJwt } from './utils/can-activate-by-jwt';
import { BaseComponent } from './layout/base/base.component';
import { ExercisesListComponent } from './exercises/exercises-list/exercises-list.component';
import { PlanPreviewComponent } from './plans/plan-preview/plan-preview.component';

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
          }
        ]
      }
    ],
    canActivate: [canActivateByJwt]
  },
  { path: '**', redirectTo: 'auth/login' }
];
