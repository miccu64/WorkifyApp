import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { PlansListComponent } from './plans/plans-list/plans-list.component';
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
    path: 'plans',
    children: [
      {
        path: 'list',
        component: PlansListComponent
      }
    ],
    canActivate: [canActivateByJwt]
  },
  { path: '**', redirectTo: 'auth/login' }
];
