import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { PlansListComponent } from './plans/plans-list/plans-list.component';
import { canActivateByJwt } from './utils/can-activate-by-jwt';
import { PlansResolver } from './resolvers/plans.resolver';
import { BaseComponent } from './layout/base/base.component';

export const routes: Routes = [
  {
    path: 'auth',
    children: [
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent }
    ]
  },
  {
    path: '',
    component: BaseComponent,
    children: [
      {
        path: 'plans',
        children: [
          {
            path: 'list',
            component: PlansListComponent,
            resolve: { plans: PlansResolver }
          }
        ],
        canActivate: [canActivateByJwt]
      }
    ]
  },
  { path: '**', redirectTo: 'auth/login' }
];
